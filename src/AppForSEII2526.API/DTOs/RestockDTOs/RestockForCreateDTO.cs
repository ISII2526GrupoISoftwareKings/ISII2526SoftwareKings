using DataType = System.ComponentModel.DataAnnotations.DataType;

namespace AppForSEII2526.API.DTOs.RestockDTOs
{
    public class RestockForCreateDTO
    {
        public RestockForCreateDTO(string deliveryAddress, string title, string? description, DateTime? expectedDate, IList<RestockItemDTO> restockItems)
        {
            DeliveryAddress = deliveryAddress ?? throw new ArgumentNullException(nameof(deliveryAddress));
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Description = description;
            ExpectedDate = expectedDate;
            RestockItems = restockItems ?? throw new ArgumentNullException(nameof(restockItems));
        }

        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        [Display(Name = "Delivery Address")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "Delivery address must have at least 10 characters")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set your address for delivery")]
        public string DeliveryAddress { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set a title for the restock")]
        public string Title { get; set; }

        public string? Description { get; set; }

        public DateTime? ExpectedDate { get; set; }

        public IList<RestockItemDTO> RestockItems { get; set; }

    }
}
