using AppForSEII2526.Web.API;

namespace AppForSEII2526.Web
{
    public class RestockStateContainer
    {
        public RestockForCreateDTO Restock { get; private set; } = new RestockForCreateDTO()
        {
            RestockItems = new List<RestockItemDTO>()
        };

        public decimal TotalPrice
        {
            get
            {
                return (decimal) Restock.RestockItems.Sum(i => (i.PriceOfRestock ?? 0) * i.QuantityForRestock);
            }
        }

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();

        public void AddItemToRestock(ItemForRestockDTO itemDto)
        {
            if (!Restock.RestockItems.Any(ri => ri.ItemId == itemDto.Id))
            {

                Restock.RestockItems.Add(new RestockItemDTO()
                {
                    ItemId = itemDto.Id,
                    Name = itemDto.Name,
                    Brand = itemDto.Brand,
                    QuantityInRestock = itemDto.QuantityForRestock,
                    PriceOfRestock = itemDto.RestockPrice,
                    QuantityForRestock = 1
                });
            }
        }

        public void RemoveItemFromRestock(RestockItemDTO item)
        {
            Restock.RestockItems.Remove(item);
        }

        public void ClearRestockCart()
        {
            Restock.RestockItems.Clear();
        }

        public void RestockProcessed()
        {
            Restock = new RestockForCreateDTO()
            {
                RestockItems = new List<RestockItemDTO>()
            };
        }
    }
}