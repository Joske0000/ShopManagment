﻿using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;


namespace ShopManagment.Pages
{
    internal class Shops
    {
        private readonly IWebDriver _driver;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly WebDriverWait _wait;

        public Shops(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }
        
        IWebElement addShop => _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"filters-shop-add\"]/span/i")));
        IWebElement dropdownSelectPartner => _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div#shop-modal-form-partner-container div.relative.z-20.text-left.text-xs.h-4.min-w-\\5b 1px\\5d.cursor-pointer")));
        IWebElement PartnerinShop => _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("input#shop-modal-form-partner-input-search")));
        IWebElement PartnerSelect => _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div#shop-modal-form-partner-container label > span")));
        IWebElement submitShop => _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("button#shop-modal-buttons-save")));
       
       
        public void AddShop(string ShopName, string City, string Address, string PartnerName, int Timeout)
        {
            _driver.Navigate().GoToUrl("https://kmw-retail-sk.apps.ocp01-shared.t.dc1.cz.ipa.ifortuna.cz/kmw/shop");
            Logger.Info("Shop tab opened");
            addShop.Click(); 
            _driver.FindElement(By.XPath("//*[@id=\"shop-modal-form-name\"]/div/div/div/div/input")).SendKeys(ShopName);  
            _driver.FindElement(By.XPath("//*[@id=\"shop-modal-form-city\"]/div/div/div/div/input")).SendKeys(City);   
            _driver.FindElement(By.XPath("//*[@id=\"shop-modal-form-address\"]/div/div/div/div/input")).SendKeys(Address);
            dropdownSelectPartner.Click();    
            Thread.Sleep(Timeout);
            PartnerinShop.SendKeys(PartnerName);
            Thread.Sleep(Timeout);
            PartnerSelect.Click();
            Thread.Sleep(Timeout);
            submitShop.Click();
            Thread.Sleep(Timeout);
        }    

        public void SearchShop(string ShopName, int Timeout)
        {
            _driver.Navigate().GoToUrl("https://kmw-retail-sk.apps.ocp01-shared.t.dc1.cz.ipa.ifortuna.cz/kmw/shop");
            Logger.Info("Shop tab opened");
            Thread.Sleep(Timeout);
            IWebElement search = _driver.FindElement(By.CssSelector("div#filters-shop-name input"));
            Thread.Sleep(Timeout);
            search.SendKeys(ShopName);
            Thread.Sleep(Timeout);

            IWebElement table = _driver.FindElement(By.ClassName("overflow-auto"));
            IList<IWebElement> tableRow = table.FindElements(By.TagName("tr"));

            bool shopFound = false;

            foreach (var row in tableRow)
            {
                if (row.Text.Contains(ShopName))
                {
                    Logger.Info($"Shop: {ShopName} FOUND");
                    shopFound = true;
                    break;
                }
            }
            if (!shopFound)
            {
                Logger.Warn($"Shop: {ShopName} NOT FOUND");
            }
        }
        public void ShopDetails(string ShopName, int Timeout)
        {
            _driver.Navigate().GoToUrl("https://kmw-retail-sk.apps.ocp01-shared.t.dc1.cz.ipa.ifortuna.cz/kmw/shop");
            Logger.Info("Shop tab opened");
            Thread.Sleep(Timeout);
            IWebElement search = _driver.FindElement(By.CssSelector("div#filters-shop-name input"));
            Thread.Sleep(Timeout);
            search.SendKeys(ShopName);
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
                        string ID = tableDataCells[1].Text;
                        string Date = tableDataCells[7].Text;
                        string City = tableDataCells[4].Text;
                        string Address = tableDataCells[5].Text;
                        string Name = tableDataCells[3].Text;
                        string DisabledShop = tableDataCells[8].Text;
                        if (DisabledShop == "-")
                        {
                            Logger.Info($"Shop: {Name} with ID: {ID}, addres: {Address},{City} created on: {Date} is enabled");
                        }
                        else
                        {
                            Logger.Info($"Shop: {Name} with ID: {ID}, addres: {Address},{City} created on: {Date} is DISABLED");
                        }
                    }
                }
            }
        }
    }
}