using System;
using System.Collections.Generic;

namespace XtremeEngineXNA.EntityComponent
{
    /// <summary>
    /// Default implementation of the IEntityManager interface.
    /// </summary>
    internal class DefaultEntityManager : EntityManagerBase
    {
        #region Attributes

        /// <summary>
        /// Entities that are managed by the manager.
        /// </summary>
        List<Entity> mEntities;

        #endregion

        #region Public methods

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="root">Root object to which the manager belongs.</param>
        /// <param name="name">Name of the manager.</param>
        public DefaultEntityManager(Root root, string name)
            : base(root, name)
        {
            mEntities = new List<Entity>();
        }

        /// <summary>
        /// Destroys the manager.
        /// </summary>
        public override void Destroy()
        {
            if (mEntities != null)
            {
                foreach (Entity entity in mEntities)
                {
                    entity.RemoveAllComponents();
                }

                mEntities.Clear();
                mEntities = null;
            }
        }

        /// <summary>
        /// Updates the state of the updateable.
        /// </summary>
        /// <param name="elapsedTime">Time elapsed since the last update.</param>
        public override void Update(TimeSpan elapsedTime)
        {
            foreach (Entity entity in mEntities)
            {
                if (entity.Enabled)
                {
                    entity.Update(elapsedTime);
                }
            }
        }

        /// <summary>
        /// Adds an entity to the manager.
        /// </summary>
        /// <param name="entity">Entity which is to be added.</param>
        public override void AddEntity(Entity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("EntityManager.AddEntity(): null entity.");
            }

            if (mEntities.Find(e => e == entity) != null)
            {
                throw new ArgumentException("EntityManager.AddEntity(): duplicate entity.");
            }

            mEntities.Add(entity);

            if (EntityAdded != null)
                EntityAdded(entity);
        }

        /// <summary>
        /// Removes an entity from the manager.
        /// </summary>
        /// <param name="entity">Entity which is to be removed.</param>
        public override void RemoveEntity(Entity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("EntityManager.RemoveEntity(): null entity.");
            }

            if (mEntities.Remove(entity))
            {
                if (EntityRemoved != null)
                {
                    EntityRemoved(entity);
                }
            }
            else
            {
                throw new ArgumentException("EntityManager.RemoveEntity(): entity not found.");
            }
        }

        /// <summary>
        /// Removes all the entities currently being managed by the manager.
        /// </summary>
        public override void RemoveAllEntities()
        {
            List<Entity> entities = new List<Entity>(mEntities);
            foreach (Entity entity in entities)
            {
                RemoveEntity(entity);
            }

            if (EntitiesChanged != null)
                EntitiesChanged(this, new EventArgs());
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a list with all the entities managed by the manager.
        /// </summary>
        public override List<Entity> Entities
        {
            get { return mEntities; }
        }

        #endregion

        #region Events

        /// <summary>
        /// Event which is triggered when the list of managed entities is changed.
        /// </summary>
        public override event EventHandler EntitiesChanged;

        /// <summary>
        /// Event which is triggered when an entity is added to the manager.
        /// </summary>
        public override event OnEntityAddedDelegate EntityAdded;

        /// <summary>
        /// Event which is triggered when an entity is removed from the manager.
        /// </summary>
        public override event OnEntityRemovedDelegate EntityRemoved;

        #endregion
    }
}
