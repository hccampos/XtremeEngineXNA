using Microsoft.Xna.Framework.Graphics;
using XtremeEngineXNA.Content;
using XtremeEngineXNA.Scene;

namespace XtremeEngineXNA.EntityComponent.Components
{
    /// <summary>
    /// Default implementation of the IModelComponent interface.
    /// </summary>
    internal class DefaultModelComponent : NodeBasedComponent, IModelComponent
    {
        #region Attributes

        /// <summary>
        /// Scene node which contains the model of the entity.
        /// </summary>
        private ModelNode mModelNode;

        #endregion

        #region Public methods

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="root">Root object to which the component belongs.</param>
        /// <param name="name">Name of the component.</param>
        public DefaultModelComponent(Root root, string name)
            : base(root, name)
        {
            mModelNode = new ModelNode(root);
        }

        /// <summary>
        /// Loads the model from the specified file.
        /// </summary>
        /// <param name="file">File which is to be loaded.</param>
        public void LoadModel(string file)
        {
            mModelNode.DettachAll();
            Model model = Root.ContentManager.Load<Model>(file);
            mModelNode.GenerateFromXNAModel(file, model);
        }

        /// <summary>
        /// Generates the model to represent the entity from a previously loaded XNA model.
        /// </summary>
        /// <param name="xnaModel">Model which is to be added to the component.</param>
        public void GenerateFromXNAModel(Model xnaModel)
        {
            mModelNode.DettachAll();
            mModelNode.GenerateFromXNAModel("", xnaModel);
        }

        #endregion

        #region Private/Protected methods

        /// <summary>
        /// Attaches the node of the component to the node passed in the parameter.
        /// </summary>
        /// <param name="node">Node to which the node of the component is to be attached.</param>
        protected override void Attach(SceneNode node)
        {
            node.AttachChild(mModelNode);
        }

        /// <summary>
        /// Dettaches the node of the component from the node passed in the parameter.
        /// </summary>
        /// <param name="node">Node from which the node of the component is to be dettached.</param>
        protected override void Dettach(SceneNode node)
        {
            node.DettachChild(mModelNode);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the node which contains the model of the entity.
        /// </summary>
        public ModelNode ModelNode
        {
            get { return mModelNode; }
        }

        #endregion
    }
}
