using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XtremeEngineXNA.Scene;

namespace XtremeEngineXNA.Content
{
    /// <summary>
    /// Class which represents a model that can be added to the scene manager just like a normal
    /// scene node.
    /// </summary>
    public class ModelNode : SceneNode, IDrawableObject
    {
        #region Attributes

        /// <summary>
        /// Name of the file from which the model is to be loaded.
        /// </summary>
        private string mFilename;

        /// <summary>
        /// Layer to which the model belongs.
        /// </summary>
        private int mLayer = 0;

        #endregion

        #region ModelNode Members

        /// <summary>
        /// Creates a new empty model node.
        /// </summary>
        /// <param name="root">Root object to which the model node belongs.</param>
        public ModelNode(Root root) : base(root)
        {
        }

        /// <summary>
        /// Creates a new model node by loading a model from a file.
        /// </summary>
        /// <param name="root">Root object to which the model node belongs.</param>
        /// <param name="filename">File from which the model is to be loaded.</param>
        public ModelNode(Root root, string filename) : base(root)
        {
            Model model = root.ContentManager.Load<Model>(filename);
            GenerateFromXNAModel(filename, model);
        }

        /// <summary>
        /// Creates a new model node from an XNA model.
        /// </summary>
        /// <param name="root">Root object to which the model node belongs.</param>
        /// <param name="xnaModel">XNA model from which the model node should be created.</param>
        public ModelNode(Root root, Model xnaModel) : base(root)
        {
            GenerateFromXNAModel("", xnaModel);
        }

        /// <summary>
        /// Initializes the model node.
        /// </summary>
        /// <param name="filename">File from which the model is to be loaded.</param>
        /// <param name="model">XNA model from which the model node should be created.</param>
        public virtual void GenerateFromXNAModel(string filename, Model model)
        {
            try
            {
                mFilename = filename;
                CreateNodes(model, model.Root, this);
            }
            catch (Exception e)
            {
                throw new Exception("ModelNode.InitializeModel(): " + e.Message);
            }  
        }

        /// <summary>
        /// Creates all the child MeshNodes of a certain node.
        /// </summary>
        /// <param name="model">Model from which the nodes are to be created.</param>
        /// <param name="bone">Bone being checked.</param>
        /// <param name="node">Node to which child nodes are to be added.</param>
        private void CreateNodes(Model model, ModelBone bone, SceneNode node)
        {
            bool childFound = false;

            //Go through the meshes in the model and try to find one whose parent bone is the
            //one for which nodes are being created.
            foreach (ModelMesh mesh in model.Meshes)
            {
                //If we found the mesh we were looking for...
                if (mesh.ParentBone == bone)
                {
                    childFound = true;

                    //Create the MeshPart objects from the XNA ModelMeshPart objects.
                    List<MeshPart> parts = new List<MeshPart>();
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        MeshPart newPart = new MeshPart(Root, part.IndexBuffer,
                            part.NumVertices, part.PrimitiveCount, part.StartIndex,
                            part.VertexBuffer, part.VertexOffset, null);
                        parts.Add(newPart);
                    }

                    //Create the new mesh node and attach it to the node to which we are
                    //currently adding children.
                    MeshNode newMesh = new MeshNode(Root, parts);
                    node.AttachChild(newMesh);

                    //Set the mesh transform matrix in the mesh node.
                    newMesh.MeshTransformMatrix = bone.Transform;

                    //Go through each child bone of the bone passed in the argument.
                    foreach (ModelBone b in bone.Children)
                    {
                        //Repeat the process but now check the children of the current bone and add
                        //the nodes to the new mesh node.
                        CreateNodes(model, b, newMesh);
                    }

                    //We already found a mesh whose parent bone matched the current bone so
                    //we can break and test another child bone or return.
                    break;
                }
            }

