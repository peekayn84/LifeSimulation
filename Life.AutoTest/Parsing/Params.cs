namespace Life.AutoTest.Parsing
{
    public class Params
    {
        private int _from = 0;

        public string Param { get; set; } = "ChanceToInfectNeighbour";
        public int From
        {
            get { return _from; }
            set
            {
                _from = value;
                CurrentValue = value;
            }
        }

        public int To { get; set; } = 100;
        public int Step { get; set; } = 10;
        public int CurrentValue { get; set; } = 0;

        public int Duration { get; set; } = 100;

        public int MinPeople { get; set; } = 3;
        public int MaxPeople { get; set; } = 10;
        public int MinHouses { get; set; } = 3;
        public int MaxHouses { get; set; } = 10;

        [IgnoreCsv]
        public string Directory { get; set; } = "test-results";


        public bool Next()
        {
            CurrentValue += Step;

            return CurrentValue <= To;
        }

        public int GetProgressPercentage()
        {
            return (int)Math.Round(CurrentValue / (double)To * 100.0);
        }
    }
}