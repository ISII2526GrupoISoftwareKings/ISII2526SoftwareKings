using DataType = System.ComponentModel.DataAnnotations.DataType;

namespace AppForSEII2526.API.Models
{
    public class PurchaseItem
    {
        public int Id { get; set; }

        [Display(Name = "Amount Bought")]
        [Range(1, int.MaxValue, ErrorMessage = "Minimum amount bought is 1")]
        public int AmountBought { get; set; }

        [DataType(DataType.Currency)]
        [Precision(10, 2)]
        public decimal Price { get; set; }

        public int ItemId { get; set; }
        public Item Item { get; set; }

        public int PurchaseId { get; set; }
        public Purchase Purchase { get; set; }
    }
}
