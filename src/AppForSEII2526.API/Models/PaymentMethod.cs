namespace AppForSEII2526.API.Models
{
    public abstract class PaymentMethod
    {

        public PaymentMethod()
        {
            
        }
        protected PaymentMethod(int id, ApplicationUser user)
        {
            Id = id;
            User = user;
        }

        public int Id { get; set; }

        public ApplicationUser User { get; set; }

        public List<Plan> Plans { get; set; }
    }
}
