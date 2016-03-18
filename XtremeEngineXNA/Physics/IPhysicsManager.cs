using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XtremeEngineXNA.Physics
{
    /// <summary>
    /// Interface of the physics manager. The physics manager is responsible for managing all the
    /// physics objects and controllers.
    /// </summary>
    public interface IPhysicsManager : IPlugin, IUpdateable
    {
        /// <summary>
        /// Adds a controller to the physics manager.
        /// </summary>
        /// <param name="controller">The controller which is to be added.</param>
        void AddController(IPhysicsController controller);

        /// <summary>
        /// Removes a controller from the physics manager.
        /// </summary>
        /// <param name="controller">The controller which is to be removed.</param>
        void RemoveController(IPhysicsController controller);

        /// <summary>
        /// Gets or sets the gravity force which is applied to all the physics objects.
        /// </summary>
        Vector3 Gravity { get; set; }
    }
}
