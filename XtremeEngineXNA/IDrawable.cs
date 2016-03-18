using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XtremeEngineXNA
{
    /// <summary>
    /// Interface which must be implemented by all the objects which want to be drawn.
    /// </summary>
    public interface IDrawable
    {
        /// <summary>
        /// Draws the object.
        /// </summary>
        void Draw();

        /// <summary>
        /// Gets or sets the layer to which the drawable belongs.
        /// </summary>
        /// <value>
        /// The layer to which the drawable belongs.
        /// </value>
        int Layer
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets whether the object is visible or not.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        bool Visible
        {
            get;
            set;
        }

        /// <summary>
        /// Occurs when the layer property changes.
        /// </summary>
        event EventHandler<EventArgs> LayerChanged;

        /// <summary>
        /// Occurs when the Visible property changes.
        /// </summary>
        event EventHandler<EventArgs> VisibleChanged;
    }
}
