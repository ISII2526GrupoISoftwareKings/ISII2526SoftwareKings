using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ItemDTOs;
using AppForSEII2526.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.ItemsController_test
{
    public class GetItemsForRestock_test : AppForSEII25264SqliteUT
    {
        public GetItemsForRestock_test()
        {
            var brands = new List<Brand>()
            {
                new Brand("Elyte"),
                new Brand("Domyos"),
            };

            var typeItems = new List<TypeItem>()
            {
                new TypeItem { Name = "Supplement" },
                new TypeItem { Name = "Equipment" }
            };

            var items = new List<Item>()
            {
                new Item()
                {
                    Name = "Protein Bar",
                    Brand = brands[0],
                    TypeItem = typeItems[0],
                    Description = "Delicious protein bar",
                    PurchasePrice = 2.5m,
                    QuantityAvailableForPurchase = 5,
                    QuantityForRestock = 10,
                    RestockPrice = 1.5m
                },
                new Item()
                {
                    Name = "Yoga Mat",
                    Brand = brands[1],
                    TypeItem = typeItems[1],
                    Description = "Comfortable yoga mat",
                    PurchasePrice = 20.0m,
                    QuantityAvailableForPurchase = 10,
                    QuantityForRestock = 5,
                    RestockPrice = 15.0m
                },
                new Item()
                {
                    Name = "Dumbbells Set",
                    Brand = brands[0],
                    TypeItem = typeItems[1],
                    Description = "Set of dumbbells for strength training",
                    PurchasePrice = 50.0m,
                    QuantityAvailableForPurchase = 2,
                    QuantityForRestock = 3,
                    RestockPrice = 40.0m
                }
            };

            _context.AddRange(brands);
            _context.AddRange(typeItems);
            _context.AddRange(items);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_GetItemsForRestock_OK()
        {
            var itemDTOs = new List<ItemForRestockDTO>()
            {
                new ItemForRestockDTO(1, "Protein Bar",   "Elyte",  1.5m, 10),
                new ItemForRestockDTO(3, "Dumbbells Set", "Elyte", 40.0m, 3)
            };

            var itemDTOsTC1 = new List<ItemForRestockDTO>() { itemDTOs[1], itemDTOs[0] };

            var itemDTOsTC2 = new List<ItemForRestockDTO>() { itemDTOs[0] };

            var itemDTOsTC3 = new List<ItemForRestockDTO>() { itemDTOs[1] };

            var allTests = new List<object[]>
            {
                new object[] { null,    null, itemDTOsTC1 },
                new object[] { "Protein", null, itemDTOsTC2 },
                new object[] { null,    3,    itemDTOsTC3 },
            };

            return allTests;
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [MemberData(nameof(TestCasesFor_GetItemsForRestock_OK))]
        public async Task GetItemsForRestock_filter_test(string? name, int? quantityAvailableForPurchase, List<ItemForRestockDTO> expectedItems)
        {
            var controller = new ItemsController(_context, null);

            var result = await controller.GetItemsForRestock(name, quantityAvailableForPurchase);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var itemDTOsActual = Assert.IsType<List<ItemForRestockDTO>>(okResult.Value);

            Assert.Equal(expectedItems, itemDTOsActual);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetItemsForRestock_badrequest_test()
        {
            var mock = new Mock<ILogger<ItemsController>>();
            ILogger<ItemsController> logger = mock.Object;
            var controller = new ItemsController(_context, logger);

            var result = await controller.GetItemsForRestock(null, 0);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var message = Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal("There are no items available for restock.", message);
        }
    }
}
