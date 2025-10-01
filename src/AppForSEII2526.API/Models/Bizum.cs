namespace AppForSEII2526.API.Models
{
    public class Bizum : PaymentMethod
    {
        [Key]
        [Display (Name = "Telephone Number")]
        public string telephoneNumber { get; set; }
    }
}
