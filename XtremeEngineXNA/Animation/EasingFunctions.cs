using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XtremeEngineXNA.Animation
{
    /// <summary>
    /// Delegate for a function used to ease the animation.
    /// </summary>
    /// <param name="position">Current position of the animation.</param>
    /// <returns>The eased position of the animation.</returns>
    public delegate double EasingFunctionDelegate(double position);

    /// <summary>
    /// Class which contains a few useful easing functions.
    /// </summary>
    public class EasingFunctions
    {
        /// <summary>
        /// In-out cubic easing function.
        /// </summary>
        /// <param name="position">Current position of the animation.</param>
        /// <returns>The eased position of the animation.</returns>
        public static double EaseInOutCubic(double position)
        {
            double square = position * position;
            double cubic = square * position;

            return -2 * cubic + 3 * square;
        }

        /// <summary>
        /// Out elastic easing function.
        /// </summary>
        /// <param name="position">Current position of the animation.</param>
        /// <returns>The eased position of the animation.</returns>
        public static double EaseOutElastic(double position)
        {
            double ts = position * position;
            double tc = ts * position;

            return 33 * tc * ts - 106 * ts * ts + 126 * tc - 67 * ts + 15 * position; 
        }

       /* function(t:Number, b:Number, c:Number, d:Number):Number {
	var ts:Number=(t/=d)*t;
	var tc:Number=ts*t;
	return b+c*(33*tc*ts + -106*ts*ts + 126*tc + -67*ts + 15*t);
}*/
    }
}
