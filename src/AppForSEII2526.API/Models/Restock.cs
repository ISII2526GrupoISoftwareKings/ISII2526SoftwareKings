namespace AppForSEII2526.API.Models
{
    public class Restock
    {
        public Restock()
        {
        }

        public Restock(string description, string deliveryAddress, DateTime? expectedDate, DateTime restockDate, string title, IList<RestockItem> restockItems)
        {
            Description = description;
            DeliveryAddress = deliveryAddress;
            ExpectedDate = expectedDate;
            RestockDate = restockDate;
            Title = title;
            RestockItems = restockItems;
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public string DeliveryAddress { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public DateTime RestockDate { get; set; }
        public string Title { get; set; }
        public decimal? TotalPrice { get; set; }
        public IList<RestockItem> RestockItems { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

    }
}
