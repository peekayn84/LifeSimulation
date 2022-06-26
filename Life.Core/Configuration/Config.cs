namespace Life.Core.Configuration {
    ///<summary>
    /// Configuration for "Life" game in JSON format.
    /// The "Chance" word means the param is measured in percents
    ///</summary>
    public class Config {
        // Field settings
        ///<summary>
        /// Field columns count.
        /// Default = 15
        ///</summary>
        public int ColumnsCount { get; set; } = 15;
        ///<summary>
        /// Field rows count.
        /// Default = 10
        ///</summary>
        public int RowsCount { get; set; } = 10;

        //People settings
        ///<summary>
        /// Chance that a person will move one cell in certain direction.
        /// Default = 100
        ///</summary>
        public int PersonMoveChance { get; set; } = 100;
        ///<summary>
        /// Default saturation per person.
        /// Default = 15
        ///</summary>
        public int PersonDefaultSaturation { get; set; } = 15;
        ///<summary>
        /// Default health per person. Maximum is set by default.
        /// Default = 50
        ///</summary>
        public int PersonDefaultHealth { get; set; } = 50;
        ///<summary>
        /// Default age per person.
        /// Default = 0
        ///</summary>
        public int PersonDefaultAge { get; set; } = 0;
        ///<summary>
        /// How many people needed to create a new person.
        /// Default = 2
        ///</summary>
        public int PeopleToCreatePersonCount { get; set; } = 2;
        ///<summary>
        /// Chance of creating a new person by a certain group of people.
        /// Default = 45
        ///</summary>
        public int PersonCreateChance { get; set; } = 45;
        ///<summary>
        /// The age when a new person becomes adult.
        /// Default = 16
        ///</summary>
        public int AdultAge { get; set; } = 16;
        ///<summary>
        /// The age what which a person is considered to be old.
        /// Default = 80
        ///</summary>
        public int OldAge { get; set; } = 80;
        ///<summary>
        /// The max age after what the person dies.
        /// Default = 100
        ///</summary>
        public int MaxAge { get; set; } = 100;
        ///<summary>
        /// The number of cells as far as a person sees around him.
        /// Used when choosing a direction.
        /// Default = 3
        ///</summary>
        public int VisionRadius { get; set; } = 3;

        // House settings
        ///<summary>
        /// Minimum people count per house.
        /// Default = 10
        ///</summary>
        public int MinHouseCapacity { get; set; } = 10;
        ///<summary>
        /// Maximum people count per house.
        /// Default = 20
        ///</summary>
        public int MaxHouseCapacity { get; set; } = 20;

        // Virus settings
        ///<summary>
        /// The chance person gets a virus through air.
        /// Default = 1
        ///</summary>
        public int ChanceToGetInfectedWithAir { get; set; } = 1;
        ///<summary>
        /// The chance to infect neightbour person.
        /// If there are 2 infected neighbours, the chance adds up.
        /// Default = 50
        ///</summary>
        public int ChanceToInfectNeighbour { get; set; } = 50;
        ///<summary>
        /// Chance to be infected by a person(infected) that wears mask.
        /// Default = 25
        ///</summary>
        public int ChanceToInfectWhenInMask { get; set; } = 25;
        ///<summary>
        /// Chance to be infected when a person(healthy) wears mask.
        /// Default = 15
        ///</summary>
        public int ChanceToBeInfectedWhenInMask { get; set; } = 15;
        ///<summary>
        /// The initial chance the person starts to recover from virus.
        /// Updated after each move.
        /// Default = 10
        ///</summary>
        public int InitialChanceVirusGoDown { get; set; } = 10;
        ///<summary>
        /// The chance to leave a virus after infected person dies.
        /// Default = 50
        ///</summary>
        public int ChanceToCreateVirusOnDeath { get; set; } = 50;
        ///<summary>
        /// How much moves will the virus live in its cell.
        /// Default = 3
        ///</summary>
        public int MovesToVirusLive { get; set; } = 3;

        // Vaccine settings
        ///<summary>
        /// Initial vaccine chance to save from infection.
        /// Default = 70
        ///</summary>
        public int InitialVaccinePower { get; set; } = 70;
        ///<summary>
        /// The decrement for the person's vaccine power per move.
        /// Default = 3
        ///</summary>
        public int VaccinePowerDecrement { get; set; } = 3;
        ///<summary>
        /// The chance to generate vaccine per cell.
        /// Default = 10
        ///</summary>
        public int ChanceToGenerateVaccine { get; set; } = 10;
        ///<summary>
        /// Max limit of the vaccine items to exist on field per each move.
        /// Default = 5
        ///</summary>
        public int MaxVaccineItemsOnField { get; set; } = 5;

        // Food settings
        ///<summary>
        /// Minimum food points to be given to person who picks up a food item.
        /// Default = 10
        ///</summary>
        public int MinFoodIncrement { get; set; } = 10;
        ///<summary>
        /// Maximum food points to be given to person who picks up a food item.
        /// Default = 15
        ///</summary>
        public int MaxFoodIncrement { get; set; } = 15;
        ///<summary>
        /// The chance to generate food per cell.
        /// Default = 50
        ///</summary>
        public int ChanceToGenerateFood { get; set; } = 50;
        ///<summary>
        /// The max limit of the food items to exist on field per each move.
        /// Default = 20
        ///</summary>
        public int MaxFoodItemsOnMap { get; set; } = 20;

    }
}