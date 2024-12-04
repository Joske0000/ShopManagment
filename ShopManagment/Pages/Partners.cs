using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ShopManagment.Pages
{
    internal class Partners
    {

        private readonly IWebDriver _driver;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly WebDriverWait _wait;

        public Partners(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }
        IWebElement AddPartnerButton => _driver.FindElement(By.Id("filters-partner-add"));
        IWebElement AddPartnerName => _driver.FindElement(By.XPath("//*[@id=\"partner-modal-form-name\"]/div/div/div/div/input"));

        public void AddPartner(string PartnerName, int Timeout)
        {
            _driver.Navigate().GoToUrl("https://kmw-retail-sk.apps.ocp01-shared.t.dc1.cz.ipa.ifortuna.cz/kmw/partner");
            Logger.Info("Partner tab opened");
            AddPartnerButton.Click();
            Thread.Sleep(Timeout);
            AddPartnerName.SendKeys(PartnerName);
            Thread.Sleep(Timeout);
            _driver.FindElement(By.XPath("//*[@id=\"partner-modal-buttons-save\"]")).Click();
            Thread.Sleep(Timeout);
        }

        public void SearchPartner(string PartnerName, int Timeout)
        {
            _driver.Navigate().GoToUrl("https://kmw-retail-sk.apps.ocp01-shared.t.dc1.cz.ipa.ifortuna.cz/kmw/partner");
            Logger.Info("Partner tab opened");
            Thread.Sleep(Timeout);
            IWebElement search = _driver.FindElement(By.CssSelector("div#filters-partner-name input"));
            Thread.Sleep(Timeout);
            search.SendKeys(PartnerName);
            Thread.Sleep(Timeout);

            IWebElement table = _driver.FindElement(By.ClassName("overflow-auto"));
            IList<IWebElement> tableRow = table.FindElements(By.TagName("tr"));

            bool shopFound = false;

            foreach (var row in tableRow)
            {
                if (row.Text.Contains(PartnerName))
                {
                    Logger.Info($"Partner: {PartnerName} FOUND");
                    shopFound = true;
                    break;
                }
            }
            if (!shopFound)
            {
                Logger.Warn($"Partner: {PartnerName} NOT FOUND");
            }
        }
        public void PartnerDetails(string PartnerName, int Timeout)
        {
            _driver.Navigate().GoToUrl("https://kmw-retail-sk.apps.ocp01-shared.t.dc1.cz.ipa.ifortuna.cz/kmw/partner");
            Logger.Info("Partner tab opened");
            Thread.Sleep(Timeout);
            IWebElement search = _driver.FindElement(By.CssSelector("div#filters-partner-name input"));
            Thread.Sleep(Timeout);
            search.SendKeys(PartnerName);
            Thread.Sleep(Timeout);

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
                        }
                        else
                        {
                            Logger.Info($"Partner: {Name} with ID: {ID} was created on: {Date} is DISABLED");
                        }
                    }
                }
            }
        }
    }
}
