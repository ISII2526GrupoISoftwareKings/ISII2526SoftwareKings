using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.PurchaseDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.PurchasesController_test
{
    public class GetPurchase_test : AppForSEII25264SqliteUT
    {
        public GetPurchase_test()
        {
            var user = new ApplicationUser("1", "Samuel", "García Picazo", "samuel@uclm.es", "Calle Real 10");
            var paymentMethod = new CreditCard("1234567890123456", new DateTime(2027, 12, 31));
            paymentMethod.User = user;

            var brand = new Brand("Elyte");
            var typeItem = new TypeItem("Supplement");

            var item = new Item()
            {
                Name = "Protein Bar",
                Description = "Delicious protein bar",
                PurchasePrice = 2.50m,
                QuantityAvailableForPurchase = 20,
                QuantityForRestock = 0,
                RestockPrice = null,
                TypeItem = typeItem,
                Brand = brand,
                PurchaseItems = new List<PurchaseItem>(),
                RestockItems = new List<RestockItem>()
            };

            var purchase = new Purchase(
                city: "Madrid",
                country: "España",
                date: new DateTime(2025, 10, 2),
                street: "Calle Real 10",
                totalPrice: 5.00m,
                paymentMethod: paymentMethod,
                description: "Compra de snacks"
            );

            _context.Users.Add(user);
            _context.PaymentMethods.Add(paymentMethod);
            _context.Brands.Add(brand);
            _context.TypeItems.Add(typeItem);
            _context.Items.Add(item);
            _context.SaveChanges();

            purchase.PurchaseItems.Add(new PurchaseItem
            {
                ItemId = item.Id,
                Item = item,
                Purchase = purchase,
                AmountBought = 2,
                Price = 2.50m
            });

            purchase.TotalPrice = purchase.PurchaseItems.Sum(pi => pi.Price * pi.AmountBought);

            _context.Purchases.Add(purchase);
            _context.SaveChanges();
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetPurchase_NotFound_test()
        {
            // Arrange
            var mock = new Mock<ILogger<PurchasesController>>();
            ILogger<PurchasesController> logger = mock.Object;
            var controller = new PurchasesController(_context, logger);

            // Act
            var result = await controller.GetPurchase(0);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetPurchase_Found_test()
        {
            // Arrange
            var mock = new Mock<ILogger<PurchasesController>>();
            ILogger<PurchasesController> logger = mock.Object;
            var controller = new PurchasesController(_context, logger);

            var expected = new PurchaseForDetailDTO(
                id: 1,
                totalPrice: 5.00m,
                city: "Madrid",
                country: "España",
                street: "Calle Real 10",
                description: "Compra de snacks",
                date: new DateTime(2025, 10, 2),
                paymentMethod: new CreditCard("1234567890123456", new DateTime(2027, 12, 31)),
                purchaseItems: new List<PurchaseItemDTO>()
            );

            expected.PurchaseItems.Add(new PurchaseItemDTO(
                itemId: 1,
                name: "Protein Bar",
                brand: "Elyte",
                amountBought: 2,
                price: 2.50m
            ));

            // Act
            var result = await controller.GetPurchase(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var purchaseDtoActual = Assert.IsType<PurchaseForDetailDTO>(okResult.Value);
            Assert.Equal(expected, purchaseDtoActual);
        }
    }
}

