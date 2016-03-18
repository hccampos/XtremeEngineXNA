using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XtremeEngineXNA.Scene;
using System;

namespace XtremeEngineXNA.Graphics.EffectParameters
{
    /// <summary>
    /// Effect parameter which stores a Matrix.
    /// </summary>
    public class MatrixParameter : XEffectParameter<Matrix>
    {
        /// <summary>
        /// Creates a new parameter.
        /// </summary>
        /// <param name="root">Root object to which the parameter belongs.</param>
        /// <param name="name">Name of the effect parameter.</param>
        /// <param name="value">Value of the effect parameter.</param>
        public MatrixParameter(Root root, string name, Matrix value) :
            base(root, name, value) { }

        /// <summary>
        /// Creates a new parameter.
        /// </summary>
        /// <param name="root">Root object to which the parameter belongs.</param>
        /// <param name="name">Name of the new parameter.</param>
        /// <param name="value">Value of the new parameter.</param>
        /// <param name="effect">Effect to which the parameter belongs.</param>
        public MatrixParameter(Root root, string name, Matrix value, Effect effect) :
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
                throw new Exception("MatrixParameter.SetOnEffect(): " + e.Message);
            }
#endif
        }
    }
}