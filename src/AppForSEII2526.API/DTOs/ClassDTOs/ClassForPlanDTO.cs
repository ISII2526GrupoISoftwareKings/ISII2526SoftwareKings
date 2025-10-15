namespace AppForSEII2526.API.DTOs.ClassDTOs
{
    public class ClassForPlanDTO
    {
        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }

        public IList<TypeItem> TypeItems { get; set; }

        public ClassForPlanDTO(int id, string name, IList<TypeItem> typeItems, DateTime date, decimal price)
        {
            Id = id;
            Name = name;
            TypeItems = typeItems;
            Date = Date;
            Price = price;
        }
    }
}
