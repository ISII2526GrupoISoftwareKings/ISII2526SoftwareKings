using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ClassDTOs;
using AppForSEII2526.API.DTOs.PlanDTOs;
using AppForSEII2526.API.DTOs.TypeItemDTOs;
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
        private const string _surnameUser = "Garcia";
        private const string _userName = "samuel@uclm.es";
        private const string _paymentMethodName = "Card";
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
                Address = "10 Real Street"
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
                    new DateTime(2025, 2, 10),
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
            var validPaymentMethod = new PaymentmethodDTO
            (
                1,
                user.UserName
            );
            var invalidPaymentMethod = new PaymentmethodDTO(0, user.UserName);

            var validPlanItems = new List<PlanItemDTO>
            {
                new PlanItemDTO (1, _class1Name, new List<TypeItemForClassDTO>{new TypeItemForClassDTO(1, "Yoga Mat") }, 25.0m, 10, DateTime.Today.AddDays(3), "Improve flexibility") 
            };

            var fullClassItems = new List<PlanItemDTO>
            {
                new PlanItemDTO (2, _class2Name, new List<TypeItemForClassDTO>{new TypeItemForClassDTO(2, "Dumbbell") }, 30.0m, 0, DateTime.Today.AddDays(5), "Class full")

            };
            var pastClassItems = new List<PlanItemDTO>
            {
                new PlanItemDTO (3, _class3Name, new List<TypeItemForClassDTO>{new TypeItemForClassDTO(3, "Mat") }, 15.0m, 10, new DateTime(2025, 2, 10), "Past class")
            };

            // DTO without classes
            var planWithoutItems = new PlanForCreateDTO(
                "No Items Plan",
                "samuel@uclm.es",
                "Garcia",
                "Plan without classes",
                4,
                today,
                "",
                validPaymentMethod,
                new List<PlanItemDTO>()
            );

            // Class with invalid date (before today)
            var planWithPastClass = new PlanForCreateDTO(
                "Past Class Plan",
                "samuel@uclm.es",
                "Garcia",
                "Includes class with invalid date",
                4,
                today,
                "",
                validPaymentMethod,
                pastClassItems
            );

            // Plan without name
            var planWithoutName = new PlanForCreateDTO(
                "",
                "samuel@uclm.es",
                "Garcia",
                "Missing plan name",
                4,
                today,
                "",
                validPaymentMethod,
                validPlanItems
            );

            // Plan with weeks <= 0
            var planWithZeroWeeks = new PlanForCreateDTO(
                "Zero Weeks Plan",
                "samuel@uclm.es",
                "Garcia",
                "Weeks equal to 0",
                0,
                today,
                "",
                validPaymentMethod,
                validPlanItems
            );

            // Plan with invalid payment method
            var planWithInvalidPayment = new PlanForCreateDTO(
                "Invalid Payment Plan",
                "samuel@uclm.es",
                "Garcia",
                "Invalid payment method",
                4,
                today,
                "",
                invalidPaymentMethod,
                validPlanItems
            );

            // Unregistered user
            var planWithUnregisteredUser = new PlanForCreateDTO(
                "Unregistered User Plan",
                "victor.lopez@uclm.es",
                "Lopez",
                "Unregistered user",
                4,
                today,
                "",
                validPaymentMethod,
                validPlanItems
            );

            // Class without capacity
            var planWithFullClass = new PlanForCreateDTO(
                "Full Class Plan",
                "samuel@uclm.es",
                "Garcia",
                "Class without capacity",
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
                Address = "11 Real Street"
            };

            var paymentMethod2 = new PaymentmethodDTO
            (
                2,
                user2.UserName
            );
            
            var paymentMethodEntity = new CreditCard
            (
                "6543210987654321",
                new DateTime(2028, 11, 30),
                2,
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
                new PlanItemDTO (4, "Yoga Basics", new List<TypeItemForClassDTO>{new TypeItemForClassDTO(4, "Basic Mat") }, 25.0m, 10, DateTime.Today.AddDays(3), "Improve flexibility")
            };

            var planDto = new PlanForCreateDTO(
                "Valid Plan",
                "samuel@uclm.es",
                "Garcia",
                "Plan with valid classes",
                4,
                today,
                "",
                paymentMethod2,
                validPlanItems
            );

            var expectedDto = new PlanForDetailDTO
            (
                1,
                25.0m,
                today,
                "Valid Plan",
                _userName,
                _surnameUser,
                "Plan with valid classes",
                4,
                today,
                "",
                new PaymentmethodDTO(2, _nameUser),
                new List<PlanItemDTO>
                {
                    new PlanItemDTO (4, "Yoga Basics", new List<TypeItemForClassDTO>{new TypeItemForClassDTO(4, "Basic Mat") }, 25.0m, 10, DateTime.Today.AddDays(3), "Improve flexibility")
                }
            );

            _context.Users.Add(user2);
            _context.PaymentMethods.Add(paymentMethodEntity);
            _context.Classes.Add(yogaClass);
            _context.SaveChanges();

            // Act
            var result = await controller.CreatePlan(planDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var planCreated = Assert.IsType<PlanForDetailDTO>(createdResult.Value);

            Assert.Equal(expectedDto, planCreated);
        }
    }
}

