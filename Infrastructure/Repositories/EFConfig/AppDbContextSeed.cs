using Core.Entities.ProductAggregate;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.EFConfig;

public static class AppDbContextSeed
{
    public static async Task SeedAsync(this AppDbContext appDbContext)
    {
        if (!await appDbContext.Brands.AnyAsync())
        {
            var list = new List<Brand>();
            list.Add(new Brand() { Name = "Asus", LogoPath = "brands/asus.png" });
            list.Add(new Brand() { Name = "Dell", LogoPath = "brands/dell.png" });
            list.Add(new Brand() { Name = "HP", LogoPath = "brands/hp.png" });
            list.Add(new Brand() { Name = "Logitech", LogoPath = "brands/logitech.png" });
            appDbContext.Brands.AddRange(list);
            await appDbContext.SaveChangesAsync();
        }
        if (!await appDbContext.Categories.AnyAsync())
        {
            var list = new List<Category>();
            list.Add(new Category() { Name = "Laptops", IconPath = "categories/laptops.png" });
            list.Add(new Category() { Name = "Monitors", IconPath = "categories/monitors.png" });
            list.Add(new Category() { Name = "Accessories", IconPath = "categories/accessories.png" });
            appDbContext.Categories.AddRange(list);
            await appDbContext.SaveChangesAsync();
        }
        // if (!await appDbContext.Products.AnyAsync())
        // {
        //     var list = new List<Product>();
        //     list.Add(new Product() { Name = "Asus Vivobook 16x", Images = new List<ProductImage> { new ProductImage { Path = "Images/Products/1ad24e12-db6f-40c5-bb83-bc823c502077.jpg" } }, Description = "Description 1", Quantity = 3, Price = 9.99, BrandId = 1, CategoryId = 1 });
        //     list.Add(new Product() { Name = "Asus Tuf Gaming", Images = new List<ProductImage> { new ProductImage { Path = "Images/Products/1ad24e12-db6f-40c5-bb83-bc823c502077.jpg" } }, Description = "Description 3", Quantity = 2, Price = 29.99, BrandId = 1, CategoryId = 1 });
        //     list.Add(new Product() { Name = "Asus Rog", Images = new List<ProductImage> { new ProductImage { Path = "Images/Products/1ad24e12-db6f-40c5-bb83-bc823c502077.jpg" } }, Description = "Description 3", Quantity = 2, Price = 29.99, BrandId = 1, CategoryId = 1 });
        //     list.Add(new Product() { Name = "Dell Vostoro 3500", Images = new List<ProductImage> { new ProductImage { Path = "Images/Products/1ad24e12-db6f-40c5-bb83-bc823c502077.jpg" } }, Description = "Description 2", Quantity = 3, Price = 19.99, BrandId = 2, CategoryId = 1 });
        //     list.Add(new Product() { Name = "Dell Latitude", Images = new List<ProductImage> { new ProductImage { Path = "Images/Products/1ad24e12-db6f-40c5-bb83-bc823c502077.jpg" } }, Description = "Description 2", Quantity = 3, Price = 19.99, BrandId = 2, CategoryId = 1 });
        //     list.Add(new Product() { Name = "HP Envy", Images = new List<ProductImage> { new ProductImage { Path = "Images/Products/1ad24e12-db6f-40c5-bb83-bc823c502077.jpg" } }, Description = "Description 4", Quantity = 5, Price = 39.99, BrandId = 3, CategoryId = 1 });

        //     list.Add(new Product() { Name = "Asus Monitor", Images = new List<ProductImage> { new ProductImage { Path = "Images/Products/1ad24e12-db6f-40c5-bb83-bc823c502077.jpg" } }, Description = "Description 1", Quantity = 12, Price = 409.99, BrandId = 1, CategoryId = 2 });
        //     list.Add(new Product() { Name = "Dell Monitor", Images = new List<ProductImage> { new ProductImage { Path = "Images/Products/1ad24e12-db6f-40c5-bb83-bc823c502077.jpg" } }, Description = "Description 1", Quantity = 5, Price = 49.99, BrandId = 2, CategoryId = 2 });
        //     list.Add(new Product() { Name = "HP Monitor", Images = new List<ProductImage> { new ProductImage { Path = "Images/Products/1ad24e12-db6f-40c5-bb83-bc823c502077.jpg" } }, Description = "Description 1", Quantity = 7, Price = 49.99, BrandId = 3, CategoryId = 2 });

        //     list.Add(new Product() { Name = "Logictech RGB Keyboard", Images = new List<ProductImage> { new ProductImage { Path = "Images/Products/1ad24e12-db6f-40c5-bb83-bc823c502077.jpg" } }, Description = "Description 1", Quantity = 1, Price = 49.99, BrandId = 4, CategoryId = 3 });
        //     list.Add(new Product() { Name = "Logictech RGB Mouse", Images = new List<ProductImage> { new ProductImage { Path = "Images/Products/1ad24e12-db6f-40c5-bb83-bc823c502077.jpg" } }, Description = "Description 1", Quantity = 1, Price = 49.99, BrandId = 4, CategoryId = 3 });

        //     appDbContext.Products.AddRange(list);
        //     await appDbContext.SaveChangesAsync();
        // }
        // if (!await appDbContext.ProductReviews.AnyAsync())
        // {
        //     var list = new List<ProductReview>();
        //     list.Add(new ProductReview() { ProductId = 33, CustomerEmail = "customer1@example.com", Rating = 4, Comment = "Great product!" });
        //     list.Add(new ProductReview() { ProductId = 33, CustomerEmail = "customer2@example.com", Rating = 5, Comment = "Excellent quality!" });
        //     list.Add(new ProductReview() { ProductId = 33, CustomerEmail = "customer3@example.com", Rating = 3, Comment = "Could be better." });
        //     list.Add(new ProductReview() { ProductId = 33, CustomerEmail = "customer4@example.com", Rating = 2, Comment = "Not satisfied." });
        //     list.Add(new ProductReview() { ProductId = 34, CustomerEmail = "customer5@example.com", Rating = 5, Comment = "Love it!" });
        //     list.Add(new ProductReview() { ProductId = 34, CustomerEmail = "customer6@example.com", Rating = 4, Comment = "Good value for money." });
        //     list.Add(new ProductReview() { ProductId = 34, CustomerEmail = "customer7@example.com", Rating = 1, Comment = "Terrible product." });
        //     appDbContext.ProductReviews.AddRange(list);
        //     await appDbContext.SaveChangesAsync();
        // }
    }
}
