using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.PlanDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.PlansController_test
{
    public class GetPlan_test : AppForSEII25264SqliteUT
    {
        public GetPlan_test()
        {

            var user = new ApplicationUser("1", "Samuel", "García Picazo", "samuel@uclm.es", "Calle Real 10");
            var paymentMethod = new CreditCard("1234567890123456", new DateTime(2027, 12, 31),1 , user);
            paymentMethod.User = user;

            var classType = new List<TypeItem>() { new TypeItem("Dumbell"), new TypeItem("Mat") };
            var trainingClass = new Class(1, "Clase de Yoga", 25.0m, 15, new DateTime(2025, 10, 6), classType);

            var plan = new Plan("Plan Fitness", 6, new DateTime(2025, 10, 1), paymentMethod, new List<PlanItem>(), "Rutina semanal", "Ninguno");

            _context.Classes.Add(trainingClass);
            _context.SaveChanges();

            plan.PlanItems.Add(new PlanItem(trainingClass.Id, 35.0m, plan, "Mantener forma física"));


            plan.TotalPrice = plan.PlanItems.Sum(pi => pi.Price);


            _context.Users.Add(user);
            _context.PaymentMethods.Add(paymentMethod);
            _context.Plans.Add(plan);
            _context.SaveChanges();
        }

            [Fact]
            [Trait("Database", "WithoutFixture")]
            [Trait("LevelTesting", "Unit Testing")]
            public async Task GetPlan_NotFound_test()
            {
                // Arrange
                var mock = new Mock<ILogger<PlansController>>();
                ILogger<PlansController> logger = mock.Object;

                var controller = new PlansController(_context, logger);

                // Act
                var result = await controller.GetPlan(0);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }

            [Fact]
            [Trait("Database", "WithoutFixture")]
            [Trait("LevelTesting", "Unit Testing")]
            public async Task GetPlan_Found_test()
            {
                // Arrange
                var mock = new Mock<ILogger<PlansController>>();
                ILogger<PlansController> logger = mock.Object;

                var controller = new PlansController(_context, logger);

                var user = new ApplicationUser("1", "Samuel", "García Picazo", "samuel@uclm.es", "Calle Real 10");

            // Expected DTO
            var expectedPlan = new PlanForDetailDTO(
                    1,
                    35.0m,
                    new DateTime(2025, 10, 1),
                    "Plan Fitness",
                    "Samuel",
                    "García Picazo",
                    "Rutina semanal",
                    6,
                    new DateTime(2025, 10, 1),
                    "Ninguno",
                    new CreditCard("1234567890123456", new DateTime(2027, 12, 31), 1, user),
                    new List<PlanItemDTO>()
                );

            var classType = new List<TypeItem>() { new TypeItem("Dumbell"), new TypeItem("Mat") };
            var trainingClass = new Class(1, "Clase de Yoga", 25.0m, 15, new DateTime(2025, 10, 6), classType);

            expectedPlan.PlanItems.Add(new PlanItemDTO(
                    1, trainingClass.Id, 25.0m, 15, new DateTime(2025, 10, 6), "Mantener forma física"
                ));

                // Act
                var result = await controller.GetPlan(1);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var planDTOActual = Assert.IsType<PlanForDetailDTO>(okResult.Value);

                Assert.Equal(expectedPlan, planDTOActual);
            }
    }
}
