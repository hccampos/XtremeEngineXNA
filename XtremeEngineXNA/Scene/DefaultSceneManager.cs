using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XtremeEngineXNA;
using XtremeEngineXNA.Graphics;
using XtremeEngineXNA.Scene;

namespace XtremeEngineXNA.Scene
{
    /// <summary>
    /// Default scene manager for XtremeEngine.
    /// </summary>
    internal class DefaultSceneManager : SceneManagerBase
    {
        #region Attributes

        /// <summary>
        /// Scene node which is the root of the scene graph.
        /// </summary>
        private SceneNode mRootSceneNode;

        /// <summary>
        /// Camera which is currently active.
        /// </summary>
        private CameraNode mActiveCamera;

        #endregion

        #region DefaultSceneManager members

        /// <summary>
        /// Creates a new default scene manager.
        /// </summary>
        /// <param name="root">Root object to which the new scene manager belongs.</param>
        /// <param name="name">Name of the new scene manager.</param>
        public DefaultSceneManager(Root root, string name) : base(root, name)
        {
            mRootSceneNode = new SceneNode(root);
            mActiveCamera = null;
        }

        /// <summary>
        /// Initializes the scene manager.
        /// </summary>
        public override void Initialize()
        {
            //do nothing;
        }

        public override void Destroy()
        {
            mRootSceneNode = null;
            mActiveCamera = null;
        }

