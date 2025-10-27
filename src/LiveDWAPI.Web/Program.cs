
using Serilog;

namespace LiveDWAPI.Web
{
    public class Program
    {
        public static  void Main(string[] args)
        {
            try
            {
                var app = WebApplication.CreateBuilder(args)
                    .RegisterServices()
                    .Build()
                    .SetupMiddleware();
                Log.Information("LiveDWAPI Starting...");
                
                var initResponse =  ConfigureServices.SetupDatabases(app);
                app.Run();
            }
            catch (Exception e)
            {
                Log.Fatal(e, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
