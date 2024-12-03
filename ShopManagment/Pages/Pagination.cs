using NLog;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Interactions;
using SeleniumExtras.WaitHelpers;

namespace ShopManagment.Pages
{
    internal class Pagination
    {
        private readonly IWebDriver _driver;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly WebDriverWait _wait;

        public Pagination(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        public string? GetLastShopIdOnPage(int n)
        {
            pagination(n);
            Logger.Info("Shop tab opened");
            IWebElement table = _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#shop-table > table > tbody")));
            Thread.Sleep(1000);
            Logger.Info("Table found");
            IList<IWebElement> tableRows = table.FindElements(By.TagName("tr"));
            Thread.Sleep(1000);

            if (tableRows.Count > 0)
            {
                IWebElement lastRow = tableRows[tableRows.Count - 1];
                string lastrowid = lastRow.GetDomAttribute("Id");
                IWebElement specificRow = table.FindElement(By.Id(lastrowid));
                IList<IWebElement> tableDataCells = specificRow.FindElements(By.TagName("td"));
                if (tableDataCells.Count > 0)
                {
                    string shopId = tableDataCells[1].Text;
                    string shopName = tableDataCells[3].Text;
                    Logger.Info($"Last shop on page with ID {shopId} and NAME {shopName} found");
                    return shopId;
                }
            }
            return null;
        }
        public void pagination(int numberOfScrolls)
        {
            _driver.Navigate().GoToUrl("https://kmw-retail-sk.apps.ocp01-shared.t.dc1.cz.ipa.ifortuna.cz/kmw/shop");
            By shopTableXPath = By.XPath("//*[@id='shop-table-1']");
            IWebElement shopTableElement = _wait.Until(ExpectedConditions.ElementToBeClickable(shopTableXPath));
            shopTableElement.Click();
            Actions actions = new Actions(_driver);

            for (int i = 0; i < numberOfScrolls; i++)
            {
                actions.SendKeys(Keys.PageDown).Perform();
                Logger.Info($"PageDown {i} performed");
                Thread.Sleep(1000);
            }
        }
    }
}
