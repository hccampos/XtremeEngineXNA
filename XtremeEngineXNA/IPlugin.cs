namespace XtremeEngineXNA
{
    /// <summary>
    /// Interface which defines all the basic functionality that an XtremeEngine plug-in must 
    /// implement.
    /// </summary>
    public interface IPlugin : IBase
    {
        /// <summary>
        /// Initializes the plug-in.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Called before the plug-in is removed from the root object or when the root object is
        /// about to be destroyed. The plug-in should destroy any resources it may have created.
        /// </summary>
        void Destroy();

        /// <summary>
        /// Returns the name of the plug-in.
        /// </summary>
        string Name { get; }
    }
}
