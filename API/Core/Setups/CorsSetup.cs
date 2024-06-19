namespace Api.Core.Setups
{
    public static class CorsSetup
    {
        private static readonly string CorsKey = "MyApplicationPolicy";
        public static void AddCorsSettings(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(CorsKey, policy =>
                {
                    policy.AllowAnyOrigin();
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                });
            });
        }

        public static void UseCorsSettings(this WebApplication app)
        {
            app.UseCors(CorsKey);
        }

    }
}
