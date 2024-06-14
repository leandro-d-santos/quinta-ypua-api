namespace Configuration.Configuration
{
    public sealed class AppSettings : IAppSettings
    {
        public bool IsDebug => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

        public string? ConnectionStringName => Environment.GetEnvironmentVariable("CONNECTION_STRING_NAME");
    }
}