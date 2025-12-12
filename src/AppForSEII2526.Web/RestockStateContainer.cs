using AppForSEII2526.Web.API;

namespace AppForSEII2526.Web
{
    public class RestockStateContainer
    {
        // We create an instance of RestockForCreateDTO when an instance of RestockStateContainer is created
        public RestockForCreateDTO Restock { get; private set; } = new RestockForCreateDTO()
        {
            RestockItems = new List<RestockItemDTO>()
        };

        // We compute the TotalPrice of the items we have selected for the restock
        public decimal TotalPrice
        {
            get
            {
                // Sum of (Price * Quantity to buy). We handle null prices with ?? 0
                return (decimal) Restock.RestockItems.Sum(i => (i.PriceOfRestock ?? 0) * i.QuantityForRestock);
            }
        }

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();

        public void AddItemToRestock(ItemForRestockDTO itemDto)
        {
            // Before adding an item we check whether it has been already added to avoid duplicates
            if (!Restock.RestockItems.Any(ri => ri.ItemId == itemDto.Id))
            {
                // We add it if it is not in the list
                // Note: We set a default QuantityForRestock (amount to buy) to 1
                Restock.RestockItems.Add(new RestockItemDTO()
                {
                    ItemId = itemDto.Id,
                    Name = itemDto.Name,
                    Brand = itemDto.Brand,
                    QuantityInRestock = itemDto.QuantityForRestock, // Current stock level from DB
                    PriceOfRestock = itemDto.RestockPrice,
                    QuantityForRestock = 1 // Default purchase amount
                });
            }
        }

        // To delete items from the list of selected restock items
        public void RemoveItemFromRestock(RestockItemDTO item)
        {
            Restock.RestockItems.Remove(item);
        }

        // We eliminate all the items from the list
        public void ClearRestockCart()
        {
            Restock.RestockItems.Clear();
        }

        // We have already finished the process of restocking, thus, we create a new Restock object
        public void RestockProcessed()
        {
            // We have finished the restock process so we create a new object without data
            Restock = new RestockForCreateDTO()
            {
                RestockItems = new List<RestockItemDTO>()
            };
        }
    }
}