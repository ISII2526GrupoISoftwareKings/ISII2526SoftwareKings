using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.RestockDTOs;

namespace AppForSEII2526.UT.RestocksController_test
{
    public class CreateRestock_test : AppForSEII25264SqliteUT
    {
        private const string _userName = "alberto.bueno@uclm.es";
        private const string _userFirstName = "Alberto";
        private const string _userSurname = "Bueno";
        private const string _deliveryAddress = "Avda. España s/n, Albacete 02071";
        private const string _brandName = "Elyte";
        private const string _itemName = "Protein Bar";
        private const decimal _itemRestockPrice = 1.50m;
        private const int _seedItemId = 1;

        public CreateRestock_test()
        {
            var user = new ApplicationUser
            {
                UserName = _userName,
                Name = _userFirstName,
                Surname = _userSurname,
                Address = _deliveryAddress
            };

            var brand = new Brand(_brandName);
            var typeItem = new TypeItem { Name = "Supplement" };

            var item = new Item
            {
                Name = _itemName,
                Description = "High-protein bar for restocking",
                PurchasePrice = 2.50m,
                QuantityAvailableForPurchase = 0,
                QuantityForRestock = 0,
                RestockPrice = _itemRestockPrice,
                TypeItem = typeItem,
                Brand = brand,
                PurchaseItems = new List<PurchaseItem>(),
                RestockItems = new List<RestockItem>()
            };

            _context.ApplicationUsers.Add(user);
            _context.Brands.Add(brand);
            _context.TypeItems.Add(typeItem);
            _context.Items.Add(item);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_CreateRestock()
        {
            var today = DateTime.Today;
            var futureDate = today.AddDays(7);

            RestockItemDTO CreateValidRestockItem(int quantity) =>
                new RestockItemDTO(_seedItemId, _itemName, _brandName, 0, null, quantity);

            // AF1: no hay ningún item en el restock
            var restockWithoutItems = new RestockForCreateDTO
            {
                ApplicationUserName = _userName,
                DeliveryAddress = _deliveryAddress,
                Title = "Restock without items",
                Description = "No items selected",
                ExpectedDate = futureDate,
                RestockDate = today,
                RestockItems = new List<RestockItemDTO>()
            };

            // AF2: falta el título
            var restockWithoutTitle = new RestockForCreateDTO
            {
                ApplicationUserName = _userName,
                DeliveryAddress = _deliveryAddress,
                Title = "",
                Description = "Missing title",
                ExpectedDate = futureDate,
                RestockDate = today,
                RestockItems = new List<RestockItemDTO>
                {
                    CreateValidRestockItem(3)
                }
            };

            // AF3: fecha esperada en el pasado
            var restockWithPastExpectedDate = new RestockForCreateDTO
            {
                ApplicationUserName = _userName,
                DeliveryAddress = _deliveryAddress,
                Title = "Restock with past date",
                Description = "Expected date in the past",
                ExpectedDate = today.AddDays(-1),
                RestockDate = today,
                RestockItems = new List<RestockItemDTO>
                {
                    CreateValidRestockItem(3)
                }
            };

            // AF4: nombre de usuario no registrado
            var restockWithUnregisteredUser = new RestockForCreateDTO
            {
                ApplicationUserName = "unknown.user@uclm.es",
                DeliveryAddress = _deliveryAddress,
                Title = "Restock with unregistered user",
                Description = "User not in DB",
                ExpectedDate = futureDate,
                RestockDate = today,
                RestockItems = new List<RestockItemDTO>
                {
                    CreateValidRestockItem(3)
                }
            };

            // AF5: itemId > 0 pero el item no existe en la BBDD
            var invalidItemId = _seedItemId + 1;
            var restockWithInvalidItem = new RestockForCreateDTO
            {
                ApplicationUserName = _userName,
                DeliveryAddress = _deliveryAddress,
                Title = "Restock with invalid item",
                Description = "Item not found in DB",
                ExpectedDate = futureDate,
                RestockDate = today,
                RestockItems = new List<RestockItemDTO>
                {
                    new RestockItemDTO(invalidItemId, "Non existing item", _brandName, 0, null, 3)
                }
            };

            // AF6: cantidad a reponer <= 0
            var restockWithNonPositiveQuantity = new RestockForCreateDTO
            {
                ApplicationUserName = _userName,
                DeliveryAddress = _deliveryAddress,
                Title = "Restock with invalid quantity",
                Description = "Quantity <= 0",
                ExpectedDate = futureDate,
                RestockDate = today,
                RestockItems = new List<RestockItemDTO>
                {
                    CreateValidRestockItem(0)
                }
            };

            // AF7: itemId <= 0
            var restockWithInvalidItemId = new RestockForCreateDTO
            {
                ApplicationUserName = _userName,
                DeliveryAddress = _deliveryAddress,
                Title = "Restock with invalid item id",
                Description = "ItemId <= 0",
                ExpectedDate = futureDate,
                RestockDate = today,
                RestockItems = new List<RestockItemDTO>
                {
                    new RestockItemDTO(0, _itemName, _brandName, 0, null, 3)
                }
            };

            var allTests = new List<object[]>
            {
                new object[] { restockWithoutItems,        "Error! You must include at least one item to restock" },
                new object[] { restockWithoutTitle,        "Error! Title is mandatory" },
                new object[] { restockWithPastExpectedDate,"Error! Expected date cannot be in the past" },
                new object[] { restockWithUnregisteredUser,"Error! UserName is not registered" },
                new object[] { restockWithInvalidItem,     "Error! Item 'Non existing item (Elyte)' is not valid for restocking" },
                new object[] { restockWithNonPositiveQuantity, "Error! Quantity to buy must be greater than zero" },
                new object[] { restockWithInvalidItemId,   "Error! Invalid item identifier" }
            };

            return allTests;
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [MemberData(nameof(TestCasesFor_CreateRestock))]
        public async Task CreateRestock_BadRequest_test(RestockForCreateDTO restockDTO, string expectedError)
        {
            // Arrange
            var mock = new Mock<ILogger<RestocksController>>();
            ILogger<RestocksController> logger = mock.Object;
            var controller = new RestocksController(_context, logger);

            // Act
            var result = await controller.CreateRestock(restockDTO);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);

            var errorActual = problemDetails.Errors.First().Value[0];
            Assert.StartsWith(expectedError, errorActual);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreateRestock_Success_test()
        {
            var mock = new Mock<ILogger<RestocksController>>();
            ILogger<RestocksController> logger = mock.Object;
            var controller = new RestocksController(_context, logger);

            var today = DateTime.Today;
            var futureDate = today.AddDays(7);

            var restockRequest = new RestockForCreateDTO
            {
                ApplicationUserName = _userName,
                DeliveryAddress = _deliveryAddress,
                Title = "Valid restock",
                Description = $"Restock of {_itemName}",
                ExpectedDate = futureDate,
                RestockDate = today,
                RestockItems = new List<RestockItemDTO>
                {
                    new RestockItemDTO(_seedItemId, _itemName, _brandName, 0, null, 3)
                }
            };

            var result = await controller.CreateRestock(restockRequest);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetRestock", createdResult.ActionName);

            var restockCreated = Assert.IsType<RestockDetailDTO>(createdResult.Value);

            Assert.Equal(DateTime.Today, restockCreated.RestockDate.Date);

            var expectedRestockItems = new List<RestockItemDTO>
            {
                new RestockItemDTO(
                    restockRequest.RestockItems[0].ItemId,
                    restockRequest.RestockItems[0].Name,
                    restockRequest.RestockItems[0].Brand,
                    restockCreated.RestockItems[0].QuantityInRestock,
                    restockCreated.RestockItems[0].PriceOfRestock,
                    restockRequest.RestockItems[0].QuantityForRestock
                )
            };

            var expectedRestock = new RestockDetailDTO(
                restockCreated.Id,
                restockCreated.RestockDate,
                restockRequest.DeliveryAddress,
                restockRequest.Title,
                restockRequest.Description,
                restockRequest.ExpectedDate,
                expectedRestockItems,
                restockCreated.TotalPrice
            );

            expectedRestock.ApplicationUserName = restockRequest.ApplicationUserName;
            expectedRestock.AdminName = null;
            expectedRestock.AdminSurname = null;
            Assert.Equal(expectedRestock.RestockItems, restockCreated.RestockItems);

            //Assert.Equal(expectedRestock, restockCreated);
        }

    }
}
