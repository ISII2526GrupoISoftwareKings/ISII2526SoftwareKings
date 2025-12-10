using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.RestockDTOs;

namespace AppForSEII2526.UT.RestocksController_test
{
    public class GetRestock_test : AppForSEII25264SqliteUT
    {
        private const string _userName = "alberto.bueno@uclm.es";
        private const string _adminName = "Alberto";
        private const string _adminSurname = "Bueno";
        private const string _deliveryAddress = "Avda. España s/n, Avda. España s/n, Albacete 02071";
        private const string _brandName = "Elyte";
        private const string _itemName = "Protein Bar";

        private static readonly DateTime _restockDate = new DateTime(2025, 11, 14);
        private static readonly DateTime _expectedDate = new DateTime(2025, 11, 21);

        private const decimal _restockPrice = 1.5m;
        private const int _quantityForRestock = 3;

        public GetRestock_test()
        {
            var user = new ApplicationUser("1", _adminName, _adminSurname, _userName, _deliveryAddress);

            var brand = new Brand(_brandName);
            var typeItem = new TypeItem("Supplement");

            var item = new Item
            {
                Name = _itemName,
                Description = "High-protein bar for restocking",
                PurchasePrice = 2.50m,
                QuantityAvailableForPurchase = 0,
                QuantityForRestock = 0,
                RestockPrice = _restockPrice,
                TypeItem = typeItem,
                Brand = brand,
                PurchaseItems = new List<PurchaseItem>(),
                RestockItems = new List<RestockItem>()
            };

            var restock = new Restock
            {
                Description = $"Restock of {_itemName}",
                DeliveryAddress = _deliveryAddress,
                ExpectedDate = _expectedDate,
                RestockDate = _restockDate,
                Title = "Restock order 1",
                RestockItems = new List<RestockItem>(),
                ApplicationUser = user
            };

            var restockItem = new RestockItem
            {
                Item = item,
                Restock = restock,
                Quantity = _quantityForRestock,
                RestockPrice = _restockPrice
            };

            restock.RestockItems.Add(restockItem);
            item.RestockItems.Add(restockItem);

            restock.TotalPrice = restock.RestockItems.Sum(ri => (ri.RestockPrice ?? 0m) * ri.Quantity);

            _context.ApplicationUsers.Add(user);
            _context.Brands.Add(brand);
            _context.TypeItems.Add(typeItem);
            _context.Items.Add(item);
            _context.Restocks.Add(restock);
            _context.SaveChanges();
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetRestock_NotFound_test()
        {
            var mock = new Mock<ILogger<RestocksController>>();
            ILogger<RestocksController> logger = mock.Object;

            var controller = new RestocksController(_context, logger);

            var result = await controller.GetRestock(0);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetRestock_Found_test()
        {
            var mock = new Mock<ILogger<RestocksController>>();
            ILogger<RestocksController> logger = mock.Object;

            var controller = new RestocksController(_context, logger);

            var expectedRestock = new RestockDetailDTO(
                1,
                _restockDate,
                _deliveryAddress,
                "Restock order 1",
                $"Restock of {_itemName}",
                _expectedDate,
                new List<RestockItemDTO>(),
                _restockPrice * _quantityForRestock
            );

            expectedRestock.AdminName = _adminName;
            expectedRestock.AdminSurname = _adminSurname;

            expectedRestock.RestockItems.Add(
                new RestockItemDTO(
                    1,
                    _itemName,
                    _brandName,
                    0,
                    _restockPrice,
                    _quantityForRestock
                )
            );

            var result = await controller.GetRestock(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var restockDTOActual = Assert.IsType<RestockDetailDTO>(okResult.Value);

            Assert.Equal(expectedRestock, restockDTOActual);
        }
    }
}
