using XtremeEngineXNA.Objects;
using XtremeEngineXNA.Scene;

namespace XtremeEngineXNA.EntityComponent.Components
{
    internal class DefaultText3DComponent : NodeBasedComponent, IText3DComponent
    {
        #region Attributes

        /// <summary>
        /// Model of the text.
        /// </summary>
        private Text3D mTextModel;

        #endregion

        #region Public methods

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="root">Root object to which the component belongs.</param>
        /// <param name="name">Name of the component.</param>
        public DefaultText3DComponent(Root root, string name)
            : base(root, name)
        {
            mTextModel = new Text3D(root);
        }

        #endregion

        #region Private/Protected methods

        /// <summary>
        /// Attaches the node of the component to the node passed in the parameter.
        /// </summary>
        /// <param name="node">Node to which the node of the component is to be attached.</param>
        protected override void Attach(SceneNode node)
        {
            node.AttachChild(mTextModel);
        }

        /// <summary>
        /// Dettaches the node of the component from the node passed in the parameter.
        /// </summary>
        /// <param name="node">Node from which the node of the component is to be dettached.</param>
        protected override void Dettach(SceneNode node)
        {
            node.DettachChild(mTextModel);
        }

        #endregion

        #region Properties

        /// <summary>
        /// 3D model of the text. This can be used to set materials and other properties.
        /// </summary>
        public Text3D TextModel
        {
            get { return mTextModel; }
        }

        /// <summary>
        /// Gets or sets the text which is displayed by the component.
        /// </summary>
        public string Text
        {
            get { return mTextModel.Text; }
            set { mTextModel.Text = value; }
        }

        #endregion
    }
}
