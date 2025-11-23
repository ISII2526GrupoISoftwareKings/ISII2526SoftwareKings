using System.ComponentModel.DataAnnotations;
using DataType = System.ComponentModel.DataAnnotations.DataType;

namespace AppForSEII2526.API.DTOs.PurchaseDTOs
{
    public class PurchaseItemDTO
    {
        public PurchaseItemDTO()
        {
        }

        public PurchaseItemDTO(int itemId, string name, string brand, int amountBought, decimal price)
        {
            ItemId = itemId;
            Name = name;
            Brand = brand;
            AmountBought = amountBought;
            Price = price;
        }

        public int ItemId { get; set; }

        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string Name { get; set; }

        [StringLength(50, ErrorMessage = "Brand cannot be longer than 50 characters.")]
        public string Brand { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Minimum amount bought is 1")]
        public int AmountBought { get; set; }

        [DataType(DataType.Currency)]
        [Precision(10, 2)]
        public decimal Price { get; set; }
    }
}
