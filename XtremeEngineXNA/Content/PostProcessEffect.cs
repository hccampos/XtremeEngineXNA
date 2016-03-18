using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using XtremeEngineXNA.Graphics.EffectParameters;

namespace XtremeEngineXNA.Content
{
    /// <summary>
    /// Class which represents a post-processing effect.
    /// </summary>
    public class PostProcessEffect : XEffect
    {
        /// <summary>
        /// Creates a new post-processing effect from an existing XNA effect.
        /// </summary>
        /// <param name="root">Root object to which the effect belongs.</param>
        public PostProcessEffect(Root root) : base(root) { }

        /// <summary>
        /// Creates a new post-processing effect from an existing XNA effect.
        /// </summary>
        /// <param name="root">Root object to which the effect belongs.</param>
        /// <param name="effect">XNA Effect used by the effect.</param>
        /// <param name="technique">Name of the technique used by the effect.</param>
        /// <param name="parameters">
        /// List of effect parameters which are to be set when the effect is applied.
        /// </param>
        public PostProcessEffect(Root root, Effect effect, string technique,
            List<IEffectParameter> parameters)
            : base(root, effect, technique, parameters) { }

        /// <summary>
        /// Creates a new post-processing effect by loading an XNA effect from a file.
        /// </summary>
        /// <param name="root">Root object to which the material belongs.</param>
        /// <param name="effect">File name of the effect used by the material.</param>
        /// <param name="technique">Name of the technique used by the material.</param>
        /// <param name="parameters">
        /// List of effect parameters which are to be set on the effect.
        /// </param>
        public PostProcessEffect(Root root, string effect, string technique,
            List<IEffectParameter> parameters) :
            base(root, null, technique, parameters)
        {
            this.Effect = root.ContentManager.Load<Effect>(effect);
        }
    }
}
