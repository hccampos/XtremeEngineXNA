using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XtremeEngineXNA.Gui.Widgets
{
    /// <summary>
    /// Abstract class which helps implement box widgets.
    /// </summary>
    public abstract class BoxWidget : Widget
    {
        #region Attributes

        /// <summary>
        /// Gap used to separate the children of the widget.
        /// </summary>
        private float mGap = 50.0f;

        #endregion

        #region Public methods

        /// <summary>
        /// Initializes a new instance of the <see cref="BoxWidget"/> class.
        /// </summary>
        /// <param name="root">The root.</param>
        public BoxWidget(Root root)
            : base(root)
        {
        }

        /// <summary>
        /// Validates all the properties of the widget.
        /// </summary>
        public override void ValidateProperties()
        {
            // We only validate if it's necessary.
            if (!NeedsValidation)
                return;

            LayoutChildren();

            base.ValidateProperties();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the gap used to separate the children of the widget.
        /// </summary>
        public virtual float Gap
        {
            get { return mGap; }
            set
            {
                if (value != mGap)
                {
                    mGap = value;
                    InvalidateProperties();
                }
            }
        }

        #endregion
    }
}
