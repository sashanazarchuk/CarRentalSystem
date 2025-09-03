using NLog;
using NLog.Web;

namespace CarRentalSystem.API.Extensions
{
    public static class LoggerExtensions
    {
        public static WebApplicationBuilder AddLoggingServices(this WebApplicationBuilder services)
        {
            LogManager.Setup().LoadConfigurationFromAppSettings();
            var logger = LogManager.GetCurrentClassLogger();
            logger.Debug("NLog initialized");

            services.Logging.ClearProviders();
            services.Host.UseNLog();

            return services;
        }

        public static void RunWithNLog(this WebApplication app)
        {
            var logger = LogManager.GetCurrentClassLogger();

            try
            {
                app.Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Application stopped due to exception");
                throw;
            }
            finally
            {
                LogManager.Shutdown();
            }
        }
    }
}