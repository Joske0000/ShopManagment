using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using TextCopy;


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
        IWebElement ShopInTerminal => _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("terminal-modal-form-shop-input-search")));
        IWebElement containerTerminal => _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("body > div:nth-child(4) > div:nth-child(1) > div:nth-child(2) > div:nth-child(1) > form:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > div:nth-child(1) > span:nth-child(1) > label:nth-child(1) > div:nth-child(1) > div:nth-child(2)")));
        IWebElement SelectShop => _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"terminal-modal-form-shop-container\"]/div[2]/div/div[2]/label/span")));

        public void AddTerminal(string ShopName, string TerminalName, int Timeout)
        {
            
            _driver.Navigate().GoToUrl("https://kmw-retail-sk.apps.ocp01-shared.t.dc1.cz.ipa.ifortuna.cz/kmw/terminal");
            Logger.Info("Terminal tab opened");
            addTerminal.Click();
            Thread.Sleep(Timeout);
            addTerminalName.SendKeys(TerminalName);
            Thread.Sleep(Timeout);
            containerTerminal.Click();
            Thread.Sleep(Timeout);
            ShopInTerminal.SendKeys(ShopName);    
            Thread.Sleep(Timeout);
            SelectShop.Click();       
            submitTerminal.Click();
            Thread.Sleep(Timeout);
            Logger.Info($"TERMINAL: {TerminalName} ADDED");
        }
        public void SearchTerminal(string TerminalName, int Timeout)
        {
            _driver.Navigate().GoToUrl("https://kmw-retail-sk.apps.ocp01-shared.t.dc1.cz.ipa.ifortuna.cz/kmw/terminal");
            Logger.Info("Terminal tab opened");
            Thread.Sleep(Timeout);
            IWebElement search = _driver.FindElement(By.CssSelector("div#filters-terminal-name input"));
            Thread.Sleep(Timeout);
            search.SendKeys(TerminalName);
            Thread.Sleep(Timeout);

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
            _driver.Navigate().GoToUrl("https://kmw-retail-sk.apps.ocp01-shared.t.dc1.cz.ipa.ifortuna.cz/kmw/terminal");
            Logger.Info("Terminal tab opened");
          
            IWebElement search = _wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("div#filters-terminal-name input")));           
            search.SendKeys(TerminalName);
            Thread.Sleep(Timeout);
            IWebElement table = _wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("overflow-auto"))); 
            Thread.Sleep(Timeout);
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
            _driver.Navigate().GoToUrl("https://kmw-retail-sk.apps.ocp01-shared.t.dc1.cz.ipa.ifortuna.cz/kmw/terminal");
            Logger.Info("Terminal tab opened");         
            _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("filters-terminal-shop-container")));
            _driver.FindElement(By.Id("filters-terminal-shop-container")).Click();        
            _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("filters-terminal-shop-input-search")));
            _driver.FindElement(By.Id("filters-terminal-shop-input-search")).SendKeys(ShopName);
            Thread.Sleep(Timeout);
            _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"filters-terminal-shop-container\"]/div[2]/div/div[2]/label/span")));
            _driver.FindElement(By.XPath("//*[@id=\"filters-terminal-shop-container\"]/div[2]/div/div[2]/label/span")).Click();
            _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div#filters-terminal-name input")));
            IWebElement search = _driver.FindElement(By.CssSelector("div#filters-terminal-name input"));        
            search.SendKeys(TerminalName);
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
                        IWebElement enroll = tableDataCells[9].FindElement(By.TagName("i"));
                        Thread.Sleep(Timeout);
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
                }
          
            }
        }

}
}
