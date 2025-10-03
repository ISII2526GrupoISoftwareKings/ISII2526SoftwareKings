using Microsoft.Identity.Client;

namespace AppForSEII2526.API.Models
{
    public class Brand
    {
        public int Id { get; set; }

        [StringLength(100, ErrorMessage = "Brand name cannot be longer than 100 characters.")]
        public string Name { get; set; }
        public List<Item> Items { get; set; }
    }
}
