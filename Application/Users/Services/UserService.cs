using Application.Users.ViewModels;
using Domain.Common.Data;
using Domain.Users.Models;
using Domain.Users.Repositories;

namespace Application.Users.Services
{
    public sealed class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IUnitOfWork unitOfWork;

        public UserService(IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<bool> CreateAsync(UserViewModel userViewModel)
        {
            User user = TransformToModel(userViewModel);
            userRepository.Add(user);
            await unitOfWork.Complete();
            return true;
        }

        public Task<IList<UserViewModel>> FindAllAsync()
        {
            IList<User> users = userRepository.FindAll();
            IList<UserViewModel> usersViewModel = users.Select(TransformToViewModel).ToList();
            return Task.FromResult(usersViewModel);
        }

        public Task<UserViewModel?> FindByUserNameAsync(string userName)
        {
            User? user = userRepository.FindByUserName(userName);
            if (user is null)
            {
                return Task.FromResult<UserViewModel?>(null);
            }
            return Task.FromResult<UserViewModel?>(TransformToViewModel(user));
        }

        public async Task<bool> UpdateAsync(UserViewModel userViewModel)
        {
            User? user = userRepository.FindByUserName(userViewModel.UserName);
            if (user is null)
            {
                throw new ArgumentException("Usuario não encontrado.");
            }
            user.Name = userViewModel.Name;
            user.Email = userViewModel.Email;
            userRepository.Update(user);
            await unitOfWork.Complete();
            return true;
        }

        public async Task<bool> RemoveAsync(string userName)
        {
            User? user = userRepository.FindByUserName(userName);
            if (user is null)
            {
                throw new ArgumentException("Usuario não encontrado.");
            }
            userRepository.Remove(user);
            await unitOfWork.Complete();
            return true;
        }

        private static User TransformToModel(UserViewModel user)
        {
            return new User()
            {
                UserName = user.UserName,
                Name = user.Name,
                Email = user.Email,
                Password = user.Password
            };
        }

        private static UserViewModel TransformToViewModel(User user)
        {
            return new UserViewModel()
            {
                UserName = user.UserName,
                Name = user.Name,
                Email = user.Email
            };
        }
    }
}