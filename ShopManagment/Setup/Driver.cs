using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools;

namespace ShopManagment.Setup
{
    public class SetupDriver : LoggerSetup
    {
        public IWebDriver? _driver;
        private OpenQA.Selenium.DevTools.V131.Network.NetworkAdapter? _networkAdapter;
        public Logger Logger = LogManager.GetCurrentClassLogger();

        public void Setup(string browserName)
        {
            ConfigureLogging();

            if (browserName.Equals("chrome"))
            {
                _driver = new ChromeDriver();
                _driver.Manage().Window.Maximize();
                InitializeNetworkAdapter();
                LoggerSetup.ConfigureLogging();    
            }
        }

        private void InitializeNetworkAdapter()
        {
            if (_driver is IDevTools devTools)
            {
                var session = devTools.GetDevToolsSession();
                _networkAdapter = new OpenQA.Selenium.DevTools.V131.Network.NetworkAdapter(session);
                _networkAdapter.Enable(new OpenQA.Selenium.DevTools.V131.Network.EnableCommandSettings());
            }
            Logger.Info("Network adapter initialized.");
        }

        public void SlowNetworkConditions()
        {
            _networkAdapter?.EmulateNetworkConditions(new OpenQA.Selenium.DevTools.V131.Network.EmulateNetworkConditionsCommandSettings()
            {          
                Offline = false,
                Latency = 200,
                DownloadThroughput = 500 * 1024 / 8,
                UploadThroughput = 500 * 1024 / 8
            });
            Logger.Info("Network conditions set to slow.");
        }

        public void NetworkDisconnection()
        {
            _networkAdapter?.EmulateNetworkConditions(new OpenQA.Selenium.DevTools.V131.Network.EmulateNetworkConditionsCommandSettings()
            {
                Offline = true, 
                Latency = 0,
                DownloadThroughput = 0,
                UploadThroughput = 0
            });

            Logger.Info("Network disconnected.");         
        }

        private void ReconnectNetwork()
        {
            _networkAdapter?.EmulateNetworkConditions(new OpenQA.Selenium.DevTools.V131.Network.EmulateNetworkConditionsCommandSettings()
            {
                Offline = false,
                Latency = 0, 
                DownloadThroughput = int.MaxValue, 
                UploadThroughput = int.MaxValue
            });

            Logger.Info("Network reconnected.");
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                _driver?.Quit();
                _driver?.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}