using System.ComponentModel.DataAnnotations;
using DataType = System.ComponentModel.DataAnnotations.DataType;

namespace AppForSEII2526.API.DTOs.PurchaseDTOs
{
    public class PurchaseForCreateDTO
    {
        public PurchaseForCreateDTO(string city, string country, string street, string? description, DateTime date, decimal totalPrice, PaymentMethod paymentMethod, List<PurchaseItemDTO> purchaseItems)
        {
            City = city;
            Country = country;
            Street = street;
            Description = description;
            Date = date;
            TotalPrice = totalPrice;
            PaymentMethod = paymentMethod;
            PurchaseItems = purchaseItems;
        }

        [StringLength(50, ErrorMessage = "City cannot be longer than 50 characters.")]
        public string City { get; set; }

        [StringLength(50, ErrorMessage = "Country cannot be longer than 50 characters.")]
        public string Country { get; set; }

        [StringLength(100, ErrorMessage = "Street cannot be longer than 100 characters.")]
        public string Street { get; set; }

        [StringLength(200, ErrorMessage = "Description cannot be longer than 200 characters.")]
        public string? Description { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [DataType(DataType.Currency)]
        [Precision(10, 2)]
        public decimal TotalPrice { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public List<PurchaseItemDTO> PurchaseItems { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PurchaseForCreateDTO dTO &&
                   City == dTO.City &&
                   Country == dTO.Country &&
                   Street == dTO.Street &&
                   Description == dTO.Description &&
                   Date == dTO.Date &&
                   TotalPrice == dTO.TotalPrice &&
                   EqualityComparer<PaymentMethod>.Default.Equals(PaymentMethod, dTO.PaymentMethod) &&
                   EqualityComparer<List<PurchaseItemDTO>>.Default.Equals(PurchaseItems, dTO.PurchaseItems);
        }
    }
}
