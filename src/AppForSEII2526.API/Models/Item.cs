namespace AppForSEII2526.API.Models
{
    public class Item
    {
        public int Id { get; set; }

        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
        public string Description { get; set; }

        public decimal PurchasePrice { get; set; }

        public int QuantityAvailableForPurchase { get; set; }

        public int QuantityForRestock { get; set; }

        public decimal? RestockPrice { get; set; }
    }

}
