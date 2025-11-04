using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForSEII2526.API.Controllers;

namespace AppForSEII2526.UT.PlansController_test
{
    internal class GetPlan_test : AppForSEII25264SqliteUT
    {
        public GetPlan_test()
        {
            
            var user = new ApplicationUser("1", "Samuel", "García Picazo", "samuel@uclm.es", "Calle Real 10");
            var paymentMethod = new CreditCard("1234567890123456", new DateTime(2027, 12, 31));
            paymentMethod.User = user;

            var classType = new List<TypeItem>() { new TypeItem("Dumbell"), new TypeItem("Mat") };
            var trainingClass = new Class("Clase de Yoga", 25.0m, 15, new DateTime(2025, 10, 6), classType);

            var plan = new Plan("Plan Fitness", 6, new DateTime(2025, 10, 1), paymentMethod, new List<PlanItem>());

            
            plan.PlanItems.Add(new PlanItem(trainingClass.Id,35.0m, plan, "Mantener forma física"));

            
            plan.TotalPrice = plan.PlanItems.Sum(pi => pi.Class.Price);

            
            _context.Users.Add(user);
            _context.PaymentMethods.Add(paymentMethod);
            _context.Classes.Add(trainingClass);
            _context.Plans.Add(plan);
            _context.SaveChanges();
        }
    }
}
