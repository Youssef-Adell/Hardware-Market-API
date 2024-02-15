using Microsoft.AspNetCore.Identity;

namespace Infrastructure.ExternalServices.AuthService.EFConfig;

public class Account : IdentityUser
{
    public Account(string userName, string email, Guid ownerId)
    {
        this.UserName = userName;
        this.Email = email;
        this.OwnerId = ownerId;
    }

    public Guid OwnerId { get; set; } //the id of the busieness user (the account owner) that created while creating this IdentityUser and it will be returned in the token to be referenced as a forigen key by business entities (blogs, orders, ..)
    public string? RefreshToken { get; set; }
    // public DateTime? RefreshTokenExpiryTime { get; set; } //we gonna make the refresh token not expired until we expire it manually and it will be changed every time the user login in so if it stolen no problem because it wont last a long time
}