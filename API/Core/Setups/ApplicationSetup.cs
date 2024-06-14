using Microsoft.AspNetCore.Builder;

namespace Api.Core.Setups
{
    public static class ApplicationSetup
    {
        public static void Setup(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.AddSwaggerUI();
            }
            app.UseAuthorization();
            app.UseMiddlewares();
            app.MapControllers();
        }
    }
}
