using Life.Core.Configuration;

namespace Life.AutoTest.Parsing
{
    internal sealed class ParamsParser
    {
        private const char keyValueSeparator = '=';
        private const string paramPropName = "Param";

        private static readonly Type paramsObjType = typeof(Params);
        private static readonly List<string> validParamNames = typeof(Config)
            .GetProperties()
            .Select((p) => p.Name)
            .ToList();
        private static readonly Dictionary<string, Type> validParams = paramsObjType
            .GetProperties()
            .ToDictionary((p) => p.Name, (p) => p.PropertyType);
        

        public Params Params { get; private set; }

        public ParamsParser(string[] args)
        {
            Params = Parse(args);
        }

        public void OverrideConfigProp(Config config)
        {
            config.GetType().GetProperty(Params.Param)?.SetValue(config, Params.From);
        }

        public static Params Parse(string[] args)
        {
            string[][] splittedParams = args.Select((arg) => arg.Split(keyValueSeparator)).ToArray();

            ValidateSplittedParams(splittedParams);

            return BuildParamsObj(splittedParams);
        }

        private static void ValidateSplittedParams(string[][] splittedParams)
        {
            foreach (string[] pair in splittedParams)
            {
                if (pair.Length != 2)
                {
                    throw new Exception($"Invalid parameter: \"{string.Join(keyValueSeparator, pair)}\"");
                }

                string key = pair[0];
                string value = pair[1];

                if (!validParams.ContainsKey(key))
                {
                    throw new Exception($"Invalid parameter: \"{string.Join(keyValueSeparator, pair)}\"");
                }

                if(key == paramPropName && !validParamNames.Contains(value))
                {
                    throw new Exception($"Invalid param name: \"{value}\".\nValid param names:\n{Formatter.MakeConsoleTable(validParamNames, 4)}");
                }

                var valueType = validParams[key];
                var convertedValue = Convert.ChangeType(value, valueType);

                if (convertedValue == null)
                {
                    throw new Exception("Invlid value type for param: \"{key}\"");
                }

                if(int.TryParse(value, out int test) && test < 0)
                {
                    throw new Exception($"Integer values must be greater than 0. Param: \"{key}\"");
                }
            }
        }

        private static Params BuildParamsObj(string[][] splittedParams)
        {
            object result = new Params();

            foreach (string[] pair in splittedParams)
            {
                string key = pair[0];
                string value = pair[1];

                var prop = paramsObjType.GetProperty(key);
                var valueType = validParams[key];

                prop?.SetValue(result, Convert.ChangeType(value, valueType));
            }

            return (Params)result;
        }
    }
}