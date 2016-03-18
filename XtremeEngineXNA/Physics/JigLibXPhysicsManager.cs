using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using JigLibX.Physics;
using JigLibX.Collision;

namespace XtremeEngineXNA.Physics
{
    /// <summary>
    /// Physics manager implementation based on the JigLibX physics engine.
    /// </summary>
    internal class JigLibXPhysicsManager: PhysicsManagerBase
    {
        #region Attributes

        /// <summary>
        /// JigLibX physics system.
        /// </summary>
        private PhysicsSystem mPhysicsSystem;

        /// <summary>
        /// Physics bodies which are in the physics manager.
        /// </summary>
        private List<PhysicsBody> mBodies;

        /// <summary>
        /// Controllers which are in the physics manager.
        /// </summary>
        private List<JigLibXController> mControllers;

        #endregion

        #region Public methods

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="root">Root object to which the manager belongs.</param>
        /// <param name="name">Name of the new physics manager.</param>
        public JigLibXPhysicsManager(Root root, string name)
            : base(root, name)
        {
            mPhysicsSystem = new PhysicsSystem();
            mBodies = new List<PhysicsBody>();
            mControllers = new List<JigLibXController>();
        }

        /// <summary>
        /// Initializes the physics manager.
        /// </summary>
        public override void Initialize()
        {
            mPhysicsSystem.CollisionSystem = new CollisionSystemSAP();
            mPhysicsSystem.EnableFreezing = false;
            mPhysicsSystem.SolverType = PhysicsSystem.Solver.Normal;
            mPhysicsSystem.CollisionSystem.UseSweepTests = false;
            mPhysicsSystem.NumCollisionIterations = 16;
            mPhysicsSystem.NumContactIterations = 16;
            mPhysicsSystem.NumPenetrationRelaxtionTimesteps = 8;
            JigLibX.Physics.PhysicsSystem.CurrentPhysicsSystem = mPhysicsSystem;
        }

        /// <summary>
        /// Destroys the plug-in.
        /// </summary>
        public override void Destroy()
        {
            foreach (Body body in mBodies)
            {
                mPhysicsSystem.RemoveBody(body);
            }
            mBodies.Clear();

            foreach (Controller c in mControllers)
            {
                mPhysicsSystem.RemoveController(c);
            }
            mControllers.Clear();

            mPhysicsSystem = null;
            JigLibX.Physics.PhysicsSystem.CurrentPhysicsSystem = null;
        }

        /// <summary>
        /// Adds a physics body to the physics manager.
        /// </summary>
        /// <param name="body">The body which is to be added to the physics manager.</param>
        internal override void AddBody(PhysicsBody body)
        {
            if (body == null)
            {
                throw new Exception("PhysicsManager.AddPhysicsObject: null object.");
            }

            mBodies.Add(body);
            body.EnableBody();
        }

        /// <summary>
        /// Removes a physics object from the physics manager.
        /// </summary>
        /// <param name="body">The body which is to be removed from the physics manager.</param>
        internal override void RemoveBody(PhysicsBody body)
        {
            if (body == null)
            {
                throw new Exception("PhysicsManager.RemovePhysicsObject: null object.");
            }

            if (mBodies.Remove(body))
            {
                body.DisableBody();
            }
            else
            {
                throw new Exception("PhysicsManager.RemovePhysicsObject: object not found.");
            }
        }

        /// <summary>
        /// Adds a controller to the physics manager.
        /// </summary>
        /// <param name="controller">The controller which is to be added.</param>
        public override void AddController(IPhysicsController controller)
        {
            if (controller == null)
            {
                throw new Exception("PhysicsManager.AddController: null controller.");
            }

            JigLibXController wrapper = new JigLibXController(controller);
            mControllers.Add(wrapper);
            mPhysicsSystem.AddController(wrapper);
        }

        /// <summary>
        /// Removes a controller from the physics manager.
        /// </summary>
        /// <param name="controller">The controller which is to be removed.</param>
        public override void RemoveController(IPhysicsController controller)
        {
            if (controller == null)
            {
                throw new Exception("PhysicsManager.RemoveController: null controller.");
            }

            JigLibXController wrapper = mControllers.Find(c => c.Controller == controller);
            if (wrapper == null)
            {
                throw new Exception("PhysicsManager.RemoveController: controller not found.");
            }

            mControllers.Remove(wrapper);
            mPhysicsSystem.RemoveController(wrapper);
        }

        /// <summary>
        /// Updates the state of the updateable.
        /// </summary>
        /// <param name="elapsedTime">Time elapsed since the last update.</param>
        public override void Update(TimeSpan elapsedTime)
        {
            float timeStep = (float)elapsedTime.Ticks / TimeSpan.TicksPerSecond;
            //if (timeStep < 1.0f / 60.0f)
            {
                mPhysicsSystem.Integrate(timeStep);
            }
            //else
            //{
            //    mPhysicsSystem.Integrate(1.0f / 60.0f);
            //}
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the gravity force which is applied to all the physics objects.
        /// </summary>
        public override Vector3 Gravity
        {
            get { return mPhysicsSystem.Gravity; }
            set { mPhysicsSystem.Gravity = value; }
        }

        #endregion
    }
}
