using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XtremeEngineXNA.Scene.Animators
{
    /// <summary>
    /// Animator which makes a node move towards a certain position.
    /// </summary>
    public class MoveToPositionAnimator : Base, IAnimator
    {
        /// <summary>
        /// Position to which the animator should move the node.
        /// </summary>
        private Vector3 mTargetPosition;

        /// <summary>
        /// Factor by which the movement is multiplied. The larger this value the faster the node
        /// holding this animator will move towards the target position.
        /// </summary>
        private float mSpeed;

        /// <summary>
        /// Initializes a new instance of the <see cref="MoveToPositionAnimator"/> class.
        /// </summary>
        /// <param name="root">Root object to which the animator belongs.</param>
        /// <param name="targetPosition">Position towards which the node should move.</param>
        /// <param name="speed">
        /// How fast the animator moves a node towards the target position.
        /// </param>
        public MoveToPositionAnimator(Root root, Vector3 targetPosition, float speed = 1.0f)
            : base(root)
        {
            mTargetPosition = targetPosition;
            mSpeed = speed;
        }

        /// <summary>
        /// Tells the animator to update a certain node.
        /// </summary>
        /// <param name="elapsedTime">Time that has passed since the last update.</param>
        /// <param name="node">Node which is to be animated.</param>
        public virtual void Update(TimeSpan elapsedTime, SceneNode node)
        {
            // Only update the position if the target node is not null.
            if (node != null)
            {
                Vector3 currentPos = node.AbsolutePosition;
                Vector3 difference = mTargetPosition - currentPos;
                Vector3 movement = difference * (float)elapsedTime.TotalSeconds * mSpeed;
 
                // If the movement would make the node go past the target node's position we only
                // move the node to the target node's position.
                float dist = Vector3.Distance(currentPos, mTargetPosition);
                if (movement.Length() > dist)
                {
                    node.Translate(difference);
                }
                else
                {
                    node.Translate(movement);
                }
            }
        }

        /// <summary>
        /// Gets or sets the position towards which the animator should move nodes.
        /// </summary>
        /// <value>
        /// The position towards which the animator should move nodes.
        /// </value>
        public Vector3 TargetPosition
        {
            get { return mTargetPosition; }
            set { mTargetPosition = value; }
        }

        /// <summary>
        /// Gets or sets how fast the animator moves a node towards the target node's position.
        /// </summary>
        /// <value>
        /// How fast the animator moves a node towards the target node's position.
        /// </value>
        public float Speed
        {
            get { return mSpeed; }
            set { mSpeed = value; }
        }
    }
}
