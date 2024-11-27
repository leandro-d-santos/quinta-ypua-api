using Api.Core.Setups;
using Configuration.Configuration;
using IoC;

namespace Api.Core
{
    public static class CreateWebApplication
    {
        public static WebApplication Create(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            builder.Services.AddCorsSettings();
            builder.Services.AddSwagger();
            builder.Services.AddMiddlewares();
            builder.Services.AddSingleton<IAppSettings, AppSettings>();
            Bootstrap.RegisterServices(builder.Services);
            return builder.Build();
        }
    }
}
