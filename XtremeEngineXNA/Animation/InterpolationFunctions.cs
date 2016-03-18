using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XtremeEngineXNA.Animation
{
    /// <summary>
    /// Delegate for a function which interpolates values.
    /// </summary>
    /// <param name="position">Current position of the animation (0: beginning -> 1: end).</param>
    /// <param name="initialValue">Initial value of the property.</param>
    /// <param name="finalValue">Final value of the property.</param>
    /// <returns>The current value of the property.</returns>
    public delegate T InterpolationFunctionDelegate<T>(double position, T initialValue, T finalValue);

    /// <summary>
    /// Class which contains some useful interpolation functions.
    /// </summary>
    public class InterpolationFunctions
    {
        /// <summary>
        /// Linearly interpolates a float value.
        /// </summary>
        /// <param name="position">Position where we want to interpolate (0: beginning -> 1: end).</param>
        /// <param name="initialValue">Initial value of the property.</param>
        /// <param name="finalValue">Final value of the property.</param>
        /// <returns>The current value of the property.</returns>
        public static float FloatLerp(double position, float initialValue, float finalValue)
        {
            return initialValue + (float)position * (finalValue - initialValue);
        }

        /// <summary>
        /// Linearly interpolates a double value.
        /// </summary>
        /// <param name="position">Position where we want to interpolate (0: beginning -> 1: end).</param>
        /// <param name="initialValue">Initial value of the property.</param>
        /// <param name="finalValue">Final value of the property.</param>
        /// <returns>The current value of the property.</returns>
        public static double DoubleLerp(double position, double initialValue, double finalValue)
        {
            return initialValue + position * (finalValue - initialValue);
        }

        /// <summary>
        /// Linearly interpolates a double value.
        /// </summary>
        /// <param name="position">Position where we want to interpolate (0: beginning -> 1: end).</param>
        /// <param name="initialValue">Initial value of the property.</param>
        /// <param name="finalValue">Final value of the property.</param>
        /// <returns>The current value of the property.</returns>
        public static Vector3 Vector3Lerp(double position, Vector3 initialValue, Vector3 finalValue)
        {
            return initialValue + (float)position * (finalValue - initialValue);
        }
    }
}
