using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options) {
    public DbSet<PaymentMethod> PaymentMethods { get; set;}

    public DbSet<CreditCard> CreditCards { get; set; }
    public DbSet<Paypal> Paypals { get; set; }
    public DbSet<Bizum> Bizums { get; set; }

    public DbSet<TypeItem> TypeItems { get; set; }
    public DbSet<Class> Classes { get; set; }
    public DbSet<PlanItem> PlanItems { get; set; }
    public DbSet<Plan> Plans { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
