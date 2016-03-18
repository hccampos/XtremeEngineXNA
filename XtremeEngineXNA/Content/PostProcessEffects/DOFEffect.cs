using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XtremeEngineXNA.Graphics.EffectParameters;

namespace XtremeEngineXNA.Content.PostProcessEffects
{
    /// <summary>
    /// Effect which simulates Depth-of-field.
    /// </summary>
    public class DOFEffect : PostProcessEffect
    {
        #region Attributes

        private FloatParameter mDistanceParam;
        private FloatParameter mRangeParam;

        #endregion

        #region DOFEffect public members

        /// <summary>
        /// Creates a new blur post process effect.
        /// </summary>
        /// <param name="root">Root object to which the material belongs.</param>
        /// <param name="distance">The distance from the camera at which objects are focused.</param>
        /// <param name="range">Range of the focused area.</param>
        public DOFEffect(Root root, float distance = 30.0f, float range = 10.0f) : base(root)
        {
            mDistanceParam = new FloatParameter(Root, "distance", distance);
            mRangeParam = new FloatParameter(Root, "range", range);

            Setup();
        }

        #endregion

        #region DOFEffect private members

        /// <summary>
        /// Adds the required effect parameters to the effect.
        /// </summary>
        private void Setup()
        {
            AddParameter(mDistanceParam);
            AddParameter(mRangeParam);
            AddParameter(new EngineParameter(Root, "nearPlane", EngineParameterType.PARAM_CAMERA_NEAR_PLANE));
            AddParameter(new EngineParameter(Root, "farPlane", EngineParameterType.PARAM_CAMERA_FAR_PLANE));
            AddParameter(new EngineParameter(Root, "depthTexture", EngineParameterType.PARAM_DEPTH_TEXTURE));
            AddParameter(new EngineParameter(Root, "sceneTexture", EngineParameterType.PARAM_SCENE_TEXTURE));
            AddParameter(new EngineParameter(Root, "blurredSceneTexture", EngineParameterType.PARAM_PREVIOUS_RT_TEXTURE));

            this.TechniqueName = "Render";
            this.Effect = Root.ContentManager.Load<Effect>("Effects/DOFEffect");
        }

        #endregion

        #region Properties

        /// <summary>
        /// The distance from the camera at which objects are focused.
        /// </summary>
        /// <value>The distance from the camera at which objects are focused.</value>
        public float Distance
        {
            get { return mDistanceParam.Value; }
            set { mDistanceParam.Value = value; }
        }

        /// <summary>
        /// Range of the focused area.
        /// </summary>
        public float Range
        {
            get { return mRangeParam.Value; }
            set { mRangeParam.Value = value; }
        }

        #endregion
    }
}
