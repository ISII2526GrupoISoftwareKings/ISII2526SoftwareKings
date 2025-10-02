using DataType = System.ComponentModel.DataAnnotations.DataType;

namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(PlanId), nameof(ClassId))]
    public class PlanItem
    {
        public int PlanId { get; set; }

        public int ClassId { get; set; }

        [StringLength(100, ErrorMessage = "Goal can be neither longer than 100 characters nor shorter than 1", MinimumLength = 1)]
        public string Goal { get; set; }

        [DataType(DataType.Currency)]
        [Precision(5,2)]
        public decimal Price { get; set; }


        public Plan Plan { get; set; }
    }
}
