using Microsoft.AspNetCore.Identity;

namespace AppForSEII2526.API.Models;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser {
    [StringLength(30, ErrorMessage = "Name cannot be longer than 30 characters.")]
    public string Name { get; set; }

    [StringLength(30, ErrorMessage = "Surname cannot be longer than 30 characters.")]
    public string Surname { get; set; }
}