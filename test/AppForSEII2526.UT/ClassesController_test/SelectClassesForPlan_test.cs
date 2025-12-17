using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ClassDTOs;
using AppForSEII2526.API.DTOs.TypeItemDTOs;
using AppForSEII2526.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AppForSEII2526.UT.ClassesController_test
{
    public class SelectClassesForPlan_test : AppForSEII25264SqliteUT
    {
        public SelectClassesForPlan_test()
        {

            var typeItems = new List<TypeItem>() { new TypeItem("Dumbbell"), new TypeItem("Mat") };

            var classes = new List<Class>() {
            new Class(1, "Yoga Basics", 10m, 10, DateTime.Today.AddDays(1), typeItems),
            new Class(2, "Pilates Advanced", 15m, 20, DateTime.Today.AddDays(3), typeItems),
            new Class(3, "CrossFit", 20m, 15, DateTime.Today.AddDays(8), typeItems),
        };
            _context.AddRange(typeItems);
            _context.AddRange(classes);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_GetClassesForPlan_OK()
        {
            var typeItems = new List<TypeItemForClassDTO>() { new TypeItemForClassDTO(1, "Dumbbell") };

            var expected1 = new List<ClassForPlanDTO>()
            {
                new ClassForPlanDTO(1, "Yoga Basics", typeItems, DateTime.Today.AddDays(1), 10m, 10),
                new ClassForPlanDTO(2, "Pilates Advanced", typeItems, DateTime.Today.AddDays(3), 15m, 20)
            };


            var expected2 = new List<ClassForPlanDTO>()
            {
                new ClassForPlanDTO(1, "Yoga Basics", typeItems, DateTime.Today.AddDays(1), 10m, 10)
            };


            var expected3 = new List<ClassForPlanDTO>()
            {
                new ClassForPlanDTO(2, "Pilates Advanced", typeItems, DateTime.Today.AddDays(3), 15m, 20),
                new ClassForPlanDTO(3, "CrossFit", typeItems, DateTime.Today.AddDays(8), 20m, 15)
            };


            var expected4 = new List<ClassForPlanDTO>()
            {
                new ClassForPlanDTO(2, "Pilates Advanced", typeItems, DateTime.Today.AddDays(3), 15m, 20)
            };

            var allTests = new List<object[]> {
                new object[] { null, null, 30, expected1 },
                new object[] { "Yoga", null, 30, expected2 },
                new object[] { null, DateTime.Today.AddDays(2), 30, expected3 },
                new object[] { "Pilates", DateTime.Today.AddDays(3), 30, expected4 },
                new object[] { null, null, 15, expected1 },
            };

            return allTests;
        }


        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [MemberData(nameof(TestCasesFor_GetClassesForPlan_OK))]
        public async Task GetClassesForPlan_filter_test(string? className, DateTime? date, decimal maximumprice, List<ClassForPlanDTO> expectedClasses)
        {
            // Arrange
            var mock = new Mock<ILogger<ClassesController>>();
            ILogger<ClassesController> logger = mock.Object;
            var controller = new ClassesController(_context, logger);

            // Act
            var result = await controller.GetClassesForPlan(className, date, maximumprice);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var classesActual = Assert.IsType<List<ClassForPlanDTO>>(okResult.Value);
            Assert.Equal(expectedClasses, classesActual);
        }


    }
}
