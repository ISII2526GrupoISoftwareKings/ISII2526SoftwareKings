using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ItemDTOs;
using AppForSEII2526.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AppForSEII2526.UT.ItemsController_test
{
    public class GetItemsForPurchase_test : AppForSEII25264SqliteUT
    {
        public GetItemsForPurchase_test()
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
                new Item() {Name = "Protein Bar", Brand = brands[0], TypeItem = typeItems[0], Description = "Delicious protein bar", PurchasePrice = 2.5m, QuantityAvailableForPurchase = 10 },
                new Item() { Name = "Yoga Mat", Brand = brands[1], TypeItem = typeItems[1], Description = "Comfortable yoga mat", PurchasePrice = 20.0m, QuantityAvailableForPurchase = 0 },
                new Item() { Name = "Dumbbells Set", Brand = brands[0], TypeItem = typeItems[1], Description = "Set of dumbbells for strength training", PurchasePrice = 50.0m, QuantityAvailableForPurchase = 5 }
            };
            _context.AddRange(brands);
            _context.AddRange(typeItems);
            _context.AddRange(items);
            _context.SaveChanges();


        }
        //[Fact]
        //[Trait("Level ", "Unit Testing")]
        //public async Task GetItemsForPurchasing_NULL_NAME_BRAND(string? itemName, string? itemBrand, List<ItemForPurchasingDTO> expectedItems) 
        //{
        //    //Arrange
        //    var expectedItems = new List<ItemForPurchasingDTO>()
        //    {
        //        new ItemForPurchasingDTO(1, "Protein Bar", "Elyte", "Delicious protein bar", 2.5m, 10),
        //        new ItemForPurchasingDTO(3, "Dumbbells Set", "Elyte", "Set of dumbbells for strength training", 50.0m, 5)
        //    };
        //    var controller = new ItemsController(_context, null);

        //    //Act
        //    var result = await controller.GetItemsForPurchasing(null, null);

        //    //Assert    
        //    //We check that the response type is Ok
        //    var okResult = Assert.IsType<OkObjectResult>(result);
        //    //and obtain the list of items
        //    var itemDTOsActual = Assert.IsType<List<ItemForPurchasingDTO>>(okResult.Value);
        //    Assert.Equal(expectedItems, itemDTOsActual);

        //}

        public static IEnumerable<object[]> TestCasesFor_GetItemsForPurchasing_OK()
        {

            var itemDTOs = new List<ItemForPurchasingDTO>()
            {
                new ItemForPurchasingDTO(1, "Protein Bar", "Elyte", "Delicious protein bar", 2.5m, 10),
                new ItemForPurchasingDTO(3, "Dumbbells Set", "Elyte", "Set of dumbbells for strength training", 50.0m, 5)
            };

            var itemDTOsTC1 = new List<ItemForPurchasingDTO>() { itemDTOs[1], itemDTOs[0] };
            // the GetItemsForPurchasing method returns the items ordered by Name


            var itemDTOsTC2 = new List<ItemForPurchasingDTO>() { itemDTOs[0] };
            var itemDTOsTC3 = new List<ItemForPurchasingDTO>() { itemDTOs[1], itemDTOs[0] };


            var allTests = new List<object[]>
            {
                            //filters to apply - expected items

                new object[] { null, null, itemDTOsTC1 },
                new object[] { "Protein", null, itemDTOsTC2 },
                new object[] { null, "Elyte", itemDTOsTC3 },


            };

            return allTests;
        }


        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [MemberData(nameof(TestCasesFor_GetItemsForPurchasing_OK))]
        public async Task GetMoviesForPurchase_filter_test(string? name, string? brand, List<ItemForPurchasingDTO> expectedItems)
        {
            // Arrange

            var controller = new ItemsController(_context, null);

            // Act
            var result = await controller.GetItemsForPurchasing(name, brand);

            //Assert
            //we check that the response type is OK 
            var okResult = Assert.IsType<OkObjectResult>(result);
            //and obtain the list of items
            var itemDTOsActual = Assert.IsType<List<ItemForPurchasingDTO>>(okResult.Value);
            Assert.Equal(expectedItems, itemDTOsActual);
        }
    }
}
