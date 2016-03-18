using XtremeEngineXNA.Scene;
using XtremeEngineXNA.EntityComponent.Components;

namespace XtremeEngineXNA.EntityComponent
{
    /// <summary>
    /// Abstract class which helps implement components that attach a scene node to the scene
    /// node of the spatial component of an entity. This component automatically checks if the
    /// spatial component is added to the entity and only attaches/dettaches nodes if they are
    /// available.
    /// </summary>
    public abstract class NodeBasedComponent : EntityComponent
    {
        #region Attributes

        /// <summary>
        /// Scene node to which the node of the component is attached.
        /// </summary>
        private SceneNode mSceneNode;

        #endregion

        #region Public methods

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="root">Root object to which the component belongs.</param>
        /// <param name="name">Name of the component.</param>
        public NodeBasedComponent(Root root, string name)
            : base(root, name)
        {
            mSceneNode = null;
        }

        /// <summary>
        /// Called when another component is added or removed from the entity. This method should
        /// be used by the component to aquire references to other components in the entity.
        /// </summary>
        public override void OnReset()
        {
            base.OnReset();

            ISpatialComponent spatial = this.Owner.GetComponent<ISpatialComponent>();
            if (spatial != null)
            {
                SceneNode oldNode = mSceneNode;
                mSceneNode = spatial.SceneNode;

                if (mSceneNode != oldNode) // Only attach/dettach if the node has changed.
                {
                    // Dettach the component's node from the old scene node (if any).
                    if (oldNode != null)
                        Dettach(oldNode);

                    // And attach it to the new scene node (if any).
                    if (mSceneNode != null)
                        Attach(mSceneNode);
                }
            }
            // If there is no spatial component, we dettach the quad.
            else
            {
                if (mSceneNode != null)
                    Dettach(mSceneNode);
            }
        }

        /// <summary>
        /// Called when the component is removed from an entity.
        /// </summary>
        public override void OnRemove()
        {
            base.OnRemove();

            if (mSceneNode != null)
                Dettach(mSceneNode);

            mSceneNode = null;
        }

        #endregion

        #region Private/Protected methods

        /// <summary>
        /// Attaches the node of the component to the node passed in the parameter.
        /// </summary>
        /// <param name="node">Node to which the node of the component is to be attached.</param>
        protected abstract void Attach(SceneNode node);

        /// <summary>
        /// Dettaches the node of the component from the node passed in the parameter.
        /// </summary>
        /// <param name="node">Node from which the node of the component is to be dettached.</param>
        protected abstract void Dettach(SceneNode node);

        /// <summary>
        /// Gets the scene node of the spatial component of the entity to which the component is
        /// added (if any).
        /// </summary>
        protected SceneNode SceneNode
        {
            get { return mSceneNode; }
        }

        #endregion
    }
}