            if (!childFound)
            {
                foreach (ModelBone b in bone.Children)
                {
                    CreateNodes(model, b, node);
                }
            }
        }      

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this model casts shadows. A model casts shadows
        /// if any of its meshes casts shadows.
        /// </summary>
        /// <value><c>true</c> if this model casts shadows; otherwise, <c>false</c>.</value>
        public bool CastShadows
        {
            get
            {
                foreach (MeshNode mesh in MeshesList)
                {
                    if (mesh.CastShadows)
                    {
                        return true;
                    }
                }

                return false;
            }
            set
            {
                foreach (MeshNode mesh in MeshesList)
                {
                    mesh.CastShadows = value;
                }

                if (CastShadowsChanged != null)
                {
                    CastShadowsChanged(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the mesh casts shadows when it is invisible.
        /// </summary>
        /// <value>
        /// <c>true</c> if the mesh casts shadows when it is invisible; otherwise, <c>false</c>.
        /// </value>
        public bool CastShadowsWhenInvisible
        {
            get 
            {
                foreach (MeshNode mesh in MeshesList)
                {
                    if (mesh.CastShadowsWhenInvisible)
                    {
                        return true;
                    }
                }

                return false; 
            }
            set
            {
                foreach (MeshNode mesh in MeshesList)
                {
                    mesh.CastShadowsWhenInvisible = value;
                }

                if (CastShadowsWhenInvisibleChanged != null)
                {
                    CastShadowsWhenInvisibleChanged(this, new EventArgs());
                }
            }
        }       

        /// <summary>
        /// Sets the material on each part of each mesh of this model.
        /// </summary>
        /// <value>The material.</value>
        public Material Material
        {
            get
            {
                return null;
            }
            set
            {
                foreach (MeshNode mesh in MeshesList)
                {
                    mesh.Material = value;
                }
            }
        }

        /// <summary>
        /// Gets the name of the file from which the model was loaded.
        /// </summary>
        public string Filename
        {
            get { return mFilename; }
        }

        /// <summary>
        /// Returns all the mesh nodes of this model.
        /// </summary>
        /// <value>The meshes list.</value>
        public List<MeshNode> MeshesList
        {
            get
            {
                List<MeshNode> meshes = new List<MeshNode>();
                foreach (Node n in Descendants)
                {
                    MeshNode mesh = n as MeshNode;
                    if (mesh != null)
                    {
                        meshes.Add(mesh);
                    }
                }

                return meshes;
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the CastShadows property changes.
        /// </summary>
        public event EventHandler<EventArgs> CastShadowsChanged;

        /// <summary>
        /// Occurs when the CastShadowsWhenInvisible property changes.
        /// </summary>
        public event EventHandler<EventArgs> CastShadowsWhenInvisibleChanged;

        #endregion

        #region IDrawable Members

        /// <summary>
        /// Prepares the object for drawing (e.g. setting materials, device states, etc).
        /// </summary>
        /// <remarks>
        /// Everything related to geometry (i.e. setting vertex and index buffers, etc) should be
        /// done in the Draw() method.
        /// </remarks>
        public virtual void BeginDraw()
        {
            //Nothing to be done here because drawing is done by the MeshNode objects. Model node
            //is only an IDrawable so we can change the visibility of the whole model without
            //having to change the visibility of each individual mesh node.
        }

        /// <summary>
        /// Not implemented. Drawing is done by the MeshNode objects.
        /// </summary>
        public virtual void Draw()
        {
            //Nothing to be done here because drawing is done by the MeshNode objects. Model node
            //is only an IDrawable so we can change the visibility of the whole model without
            //having to change the visibility of each individual mesh node.
        }

        /// <summary>
        /// Ends the drawing of the object.
        /// </summary>
        public virtual void EndDraw()
        {
            //Nothing to be done here because drawing is done by the MeshNode objects. Model node
            //is only an IDrawable so we can change the visibility of the whole model without
            //having to change the visibility of each individual mesh node.
        }

        /// <summary>
        /// Gets or sets whether the model is visible or not.
        /// </summary>
        public bool Visible
        {
            get
            {
                foreach (MeshNode mesh in MeshesList)
                {
                    if (mesh.Visible)
                    {
                        return true;
                    }
                }

                return false;
            }
            set
            {
                foreach (MeshNode mesh in MeshesList)
                {
                    mesh.Visible = value;
                }

                if (VisibleChanged != null)
                {
                    VisibleChanged(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// No documentation.
        /// </summary>
        public event EventHandler<EventArgs> VisibleChanged;

        /// <summary>
        /// Layer in which the model is to be drawn.
        /// </summary>
        public int Layer
        {
            get
            {
                return mLayer;
            }
            set
            {
                if (LayerChanged != null)
                {
                    mLayer = value;
                    LayerChanged(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Event which indicates that the Layer to which the model belongs has changed.
        /// </summary>
        public event EventHandler<EventArgs> LayerChanged;

        /**
         * Node which contains the model transformation matrices.
         */
        public SceneNode NodeWithTransforms
        {
            get { return this; }
        }

        #endregion
    }
}
