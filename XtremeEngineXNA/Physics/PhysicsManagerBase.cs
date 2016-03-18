using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XtremeEngineXNA.Physics
{
    /// <summary>
    /// Abstract class which provides the basic methods required by the IPhysicsManager interface.
    /// </summary>
    public abstract class PhysicsManagerBase: PluginBase, IPhysicsManager
    {
        #region Attributes

        /// <summary>
        /// Whether the manager is enabled or not.
        /// </summary>
        private bool mEnabled;

        /// <summary>
        /// Update order of the physics manager.
        /// </summary>
        private int mUpdateOrder;

        #endregion //Attributes

        #region Public methods

        /// <summary>
        /// Creates a new physics manager.
        /// </summary>
        /// <param name="root">Root object to which the manager belongs.</param>
        /// <param name="name">Name of the new physics manager.</param>
        public PhysicsManagerBase(Root root, string name)
            : base(root, name)
        {
            mEnabled = true;
            mUpdateOrder = 1;
        }

        /// <summary>
        /// Adds a physics body to the physics manager.
        /// </summary>
        /// <param name="body">The physics body which is to be added to the physics manager.</param>
        internal abstract void AddBody(PhysicsBody body);

        /// <summary>
        /// Removes a physics body from the physics manager.
        /// </summary>
        /// <param name="body">The body which is to be removed from the physics manager.</param>
        internal abstract void RemoveBody(PhysicsBody body);

        /// <summary>
        /// Adds a controller to the physics manager.
        /// </summary>
        /// <param name="controller">The controller which is to be added.</param>
        public abstract void AddController(IPhysicsController controller);

        /// <summary>
        /// Removes a controller from the physics manager.
        /// </summary>
        /// <param name="controller">The controller which is to be removed.</param>
        public abstract void RemoveController(IPhysicsController controller);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the gravity force which is applied to all the physics objects.
        /// </summary>
        public abstract Vector3 Gravity { get; set; }

        /// <summary>
        /// Gets or sets whether the object is enabled or not.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool  Enabled
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
        /// Updates the state of the updateable.
        /// </summary>
        /// <param name="elapsedTime">Time elapsed since the last update.</param>
        public abstract void Update(TimeSpan elapsedTime);

        /// <summary>
        /// Returns the update order of the scene node.
        /// </summary>
        /// <value>The update order of the object.</value>
        public int  UpdateOrder
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

        #endregion

        #region Events

        /// <summary>
        /// Event which is triggered when the physics manager is enabled.
        /// </summary>
        public event EventHandler<EventArgs> EnabledChanged;

        /// <summary>
        /// Event which is triggered when the update order of the physics manager is changed.
        /// </summary>
        public event EventHandler<EventArgs> UpdateOrderChanged;

        #endregion
    }
}
