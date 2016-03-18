using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XtremeEngineXNA.Scene;

namespace XtremeEngineXNA.Gui.Widgets
{
    /// <summary>
    /// Abstract class which serves as the base for all the widgets. A widget (or control) is a
    /// gui element which can be placed on the screen to display/gather information.
    /// </summary>
    public abstract class Widget : GuiNode, IDrawable
    {
        #region Attributes

        /// <summary>
        /// Position of the widget in relative coordinates (0,0 -> 1,1).
        /// </summary>
        private Vector2? mRelativePosition = null;

        /// <summary>
        /// The distance of the widget from the left of the parent widget.
        /// </summary>
        private float? mLeft = null;

        /// <summary>
        /// The distance of the widget from the right of the parent widget.
        /// </summary>
        private float? mRight = null;

        /// <summary>
        /// The distance of the widget from the top of the parent widget.
        /// </summary>
        private float? mTop = null;

        /// <summary>
        /// The distance of the widget from the bottom of the parent widget.
        /// </summary>
        private float? mBottom = null;

        /// <summary>
        /// Width of the widget.
        /// </summary>
        private float mWidth = 0.0f;

        /// <summary>
        /// Height of the widget.
        /// </summary>
        private float mHeight = 0.0f;

        /// <summary>
        /// Preferred width of the widget.
        /// </summary>
        private float? mPreferredWidth = null;

        /// <summary>
        /// Preferred height of the widget.
        /// </summary>
        private float? mPreferredHeight = null;

        /// <summary>
        /// Width of the widget as a percentage of its parent's size.
        /// </summary>
        private float? mPercentWidth = null;

        /// <summary>
        /// Height of the widget as a percentage of its parent's size.
        /// </summary>
        private float? mPercentHeight = null;

        /// <summary>
        /// Whether the widget is visible.
        /// </summary>
        private bool mVisible = true;

        /// <summary>
        /// Layer to which the widget belongs.
        /// </summary>
        private int mLayer = 0;

        /// <summary>
        /// Sprite batch used to draw the widget.
        /// </summary>
        private SpriteBatch mSpriteBatch = null;

        /// <summary>
        /// Whether the widget's properties need to be validated.
        /// </summary>
        private bool mNeedsValidation = true;

        /// <summary>
        /// Whether the widget's size needs to be validated.
        /// </summary>
        private bool mSizeNeedsValidation = true;

        /// <summary>
        /// Whether the widget's layout needs to be validated.
        /// </summary>
        private bool mLayoutNeedsValidation = true;

        #endregion

        #region Public methods

        /// <summary>
        /// Initializes a new instance of the <see cref="Widget"/> class.
        /// </summary>
        /// <param name="root">Root object to which the object being created belongs.</param>
        public Widget(Root root)
            : base(root)
        {
            mSpriteBatch = new SpriteBatch(root.GraphicsDevice);
        }

        /// <summary>
        /// Draws the object.
        /// </summary>
        /// <remarks>Nothing is done here. Should be overriden by subclasses.</remarks>
        public virtual void Draw() {}

        #region Node overrides

        /// <summary>
        /// Attaches a node to this node.
        /// </summary>
        /// <param name="child">Node which is to be attached.</param>
        public override void AttachChild(Node child)
        {
            base.AttachChild(child);
            InvalidateProperties();
            InvalidateSize();
            InvalidateLayout();
        }

        /// <summary>
        /// Detaches a node from this node.
        /// </summary>
        /// <param name="child">Child node which is to be detached.</param>
        public override void DettachChild(Node child)
        {
            base.DettachChild(child);
            InvalidateProperties();
            InvalidateSize();
            InvalidateLayout();
        }

        /// <summary>
        /// Detaches all the children of this node.
        /// </summary>
        public override void DettachAll()
        {
            base.DettachAll();
            InvalidateProperties();
            InvalidateSize();
            InvalidateLayout();
        }

        #endregion

        /// <summary>
        /// Commits all the properties of the widget.
        /// </summary>
        public virtual void ValidateProperties()
        {
            if (this.NeedsValidation)
            {
                CommitProperties();
                this.NeedsValidation = false;
            }
            
            foreach (Widget widget in this.Children)
            {
                widget.ValidateProperties();
            }
        }

        /// <summary>
        /// Measures the default size of the widget.
        /// </summary>
        public void ValidateSize()
        {
            // We validate the children first so we go over the graph bottom-up.
            foreach (Widget widget in this.Children)
            {
                widget.ValidateSize();
            }

            if (this.SizeNeedsValidation)
            {
                Measure();
                this.SizeNeedsValidation = false;
            }
        }

