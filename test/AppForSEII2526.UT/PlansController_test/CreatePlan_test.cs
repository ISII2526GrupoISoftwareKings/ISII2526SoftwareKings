using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ClassDTOs;
using AppForSEII2526.API.DTOs.PlanDTOs;
using AppForSEII2526.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.PlansController_test
{
    public class CreatePlan_test : AppForSEII25264SqliteUT
    {
        private const string _nameUser = "Samuel";
        private const string _surnameUser = "García";
        private const string _userName = "samuel@uclm.es";
        private const string _paymentMethodName = "Tarjeta";
        private const string _class1Name = "Yoga Advanced";
        private const string _class2Name = "Crossfit";
        private const string _class3Name = "Pilates";

        public CreatePlan_test()
        {
            
            var user = new ApplicationUser
            {
                UserName = _userName,
                Name = _nameUser,
                Surname = _surnameUser,
                Address = "Calle Real 10"
            };
            _context.Users.Add(user);

            
            var paymentMethod = new CreditCard
            (
                "1234567890123456", 
                new DateTime(2027, 12, 31),
                1,
                user
            );
            _context.PaymentMethods.Add(paymentMethod);

           
            var classes = new List<Class>
            {
                new Class
                (
                    1,
                    _class1Name,
                    25,
                    10,
                    DateTime.Today.AddDays(3),
                    new List<TypeItem>{new TypeItem ("Yoga Mat" )}
                ),
                new Class
                (
                    2,
                    _class2Name,
                    30,
                    0,
                    DateTime.Today.AddDays(5),
                    new List<TypeItem>{new TypeItem ("Dumbbell" )}
                ),
                new Class
                (
                    3,
                    _class3Name,
                    20,
                    12,
                    DateTime.Today.AddDays(-2),
                    new List<TypeItem>{new TypeItem ("Mat" )}
                )
            };

            _context.Classes.AddRange(classes);
            _context.SaveChanges();

        }


        public static IEnumerable<object[]> TestCasesFor_CreatePlan()
        {
            var user = new ApplicationUser
            {
                UserName = _userName,
                Name = _nameUser,
                Surname = _surnameUser,
                Address = "Calle Real 10"
            };

            var today = DateTime.Today;
            var validPaymentMethod = new CreditCard
            (
                "1234567890123456",
                new DateTime(2027, 12, 31),
                2,
                user
            );
            CreditCard invalidPaymentMethod = null;

            var validPlanItems = new List<PlanItemDTO>
            {
                new PlanItemDTO (1, 1, 25.0m, 10, DateTime.Today.AddDays(3), "Improve flexibility") 
            };

            var fullClassItems = new List<PlanItemDTO>
            {
                new PlanItemDTO (2, 2, 30.0m, 0, DateTime.Today.AddDays(5), "Class full")

            };
            var pastClassItems = new List<PlanItemDTO>
            {
                new PlanItemDTO (3, 3, 15.0m, 10, DateTime.Today.AddDays(-2), "Past class")
            };

            // DTO sin clases
            var planWithoutItems = new PlanForCreateDTO(
                "No Items Plan",
                "samuel@uclm.es",
                "García",
                "Plan sin clases",
                4,
                today,
                "",
                validPaymentMethod,
                new List<PlanItemDTO>()
            );

            // Clase con fecha inválida (antes de hoy)
            var planWithPastClass = new PlanForCreateDTO(
                "Past Class Plan",
                "samuel@uclm.es",
                "García",
                "Incluye clase con fecha inválida",
                4,
                today,
                "",
                validPaymentMethod,
                pastClassItems
            );

            // Plan sin nombre
            var planWithoutName = new PlanForCreateDTO(
                "",
                "samuel@uclm.es",
                "García",
                "Falta el nombre del plan",
                4,
                today,
                "",
                validPaymentMethod,
                validPlanItems
            );

            // Plan con semanas <= 0
            var planWithZeroWeeks = new PlanForCreateDTO(
                "Zero Weeks Plan",
                "samuel@uclm.es",
                "García",
                "Semanas igual a 0",
                0,
                today,
                "",
                validPaymentMethod,
                validPlanItems
            );

            // Plan con método de pago inválido
            var planWithInvalidPayment = new PlanForCreateDTO(
                "Invalid Payment Plan",
                "samuel@uclm.es",
                "García",
                "Método de pago inválido",
                4,
                today,
                "",
                invalidPaymentMethod,
                validPlanItems
            );

            // Usuario no registrado
            var planWithUnregisteredUser = new PlanForCreateDTO(
                "Unregistered User Plan",
                "victor.lopez@uclm.es",
                "Lopez",
                "Usuario no registrado",
                4,
                today,
                "",
                validPaymentMethod,
                validPlanItems
            );

            // Clase sin capacidad
            var planWithFullClass = new PlanForCreateDTO(
                "Full Class Plan",
                "samuel@uclm.es",
                "García",
                "Clase sin capacidad",
                4,
                today,
                "",
                validPaymentMethod,
                fullClassItems
            );


            
            var allTests = new List<object[]>
            {
                new object[] { planWithoutItems, "You must select at least one class to create a plan." },
                new object[] { planWithPastClass, "One or more selected classes have invalid dates (before today)." },
                new object[] { planWithoutName, "Plan name is mandatory." },
                new object[] { planWithZeroWeeks, "Number of weeks must be greater than 0." },
                new object[] { planWithInvalidPayment, "A valid payment method must be provided." },
                new object[] { planWithUnregisteredUser, "Error! UserName is not registered" },
                new object[] { planWithFullClass, "Class 'Crossfit' does not have enough capacity." },
            };

            return allTests;
        }


        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [MemberData(nameof(TestCasesFor_CreatePlan))]
        public async Task CreatePlan_Error_Test(PlanForCreateDTO planDTO, string errorExpected)
        {
            // Arrange
            var mock = new Mock<ILogger<PlansController>>();
            ILogger<PlansController> logger = mock.Object;
            var controller = new PlansController(_context, logger);

            // Act
            var result = await controller.CreatePlan(planDTO);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);

            var errorActual = problemDetails.Errors.First().Value[0];
            Assert.StartsWith(errorExpected, errorActual);
        }


        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreatePlan_Success_Test()
        {
            // Arrange
            var mock = new Mock<ILogger<PlansController>>();
            ILogger<PlansController> logger = mock.Object;
            var controller = new PlansController(_context, logger);

            DateTime today = DateTime.Today;

            var user2 = new ApplicationUser
            {
                UserName = _userName,
                Name = _nameUser,
                Surname = _surnameUser,
                Address = "Calle Real 11"
            };

            var paymentMethod2 = new CreditCard
            (
                "1234567890123456",
                new DateTime(2027, 12, 31),
                3,
                user2
            );

            var yogaClass = new Class
            {
                Id = 4,
                Name = "Yoga Basics",
                Date = DateTime.Today.AddDays(3),
                Capacity = 10,
                Price = 25m,
                TypeItems = new List<TypeItem>
                {
                    new TypeItem ( "Basic Mat" )
                }
            };

            var validPlanItems = new List<PlanItemDTO>
            {
                new PlanItemDTO (1, 4, 25.0m, 10, DateTime.Today.AddDays(3), "Improve flexibility")
            };

            var expectedDto = new PlanForCreateDTO(
                "Plan Correcto",
                "samuel@uclm.es",
                "García",
                "Plan con clases válidas",
                4,
                today,
                "",
                paymentMethod2,
                validPlanItems
            );

            _context.Users.Add(user2);
            _context.PaymentMethods.Add(paymentMethod2);
            _context.Classes.Add(yogaClass);
            _context.SaveChanges();

            // Act
            var result = await controller.CreatePlan(expectedDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var planCreated = Assert.IsType<PlanForDetailDTO>(createdResult.Value);

            Assert.Equal(expectedDto, planCreated);
        }
    }
}

