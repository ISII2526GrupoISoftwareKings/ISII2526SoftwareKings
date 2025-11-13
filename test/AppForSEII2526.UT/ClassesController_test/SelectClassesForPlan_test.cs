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
            new Class(1, "Yoga Basics", 10m, 10, new DateTime(2025, 10, 11), typeItems),
            new Class(2, "Pilates Advanced", 15m, 20, new DateTime(2025, 10, 13), typeItems),
            new Class(3, "CrossFit", 20m, 15, new DateTime(2025, 10, 18), typeItems),
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
                new ClassForPlanDTO(1, "Yoga Basics", typeItems, new DateTime(2025, 10, 11), 10m),
                new ClassForPlanDTO(2, "Pilates Advanced", typeItems, new DateTime(2025, 10, 13), 15m)
            };


            var expected2 = new List<ClassForPlanDTO>()
            {
                new ClassForPlanDTO(1, "Yoga Basics", typeItems, new DateTime(2025, 10, 11), 10m)
            };


            var expected3 = new List<ClassForPlanDTO>()
            {
                new ClassForPlanDTO(2, "Pilates Advanced", typeItems, new DateTime(2025, 10, 13), 15m),
                new ClassForPlanDTO(3, "CrossFit", typeItems, new DateTime(2025, 10, 18), 20m)
            };


            var expected4 = new List<ClassForPlanDTO>()
            {
                new ClassForPlanDTO(2, "Pilates Advanced", typeItems, new DateTime(2025, 10, 13), 15m)
            };

            var allTests = new List<object[]> {
                new object[] { null, null, expected1 },
                new object[] { "Yoga", null, expected2 },
                new object[] { null, new DateTime(2025, 10, 12), expected3 },
                new object[] { "Pilates", new DateTime(2025,10,13), expected4 },
            };

            return allTests;
        }


        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [MemberData(nameof(TestCasesFor_GetClassesForPlan_OK))]
        public async Task GetClassesForPlan_filter_test(string? className, DateTime? date, List<ClassForPlanDTO> expectedClasses)
        {
            // Arrange
            var controller = new ClassesController(_context, null);

            // Act
            var result = await controller.GetClassesForPlan(className, date);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var classesActual = Assert.IsType<List<ClassForPlanDTO>>(okResult.Value);
            Assert.Equal(expectedClasses, classesActual);
        }


    }
}
