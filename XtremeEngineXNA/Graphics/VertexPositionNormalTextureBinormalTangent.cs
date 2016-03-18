using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XtremeEngineXNA.Graphics
{
    /// <summary>
    /// Describes a custom vertex format structure that contains position, normal, texture
    /// coordinates, binormal and tangent.
    /// </summary>
    struct VertexPositionNormalTextureBinormalTangent : IVertexType
    {
        /// <summary>
        /// Position of the vertex.
        /// </summary>
        public Vector3 Position;

        /// <summary>
        /// Normal of the vertex.
        /// </summary>
        public Vector3 Normal;

        /// <summary>
        /// Texture-coordinates of the vertex.
        /// </summary>
        public Vector2 TexCoords;

        /// <summary>
        /// Binormal of the vertex.
        /// </summary>
        public Vector3 Binormal;

        /// <summary>
        /// Tangent of the vertex.
        /// </summary>
        public Vector3 Tangent;

        /// <summary>
        /// Vertex declaration for the vertex type.
        /// </summary>
        public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration
        (
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
            new VertexElement(24, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
            new VertexElement(32, VertexElementFormat.Vector3, VertexElementUsage.Binormal, 0),
            new VertexElement(44, VertexElementFormat.Vector3, VertexElementUsage.Tangent, 0)
        );

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexPositionNormalTextureBinormalTangent"/> struct.
        /// </summary>
        /// <param name="position">The position of the vertex.</param>
        /// <param name="normal">The normal of the vertex.</param>
        /// <param name="texCoords">The tex coords of the vertex.</param>
        /// <param name="binormal">The binormal of the vertex.</param>
        /// <param name="tangent">The tangent of the vertex.</param>
        public VertexPositionNormalTextureBinormalTangent(Vector3 position, Vector3 normal,
            Vector2 texCoords, Vector3 binormal, Vector3 tangent)
        {
            Position = position;
            Normal = normal;
            TexCoords = texCoords;
            Binormal = binormal;
            Tangent = tangent;
        }

        /// <summary>
        /// Gets the vertex declaration.
        /// </summary>
        /// <value>The vertex declaration.</value>
        VertexDeclaration IVertexType.VertexDeclaration
        {
            get { return VertexDeclaration; }
        }
    };
}
