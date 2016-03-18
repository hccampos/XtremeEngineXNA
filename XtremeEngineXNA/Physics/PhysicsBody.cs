using JigLibX.Physics;
using XtremeEngineXNA.EntityComponent.Components;

namespace XtremeEngineXNA.Physics
{
    /// <summary>
    /// Class which builds on the JigLibX Body class to provide an owner for a body. This can be
    /// used, for instance, to identify which entities are colliding with each other.
    /// </summary>
    internal class PhysicsBody : Body
    {
        /// <summary>
        /// The IPhysicsComponent to which this body belongs.
        /// </summary>
        private IPhysicsComponent mOwner;

        /// <summary>
        /// Gets or sets the owner of this body.
        /// </summary>
        /// <value>The owner of this body.</value>
        public IPhysicsComponent Owner
        {
            get { return mOwner; }
            set { mOwner = value; }
        }
    }
}
