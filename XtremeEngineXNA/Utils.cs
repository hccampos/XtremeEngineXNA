using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace XtremeEngineXNA
{
    /// <summary>
    /// Class which contains some useful static methods.
    /// </summary>
    public class Utils
    {
        /// <summary>
        /// Combines the keyboard and gamepad inputs. The gamepad input is only added to the final
        /// input if it counteracts the keyboard input or if there is no keyboard input at all.
        /// </summary>
        /// <param name="keyboardInput">The keyboard input.</param>
        /// <param name="gamepadInput">The gamepad input.</param>
        /// <returns>The combined input of the keyboard and the gamepad.</returns>
        public static float CombineKeyboardAndGamepad(float keyboardInput, float gamepadInput)
        {
            //We only add the gamepad input if it opposes the movement due to the keyboard input
            //or if there is no keyboard input.
            if (keyboardInput * gamepadInput < 0.0f)
            {
                return keyboardInput + gamepadInput;
            }
            else if (keyboardInput == 0.0f)
            {
                return gamepadInput;
            }
            else
            {
                return keyboardInput;
            }
        }

        /// <summary>
        /// Calculates the middle point of a group of points.
        /// </summary>
        /// <param name="points">List of points.</param>
        /// <returns>The middle point of a group of points.</returns>
        public static Vector3 MiddlePoint(List<Vector3> points)
        {
            float Xsum = 0;
            float Ysum = 0;
            float Zsum = 0;

            foreach (Vector3 p in points)
            {
                Xsum += p.X;
                Ysum += p.Y;
                Zsum += p.Z;
            }

            int count = points.Count;
            return new Vector3(Xsum / count, Ysum / count, Zsum / count);
        }
    }
}
