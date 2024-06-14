using Application.Users.ViewModels;

namespace Application.Users.Services
{
    public interface IUserService
    {
        Task<bool> CreateAsync(UserViewModel userViewModel);
        Task<IList<UserViewModel>> FindAllAsync();
        Task<UserViewModel?> FindByUserNameAsync(string userName);
        Task<bool> UpdateAsync(UserViewModel userViewModel);
        Task<bool> RemoveAsync(string userName);
    }
}