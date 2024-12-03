using NLog;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void AddPartner(string PartnerName)
        {
            _driver.Navigate().GoToUrl("https://kmw-retail-sk.apps.ocp01-shared.t.dc1.cz.ipa.ifortuna.cz/kmw/partner");
            Logger.Info("Partner tab opened");
            AddPartnerButton.Click();
            Thread.Sleep(500);
            AddPartnerName.SendKeys(PartnerName);
            Thread.Sleep(500);
            _driver.FindElement(By.XPath("//*[@id=\"partner-modal-buttons-save\"]")).Click();        
        }

        public void SearchPartner(string PartnerName)
        {
            _driver.Navigate().GoToUrl("https://kmw-retail-sk.apps.ocp01-shared.t.dc1.cz.ipa.ifortuna.cz/kmw/partner");
            Logger.Info("Partner tab opened");
            Thread.Sleep(1000);
            IWebElement search = _driver.FindElement(By.CssSelector("div#filters-partner-name input"));
            Thread.Sleep(1000);
            search.SendKeys(PartnerName);
            Thread.Sleep(1000);

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
    }
}
