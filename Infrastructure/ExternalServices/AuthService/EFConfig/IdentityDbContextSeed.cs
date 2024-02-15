using Core.DTOs.AuthDTOs;
using Core.Entities.ProductAggregate;
using Core.Entities.UserAggregate;
using Core.Interfaces.IExternalServices;
using Core.Interfaces.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ExternalServices.AuthService.EFConfig;

public static class IdentityDbContextSeed
{
    public static async Task SeedAsync(this IdentityDbContext identityDbContext, IAuthService authService)
    {
        if (!await identityDbContext.Roles.AnyAsync())
        {
            var list = new List<IdentityRole>{
                new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Name = "Customer", NormalizedName = "CUSTOMER" }
            };

            identityDbContext.Roles.AddRange(list);
            await identityDbContext.SaveChangesAsync();
        }

        if (!await identityDbContext.Users.AnyAsync())
        {
            //create admin at the first running of the api if there are no admins created
            var registerRequest = new RegisterRequest
            {
                UserName = "Admin",
                Email = Environment.GetEnvironmentVariable("ADMIN_EMAIL"),
                Password = Environment.GetEnvironmentVariable("ADMIN_PASSWORD")
            };

            await authService.Register(registerRequest, "Admin");
        }
    }

}
