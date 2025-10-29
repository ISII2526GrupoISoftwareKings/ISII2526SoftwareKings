using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ClassDTOs;

namespace AppForSEII2526.UT.ClassesController_test
{
    public class SelectClassesForPlan_test : AppForSEII25264SqliteUT
    {
        public SelectClassesForPlan_test() {
            var classes = new List<Class>()
            {
                new Class(1, "Yoga Basics", 15.00m, 20, new DateTime(2025,10,12),
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
        public async Task SelectClassesForPlan_null_title_genre()
        {
            IList<ClassForPlanDTO> classesDTO = new List<ClassForPlanDTO>()
            {

            };
        }
    }
}
