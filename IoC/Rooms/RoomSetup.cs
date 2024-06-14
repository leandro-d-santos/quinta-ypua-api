using Application.Rooms.Services;
using Domain.Common.Utils;
using Domain.Rooms.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace IoC.Rooms
{
    internal static class RoomSetup
    {
        public static void AddRooms(this IServiceCollection services)
        {
            Check.ThrowIfNull(services, nameof(services));
            RegisterServices(services);
            RegisterRepositories(services);
        }

        private static void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<IRoomService, RoomService>();
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddTransient<IRoomRepository, RoomRepository>();
        }
    }
}