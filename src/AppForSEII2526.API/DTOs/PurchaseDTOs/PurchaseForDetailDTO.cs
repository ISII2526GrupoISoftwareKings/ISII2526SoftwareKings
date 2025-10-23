using System.ComponentModel.DataAnnotations;
using DataType = System.ComponentModel.DataAnnotations.DataType;

namespace AppForSEII2526.API.DTOs.PurchaseDTOs
{
    public class PurchaseForDetailDTO : PurchaseForCreateDTO
    {
        public PurchaseForDetailDTO(int id, decimal totalPrice, string city, string country, string street, string? description, DateTime date, PaymentMethod paymentMethod, List<PurchaseItemDTO> purchaseItems)
            : base(city, country, street, description, date, paymentMethod, purchaseItems)
        {
            Id = id;
            TotalPrice = totalPrice;
        }

        public int Id { get; set; }

        [DataType(DataType.Currency)]
        [Precision(10, 2)]
        public decimal TotalPrice { get; set; }
    }
}
