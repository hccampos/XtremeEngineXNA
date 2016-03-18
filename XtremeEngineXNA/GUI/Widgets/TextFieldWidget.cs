using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XtremeEngineXNA.Gui.Widgets
{
    /// <summary>
    /// Class which represents a text field.
    /// </summary>
    public class TextFieldWidget : Widget
    {
        #region Attributes

        /// <summary>
        /// Text which is displayed in the text field.
        /// </summary>
        private string mText = "";

        /// <summary>
        /// Font used to display the text.
        /// </summary>
        private SpriteFont mFont = null;

        /// <summary>
        /// Name of the font used to display text.
        /// </summary>
        private string mFontName = null;

        #endregion

        #region Public methods

        /// <summary>
        /// Initializes a new instance of the <see cref="TextFieldWidget"/> class.
        /// </summary>
        /// <param name="root">Root object to which the object being created belongs.</param>
        /// <param name="text">The text which is to be displayed in the text field.</param>
        /// <param name="fontName">The name of the font which is to be used to display the text.</param>
        public TextFieldWidget(Root root, string text = "", string fontName = null)
            : base(root)
        {
            this.Text = text;
            this.FontName = fontName; // Use the property because that also sets up the mFont attribute.
        }

        /// <summary>
        /// Draws the object.
        /// </summary>
        public override void Draw()
        {
            base.Draw();

            if (mFont == null || mText == null || mText.Length == 0)
                return;

            SpriteBatch spriteBatch = this.SpriteBatch;
            spriteBatch.Begin();
            spriteBatch.DrawString(mFont, mText, this.AbsolutePosition, Color.White);
            spriteBatch.End();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text
        {
            get { return mText; }
            set
            {
                if (value != mText)
                {
                    mText = value;
                    OnTextChanged();

                    // Invalidate the properties because the size has changed.
                    base.InvalidateProperties();
                }
            }
        }

        /// <summary>
        /// Gets or sets the font.
        /// </summary>
        public SpriteFont Font
        {
            get { return mFont; }
        }

        /// <summary>
        /// Gets or sets the name of the font used to display text.
        /// </summary>
        public string FontName
        {
            get { return mFontName; }
            set
            {
                if (value != mFontName)
                {
                    mFontName = value;
                    if (mFontName != null && mFontName != "")
                    {
                        mFont = this.Root.ContentManager.Load<SpriteFont>(mFontName);
                    }
                    else
                    {
                        mFont = null;
                    }

                    OnFontChanged();

                    // Invalidate the properties because the size has changed.
                    base.InvalidateProperties();
                }
            }
        }

        /// <summary>
        /// Gets or sets the preferred width of the widget.
        /// </summary>
        public override float? PreferredWidth
        {
            get { return this.TextSize.X; }
        }

        /// <summary>
        /// Gets or sets the preferred height of the widget.
        /// </summary>
        public override float? PreferredHeight
        {
            get { return this.TextSize.Y; }
        }

        /// <summary>
        /// Gets the size of the text.
        /// </summary>
        public Vector2 TextSize
        {
            get
            {
                if (mFont != null && mText != null && mText != "")
                {
                    return mFont.MeasureString(mText);
                }
                else
                {
                    return Vector2.Zero;
                }
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the Text property is changed.
        /// </summary>
        public event EventHandler<EventArgs> TextChanged;

        /// <summary>
        /// Invokes the TextChanged event.
        /// </summary>
        public void OnTextChanged()
        {
            if (TextChanged != null)
                TextChanged(this, new EventArgs());
        }

        /// <summary>
        /// Occurs when the Font property is changed.
        /// </summary>
        public event EventHandler<EventArgs> FontChanged;

        /// <summary>
        /// Invokes the FontChanged event.
        /// </summary>
        public void OnFontChanged()
        {
            if (FontChanged != null)
                FontChanged(this, new EventArgs());
        }

        #endregion
    }
}
