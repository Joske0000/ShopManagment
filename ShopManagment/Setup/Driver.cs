using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using Serilog;
using NUnit.Framework;
using NLog;

namespace ShopManagment.Setup;

public class SetupDriver : LoggerSetup
{
    public IWebDriver _driver;

    public Logger Logger = LogManager.GetCurrentClassLogger();

    public void Setup(string browserName)
    {
        if (browserName.Equals("chrome"))
        {

            _driver = new ChromeDriver();     
            _driver.Manage().Window.Maximize();
            LoggerSetup.ConfigureLogging();

        }
        else if (browserName.Equals("firefox"))
        {
            _driver = new FirefoxDriver();
            _driver.Manage().Window.Maximize();
           
        }
        else
        {
            throw new Exception("Unsupported browser");
        }
        

    }

    [TearDown]
    public void TearDown()
    {
        try
        {
            _driver.Quit();
            _driver.Dispose();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}

