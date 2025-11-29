using AppForSEII2526.Web.API;

namespace AppForSEII2526.Web
{
    public class PurchaseStateContainer
    {
        public PurchaseForCreateDTO Purchase { get; private set; } = new PurchaseForCreateDTO()
        {
            PurchaseItems = new List<PurchaseItemDTO>()
        };

        public decimal TotalPrice
        {
            get
            {
                return Convert.ToDecimal(Purchase.PurchaseItems.Sum(item => item.Price * item.AmountBought));
            }
        }

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();

        public void AddItemToPurchase(PurchaseItemDTO item)
        {
            if (!Purchase.PurchaseItems.Any(i => i.ItemId == item.ItemId))
                //we add it if is not in the list
                Purchase.PurchaseItems.Add(new PurchaseItemDTO()
                {
                    ItemId = item.ItemId,
                    Name = item.Name,
                    Brand = item.Brand,
                    AmountBought = 1,
                    Price = item.Price
                }
            );
        }

        //to delete an item from the list of selected items
        public void RemoveItemFromPurchase(PurchaseItemDTO item)
        {
            Purchase.PurchaseItems.Remove(item);
        }

        //We eliminate all the items from the list
        public void ClearPurchaseCart()
        {
            Purchase.PurchaseItems.Clear();
        }

        //we have already finished the process of purchasing, thus, we create a new Purchase
        public void PurchaseProcessed()
        {
            Purchase = new PurchaseForCreateDTO()
            {
                PurchaseItems = new List<PurchaseItemDTO>()
            };
        }
    }
}
