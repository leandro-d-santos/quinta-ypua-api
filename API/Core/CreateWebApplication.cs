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
            Bootstrap.RegisterServices(builder.Services);
            builder.Services.AddControllers();
            builder.Services.AddCorsSettings();
            builder.Services.AddSwagger();
            builder.Services.AddMiddlewares();
            builder.Services.AddSingleton<IAppSettings, AppSettings>();
            return builder.Build();
        }
    }
}
