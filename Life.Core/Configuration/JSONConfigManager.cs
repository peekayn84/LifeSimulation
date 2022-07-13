using System.Diagnostics;
using System.Text.Json;

namespace Life.Core.Configuration
{
    public sealed class JSONConfigManager : IConfigManager<Config>
    {
        private readonly string _filename;
        private static readonly JsonSerializerOptions s_options = new JsonSerializerOptions()
        {
            WriteIndented = true,
        };

        public Config? Configuration { get; private set; } = new Config();

        public JSONConfigManager(string filename)
        {
            _filename = filename;
        }

        public Config? LoadConfig() 
        {
            if(!File.Exists(_filename)) 
            {
                using var newFileFs = File.Create(_filename);
                JsonSerializer.Serialize(newFileFs, Configuration, s_options);
                return Configuration;
            }

            using var fs = new FileStream(
                _filename, 
                FileMode.Open, 
                FileAccess.Read, 
                FileShare.Read);

            Configuration = JsonSerializer.Deserialize<Config>(fs);

            return Configuration;
        }
    }
}