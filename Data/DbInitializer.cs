using Microsoft.EntityFrameworkCore;
using megamart_backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace megamart_backend.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(AppDbContext context)
        {
            // Ensure database and migrations are applied
            await context.Database.EnsureCreatedAsync();

            // 1. Seed Categories First (Required for Foreign Key constraint)
            // 1. Seed Categories only if the table is empty
            if (!await context.Categories.AnyAsync())
            {
                var categories = new List<Category>
                {
                    new Category { Id = 1,  Name = "Smartphones" },
                    new Category { Id = 2, Name = "Electronics" },
                    new Category { Id = 3, Name = "Cosmetics" }
                };

                context.Categories.AddRange(categories);
                await context.SaveChangesAsync();
            }

            // 2. Seed Products only if the table is empty
            if (!await context.Products.AnyAsync())
            {
                var products = new List<Product>
                {
                    // --- Product 101: Galaxy S22 Ultra 5G ---
                    new Product
                    {
                        Id = 101,
                        CategoryId = 1,
                        Name = "Galaxy S22 Ultra 5G",
                        Price = 67999,
                        StockQuantity = 18,
                        ImageUrl = "https://res.cloudinary.com/ktvvajnj/image/upload/v1791032357/sample1.jpg",
                        ImagePublicId = "sample1",
                        SecondaryImages = new List<SecondaryImage>
                        {
                            new SecondaryImage { ImageUrl = "https://res.cloudinary.com/ktvvajnj/image/upload/v1791032617/sample1_sec1.jpg", ImagePublicId = "sample1_sec1" },
                            new SecondaryImage { ImageUrl = "https://res.cloudinary.com/ktvvajnj/image/upload/v1791032713/sample1_sec2.jpg", ImagePublicId = "sample1_sec2" },
                            new SecondaryImage { ImageUrl = "https://res.cloudinary.com/ktvvajnj/image/upload/v1791211831/sample1_sec3.jpg", ImagePublicId = "sample1_sec3" }
                        },
                        Description = "High-performance flagship device engineered with premium build materials, pro-grade optics, and long battery endurance for demanding workloads.",
                        Specifications = new Specifications
                        {
                            Model = "Galaxy S22 Ultra",
                            Warranty = "1 Year Comprehensive Brand Warranty",
                            Delivery = "Free Standard Shipping available"
                        }
                    },

                    // --- Product 102: Wireless Noise Canceling Headphones ---
                    new Product
                    {
                        Id = 102,
                        CategoryId = 2,
                        Name = "Wireless Noise Canceling Headphones",
                        Price = 14999,
                        StockQuantity = 5,
                        ImageUrl = "https://res.cloudinary.com/ktvvajnj/image/upload/v1791211971/sample2.jpg",
                        ImagePublicId = "sample2",
                        SecondaryImages = new List<SecondaryImage>
                        {
                            new SecondaryImage { ImageUrl = "https://res.cloudinary.com/ktvvajnj/image/upload/v1791212046/sample2_sec1.jpg", ImagePublicId = "sample2_sec1" },
                            new SecondaryImage { ImageUrl = "https://res.cloudinary.com/ktvvajnj/image/upload/v1791212155/sample2_sec2.jpg", ImagePublicId = "sample2_sec2" },
                            new SecondaryImage { ImageUrl = "https://res.cloudinary.com/ktvvajnj/image/upload/v1791212218/sample2_sec3.jpg", ImagePublicId = "sample2_sec3" }
                        },
                        Description = "Immerse yourself in pure studio sound with active noise cancellation, ultra-soft memory foam ear cushions, and long-lasting battery life.",
                        Specifications = new Specifications
                        {
                            Model = "WH-1000XM Series",
                            Warranty = "2 Years Comprehensive Brand Warranty",
                            Delivery = "Free Standard Shipping available"
                        }
                    },

                    // --- Product 103: Hydrating Face Serum ---
                    new Product
                    {
                        Id = 103,
                        CategoryId = 3,
                        Name = "Hydrating Face Serum",
                        Price = 1299,/**/
                        StockQuantity = 40,
                        ImageUrl = "https://res.cloudinary.com/ktvvajnj/image/upload/v1791218046/sample3.jpg",
                        ImagePublicId = "sample3",
                        SecondaryImages = new List<SecondaryImage>
                        {
                            new SecondaryImage { ImageUrl = "https://res.cloudinary.com/ktvvajnj/image/upload/v1791218913/sample3_sec1.jpg", ImagePublicId = "sample3_sec1" },
                            new SecondaryImage { ImageUrl = "https://res.cloudinary.com/ktvvajnj/image/upload/v1791218978/sample3_sec2.jpg", ImagePublicId = "sample3_sec2" },
                            new SecondaryImage { ImageUrl = "https://res.cloudinary.com/ktvvajnj/image/upload/v1791219074/sample3_sec3.jpg", ImagePublicId = "sample3_sec3" }
                        },
                        Description = "Deeply nourishing facial serum enriched with essential vitamins to restore hydration and enhance natural skin glow.",
                        Specifications = new Specifications
                        {
                            Model = "HydraBoost Serum (30ml)",
                            Warranty = "100% Authentic Product Guarantee",
                            Delivery = "Free Standard Shipping available"
                        }
                    },

                    // --- Product 104: iPhone ---
                    new Product
                    {
                        Id = 104,
                        CategoryId = 1,
                        Name = "iPhone",
                        Price = 100000,
                        StockQuantity = 40,
                        ImageUrl = "https://res.cloudinary.com/ktvvajnj/image/upload/v1791219154/sample4.jpg",
                        ImagePublicId = "sample4",
                        SecondaryImages = new List<SecondaryImage>
                        {
                            new SecondaryImage { ImageUrl = "https://res.cloudinary.com/ktvvajnj/image/upload/v1791219214/sample4_sec1.jpg", ImagePublicId = "sample4_sec1" },
                            new SecondaryImage { ImageUrl = "https://res.cloudinary.com/ktvvajnj/image/upload/v1791219271/sample4_sec2.jpg", ImagePublicId = "sample4_sec2" },
                            new SecondaryImage { ImageUrl = "https://res.cloudinary.com/ktvvajnj/image/upload/v1791219321/sample4_sec3.jpg", ImagePublicId = "sample4_sec3" }
                        },
                        Description = "High-performance flagship device engineered with premium build materials, pro-grade optics, and long battery endurance for demanding workloads.",
                        Specifications = new Specifications
                        {
                            Model = "iPhone",
                            Warranty = "1 Year Comprehensive Brand Warranty",
                            Delivery = "Free Standard Shipping available"
                        }
                    }
                };


                context.Products.AddRange(products);
                await context.SaveChangesAsync();
            }

            // 3. Seed Admin User
            var adminEmail = "admin@megamart.com"; // admin email
            if (!await context.Users.AnyAsync(u => u.Email == adminEmail))
            {
                var adminUser = new User
                {
                    Name = "Admin User",
                    Email = adminEmail,
                    Role = "Admin"
                };

                // Hash the password securely so your login endpoint can verify it
                var passwordHasher = new PasswordHasher<User>();
                adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "Admin@123"); // admin password

                await context.Users.AddAsync(adminUser);
                await context.SaveChangesAsync();

            }
        }
    }

}