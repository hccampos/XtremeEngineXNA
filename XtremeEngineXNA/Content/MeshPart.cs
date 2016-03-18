using System;
using Microsoft.Xna.Framework.Graphics;
using XtremeEngineXNA.Scene;

namespace XtremeEngineXNA.Content
{
    /// <summary>
    /// Class which represents a part of a mesh. A mesh is made up of one or more mesh parts which
    /// can have their own material.
    /// </summary>
    public class MeshPart: Base, IDrawableObject
    {
        #region Attributes

        /// <summary>
        /// Index buffer which stores the indices of the vertices of the mesh part.
        /// </summary>
        private IndexBuffer mIndexBuffer;

        /// <summary>
        /// Number of vertices in the mesh part.
        /// </summary>
        private int mNumVertices;

        /// <summary>
        /// Number of primitives in a single draw call.
        /// </summary>
        private int mPrimitiveCount;

        /// <summary>
        /// Location in the index array at which to start reading vertices.
        /// </summary>
        private int mStartIndex;

        /// <summary>
        /// Vertex buffer which stores the geometry information of the mesh part.
        /// </summary>
        private VertexBuffer mVertexBuffer;

        /// <summary>
        /// Offset (in vertices) from the top of vertex buffer.
        /// </summary>
        private int mVertexOffset;

        /// <summary>
        /// Offset (in bytes) from the top of vertex buffer.
        /// </summary>
        private int mVertexOffsetBytes;

        /// <summary>
        /// Material used to draw this mesh part.
        /// </summary>
        private Material mMaterial;

        /// <summary>
        /// Mesh node to which this mesh part belongs.
        /// </summary>
        private MeshNode mParentMesh;

        #endregion

        #region MeshPart Members

        /// <summary>
        /// Creates a new model mesh part node object.
        /// </summary>
        /// <param name="root">Root object to which the mesh part belongs.</param>
        /// <param name="ib">Index buffer of the mesh part.</param>
        /// <param name="numVerts">Number of vertices in the mesh part.</param>
        /// <param name="primitiveCount">Number of primitives in the mesh part.</param>
        /// <param name="startIndex">
        /// Location in the index array at which to start reading vertices.
        /// </param>
        /// <param name="vb">Vertex buffer of the mesh part.</param>
        /// <param name="vertexOffset">Offset (in vertices) from the top of vertex buffer.</param>
        /// <param name="material">Material used to draw this mesh part.</param>
        public MeshPart(Root root, IndexBuffer ib, int numVerts, int primitiveCount,
            int startIndex, VertexBuffer vb, int vertexOffset, Material material)
            : base(root)
        {
            mIndexBuffer = ib;
            mNumVertices = numVerts;
            mPrimitiveCount = primitiveCount;
            mStartIndex = startIndex;
            mVertexBuffer = vb;
            mVertexOffset = vertexOffset;
            mVertexOffsetBytes = mVertexOffset * mVertexBuffer.VertexDeclaration.VertexStride;
            mMaterial = material;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the index buffer of the mesh part.
        /// </summary>
        public IndexBuffer IndexBuffer
        {
            get { return mIndexBuffer; }
        }

        /// <summary>
        /// Gets the number of vertices in the mesh part.
        /// </summary>
        public int NumVertices
        {
            get { return mNumVertices; }
        }

        /// <summary>
        /// Gets the number of primitives in a single draw call.
        /// </summary>
        public int PrimitiveCount
        {
            get { return mPrimitiveCount; }
        }

        /// <summary>
        /// Gets the location in the index array at which to start reading vertices.
        /// </summary>
        public int StartIndex
        {
            get { return mStartIndex; }
        }

        /// <summary>
        /// Gets the vertex buffer of the mesh part.
        /// </summary>
        public VertexBuffer VertexBuffer
        {
            get { return mVertexBuffer; }
        }

        /// <summary>
        /// Gets the offset (in vertices) from the top of vertex buffer.
        /// </summary>
        public int VertexOffset
        {
            get { return mVertexOffset; }
        }

        /// <summary>
        /// Gets the offset (in vertices) from the top of vertex buffer.
        /// </summary>
        public int VertexOffsetBytes
        {
            get { return mVertexOffsetBytes; }
        }
        
        /// <summary>
        /// Gets or sets the parent node of this mesh part.
        /// </summary>
        public MeshNode ParentMesh
        {
            get { return mParentMesh; }
            set { mParentMesh = value; }
        }

        #endregion

        #region IDrawable members

        /// <summary>
        /// Draws the geometry of the mesh part.
        /// </summary>
        public void Draw()
        {
#if DEBUG
            try
            {
                //Check to see if the index buffer is null.
                if (mIndexBuffer == null)
                {
                    throw new Exception("null index buffer.");
                }

                //Check to see if the vertex buffer is null.
                if (mVertexBuffer == null)
                {
                    throw new Exception("null vertex buffer.");
                }
#endif
                //Get the graphics device.
                GraphicsDevice device = Root.GraphicsDevice;
                //Set the index buffer.
                device.Indices = mIndexBuffer;
                //Set the vertex buffer.
                device.SetVertexBuffer(mVertexBuffer, mVertexOffset);
                //Draw the geometry primitives.
                device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, mNumVertices,
                    mStartIndex, mPrimitiveCount);
#if DEBUG
            }
            catch (Exception e)
            {
                string msg = "MeshPart.Draw(): " + e.Message;
                throw new Exception(msg);
            }
#endif
        }

