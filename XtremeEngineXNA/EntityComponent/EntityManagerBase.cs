using System;
using System.Collections.Generic;

namespace XtremeEngineXNA.EntityComponent
{
    /// <summary>
    /// Manager which is responsible for all the entities in the game.
    /// </summary>
    public abstract class EntityManagerBase : PluginBase, IEntityManager
    {
        #region Attributes

        /// <summary>
        /// Whether the plugin is enabled (i.e. is updated).
        /// </summary>
        private bool mEnabled;

        /// <summary>
        /// Update order of the plugin.
        /// </summary>
        private int mUpdateOrder;

        #endregion

        #region Public methods

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="root">Root object to which the plugin belongs.</param>
        /// <param name="name">Name of the plugin.</param>
        public EntityManagerBase(Root root, string name)
            : base(root, name)
        {
            mEnabled = true;
            mUpdateOrder = 0;
        }

        /// <summary>
        /// Adds an entity to the manager.
        /// </summary>
        /// <param name="entity">Entity which is to be added.</param>
        public abstract void AddEntity(Entity entity);

        /// <summary>
        /// Removes an entity from the manager.
        /// </summary>
        /// <param name="entity">Entity which is to be removed.</param>
        public abstract void RemoveEntity(Entity entity);

        /// <summary>
        /// Removes all the entities currently being managed by the manager.
        /// </summary>
        public abstract void RemoveAllEntities();

        #endregion

        #region Properties

        /// <summary>
        /// Gets a list with all the entities managed by the manager.
        /// </summary>
        public abstract List<Entity> Entities
        {
            get;
        }

        #endregion

        #region Events

        /// <summary>
        /// Event which is triggered when the list of managed entities is changed.
        /// </summary>
        public abstract event EventHandler EntitiesChanged;

        /// <summary>
        /// Event which is triggered when an entity is added to the manager.
        /// </summary>
        public abstract event OnEntityAddedDelegate EntityAdded;

        /// <summary>
        /// Event which is triggered when an entity is removed from the manager.
        /// </summary>
        public abstract event OnEntityRemovedDelegate EntityRemoved;

        #endregion

        #region IUpdateable members

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
                        EnabledChanged(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Updates the manager.
        /// </summary>
        /// <param name="elapsedTime"></param>
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
                        UpdateOrderChanged(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Occurs when the Enabled property changed.
        /// </summary>
        public event EventHandler<EventArgs> EnabledChanged;

        /// <summary>
        /// Occurs when the UpdateOrder property changed.
        /// </summary>
        public event EventHandler<EventArgs> UpdateOrderChanged;

        #endregion
    }
}
