using XtremeEngineXNA.Objects;
using XtremeEngineXNA.Scene;

namespace XtremeEngineXNA.EntityComponent.Components
{
    /// <summary>
    /// Default implementation of the IQuadComponent interface.
    /// </summary>
    internal class DefaultQuadComponent : NodeBasedComponent, IQuadComponent
    {
        #region Attributes

        /// <summary>
        /// Quad which represents the entity.
        /// </summary>
        private Quad mQuad;

        #endregion

        #region Public methods

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="root">Root object to which the component belongs.</param>
        /// <param name="name">Name of the component.</param>
        public DefaultQuadComponent(Root root, string name)
            : base(root, name)
        {
            mQuad = new Quad(root);
        }

        #endregion

        #region Private/Protected methods

        /// <summary>
        /// Attaches the node of the component to the node passed in the parameter.
        /// </summary>
        /// <param name="node">Node to which the node of the component is to be attached.</param>
        protected override void Attach(SceneNode node)
        {
            node.AttachChild(mQuad);
        }

        /// <summary>
        /// Dettaches the node of the component from the node passed in the parameter.
        /// </summary>
        /// <param name="node">Node from which the node of the component is to be dettached.</param>
        protected override void Dettach(SceneNode node)
        {
            node.DettachChild(mQuad);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the node which contains the model of the entity.
        /// </summary>
        public Quad Quad
        {
            get { return mQuad; }
        }

        #endregion
    }
}
