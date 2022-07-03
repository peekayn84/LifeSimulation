namespace Life.Core.Configuration
{
    public sealed class DefaultConfigManager : IConfigManager<Config>
    {

        public Config? Configuration { get; private set; } = new Config();


        public Config? LoadConfig()
        {
            return Configuration;
        }
    }
}
