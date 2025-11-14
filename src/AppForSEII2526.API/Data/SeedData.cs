namespace AppForSEII2526.API.Data
{
    public class SeedData
    {
        public static void Initialize(ApplicationDbContext dbContext, IServiceProvider serviceProvider, ILogger logger)
        {
            List<string> rolesNames = new List<string> { "Administrator", "Employee", "Customer" };

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            try
            {
                SeedRoles(roleManager, rolesNames);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding the roles in the Database.");
            }

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            try
            {
                SeedUsers(userManager, rolesNames);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding the Users in the Database.");
            }

            try
            {
                SeedBrandsTypesAndItems(dbContext);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding the Brands, TypeItems and Items in the Database.");
            }

            try
            {
                var user = dbContext.Users.OfType<ApplicationUser>().FirstOrDefault(u => u.UserName == "alberto@uclm.es");

                SeedRestock(dbContext, user);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding a Restock in the Database.");
            }
        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager, List<string> roles)
        {

            foreach (string roleName in roles)
            {
                if (!roleManager.RoleExistsAsync(roleName).Result)
                {
                    IdentityRole role = new IdentityRole();
                    role.Name = roleName;
                    role.NormalizedName = roleName;
                    IdentityResult roleResult = roleManager.CreateAsync(role).Result;
                }
            }

        }

        public static void SeedUsers(UserManager<ApplicationUser> userManager, List<string> roles)
        {
            if (userManager.FindByNameAsync("alberto@uclm.es").Result == null)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = "alberto@uclm.es",
                    Email = "alberto@uclm.es",
                    Name = "Alberto",
                    Surname = "Bueno Baquero",
                    Address = "Calle Falsa 123, Ciudad Real",
                    EmailConfirmed = true
                };

                var result = userManager.CreateAsync(user, "Password1234%");
                result.Wait();

                if (result.IsCompletedSuccessfully)
                {
                    userManager.AddToRoleAsync(user, roles[0]).Wait();
                }
            }

        }

        public static void SeedBrandsTypesAndItems(ApplicationDbContext dbcontext)
        {
            string[] brandnames = ["Apple", "Samsung", "Sony", "Dell"];
            List<Brand> brands = [];
            string[] typenamenames = ["Smartphone", "Laptop", "Headphones"];
            List<TypeItem> types = [];
            Item item;

            foreach (string bname in brandnames)
            {
                var brand = dbcontext.Brands.FirstOrDefault(b => b.Name == bname);
                if (brand == null)
                    brands.Add(new Brand { Name = bname });
                else
                    brands.Add(brand);
            }

            foreach (string tname in typenamenames)
            {
                var type = dbcontext.TypeItems.FirstOrDefault(t => t.Name == tname);
                if (type == null)
                    types.Add(new TypeItem { Name = tname });
                else
                    types.Add(type);
            }

            if (dbcontext.Items.FirstOrDefault(i => i.Name == "iPhone 15") == null)
            {
                item = new Item
                {
                    Name = "iPhone 15",
                    Description = "Smartphone 6.1\"",
                    PurchasePrice = 999.00m,
                    QuantityAvailableForPurchase = 5,
                    QuantityForRestock = 10,
                    RestockPrice = 870.00m,
                    Brand = brands[0],
                    TypeItem = types[0]
                };
                dbcontext.Items.Add(item);

            }

            if (dbcontext.Items.FirstOrDefault(i => i.Name == "XPS 13") == null)
            {
                item = new Item
                {
                    Name = "XPS 13",
                    Description = "Ultrabook 13\"",
                    PurchasePrice = 1399.00m,
                    QuantityAvailableForPurchase = 10,
                    QuantityForRestock = 6,
                    RestockPrice = 1150.00m,
                    Brand = brands[3],
                    TypeItem = types[1]
                };
                dbcontext.Items.Add(item);
            }

            dbcontext.SaveChanges();
        }

        public static void SeedRestock(ApplicationDbContext dbcontext, ApplicationUser user)
        {

            if (dbcontext.Restocks.FirstOrDefault(p => p.Id == 1) == null)
            {
                var item = dbcontext.Items.First();
                var restock = new Restock
                {
                    Title = "Restock inicial",
                    Description = "Reposición básica",
                    DeliveryAddress = "Avda. España s/n, Albacete 02071",
                    RestockDate = DateTime.Now,
                    ExpectedDate = DateTime.Today.AddDays(2),
                    RestockItems = new List<RestockItem>(),
                    ApplicationUser = user
                };
                restock.RestockItems.Add(new RestockItem
                {
                    Item = item,
                    Restock = restock,
                    Quantity = 1,
                    RestockPrice = item.RestockPrice
                });
                dbcontext.Restocks.Add(restock);
            }
            dbcontext.SaveChanges();

        }
    }
}
