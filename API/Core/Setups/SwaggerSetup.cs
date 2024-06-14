namespace Api.Core.Setups
{
    public static class SwaggerSetup
    {
        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(configuration =>
            {
                configuration.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo() { Title = "Quinta do Ypuã v1", Version = "v1" });
            });
        }

        public static void AddSwaggerUI(this WebApplication app)
        {
            app.UseSwagger(options =>
            {
                options.RouteTemplate = "api/swagger/{documentname}/swagger.json";
            });
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/api/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = "api/swagger";
            });
        }
    }
}