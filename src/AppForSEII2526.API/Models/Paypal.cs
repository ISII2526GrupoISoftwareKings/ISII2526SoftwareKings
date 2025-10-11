namespace AppForSEII2526.API.Models
{
    public class Paypal : PaymentMethod
    {
        [EmailAddress]
        public string Email { get; set; }
    }
}
