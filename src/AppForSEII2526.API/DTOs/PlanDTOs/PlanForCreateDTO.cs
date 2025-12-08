using DataType = System.ComponentModel.DataAnnotations.DataType;


namespace AppForSEII2526.API.DTOs.PlanDTOs
{
    public class PlanForCreateDTO
    {
        public PlanForCreateDTO(string name, string nameUser, string? surnameUser, string? description, int weeks, DateTime createdDate, string? healthIssues, PaymentmethodDTO paymentMethod, List<PlanItemDTO> planItems)
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

        [Required(ErrorMessage = "Plan name is required")]
        [StringLength(20, ErrorMessage = "Plan name must be between 1 and 20 characters", MinimumLength = 1)]
        public string Name { get; set; }

        [Required(ErrorMessage = "User name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "User name must be between 3 and 50 characters")]
        public string NameUser { get; set; }

        [StringLength(50, ErrorMessage = "Surname cannot be longer than 50 characters")]
        public string? SurnameUser { get; set; }

        [StringLength(100, ErrorMessage = "Description cannot be longer than 100 characters")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Number of weeks is required")]
        [Range(1, 52, ErrorMessage = "Number of weeks must be between 1 and 52")]
        public int Weeks { get; set; }

        [DataType(DataType.DateTime), Display(Name = "Created Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
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