        /// <summary>
        /// Gets or sets the material used to draw this mesh part.
        /// </summary>
        public Material Material
        {
            get { return mMaterial; }
            set { mMaterial = value; }
        }

        /// <summary>
        /// Gets or sets the layer to which the mesh part belongs.
        /// </summary>
        /// <value>
        /// The layer to which the mesh part belongs.
        /// </value>
        public int Layer
        {
            get { return mParentMesh.Layer; }
            set
            {
                if (value != Layer)
                {
                    mParentMesh.Layer = value;
                    if (LayerChanged != null)
                    {
                        LayerChanged(this, new EventArgs());
                    }
                }
            }
        }

        /// <summary>
        /// Occurs when the layer property changes.
        /// </summary>
        public event EventHandler<EventArgs> LayerChanged;

        /// <summary>
        /// Gets or sets whether the object is visible or not.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public bool Visible
        {
            get
            {
                return mParentMesh.Visible;
            }
            set
            {
                if (value != Visible)
                {
                    mParentMesh.Visible = value;
                    if (VisibleChanged != null)
                    {
                        VisibleChanged(this, new EventArgs());
                    }
                }
            }
        }

        /// <summary>
        /// Occurs when the Visible property changed.
        /// </summary>
        public event EventHandler<EventArgs> VisibleChanged;

        /// <summary>
        /// Gets the node which contains the information needed to transform the object (position,
        /// rotation, scaling, etc) in order to draw it.
        /// </summary>
        /// <value>The node with the information needed to transform the drawable.</value>
        public SceneNode NodeWithTransforms
        {
            get { return mParentMesh; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this part casts shadows.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this part casts shadows; otherwise, <c>false</c>.
        /// </value>
        public bool CastShadows
        {
            get { return mParentMesh.CastShadows; }
            set
            {
                if (value != CastShadows)
                {
                    mParentMesh.CastShadows = value;
                    if (CastShadowsChanged != null)
                    {
                        CastShadowsChanged(this, new EventArgs());
                    }
                }
            }
        }

        /// <summary>
        /// Occurs when the CastShadows property changes.
        /// </summary>
        public event EventHandler<EventArgs> CastShadowsChanged;

        /// <summary>
        /// Gets or sets a value indicating whether the shadow caster casts shadows when it is 
        /// invisible.
        /// </summary>
        /// <value>
        /// <c>true</c> if the object casts shadows when it is invisible; otherwise, <c>false</c>.
        /// </value>
        public bool CastShadowsWhenInvisible
        {
            get { return mParentMesh.CastShadowsWhenInvisible; }
            set
            {
                if (value != CastShadowsWhenInvisible)
                {
                    mParentMesh.CastShadowsWhenInvisible = value;
                    if (CastShadowsWhenInvisibleChanged != null)
                    {
                        CastShadowsWhenInvisibleChanged(this, new EventArgs());
                    }
                }
            }
        }

        /// <summary>
        /// Occurs when the CastShadowsWhenInvisible property changes.
        /// </summary>
        public event EventHandler<EventArgs> CastShadowsWhenInvisibleChanged;

        #endregion
    }
}
