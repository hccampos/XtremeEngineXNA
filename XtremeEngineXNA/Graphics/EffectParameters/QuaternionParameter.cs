using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XtremeEngineXNA.Scene;
using System;

namespace XtremeEngineXNA.Graphics.EffectParameters
{
    /// <summary>
    /// Effect parameter which stores a Quaternion.
    /// </summary>
    public class QuaternionParameter : XEffectParameter<Quaternion>
    {
        /// <summary>
        /// Creates a new parameter.
        /// </summary>
        /// <param name="root">Root object to which the parameter belongs.</param>
        /// <param name="name">Name of the effect parameter.</param>
        /// <param name="value">Value of the effect parameter.</param>
        public QuaternionParameter(Root root, string name, Quaternion value) :
            base(root, name, value) { }

        /// <summary>
        /// Creates a new parameter.
        /// </summary>
        /// <param name="root">Root object to which the parameter belongs.</param>
        /// <param name="name">Name of the new parameter.</param>
        /// <param name="value">Value of the new parameter.</param>
        /// <param name="effect">Effect to which the parameter belongs.</param>
        public QuaternionParameter(Root root, string name, Quaternion value, Effect effect) :
            base(root, name, value, effect) { }

        /// <summary>
        /// Sets the value of the parameter on its effect.
        /// </summary>
        /// <param name="node">Node from which information can be retrieved.</param>
        public override void SetOnEffect(SceneNode node)
        {
#if DEBUG
            try
            {
#endif
                Parameter.SetValue(Value);
#if DEBUG
            }
            catch (Exception e)
            {
                throw new Exception("QuaternionParameter.SetOnEffect(): " + e.Message);
            }
#endif
        }
    }
}