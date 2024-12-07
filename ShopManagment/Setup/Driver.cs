using System.Diagnostics;
using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools;
using OpenQA.Selenium.DevTools.V131.Network;

namespace ShopManagment.Setup
{
    public class SetupDriver : LoggerSetup
    {
        public IWebDriver? _driver;
        private NetworkAdapter? _networkAdapter;
        public Logger Logger = LogManager.GetCurrentClassLogger();
        public int Timeout { get; set; }    

        public void Setup(string browserName)
        {
            ConfigureLogging();

            if (browserName.Equals("chrome"))
            {;
                _driver = new ChromeDriver();
                _driver.Manage().Window.Maximize();
                InitializeNetworkAdapter();
                //SetNetworkConditions("4g");
                VisitPageAndSetTimeout("https://kmw-retail-sk.apps.ocp01-shared.t.dc1.cz.ipa.ifortuna.cz/kmw/shop");
            }
        }
        private void InitializeNetworkAdapter()
        {
            if (_driver is IDevTools devTools)
            {
                var session = devTools.GetDevToolsSession();
                _networkAdapter = new NetworkAdapter(session);
                _networkAdapter.Enable(new EnableCommandSettings());
            }
            Logger.Info("Network adapter initialized.");
        }
        public void SetNetworkConditions(string networkType)
        {
            int downloadThroughput;
            int uploadThroughput;
            int latency;

            switch (networkType.ToLower())
            {
                case "3g":
                    downloadThroughput = 750 * 1024 / 8; // 750 Kb/s (93.75 KB/s)
                    uploadThroughput = 250 * 1024 / 8;   // 250 Kb/s (31.25 KB/s)
                    latency = 200; //  3G latency
                    break;
                case "4g":
                    downloadThroughput = 4000 * 1024 / 8; // 4000 Kb/s (500 KB/s)
                    uploadThroughput = 3000 * 1024 / 8;   // 3000 Kb/s (375 KB/s)
                    latency = 100; // 4G latency
                    break;
                case "5g":
                    downloadThroughput = 20000 * 1024 / 8; // 20000 Kb/s (2500 KB/s)
                    uploadThroughput = 10000 * 1024 / 8;   // 10000 Kb/s (1250 KB/s)
                    latency = 50; // 5G latency
                    break;
                default:
                    throw new ArgumentException("Invalid network type. Please specify '3g', '4g', or '5g'.");
            }

            _networkAdapter?.EmulateNetworkConditions(new EmulateNetworkConditionsCommandSettings()
            {
                Offline = false,
                Latency = latency,
                DownloadThroughput = downloadThroughput,
                UploadThroughput = uploadThroughput
            });
            Logger.Info($"Network conditions set to {networkType.ToUpper()} with {downloadThroughput * 8 / 1024} Kb/s download and {uploadThroughput * 8 / 1024} Kb/s upload speed.");
        }

        public void NetworkDisconnection()
        {
            _networkAdapter?.EmulateNetworkConditions(new EmulateNetworkConditionsCommandSettings()
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
            _networkAdapter?.EmulateNetworkConditions(new EmulateNetworkConditionsCommandSettings()
            {
                Offline = false,
                Latency = 0, 
                DownloadThroughput = int.MaxValue, 
                UploadThroughput = int.MaxValue
            });

            Logger.Info("Network reconnected.");
        }
        
        private void VisitPageAndSetTimeout(string url)
        {
            var downloadSpeed = MeasurePageLoadSpeed(url);
            Logger.Info($"Measured download speed: {downloadSpeed} KB/s");

            if (downloadSpeed < 256)
            {
                Timeout = 3000;
            }
            else if (downloadSpeed < 512)
            {
                Timeout = 2000;
            }
            else
            {
                Timeout = 1000;
            }
            Logger.Info($"Timeout set to {Timeout} ms");
        }

        private double MeasurePageLoadSpeed(string url)
        {
            var stopwatch = new Stopwatch();
            double totalBytes = 0;

            EventHandler<LoadingFinishedEventArgs> loadingFinishedHandler = (sender, e) =>
            {
                totalBytes += e.EncodedDataLength;
            };

            if (_networkAdapter != null)
            {
                _networkAdapter.LoadingFinished += loadingFinishedHandler;

                stopwatch.Start();
                _driver?.Navigate().GoToUrl(url);
                stopwatch.Stop();

                _networkAdapter.LoadingFinished -= loadingFinishedHandler;
            }

            var loadTime = stopwatch.Elapsed.TotalSeconds;
            var speed = totalBytes / loadTime / 1024;
            var downloadSpeed = (int)Math.Round(speed, 0);
            Logger.Info(downloadSpeed);

            return downloadSpeed;
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