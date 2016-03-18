using System;

namespace XtremeEngineXNA.EntityComponent
{
    /// <summary>
    /// Delegate for the OwnerChanged event.
    /// </summary>
    /// <param name="component">Component whose owner has changed.</param>
    /// <param name="oldOwner">Previous owner of the component.</param>
    public delegate void OwnerChangedDelegate(IEntityComponent component, Entity oldOwner);

    /// <summary>
    /// Interface of a component of the entity-component system of XtremeEngine.
    /// </summary>
    public interface IEntityComponent : IUpdateable
    {
        /// <summary>
        /// Called when the component is added to an entity.
        /// </summary>
        void OnAdd();

        /// <summary>
        /// Called when another component is added or removed from the entity. This method should
        /// be used by the component to aquire references to other components in the entity.
        /// </summary>
        void OnReset();

        /// <summary>
        /// Called when the component is removed from an entity.
        /// </summary>
        void OnRemove();

        /// <summary>
        /// Gets the name of the component.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets or sets the Entity who owns the component, i.e. to which the component has been 
        /// added. 
        /// </summary>
        Entity Owner { get; set; }

        /// <summary>
        /// Gets the root object to which the component belongs.
        /// </summary>
        Root Root { get; }

        /// <summary>
        /// Event which occurs when the owner of the component is changed.
        /// </summary>
        event OwnerChangedDelegate OwnerChanged;
    }
}
