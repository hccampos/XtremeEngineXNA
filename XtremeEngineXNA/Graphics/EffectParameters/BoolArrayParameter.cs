using Microsoft.Xna.Framework.Graphics;
using XtremeEngineXNA.Scene;
using System;

namespace XtremeEngineXNA.Graphics.EffectParameters
{
    /// <summary>
    /// Effect parameter which stores an array of <c>bool</c> values.
    /// </summary>
    public class BoolArrayParameter : XEffectParameter<bool[]>
    {
        /// <summary>
        /// Creates a new parameter.
        /// </summary>
        /// <param name="root">Root object to which the parameter belongs.</param>
        /// <param name="name">Name of the effect parameter.</param>
        /// <param name="values">Value of the effect parameter.</param>
        public BoolArrayParameter(Root root, string name, bool[] values) :
            base(root, name, values) { }
        
        /// <summary>
        /// Creates a new parameter.
        /// </summary>
        /// <param name="root">Root object to which the parameter belongs.</param>
        /// <param name="name">Name of the new parameter.</param>
        /// <param name="values">Value of the new parameter.</param>
        /// <param name="effect">Effect to which the parameter belongs.</param>
        public BoolArrayParameter(Root root, string name, bool[] values, Effect effect) : 
            base(root, name, values, effect) { }

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
                throw new Exception("BoolArrayParameter.SetOnEffect(): " + e.Message);
            }
#endif
        }
    }
}