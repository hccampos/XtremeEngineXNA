using Microsoft.Xna.Framework;
using XtremeEngineXNA.EntityComponent.Components;

namespace XtremeEngineXNA.EntityComponent.Entities
{
    /// <summary>
    /// Class which represents a physical plane.
    /// </summary>
    public class PhysicsPlane : Entity
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="root">Root object to which the entity belongs.</param>
        /// <param name="name">Name of the entity.</param>
        public PhysicsPlane(Root root, string name) : base(root, name)
        {
            ISpatialComponent spatial = ComponentFactory.createSpatialComponent(this.Root, "spatial");
            IPhysicsComponent physics = ComponentFactory.createPhysicsComponent(this.Root, "physicsComponent");
            physics.BuildCollisionPlane(new Vector3(0.0f, 1.0f, 0.0f), 0.0f);
            physics.Immovable = true;

            AddComponent(spatial);
            AddComponent(physics);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="root">Root object to which the entity belongs.</param>
        /// <param name="name">Name of the entity.</param>
        /// <param name="normal">Normal of the plane.</param>
        /// <param name="distance">
        /// Distance along the normal to the plane. For instance, if the normal is (0,1,0), that 
        /// is, the Y axis, the created plane will be y = distance.
        /// </param>
        public PhysicsPlane(Root root, string name, Vector3 normal, float distance) : base(root, name)
        {
            ISpatialComponent spatial = ComponentFactory.createSpatialComponent(this.Root, "spatial");
            IPhysicsComponent physics = ComponentFactory.createPhysicsComponent(this.Root, "physicsComponent");
            physics.BuildCollisionPlane(normal, distance);
            physics.Immovable = true;

            AddComponent(spatial);
            AddComponent(physics);
        }
    }
}
