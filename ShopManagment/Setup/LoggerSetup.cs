using NLog;
using NLog.Config;
using NLog.Targets;

namespace ShopManagment.Setup
{
    public class LoggerSetup
    {
        public static void ConfigureLogging()
        {
            var config = new LoggingConfiguration();

            var fileTarget = new FileTarget("logfile")
            {
                //FileName = @"C:\Users\borjos\source\repos\ShopManagment\ShopManagment\TestResults\logfile_${date:format=dd-MM-yyyy}.txt", // Windows
                FileName = "/Users/joske_mac/RiderProjects/ShopManagment/ShopManagment/TestResults/logfile_${date:format=dd-MM-yyyy}.txt", // Mac
                Layout = "${longdate} | ${level:uppercase=true} | ${message} | ${exception}"
                         
            };

            config.AddRule(LogLevel.Info, LogLevel.Fatal, fileTarget);

            LogManager.Configuration = config;
        }
    }
}