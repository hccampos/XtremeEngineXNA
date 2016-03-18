using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XtremeEngineXNA.Gui.Widgets
{
    /// <summary>
    /// Widget which automatically aligns it's children vertically.
    /// </summary>
    public class VBoxWidget : BoxWidget
    {
        #region Attributes

        /// <summary>
        /// How the children should be aligned horizontally.
        /// </summary>
        private HAlignMode mAlign = HAlignMode.LEFT;

        #endregion

        #region Public methods

        /// <summary>
        /// Initializes a new instance of the <see cref="HBoxWidget"/> class.
        /// </summary>
        /// <param name="root">Root object to which the object being created belongs.</param>
        public VBoxWidget(Root root)
            : base(root)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the horizontal alignment of the children.
        /// </summary>
        public HAlignMode HAlign
        {
            get { return mAlign; }
            set
            {
                if (value != mAlign)
                {
                    mAlign = value;
                    InvalidateProperties();
                }
            }
        }

        #endregion

        #region Private/Protected methods

        /// <summary>
        /// Commits all the properties of the widget.
        /// </summary>
        protected override void CommitProperties()
        {
            base.CommitProperties();
        }

        /// <summary>
        /// Measures the default size of the widget for layout purposes.
        /// </summary>
        protected override void Measure()
        {
            base.Measure();

            float w = 0.0f;
            float h = 0.0f;

            // Add the heights and get the maximum width.
            foreach (Widget widget in this.Children)
            {
                w = Math.Max(w, widget.Width);
                h += widget.Height;
            }

            // Add the gaps.
            h += this.Gap * (this.Children.Count - 1);

            // Set the dimensions of the widget.
            this.Width = this.PreferredWidth.HasValue ? this.PreferredWidth.Value : w;
            this.Height = this.PreferredHeight.HasValue ? this.PreferredHeight.Value : h;
        }

        /// <summary>
        /// Lays out the widget and its children.
        /// </summary>
        protected override void LayoutChildren()
        {
            base.LayoutChildren();

            float x = 0.0f; // HAlignMode.LEFT;
            float y = 0.0f;

            // Determine our reference point on the x coordinate.
            switch (this.HAlign)
            {
                case HAlignMode.CENTER:
                    x = this.Width / 2;
                    break;
                case HAlignMode.RIGHT:
                    x = this.Width;
                    break;
                default:
                    break;
            }

            // Position each child.
            foreach (Widget widget in this.Children)
            {
                // Determine the offset used to center the widget vertically.
                float offset = 0.0f; // HAlignMode.LEFT
                switch (this.HAlign)
                {
                    case HAlignMode.CENTER:
                        offset = widget.Width / 2;
                        break;
                    case HAlignMode.RIGHT:
                        offset = widget.Width;
                        break;
                    default:
                        break;
                }

                // Position the widget.
                widget.Position = new Vector2(x - offset, y);

                // Move down.
                y += widget.Height + this.Gap;
            }
        }

        #endregion
    }
}
