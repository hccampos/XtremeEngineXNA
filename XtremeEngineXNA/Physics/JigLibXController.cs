using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JigLibX.Physics;
using Microsoft.Xna.Framework;

namespace XtremeEngineXNA.Physics
{
    /// <summary>
    /// JigLibX which wraps an IPhysicsController so that it can be added to
    /// JigLibX.
    /// </summary>
    internal class JigLibXController : Controller
    {
        /// <summary>
        /// IPhysicsController which is wrapped.
        /// </summary>
        private IPhysicsController mController;

        /// <summary>
        /// Constructor.
        /// </summary>
        public JigLibXController(IPhysicsController controller) : base()
        {
            mController = controller;
        }

        /// <summary>
        /// Updates the PhysicsController object owned by the JigLibXController.
        /// </summary>
        /// <param name="elapsedMillis">
        /// The time that has passed since the last update (milliseconds).
        /// </param>
        public override void UpdateController(float elapsedMillis)
        {
            if (mController != null)
            {
                TimeSpan elapsedTime = new TimeSpan((long)((double)elapsedMillis * 10000.0));
                mController.UpdatePhysics(elapsedTime);
            }
        }

        /// <summary>
        /// IPhysicsController which is wrapped by this.
        /// </summary>
        public IPhysicsController Controller
        {
            get { return mController; }
            set { mController = value; }
        }
    }
}
