using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ClassDTOs;
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
        public SelectClassesForPlan_test() {
            var classes = new List<Class>()
            {
                new Class(1, "Yoga Basics", 15.0m, 20, new DateTime(2025,10,12),
                    new List<TypeItem>{ new TypeItem{ Id=1, Name="Yoga Mat" } },
                    new List<PlanItem>()),
                new Class(2, "Advanced Pilates", 20.00m, 15, new DateTime(2025,10,15),
                    new List<TypeItem>{ new TypeItem { Id = 2, Name = "Pilates Block"} },
                    new List<PlanItem>())
            };
            _context.Classes.AddRange(classes);
            _context.SaveChanges();
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task SelectClassesForPlan_null_classname_date()
        {
            var expectedClasses = new List<ClassForPlanDTO>()
            {
                    new ClassForPlanDTO(1, "Yoga Basics",
                        new List<TypeItem>{ new TypeItem{ Id=1, Name="Yoga Mat" } },
                        new DateTime(2025,10,12),
                        15.00m),

                    new ClassForPlanDTO(2, "Advanced Pilates",
                        new List<TypeItem>{ new TypeItem { Id = 2, Name = "Pilates Block"} },
                        new DateTime(2025,10,15),
                        20.00m)
            };

            var mock = new Mock<ILogger<ClassesController>>();
            ILogger<ClassesController> logger = mock.Object;
            ClassesController controller = new ClassesController(_context, logger);

            var result = await controller.GetClassesForPlan(null, null);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var moviesactualresult = Assert.IsType<List<ClassForPlanDTO>>(okResult.Value);
            Assert.Equivalent(expectedClasses, moviesactualresult);
        }
    }
}
