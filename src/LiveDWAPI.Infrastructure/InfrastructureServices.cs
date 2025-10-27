using LiveDWAPI.Application.Immunization;
using LiveDWAPI.Domain.Common;
using LiveDWAPI.Infrastructure.Immunization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LiveDWAPI.Infrastructure;

public static class InfrastructureServices
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration, bool devMode = false, string overrideConnection = "")
    {
        var migrationsAssembly = typeof(ImmunizationContext).Assembly.FullName;
        var connectionString = string.IsNullOrWhiteSpace(overrideConnection)
            ? configuration.GetConnectionString("LiveConnection")
            : overrideConnection;

        services.AddDbContext<ImmunizationContext>(x => x
            .EnableSensitiveDataLogging(devMode)
            .UseNpgsql(connectionString,
                builder => builder.MigrationsAssembly(migrationsAssembly))
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            .UseSnakeCaseNamingConvention()
        );
        
        services.Configure<ServicesApiOptions>(configuration.GetSection(ServicesApiOptions.ServicesApi));

        services.AddScoped<IImmunizationContext>(provider => provider.GetRequiredService<ImmunizationContext>());
        return services;
    }
}