        /// <summary>
        /// Takes care of the layout phase.
        /// </summary>
        public void ValidateLayout()
        {
            if (this.LayoutNeedsValidation)
            {
                // Update the sizes of children that specify their size as a percentage of the size of the parent.
                foreach (Widget widget in this.Children)
                {
                    bool changed = false;

                    // If the width of the widget is a percentage of the parent (this) have to update it.
                    if (widget.PercentWidth.HasValue)
                    {
                        widget.Width = widget.PercentWidth.Value / 100 * this.Width;
                        changed = true;
                    }

                    // If the height of the widget is a percentage of the parent (this) have to update it.
                    if (widget.PercentHeight.HasValue)
                    {
                        widget.Height = widget.PercentHeight.Value / 100 * this.Height;
                        changed = true;
                    }

                    // Invalidate the layout but make sure we don't measure again.
                    if (changed)
                    {
                        widget.SizeNeedsValidation = false;
                        widget.LayoutNeedsValidation = true;
                    }
                }

                LayoutChildren();
                this.LayoutNeedsValidation = false;
            }

            // We validate the children last so we go over the graph top-down.
            foreach (Widget widget in this.Children)
            {
                widget.ValidateLayout();
            }
        }

        /// <summary>
        /// Invalidates the properties of the graph causing the CommitProperties method to be
        /// called on the next update.
        /// </summary>
        public virtual void InvalidateProperties()
        {
            this.NeedsValidation = true;
        }

        /// <summary>
        /// Invalidates the size of the widget causing the Measure method to be called on the next
        /// update.
        /// </summary>
        public void InvalidateSize()
        {
            this.SizeNeedsValidation = true;

            // The size was changed so we have to re-layout the widget.
            InvalidateLayout();

            // Since the widget changed, the parent will have to measure and layout again so
            // invalidate the size.
            Widget parent = this.ParentWidget;
            if (parent != null)
            {
                parent.InvalidateSize();
            }
        }

