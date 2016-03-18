namespace XtremeEngineXNA
{
    /// <summary>
    /// Abstract class which implements the common members of the IPlugin interface. Subclasses
    /// should override the Initialize and Destroy methods.
    /// </summary>
    public abstract class PluginBase : Base, IPlugin
    {
        /// <summary>
        /// Name of the plug-in.
        /// </summary>
        private string mName;

        /// <summary>
        /// Initializes a plug-in.
        /// </summary>
        /// <param name="root">Root object to which the plug-in belongs.</param>
        /// <param name="name">Name of the new plug-in.</param>
        public PluginBase(Root root, string name)
            : base(root)
        {
            mName = name;
        }

        /// <summary>
        /// Initializes the plug-in.
        /// </summary>
        public virtual void Initialize()
        {
        }

        /// <summary>
        /// Called before the plug-in is removed from the root object or when the root object is
        /// about to be destroyed. The plug-in should destroy any resources it may have created.
        /// </summary>
        public virtual void Destroy()
        {
        }

        /// <summary>
        /// Returns the name of the plug-in.
        /// </summary>
        public string Name
        {
            get { return mName; }
        }
    }
}
