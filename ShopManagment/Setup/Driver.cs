using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace ShopManagment.Setup
{
    public class SetupDriver : LoggerSetup
    {
        public IWebDriver? _driver;
        private OpenQA.Selenium.DevTools.V131.Network.NetworkAdapter? _networkAdapter;
        public Logger Logger = LogManager.GetCurrentClassLogger();
        public int Timeout { get; set; }    
        public bool _isNetworkConditionSet = false; 

        public void Setup(string browserName)
        {
            ConfigureLogging();

            if (browserName.Equals("chrome"))
            {;
                _driver = new ChromeDriver();
                _driver.Manage().Window.Maximize();
                InitializeNetworkAdapter();
                //SlowNetworkConditions();
                LoggerSetup.ConfigureLogging();           
                if (_isNetworkConditionSet == false)
                {
                    Timeout = 1000;
                }
                else
                {
                    Timeout = 3000;
                }
            }
        }
        public IWebElement WaitForTableToLoad(IWebDriver driver, string tableCssSelector, TimeSpan timeout)
        {
            WebDriverWait wait = new WebDriverWait(driver, timeout);

            IWebElement table = wait.Until(ExpectedConditions.ElementExists(By.ClassName(tableCssSelector)));

            int previousRowCount = 0;
            int currentRowCount = 0;

            do
            {
                previousRowCount = currentRowCount;

                Thread.Sleep(500);

                currentRowCount = table.FindElements(By.TagName("tr")).Count;

            } while (currentRowCount > previousRowCount);

            return table;
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
                DownloadThroughput = 256 * 1024 / 8,
                UploadThroughput = 256 * 1024 / 8
            });
            _isNetworkConditionSet = true;
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
        public void Cleanup()
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