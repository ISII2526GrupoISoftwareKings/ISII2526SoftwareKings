using System.ComponentModel.DataAnnotations;

namespace AppForSEII2526.API.DTOs.PurchaseDTOs
{
    public class PaymentMethodDTO
    {
        public PaymentMethodDTO()
        {
        }

        public PaymentMethodDTO(int id)
        {
            Id = id;
        }

        [Required(ErrorMessage = "Payment method ID is required.")]
        public int Id { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PaymentMethodDTO dTO &&
                   Id == dTO.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}

