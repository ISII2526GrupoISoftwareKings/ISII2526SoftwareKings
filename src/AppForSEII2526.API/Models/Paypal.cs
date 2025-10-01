namespace AppForSEII2526.API.Models
{
    public class Paypal : PaymentMethod
    {
        [Key]
        public string Email { get; set; }
    }
}
