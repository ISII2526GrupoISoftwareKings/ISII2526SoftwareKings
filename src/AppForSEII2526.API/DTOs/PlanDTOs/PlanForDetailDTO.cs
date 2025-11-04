using DataType = System.ComponentModel.DataAnnotations.DataType;
namespace AppForSEII2526.API.DTOs.PlanDTOs
{
    public class PlanForDetailDTO:PlanForCreateDTO
    {
        public PlanForDetailDTO(int id, decimal totalPrice, DateTime planDate, string name, string nameUser, string surnameUser, string? description, int weeks, DateTime createdDate, string? healthIssues, PaymentMethod paymentMethod, List<PlanItemDTO> planItems) : base(name, nameUser, surnameUser, description, weeks, createdDate, healthIssues, paymentMethod, planItems)
        {
            Id = id;
            TotalPrice = totalPrice;
            PlanDate = planDate;
        }

        public int Id { get; set; }

        [DataType(DataType.Currency),]
        [Display(Name = "Total Price")]
        [Precision(5, 2)]
        public decimal TotalPrice { get; set; }

        public DateTime PlanDate { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PlanForDetailDTO dTO &&
                   base.Equals(obj) &&
                   Name == dTO.Name &&
                   NameUser == dTO.NameUser &&
                   SurnameUser == dTO.SurnameUser &&
                   Description == dTO.Description &&
                   Weeks == dTO.Weeks &&
                   CreatedDate == dTO.CreatedDate &&
                   HealthIssues == dTO.HealthIssues &&
                   EqualityComparer<PaymentMethod>.Default.Equals(PaymentMethod, dTO.PaymentMethod) &&
                   EqualityComparer<List<PlanItemDTO>>.Default.Equals(PlanItems, dTO.PlanItems) &&
                   Id == dTO.Id &&
                   TotalPrice == dTO.TotalPrice &&
                   PlanDate == dTO.PlanDate;
        }
    }
}
