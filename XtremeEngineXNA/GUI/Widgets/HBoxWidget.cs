using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XtremeEngineXNA.Gui.Widgets;
using Microsoft.Xna.Framework;

namespace XtremeEngineXNA.Gui.Widgets
{
    /// <summary>
    /// Widget which automatically aligns it's children horizontally.
    /// </summary>
    public class HBoxWidget : BoxWidget
    {
        #region Attributes

        /// <summary>
        /// How the children should be aligned vertically.
        /// </summary>
        private VAlignMode mAlign = VAlignMode.TOP;

        #endregion

        #region Public methods

        /// <summary>
        /// Initializes a new instance of the <see cref="HBoxWidget"/> class.
        /// </summary>
        /// <param name="root">Root object to which the object being created belongs.</param>
        public HBoxWidget(Root root)
            : base(root)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the vertical alignment of the children.
        /// </summary>
        public VAlignMode VAlign
        {
            get { return mAlign; }
            set
            {
                if (value != mAlign)
                {
                    mAlign = value;
                    InvalidateLayout();
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

            // Add the widths and get the maximum height.
            foreach (Widget widget in this.Children)
            {
                w += widget.PreferredWidth.HasValue ? widget.PreferredWidth.Value : widget.Width;
                h = Math.Max(h, widget.PreferredHeight.HasValue ? widget.PreferredHeight.Value : widget.Height);
            }

            // Add the gaps.
            w += this.Gap * (this.Children.Count - 1);

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

            float x = 0.0f;
            float y = 0.0f; // VAlignMode.TOP

            // Determine our reference point on the y coordinate.
            switch (this.VAlign)
            {
                case VAlignMode.MIDDLE:
                    y = this.Height / 2;
                    break;
                case VAlignMode.BOTTOM:
                    y = this.Height;
                    break;
                default:
                    break;
            }

            // Position and size each child.
            foreach (Widget widget in this.Children)
            {
                // Determine the offset used to center the widget vertically.
                float offset = 0.0f; // VAlignMode.TOP
                switch (this.VAlign)
                {
                    case VAlignMode.MIDDLE:
                        offset = widget.Height / 2;
                        break;
                    case VAlignMode.BOTTOM:
                        offset = widget.Height;
                        break;
                    default:
                        break;
                }

                // Position the widget.
                widget.Position = new Vector2(x, y - offset);

                // Move to the right.
                x += widget.Width + this.Gap;
            }
        }

        #endregion
    }
}
