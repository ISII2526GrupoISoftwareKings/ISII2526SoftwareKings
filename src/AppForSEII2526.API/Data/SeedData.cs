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

            try
            {
                var user = dbContext.Users.OfType<ApplicationUser>().FirstOrDefault(u => u.UserName == "alberto@uclm.es");
                SeedPaymentMethods(dbContext, user);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding PaymentMethods in the Database.");
            }

            try
            {
                SeedClassesAndTypeItems(dbContext);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding Classes and TypeItems in the Database.");
            }

            try
            {
                var user = dbContext.Users.OfType<ApplicationUser>().FirstOrDefault(u => u.UserName == "alberto@uclm.es");
                SeedPlansAndPlanItems(dbContext, user);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding Plans and PlanItems in the Database.");
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
            // Seed Alberto - Administrator
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

                var result = userManager.CreateAsync(user, "Pass123$");
                result.Wait();

                if (result.IsCompletedSuccessfully)
                {
                    userManager.AddToRoleAsync(user, roles[0]).Wait(); // Administrator
                }
            }

            // Seed Samuel - Employee
            if (userManager.FindByNameAsync("samuel@uclm.es").Result == null)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = "samuel@uclm.es",
                    Email = "samuel@uclm.es",
                    Name = "Samuel",
                    Surname = "Garcia Picazo",
                    Address = "Calle Principal 456, Toledo",
                    EmailConfirmed = true
                };

                var result = userManager.CreateAsync(user, "Pass123$");
                result.Wait();

                if (result.IsCompletedSuccessfully)
                {
                    userManager.AddToRoleAsync(user, roles[1]).Wait(); // Employee
                }
            }

            // Seed Customer
            if (userManager.FindByNameAsync("customer@uclm.es").Result == null)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = "customer@uclm.es",
                    Email = "customer@uclm.es",
                    Name = "Customer",
                    Surname = "Test",
                    Address = "Calle Ejemplo 789, Albacete",
                    EmailConfirmed = true
                };

                var result = userManager.CreateAsync(user, "Pass123$");
                result.Wait();

                if (result.IsCompletedSuccessfully)
                {
                    userManager.AddToRoleAsync(user, roles[2]).Wait(); // Customer
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

        public static void SeedPaymentMethods(ApplicationDbContext dbcontext, ApplicationUser user)
        {
            // Seed CreditCard payment method (Id will be 1)
            if (dbcontext.PaymentMethods.OfType<CreditCard>().FirstOrDefault(pm => pm.User.UserName == user.UserName) == null)
            {
                var creditCard = new CreditCard
                {
                    User = user,
                    CreditCardNumber = "1234567890123456",
                    ExpirationDate = new DateTime(2030, 12, 31)
                };
                dbcontext.PaymentMethods.Add(creditCard);
                dbcontext.SaveChanges();
            }
            
            // Seed Paypal payment method (Id will be 2)
            if (dbcontext.PaymentMethods.OfType<Paypal>().FirstOrDefault(pm => pm.User.UserName == user.UserName) == null)
            {
                var paypal = new Paypal
                {
                    User = user,
                    Email = user.Email ?? "paypal@example.com"
                };
                dbcontext.PaymentMethods.Add(paypal);
                dbcontext.SaveChanges();
            }
            
            // Seed Bizum payment method (Id will be 3)
            if (dbcontext.PaymentMethods.OfType<Bizum>().FirstOrDefault(pm => pm.User.UserName == user.UserName) == null)
            {
                var bizum = new Bizum
                {
                    User = user,
                    telephoneNumber = "636187115"
                };
                dbcontext.PaymentMethods.Add(bizum);
                dbcontext.SaveChanges();
            }
        }

        public static void SeedClassesAndTypeItems(ApplicationDbContext dbcontext)
        {
            // Seed Classes
            List<Class> classes = new List<Class>();

            if (dbcontext.Classes.FirstOrDefault(c => c.Name == "Yoga") == null)
            {
                var yoga = new Class
                {
                    Name = "Yoga",
                    Price = 11.00m,
                    Capacity = 20,
                    Date = new DateTime(2026, 1, 10, 18, 0, 0)
                };
                classes.Add(yoga);
                dbcontext.Classes.Add(yoga);
            }

            if (dbcontext.Classes.FirstOrDefault(c => c.Name == "Spinning") == null)
            {
                var spinning = new Class
                {
                    Name = "Spinning",
                    Price = 11.00m,
                    Capacity = 20,
                    Date = new DateTime(2026, 1, 11, 17, 0, 0)
                };
                classes.Add(spinning);
                dbcontext.Classes.Add(spinning);
            }

            if (dbcontext.Classes.FirstOrDefault(c => c.Name == "CrossFit") == null)
            {
                var crossfit = new Class
                {
                    Name = "CrossFit",
                    Price = 10.00m,
                    Capacity = 0,
                    Date = new DateTime(2026, 1, 12, 16, 0, 0)
                };
                classes.Add(crossfit);
                dbcontext.Classes.Add(crossfit);
            }

            if (dbcontext.Classes.FirstOrDefault(c => c.Name == "Strech & Relax") == null)
            {
                var stretchRelax = new Class
                {
                    Name = "Strech & Relax",
                    Price = 10.00m,
                    Capacity = 25,
                    Date = new DateTime(2026, 1, 13, 20, 0, 0)
                };
                classes.Add(stretchRelax);
                dbcontext.Classes.Add(stretchRelax);
            }

            if (dbcontext.Classes.FirstOrDefault(c => c.Name == "Zumba") == null)
            {
                var zumba = new Class
                {
                    Name = "Zumba",
                    Price = 8.00m,
                    Capacity = 20,
                    Date = new DateTime(2026, 1, 14, 17, 30, 0)
                };
                classes.Add(zumba);
                dbcontext.Classes.Add(zumba);
            }

            if (dbcontext.Classes.FirstOrDefault(c => c.Name == "Pilates") == null)
            {
                var pilates = new Class
                {
                    Name = "Pilates",
                    Price = 10.00m,
                    Capacity = 25,
                    Date = new DateTime(2026, 1, 15, 15, 30, 0)
                };
                classes.Add(pilates);
                dbcontext.Classes.Add(pilates);
            }

            dbcontext.SaveChanges();

            // Seed TypeItems (equipment for each class)
            var yogaClass = dbcontext.Classes.FirstOrDefault(c => c.Name == "Yoga");
            if (yogaClass != null && (yogaClass.TypeItems == null || yogaClass.TypeItems.Count == 0))
            {
                yogaClass.TypeItems = new List<TypeItem>
                {
                    new TypeItem { Name = "Yoga mat" },
                    new TypeItem { Name = "Yoga block" }
                };
            }

            var spinningClass = dbcontext.Classes.FirstOrDefault(c => c.Name == "Spinning");
            if (spinningClass != null && (spinningClass.TypeItems == null || spinningClass.TypeItems.Count == 0))
            {
                spinningClass.TypeItems = new List<TypeItem>
                {
                    new TypeItem { Name = "Towel" },
                    new TypeItem { Name = "Water bottle" }
                };
            }

            var crossfitClass = dbcontext.Classes.FirstOrDefault(c => c.Name == "CrossFit");
            if (crossfitClass != null && (crossfitClass.TypeItems == null || crossfitClass.TypeItems.Count == 0))
            {
                crossfitClass.TypeItems = new List<TypeItem>
                {
                    new TypeItem { Name = "Dumbbells" },
                    new TypeItem { Name = "Kettlebell" }
                };
            }

            var stretchClass = dbcontext.Classes.FirstOrDefault(c => c.Name == "Strech & Relax");
            if (stretchClass != null && (stretchClass.TypeItems == null || stretchClass.TypeItems.Count == 0))
            {
                stretchClass.TypeItems = new List<TypeItem>
                {
                    new TypeItem { Name = "Foam Roller" },
                    new TypeItem { Name = "Stretching band" }
                };
            }

            var zumbaClass = dbcontext.Classes.FirstOrDefault(c => c.Name == "Zumba");
            if (zumbaClass != null && (zumbaClass.TypeItems == null || zumbaClass.TypeItems.Count == 0))
            {
                zumbaClass.TypeItems = new List<TypeItem>
                {
                    new TypeItem { Name = "Resistance band" },
                    new TypeItem { Name = "Floor mat" }
                };
            }

            var pilatesClass = dbcontext.Classes.FirstOrDefault(c => c.Name == "Pilates");
            if (pilatesClass != null && (pilatesClass.TypeItems == null || pilatesClass.TypeItems.Count == 0))
            {
                pilatesClass.TypeItems = new List<TypeItem>
                {
                    new TypeItem { Name = "Medicine Ball" },
                    new TypeItem { Name = "Magic Ring" }
                };
            }

            dbcontext.SaveChanges();
        }

        public static void SeedPlansAndPlanItems(ApplicationDbContext dbcontext, ApplicationUser user)
        {
            // Get the payment method for the user
            var paymentMethod = dbcontext.PaymentMethods.FirstOrDefault(pm => pm.User.UserName == user.UserName);
            
            if (paymentMethod == null)
                return; // Payment method must exist first

            // Seed Plan
            if (dbcontext.Plans.FirstOrDefault(p => p.Name == "PLAN777") == null)
            {
                var plan = new Plan
                {
                    Name = "PLAN777",
                    Description = null,
                    Weeks = 6,
                    CreatedDate = new DateTime(2025, 10, 10),
                    TotalPrice = 50.00m,
                    HealthIssues = null,
                    PaymentMethod = paymentMethod
                };
                dbcontext.Plans.Add(plan);
                dbcontext.SaveChanges();

                // Seed PlanItem
                var yogaClass = dbcontext.Classes.FirstOrDefault(c => c.Name == "Yoga");
                if (yogaClass != null)
                {
                    var planItem = new PlanItem
                    {
                        Plan = plan,
                        Class = yogaClass,
                        Goal = "Sport",
                        Price = 11.00m
                    };
                    dbcontext.PlanItems.Add(planItem);
                    dbcontext.SaveChanges();
                }
            }
        }
    }
}
