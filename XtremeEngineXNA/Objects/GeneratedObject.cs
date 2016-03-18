﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XtremeEngineXNA.Content;
using XtremeEngineXNA.Scene;

namespace XtremeEngineXNA.Objects
{
    /// <summary>
    /// Class which is the base of any generated objects such as quads or skyboxes. A 
    /// GeneratedObject is different from a model in that the geometry of the GeneratedObject is 
    /// generated by the engine instead of being loaded from a file.
    /// </summary>
    public class GeneratedObject : SceneNode, IDrawableObject
    {
        #region Attributes

        /// <summary>
        /// Index buffer which stores the indices of the vertices that make up the object.
        /// </summary>
        private IndexBuffer mIndexBuffer;

        /// <summary>
        /// Number of primitives in a single draw call.
        /// </summary>
        private int mPrimitiveCount;

        /// <summary>
        /// Vertex buffer which stores the geometry information of the object.
        /// </summary>
        private VertexBuffer mVertexBuffer;

        /// <summary>
        /// Material used to draw this object.
        /// </summary>
        private Material mMaterial;

        /// <summary>
        /// Type of primitives that make up the object.
        /// </summary>
        private PrimitiveType mPrimitiveType = PrimitiveType.TriangleList;

        /// <summary>
        /// Layer to which the object belongs.
        /// </summary>
        private int mLayer = 0;

        /// <summary>
        /// Whether the object is visible or not.
        /// </summary>
        private bool mVisible = true;

        /// <summary>
        /// Whther the object casts shadows or not.
        /// </summary>
        private bool mCastShadows = false;

        /// <summary>
        /// Whether the object casts shadows even when it is invisible.
        /// </summary>
        private bool mCastShadowsWhenInvisible = false;

        #endregion

        #region DrawableObject members

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneratedObject"/> class.
        /// </summary>
        /// <param name="root">Root object to which the new object belongs.</param>
        /// <param name="material">Material used to draw the object.</param>
        public GeneratedObject(Root root, Material material = null) : base(root)
        {
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
            set { mIndexBuffer = value; }
        }

        /// <summary>
        /// Gets the number of primitives in a single draw call.
        /// </summary>
        /// <value>The number of primitives in a single draw call.</value>
        public int PrimitiveCount
        {
            get { return mPrimitiveCount; }
            set { mPrimitiveCount = value; }
        }

        /// <summary>
        /// Gets the vertex buffer of the object.
        /// </summary>
        /// <value>The vertex buffer of the object.</value>
        public VertexBuffer VertexBuffer
        {
            get { return mVertexBuffer; }
            set { mVertexBuffer = value; }
        }

        /// <summary>
        /// Gets or sets the type of the primitives that make up the object.
        /// </summary>
        /// <value>The type of the primitives that make up the object.</value>
        public PrimitiveType PrimitiveType
        {
            get { return mPrimitiveType; }
            set { mPrimitiveType = value; }
        }

        #endregion

        #region IDrawable members

        /// <summary>
        /// Draws the geometry of the object.
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
                device.SetVertexBuffer(mVertexBuffer, 0);
                //Draw the geometry primitives.
                device.DrawIndexedPrimitives(mPrimitiveType, 0, 0, mVertexBuffer.VertexCount, 0, 
                    mPrimitiveCount);
#if DEBUG
            }
            catch (Exception e)
            {
                string msg = "DrawableObject.Draw(): " + e.Message;
                throw new Exception(msg);
            }
#endif
        }

        /// <summary>
        /// Gets or sets the material used to draw this mesh part.
        /// </summary>
        /// <value>The material used to draw the object.</value>
        public Material Material
        {
            get { return mMaterial; }
            set { mMaterial = value; }
        }

        /// <summary>
        /// Gets or sets the layer to which the object belongs.
        /// </summary>
        /// <value>
        /// The layer to which the object belongs.
        /// </value>
        public int Layer
        {
            get { return mLayer; }
            set
            {
                if (value != mLayer)
                {
                    mLayer = value;
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
            get { return mVisible; }
            set
            {
                if (value != mVisible)
                {
                    mVisible = value;
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
            get { return this; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this object casts shadows.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this object casts shadows; otherwise, <c>false</c>.
        /// </value>
        public bool CastShadows
        {
            get { return mCastShadows; }
            set
            {
                if (value != mCastShadows)
                {
                    mCastShadows = value;
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
        /// 	<c>true</c> if the object casts shadows when it is invisible; otherwise, <c>false</c>.
        /// </value>
        public bool CastShadowsWhenInvisible
        {
            get { return mCastShadowsWhenInvisible; }
            set
            {
                if (value != mCastShadowsWhenInvisible)
                {
                    mCastShadowsWhenInvisible = value;
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
