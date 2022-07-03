namespace Life.Core.Configuration
{
    ///<summary>
    /// Settings for managing UI assets
    ///</summary>
    public static class AssetsSettings
    {
        ///<summary>
        /// Total person icons count in Assets/ folder
        ///</summary>
        public const int PersonIconsCount = 3;
        ///<summary>
        /// Total house icons count in Assets/ folder
        ///</summary>
        public const int HouseIconsCount = 2;
        ///<summary>
        /// Total food icons count in Assets/ folder
        ///</summary>
        public const int FoodIconsCount = 10;
        ///<summary>
        /// Total vaccine icons count in Assets/ folder
        ///</summary>
        public const int VaccineIconsCount = 1;

        ///<summary>
        /// Size of each person status bar in pixels
        ///</summary>
        public const int BarSizePX = 4;
        ///<summary>
        /// Padding for each person status bar in pixels
        ///</summary>
        public const int BarPaddingPX = 5;
        ///<summary>
        ///  Cell size in pixels
        ///</summary>
        public const int CellSizePX = 100;

        ///<summary>
        /// JSON settings file name
        ///</summary>
        public const string JSONSettingsFilename = "settings.json";
    }
}