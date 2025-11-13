using DataType = System.ComponentModel.DataAnnotations.DataType;


namespace AppForSEII2526.API.DTOs.PlanDTOs

{
    public class PlanItemDTO
    {

        public PlanItemDTO() {
        }
    
        public PlanItemDTO(int planId, int classId, decimal price, int capacity, DateTime date, string goal)
        {
            PlanId = planId;
            ClassId = classId;
            Price = price;
            Goal = goal;
            Capacity = capacity;
        }

        public int PlanId { get; set; }
        public int ClassId { get; set; }

        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        [Precision(5, 2)]
        public decimal Price { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Minimum capacity is 1.")]
        public int Capacity { get; set; }

        [StringLength(100, ErrorMessage = "Goal can be neither longer than 100 characters nor shorter than 1", MinimumLength = 1)]
        public string Goal { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PlanItemDTO dTO &&
                   ClassId == dTO.ClassId &&
                   Name == dTO.Name &&
                   Price == dTO.Price &&
                   Capacity == dTO.Capacity &&
                   Goal == dTO.Goal;
        }
    }
}
