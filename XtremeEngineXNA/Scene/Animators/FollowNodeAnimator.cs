using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XtremeEngineXNA.Scene.Animators
{
    /// <summary>
    /// Animator which makes a node follow another node.
    /// </summary>
    public class FollowNodeAnimator : MoveToPositionAnimator
    {
        /// <summary>
        /// Node which is to be followed.
        /// </summary>
        private SceneNode mNodeToFollow;

        /// <summary>
        /// Initializes a new instance of the <see cref="FollowNodeAnimator"/> class.
        /// </summary>
        /// <param name="root">Root object to which the animator belongs.</param>
        /// <param name="nodeToFollow">Node which is to be followed.</param>
        /// <param name="speed">
        /// How fast the animator moves a node towards the target node's position.
        /// </param>
        public FollowNodeAnimator(Root root, SceneNode nodeToFollow, 
            float speed = 1.0f) : base(root, Vector3.Zero, speed)
        {
            mNodeToFollow = nodeToFollow;
        }

        /// <summary>
        /// Tells the animator to update a certain node.
        /// </summary>
        /// <param name="elapsedTime">Time that has passed since the last update.</param>
        /// <param name="node">Node which is to be animated.</param>
        public override void Update(TimeSpan elapsedTime, SceneNode node)
        {
            // Only update the position if the target node is not null.
            if (node != null)
            {
                this.TargetPosition = mNodeToFollow.AbsolutePosition;
                base.Update(elapsedTime, node);
            }
        }

        /// <summary>
        /// Gets or sets the node to which is to be followed.
        /// </summary>
        /// <value>
        /// The node to which is to be followed.
        /// </value>
        public SceneNode NodeToFollow
        {
            get { return mNodeToFollow; }
            set { mNodeToFollow = value; }
        }
    }
}
