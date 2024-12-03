using NLog;
using ShopManagment.Data;
using ShopManagment.Pages;
using ShopManagment.Setup;


namespace ShopManagment
{

    public class Tests : SetupDriver
    {
        [Test]
        public void Add_New_Shop_Terminal()
        {
            Setup("chrome");

            Shops Shop = new Shops(_driver);
            Partners Partner = new Partners(_driver);
            Terminals Terminal = new Terminals(_driver);    

            var shopData = DataShopManagmentRepository.data[0];

            if (shopData.ShopName != null && shopData.City != null && shopData.Address != null && shopData.TerminalName != null && shopData.PartnerName != null)
            {
                Partner.AddPartner(shopData.PartnerName);         

                NetworkDisconnection();

                //Shop.AddShop(shopData.ShopName, shopData.City, shopData.Address, shopData.PartnerName);

                Thread.Sleep(200);

                //Terminal.AddTerminal(shopData.ShopName, shopData.TerminalName);

                //Thread.Sleep(100);

                Partner.SearchPartner(shopData.PartnerName);

                //Thread.Sleep(100);

                //Shop.SearchShop(shopData.ShopName);

                //Thread.Sleep(100);

                //Terminal.SearchTerminal(shopData.TerminalName);
            }
            else
            {
                Logger.Error("No data from repository");
                Assert.Fail("No data from repository");
            }

            _driver.Quit();
          
        }

        [Test]
        public void Pagination_Test()
        {
            Setup("chrome");

            Pagination pagination = new Pagination(_driver);

            string? page1 = pagination.GetLastShopIdOnPage(0);   

            string? page2 = pagination.GetLastShopIdOnPage(2);

            if (page1 != page2)
            {
                Logger.Info("Pagination is working");             
            }
            else
            {
                Logger.Error("Pagination is not working");
            }

            _driver.Quit();
        }
      
    }

}
