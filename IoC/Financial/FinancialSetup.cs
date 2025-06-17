using Application.Financial.Services;
using Domain.Common.Utils;
using Domain.Financial.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace IoC.Financial
{
    internal static class FinancialSetup
    {
        public static void AddFinancial(this IServiceCollection services)
        {
            Check.ThrowIfNull(services, nameof(services));
            RegisterServices(services);
            RegisterRepositories(services);
        }

        private static void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<IFinancialService, FinancialService>();
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddTransient<IFinancialRepository, FinancialRepository>();
        }
    }
}