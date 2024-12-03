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

            Metods Shop = new Metods(_driver);

            var shopData = DataShopManagmentRepository.data[0];

            if (shopData.ShopName != null && shopData.City != null && shopData.Address != null && shopData.TerminalName != null && shopData.PartnerName != null)
            {
                Shop.AddPartner(shopData.PartnerName);         

                NetworkDisconnection();

                //Shop.AddShop(shopData.ShopName, shopData.City, shopData.Address, shopData.PartnerName);

                Thread.Sleep(200);

                //Shop.AddTerminal(shopData.ShopName, shopData.TerminalName);

                //Thread.Sleep(100);

                Shop.SearchPartner(shopData.PartnerName);

                Thread.Sleep(100);

                //Shop.SearchShop(shopData.ShopName);

                //Thread.Sleep(100);

                //Shop.SearchTerminal(shopData.TerminalName);
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

            Metods Shop = new Metods(_driver);        

            string? page1 = Shop.GetLastShopIdOnPage(0);   

            string? page2 = Shop.GetLastShopIdOnPage(2);

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