        /// <summary>
        /// Sets which camera is active.
        /// </summary>
        /// <param name="camera">Camera which is to be the active camera.</param>
        public override void SetActiveCamera(CameraNode camera)
        {
            //If the camera was found we set it as the active camera.
            ActiveCamera = camera;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the root of the scene manager's scene graph.
        /// </summary>
        public override SceneNode RootSceneNode
        {
            get { return mRootSceneNode; }
        }

        /// <summary>
        /// Gets the list of nodes which are to be drawn.
        /// </summary>
        /// <value>The list of nodes which are to be drawn.</value>
        public override List<IDrawableObject> VisibleDrawableNodes
        {
            get
            {
                List<IDrawableObject> drawables = new List<IDrawableObject>();
                foreach (Node node in mRootSceneNode.Children)
                {
                    GetVisibleDrawableNodes(node, ref drawables);
                }
                return drawables;
            }
        }

        /// <summary>
        /// Gets the list of nodes which cast shadows.
        /// </summary>
        /// <value>The list of nodes which cast shadows.</value>
        public override List<IDrawableObject> ShadowCastersList
        {
            get
            {
                List<IDrawableObject> shadowCasters = new List<IDrawableObject>();
                foreach (Node node in mRootSceneNode.Children)
                {
                    GetShadowCasters(node, ref shadowCasters);
                }
                return shadowCasters;
            }
        }

        /// <summary>
        /// Gets the list of lights which are to be used to draw the scene.
        /// </summary>
        public override List<LightNode> LightsList
        {
            get
            {
                List<LightNode> lights = new List<LightNode>();
                foreach (Node node in mRootSceneNode.Children)
                {
                    GetLightNodes(node, ref lights);
                }
                return lights;
            }
        }

        /// <summary>
        /// Gets/Sets the camera which is currently active.
        /// </summary>
        public override CameraNode ActiveCamera
        {
            get { return mActiveCamera; }
            set { mActiveCamera = value; }
        }

        /// <summary>
        /// Returns a list with all the cameras in the scene.
        /// </summary>
        public override List<CameraNode> CamerasList
        {
            get
            {
                List<CameraNode> cameras = new List<CameraNode>();
                foreach (Node n in mRootSceneNode.Descendants)
                {
                    CameraNode camera = n as CameraNode;
                    if (camera != null)
                    {
                        cameras.Add(camera);
                    }
                }

                return cameras;
            }
        }

        #endregion

        #region DefaultSceneManager private members

        /// <summary>
        /// Fills a list with all the children of a node which are drawable and visible.
        /// </summary>
        /// <param name="node">Node which is to be recursively searched.</param>
        /// <param name="drawables">List to be filled with the drawable nodes.</param>
        private void GetVisibleDrawableNodes(Node node, ref List<IDrawableObject> drawables)
        {
            //Verify if the node is an IDrawable.
		    IDrawableObject drawable = node as IDrawableObject;

		    //Add the node if it is drawable and visible.
		    if(drawable != null && drawable.Visible)
		    {
			    //Add the node to the list of drawable nodes.
			    drawables.Add(drawable);
            }

            //If the node is a drawable container we add all of the visible drawables in it.
            IDrawableContainer drawableContainer = node as IDrawableContainer;
            if (drawableContainer != null && drawableContainer.Visible)
            {
                drawables.AddRange(drawableContainer.VisibleDrawables);
            }

            //If the node is not a drawable or, if it is drawable and is visible, we check all its
            //children.
            if (drawable == null || drawable.Visible)
            {
                //Search the node's children.
                foreach (Node n in node.Children)
                {
                    GetVisibleDrawableNodes(n, ref drawables);
                }
            }
        }

        /// <summary>
        /// Fills a list with all the children of a node which are shadow casters and whose
        /// CastShadows and Visible properties are true or whose CastShadowsWhenInvisible and
        /// CastShadows properties are true.
        /// </summary>
        /// <param name="node">Node which is to be recursively searched.</param>
        /// <param name="shadowCasters">List to be filled with the shadow casters.</param>
        private void GetShadowCasters(Node node, ref List<IDrawableObject> shadowCasters)
        {
            //Verify if the node is an IShadowCaster.
            IDrawableObject shadowCaster = node as IDrawableObject;

            //Add the node if it is a shadow caster and its Visible and CastShadows properties are
            //true or if it is a shadow caster and its Visible, CastShadows and
            //CastShadowsWhenInvisible properties are true.
            if (shadowCaster != null && shadowCaster.CastShadows)
            {
                if (shadowCaster.Visible || shadowCaster.CastShadowsWhenInvisible)
                {
                    //Add the node to the list of shadow casters.
                    shadowCasters.Add(shadowCaster);
                }
            }

            //If the node is a shadow caster container we add all of the shadow casters in it which
            //are active shadow casters and visible or whose CastShadowsWhenInvisible property is
            //true.
            IDrawableContainer shadowCasterContainer = node as IDrawableContainer;
            if (shadowCasterContainer != null && shadowCasterContainer.CastShadows &&
                (shadowCasterContainer.Visible || shadowCasterContainer.CastShadowsWhenInvisible))
            {
                //Chech each shadow caster in the container because some may be deactivated or
                //invisible (For the MeshNode the shadow caster properties always match the
                //container properties, but that need not be the case).
                foreach (IDrawableObject c in shadowCasterContainer.EnabledShadowCasters)
                {
                    if (shadowCasterContainer.Visible || shadowCasterContainer.CastShadowsWhenInvisible)
                    {
                        shadowCasters.Add(c);
                    }
                }
            }

            //Check the children.
            if (shadowCaster == null || (shadowCaster.CastShadows && 
                (shadowCaster.Visible || shadowCaster.CastShadowsWhenInvisible)))
            {
                //Search the node's children.
                foreach (Node n in node.Children)
                {
                    GetShadowCasters(n, ref shadowCasters);
                }
            }
        }

        /// <summary>
        /// Fills a list with all the children of a node which are lights.
        /// </summary>
        /// <param name="node">Node which is to be recursively searched.</param>
        /// <param name="lights">List to be filled with the light nodes.</param>
        private void GetLightNodes(Node node, ref List<LightNode> lights)
        {
            //Verify if the node is an IDrawable.
		    IDrawableObject drawable = node as IDrawableObject;
		    //If the node is an IDrawable which is invisible we return because we don't want to add 
            //it (or its children) to the list of lights.
		    if(drawable != null && !drawable.Visible)
            {
			    return;
            }

		    //Here we know that the node is not an IDrawable or, if it is, it is also visible so we 
		    //check to see if it is a light.
		    LightNode light = node as LightNode;

		    //Only add the node if it is a light and it is on.
		    if(light != null && light.IsOn)
		    {
			    //Add the node to the list with all the lights.
			    lights.Add(light);
		    }

            //Check all the children.
            foreach(Node n in node.Children)
            {
                GetLightNodes(n, ref lights);
            }
        }

        /// <summary>
        /// Updates a node and all its children.
        /// </summary>
        /// <param name="elapsedTime">Time elapsed since the last update.</param>
        /// <param name="node">Node which is to be updated.</param>
        private void UpdateNode(TimeSpan elapsedTime, Node node)
        {
            //Verify if the node is updateable.
		    IUpdateable updateable = node as IUpdateable;
		    //Only update the node and it's children if the node is enabled.
		    if(updateable != null && updateable.Enabled)
		    {
			    //Update the node.
                updateable.Update(elapsedTime);

                foreach(Node n in node.Children)
                {
                    UpdateNode(elapsedTime, n);
                }
		    }
        }

        #endregion

        #region IUpdateable members

        /// <summary>
        /// Updates the scene manager.
        /// </summary>
        /// <param name="elapsedTime">Time elapsed since the last update.</param>
        public override void Update(TimeSpan elapsedTime)
        {
            //Update the matrices of the nodes in the scene graph.
            mRootSceneNode.UpdateMatrices(Matrix.Identity);
            //Update all the updateable nodes.
            this.UpdateNode(elapsedTime, mRootSceneNode);
        }

        #endregion
    }
}
