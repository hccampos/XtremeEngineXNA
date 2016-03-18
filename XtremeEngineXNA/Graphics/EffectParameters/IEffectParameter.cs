using Microsoft.Xna.Framework.Graphics;
using XtremeEngineXNA.Scene;

namespace XtremeEngineXNA.Graphics.EffectParameters
{
    /// <summary>
    /// Interface which defines the methods that an effect parameter must implement. An effect
    /// parameter is used to hold a certain value to be set on an effect. Each Material has a
    /// collection of effect parameters and before drawing an object, all of the effect parameters
    /// in the Material are set on the effect used by the Material (the Material calls
    /// <c>setOnEffect()</c> for each shader).
    /// </summary>
    public interface IEffectParameter
    {
        /// <summary>
        /// Sets the value of the parameter on its effect.
        /// </summary>
        /// <param name="node">Node from which information can be retrieved.</param>
        void SetOnEffect(SceneNode node);

        /// <summary>
        /// Gets the name of the effect parameter.
        /// </summary>
        string Name
        {
            get;
        }

        /// <summary>
        /// Gets the XNA effect parameter.
        /// </summary>
        EffectParameter Parameter
        {
            get;
        }

        /// <summary>
        /// Gets/Sets the effect to which this parameter belongs.
        /// </summary>
        Effect Effect
        {
            get;
            set;
        }
    }
}