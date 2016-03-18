using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XtremeEngineXNA.Graphics.EffectParameters;

namespace XtremeEngineXNA.Content.PostProcessEffects
{
    /// <summary>
    /// Effect which blurs out the scene.
    /// </summary>
    public class BlurEffect : PostProcessEffect
    {
        #region Attributes

        /// <summary>
        /// Whether the effect is to be applied as a first blur pass.
        /// </summary>
        private bool mIsFirstPass;

        /// <summary>
        /// How much the image will be blurred by the effect.
        /// </summary>
        private FloatParameter mBlurDistanceParam;

        #endregion

        #region BlurEffect public members

        /// <summary>
        /// Creates a new blur post process effect.
        /// </summary>
        /// <param name="root">Root object to which the material belongs.</param>
        /// <param name="isFirstPass">Whether the effect is to be applied as a first blur pass.</param>
        /// <param name="blurDistance">How much the image will be blurred by the effect.</param>
        public BlurEffect(Root root, bool isFirstPass = true, float blurDistance = 0.003f)
            : base(root)
        {
            mIsFirstPass = isFirstPass;
            mBlurDistanceParam = new FloatParameter(root, "blurDistance", blurDistance);

            Setup();
        }

        #endregion

        #region BlurEffect private members

        /// <summary>
        /// Adds the required effect parameters to the effect.
        /// </summary>
        private void Setup()
        {
            AddParameter(mBlurDistanceParam);

            if (mIsFirstPass)
            {
                AddParameter(new EngineParameter(Root, "sceneTexture", EngineParameterType.PARAM_SCENE_TEXTURE));
            }
            else
            {
                AddParameter(new EngineParameter(Root, "sceneTexture", EngineParameterType.PARAM_PREVIOUS_RT_TEXTURE));
            }

            this.TechniqueName = "Render";
            this.Effect = Root.ContentManager.Load<Effect>("Effects/BlurEffect");
        }

        #endregion

        #region Properties

        /// <summary>
        /// The distance used to sample neighbor pixels. The larger this value is,
        /// the more blurred the image will be.
        /// </summary>
        /// <value>The distance used to sample neighbor pixels.</value>
        public float BlurDistance
        {
            get { return mBlurDistanceParam.Value; }
            set { mBlurDistanceParam.Value = value; }
        }

        #endregion
    }
}
