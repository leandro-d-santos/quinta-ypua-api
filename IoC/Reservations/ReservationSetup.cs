using Application.Reservations.Services;
using Domain.Common.Utils;
using Domain.Reservations.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace IoC.Reservations
{
    internal static class ReservationSetup
    {
        public static void AddReservations(this IServiceCollection services)
        {
            Check.ThrowIfNull(services, nameof(services));
            RegisterServices(services);
            RegisterRepositories(services);
        }

        private static void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<IReservationService, ReservationService>();
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddTransient<IReservationRepository, ReservationRepository>();
        }
    }
}