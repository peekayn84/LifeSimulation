using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Threading.Tasks;

namespace Life.Core.Configuration
{
    public sealed class ConsoleConfigManager : IConfigManager<Config>
    {
        public const char KeyValueSeparator = '=';

        private readonly Dictionary<string, int> _parsedArgs;
        private readonly PropertyInfo[] _configProps;

        public Config? Configuration { get; private set; } = new Config();

        public ConsoleConfigManager(string[] args)
        {
            _configProps = typeof(Config).GetProperties();
            _parsedArgs = ParseArgs(args);
        }

        private static Dictionary<string, int> ParseArgs(string[] args)
        {
            var parsedArgs = new Dictionary<string, int>();
            foreach(string arg in args)
            {
                string[] pair = arg.Split(KeyValueSeparator);

                if(IsValidKeyValuePair(pair)) {
                    string key = pair[0];
                    int value = int.Parse(pair[1]);
                    if(parsedArgs.ContainsKey(key)) {
                        parsedArgs[key] = value;
                        continue;
                    }

                    parsedArgs.Add(key, value);
                }
            }

            return parsedArgs;
        }

        private static bool IsValidKeyValuePair(string[] pair) =>
            pair.Length == 2 && int.TryParse(pair[1], out int value);

        public Task<Config?> LoadConfig()
        {
            var config = new Config();
            foreach(var (key, value) in _parsedArgs)
            {
                var prop = _configProps.FirstOrDefault((p) => p.Name == key);

                if(prop != null) {
                    prop.SetValue(config, value);
                }
            }

            Configuration = config;

            return Task.FromResult<Config?>(config);
        }
    }
}