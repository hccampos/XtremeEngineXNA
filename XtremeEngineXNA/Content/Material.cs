using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using XtremeEngineXNA.Graphics.EffectParameters;

namespace XtremeEngineXNA.Content
{
    /// <summary>
    /// Class which represents a material. A material object is made up of an effect and the
    /// parameters that need to be set on it before drawing an object to achieve a certain look.
    /// </summary>
    public class Material : XEffect
    {
        /// <summary>
        /// Creates a new material from an existing effect.
        /// </summary>
        /// <param name="root">Root object to which the material belongs.</param>
        public Material(Root root) : base(root) { }

        /// <summary>
        /// Creates a new material from an existing effect.
        /// </summary>
        /// <param name="root">Root object to which the material belongs.</param>
        /// <param name="effect">Effect used by the material.</param>
        /// <param name="technique">Name of the technique used by the material.</param>
        /// <param name="parameters">
        /// List of effect parameters which are to be set on the effect.
        /// </param>
        public Material(Root root, Effect effect, string technique, 
            List<IEffectParameter> parameters):
            base(root, effect, technique, parameters) { }

        /// <summary>
        /// Creates a new material by loading an effect from a file.
        /// </summary>
        /// <param name="root">Root object to which the material belongs.</param>
        /// <param name="effect">File name of the effect used by the material.</param>
        /// <param name="technique">Name of the technique used by the material.</param>
        /// <param name="parameters">
        /// List of effect parameters which are to be set on the effect.
        /// </param>
        public Material(Root root, string effect, string technique,
            List<IEffectParameter> parameters) :
            base(root, null, technique, parameters)
        {
            this.Effect = root.ContentManager.Load<Effect>(effect);
        }
    }
}
