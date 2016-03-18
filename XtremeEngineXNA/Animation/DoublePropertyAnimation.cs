using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XtremeEngineXNA.Animation
{
    /// <summary>
    /// Animation used to animate a double property.
    /// </summary>
    public class DoublePropertyAnimation : PropertyAnimation<double>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="target">Object on which a property is to be animated.</param>
        /// <param name="property">Property which is to be animated.</param>
        /// <param name="initialValue">Initial value of the property.</param>
        /// <param name="finalValue">Final value of the property.</param>
        /// <param name="duration">Duration of the animation (in seconds).</param>
        /// <param name="easingFunction">Function used to ease the animation.</param>
        /// <param name="removeOnCompletion">
        /// Whether the animation should be removed from the animation manager upon completion.
        /// </param>
        public DoublePropertyAnimation(object target, string property, double initialValue, double finalValue,
            int duration, EasingFunctionDelegate easingFunction = null, bool removeOnCompletion = false)
            :base(target, property, initialValue, finalValue, duration, easingFunction, removeOnCompletion)
        {
            this.InterpolationFunction = InterpolationFunctions.DoubleLerp;
        }
    }
}
