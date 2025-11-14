using DataType = System.ComponentModel.DataAnnotations.DataType;

namespace AppForSEII2526.API.DTOs.ItemDTOs
{
    public class ItemForRestockDTO
    {
        public ItemForRestockDTO(int id, string name, string brandName, decimal? restockPrice, int quantityForStock)
        {
            Id = id;
            Name = name;
            Brand = brandName;
            RestockPrice = restockPrice;
            QuantityForRestock = quantityForStock;
        }

        public int Id { get; set; }

        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Name { get; set; }

        public string Brand { get; set; }

        [DataType(DataType.Currency)]
        [Precision(10, 2)]
        public decimal? RestockPrice { get; set; }

        [Display(Name = "Quantity In Stock")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity in stock must be 0 or more")]
        public int QuantityForRestock { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ItemForRestockDTO dTO &&
                   Id == dTO.Id &&
                   Name == dTO.Name &&
                   Brand == dTO.Brand &&
                   RestockPrice == dTO.RestockPrice &&
                   QuantityForRestock == dTO.QuantityForRestock;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, Brand, RestockPrice, QuantityForRestock);
        }
    }
}
