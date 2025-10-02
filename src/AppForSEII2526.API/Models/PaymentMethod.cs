namespace AppForSEII2526.API.Models
{
    public abstract class PaymentMethod
    {
        public int Id { get; set; }

        public ApplicationUser User { get; set; }
        public List<PaymentMethod> PaymentMethods { get; set; }
    }
}
