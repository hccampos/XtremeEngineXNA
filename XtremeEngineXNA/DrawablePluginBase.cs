using System;

namespace XtremeEngineXNA
{
    /// <summary>
    /// Abstract class which implements the most common members of the IDrawablePlugin interface. 
    /// </summary>
    public abstract class DrawablePluginBase : PluginBase, IDrawablePlugin
    {
        /// <summary>
        /// Whether the plugin is visible or not.
        /// </summary>
        private bool mVisible = true;

        /// <summary>
        /// Layer to which the plug-in belongs.
        /// </summary>
        private int mLayer = 0;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="root">Root object to which the plug-in belongs.</param>
        /// <param name="name">Name of the new plug-in.</param>
        public DrawablePluginBase(Root root, string name) : base(root, name) { }

        /// <summary>
        /// Draws the plugin.
        /// </summary>
        public abstract void Draw();

        /// <summary>
        /// Gets or sets the layer to which the drawable belongs.
        /// </summary>
        /// <value>
        /// The layer to which the drawable belongs.
        /// </value>
        public int Layer
        {
            get { return mLayer; }
            set
            {
                if (value != mLayer)
                {
                    mLayer = value;
                    if (LayerChanged != null)
                    {
                        LayerChanged(this, new EventArgs());
                    }
                }
            }
        }

        /// <summary>
        /// Occurs when the layer property changes.
        /// </summary>
        public event EventHandler<EventArgs> LayerChanged;

        /// <summary>
        /// Gets or sets whether the plugin is visible or not.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public bool Visible
        {
            get { return mVisible; }
            set
            {
                if (value != mVisible)
                {
                    mVisible = value;
                    if (VisibleChanged != null)
                    {
                        VisibleChanged(this, new EventArgs());
                    }
                }
            }
        }

        /// <summary>
        /// Occurs when the Visible property changes.
        /// </summary>
        public event EventHandler<EventArgs> VisibleChanged;  
    }
}
