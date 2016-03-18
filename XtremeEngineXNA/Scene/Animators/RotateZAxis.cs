using System;
using Microsoft.Xna.Framework;

namespace XtremeEngineXNA.Scene.Animators
{
    /// <summary>
    /// Animator which rotates a node around the Z axis at a certain speed.
    /// </summary>
    public class RotateZAxis : Base, IAnimator
    {
        #region Attributes

        /// <summary>
        /// Speed at which the node is to be rotated in radians per second.
        /// </summary>
        private float mRotationSpeed;

        #endregion

        #region RotateZAxis Members

        /// <summary>
        /// Creates a new animator which rotates a node around the Z axis.
        /// </summary>
        /// <param name="root">Root object to which the animator belongs.</param>
        /// <param name="rotationSpeed">
        /// Speed of the rotation in radians per second.
        /// </param>
        public RotateZAxis(Root root, float rotationSpeed): base(root)
        {
            mRotationSpeed = rotationSpeed;
        }

        #endregion

        #region RotateZAxis Properties

        /// <summary>
        /// Gets/Sets the speed at which the animator rotates a scene node.
        /// </summary>
        public float RotationSpeed
        {
            get { return mRotationSpeed; }
            set { mRotationSpeed = value; }
        }

        #endregion

        #region IAnimator Members

        /// <summary>
        /// Updates the scene node passed as an argument.
        /// </summary>
        /// <param name="elapsedTime">Time elapsed since the last update.</param>
        /// <param name="node">Node which is to be updated.</param>
        public void Update(TimeSpan elapsedTime, SceneNode node)
        {
            //Get the time elapsed since the last update.
            float elapsed = (float)elapsedTime.TotalSeconds;
            //Rotate the node by mRotSpeed * elapsed because rad/sec * sec = rad.
            node.Rotate(Vector3.UnitZ, mRotationSpeed * elapsed);
        }

        #endregion
    }
}
