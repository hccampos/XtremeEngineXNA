using System;
using System.Collections.Generic;

namespace XtremeEngineXNA
{
    /// <summary>
    /// Interface which should be implemented by the objects which hold a list of IDrawable's.
    /// </summary>
    public interface IDrawableContainer
    {
        /// <summary>
        /// Gets the drawables in the container.
        /// </summary>
        /// <value>The drawables in the container.</value>
        List<IDrawableObject> Drawables
        {
            get;
        }

        /// <summary>
        /// Gets the visible drawables in the container.
        /// </summary>
        /// <value>The visible drawables in the container.</value>
        List<IDrawableObject> VisibleDrawables
        {
            get;
        }

        /// <summary>
        /// Gets or sets the layer to which the drawables in the container belong.
        /// </summary>
        /// <value>
        /// The layer to which the drawables in the container belong.
        /// </value>
        int Layer
        {
            get;
            set;
        }

        /// <summary>
        /// Occurs when the layer property changes.
        /// </summary>
        event EventHandler<EventArgs> LayerChanged;

        /// <summary>
        /// Gets or sets whether the drawables in the container are visible or not.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        bool Visible
        {
            get;
            set;
        }

        /// <summary>
        /// Occurs when the Visible property changes.
        /// </summary>
        event EventHandler<EventArgs> VisibleChanged;

        /// <summary>
        /// Gets the shadow casters in the container.
        /// </summary>
        /// <value>The shadow casters in the container.</value>
        List<IDrawableObject> ShadowCasters
        {
            get;
        }

        /// <summary>
        /// Gets the shadow casters whose CastShadows property is true.
        /// </summary>
        /// <value>The shadow casters whose CastShadows property is true..</value>
        List<IDrawableObject> EnabledShadowCasters
        {
            get;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the shadow casters in the container cast
        /// shadows.
        /// </summary>
        /// <value>
        /// <c>true</c> if the shadow casters in the container cast shadows; otherwise, 
        /// <c>false</c>.
        /// </value>
        bool CastShadows
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the shadow casters in the container cast 
        /// shadows when they are invisible.
        /// </summary>
        /// <value>
        /// <c>true</c> if the shadow casters in the container cast shadows when they are 
        /// invisible.; otherwise, <c>false</c>.
        /// </value>
        bool CastShadowsWhenInvisible
        {
            get;
            set;
        }       

        /// <summary>
        /// Occurs when the CastShadows property changes.
        /// </summary>
        event EventHandler<EventArgs> CastShadowsChanged;

        /// <summary>
        /// Occurs when the CastShadowsWhenInvisible property changes.
        /// </summary>
        event EventHandler<EventArgs> CastShadowsWhenInvisibleChanged;
    }
}
