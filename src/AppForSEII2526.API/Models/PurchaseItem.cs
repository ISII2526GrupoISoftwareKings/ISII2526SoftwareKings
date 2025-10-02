namespace AppForSEII2526.API.Models
{
    public class PurchaseItem
    {
        public int Id { get; set; }

        public int AmountBought { get; set; }

        public decimal Price { get; set; }

        // Relación con Item
        public int ItemId { get; set; }
        public Item Item { get; set; }

        // Relación con Purchase
        public int PurchaseId { get; set; }
        public Purchase Purchase { get; set; }
    }
}
