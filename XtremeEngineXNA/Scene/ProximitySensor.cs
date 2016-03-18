using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XtremeEngineXNA.Scene
{
    /// <summary>
    /// Special type of scene node which can be placed anywhere and which will trigger events
    /// if any nodes added to it via the AddSensedNode() method get closer than a certain
    /// threshold value.
    /// </summary>
    public class ProximitySensor : SceneNode
    {
        #region Attributes

        /// <summary>
        /// Nodes which are to be tested for hits.
        /// </summary>
        private List<SceneNode> mSensedNodes;

        /// <summary>
        /// Square of the distance at which the sensor will trigger the HitDetected event.
        /// </summary>
        private float mThresholdSq;

        /// <summary>
        /// Distance at which the sensor will trigger the HitDetected event.
        /// </summary>
        private float mThreshold;

        #endregion

        #region ProximitySensor members

        /// <summary>
        /// Initializes a new instance of the <see cref="ProximitySensor"/> class.
        /// </summary>
        /// <param name="root">Root object to which the scene node belongs.</param>
        /// <param name="threshold">
        /// Distance at which the sensor will trigger the HitDetected event.
        /// </param>
        public ProximitySensor(Root root, float threshold = 1.0f) : base(root)
        {
            mSensedNodes = new List<SceneNode>();
            this.Threshold = threshold;
        }

        /// <summary>
		/// Adds a node to the proximity sensor so that it can trigger a hit.
		/// </summary>
		/// <param name="node">Node which is to be added.</param>
        public void AddSensedNode(SceneNode node)
        {
            //Throw an exception if no node was passed to the method.
            if (node == null)
            {
                throw new ArgumentException("ProximitySensor.AddSensedNode(): null node.");
            }

            mSensedNodes.Add(node);
        }

        /// <summary>
        /// Removes a node from the sensor.
        /// </summary>
        /// <param name="node">Node which is to be removed.</param>
        public void RemoveSensedNode(SceneNode node)
        {
            //Throw an exception if the node is null.
            if (node == null)
            {
                throw new ArgumentException("ProximitySensor.RemoveSensedNode(): null node.");
            }
            else
            {
                if (!mSensedNodes.Remove(node))
                {
                    throw new ArgumentException("ProximitySensor.RemoveSensedNode(): node not found.");
                }
            }
        }

		/// <summary>
		/// Removes all the nodes from the sensor.
		/// </summary>
        public void RemoveAllSensedNodes()
        {
            mSensedNodes.Clear();
        }

        /// <summary>
        /// Checks if any node is close enough to the proximity sensor and if it is triggers the
        /// HitDetected event.
        /// </summary>
        /// <param name="elapsedTime">Time elapsed since the last update.</param>
        public override void Update(TimeSpan elapsedTime)
        {
            foreach (SceneNode node in mSensedNodes)
            {
                float SqDist = Vector3.DistanceSquared(node.AbsolutePosition, this.AbsolutePosition);
                if (SqDist < this.ThresholdSquared)
                {
                    if (HitDetected != null)
                        HitDetected(node);
                }
            }

            base.Update(elapsedTime);
        }

        /// <summary>
        /// Delegate type for the HitDetected event.
        /// </summary>
        /// <param name="node">Scene node which triggered the HitDetected event.</param>
        public delegate void HitDetectedDelegate(SceneNode node);

        /// <summary>
        /// Occurs when one of the scene nodes added to the proximity sensor is at a distance
        /// from the sensor which is smaller than the specified threshold.
        /// </summary>
        public event HitDetectedDelegate HitDetected;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the distance at which the sensor will trigger the HitDetected event.
        /// </summary>
        /// <value>The distance at which the sensor will trigger the HitDetected event.</value>
        public float Threshold
        {
            get { return mThreshold; }
            set
            {
                mThreshold = value;
                mThresholdSq = value * value;
            }
        }

        /// <summary>
        /// Gets or sets the square of the distance at which the sensor will trigger the 
        /// HitDetected event.
        /// </summary>
        /// <value>
        /// The square of the distance at which the sensor will trigger the HitDetected event.
        /// </value>
        public float ThresholdSquared
        {
            get { return mThresholdSq; }
            set
            {
                mThreshold = (float)Math.Sqrt(value);
                mThresholdSq = value;
            }
        }

        /// <summary>
        /// Gets a list with all the nodes that can trigger a proximity event.
        /// </summary>
        public List<SceneNode> SensedNodes
        {
            get { return mSensedNodes; }
        }

        #endregion
    }
}
