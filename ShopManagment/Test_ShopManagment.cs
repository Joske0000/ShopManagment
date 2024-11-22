using NLog;
using ShopManagment.Pages;
using ShopManagment.Setup;
using ShopManagment.Data;


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

            if (shopData.ShopName != null && shopData.City != null && shopData.Address != null && shopData.TerminalName != null)
            {
                Shop.AddShop(shopData.ShopName, shopData.City, shopData.Address);
                Logger.Info("SHOP ADDED");
                Thread.Sleep(100);
                Shop.SearchShop(shopData.ShopName);
                Logger.Info("SHOP FOUND");
                Thread.Sleep(500);
                Shop.AddTerminal(shopData.TerminalName);
                Logger.Info("TERMINAL ADDED");
                Thread.Sleep(100);
                Shop.SearchTerminal(shopData.TerminalName);
                Logger.Info("TERMINAL FOUND");
                Thread.Sleep(500);  
            }
            else
            {
                Logger.Error("No data from repository");
                Assert.Fail("Cannot add shop or terminal");
            }


            Thread.Sleep(500);
        }

        [Test]
        public void Pagination_Test()
        {
            Setup("chrome");

            Metods Shop = new Metods(_driver);

            string? page1 = Shop.GetLastShopIdOnPage(0);

            string? page2 = Shop.GetLastShopIdOnPage(5);

            if (page1 != page2)
            {
                Logger.Info("Pagination is working");
                Assert.Pass($"Pagination is working");
            }
            else
            {
                Logger.Error("Pagination is not working");
                Assert.Fail($"Pagination is not working");
            }

            Thread.Sleep(2500);
        }

       
    }
}
