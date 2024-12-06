﻿using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;


namespace ShopManagment.Pages
{
    internal class Terminals
    {
        private readonly IWebDriver _driver;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly WebDriverWait _wait;
        string tableselector = "overflow-auto";

        public Terminals(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            _driver.Navigate().GoToUrl("https://kmw-retail-sk.apps.ocp01-shared.t.dc1.cz.ipa.ifortuna.cz/kmw/terminal");
        }

        public void AddTerminal(string ShopName, string TerminalName, int Timeout)
        {
                    
            Logger.Info("Terminal tab opened");   
            _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("filters-terminal-add"))).Click();
            _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div#terminal-modal-form-name input"))).SendKeys(TerminalName);
            _wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("body > div:nth-child(4) > div:nth-child(1) > div:nth-child(2) > div:nth-child(1) > form:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > span:nth-child(1) > label:nth-child(1) > div:nth-child(1) > div:nth-child(2)"))).Click();
            _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("terminal-modal-form-shop-input-search"))).SendKeys(ShopName);
            Thread.Sleep(Timeout);
            _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#terminal-modal-form-shop-container > div.absolute.left-\\[-1px\\].w-\\[calc\\(100\\%\\+2px\\)\\].z-50.rounded-b-md.top-full.bg-white.border-r.border-b.border-l.border-t.text-left.overflow-y-auto.border-grey-200.max-h-24 > div > div.relative.flex.flex-col.justify-center.items-center.w-full > label > span"))).Click();
            _wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("button#shop-modal-buttons-save"))).Click();
            WaitForTableToLoad(_driver, "overflow-auto", TimeSpan.FromMilliseconds(Timeout));
            Logger.Info($"TERMINAL: {TerminalName} ADDED");
        }
        public void SearchTerminal(string TerminalName, int Timeout)
        {
            Logger.Info("Searching for Terminal");  
            _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div#filters-terminal-name input"))).Clear();
            _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div#filters-terminal-name input"))).SendKeys(TerminalName);
            WaitForTableToLoad(_driver, "overflow-auto", TimeSpan.FromMilliseconds(Timeout));                
            IWebElement table = _driver.FindElement(By.ClassName("overflow-auto"));
            IList<IWebElement> tableRow = table.FindElements(By.TagName("tr"));

            bool TerminalFound = false;

            foreach (var row in tableRow)
            {
                if (row.Text.Contains(TerminalName))
                {
                    Logger.Info($"Terminal: {TerminalName} FOUND");
                    TerminalFound = true;
                    break;
                }
            }
            if (!TerminalFound)
            {
                Logger.Warn($"Terminal: {TerminalName} NOT FOUND");
            }
        }
        public void TerminalDetails(string TerminalName, int Timeout)
        {
            Logger.Info("Getting Terminal Details");
            _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div#filters-terminal-name input"))).Clear();
            _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div#filters-terminal-name input"))).SendKeys(TerminalName);
            WaitForTableToLoad(_driver, tableselector, TimeSpan.FromMilliseconds(Timeout));
            IWebElement table = _wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("overflow-auto"))); 
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
                        string ShopName = tableDataCells[5].Text;
                        string Name = tableDataCells[4].Text;
                        string Date = tableDataCells[6].Text;          
                        string DisabledTerminal = tableDataCells[7].Text;
                        if (DisabledTerminal == "-") 
                        {               
                            Logger.Info($"Terminal: {Name} on shop: {ShopName} created on: {Date} is enabled");
                        }
                        else 
                        {
                        Logger.Info($"Terminal: {Name} on shop: {ShopName} was created on: {Date} is DISABLED");
                        }
                    }
                }

            }
        }
        public void EnrollTerminal(string ShopName, string TerminalName, int Timeout)
        {
            Logger.Info("Enrolling Terminal");
            _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div#filters-terminal-name input"))).SendKeys(TerminalName);               
            _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("filters-terminal-shop-container"))).Click();
            _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("filters-terminal-shop-input-search"))).SendKeys(ShopName);
            Thread.Sleep(Timeout);
            _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"filters-terminal-shop-container\"]/div[2]/div/div[2]/label/span"))).Click();
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
                        IWebElement enroll = tableDataCells[9].FindElement(By.TagName("i"));
                        string enrolled = enroll.GetDomAttribute("title");
                        string ID = tableDataCells[2].Text;
                        Logger.Info($"Terminal: {TerminalName} with ID: {ID} is {enroll.GetDomAttribute("title")}");

                        if (enrolled == "Enrolled")
                        {
                            Logger.Info($"Terminal: {TerminalName} with ID: {ID} is already enrolled");
                        }
                        else
                        {
                            string path = $"terminal-table-generate-enroll-{ID}";
                            enroll.Click();
                            Thread.Sleep(Timeout);
                            _driver.FindElement(By.Id(path)).Click();
                            Thread.Sleep(Timeout);
                            _driver.FindElement(By.Id("terminal-enrollModal-copyEnroll")).Click();
                            Thread.Sleep(Timeout);
                            Logger.Info($"Terminal: {TerminalName} Enroll Code with ID: {ID} copied");
                        }
                    }
                    else
                    {
                        Logger.Warn($"Terminal: {TerminalName} NOT FOUND");
                    }

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

                Thread.Sleep(timeout);

                currentRowCount = table.FindElements(By.TagName("tr")).Count;

            } while (currentRowCount > previousRowCount);

            return table;
        }
    }

}
