using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XtremeEngineXNA.Content;
using XtremeEngineXNA.Scene;
using Nuclex.Fonts;
using Nuclex.Graphics;

namespace XtremeEngineXNA.Objects
{
    /// <summary>
    /// Types of text alignment.
    /// </summary>
    public enum TEXT_ALIGN
    {
        /// <summary>
        /// Align the text to the left.
        /// </summary>
        ALIGN_LEFT,
        /// <summary>
        /// Align the text to the right.
        /// </summary>
        ALIGH_RIGHT,
        /// <summary>
        /// Align the text to the center.
        /// </summary>
        ALIGN_CENTER
    };

    /// <summary>
    /// Class which represents a string of 3D text characters.
    /// </summary>
    public class Text3D : GeneratedObject
    {
        #region Attribues

        /// <summary>
        /// String which is to be displayed.
        /// </summary>
        private string mText = "";

        /// <summary>
        /// How the text should be aligned.
        /// </summary>
        private TEXT_ALIGN mTextAlign = TEXT_ALIGN.ALIGN_LEFT;

        /// <summary>
        /// Name of the font used to create the text mesh.
        /// </summary>
        private string mFontName = "";

        /// <summary>
        /// Font used to create the text mesh.
        /// </summary>
        private Nuclex.Fonts.VectorFont mFont = null;

        /// <summary>
        /// Total width of the string in world units.
        /// </summary>
        private float mWidth = 0.0f;

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes a new instance of the <see cref="Text3D"/> class.
        /// </summary>
        /// <param name="root">Root object to which the new text object belongs.</param>
        /// <param name="text">Text which is to be drawn.</param>
        /// <param name="fontName">Name of the font used to draw the text.</param>
        /// <param name="align">Alignment of the text.</param>
        /// <param name="material">Material used to draw the object.</param>
        public Text3D(Root root, string text = "", string fontName = "", 
            TEXT_ALIGN align = TEXT_ALIGN.ALIGN_LEFT, Material material = null)
            : base(root, material)
        {
            mText = text;
            mTextAlign = align;
            mFontName = fontName;

            if (fontName == null || fontName.Length == 0)
                return;

            //Load the vector font from which the text mesh will be extruded.
            mFont = root.ContentManager.Load<VectorFont>(fontName);
            GenerateTextMesh();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the name of the font used to create the text mesh.
        /// </summary>
        /// <value>The name of the font used to create the text mesh.</value>
        public string FontName
        {
            get { return mFontName; }
            set
            {
                if (value != mFontName)
                {
                    mFontName = value;
                    mFont = this.Root.ContentManager.Load<VectorFont>(mFontName);
                    GenerateTextMesh();
                }
            }
        }

        /// <summary>
        /// Gets the height of the text.
        /// </summary>
        public float Height
        {
            get { return mFont.LineHeight * this.ScalingFactors.Y; }
        }

        /// <summary>
        /// Gets the total width of the text in world units.
        /// </summary>
        public float Width
        {
            get { return mWidth * this.ScalingFactors.X; }
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text
        {
            get { return mText; }
            set
            {
                if (value != mText)
                {
                    mText = value;
                    GenerateTextMesh();
                }
            }
        }

        /// <summary>
        /// Gets or sets how the text should be aligned.
        /// </summary>
        /// <value>
        /// How the text should be aligned.
        /// </value>
        public TEXT_ALIGN TextAlign
        {
            get { return mTextAlign; }
            set
            {
                mTextAlign = value;
                GenerateTextMesh();
            }
        }

        #endregion

        #region Private/Protected members

        /// <summary>
        /// Generates the text mesh by extruding the vector font for each character of the current 
        /// text.
        /// </summary>
        private void GenerateTextMesh()
        {
            //Create the text mesh using the font and the current text.
            Nuclex.Fonts.Text mesh = mFont.Extrude(mText);

            mWidth = mesh.Width;

            //Align the text.
            for (int i = 0; i < mesh.Vertices.Length; ++i)
            {
                Matrix alignTransform = Matrix.Identity;
                if (mTextAlign == TEXT_ALIGN.ALIGN_CENTER)
                {
                    alignTransform = Matrix.CreateTranslation(new Vector3(-this.mWidth / 2, 0, 0));
                }
                else if (mTextAlign == TEXT_ALIGN.ALIGH_RIGHT)
                {
                    alignTransform = Matrix.CreateTranslation(new Vector3(-this.Width, 0, 0));
                }

                mesh.Vertices[i].Position = Vector3.Transform(mesh.Vertices[i].Position, alignTransform);
            }

            int vertexCount = mesh.Vertices.Length;
            int indexCount = mesh.Indices.Length;

            if (vertexCount == 0 || indexCount == 0)
            {
                this.VertexBuffer = null;
                this.IndexBuffer = null;
                this.PrimitiveCount = 0;
                return;
            }

            //Create the vertex buffer.
            VertexBuffer vb = new VertexBuffer(Root.GraphicsDevice,
                typeof(VertexPositionNormalTexture), vertexCount, BufferUsage.None);
            vb.SetData<VertexPositionNormalTexture>(mesh.Vertices);

            //Create the index buffer.
            IndexBuffer ib = new IndexBuffer(Root.GraphicsDevice, IndexElementSize.SixteenBits,
                indexCount, BufferUsage.None);
            ib.SetData<short>(mesh.Indices);

            this.VertexBuffer = vb;
            this.IndexBuffer = ib;
            this.PrimitiveType = mesh.PrimitiveType;
            this.PrimitiveCount = VertexHelper.GetPrimitiveCount(indexCount, this.PrimitiveType);            
        }

        #endregion
    }
}
