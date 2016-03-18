using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XtremeEngineXNA.Content.PostProcessEffects
{
    /// <summary>
    /// Effect chain which applies a depth of field effect to
    /// the scene.
    /// </summary>
    public class DOFEffectChain : PostProcessEffectChain
    {
        #region Attributes

        /// <summary>
        /// Depth-of-field effect.
        /// </summary>
        private DOFEffect mDOFEffect;

        #endregion

        #region DOFEffectChain public members

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="root">Root object which will use the effect chain.</param>
        public DOFEffectChain(Root root) : base(root)
        {
            mDOFEffect = new DOFEffect(root);
            AddEffect(new BlurEffect(root, true, 0.001f));
            AddEffect(new BlurEffect(root, false, 0.002f));
            AddEffect(new BlurEffect(root, false, 0.003f));
            AddEffect(mDOFEffect);
        }

        #endregion

        #region Properties

        /// <summary>
        /// The distance from the camera at which objects are focused.
        /// </summary>
        /// <value>The distance from the camera at which objects are focused.</value>
        public float Distance
        {
            get { return mDOFEffect.Distance; }
            set { mDOFEffect.Distance = value; }
        }

        /// <summary>
        /// Range of the focused area.
        /// </summary>
        public float Range
        {
            get { return mDOFEffect.Range; }
            set { mDOFEffect.Range = value; }
        }

        #endregion
    }
}
