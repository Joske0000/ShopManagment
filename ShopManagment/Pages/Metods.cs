using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;


namespace ShopManagment.Pages
{
    internal class Metods
    {
        private readonly IWebDriver _driver;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly WebDriverWait _wait;

        public Metods(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        IWebElement AddPartnerButton => _driver.FindElement(By.Id("filters-partner-add"));
        IWebElement AddPartnerName => _driver.FindElement(By.XPath("//*[@id=\"partner-modal-form-name\"]/div/div/div/div/input"));
        IWebElement addShop => _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"filters-shop-add\"]/span/i")));
        IWebElement dropdownSelectPartner => _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div#shop-modal-form-partner-container div.relative.z-20.text-left.text-xs.h-4.min-w-\\5b 1px\\5d.cursor-pointer")));
        IWebElement PartnerinShop => _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("input#shop-modal-form-partner-input-search")));
        IWebElement PartnerSelect => _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div#shop-modal-form-partner-container label > span")));
        IWebElement containerTerminal => _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div#terminal-modal-form-shop-container div.relative.z-20.text-left.text-xs.h-4.min-w-\\5b 1px\\5d.cursor-pointer")));
        IWebElement submitShop => _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("button#shop-modal-buttons-save")));
        IWebElement submitTerminal => _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("button#shop-modal-buttons-save")));
        IWebElement addTerminal => _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("filters-terminal-add")));
        IWebElement addTerminalName => _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div#terminal-modal-form-name input")));
        IWebElement ShopInTerminal => _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("input#terminal-modal-form-shop-input-search")));
        IWebElement SelectShop => _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div#terminal-modal-form-shop-container label > span")));

        public void AddPartner(string PartnerName)
        {
            _driver.Navigate().GoToUrl("https://kmw-retail-sk.apps.ocp01-shared.t.dc1.cz.ipa.ifortuna.cz/kmw/partner");
            Logger.Info("Partner tab opened");
            AddPartnerButton.Click();
            Thread.Sleep(500);
            AddPartnerName.SendKeys(PartnerName);
            Thread.Sleep(500);
            _driver.FindElement(By.XPath("//*[@id=\"partner-modal-buttons-save\"]")).Click();
            Logger.Info($"PARTNER: {PartnerName} ADDED");
        }
        public void AddShop(string ShopName, string City, string Address, string PartnerName)
        {
            _driver.Navigate().GoToUrl("https://kmw-retail-sk.apps.ocp01-shared.t.dc1.cz.ipa.ifortuna.cz/kmw/shop");
            Logger.Info("Shop tab opened");
            addShop.Click(); 
            _driver.FindElement(By.XPath("//*[@id=\"shop-modal-form-name\"]/div/div/div/div/input")).SendKeys(ShopName);  
            _driver.FindElement(By.XPath("//*[@id=\"shop-modal-form-city\"]/div/div/div/div/input")).SendKeys(City);   
            _driver.FindElement(By.XPath("//*[@id=\"shop-modal-form-address\"]/div/div/div/div/input")).SendKeys(Address);
            Thread.Sleep(100);
            dropdownSelectPartner.Click();    
            Thread.Sleep(100);
            PartnerinShop.SendKeys(PartnerName);
            Thread.Sleep(100);
            PartnerSelect.Click();
            Thread.Sleep(100);
            submitShop.Click();
            Thread.Sleep(100);
            Logger.Info($"SHOP: {ShopName} ADDED");
        }

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

        public void SearchShop(string ShopName)
        {
            _driver.Navigate().GoToUrl("https://kmw-retail-sk.apps.ocp01-shared.t.dc1.cz.ipa.ifortuna.cz/kmw/shop");
            Logger.Info("Shop tab opened");
            Thread.Sleep(1000);
            IWebElement search = _driver.FindElement(By.CssSelector("div#filters-shop-name input"));
            Thread.Sleep(1000);
            search.SendKeys(ShopName);
            Thread.Sleep(1000);

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
        public string? GetLastShopIdOnPage(int n)
        {
            Pagination(n);
            Logger.Info("Shop tab opened");
            IWebElement table = _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#shop-table > table > tbody")));
            Thread.Sleep(1000);
            Logger.Info("Table found");
            IList<IWebElement> tableRows = table.FindElements(By.TagName("tr"));
            Thread.Sleep(1000);

            if (tableRows.Count > 0)
            {
                IWebElement lastRow = tableRows[tableRows.Count -1];          
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
        public void Pagination(int numberOfScrolls)
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