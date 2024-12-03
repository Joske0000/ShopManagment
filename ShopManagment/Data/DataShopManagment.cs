namespace ShopManagment.Data
{
    public class DataShopManagment
    {
        public string? ShopName { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }
        public string? TerminalName { get; set; }
        public string? PartnerName { get; set; }
    }
    public static class DataShopManagmentRepository
    {
        public static List<DataShopManagment> data = new()
        {
            new DataShopManagment
            {
                ShopName = Faker.Name.FullName(),
                City = Faker.Address.City(),
                Address = Faker.Address.StreetAddress(),
                TerminalName = Faker.Name.First(),
                PartnerName = Faker.Company.Name()
            }
        };
    }
}
