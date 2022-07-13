using Life.AutoTest.Parsing;
using Life.Core.Configuration;
using Life.Core.Models;
using System.Reflection;
using System.Text;

namespace Life
{
    public class CsvGenerator
    {
        private const char separator = ',';

        private readonly IEnumerable<string> _columns;
        private readonly List<string[]> _rows;

        private readonly string _filename;
        private readonly string _folder;
        private readonly string _relativeFilePath;

        private readonly Colony _colony;
        private readonly Params _params;
        private readonly Config _config;

        public CsvGenerator(
            IEnumerable<string> columns, 
            string filename, 
            Colony colony, 
            Config config,
            Params testParams)
        {
            _columns = columns;
            _rows = new List<string[]>();

            _folder = testParams.Directory;
            _filename = filename;
            _relativeFilePath = Path.Combine(_folder, _filename);

            _colony = colony;
            _config = config;
            _params = testParams;
        }

        public void RecordColonyStats()
        {
            _rows.Add(GetRow());
        }

        private string[] GetRow()
            => new string[]
            {
                _colony.People.Count.ToString(),
                _colony.People.Count((p) => p.VirusStrength > 0).ToString(),
                _colony.FoodItems.Count((f) => !f.IsVaccine).ToString(),
                _colony.Viruses.Count.ToString(),
                _colony.FoodItems.Count((f) => f.IsVaccine).ToString(),
                GetAveragePeopleAge().ToString(),
            };

        private int GetAveragePeopleAge()
        {
            if(_colony.People.Count == 0)
            {
                return 0;
            }

            return (int)Math.Round(_colony.People.Average((p) => p.Age));
        }

        public void GenerateFile()
        {
            var builder = new StringBuilder();

            AppendConfig(builder);
            AppendParamUnderStudy(builder);
            AppendColonyStats(builder);

            if(!Directory.Exists(_folder)) Directory.CreateDirectory(_folder);

            using var fs = new FileStream(_relativeFilePath, FileMode.Create, FileAccess.Write, FileShare.None);
            using var writer = new StreamWriter(fs, Encoding.UTF8);
            writer.Write(builder.ToString());
        }

        private void AppendColonyStats(StringBuilder builder)
        {
            builder.AppendLine(string.Join(separator, _columns));

            foreach (var row in _rows)
            {
                builder.AppendLine(string.Join(separator, row));
            }
        }

        private void AppendConfig(StringBuilder builder, int marginBottom = 5)
        {
            string[] columns = typeof(Config).GetProperties()
                                            .Select((p) => p.Name)
                                            .ToArray();
            string?[] values = typeof(Config).GetProperties()
                                             .Select((p) => p.GetValue(_config)?.ToString())
                                             .ToArray();

            builder.AppendLine(string.Join(separator, columns));
            builder.AppendLine(string.Join(separator, values));

            builder.Append(new string('\n', marginBottom));
        }

        private void AppendParamUnderStudy(StringBuilder builder, int marginBottom = 5)
        {
            string[] columns = typeof(Params).GetProperties()
                                            .Where((p) => p.GetCustomAttribute<IgnoreCsvAttribute>() == null)
                                            .Select((p) => p.Name)
                                            .ToArray();

            string?[] values = typeof(Params).GetProperties()
                                             .Where((p) => p.GetCustomAttribute<IgnoreCsvAttribute>() == null)
                                             .Select((p) => p.GetValue((object?)_params)?.ToString())
                                             .ToArray();

            builder.AppendLine(string.Join(separator, columns));
            builder.AppendLine(string.Join(separator, values));

            builder.Append(new string('\n', marginBottom));
        }
    }
}
