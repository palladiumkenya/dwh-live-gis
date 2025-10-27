using LiveDWAPI.Application.Immunization;
using LiveDWAPI.Infrastructure;
using LiveDWAPI.Infrastructure.Immunization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace LiveDWAPI.Application.Tests;

[SetUpFixture]
public class TestInitializer
{ 
    public static IServiceProvider ServiceProvider;
    [OneTimeSetUp]
    public void Init()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .CreateLogger();
        SetupDependencyInjection();
    }
      
    private void SetupDependencyInjection()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Test.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.Test.Development.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
        var services = new ServiceCollection();
        services.AddApplication(config);
        services.AddInfrastructure(config);
        ServiceProvider = services.BuildServiceProvider();
        InitDB();
    }
    
    private void InitDB()
    {
        var ct = ServiceProvider.GetRequiredService<ImmunizationContext>();
        ct.Initialize();
    }
}