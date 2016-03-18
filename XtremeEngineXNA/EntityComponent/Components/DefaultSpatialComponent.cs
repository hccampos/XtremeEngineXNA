using XtremeEngineXNA.Scene;

namespace XtremeEngineXNA.EntityComponent.Components
{
    /// <summary>
    /// Default implementation of the <see cref="ISpatialComponent"/> interface.
    /// </summary>
    internal class DefaultSpatialComponent : EntityComponent, ISpatialComponent
    {
        #region Attributes

        /// <summary>
        /// Scene node which contains the position, rotation and scale for an entity.
        /// </summary>
        private SceneNode mSceneNode;

        #endregion

        #region Public methods

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="root">Root object to which the component belongs.</param>
        /// <param name="name">Name of the component.</param>
        public DefaultSpatialComponent(Root root, string name)
            : base(root, name)
        {
            mSceneNode = new SceneNode(root);
            root.SceneManager.RootSceneNode.AttachChild(mSceneNode);
        }

        /// <summary>
        /// Called when the component is added to an entity.
        /// </summary>
        public override void OnAdd()
        {
            base.OnAdd();

            this.Root.SceneManager.RootSceneNode.AttachChild(mSceneNode);
        }

        /// <summary>
        /// Called when the component is removed from an entity.
        /// </summary>
        public override void OnRemove()
        {
            base.OnRemove();

            this.Root.SceneManager.RootSceneNode.DettachChild(mSceneNode);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Scene node which contains the position, rotation and scale for an entity.
        /// </summary>
        public SceneNode SceneNode
        {
            get { return mSceneNode; }
        }

        #endregion

        #region Events

        #endregion
    }
}
