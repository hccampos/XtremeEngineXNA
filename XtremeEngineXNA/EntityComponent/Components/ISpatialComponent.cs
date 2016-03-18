using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XtremeEngineXNA.Scene;

namespace XtremeEngineXNA.EntityComponent.Components
{
    /// <summary>
    /// Interface of a spatial component. A spatial component provides access to the position, 
    /// rotation and scale of an entity.
    /// </summary>
    public interface ISpatialComponent : IEntityComponent
    {
        /// <summary>
        /// Gets the scene node which contains the position, rotation and scale for an entity.
        /// </summary>
        SceneNode SceneNode
        {
            get;
        }
    }
}
