using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XtremeEngineXNA.Physics;

namespace XtremeEngineXNA.EntityComponent.Components
{
    /// <summary>
    /// Handler for the CollisionDetected event.
    /// </summary>
    /// <param name="owner">Entity with which there was a collision.</param>
    /// <param name="collidee">Entity which collided with the object in the first argument.</param>
    public delegate void CollisionDetectedDelegate(Entity owner, Entity collidee);

    /// <summary>
    /// Interface of the Physics component. A physics component adds physical behavior to an 
    /// entity.
    /// </summary>
    public interface IPhysicsComponent : IEntityComponent
    {
        #region Methods

        /// <summary>
        /// Builds a collision skin for the entity using the vertices of a model.
        /// </summary>
        /// <param name="model">The model used to generate the collision skin.</param>
        /// <param name="skinType">Type collision skin which is to be generated.</param>
        /// <param name="scale">Scale factors which are to be applied to each vertex.</param>
        void BuildCollisionSkin(Model model, CollisionSkinType skinType, Vector3 scale = new Vector3());

        /// <summary>
        /// Builds a planar collision skin for the entity.
        /// </summary>
        /// <param name="normal">Normal of the plane.</param>
        /// <param name="distance">
        /// Distance along the normal to the plane. For instance, if the normal is (0,1,0), that 
        /// is, the Y axis, the created plane will be y = distance.
        /// </param>
        void BuildCollisionPlane(Vector3 normal, float distance);

        /// <summary>
        /// Applies a local force to the entity.
        /// </summary>
        /// <param name="force">The force to be applied to the entity.</param>
        void ApplyForce(Vector3 force);

        /// <summary>
        /// Applies a local force to the entity.
        /// </summary>
        /// <param name="x">The x component of the force.</param>
        /// <param name="y">The y component of the force.</param>
        /// <param name="z">The z component of the force.</param>
        void ApplyForce(float x, float y, float z);

        /// <summary>
        /// Applies a local torque to the physics object.
        /// </summary>
        /// <param name="torque">The torque to be applied to the entity.</param>
        void ApplyTorque(Vector3 torque);

        /// <summary>
        /// Applies a local torque to the entity.
        /// </summary>
        /// <param name="x">The x component of the torque.</param>
        /// <param name="y">The y component of the torque.</param>
        /// <param name="z">The z component of the torque.</param>
        void ApplyTorque(float x, float y, float z);

        /// <summary>
        /// Applies a world force to the entity.
        /// </summary>
        /// <param name="force">The force to be applied to the entity.</param>
        void ApplyWorldForce(Vector3 force);

        /// <summary>
        /// Applies a world force to the entity.
        /// </summary>
        /// <param name="x">The x component of the force.</param>
        /// <param name="y">The y component of the force.</param>
        /// <param name="z">The z component of the force.</param>
        void ApplyWorldForce(float x, float y, float z);

        /// <summary>
        /// Applies a world torque to the entity.
        /// </summary>
        /// <param name="torque">The torque to be applied to the entity.</param>
        void ApplyWorldTorque(Vector3 torque);

        /// <summary>
        /// Applies a world torque to the entity.
        /// </summary>
        /// <param name="x">The x component of the torque.</param>
        /// <param name="y">The y component of the torque.</param>
        /// <param name="z">The z component of the torque.</param>
        void ApplyWorldTorque(float x, float y, float z);

        /// <summary>
        /// Moves the entity to the specified position.
        /// </summary>
        /// <param name="position">Position to which the entity is to be moved.</param>
        void MoveTo(Vector3 position);

        /// <summary>
        /// Sets the rotates of the entity according to the parameter.
        /// </summary>
        /// <param name="rotation">New rotation of the entity.</param>
        void SetRotation(Quaternion rotation);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the velocity of the entity.
        /// </summary>
        /// <value>The velocity of the entity.</value>
        Vector3 Velocity { get; set; }

        /// <summary>
        /// Gets the center of mass of the entity.
        /// </summary>
        /// <value>The center of mass of the entity.</value>
        Vector3 CenterOfMass { get; }

        /// <summary>
        /// Gets or sets the mass of the entity
        /// </summary>
        /// <value>The mass of the entity.</value>
        float Mass { get; set; }

        /// <summary>
        /// Gets or sets the density of the entity.
        /// </summary>
        /// <value>The density of the entity.</value>
        float Density { get; set; }

        /// <summary>
        /// Gets or sets the static roughness of the entity.
        /// </summary>
        /// <value>The dynamic roughness of the entity.</value>
        float StaticRoughness { get; set; }

        /// <summary>
        /// Gets or sets the dynamic roughness of the entity.
        /// </summary>
        /// <value>The dynamic roughness of the entity.</value>
        float DynamicRoughness { get; set; }

        /// <summary>
        /// Gets or sets the elasticity of the entity.
        /// </summary>
        /// <value>The elasticity of the entity.</value>
        float Elasticity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is immovable.
        /// </summary>
        /// <value><c>true</c> if immovable; otherwise, <c>false</c>.</value>
        bool Immovable { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when a collision with this entity is detected.
        /// </summary>
        event CollisionDetectedDelegate CollisionDetected;

        /// <summary>
        /// Occurs when the value of the CenterOfMass property changes.
        /// </summary>
        event EventHandler<EventArgs> CenterOfMassChanged;

        /// <summary>
        /// Occurs when the value of the Density property changes.
        /// </summary>
        event EventHandler<EventArgs> DensityChanged;

        /// <summary>
        /// Occurs when the value of the DynamicRoughness property changes.
        /// </summary>
        event EventHandler<EventArgs> DynamicRoughnessChanged;

        /// <summary>
        /// Occurs when the value of the Elasticity property changes.
        /// </summary>
        event EventHandler<EventArgs> ElasticityChanged;

        /// <summary>
        /// Occurs when the value of either the DynamicRoughness, Elasticity or StaticRoughness
        /// properties changes.
        /// </summary>
        event EventHandler<EventArgs> MaterialPropertiesChanged;

        /// <summary>
        /// Occurs when the value of the Immovable property changes.
        /// </summary>
        event EventHandler<EventArgs> ImmovableChanged;

        /// <summary>
        /// Occurs when the value of the Mass property changes.
        /// </summary>
        event EventHandler<EventArgs> MassChanged;

        /// <summary>
        /// Occurs when the value of the StaticRoughness property changes.
        /// </summary>
        event EventHandler<EventArgs> StaticRoughnessChanged;

        #endregion
    }
}
