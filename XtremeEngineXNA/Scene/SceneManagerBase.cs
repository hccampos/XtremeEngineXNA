using System.Collections.Generic;
using System;

namespace XtremeEngineXNA.Scene
{
    /// <summary>
    /// Abstract class which provides the basic functionality required by the ISceneManager
    /// interface
    /// </summary>
    public abstract class SceneManagerBase : PluginBase, ISceneManager
    {
        #region Attributes

        /// <summary>
        /// Whether the scene manager is enabled or not.
        /// </summary>
        private bool mEnabled;

        /// <summary>
        /// Value used to update some components before others.
        /// </summary>
        private int mUpdateOrder;

        #endregion

        #region SceneManager members

        /// <summary>
        /// Initializes a new scene manager.
        /// </summary>
        /// <param name="root">Root object to which the new scene manager belongs.</param>
        /// <param name="name">Name of the new scene manager.</param>
        public SceneManagerBase(Root root, string name)
            : base(root, name)
        {
            mEnabled = true;
            mUpdateOrder = 1;
        }

        /// <summary>
        /// Sets which camera is active.
        /// </summary>
        /// <param name="camera">Camera which is to be the active camera.</param>
        public abstract void SetActiveCamera(CameraNode camera);

        #endregion

        #region Properties

        /// <summary>
        /// Gets the root of the scene manager's scene graph.
        /// </summary>
        public abstract SceneNode RootSceneNode
        {
            get;
        }

        /// <summary>
        /// Gets the list of nodes which are to be drawn.
        /// </summary>
        public abstract List<IDrawableObject> VisibleDrawableNodes
        {
            get;
        }

        /// <summary>
        /// Gets the list of nodes which cast shadows.
        /// </summary>
        public abstract List<IDrawableObject> ShadowCastersList
        {
            get;
        }

        /// <summary>
        /// Gets the list of lights which are to be used to draw the scene.
        /// </summary>
        public abstract List<LightNode> LightsList
        {
            get;
        }

        /// <summary>
        /// Gets/Sets the camera which is currently active.
        /// </summary>
        public abstract CameraNode ActiveCamera
        {
            get;
            set;
        }

        /// <summary>
        /// Returns a list with all the cameras in the scene.
        /// </summary>
        public abstract List<CameraNode> CamerasList
        {
            get;
        }

        #endregion

        #region IUpdateable Members

        /// <summary>
        /// Gets or sets whether the object is enabled or not.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled
        {
            get { return mEnabled; }
            set
            {
                if (value != mEnabled)
                {
                    mEnabled = value;
                    if (EnabledChanged != null)
                    {
                        EnabledChanged(this, new EventArgs());
                    }
                }
            }
        }

        /// <summary>
        /// Occurs when the Enabled property changed.
        /// </summary>
        public event EventHandler<EventArgs> EnabledChanged;

        /// <summary>
        /// Updates the state of the updateable.
        /// </summary>
        /// <param name="elapsedTime">Time elapsed since the last update.</param>
        public abstract void Update(TimeSpan elapsedTime);

        /// <summary>
        /// Returns the update order of the scene node.
        /// </summary>
        /// <value>The update order of the object.</value>
        public int UpdateOrder
        {
            get { return mUpdateOrder; }
            set
            {
                if (value != mUpdateOrder)
                {
                    mUpdateOrder = value;
                    if (UpdateOrderChanged != null)
                    {
                        UpdateOrderChanged(this, new EventArgs());
                    }
                }
            }
        }

        /// <summary>
        /// Occurs when the UpdateOrder property changed.
        /// </summary>
        public event EventHandler<EventArgs> UpdateOrderChanged;

        #endregion
    }
}
