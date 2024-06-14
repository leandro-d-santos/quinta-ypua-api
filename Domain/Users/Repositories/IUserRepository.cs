using Domain.Users.Models;

namespace Domain.Users.Repositories
{
    public interface IUserRepository
    {
        void Add(User user);
        IList<User> FindAll();
        User? FindByUserName(string userName);
        void Update(User user);
        void Remove(User user);
    }
}