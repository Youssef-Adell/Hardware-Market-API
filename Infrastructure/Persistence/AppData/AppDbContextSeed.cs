using Core.Entities.ProductAggregate;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.AppData;

public class AppDbContextSeed
{
    public static async Task SeedAsync(AppDbContext appDbContext)
    {
            if (!await appDbContext.ProductBrands.AnyAsync())
            {
                var list = new List<ProductBrand>();
                list.Add(new ProductBrand() {  Name = "Asus", LogoUrl = "brands/asus.png" });
                list.Add(new ProductBrand() { Name = "Dell", LogoUrl = "brands/dell.png" });
                list.Add(new ProductBrand() { Name = "HP", LogoUrl = "brands/hp.png" });
                list.Add(new ProductBrand() { Name = "Logitech", LogoUrl = "brands/logitech.png" });
                appDbContext.ProductBrands.AddRange(list);
                await appDbContext.SaveChangesAsync();
            }
            if (!await appDbContext.ProductCategories.AnyAsync())
            {
                var list = new List<ProductCategory>();
                list.Add(new ProductCategory() { Name = "Laptops", IconUrl = "categories/laptops.png" });
                list.Add(new ProductCategory() { Name = "PC Bundles", IconUrl = "categories/pc-bundles.png" });
                list.Add(new ProductCategory() { Name = "Accessories", IconUrl = "categories/accessories.png" });
                appDbContext.ProductCategories.AddRange(list);
                await appDbContext.SaveChangesAsync();
            }
            if (!await appDbContext.Products.AnyAsync())
            {
                var list = new List<Product>();
                list.Add(new Product() { Name = "Asus Vivobook 16x", ImageUrls = new List<string>() { "products/vivobook1.png", "products/vivobook2.png" }, Description = "Description 1", Quantity = 3, Price = 9.99, BrandId = 1, CategoryId = 1 });
                list.Add(new Product() { Name = "Asus Tuf Gaming", ImageUrls = new List<string>() { "products/tuf1.png", "products/tuf2.png" }, Description = "Description 3", Quantity = 2, Price = 29.99, BrandId = 1, CategoryId = 1 });
                list.Add(new Product() { Name = "Asus Rog", ImageUrls = new List<string>() { "products/rog1.png", "products/rog2.png" }, Description = "Description 3", Quantity = 2, Price = 29.99, BrandId = 1, CategoryId = 1 });
                list.Add(new Product() { Name = "Dell Vostoro 3500", ImageUrls = new List<string>() { "products/vostoro1.png", "products/vostoro2.png" }, Description = "Description 2", Quantity = 3, Price = 19.99, BrandId = 2, CategoryId = 1 });
                list.Add(new Product() { Name = "Dell Latitude", ImageUrls = new List<string>() { "products/latitiude1.png", "products/latitude2.png" }, Description = "Description 2", Quantity = 3, Price = 19.99, BrandId = 2, CategoryId = 1 });
                list.Add(new Product() { Name = "HP Envy", ImageUrls = new List<string>() { "products/envy1.png", "products/envy2.png" }, Description = "Description 4", Quantity = 5, Price = 39.99, BrandId = 3, CategoryId = 1 });

                list.Add(new Product() { Name = "Ghost Bundle", ImageUrls = new List<string>() { "products/ghost1.png", "products/ghost2.png" }, Description = "Description 1", Quantity = 12, Price = 409.99, BrandId = null, CategoryId = 2 });
                list.Add(new Product() { Name = "Gamers Bundle", ImageUrls = new List<string>() { "products/gamers1.png", "products/gamers2.png" }, Description = "Description 1", Quantity = 5, Price = 49.99, BrandId = null, CategoryId = 2 });
                list.Add(new Product() { Name = "X Bundle", ImageUrls = new List<string>() { "products/hero1.png", "products/hero2.png" }, Description = "Description 1", Quantity = 7, Price = 49.99, BrandId = null, CategoryId = 2 });
                list.Add(new Product() { Name = "Ninja Bundle", ImageUrls = new List<string>() { "products/ninja1.png", "products/ninja2.png" }, Description = "Description 1", Quantity = 1, Price = 49.99, BrandId = null, CategoryId = 2 });
                
                list.Add(new Product() { Name = "Logictech RGB Keyboard", ImageUrls = new List<string>() { "products/logi-keyboard1.png", "products/logi-keyboard2.png" }, Description = "Description 1", Quantity = 1, Price = 49.99, BrandId = 4, CategoryId = 3 });
                list.Add(new Product() { Name = "Logictech RGB Mouse", ImageUrls = new List<string>() { "products/logi-mouse1.png", "products/logi-mouse2.png" }, Description = "Description 1", Quantity = 1, Price = 49.99, BrandId = 4, CategoryId = 3 });
                
                appDbContext.Products.AddRange(list);
                await appDbContext.SaveChangesAsync();
            }
            if (!await appDbContext.ProductReveiews.AnyAsync())
            {
            var list = new List<ProductReview>();
                list.Add(new ProductReview() { ProductId = 1, CustomerEmail = "customer1@example.com", Stars = 4, Comment = "Great product!" });
                list.Add(new ProductReview() { ProductId = 1, CustomerEmail = "customer2@example.com", Stars = 5, Comment = "Excellent quality!" });
                list.Add(new ProductReview() { ProductId = 1, CustomerEmail = "customer3@example.com", Stars = 3, Comment = "Could be better." });
                list.Add(new ProductReview() { ProductId = 1, CustomerEmail = "customer4@example.com", Stars = 2, Comment = "Not satisfied." });
                list.Add(new ProductReview() { ProductId = 2, CustomerEmail = "customer5@example.com", Stars = 5, Comment = "Love it!" });
                list.Add(new ProductReview() { ProductId = 2, CustomerEmail = "customer6@example.com", Stars = 4, Comment = "Good value for money." });
                list.Add(new ProductReview() { ProductId = 2, CustomerEmail = "customer7@example.com", Stars = 1, Comment = "Terrible product." });
                appDbContext.ProductReveiews.AddRange(list);
                await appDbContext.SaveChangesAsync();
            }
    }
}
