using System.IO;
using System.Text.Json;

namespace Life.Core.Configuration
{
    public sealed class JSONConfigManager : IConfigManager<Config>
    {
        private readonly string _filename;

        public Config Configuration { get; private set; } = new Config();

        public JSONConfigManager(string filename)
        {
            _filename = filename;
        }

        public async Task<Config> LoadConfig() {
            using var fs = new FileStream(_filename, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);

            Configuration = await JsonSerializer.DeserializeAsync<Config>(fs);

            return Configuration;
        }
    }
}