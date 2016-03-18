using System;
using XtremeEngineXNA.Content;
using XtremeEngineXNA.Scene;

namespace XtremeEngineXNA
{
    /// <summary>
    /// Interface which must be implemented by all the objects in extreme engine that want to be
    /// placed in a scene and drawn by the renderer.
    /// </summary>
    public interface IDrawableObject : IDrawable
    {
        /// <summary>
        /// Gets the material used to draw the object.
        /// </summary>
        Material Material
        {
            get;
        }

        /// <summary>
        /// Gets the node which contains the information needed to transform the object (position,
        /// rotation, scaling, etc) in order to draw the object.
        /// </summary>
        /// <value>The node with the information needed to transform the drawable.</value>
        SceneNode NodeWithTransforms
        {
            get;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this drawable casts shadows.
        /// </summary>
        /// <value><c>true</c> if this drawable casts shadows; otherwise, <c>false</c>.</value>
        bool CastShadows
        {
            get;
            set;
        }

        /// <summary>
        /// Occurs when the CastShadows property changes.
        /// </summary>
        event EventHandler<EventArgs> CastShadowsChanged;

        /// <summary>
        /// Gets or sets a value indicating whether the shadow caster casts shadows when it is 
        /// invisible.
        /// </summary>
        /// <value>
        /// <c>true</c> if the object casts shadows when it is invisible; otherwise, <c>false</c>.
        /// </value>
        bool CastShadowsWhenInvisible
        {
            get;
            set;
        }

        /// <summary>
        /// Occurs when the CastShadowsWhenInvisible property changes.
        /// </summary>
        event EventHandler<EventArgs> CastShadowsWhenInvisibleChanged;
    }
}
