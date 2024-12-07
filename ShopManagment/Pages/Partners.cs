using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using ShopManagment.Setup;

namespace ShopManagment.Pages
{
    internal class Partners
    {

        private readonly IWebDriver _driver;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly WebDriverWait _wait;
        public string tableselector = "overflow-auto";

        public Partners(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            _driver.Navigate().GoToUrl("https://kmw-retail-sk.apps.ocp01-shared.t.dc1.cz.ipa.ifortuna.cz/kmw/partner");
        }
       
        public void AddPartner(string PartnerName, int Timeout)
        {
            Logger.Info("Adding Partner");
            _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("filters-partner-add"))).Click();       
            _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"partner-modal-form-name\"]/div/div/div/div/input"))).SendKeys(PartnerName);
            _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"partner-modal-buttons-save\"]"))).Click();      
            Logger.Info($"Partner Added: {PartnerName}");
            WaitForTableToLoad(_driver, tableselector , TimeSpan.FromMilliseconds(Timeout));
        }

        public void SearchPartner(string PartnerName, int Timeout)
        {
            Logger.Info("Searching for Partner");
            _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div#filters-partner-name input"))).SendKeys(PartnerName);
            WaitForTableToLoad(_driver, tableselector, TimeSpan.FromMilliseconds(Timeout));         
            IWebElement table = _driver.FindElement(By.ClassName("overflow-auto"));
            IList<IWebElement> tableRow = table.FindElements(By.TagName("tr"));

            bool patnerFound = false;

            foreach (var row in tableRow)
            {
                if (row.Text.Contains(PartnerName))
                {
                    Logger.Info($"Partner: {PartnerName} FOUND");
                    patnerFound = true;
                    _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div#filters-partner-name input"))).Clear();
                    break;
                }
            }
            if (!patnerFound)
            {
                Logger.Warn($"Partner: {PartnerName} NOT FOUND");
            }
        }
        public void PartnerDetails(string PartnerName, int Timeout)
        {
            Logger.Info("Getting Partner Details");
            _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div#filters-partner-name input"))).SendKeys(PartnerName);
            
            WaitForTableToLoad(_driver, tableselector, TimeSpan.FromMilliseconds(Timeout));

            IWebElement table = _driver.FindElement(By.ClassName("overflow-auto"));
            IList<IWebElement> tableRow = table.FindElements(By.TagName("tr"));
            foreach (var row in tableRow)
            {
                string rowid = row.GetDomAttribute("Id");
                if (!string.IsNullOrEmpty(rowid))
                {
                    IWebElement specificRow = table.FindElement(By.Id(rowid));
                    IList<IWebElement> tableDataCells = specificRow.FindElements(By.TagName("td"));
                    if (tableDataCells.Count > 0)
                    {
                        string Name = tableDataCells[3].Text;
                        string ID = tableDataCells[1].Text;
                        string Date = tableDataCells[4].Text;
                        string DisabledPartner = tableDataCells[5].Text;
                        if (DisabledPartner == "-")
                        {
                            Logger.Info($"Partner: {Name} with ID: {ID} created on: {Date} is enabled");
                            _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div#filters-partner-name input"))).Clear();
                        }
                        else
                        {
                            Logger.Info($"Partner: {Name} with ID: {ID} was created on: {Date} is DISABLED");
                            _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div#filters-partner-name input"))).Clear();
                        }
                    }
                }
            }
        }
        static IWebElement WaitForTableToLoad(IWebDriver driver, string tableCssSelector, TimeSpan timeout)
        {
            WebDriverWait wait = new WebDriverWait(driver, timeout);

            IWebElement table = wait.Until(ExpectedConditions.ElementExists(By.ClassName(tableCssSelector)));

            int previousRowCount = 0;
            int currentRowCount = 0;

            do
            {
                previousRowCount = currentRowCount;

                Thread.Sleep(timeout);

                currentRowCount = table.FindElements(By.TagName("tr")).Count;

            } while (currentRowCount > previousRowCount);

            return table;
        }

    }
}
