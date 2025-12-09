namespace AppForSEII2526.API.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    [Index(nameof(Name), IsUnique = true)]
    public class Plan
    {
        public Plan()
        {

        }

        public Plan(string name, int weeks, DateTime createdDate, PaymentMethod paymentMethod, List<PlanItem> planItems, string description, string healthIssues)
        {
            Name = name;
            Weeks = weeks;
            CreatedDate = createdDate;
            PaymentMethod = paymentMethod;
            PlanItems = planItems;
            Description = description;
            HealthIssues = healthIssues;
        }

        public int Id { get; set; }

        [StringLength(20, ErrorMessage = "Name of Plan can be neither longer than 20 characters nor shorter than 1", MinimumLength = 1)]
        public string Name { get; set; }

        [StringLength(100, ErrorMessage = "Description cannot be longer than 100 characters.")]
        public string? Description { get; set; }

        [Range(1, 52, ErrorMessage = "Number of weeks must be between 1 and 52")]
        public int Weeks { get; set; }

        [DataType(DataType.DateTime), Display(Name = "Created Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }

        [DataType(DataType.Currency),]
        [Display(Name = "Total Price")]
        [Precision(5, 2)]
        public decimal TotalPrice { get; set; }

        [Display(Name = "Health Issues")]
        [StringLength(100, ErrorMessage = "Health Issues cannot be longer than 100 characters.")]
        public string? HealthIssues { get; set; }


        public PaymentMethod PaymentMethod { get; set; }
        public List<PlanItem> PlanItems { get; set; }
    }
}
