using NLog;
using ShopManagment.Data;
using ShopManagment.Pages;
using ShopManagment.Setup;
using TextCopy;

namespace ShopManagment
{
    [TestFixture]
    public class Tests : SetupDriver
    {     
        private List<DataShopManagment> _data;  

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _data = DataShopManagmentRepository.data;
            Setup("chrome");      
           //SlowNetworkConditions();                  
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Cleanup();
        }

        [Test]
        public void Pagination_Test()
        {
            int timeout = 800;

            Pagination pagination = new Pagination(_driver);

            string? page1 = pagination.GetLastShopIdOnPage(0, timeout);

            string? page2 = pagination.GetLastShopIdOnPage(2, timeout);

            if (page1 != page2)
            {
                Logger.Info("Pagination is working");
            }
            else
            {
                Logger.Error("Pagination is not working");
            }

        }

        [Test]
        public void Add_New_Partner()
        {
           
            Partners Partner = new Partners(_driver);

            var Data = _data[0];

            int timeout = 500;        

            if (Data.PartnerName != null)
            {
              
                Partner.AddPartner(Data.PartnerName, timeout);

                Partner.SearchPartner(Data.PartnerName, timeout);
                
            }
            else
            {
                Logger.Error("No data from repository");
                Assert.Fail("No data from repository");
            }
         
        }

        [Test]
        public void Add_New_Shop()
        {         
            Shops Shop = new Shops(_driver);

            var Data = _data[0];

            int timeout = 500;
      
            if (Data.PartnerName != null && Data.ShopName != null && Data.City != null && Data.Address != null)
            {

                Shop.AddShop(Data.ShopName, Data.City, Data.Address, Data.PartnerName, timeout);           
                Shop.SearchShop(Data.ShopName, timeout);
            }
            else
            {
                Logger.Error("No data from repository");
                Assert.Fail("No data from repository");
            }

        }

        [Test]
        public void Add_New_Terminal()
        {
            Terminals Terminal = new Terminals(_driver);
    
            var Data = _data[0];

            int timeout = 500;
       
            if (Data.ShopName != null && Data.TerminalName != null)
            {

                Terminal.AddTerminal(Data.ShopName, Data.TerminalName, timeout);
                Terminal.SearchTerminal(Data.TerminalName, timeout);

            }
            else
            {
                Logger.Error("No data from repository");
                Assert.Fail("No data from repository");
            }

        }

        [Test]
        public void Enroll_Terminal()
        {
            int timeout = 800;

            var clipboard = new Clipboard();

            Terminals Terminal = new Terminals(_driver);
              
            Terminal.EnrollTerminal("Podstrana Mir","Kasanova 1", timeout);

            string EnrollCode = clipboard.GetText();

            Logger.Info($"Clipboard content: {EnrollCode}");

        }

  
      
    }

}
