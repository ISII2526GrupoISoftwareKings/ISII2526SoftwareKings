using DataType = System.ComponentModel.DataAnnotations.DataType;

namespace AppForSEII2526.API.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Class
    {
        public Class(int id, string name, decimal price, int capacity, DateTime date, List<TypeItem> typeItems, List<PlanItem> planItems)
        {
            Id = id;
            Name = name;
            Price = price;
            Capacity = capacity;
            Date = date;
            TypeItems = typeItems;
            PlanItems = planItems;
        }

        public int Id { get; set; }

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

        public List<TypeItem> TypeItems { get; set; }
        public List<PlanItem> PlanItems { get; set; }
    }
}
