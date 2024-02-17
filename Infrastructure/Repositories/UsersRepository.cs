using Core.Entities.UserAggregate;
using Core.Interfaces.IRepositories;
using Infrastructure.Repositories.EFConfig;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly AppDbContext appDbContext;

    public UsersRepository(AppDbContext appDbContext)
    {
        this.appDbContext = appDbContext;
    }

    public void AddUser(User user)
    {
        appDbContext.Users.Add(user);
    }

    public async Task<User?> GetUser(Guid id)
    {
        return await appDbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
    }
}
