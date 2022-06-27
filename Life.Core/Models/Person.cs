namespace Life.Core.Models
{
    public sealed class Person : CellBase
    {
        public int Health { get; set; }
        public int Saturation { get; set; }
        public int Age { get; set; }
        public int VirusStrength { get; set; }
        public bool VirusGoDown { get; set; }
        public bool HasMask { get; set; }
        public int VaccineProtection { get; set; }
        public bool AtHouse { get; set; }

        public Person(
            int x,
            int y,
            int health,
            int saturation,
            int age,
            int visualType,
            int virusStrength,
            bool virusGoDown,
            bool hasMask,
            int vaccineProtection,
            bool atHouse)
            : base(x, y, visualType)
        {
            Health = health;
            Saturation = saturation;
            Age = age;
            VirusStrength = virusStrength;
            VirusGoDown = virusGoDown;
            HasMask = hasMask;
            VaccineProtection = vaccineProtection;
            AtHouse = atHouse;
        }
    }
}
