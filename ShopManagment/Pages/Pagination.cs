﻿using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
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
            _driver.Navigate().GoToUrl("https://kmw-retail-sk.apps.ocp01-shared.t.dc1.cz.ipa.ifortuna.cz/kmw/shop");
        }

        public string? GetLastShopIdOnPage(int n, int Timeout)
        {
            pagination(n, Timeout);                   
            WaitForTableToLoad(_driver, "#shop-table > table > tbody", TimeSpan.FromMilliseconds(Timeout));
            IWebElement table = _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#shop-table > table > tbody")));
            IList<IWebElement> tableRows = table.FindElements(By.TagName("tr"));

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
        public void pagination(int numberOfScrolls, int Timeout)
        {          
            By shopTableXPath = By.XPath("//*[@id='shop-table-1']");
            IWebElement shopTableElement = _wait.Until(ExpectedConditions.ElementToBeClickable(shopTableXPath));
            shopTableElement.Click();
            Actions actions = new Actions(_driver);

            for (int i = 0; i < numberOfScrolls; i++)
            {
                actions.SendKeys(Keys.PageDown).Perform();
                WaitForTableToLoad(_driver, "#shop-table > table > tbody", TimeSpan.FromMilliseconds(Timeout));
            }
        }

        static IWebElement WaitForTableToLoad(IWebDriver driver, string tableCssSelector, TimeSpan timeout)
        {
            WebDriverWait wait = new WebDriverWait(driver, timeout);

            IWebElement table = wait.Until(ExpectedConditions.ElementExists(By.CssSelector(tableCssSelector)));

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
