using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XtremeEngineXNA;
using XtremeEngineXNA.Content;
using XtremeEngineXNA.Content.Materials;
using XtremeEngineXNA.Graphics;
using XtremeEngineXNA.Scene;

namespace XtremeEngineXNA.Objects
{
    /// <summary>
    /// Class which represents a quad.
    /// </summary>
    public class Quad : GeneratedObject
    {
        #region Attributes

        /// <summary>
        /// Width of the quad.
        /// </summary>
        private float mWidth;

        /// <summary>
        /// Height of the quad.
        /// </summary>
        private float mHeight;

        #endregion

        #region Quad members

        /// <summary>
        /// Initializes a new instance of the <see cref="Quad"/> class.
        /// </summary>
        /// <param name="root">Root object to which the new quad belongs.</param>
        /// <param name="material">Material used to draw the new quad.</param>
        /// <param name="width">Width of the new quad.</param>
        /// <param name="height">Height of the new quad.</param>
        /// <param name="tilingU">
        /// The tiling factor along the u texture axis (i.e. the number of times that the texture
        /// repeats itself along the u axis.
        /// </param>
        /// <param name="tilingV">
        /// The tiling factor along the v texture axis (i.e. the number of times that the texture
        /// repeats itself along the v axis.
        /// </param>
        public Quad(Root root, Material material = null, float width = 1, float height = 1, 
            float tilingU = 1.0f, float tilingV = 1.0f)
            : base(root, material)
        {
            mWidth = width;
            mHeight = height;

            //Declare the vertices.
            Vector3 binormal = new Vector3(-1, 0, 0);
            Vector3 tangent = new Vector3(0, -1, 0);
            VertexPositionNormalTextureBinormalTangent[] vertices = new VertexPositionNormalTextureBinormalTangent[4];
            vertices[0] = new VertexPositionNormalTextureBinormalTangent(new Vector3(-0.5f, 0.5f, 0f), 
                Vector3.UnitZ, new Vector2(0, 0), binormal, tangent);
            vertices[1] = new VertexPositionNormalTextureBinormalTangent(new Vector3(0.5f, 0.5f, 0f), 
                Vector3.UnitZ, new Vector2(tilingU, 0), binormal, tangent);
            vertices[2] = new VertexPositionNormalTextureBinormalTangent(new Vector3(-0.5f, -0.5f, 0f), 
                Vector3.UnitZ, new Vector2(0, tilingV), binormal, tangent);
            vertices[3] = new VertexPositionNormalTextureBinormalTangent(new Vector3(0.5f, -0.5f, 0f), 
                Vector3.UnitZ, new Vector2(tilingU, tilingV), binormal, tangent);

            //Create the vertex buffer.
            VertexBuffer vb = new VertexBuffer(Root.GraphicsDevice, 
                typeof(VertexPositionNormalTextureBinormalTangent), 4, BufferUsage.None);
            vb.SetData<VertexPositionNormalTextureBinormalTangent>(vertices);

            //Declare the indices.
            short[] indices = new short[6] { 0, 1, 2, 2, 1, 3 };

            //Create the index buffer.
            IndexBuffer ib = new IndexBuffer(Root.GraphicsDevice, IndexElementSize.SixteenBits, 6,
                BufferUsage.None);
            ib.SetData<short>(indices);

            this.VertexBuffer = vb;
            this.IndexBuffer = ib;
            this.PrimitiveCount = 2;
            this.PrimitiveType = PrimitiveType.TriangleList;
            this.ScalingFactors = new Vector3(mWidth, mHeight, 1.0f);
        }

        
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the width of the quad.
        /// </summary>
        /// <value>The width of the quad.</value>
        public float Width
        {
            get { return mWidth; }
            set
            {
                mWidth = value;
                ScalingFactors = new Vector3(mWidth, ScalingFactors.Y, ScalingFactors.Z);
            }
        }

        /// <summary>
        /// Gets or sets the height of the quad.
        /// </summary>
        /// <value>The height of the quad.</value>
        public float Height
        {
            get { return mHeight; }
            set
            {
                mHeight = value;
                ScalingFactors = new Vector3(ScalingFactors.X, mHeight, ScalingFactors.Z);
            }
        }

        #endregion

    }
}
