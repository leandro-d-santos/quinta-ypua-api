using Application.Guests.Services;
using Domain.Common.Utils;
using Domain.Guests.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace IoC.Guests
{
    internal static class GuestSetup
    {
        public static void AddGuests(this IServiceCollection services)
        {
            Check.ThrowIfNull(services, nameof(services));
            RegisterServices(services);
            RegisterRepositories(services);
        }

        private static void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<IGuestService, GuestService>();
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddTransient<IGuestRepository, GuestRepository>();
        }
    }
}