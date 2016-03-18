using System;

namespace XtremeEngineXNA.Physics
{
    /// <summary>
    /// Interface which all the physics controllers must implement.
    /// 
    /// The controller is used to update an object for instance by applying a force.
    /// </summary>
    public interface IPhysicsController
    {
        /// <summary>
        /// Performs any operations necessary to update the state of the physics world.
        /// </summary>
        /// <param name="elapsedTime">Time elapsed since the last update.</param>
        void UpdatePhysics(TimeSpan elapsedTime);
    }
}
