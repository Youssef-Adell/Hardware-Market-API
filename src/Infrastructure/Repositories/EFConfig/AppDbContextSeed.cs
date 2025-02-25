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
            list.Add(new Brand() { Name = "Asus", LogoPath = "Images/Brands/asus.png" });
            list.Add(new Brand() { Name = "Dell", LogoPath = "Images/Brands/dell.png" });
            list.Add(new Brand() { Name = "Lenovo", LogoPath = "Images/Brands/lenovo.png" });
            list.Add(new Brand() { Name = "HP", LogoPath = "Images/Brands/hp.png" });
            list.Add(new Brand() { Name = "Logitech", LogoPath = "Images/Brands/logitech.png" });
            appDbContext.Brands.AddRange(list);
            await appDbContext.SaveChangesAsync();
        }
        if (!await appDbContext.Categories.AnyAsync())
        {
            var list = new List<Category>();
            list.Add(new Category() { Name = "Laptops", IconPath = "Images/Categories/laptops.png" });
            list.Add(new Category() { Name = "Monitors", IconPath = "Images/Categories/monitors.png" });
            list.Add(new Category() { Name = "Accessories", IconPath = "Images/Categories/accessories.png" });
            appDbContext.Categories.AddRange(list);
            await appDbContext.SaveChangesAsync();
        }

        if (!await appDbContext.Products.AnyAsync())
        {
            var brands = await appDbContext.Brands.ToDictionaryAsync(b => b.Name, b => b.Id);
            var categories = await appDbContext.Categories.ToDictionaryAsync(c => c.Name, c => c.Id);

            var list = new List<Product>();
            list.Add(new Product() { Name = "Asus Zenbook Pro 16", Images = new List<ProductImage> { new() { Path = "Images/Products/1.1.jpg" }, new() { Path = "Images/Products/1.2.jpg" } }, Description = @"ASUS Zenbook Pro 16 Laptop 16 165Hz Refresh Rate Display, Intel i7-12650H CPU, DialPad, NVidia GeForce RTX 3070 Ti Graphics, 32GB RAM, 1TB SSD, Windows 11 Home, Tech Black, UX6601ZW-DB76", Quantity = 12, Price = 35000, BrandId = brands["Asus"], CategoryId = categories["Laptops"] });
            list.Add(new Product() { Name = "Asus Zenbook Pro 14", Images = new List<ProductImage> { new() { Path = "Images/Products/2.1.jpg" }, new() { Path = "Images/Products/2.2.jpg" } }, Description = @"ASUS Zenbook Pro 14 OLED 14.5â€ OLED 16:10 Touch Display, DialPad, Intel i9-13900H CPU, GeForce RTX 4070 Graphics, 32GB RAM, 1TB SSD, Windows 11 Home, Tech Black, UX6404VI-DS96T", Quantity = 25, Price = 55800, BrandId = brands["Asus"], CategoryId = categories["Laptops"] });
            list.Add(new Product() { Name = "Asus Vivobook M3704Y", Images = new List<ProductImage> { new() { Path = "Images/Products/3.1.jpg" }, new() { Path = "Images/Products/3.2.jpg" } }, Description = @"ASUS Vivobook 17.3 FHD (1920x1080) IPS Laptop 2023 | AMD Ryzen 7 7730U 8-Core | AMD Radeon Graphics | Backlit Keyboard | Fingerprint | Wi-Fi | USB-C | 40GB DDR4 2TB SSD | Win11 Pro , Indie Black", Quantity = 9, Price = 42000, BrandId = brands["Asus"], CategoryId = categories["Laptops"] });
            list.Add(new Product() { Name = "ASUS TUF Gaming F15", Images = new List<ProductImage> { new() { Path = "Images/Products/4.1.jpg" }, new() { Path = "Images/Products/4.2.jpg" } }, Description = @"ASUS TUF Gaming F15 (2022) Gaming Laptop, 15.6” FHD 144Hz Display, GeForce RTX 3050 & TUF Gaming F15 Gaming Laptop, 15.6” 144Hz FHD Display, Intel Core i5-11400H Processor", Quantity = 3, Price = 19.99, BrandId = brands["Asus"], CategoryId = categories["Laptops"] });
            list.Add(new Product() { Name = "ASUS C433", Images = new List<ProductImage> { new() { Path = "Images/Products/5.1.jpg" }, new() { Path = "Images/Products/5.2.jpg" } }, Description = @"ASUS Flip 2-in-1 15.6 FHD Touchscreen Chromebook Laptop, Intel Core i3-1115G4(Up to 4.1GHz), 8GB DDR4 RAM, 128GB SSD, Backlit Keyboard, WiFi 6, USB Type C, Chrome OS, White", Quantity = 7, Price = 18000, BrandId = brands["Asus"], CategoryId = categories["Laptops"] });

            list.Add(new Product() { Name = "Dell Inspiron 14", Images = new List<ProductImage> { new() { Path = "Images/Products/6.1.jpg" }, new() { Path = "Images/Products/6.2.jpg" } }, Description = @"Dell 2023 Newest Inspiron 14 Laptop, 14 FHD(1920 x 1200) Display, AMD Ryzen 7 5825U Processor(8-core), 16GB RAM, 512GB SSD, AMD Radeon Graphics, Wi-Fi 6, Bluetooth, Window 11 Pro, Pebble Green", Quantity = 15, Price = 61500, BrandId = brands["Dell"], CategoryId = categories["Laptops"] });
            list.Add(new Product() { Name = "Dell Latitude 7000 15.6", Images = new List<ProductImage> { new() { Path = "Images/Products/7.1.jpg" }, new() { Path = "Images/Products/17.2.jpg" } }, Description = @"Dell Latitude 7000 7530 15.6 Notebook - Full HD - 1920 x 1080 - Intel Core i7 12th Gen i7-1270P Dodeca-core (12 Core) 2.20 GHz - 16 GB Total RAM - 16 GB On-Board Memory - 512 GB SSD - Carbon Fiber -", Quantity = 11, Price = 55250, BrandId = brands["Dell"], CategoryId = categories["Laptops"] });
            list.Add(new Product() { Name = "Dell Inspiron 5000", Images = new List<ProductImage> { new() { Path = "Images/Products/8.1.jpg" }, new() { Path = "Images/Products/8.2.jpg" } }, Description = @"Dell Inspiron 2022 | 16 FHD+ (1920x1200) Anti-Glare | 12th Intel Core i7-1255U 10-Core | 16GB DDR4 1TB NVMe SSD Iris Xe Graphics | Backlit Keyboard Fingerprint Reader WiFi 6E | Win 11 Home", Quantity = 20, Price = 73000, BrandId = brands["Dell"], CategoryId = categories["Laptops"] });

            list.Add(new Product() { Name = "Lenovo IdeaPad 3i 14", Images = new List<ProductImage> { new() { Path = "Images/Products/9.1.jpg" }, new() { Path = "Images/Products/9.2.jpg" } }, Description = @"Lenovo IdeaPad 3i 14 Laptop, Student and Business, 14 FHD Screen, Intel i3-1115G4 Processor, 20GB RAM, 1TB SSD, HDMI, Media Card Reader, Webcam, Dolby Audio, Wi-Fi 6, Windows 11 Home, Platinum Grey", Quantity = 17, Price = 39700, BrandId = brands["Lenovo"], CategoryId = categories["Laptops"] });
            list.Add(new Product() { Name = "Lenovo ThinkPad E16 Gen 1", Images = new List<ProductImage> { new() { Path = "Images/Products/10.1.jpg" }, new() { Path = "Images/Products/10.2.jpg" } }, Description = "Lenovo ThinkPad E16 Gen 1 21JN0073US 16 Notebook - WUXGA - 1920 x 1200 - Intel Core i7 13th Gen i7-1355U Deca-core (10 Core) 1.70 GHz - 16 GB Total RAM - 8 GB On-Board Memory - 512 GB SSD - Graphite", Quantity = 15, Price = 64400, BrandId = brands["Lenovo"], CategoryId = categories["Laptops"] });
            list.Add(new Product() { Name = "Lenovo C340 i3 Chromebook", Images = new List<ProductImage> { new() { Path = "Images/Products/11.1.jpg" }, new() { Path = "Images/Products/11.2.jpg" } }, Description = @"lenovo 2022 Newest C340 15.6 FHD Touchscreen 2-in-1 Chromebook Laptop, Intel i3 CPU(Up to 3.4GHz), 4GB RAM, 64GB eMMC, USB-C, Wi-Fi, Bluetooth, Webcam, Chrome OS", Quantity = 17, Price = 39000, BrandId = brands["Lenovo"], CategoryId = categories["Laptops"] });
            list.Add(new Product() { Name = "Lenovo V15 Gen 4", Images = new List<ProductImage> { new() { Path = "Images/Products/12.1.jpg" }, new() { Path = "Images/Products/12.2.jpg" } }, Description = @"Lenovo V15 Gen 4 Business Laptop, 15.6 FHD Display, AMD Ryzen 3 7320U, 16GB RAM, 1TB PCIe SSD, Webcam, Type-C, HDMI, RJ45, Wi-Fi, Windows 11 Home, Black FHD Screen, Intel i3-1115G4 Processor, 20GB RAM, 1TB SSD, HDMI, Media Card Reader, Webcam, Dolby Audio, Wi-Fi 6, Windows 11 Home, Platinum Grey", Quantity = 17, Price = 43000, BrandId = brands["Lenovo"], CategoryId = categories["Laptops"] });

            list.Add(new Product() { Name = "HP Victus 15.6", Images = new List<ProductImage> { new() { Path = "Images/Products/13.1.jpg" }, new() { Path = "Images/Products/13.2.jpg" } }, Description = @"HP Victus 15.6 Full HD 144Hz Gaming Laptop | AMD Ryzen 5 7535HS | NVIDIA GeForce RTX 2050 | 8GB RAM DDR5| 512GB SSD | Backlit | Windows 11 Home | Bundle with 64GB USB Flash Drive, Mica Silver", Quantity = 12, Price = 47299, BrandId = brands["HP"], CategoryId = categories["Laptops"] });
            list.Add(new Product() { Name = "HP ZBook Power G9", Images = new List<ProductImage> { new() { Path = "Images/Products/14.1.jpg" }, new() { Path = "Images/Products/14.2.jpg" } }, Description = @"HP ZBook Power G9 Business Mobile Workstation Laptop, 15.6 FHD Display, Intel Core i7-12700H, NVIDIA RTX A1000, 32GB DDR5 RAM, 1TB PCIe SSD, HDMI, Webcam, Wi-Fi 6, Windows 11 Pro, Grey", Quantity = 17, Price = 39000, BrandId = brands["HP"], CategoryId = categories["Laptops"] });


            list.Add(new Product() { Name = "ASUS ProArt Display PA278QV 27", Images = new List<ProductImage> { new() { Path = "Images/Products/15.1.jpg" }, new() { Path = "Images/Products/15.2.jpg" } }, Description = @"ASUS ProArt Display PA278QV 27 WQHD (2560 x 1440) Monitor, 100% sRGB/Rec. 709 ΔE < 2, IPS, DisplayPort HDMI DVI-D Mini DP, Calman Verified, Eye Care, Anti-glare, Tilt Pivot Swivel Height Adjustable", Quantity = 12, Price = 3999.99, BrandId = brands["Asus"], CategoryId = categories["Monitors"] });
            list.Add(new Product() { Name = "ASUS TUF Gaming Display 23.6", Images = new List<ProductImage> { new() { Path = "Images/Products/16.1.jpg" }, new() { Path = "Images/Products/16.2.jpg" } }, Description = @"ASUS TUF Gaming 23.6 Widescreen VA LED Black Multimedia Curved Monitor (1920x1080/1ms/2xHDMI/DP)", Quantity = 5, Price = 20842, BrandId = brands["Asus"], CategoryId = categories["Monitors"] });
            list.Add(new Product() { Name = "Lenovo monitor Legion R27q-30", Images = new List<ProductImage> { new() { Path = "Images/Products/17.1.jpg" }, new() { Path = "Images/Products/17.2.jpg" } }, Description = @"Lenovo monitor Legion R27q-30 Size 27 Resolution 2560x1440 Refresh Rate 165Hz Aspect Ratio 16:9 Raven Black", Quantity = 9, Price = 18560, BrandId = brands["Lenovo"], CategoryId = categories["Monitors"] });
            list.Add(new Product() { Name = "Lenovo Gaming monitor G27c-30", Images = new List<ProductImage> { new() { Path = "Images/Products/1817.1.jpg" }, new() { Path = "Images/Products/18.2.jpg" } }, Description = @"Lenovo Gaming monitor G27c-30 Display Size 27 Resolution 1920x1080 Response Time 7ms (Level 1) / 6ms (Level 2) / 5ms (Level 3) / 4ms (Level 4) / 1ms (MPRT) Refresh Rate 165Hz", Quantity = 7, Price = 49.99, BrandId = brands["Lenovo"], CategoryId = categories["Monitors"] });


            list.Add(new Product() { Name = "Logitech MK850 Performance Wireless Keyboard and Mouse Combo", Images = new List<ProductImage> { new() { Path = "Images/Products/19.1.jpg" }, new() { Path = "Images/Products/19.2.jpg" } }, Description = @"Usb wireless Bluetooth/RF USB wireless Bluetooth/RF Optical - 1000 dpi - 8 button - scroll wheel - AAA, AA - Compatible with desktop computer, smartphone, notebook, tablet (Chrome OS, Windows, Android, Mac, iOS)", Quantity = 25, Price = 1200, BrandId = brands["Logitech"], CategoryId = categories["Accessories"] });
            list.Add(new Product() { Name = "Logitech MX Master 3S - Wireless Performance Mouse", Images = new List<ProductImage> { new() { Path = "Images/Products/20.1.jpg" }, new() { Path = "Images/Products/20.2.jpg" } }, Description = @"Logitech MX Master 3S - Wireless Performance Mouse, Ergo, 8K DPI, Track on Glass, Quiet Clicks, USB-C, Bluetooth, Windows, Linux, Chrome - Graphite - With Free Adobe Creative Cloud Subscription", Quantity = 35, Price = 960, BrandId = brands["Logitech"], CategoryId = categories["Accessories"] });
            list.Add(new Product() { Name = "Logitech G332 SE Stereo Gaming Headset", Images = new List<ProductImage> { new() { Path = "Images/Products/21.1.jpg" }, new() { Path = "Images/Products/21.2.jpg" } }, Description = @"Logitech G332 SE Stereo Gaming Headset for PC, PS4, Xbox One, Nintendo Switch", Quantity = 27, Price = 699, BrandId = brands["Logitech"], CategoryId = categories["Accessories"] });
            list.Add(new Product() { Name = "Logitech G Extreme 3D Pro USB Joystick", Images = new List<ProductImage> { new() { Path = "Images/Products/22.1.jpg" }, new() { Path = "Images/Products/22.2.jpg" } }, Description = @"Logitech Exreme 3D Pro Joystick, Take Control: With advanced controls and a custom twist-handle rudder, this joystick is stable and precise whether you’re dropping bombs or firing guns", Quantity = 21, Price = 1699, BrandId = brands["Logitech"], CategoryId = categories["Accessories"] });

            appDbContext.Products.AddRange(list);
            await appDbContext.SaveChangesAsync();
        }
    }
}