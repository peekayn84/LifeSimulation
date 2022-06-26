namespace Life.Core.Abstractions
{
    ///<summary>
    /// Configuration manager interface for
    /// managing specific <typeparam name="TConfig">configuration type</typeparam>
    ///</summary>
    public interface IConfigManager<TConfig>
    {
        ///<summary>
        /// Configuration
        ///</summary>
        TConfig Configuration { get; }

        ///<summary>
        /// Method that asynchronously reads configuration in specific format and returns
        /// specific configuration type
        ///</summary>
        Task<TConfig> LoadConfig();
    }
}
