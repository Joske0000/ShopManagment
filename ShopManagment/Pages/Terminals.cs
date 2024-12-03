using NLog;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeleniumExtras.WaitHelpers;

namespace ShopManagment.Pages
{
    internal class Terminals
    {
        private readonly IWebDriver _driver;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly WebDriverWait _wait;

        public Terminals(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }
        IWebElement submitTerminal => _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("button#shop-modal-buttons-save")));
        IWebElement addTerminal => _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("filters-terminal-add")));
        IWebElement addTerminalName => _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div#terminal-modal-form-name input")));
        IWebElement ShopInTerminal => _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("input#terminal-modal-form-shop-input-search")));
        IWebElement containerTerminal => _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div#terminal-modal-form-shop-container div.relative.z-20.text-left.text-xs.h-4.min-w-\\5b 1px\\5d.cursor-pointer")));
        IWebElement SelectShop => _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div#terminal-modal-form-shop-container label > span")));

        public void AddTerminal(string ShopName, string TerminalName)
        {
            _driver.Navigate().GoToUrl("https://kmw-retail-sk.apps.ocp01-shared.t.dc1.cz.ipa.ifortuna.cz/kmw/terminal");
            Logger.Info("Terminal tab opened");
            addTerminal.Click();
            Thread.Sleep(200);
            addTerminalName.SendKeys(TerminalName);
            Thread.Sleep(200);
            containerTerminal.Click();
            ShopInTerminal.SendKeys(ShopName);
            Thread.Sleep(200);
            SelectShop.Click();
            Thread.Sleep(100);
            submitTerminal.Click();
            Logger.Info($"TERMINAL: {TerminalName} ADDED");
        }
        public void SearchTerminal(string TerminalName)
        {
            _driver.Navigate().GoToUrl("https://kmw-retail-sk.apps.ocp01-shared.t.dc1.cz.ipa.ifortuna.cz/kmw/terminal");
            Logger.Info("Terminal tab opened");
            Thread.Sleep(1000);
            IWebElement search = _driver.FindElement(By.CssSelector("div#filters-terminal-name input"));
            Thread.Sleep(1000);
            search.SendKeys(TerminalName);
            Thread.Sleep(1000);

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
    }
}
