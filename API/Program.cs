using Api.Core;
using Api.Core.Setups;

namespace Api
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var app = CreateWebApplication.Create(args);
            app.Setup();
            app.Run();
        }
    }
}