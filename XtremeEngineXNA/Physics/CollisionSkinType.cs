using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XtremeEngineXNA.Physics
{
    /// <summary>
    /// Defines the types of collision skins supported by the Physics Object.
    /// </summary>
    public enum CollisionSkinType
    {
        /// <summary>
        /// Bounding box collision skin.
        /// </summary>
        BOUNDING_BOX,
        /// <summary>
        /// Bounding sphere collision skin.
        /// </summary>
        BOUNDING_SPHERE,
        /// <summary>
        /// Triangle mesh collision skin.
        /// </summary>
        TRIANGLE_MESH,
        /// <summary>
        /// If this value is passed to the PhysicsObject constructor, no collision skin will be
        /// generated.
        /// </summary>
        DONT_GENERATE
    };
}
