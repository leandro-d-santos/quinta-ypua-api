using Configuration.Middlewares;

namespace Api.Core.Setups
{
    public static class MiddlewaresSetup
    {
        public static void AddMiddlewares(this IServiceCollection services)
        {
            services.AddTransient<ErrorHandlingMiddleware>();
        }

        public static void UseMiddlewares(this WebApplication app)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}