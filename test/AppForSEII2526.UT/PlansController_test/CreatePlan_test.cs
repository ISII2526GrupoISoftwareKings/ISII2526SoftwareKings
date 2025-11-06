using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ClassDTOs;
using AppForSEII2526.API.DTOs.PlanDTOs;
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
        private const string _class1Name = "Yoga";
        private const string _class2Name = "Crossfit";

        public CreatePlan_test()
        {
            // 🔹 Crear un usuario
            var user = new ApplicationUser
            {
                UserName = _userName,
                Name = _nameUser,
                Surname = _surnameUser
            };
            _context.Users.Add(user);

            // 🔹 Crear método de pago asociado al usuario
            var paymentMethod = new CreditCard
            (
                "1234567890123456", 
                new DateTime(2027, 12, 31)
            );
            _context.PaymentMethods.Add(paymentMethod);

            // 🔹 Crear clases disponibles
            var classes = new List<Class>
            {
                new Class
                {
                    Id = 1,
                    Name = _class1Name,
                    Date = DateTime.Today.AddDays(3), // fecha futura válida
                    Capacity = 10,
                    Price = 25
                },
                new Class
                {
                    Id = 2,
                    Name = _class2Name,
                    Date = DateTime.Today.AddDays(5),
                    Capacity = 0, // sin capacidad -> se usará en tests de error
                    Price = 30
                }
            };

            _context.Classes.AddRange(classes);
            _context.SaveChanges();

            // 🔹 Crear un plan existente (opcional, si más adelante lo necesitas)
            var existingPlan = new Plan
            {
                Name = "Plan de prueba",
                Description = "Plan inicial cargado para pruebas",
                Weeks = 4,
                CreatedDate = DateTime.Today.AddDays(-1),
                HealthIssues = "Ninguno",
                PaymentMethod = paymentMethod,
                TotalPrice = 25,
                PlanItems = new List<PlanItem>
                {
                    new PlanItem { ClassId = 1, Goal = "Probar funcionalidad" }
                }
            };

            _context.Plans.Add(existingPlan);
            _context.SaveChanges();
        }


        public static IEnumerable<object[]> TestCasesFor_CreatePlan()
        {
            // Datos base
            var today = DateTime.Today;
            

            // Clases de ejemplo
            var futureClass = new ClassForPlanDTO
            {
                Id = 1,
                Name = "Yoga Class",
                Date = today.AddDays(3),
                Capacity = 10,
                Price = 20.0m
            };

            var pastClass = new ClassForPlanDTO
            {
                Id = 2,
                Name = "Pilates Class",
                Date = today.AddDays(-2),
                Capacity = 10,
                Price = 15.0m
            };

            // ITEMS
            var validPlanItems = new List<PlanItemDTO>
            {
                new PlanItemDTO { ClassId = 1, Goal = "Improve flexibility", Price = 20.0m }
            };

            var invalidPlanItems = new List<PlanItemDTO>
            {
                new PlanItemDTO { ClassId = 2, Goal = "Invalid class date", Price = 15.0m }
            };

            // 1️⃣ DTO sin clases
            var planWithoutItems = new PlanForCreateDTO
            {
                Name = "Basic Plan",
                Description = "No items test",
                Weeks = 4,
                CreatedDate = today,
                HealthIssues = "",
                PaymentMethod = validPaymentMethod,
                NameUser = "elena@uclm.es",
                SurnameUser = "Martinez",
                PlanItems = new List<PlanItemDTO>()
            };

            // 2️⃣ Clase con fecha inválida (antes de hoy)
            var planWithPastClass = new PlanForCreateDTO
            {
                Name = "Past Class Plan",
                Description = "Includes class with invalid date",
                Weeks = 4,
                CreatedDate = today,
                HealthIssues = "",
                PaymentMethod = validPaymentMethod,
                NameUser = "elena@uclm.es",
                SurnameUser = "Martinez",
                PlanItems = invalidPlanItems
            };

            // 3️⃣ Plan sin nombre
            var planWithoutName = new PlanForCreateDTO
            {
                Name = "",
                Description = "Missing name",
                Weeks = 4,
                CreatedDate = today,
                HealthIssues = "",
                PaymentMethod = validPaymentMethod,
                NameUser = "elena@uclm.es",
                SurnameUser = "Martinez",
                PlanItems = validPlanItems
            };

            // 4️⃣ Plan con semanas <= 0
            var planWithZeroWeeks = new PlanForCreateDTO
            {
                Name = "Zero Weeks Plan",
                Description = "Weeks is zero",
                Weeks = 0,
                CreatedDate = today,
                HealthIssues = "",
                PaymentMethod = validPaymentMethod,
                NameUser = "elena@uclm.es",
                SurnameUser = "Martinez",
                PlanItems = validPlanItems
            };

            // 5️⃣ Plan con método de pago inválido
            var planWithInvalidPayment = new PlanForCreateDTO
            {
                Name = "Invalid Payment Plan",
                Description = "Payment method invalid",
                Weeks = 4,
                CreatedDate = today,
                HealthIssues = "",
                PaymentMethod = invalidPaymentMethod,
                NameUser = "elena@uclm.es",
                SurnameUser = "Martinez",
                PlanItems = validPlanItems
            };

            // 6️⃣ Usuario no registrado
            var planWithUnregisteredUser = new PlanForCreateDTO
            {
                Name = "Unregistered User Plan",
                Description = "User not registered in DB",
                Weeks = 4,
                CreatedDate = today,
                HealthIssues = "",
                PaymentMethod = validPaymentMethod,
                NameUser = "victor.lopez@uclm.es", // no registrado
                SurnameUser = "Lopez",
                PlanItems = validPlanItems
            };

            // 7️⃣ Clase sin capacidad (capacity <= 0)
            var planWithFullClass = new PlanForCreateDTO
            {
                Name = "Full Class Plan",
                Description = "Class with no capacity",
                Weeks = 4,
                CreatedDate = today,
                HealthIssues = "",
                PaymentMethod = validPaymentMethod,
                NameUser = "elena@uclm.es",
                SurnameUser = "Martinez",
                PlanItems = new List<PlanItemDTO>
                {
                    new PlanItemDTO { ClassId = 3, Goal = "Too popular", Price = 30.0m }
                }
            };

            // Todos los casos de prueba con el mensaje esperado
            var allTests = new List<object[]>
            {
                new object[] { planWithoutItems, "You must select at least one class to create a plan." },
                new object[] { planWithPastClass, "One or more selected classes have invalid dates (before today)." },
                new object[] { planWithoutName, "Plan name is mandatory." },
                new object[] { planWithZeroWeeks, "Number of weeks must be greater than 0." },
                new object[] { planWithInvalidPayment, "A valid payment method must be provided." },
                new object[] { planWithUnregisteredUser, "Error! UserName is not registered" },
                new object[] { planWithFullClass, "Class 'Yoga Class' does not have enough capacity." }
            };

            return allTests;
        }

    }
}

