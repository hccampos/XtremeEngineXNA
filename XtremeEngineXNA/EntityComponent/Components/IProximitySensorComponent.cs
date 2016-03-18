using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XtremeEngineXNA;
using XtremeEngineXNA.EntityComponent;
using XtremeEngineXNA.EntityComponent.Components;
using XtremeEngineXNA.Scene;

namespace XtremeEngineXNA.EntityComponent.Components
{
    /// <summary>
    /// Delegate type for the HitDetected event.
    /// </summary>
    /// <param name="entity">Entity which triggered the event.</param>
    public delegate void HitDetectedDelegate(Entity entity);

    /// <summary>
    /// Component which detects when entities are within a certain distance of a sensor attached to
    /// the entity which owns this component.
    /// </summary>
    public interface IProximitySensorComponent : IEntityComponent
    {
        /// <summary>
        /// Adds an entity to the list of entities that are to be monitored by the component.
        /// </summary>
        /// <param name="entity">Entity which is to be added.</param>
        void AddSensedEntity(Entity entity);

        /// <summary>
        /// Removes an entity from the list of entities that are sensed by the component.
        /// </summary>
        /// <param name="entity">Entity which is to be removed.</param>
        void RemoveSensedEntity(Entity entity);

        /// <summary>
        /// Removes all the entities from the component.
        /// </summary>
        void RemoveAllSensedEntities();

        /// <summary>
        /// Gets or sets the list of entities that are sensed by the component.
        /// </summary>
        List<Entity> SensedEntities { get; set; }

        /// <summary>
        /// Gets or sets the distance at which the sensor will trigger the HitDetected event.
        /// </summary>
        float Threshold { get; set; }

        /// <summary>
        /// Gets or sets the square of the distance at which the sensor will trigger the 
        /// HitDetected event.
        /// </summary>
        float ThresholdSquared { get; set; }

        /// <summary>
        /// Event which occurs when one of the entities added to the component is at a distance
        /// from the sensor which is smaller than the specified threshold.
        /// </summary>
        event HitDetectedDelegate HitDetected;
    }
}
