using System;

namespace XtremeEngineXNA.EntityComponent
{
    /// <summary>
    /// Base class for all the components in XtremeEngine. A component can be added to an entity
    /// to provide functionality to that entity. For instance, an entity can have a physics
    /// component that adds physics behaviour to it.
    /// </summary>
    public abstract class EntityComponent : Base, IEntityComponent
    {
        #region Attributes

        /// <summary>
        /// Name of the component.
        /// </summary>
        private string mName;

        /// <summary>
        /// Entity who owns the component, i.e. to which the component has been added.
        /// </summary>
        private Entity mOwner;

        /// <summary>
        /// Whether the component is enabled.
        /// </summary>
        private Boolean mEnabled;

        /// <summary>
        /// Update order of the component.
        /// </summary>
        private int mUpdateOrder;

        #endregion

        #region Public methods

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="root">Root object to which the component belongs.</param>
        /// <param name="name">Name of the component.</param>
        public EntityComponent(Root root, string name) : base(root)
        {
            mName = name;
            mOwner = null;
            mEnabled = true;
            mUpdateOrder = 0;
        }

        /// <summary>
        /// Called when the component is added to an entity.
        /// </summary>
        public virtual void OnAdd()
        {
        }

        /// <summary>
        /// Called when another component is added or removed from the entity. This method should
        /// be used by the component to aquire references to other components in the entity.
        /// </summary>
        public virtual void OnReset()
        {
        }

        /// <summary>
        /// Called when the component is removed from an entity.
        /// </summary>
        public virtual void OnRemove()
        {
        }

        /// <summary>
        /// Updates the component.
        /// </summary>
        /// <param name="elapsedTime">Time that has passed since the last update.</param>
        /// <remarks>Should be overriden by subclasses if they wish to be updated.</remarks>
        public virtual void Update(TimeSpan elapsedTime)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of the component.
        /// </summary>
        public string Name
        {
            get { return mName; }
        }

        /// <summary>
        /// Gets or sets the Entity who owns the component, i.e. to which the component has been 
        /// added. 
        /// </summary>
        public Entity Owner
        {
            get { return mOwner; }
            set 
            {
                if (value != mOwner)
                {
                    Entity oldOwner = mOwner;
                    mOwner = value;

                    if (OwnerChanged != null)
                        OwnerChanged(this, oldOwner);
                }
            }
        }

        /// <summary>
        /// Gets or sets whether the component is enabled, i.e. if it is updated.
        /// </summary>
        public bool Enabled
        {
            get
            {
                return mEnabled;
            }
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
        /// Gets or sets the update order of the component.
        /// </summary>
        public int UpdateOrder
        {
            get { throw new NotImplementedException(); }
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

        #endregion

        #region Events

        /// <summary>
        /// Event which is triggered when the owner of the component is changed.
        /// </summary>
        public event OwnerChangedDelegate OwnerChanged;

        /// <summary>
        /// Event which is triggered when the Enabled property of the component is changed.
        /// </summary>
        public event EventHandler<EventArgs> EnabledChanged;

        /// <summary>
        /// Event which is triggered when the UpdateOrder property of the component is changed.
        /// </summary>
        public event EventHandler<EventArgs> UpdateOrderChanged;

        #endregion
    }
}
