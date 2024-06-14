using Application.Users.Services;
using Domain.Common.Utils;
using Domain.Users.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace IoC.Users
{
    internal static class UserSetup
    {
        public static void AddUsers(this IServiceCollection services)
        {
            Check.ThrowIfNull(services, nameof(services));
            RegisterServices(services);
            RegisterRepositories(services);
        }

        private static void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddTransient<IUserRepository, UserRepository>();
        }
    }
}