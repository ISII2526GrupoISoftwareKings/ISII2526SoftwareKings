namespace AppForSEII2526.API.DTOs.RestockDTOs
{
    public class RestockDetailDTO : RestockForCreateDTO
    {
        public RestockDetailDTO(
            int id,
            DateTime restockDate,
            string deliveryAddress,
            string title,
            string? description,
            DateTime? expectedDate,
            IList<RestockItemDTO> restockItems,
            decimal? totalPrice)
            : base(deliveryAddress, title, description, expectedDate, restockDate, restockItems)
        {
            Id = id;
            TotalPrice = totalPrice;
        }

        public int Id { get; set; }
        public decimal? TotalPrice { get; set; }

        [StringLength(30, ErrorMessage = "Name cannot be longer than 30 characters.")]
        public string? AdminName { get; set; }

        [StringLength(30, ErrorMessage = "Surname cannot be longer than 30 characters.")]
        public string? AdminSurname { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is RestockDetailDTO dto &&
                   base.Equals(obj) &&
                   TotalPrice == dto.TotalPrice &&
                   Id == dto.Id;
        }

        public override int GetHashCode()
        {
            // Igual que tu patrón: combina el hash del base con los campos propios
            return HashCode.Combine(base.GetHashCode(), TotalPrice, Id);
        }
    }
}
