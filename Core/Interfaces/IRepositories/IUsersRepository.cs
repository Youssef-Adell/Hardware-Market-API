using Core.Entities.UserAggregate;

namespace Core.Interfaces.IRepositories;

public interface IUsersRepository
{
    void AddUser(User user);
    Task<User?> GetUser(Guid id);
}