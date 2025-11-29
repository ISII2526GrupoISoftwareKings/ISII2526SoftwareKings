using DataType = System.ComponentModel.DataAnnotations.DataType;
namespace AppForSEII2526.API.DTOs.ItemDTOs
{
    public class ItemForPurchasingDTO
    {
        public ItemForPurchasingDTO(int id, string name, string brandName, string description, decimal price, int quantityAvailaibleForPurchase)
        {
            Id = id;
            Name = name;
            Brand = brandName;
            Description = description;
            Price = price;
            QuantityAvailableForPurchase = quantityAvailaibleForPurchase;
        }
        public int Id { get; set; }

        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Name { get; set; }
        public string Brand { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
        public string Description { get; set; }

        [DataType(DataType.Currency)]
        [Precision(10, 2)]
        public decimal Price { get; set; }

        [Display(Name = "Quantity Available For Purchase")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity available must be 0 or more")]
        public int QuantityAvailableForPurchase { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ItemForPurchasingDTO dTO &&
                   Id == dTO.Id &&
                   Name == dTO.Name &&
                   Brand == dTO.Brand &&
                   Description == dTO.Description &&
                   Price == dTO.Price &&
                   QuantityAvailableForPurchase == dTO.QuantityAvailableForPurchase;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, Brand, Description, Price, QuantityAvailableForPurchase);
        }
    }
}
