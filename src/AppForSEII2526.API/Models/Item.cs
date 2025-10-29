using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataType = System.ComponentModel.DataAnnotations.DataType;

namespace AppForSEII2526.API.Models
{
    public class Item
    {
        public Item()
        {
        }

        public Item(int id, string name, string description, decimal purchasePrice, int quantityAvailableForPurchase, int quantityForRestock, decimal? restockPrice, List<PurchaseItem> purchaseItems, TypeItem typeItem, Brand brand, IList<RestockItem> restockItems)
        {
            Id = id;
            Name = name;
            Description = description;
            PurchasePrice = purchasePrice;
            QuantityAvailableForPurchase = quantityAvailableForPurchase;
            QuantityForRestock = quantityForRestock;
            RestockPrice = restockPrice;
            PurchaseItems = purchaseItems;
            TypeItem = typeItem;
            Brand = brand;
            RestockItems = restockItems;
        }

        public int Id { get; set; }

        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
        public string Description { get; set; }

        [DataType(DataType.Currency)]
        [Precision(10, 2)]
        public decimal PurchasePrice { get; set; }

        [Display(Name = "Quantity Available For Purchase")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity available must be 0 or more")]
        public int QuantityAvailableForPurchase { get; set; }

        [Display(Name = "Quantity For Restock")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity for restock must be 0 or more")]
        public int QuantityForRestock { get; set; }

        [DataType(DataType.Currency)]
        [Precision(10, 2)]
        public decimal? RestockPrice { get; set; }
        public List<PurchaseItem> PurchaseItems { get; set; }
        public TypeItem TypeItem { get; set; }
        public Brand Brand { get; set; }
        public IList<RestockItem> RestockItems { get; set; }
    }

}
