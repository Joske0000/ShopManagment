using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using NLog;
using OpenQA.Selenium.Interactions;
using System.ComponentModel;


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
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
        }

        IWebElement AddPartnerButton => _driver.FindElement(By.Id("filters-partner-add"));
        IWebElement AddPartnerName => _driver.FindElement(By.XPath("//*[@id=\"partner-modal-form-name\"]/div/div/div/div/input"));
        IWebElement addShop => _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"filters-shop-add\"]/span/i")));
        IWebElement dropdownMenuShop => _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/div[3]/div/div[2]/div/form/div[1]/div/div/div/span")));
        IWebElement dropdownShops => _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/div[3]/div/div[2]/div/form/div[1]/div/div/div/div[2]/div/div[2]/label[1]/span")));
        IWebElement containerTerminal => _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"terminal-modal-form-shop-container\"]")));
        IWebElement submit => _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"shop-modal-buttons-save\"]")));
        IWebElement addTerminal => _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"filters-terminal-add\"]/span/i")));
        IWebElement addTerminalName => _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"terminal-modal-form-name\"]/div/div/div/div/input")));
        IWebElement ShopInTerminal => _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"terminal-modal-form-shop-container\"]/div[2]/div/div[2]/label[1]/span")));

        
        public void AddShop(string ShopName, string City, string Address)
        {
            _driver.Navigate().GoToUrl("https://kmw-retail-sk.apps.ocp01-shared.t.dc1.cz.ipa.ifortuna.cz/kmw/shop");
            Logger.Info("Shop tab opened");
            addShop.Click();
            Logger.Info("AddShop button clicked");
            _driver.FindElement(By.XPath("//*[@id=\"shop-modal-form-name\"]/div/div/div/div/input")).SendKeys(ShopName);
            Logger.Info($"Shop name: {ShopName} typed");
            _driver.FindElement(By.XPath("//*[@id=\"shop-modal-form-city\"]/div/div/div/div/input")).SendKeys(City);
            Logger.Info($"City: {City} typed");
            _driver.FindElement(By.XPath("//*[@id=\"shop-modal-form-address\"]/div/div/div/div/input")).SendKeys(Address);
            Logger.Info($"Adress: {Address} typed");
            dropdownMenuShop.Click();
            dropdownShops.Click();
            Thread.Sleep(1000);
            submit.Click();
            Thread.Sleep(500);
        }

        public void AddTerminal(string TerminalName)
        {
            _driver.Navigate().GoToUrl("https://kmw-retail-sk.apps.ocp01-shared.t.dc1.cz.ipa.ifortuna.cz/kmw/terminal");
            Logger.Info("Terminal tab opened");
            addTerminal.Click();
            containerTerminal.Click();
            Thread.Sleep(500);
            ShopInTerminal.Click();
            Thread.Sleep(500);
            addTerminalName.SendKeys(TerminalName);
            Logger.Info($"Terminal name: {TerminalName} typed");
            Thread.Sleep(500);
            submit.Click();
        }

        public void AddPartner(string PartnerName)
        {
            _driver.Navigate().GoToUrl("https://kmw-retail-sk.apps.ocp01-shared.t.dc1.cz.ipa.ifortuna.cz/kmw/partner");
            Logger.Info("Partner tab opened");
            AddPartnerButton.Click();
            Thread.Sleep(500);
            AddPartnerName.SendKeys(PartnerName);
            Logger.Info($"Partner name: {PartnerName} typed");
            Thread.Sleep(500);
            _driver.FindElement(By.XPath("//*[@id=\"partner-modal-buttons-save\"]")).Click();
        }

        public string? GetLastShopIdOnPage(int n)
        {
            _driver.Navigate().GoToUrl("https://kmw-retail-sk.apps.ocp01-shared.t.dc1.cz.ipa.ifortuna.cz/kmw/shop");
            Thread.Sleep(500);
            Pagination(n);
            Logger.Info("Shop tab opened");   
            IWebElement table = _driver.FindElement(By.XPath("/html/body/div[1]/main/div/div[2]/table"));
            Logger.Info("Table found");
            IList<IWebElement> tableRows = table.FindElements(By.TagName("tr"));
           
            if (tableRows.Count > 0)
            {
                IWebElement lastRow = tableRows[tableRows.Count - 1];
                Logger.Info($"Last Row: {lastRow} found");
                string lastrowid = lastRow.GetAttribute("id");
                IWebElement specificRow = table.FindElement(By.Id(lastrowid));
                IList<IWebElement> tableDataCells = specificRow.FindElements(By.TagName("td"));
                if (tableDataCells.Count > 0)
                {
                    string shopId = tableDataCells[1].Text;
                    Logger.Info($"Shop ID: {shopId} found");
                    return shopId;
                }
            }
            return null;
        }
        public void Pagination(int numberOfScrolls)
        {
            _driver.Navigate().GoToUrl("https://kmw-retail-sk.apps.ocp01-shared.t.dc1.cz.ipa.ifortuna.cz/kmw/shop");
            Thread.Sleep(2500);
            _driver.FindElement(By.XPath("//*[@id=\"shop-table-1\"]")).Click();
            Actions actions = new Actions(_driver);

            for (int i = 0; i < numberOfScrolls; i++)
            {
                actions.SendKeys(Keys.PageDown).Perform();
                Logger.Info($"PageDown {i} performed");
                Thread.Sleep(500);
            }
        }

        public void SearchShop(string ShopName)
        {
            _driver.Navigate().GoToUrl("https://kmw-retail-sk.apps.ocp01-shared.t.dc1.cz.ipa.ifortuna.cz/kmw/shop");
            Logger.Info("Shop tab opened");
            IWebElement search = _driver.FindElement(By.XPath("//*[@id=\"filters-shop-name\"]/div/div/div/div/input"));
            search.SendKeys(ShopName);
            Logger.Info($"Shop name: {ShopName} typed");
            Thread.Sleep(500);
        }

        public void SearchTerminal(string TerminalName)
        {
            _driver.Navigate().GoToUrl("https://kmw-retail-sk.apps.ocp01-shared.t.dc1.cz.ipa.ifortuna.cz/kmw/terminal");
            Logger.Info("Terminal tab opened");
            IWebElement search = _driver.FindElement(By.XPath("//*[@id=\"filters-terminal-name\"]/div/div/div/div/input"));
            search.SendKeys(TerminalName);
            Logger.Info($"Terminal name: {TerminalName} typed");
            Thread.Sleep(500);
        }
       
        
    }
}