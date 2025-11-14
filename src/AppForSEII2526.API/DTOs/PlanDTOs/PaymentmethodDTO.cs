
namespace AppForSEII2526.API.DTOs.PlanDTOs
{
    public class PaymentmethodDTO
    {
        public PaymentmethodDTO(int id, string userName)
        {
            Id = id;
            UserName = userName;
        }

        public int Id { get; set; }
        public string UserName { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PaymentmethodDTO dTO &&
                   Id == dTO.Id &&
                   UserName == dTO.UserName;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, UserName);
        }
    }
}
