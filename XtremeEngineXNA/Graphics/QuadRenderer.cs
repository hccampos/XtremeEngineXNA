using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XtremeEngineXNA.Graphics
{
    /// <summary>
    /// Class which can draw full screen quads.
    /// </summary>
    public partial class QuadRenderer : Base
    {
        #region Attributes
        
        /// <summary>
        /// Vertices of the quad.
        /// </summary>
        private VertexPositionTexture[] mVertices;

        /// <summary>
        /// Indices of the quad.
        /// </summary>
        private short[] mIndices;

        #endregion

        #region QuadRenderer Members

        /// <summary>
        /// Creates a new quad renderer.
        /// </summary>
        /// <param name="root">Root object to which the new quad renderer belongs.</param>
        public QuadRenderer(Root root) : base(root)
        {
            mVertices = new VertexPositionTexture[4];
            mVertices[0] = new VertexPositionTexture(new Vector3(-1, 1, 0f), new Vector2(0, 0));
            mVertices[1] = new VertexPositionTexture(new Vector3(1, 1, 0f), new Vector2(1, 0));
            mVertices[2] = new VertexPositionTexture(new Vector3(-1, -1, 0f), new Vector2(0, 1));  
            mVertices[3] = new VertexPositionTexture(new Vector3(1, -1, 0f), new Vector2(1, 1));
            mIndices = new short[6] { 0, 1, 2, 2, 1, 3 };
        } 

        /// <summary>
        /// Draw the full screen quad.
        /// </summary>
        public void Draw()
        {
            Root.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionTexture>(
                PrimitiveType.TriangleList, mVertices, 0, 4, mIndices, 0, 2);
        }

        #endregion
    }
}
