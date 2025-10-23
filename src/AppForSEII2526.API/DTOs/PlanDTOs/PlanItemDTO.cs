using DataType = System.ComponentModel.DataAnnotations.DataType;


namespace AppForSEII2526.API.DTOs.PlanDTOs

{
    public class PlanItemDTO
    {
        public PlanItemDTO(int classId, string name, decimal price, int capacity, DateTime date, string goal)
        {
            ClassId = classId;
            Name = name;
            Price = price;
            Capacity = capacity;
            Date = date;
            Goal = goal;
        }

        public int ClassId { get; set; }

        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        [Precision(5, 2)]
        public decimal Price { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Minimum capacity is 1.")]
        public int Capacity { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [StringLength(100, ErrorMessage = "Goal can be neither longer than 100 characters nor shorter than 1", MinimumLength = 1)]
        public string Goal { get; set; }

        public List<TypeItem> TypeItems { get; set; }
    }
}