        /// <summary>
        /// Invalidates the layout of the widget causing the Layout method to be called on the next
        /// update.
        /// </summary>
        public void InvalidateLayout()
        {
            this.LayoutNeedsValidation = true;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the parent widget.
        /// </summary>
        public Widget ParentWidget
        {
            get { return this.Parent as Widget; }
        }

        /// <summary>
        /// Gets or sets the width of the widget.
        /// </summary>
        public virtual float Width
        {
            get { return mWidth; }
            set
            {
                if (value != mWidth)
                {
                    mWidth = value;
                    InvalidateSize();
                }
            }
        }


        /// <summary>
        /// Gets or sets the height of the widget.
        /// </summary>
        public virtual float Height
        {
            get { return mHeight; }
            set
            {
                if (value != mHeight)
                {
                    mHeight = value;
                    InvalidateSize();
                }
            }
        }

        /// <summary>
        /// Gets or sets the preferred height of the widget.
        /// </summary>
        public virtual float? PreferredWidth
        {
            get { return mPreferredWidth; }
            set
            {
                if (value != mPreferredWidth)
                {
                    mPreferredWidth = value;
                    InvalidateSize();
                }
            }
        }

        /// <summary>
        /// Gets or sets the preferred height of the widget.
        /// </summary>
        public virtual float? PreferredHeight
        {
            get { return mPreferredHeight; }
            set
            {
                if (value != mPreferredHeight)
                {
                    mPreferredHeight = value;
                    InvalidateSize();
                }
            }
        }

        /// <summary>
        /// Gets or sets the width of the widget as a percentage of its parent's size.
        /// </summary>
        public virtual float? PercentWidth
        {
            get { return mPercentWidth; }
            set
            {
                if (value != mPercentWidth)
                {
                    mPercentWidth = value;
                    InvalidateSize();
                }
            }
        }

        /// <summary>
        /// Gets or sets the height of the widget as a percentage of its parent's size.
        /// </summary>
        public virtual float? PercentHeight
        {
            get { return mPercentHeight; }
            set
            {
                if (value != mPercentHeight)
                {
                    mPercentHeight = value;
                    InvalidateSize();
                }
            }
        }

        /// <summary>
        /// Gets the position of the widget in relative coordinates (0,0 -> 1,1).
        /// </summary>
        public virtual Vector2 RelativePosition
        {
            get { return mRelativePosition ?? (new Vector2()); }
            set
            {
                if (value != (mRelativePosition ?? Vector2.Zero))
                {
                    mRelativePosition = value;
                    InvalidateLayout();
                }                
            }
        }

        /// <summary>
        /// Gets the absolute position of the widget in relative coordinates (0,0 -> 1,1).
        /// </summary>
        public Vector2 AbsoluteRelativePosition
        {
            get
            {
                Vector2 parentSize = getParentSize();
                if (parentSize == Vector2.Zero)
                    return Vector2.Zero;

                Vector2 absPosition = this.AbsolutePosition;
                return new Vector2(absPosition.X / parentSize.X, absPosition.Y / parentSize.Y);
            }
        }

        /// <summary>
        /// Gets or sets the distance of the widget from the left of the screen.
        /// </summary>
        public virtual float Left
        {
            get { return mLeft ?? 0; }
            set
            {
                if (value != (mLeft ?? 0))
                {
                    mLeft = value;
                    InvalidateLayout();
                }
            }
        }

        /// <summary>
        /// Gets or sets the distance of the widget from the right of the screen.
        /// </summary>
        public virtual float Right
        {
            get { return mRight ?? 0; }
            set
            {
                if (value != (mRight ?? 0))
                {
                    mRight = value;
                    InvalidateLayout();
                }
            }
        }

        /// <summary>
        /// Gets or sets the distance of the widget from the top of the screen.
        /// </summary>
        public virtual float Top
        {
            get { return mTop ?? 0; }
            set
            {
                if (value != (mTop ?? 0))
                {
                    mTop = value;
                    InvalidateLayout();
                }
            }
        }

        /// <summary>
        /// Gets or sets the distance of the widget from the bottom of the screen.
        /// </summary>
        public virtual float Bottom
        {
            get { return mBottom ?? 0; }
            set
            {
                if (value != (mBottom ?? 0))
                {
                    mBottom = value;
                    InvalidateLayout();
                }
            }
        }

        /// <summary>
        /// Gets a <see cref="SpriteBatch"/> which can be used to draw the widget.
        /// </summary>
        public SpriteBatch SpriteBatch
        {
            get { return mSpriteBatch; }
        }

        /// <summary>
        /// Gets or sets the layer to which the widget belongs.
        /// </summary>
        public int Layer
        {
            get { return mLayer; }
            set
            {
                if (value != mLayer)
                {
                    mLayer = value;
                    OnLayerChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets whether the object is visible or not.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public bool Visible
        {
            get { return mVisible; }
            set
            {
                if (value != mVisible)
                {
                    mVisible = value;
                    OnVisibleChanged();
                }
            }
        }

        #region Validation flags

        /// <summary>
        /// Gets or sets a value indicating whether the widget needs validation.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the widget needs validation; otherwise, <c>false</c>.
        /// </value>
        public bool NeedsValidation
        {
            get { return mNeedsValidation; }
            set { mNeedsValidation = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the size of the widget needs validation.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the size of the widget needs validation; otherwise, <c>false</c>.
        /// </value>
        public bool SizeNeedsValidation
        {
            get { return mSizeNeedsValidation; }
            set { mSizeNeedsValidation = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the layout of the widget needs validation.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the layout of the widget needs validation; otherwise, <c>false</c>.
        /// </value>
        public bool LayoutNeedsValidation
        {
            get { return mLayoutNeedsValidation; }
            set { mLayoutNeedsValidation = value; }
        }

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the Layer property changes.
        /// </summary>
        public event EventHandler<EventArgs> LayerChanged;

        /// <summary>
        /// Invokes the LayerChanged event.
        /// </summary>
        public void OnLayerChanged()
        {
            if (LayerChanged != null)
                LayerChanged(this, new EventArgs());
        }

        /// <summary>
        /// Occurs when the Visible property changes.
        /// </summary>
        public event EventHandler<EventArgs> VisibleChanged;

        /// <summary>
        /// Invokes the VisibleChanged event.
        /// </summary>
        public void OnVisibleChanged()
        {
            if (VisibleChanged != null)
                VisibleChanged(this, new EventArgs());
        }

        #endregion

        #region Private/Protected methods

        /// <summary>
        /// Commits all the properties of the widget.
        /// </summary>
        protected virtual void CommitProperties()
        {
        }

        /// <summary>
        /// Measures the default size of the widget for layout purposes.
        /// </summary>
        protected virtual void Measure()
        {
            this.Width = this.PreferredWidth ?? 0;
            this.Height = this.PreferredHeight ?? 0;
        }

        /// <summary>
        /// Lays out the widget and its children.
        /// </summary>
        protected virtual void LayoutChildren()
        {
        }

        /// <summary>
        /// Gets the size of the parent.
        /// </summary>
        /// <returns>The size of the parent.</returns>
        private Vector2 getParentSize()
        {
            Widget parent = this.ParentWidget;
            return parent != null ? new Vector2(parent.Width, parent.Height) : Vector2.Zero;
        }

        #endregion
    }
}
