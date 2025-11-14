using AppForSEII2526.API.DTOs.TypeItemDTOs;
using DataType = System.ComponentModel.DataAnnotations.DataType;
namespace AppForSEII2526.API.DTOs.ClassDTOs
{
    public class ClassForPlanDTO
    {
        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string Name { get; set; }
        [DataType(DataType.Currency)]
        [Precision(5, 2)]
        public decimal Price { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        public IList<TypeItemForClassDTO> TypeItems { get; set; }

        public ClassForPlanDTO(int id, string name, IList<TypeItemForClassDTO> typeItems, DateTime date, decimal price)
        {
            Id = id;
            Name = name;
            TypeItems = typeItems;
            Date = date;
            Price = price;
        }

        public override bool Equals(object? obj)
        {
            return obj is ClassForPlanDTO dTO &&
                   Id == dTO.Id &&
                   Name == dTO.Name &&
                   Price == dTO.Price &&
                   Date == dTO.Date;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, Price, Date);
        }
    }
}
