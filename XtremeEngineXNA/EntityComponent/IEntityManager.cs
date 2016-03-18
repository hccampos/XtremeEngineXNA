using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XtremeEngineXNA.EntityComponent
{
    /// <summary>
    /// Delegate which accepts an entity as a parameter.
    /// </summary>
    /// <param name="entity">Entity which was added.</param>
    public delegate void OnEntityAddedDelegate(Entity entity);

    /// <summary>
    /// Delegate which accepts an entity as a parameter.
    /// </summary>
    /// <param name="entity">Entity which was removed.</param>
    public delegate void OnEntityRemovedDelegate(Entity entity);

    /// <summary>
    /// Interface for the entity manager. The entity manager is responsible for managing all the 
    /// entities in a game or screen.
    /// </summary>
    public interface IEntityManager : IPlugin, IUpdateable
    {
        /// <summary>
        /// Adds an entity to the manager.
        /// </summary>
        /// <param name="entity">Entity which is to be added.</param>
        void AddEntity(Entity entity);

        /// <summary>
        /// Removes an entity from the manager.
        /// </summary>
        /// <param name="entity">Entity which is to be removed.</param>
        void RemoveEntity(Entity entity);

        /// <summary>
        /// Removes all the entities currently being managed by the manager.
        /// </summary>
        void RemoveAllEntities();

        /// <summary>
        /// Gets a list with all the entities managed by the manager.
        /// </summary>
        List<Entity> Entities { get; }

        #region Events

        /// <summary>
        /// Event which is triggered when the list of managed entities is changed.
        /// </summary>
        event EventHandler EntitiesChanged;

        /// <summary>
        /// Event which is triggered when an entity is added to the manager.
        /// </summary>
        event OnEntityAddedDelegate EntityAdded;

        /// <summary>
        /// Event which is triggered when an entity is removed from the manager.
        /// </summary>
        event OnEntityRemovedDelegate EntityRemoved;

        #endregion
    }
}
