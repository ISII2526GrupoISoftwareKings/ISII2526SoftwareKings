namespace AppForSEII2526.API.Models
{
    public class Bizum : PaymentMethod
    {
        [Phone]
        [Display (Name = "Telephone Number")]
        public string telephoneNumber { get; set; }
    }
}
