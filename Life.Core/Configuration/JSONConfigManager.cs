using System.Text.Json;

namespace Life.Core.Configuration
{
    public sealed class JSONConfigManager : IConfigManager<Config>
    {
        private readonly string _filename;

        public Config? Configuration { get; private set; } = new Config();

        public JSONConfigManager(string filename)
        {
            _filename = filename;
        }

        public async Task<Config?> LoadConfig() {
            if(!File.Exists(_filename)) {
                using var newFileFs = File.Create(_filename);
                JsonSerializer.Serialize(newFileFs, Configuration);
                return Configuration;
            }

            using var fs = new FileStream(_filename, FileMode.Open, FileAccess.Read, FileShare.Read);

            Configuration = await JsonSerializer.DeserializeAsync<Config>(fs);

            return Configuration;
        }
    }
}