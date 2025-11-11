using Microsoft.AspNetCore.Identity;
using System.Net;

namespace AppForSEII2526.API.Models;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser {

    public ApplicationUser()
    {

    }

    public ApplicationUser(string id, string name, string surname, string userName, string address)
    {
        Id = id;
        Name = name;
        Surname = surname;
        UserName = userName;
        Email = userName;
        Address = address;
    }

    [Required]
    [StringLength(30, ErrorMessage = "Name cannot be longer than 30 characters.")]
    public string Name { get; set; }

    [Required]
    [StringLength(30, ErrorMessage = "Surname cannot be longer than 30 characters.")]
    public string Surname { get; set; }

    [Required]
    public string Address { get; set; }

    public List<PaymentMethod> PaymentMethods { get; set; }
    public IList<Restock> Restocks { get; set; }
}