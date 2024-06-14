namespace Configuration.Configuration
{
    public interface IAppSettings
    {
        public bool IsDebug { get; }

        public string? ConnectionStringName { get; }

    }
}