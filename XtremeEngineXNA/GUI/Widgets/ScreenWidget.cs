using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XtremeEngineXNA.Gui.Widgets
{
    /// <summary>
    /// Class which represents the screen widget, the root of the GUI graph.
    /// </summary>
    public class ScreenWidget : Widget
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScreenWidget"/> class.
        /// </summary>
        /// <param name="root">Root object to which the object being created belongs.</param>
        public ScreenWidget(Root root)
            : base(root)
        {
        }

        /// <summary>
        /// Draws the object.
        /// </summary>
        public override void Draw()
        {
            base.Draw();
            //Nothing to the done.
        }

        /// <summary>
        /// Gets the width of the widget.
        /// </summary>
        public override float Width
        {
            get { return this.Root.GraphicsDevice.Viewport.Width; }
        }

        /// <summary>
        /// Gets the height of the widget.
        /// </summary>
        public override float Height
        {
            get { return this.Root.GraphicsDevice.Viewport.Height; }
        }
    }
}
