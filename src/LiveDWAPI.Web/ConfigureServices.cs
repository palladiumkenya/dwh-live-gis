using CSharpFunctionalExtensions;
using LiveDWAPI.Application;
using LiveDWAPI.Infrastructure;
using LiveDWAPI.Infrastructure.Immunization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Serilog;

namespace LiveDWAPI.Web;

public static class ConfigureServices
{
    private static string _corsPolicy = "LiveDWAPICorsPolicy";

    public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
    {
        var devMode = builder.Environment.IsDevelopment();
        var environment = builder.Environment.EnvironmentName;

        var config = builder.Configuration
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
            .AddJsonFile("serilog.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"serilog.{environment}.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"hosting.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"hosting.{environment}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

       
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(config)
            .CreateLogger();

        builder.Host.UseSerilog((hostContext, loggerConfig) =>
        {
            loggerConfig
                .ReadFrom.Configuration(hostContext.Configuration)
                .Enrich.WithProperty("ApplicationName", hostContext.HostingEnvironment.ApplicationName);
        });
        builder.Services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders =
                ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        });
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(_corsPolicy,
                policy =>
                {
                    string[]? allowedOrigins = config.GetSection("AllowedCorsOrigins").Get<string[]>();
                    if (allowedOrigins != null && allowedOrigins.Any())
                    {
                        policy.WithOrigins(allowedOrigins)
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    }
                });
        });
        Log.Information("LiveDWAPI initializing [Services registrations] ...");

        // defaults
        builder.Services.AddControllers().AddNewtonsoftJson(options =>
        {
            // Handle reference loops
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        });
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "NDW Live", Version = "v1", Description = "API for realtime NDW data"});
        });

        // services
        builder.Services.AddApplication(config);
        builder.Services.AddInfrastructure(config, devMode);

        Log.Information("LiveDWAPI initializing [Services registrations OK]");
        return builder;
    }

  
    public static WebApplication SetupMiddleware(this WebApplication app)
    {
        app.UseSerilogRequestLogging();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        
        app.UseSwagger();
        app.UseSwaggerUI();

        if (!app.Environment.IsDevelopment())
        {
            app.UseHsts();
        }

        app.UseCors(_corsPolicy);
        app.UseForwardedHeaders();
        // app.UseHttpsRedirection();
        // app.UseStaticFiles();
        //app.UseAuthentication();
        app.UseRouting();
        //app.UseAuthorization();
        app.MapControllers();
        return app;
    }
    
    public static Result SetupDatabases(IHost host)
    {
        Log.Information("Initializing [Checking Databases ...]");

        try
        {
            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;
                var csContext = services.GetRequiredService<ImmunizationContext>();
                csContext.Database.Migrate();
            }

            Log.Information("Initializing [Checking Databases OK]");
            return Result.Success();
        }
        catch (Exception e)
        {
            Log.Fatal(e, $"SetupDatabase Failure");
            return Result.Failure(e.Message);
        }
    }
}