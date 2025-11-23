using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.PurchaseDTOs;

namespace AppForSEII2526.UT.PurchasesController_test
{
    public class CreatePurchase_test : AppForSEII25264SqliteUT
    {
        // Shared data for all the scenarios in this test class
        private const string _customerFirstName = "Samuel";
        private const string _customerSurname = "Garcia";
        private const string _customerEmail = "samuel@uclm.es";
        private const string _deliveryStreet = "10 Real Street";
        private const string _deliveryCity = "Madrid";
        private const string _deliveryCountry = "Spain";
        private const string _brandName = "Elyte";
        private const string _itemName = "Protein Bar";
        private const decimal _itemUnitPrice = 2.50m;
        private const int _seedPaymentMethodId = 1;
        private const int _seedItemId = 1;

        public CreatePurchase_test()
        {
            // Seed a customer, a payment method and an item available for purchasing
            var user = new ApplicationUser
            {
                UserName = _customerEmail,
                Name = _customerFirstName,
                Surname = _customerSurname,
                Address = _deliveryStreet
            };

            var paymentMethod = new CreditCard(
                "1234567890123456",
                new DateTime(2027, 12, 31),
                _seedPaymentMethodId,
                user
            );

            var brand = new Brand(_brandName);
            var typeItem = new TypeItem("Supplement");

            var item = new Item
            {
                Name = _itemName,
                Description = "High-protein bar",
                PurchasePrice = _itemUnitPrice,
                QuantityAvailableForPurchase = 5,
                QuantityForRestock = 0,
                RestockPrice = null,
                TypeItem = typeItem,
                Brand = brand,
                PurchaseItems = new List<PurchaseItem>(),
                RestockItems = new List<RestockItem>()
            };

            _context.Users.Add(user);
            _context.PaymentMethods.Add(paymentMethod);
            _context.Brands.Add(brand);
            _context.TypeItems.Add(typeItem);
            _context.Items.Add(item);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_CreatePurchase()
        {
            // Recreate the data needed to build DTOs for the parameterized tests
            var today = DateTime.Today;

            // Use PaymentMethodDTO with the seeded payment method ID
            var validPaymentMethod = new PaymentMethodDTO(_seedPaymentMethodId);

            // Alternate flow 4: missing mandatory payment selection
            var purchaseWithoutPaymentMethod = new PurchaseForCreateDTO(
                _deliveryCity,
                _deliveryCountry,
                _deliveryStreet,
                "Purchase without payment method",
                today,
                _itemUnitPrice,
                null!,
                new List<PurchaseItemDTO>
                {
                    new PurchaseItemDTO(_seedItemId, _itemName, _brandName, 1, _itemUnitPrice)
                }
            );

            // Alternate flow 4: missing delivery address information
            var purchaseWithoutAddress = new PurchaseForCreateDTO(
                "",
                _deliveryCountry,
                "",
                "Purchase without delivery address",
                today,
                _itemUnitPrice * 2,
                validPaymentMethod,
                new List<PurchaseItemDTO>
                {
                    new PurchaseItemDTO(_seedItemId, _itemName, _brandName, 2, _itemUnitPrice)
                }
            );

            // Alternate flow 4: no items selected for the purchase
            var purchaseWithoutItems = new PurchaseForCreateDTO(
                _deliveryCity,
                _deliveryCountry,
                _deliveryStreet,
                "Purchase without items",
                today,
                0m,
                validPaymentMethod,
                new List<PurchaseItemDTO>()
            );

            // Alternate flow 5: quantity requested is greater than the available stock
            var purchaseWithExceedingQuantity = new PurchaseForCreateDTO(
                _deliveryCity,
                _deliveryCountry,
                _deliveryStreet,
                "Purchase exceeding stock",
                today,
                _itemUnitPrice * 10,
                validPaymentMethod,
                new List<PurchaseItemDTO>
                {
                    new PurchaseItemDTO(_seedItemId, _itemName, _brandName, 10, _itemUnitPrice)
                }
            );

            var allTests = new List<object[]>
            {
                new object[] { purchaseWithoutPaymentMethod, "Error! You must select a payment method" },
                new object[] { purchaseWithoutAddress, "Error! City, Country and Street are mandatory" },
                new object[] { purchaseWithoutItems, "Error! You must include at least one item to purchase" },
                new object[] { purchaseWithExceedingQuantity, "Error! Requested quantity for item 'Protein Bar' exceeds available stock" }
            };

            return allTests;
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [MemberData(nameof(TestCasesFor_CreatePurchase))]
        public async Task CreatePurchase_Error_test(PurchaseForCreateDTO purchaseDTO, string expectedError)
        {
            // Arrange
            var mock = new Mock<ILogger<PurchasesController>>();
            ILogger<PurchasesController> logger = mock.Object;
            var controller = new PurchasesController(_context, logger);

            // Act
            var result = await controller.CreatePurchase(purchaseDTO);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);

            var errorActual = problemDetails.Errors.First().Value[0];
            Assert.StartsWith(expectedError, errorActual);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreatePurchase_Success_test()
        {
            // Arrange
            var mock = new Mock<ILogger<PurchasesController>>();
            ILogger<PurchasesController> logger = mock.Object;
            var controller = new PurchasesController(_context, logger);

            // Seed an additional customer with another payment method
            var user = new ApplicationUser
            {
                UserName = "victor.lopez@uclm.es",
                Name = "Victor",
                Surname = "Lopez",
                Address = "21 Castilla Avenue"
            };

            var paymentMethod = new CreditCard(
                "9876543210987654",
                new DateTime(2028, 6, 30),
                3,
                user
            );

            _context.Users.Add(user);
            _context.PaymentMethods.Add(paymentMethod);
            _context.SaveChanges();

            // Build a valid request where all mandatory data is provided
            var purchaseItems = new List<PurchaseItemDTO>
            {
                new PurchaseItemDTO(_seedItemId, _itemName, _brandName, 2, _itemUnitPrice)
            };

            var purchaseRequest = new PurchaseForCreateDTO(
                _deliveryCity,
                _deliveryCountry,
                _deliveryStreet,
                "Monthly snack purchase",
                DateTime.Today,
                0m, // TotalPrice is calculated from items, so this value doesn't matter
                new PaymentMethodDTO(paymentMethod.Id),
                purchaseItems
            );

            // Expected result - TotalPrice is calculated as Sum(Price * AmountBought)
            // Note: Prices are updated from database in controller, but in this test they already match
            var expectedTotalPrice = _itemUnitPrice * 2;
            var expectedPurchaseItems = new List<PurchaseItemDTO>
            {
                new PurchaseItemDTO(_seedItemId, _itemName, _brandName, 2, _itemUnitPrice)
            };
            var expectedPurchase = new PurchaseForDetailDTO(
                1, // First purchase will have Id = 1
                expectedTotalPrice,
                _deliveryCity,
                _deliveryCountry,
                _deliveryStreet,
                "Monthly snack purchase",
                DateTime.Today,
                new PaymentMethodDTO(paymentMethod.Id),
                expectedPurchaseItems
            );

            // Act
            var result = await controller.CreatePurchase(purchaseRequest);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var purchaseCreated = Assert.IsType<PurchaseForDetailDTO>(createdResult.Value);

            Assert.Equal(expectedPurchase, purchaseCreated);
        }
    }
}

