using System.ComponentModel.DataAnnotations;
using DataType = System.ComponentModel.DataAnnotations.DataType;

namespace AppForSEII2526.API.DTOs.PurchaseDTOs
{
    public class PurchaseForDetailDTO : PurchaseForCreateDTO
    {
        public PurchaseForDetailDTO(int id, decimal totalPrice, string city, string country, string street, string? description, DateTime date, PaymentMethodDTO paymentMethod, List<PurchaseItemDTO> purchaseItems)
            : base(city, country, street, description, date, totalPrice, paymentMethod, purchaseItems)
        {
            Id = id;
            TotalPrice = totalPrice;
        }

        public int Id { get; set; }

        [DataType(DataType.Currency)]
        [Precision(10, 2)]
        public decimal TotalPrice { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PurchaseForDetailDTO dTO &&
                   City == dTO.City &&
                   Country == dTO.Country &&
                   Street == dTO.Street &&
                   Description == dTO.Description &&
                   Date == dTO.Date &&
                   TotalPrice == dTO.TotalPrice &&
                   Id == dTO.Id &&
                   TotalPrice == dTO.TotalPrice;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Date);
        }
    }
}
