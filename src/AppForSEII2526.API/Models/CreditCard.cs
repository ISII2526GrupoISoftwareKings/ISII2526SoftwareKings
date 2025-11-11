using DataType = System.ComponentModel.DataAnnotations.DataType;

namespace AppForSEII2526.API.Models
{
    public class CreditCard : PaymentMethod
    {

        public CreditCard()
        {

        }
        public CreditCard(string creditCardNumber, DateTime expirationDate, int id, ApplicationUser user) : base(id, user)
        {
            CreditCardNumber = creditCardNumber;
            ExpirationDate = expirationDate;
        }
        [Required]
        [CreditCard]
        [StringLength(16, MinimumLength = 13)]
        [DataType(DataType.CreditCard)]
        [Display(Name = "Credit Card Number")]
        public string CreditCardNumber { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Expiration Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ExpirationDate { get; set; }


    }
}
