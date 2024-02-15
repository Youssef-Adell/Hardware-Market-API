using Core.Entities.UserAggregate;
using Core.Interfaces.IRepositories;
using Infrastructure.Repositories.EFConfig;

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

}
