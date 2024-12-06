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
        
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Cleanup();
        }

        [Test]
        public void Pagination_Test()
        {
            Pagination pagination = new Pagination(_driver);

            string? page1 = pagination.GetLastShopIdOnPage(0, Timeout);

            string? page2 = pagination.GetLastShopIdOnPage(3, Timeout);

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

            if (Data.PartnerName != null)
            {
              
                Partner.AddPartner(Data.PartnerName, Timeout);                            
                Partner.PartnerDetails(Data.PartnerName, Timeout); 
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
      
            if (Data.PartnerName != null && Data.ShopName != null && Data.City != null && Data.Address != null)
            {

                Shop.AddShop(Data.ShopName, Data.City, Data.Address, Data.PartnerName, Timeout);           
                Shop.ShopDetails(Data.ShopName, Timeout);
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
       
            if (Data.ShopName != null && Data.TerminalName != null)
            {

                Terminal.AddTerminal(Data.ShopName, Data.TerminalName, Timeout);
                Terminal.TerminalDetails(Data.TerminalName, Timeout);
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
               
            var Data = _data[0];

            var clipboard = new Clipboard();

            Terminals Terminal = new Terminals(_driver);

            Terminal.EnrollTerminal(Data.ShopName, Data.TerminalName, Timeout);

            string EnrollCode = clipboard.GetText();

            Logger.Info($"Enroll Code: {EnrollCode}");  
        }      
    }
}
