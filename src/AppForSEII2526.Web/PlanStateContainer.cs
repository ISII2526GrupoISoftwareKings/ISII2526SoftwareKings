using AppForSEII2526.Web.API;

namespace AppForSEII2526.Web
{
    public class PlanStateContainer
    {

        //we create an instance of Plan when an instance of PlanStateContainer is created
        public PlanForCreateDTO Plan { get; private set; } = new PlanForCreateDTO()
        {
            PlanItems = new List<PlanItemDTO>()
        };

        //we compute the TotalPrice of the classes we have selected for the plan
        public decimal TotalPrice
        {
            get
            {
                return Convert.ToDecimal(Plan.PlanItems.Sum(i => i.Price));
            }
        }

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();



        public void AddClassToPlan(ClassForPlanDTO classdto)
        {
            //before adding a class we checked whether it has been already added
            if (!Plan.PlanItems.Any(pi => pi.ClassId == classdto.Id))
                //we add it if it is not in the list
                Plan.PlanItems.Add(new PlanItemDTO()
                {
                    ClassId = classdto.Id,
                    Price = classdto.Price,
                }
            );

        }

        //to delete classes from the list of selected classes
        public void RemovePlanItemToPlan( PlanItemDTO item)
        {
            Plan.PlanItems.Remove(item);

        }

        //we eliminate all the classes from the list
        public void ClearPlanCart()
        {
            Plan.PlanItems.Clear();

        }

        //we have already finished the process of planning, thus, we create a new Plan 
        public void PlanProcessed()
        {
            //we have finished the planning process so we create a new object without data
            Plan = new PlanForCreateDTO()
            {
                PlanItems = new List<PlanItemDTO>()
            };
        }
    }
}
