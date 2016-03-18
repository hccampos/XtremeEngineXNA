using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using XtremeEngineXNA.Scene;

namespace XtremeEngineXNA.Content
{
    /// <summary>
    /// Class which represents a mesh. A mesh represents a physical entity and is usually attached 
    /// to a model node. We can move meshes just like any other scene node.
    /// </summary>
    public class MeshNode : SceneNode, IDrawableContainer
    {
        #region Attributes

        /// <summary>
        /// List which stores the MeshPart objects that contain the geometry of the mesh.
        /// </summary>
        private List<IDrawableObject> mMeshParts;

        /// <summary>
        /// Matrix which transforms the mesh in relation to its parent model.
        /// </summary>
        private Matrix mMeshTransformMatrix;

        /// <summary>
        /// Layer to which the mesh belongs.
        /// </summary>
        private int mLayer = 0;

        /// <summary>
        /// Whether the mesh is visible or not.
        /// </summary>
        private bool mVisible = true;

        /// <summary>
        /// Whether the mesh casts shadows or not.
        /// </summary>
        private bool mCastShadows = false;

        /// <summary>
        /// Whether the mesh casts shadows even when it is invisible.
        /// </summary>
        private bool mCastShadowsWhenInvisible = false;

        #endregion

        #region MeshNode Members

        /// <summary>
        /// Creates a new mesh node.
        /// </summary>
        /// <param name="root">Root object to which the node belongs.</param>
        /// <param name="meshParts">MeshPart objects that contain the geometry of the mesh.</param>
        public MeshNode(Root root, List<MeshPart> meshParts) : base(root)
        {
            mMeshParts = new List<IDrawableObject>(meshParts);
            mMeshTransformMatrix = Matrix.Identity;

            //Set this mesh part as the parent of each mesh part.
            foreach (MeshPart part in mMeshParts)
            {
                part.ParentMesh = this;
            }
        }

        /// <summary>
        /// Applies the mesh's transform in relation to the parent model.
        /// </summary>
        /// <param name="matrix">Transformation matrix inherited from the node's parent.</param>
        public override void UpdateMatrices(Matrix matrix)
        {
            base.UpdateMatrices(matrix);

            //Apply the mesh transformation matrix to the node's local and world matrices.
            LocalTransformMatrix = mMeshTransformMatrix * LocalTransformMatrix;
            WorldTransformMatrix = mMeshTransformMatrix * WorldTransformMatrix;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Sets the material on each part of this mesh.
        /// </summary>
        /// <value>The material.</value>
        public Material Material
        {
            set
            {
                foreach (MeshPart part in mMeshParts)
                {
                    part.Material = value;
                }
            }
        }

        /// <summary>
        /// Returns a list with all the parts that make up the mesh.
        /// </summary>
        /// <value>The mesh parts list.</value>
        public List<MeshPart> MeshPartsList
        {
            get
            {
                List<MeshPart> result = new List<MeshPart>(mMeshParts.Count);
                foreach (MeshPart part in mMeshParts)
                {
                    result.Add(part);
                }

                return result;
            }
        }

        /// <summary>
        /// Gets or sets the mesh transform matrix.
        /// </summary>
        /// <value>The mesh transform matrix.</value>
        public Matrix MeshTransformMatrix
        {
            get { return mMeshTransformMatrix; }
            set { mMeshTransformMatrix = value; }
        }

        #endregion

        #region IDrawableContainer Members

        /// <summary>
        /// Gets the drawables in the container.
        /// </summary>
        /// <value>The drawables in the container.</value>
        public List<IDrawableObject> Drawables
        {
            get { return mMeshParts; }
        }

        /// <summary>
        /// Gets the visible drawables in the container.
        /// </summary>
        /// <value>The visible drawables in the container.</value>
        public List<IDrawableObject> VisibleDrawables
        {
            get
            {
                if (this.Visible)
                {
                    return mMeshParts;
                }
                else
                {
                    return new List<IDrawableObject>();
                }
            }
        }

        /// <summary>
        /// Gets or sets the layer to which the drawables in the container belong.
        /// </summary>
        /// <value>
        /// The layer to which the drawables in the container belong.
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
        /// Gets or sets whether the mesh node is visible or not.
        /// </summary>
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
        /// No description.
        /// </summary>
        public event EventHandler<EventArgs> VisibleChanged;

        /// <summary>
        /// Gets the shadow casters in the container.
        /// </summary>
        /// <value>The shadow casters in the container.</value>
        public List<IDrawableObject> ShadowCasters
        {
            get { return mMeshParts; }
        }

        /// <summary>
        /// Gets the shadow casters whose CastShadows property is true.
        /// </summary>
        /// <value>The shadow casters whose CastShadows property is true.</value>
        public List<IDrawableObject> EnabledShadowCasters
        {
            get
            {
                if (this.CastShadows)
                {
                    return mMeshParts;
                }
                else
                {
                    return new List<IDrawableObject>();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this mesh casts shadows.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this mesh casts shadows; otherwise, <c>false</c>.
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
        /// Gets or sets a value indicating whether the mesh casts shadows when it is invisible.
        /// </summary>
        /// <value>
        /// <c>true</c> if the mesh casts shadows when it is invisible; otherwise, <c>false</c>.
        /// </value>
        public bool CastShadowsWhenInvisible
        {
            get { return mCastShadowsWhenInvisible; }
            set
            {
                if (value != CastShadowsWhenInvisible)
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
