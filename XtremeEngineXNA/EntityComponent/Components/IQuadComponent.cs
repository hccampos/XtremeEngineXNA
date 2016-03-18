using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XtremeEngineXNA.Objects;

namespace XtremeEngineXNA.EntityComponent.Components
{
    /// <summary>
    /// Interface of the quad component. A quad component creates a quad and adds it to the scene
    /// manager to represent the entity.
    /// </summary>
    public interface IQuadComponent : IEntityComponent
    {
        /// <summary>
        /// Gets the quad that represents the entity.
        /// </summary>
        Quad Quad { get; }
    }
}
