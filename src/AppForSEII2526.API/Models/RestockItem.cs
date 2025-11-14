

    namespace AppForSEII2526.API.Models
    {
        [PrimaryKey(nameof(ItemId), nameof(RestockId))]
        public class RestockItem
        {
            public RestockItem()
            {
            }

            public RestockItem(Item item, Restock restock, int quantity, decimal? restockPrice)
            {
                ItemId = item.Id;
                Item = item;
                RestockId = restock.Id;
                Restock = restock;
                Quantity = quantity;
                RestockPrice = restockPrice;
            }

            public RestockItem(int itemId, Restock restock, int quantity, decimal? restockPrice)
            {
                ItemId = itemId;
                RestockId = restock.Id;
                Restock = restock;
                Quantity = quantity;
                RestockPrice = restockPrice;
            }

            public int ItemId { get; set; }
            public Item Item { get; set; }

            public int RestockId { get; set; }
            public Restock Restock { get; set; }

            public int Quantity { get; set; }
            public decimal? RestockPrice { get; set; }


        }
    }
