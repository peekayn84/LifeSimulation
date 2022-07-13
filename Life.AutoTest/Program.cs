using Life.AutoTest.Parsing;
using Life.Core.Abstractions;
using Life.Core.Configuration;
using Life.Core.Models;
using System.Text;

namespace Life.AutoTest
{
    public static class Program
    {
        private const string filePrefixName = "autotest";
        private const string configFileName = "autotest_config.json";
        private static readonly string[] columns = new string[]
        {
            "Total people count",
            "Infected people count",
            "Resources of the colony",
            "Viruses count",
            "Vaccine items count",
            "Avg. age"
        };

        private static ParamsParser? _parser;
        private static IConfigManager<Config> _configManager;
        private static Colony _colony;
        private static CsvGenerator _csv;

        public static void Main(string[] args)
        {
            Console.Clear();

            try
            {
                _configManager = new JSONConfigManager(configFileName);
                _parser = new ParamsParser(args);
                _parser.OverrideConfigProp(_configManager.Configuration);
                SetUpConsole();
                DoUpdateCycle();
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine($"ERROR: {ex.Message}");
            }

            Console.Write("\nPress any key to finish...");
            Console.ReadKey(false);
        }

        private static void DoUpdateCycle()
        {
            int fileIndex = 1;
            int cursorPositionOffset = 8;
            DateTime timeStarted = DateTime.Now;

            Console.SetCursorPosition(0, cursorPositionOffset);
            Console.WriteLine("Generating .csv files...");

            do
            {
                UpdateConfigWithParams();

                _colony = new Colony(
                    _configManager.Configuration.RowsCount,
                    _configManager.Configuration.ColumnsCount,
                    _configManager);

                RandomlyAddHouses();
                RandomlyAddPeople();

                var filename = MakeFileName(fileIndex);

                _csv = new CsvGenerator(
                    columns,
                    filename,
                    _colony,
                    _configManager.Configuration,
                    _parser.Params
                );

                Console.SetCursorPosition(3, cursorPositionOffset + fileIndex);
                Console.WriteLine("- {0,-50}...", filename);

                for (int i = 0; i < _parser.Params.Duration; i++)
                {
                    _colony.UpdateColony();
                    _csv.RecordColonyStats();
                    UpdateInterface();
                    UpdateTTX(timeStarted);
                }

                _csv.GenerateFile();

                Console.SetCursorPosition(3, cursorPositionOffset + fileIndex);
                Console.WriteLine("- {0,-50} Ready", filename);

                fileIndex++;
            } while (_parser.Params.Next());
        }

        private static void UpdateConfigWithParams()
        {
            typeof(Config).GetProperty(_parser.Params.Param)?.SetValue(_configManager.Configuration, _parser.Params.CurrentValue);
        }

        private static string MakeFileName(int index) => 
            string.Format(
                "{0}_{1}_{2}-{3}_{4}__{5}.csv",
                filePrefixName,
                _parser.Params.Param,
                _parser.Params.From,
                _parser.Params.To,
                _parser.Params.Step,
                index
            );

        private static void RandomlyAddPeople()
        {
            int peopleCount = Random.Shared.Next(_parser.Params.MinPeople, _parser.Params.MaxPeople);

            while(_colony.People.Count < peopleCount)
            {
                _colony.RandomlyAddPerson();
            }
        }

        private static void RandomlyAddHouses()
        {
            int peopleCount = Random.Shared.Next(_parser.Params.MinHouses, _parser.Params.MaxHouses);

            while (_colony.Houses.Count < peopleCount)
            {
                _colony.RandomlyAddHouse();
            }
        }

        private static void UpdateInterface()
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Autotest by Maksym Diachenko from KIUKI-19-6");
            Console.WriteLine($"Parameter under research: {_parser.Params.Param}");
            Console.WriteLine($"Range: {_parser.Params.From}-{_parser.Params.To} | Step: {_parser.Params.Step}");
            Console.WriteLine($"Output dir: {_parser.Params.Directory}");

            string progressBar = GetProgressBar(_parser.Params.GetProgressPercentage());

            Console.WriteLine($"\nCurrent value: {_parser.Params.CurrentValue} {progressBar}");
        }

        private static void UpdateTTX(DateTime timeStarted)
        {
            Console.SetCursorPosition(0, 6);

            Console.WriteLine("{0:20} elapsed", GetTTX(timeStarted));
        }

        private static void SetUpConsole()
        {
            Console.Title = $"Autotest: {_parser.Params.Param}";
            Console.OutputEncoding = Encoding.UTF8;
        }

        private static string GetProgressBar(int current) =>
            string.Format("[{0}{1}] {2}%",
                new string('>', current / 5),
                new string('-', 20 - current / 5),
                current);

        private static string GetTTX(DateTime timeStarted)
        {
            TimeSpan diff = DateTime.Now - timeStarted;

            return string.Format("{0:h\\:mm\\:ss\\.FFFFFF}", diff);
        }
    }
}