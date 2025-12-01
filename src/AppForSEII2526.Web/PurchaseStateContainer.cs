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
            var existingItem = Purchase.PurchaseItems.FirstOrDefault(i => i.ItemId == item.ItemId);

            if (existingItem is null)
            {
                // we add it if it is not in the list
                Purchase.PurchaseItems.Add(new PurchaseItemDTO()
                {
                    ItemId = item.ItemId,
                    Name = item.Name,
                    Brand = item.Brand,
                    AmountBought = 1,
                    Price = item.Price
                });
                }
            else
            {
                // if it already exists, increase quantity
                existingItem.AmountBought += 1;
            }

                NotifyStateChanged();
            }
        }

        public void AddItemToPurchase(ItemForPurchasingDTO item)
        {
            var existingItem = Purchase.PurchaseItems.FirstOrDefault(i => i.ItemId == item.Id);

            if (existingItem is null)
            {
                // we add it if it is not in the list
                Purchase.PurchaseItems.Add(new PurchaseItemDTO()
                {
                    ItemId = item.Id,
                    Name = item.Name,
                    Brand = item.Brand,
                    AmountBought = 1,
                    Price = item.Price
                });
            }
            else
            {
                // if it already exists, increase quantity
                existingItem.AmountBought += 1;
                }

                NotifyStateChanged();
            }
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
