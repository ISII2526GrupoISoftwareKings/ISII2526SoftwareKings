using DataType = System.ComponentModel.DataAnnotations.DataType;


namespace AppForSEII2526.API.DTOs.PlanDTOs
{
    public class PlanForCreateDTO
    {
        public PlanForCreateDTO(string name, string nameUser, string surnameUser, string? description, int weeks, DateTime createdDate, string? healthIssues, PaymentmethodDTO paymentMethod, List<PlanItemDTO> planItems)
        {
            Name = name;
            NameUser = nameUser;
            SurnameUser = surnameUser;
            Description = description;
            Weeks = weeks;
            CreatedDate = createdDate;
            HealthIssues = healthIssues;
            PaymentMethod = paymentMethod;
            PlanItems = planItems;
        }

        [StringLength(20, ErrorMessage = "Name of Plan can be neither longer than 20 characters nor shorter than 1", MinimumLength = 1)]
        public string Name { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must have at least 3 characters")]
        public string NameUser { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage = "Surname must have at least 3 characters")]
        public string SurnameUser { get; set; }

        [StringLength(100, ErrorMessage = "Description cannot be longer than 100 characters.")]
        public string? Description { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Minimum quantity for renting is 1")]
        public int Weeks { get; set; }

        [DataType(DataType.Date), Display(Name = "Created Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }

        

        [Display(Name = "Health Issues")]
        [StringLength(100, ErrorMessage = "Health Issues cannot be longer than 100 characters.")]
        public string? HealthIssues { get; set; }


        public PaymentmethodDTO PaymentMethod { get; set; }
        public List<PlanItemDTO> PlanItems { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PlanForCreateDTO dTO &&
                   Name == dTO.Name &&
                   NameUser == dTO.NameUser &&
                   SurnameUser == dTO.SurnameUser &&
                   Description == dTO.Description &&
                   Weeks == dTO.Weeks &&
                   CreatedDate == dTO.CreatedDate &&
                   HealthIssues == dTO.HealthIssues &&
                   EqualityComparer<PaymentmethodDTO>.Default.Equals(PaymentMethod, dTO.PaymentMethod) &&
                   EqualityComparer<List<PlanItemDTO>>.Default.Equals(PlanItems, dTO.PlanItems);
        }
    }
}
