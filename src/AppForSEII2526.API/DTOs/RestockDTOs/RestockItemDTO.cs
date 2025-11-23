using DataType = System.ComponentModel.DataAnnotations.DataType;

namespace AppForSEII2526.API.DTOs.RestockDTOs
{
    public class RestockItemDTO
    {
        public RestockItemDTO(int itemId, string name, string brand, int quantityInRestock, decimal? priceOfRestock, int quantityForRestock)
        {
            ItemId = itemId;
            Name = name;
            Brand = brand;
            QuantityInRestock = quantityInRestock;
            PriceOfRestock = priceOfRestock;
            QuantityForRestock = quantityForRestock;
        }

        public int ItemId { get; set; }

        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string Name { get; set; }

        [StringLength(50, ErrorMessage = "Brand cannot be longer than 50 characters.")]
        public string Brand { get; set; }

        [Display(Name = "Quantity In Stock")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity in stock must be 0 or more")]
        public int QuantityInRestock { get; set; }

        [DataType(DataType.Currency)]
        [Precision(10, 2)]
        public decimal? PriceOfRestock { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Minimum amount bought is 1")]
        public int QuantityForRestock { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is RestockItemDTO dTO &&
                   ItemId == dTO.ItemId &&
                   Name == dTO.Name &&
                   Brand == dTO.Brand &&
                   QuantityInRestock == dTO.QuantityInRestock &&
                   PriceOfRestock == dTO.PriceOfRestock &&
                   QuantityForRestock == dTO.QuantityForRestock;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ItemId, Name, Brand, QuantityInRestock, PriceOfRestock, QuantityForRestock);
        }
    }
}
