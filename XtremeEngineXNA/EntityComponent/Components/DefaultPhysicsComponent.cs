using System;
using System.Collections.Generic;
using JigLibX.Collision;
using JigLibX.Geometry;
using JigLibX.Math;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XtremeEngineXNA.Physics;
using XtremeEngineXNA.Scene;

namespace XtremeEngineXNA.EntityComponent.Components
{
    /// <summary>
    /// Default implementation of the <see cref="IPhysicsComponent"/> interface.
    /// </summary>
    internal class DefaultPhysicsComponent : EntityComponent, IPhysicsComponent, IPhysicsController
    {
        #region Attributes

        /// <summary>
        /// Object used by the physics engine to simulate the physics of the entity.
        /// </summary>
        private PhysicsBody mBody;

        /// <summary>
        /// Object used by the physics engine to detect collisions with other entities.
        /// </summary>
        private CollisionSkin mCollisionSkin;

        /// <summary>
        /// Density of the entity.
        /// </summary>
        private float mDensity;

        /// <summary>
        /// Center of mass of the entity.
        /// </summary>
        private Vector3 mCenterOfMass;

        /// <summary>
        /// Properties of the material that makes up the entity.
        /// </summary>
        private MaterialProperties mMaterialProperties;

        /// <summary>
        /// Vector which accumulates the forces applied to the entity locally.
        /// </summary>
        private Vector3 mLocalForceCache;

        /// <summary>
        /// Vector which accumulates the forces applied to the entity globally.
        /// </summary>
        private Vector3 mWorldForceCache;

        /// <summary>
        /// Vector which accumulates the torques applied to the entity locally.
        /// </summary>
        private Vector3 mLocalTorqueCache;

        /// <summary>
        /// Vector which accumulates the torques applied to the entity globally.
        /// </summary>
        private Vector3 mWorldTorqueCache;

        /// <summary>
        /// Scene node which is to be updated by the physics component.
        /// </summary>
        private SceneNode mSceneNode;

        #endregion

        #region Public methods

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="root">Root object to which the component belongs.</param>
        /// <param name="name">Name of the component.</param>
        public DefaultPhysicsComponent(Root root, string name) : base(root, name)
        {
            mBody = new PhysicsBody();
            mBody.Owner = this;
            mCollisionSkin = new CollisionSkin(mBody);
            mBody.CollisionSkin = mCollisionSkin;
            mCollisionSkin.callbackFn += new CollisionCallbackFn(HandleCollisionDetection);
            mMaterialProperties = new MaterialProperties(0.8f, 0.8f, 0.7f);
            mDensity = 1;
            mCenterOfMass = new Vector3();

            mLocalForceCache = new Vector3();
            mWorldForceCache = new Vector3();
            mLocalTorqueCache = new Vector3();
            mWorldTorqueCache = new Vector3();
        }

        /// <summary>
        /// Called when the component is added to an entity.
        /// </summary>
        public override void  OnAdd()
        {
            base.OnAdd();

            JigLibXPhysicsManager physicsMgr = Root.PhysicsManager as JigLibXPhysicsManager;
            physicsMgr.AddBody(mBody);
            physicsMgr.AddController(this);
        }

        /// <summary>
        /// Called when another component is added or removed from the entity. This method should
        /// be used by the component to aquire references to other components in the entity.
        /// </summary>
        public override void OnReset()
        {
            base.OnReset();

            // Get the scene node from the entity's spatial component.
            SceneNode oldNode = mSceneNode;
            mSceneNode = this.Owner.GetComponent<ISpatialComponent>().SceneNode;

            // Copy the existing rotation to the physics engine.
            if (mSceneNode != oldNode && mSceneNode != null)
            {
                MoveTo(mSceneNode.Position);
                SetRotation(mSceneNode.Rotation);
            }
        }

        /// <summary>
        /// Called when the component is removed from an entity.
        /// </summary>
        public override void OnRemove()
        {
            base.OnRemove();

            JigLibXPhysicsManager physicsMgr = Root.PhysicsManager as JigLibXPhysicsManager;
            physicsMgr.RemoveBody(mBody);
            physicsMgr.RemoveController(this);
            mSceneNode = null;
        }

        /// <summary>
        /// Updates the entity's spatial component with the position and rotation calculated by 
        /// the physics engine.
        /// </summary>
        /// <param name="elapsedTime">Time that has passed since the last update.</param>
        public override void Update(TimeSpan elapsedTime)
        {
            SceneNode node = GetSceneNode();

            // Update the position of the entity.
            node.Position = mBody.Position;

            Quaternion colSkinRotation = Quaternion.CreateFromRotationMatrix(
                mBody.CollisionSkin.GetPrimitiveLocal(0).Transform.Orientation);
            Quaternion bodyRotation = Quaternion.CreateFromRotationMatrix(mBody.Orientation);

            // Update the rotation of the entity.
            node.Rotation = colSkinRotation * bodyRotation;

            base.Update(elapsedTime);
        }

        /// <summary>
        /// Performs any operations necessary to update the state of the physics world.
        /// </summary>
        /// <param name="elapsedTime">Time elapsed since the last update.</param>
        public void UpdatePhysics(TimeSpan elapsedTime)
        {
            mBody.AddBodyForce(mLocalForceCache);
            mBody.AddBodyTorque(mLocalTorqueCache);
            mBody.AddWorldForce(mWorldForceCache);
            mBody.AddWorldTorque(mWorldTorqueCache);

            mLocalForceCache = new Vector3();
            mLocalTorqueCache = new Vector3();
            mWorldForceCache = new Vector3();
            mWorldTorqueCache = new Vector3();
        }

        /// <summary>
        /// Builds the collision skin for the entity.
        /// </summary>
        /// <param name="model">The model used to generate the collision skin.</param>
        /// <param name="skinType">Type collision skin which is to be generated.</param>
        /// <param name="scale">Scale factors which are to be applied to each vertex.</param>
        /// <remarks>
        /// If the scale of the model used to generate the collision skin is changed (e.g. by 
        /// changing the ScalingFactors property of the node which contains the model) the
        /// entire collision skin has to be created again.
        /// </remarks>
        public void BuildCollisionSkin(Model model, CollisionSkinType skinType, Vector3 scale = new Vector3())
        {
            switch (skinType)
            {
                case CollisionSkinType.BOUNDING_BOX:
                    GenerateBoundingBoxSkin(model, scale);
                    break;
                case CollisionSkinType.BOUNDING_SPHERE:
                    GenerateBoundingSphere(model, scale);
                    break;
                case CollisionSkinType.TRIANGLE_MESH:
                    GenerateTriangleMeshSkin(model, scale);
                    break;
            }

            UpdateMaterialPropsOnCollisionSkin();
            UpdateMass();
        }

        /// <summary>
        /// Builds a planar collision skin for the entity.
        /// </summary>
        /// <param name="normal">Normal of the plane.</param>
        /// <param name="distance">
        /// Distance along the normal to the plane. For instance, if the normal is (0,1,0), that 
        /// is, the Y axis, the created plane will be y = distance.
        /// </param>
        public void BuildCollisionPlane(Vector3 normal, float distance)
        {
            JigLibX.Geometry.Plane plane = new JigLibX.Geometry.Plane(normal, distance);
            mCollisionSkin.RemoveAllPrimitives();
            mCollisionSkin.AddPrimitive(plane, mMaterialProperties);

            UpdateMaterialPropsOnCollisionSkin();
            UpdateMass();
        }

        /// <summary>
        /// Applies a local force to the entity.
        /// </summary>
        /// <param name="force">The force to be applied to the entity.</param>
        public void ApplyForce(Vector3 force)
        {
            mLocalForceCache = Vector3.Add(mLocalForceCache, force);
        }

        /// <summary>
        /// Applies a local force to the entity.
        /// </summary>
        /// <param name="x">The x component of the force.</param>
        /// <param name="y">The y component of the force.</param>
        /// <param name="z">The z component of the force.</param>
        public void ApplyForce(float x, float y, float z)
        {
            Vector3 force = new Vector3(x, y, z);
            mLocalForceCache = Vector3.Add(mLocalForceCache, force);
        }

        /// <summary>
        /// Applies a local torque to the physics object.
        /// </summary>
        /// <param name="torque">The torque to be applied to the entity.</param>
        public void ApplyTorque(Vector3 torque)
        {
            mLocalTorqueCache = Vector3.Add(mLocalTorqueCache, torque);
        }

        /// <summary>
        /// Applies a local torque to the entity.
        /// </summary>
        /// <param name="x">The x component of the torque.</param>
        /// <param name="y">The y component of the torque.</param>
        /// <param name="z">The z component of the torque.</param>
        public void ApplyTorque(float x, float y, float z)
        {
            Vector3 torque = new Vector3(x, y, z);
            mLocalTorqueCache = Vector3.Add(mLocalTorqueCache, torque);
        }

        /// <summary>
        /// Applies a world force to the entity.
        /// </summary>
        /// <param name="force">The force to be applied to the entity.</param>
        public void ApplyWorldForce(Vector3 force)
        {
            mWorldForceCache = Vector3.Add(mWorldForceCache, force);
        }

        /// <summary>
        /// Applies a world force to the entity.
        /// </summary>
        /// <param name="x">The x component of the force.</param>
        /// <param name="y">The y component of the force.</param>
        /// <param name="z">The z component of the force.</param>
        public void ApplyWorldForce(float x, float y, float z)
        {
            Vector3 force = new Vector3(x, y, z);
            mWorldForceCache = Vector3.Add(mWorldForceCache, force);
        }

        /// <summary>
        /// Applies a world torque to the entity.
        /// </summary>
        /// <param name="torque">The torque to be applied to the entity.</param>
        public void ApplyWorldTorque(Vector3 torque)
        {
            mWorldTorqueCache = Vector3.Add(mWorldTorqueCache, torque);
        }

        /// <summary>
        /// Applies a world torque to the entity.
        /// </summary>
        /// <param name="x">The x component of the torque.</param>
        /// <param name="y">The y component of the torque.</param>
        /// <param name="z">The z component of the torque.</param>
        public void ApplyWorldTorque(float x, float y, float z)
        {
            Vector3 torque = new Vector3(x, y, z);
            mWorldTorqueCache = Vector3.Add(mWorldTorqueCache, torque);
        }

        /// <summary>
        /// Moves the entity to the specified position.
        /// </summary>
        /// <param name="position">Position to which the entity is to be moved.</param>
        public void MoveTo(Vector3 position)
        {
            // Get the scene node from the entity's spatial component.
            SceneNode node = GetSceneNode();

            if (node == null)
                return;

            // Move the physics body.
            mBody.MoveTo(position, Matrix.CreateFromQuaternion(node.Rotation));

            // Move the scene node of the spatial component.
            node.Position = position;
        }

        /// <summary>
        /// Sets the rotates of the entity according to the parameter.
        /// </summary>
        /// <param name="rotation">New rotation of the entity.</param>
        public void SetRotation(Quaternion rotation)
        {
            // Get the scene node from the entity's spatial component.
            SceneNode node = GetSceneNode();

            if (node == null)
                return;

            node.Rotation = rotation;
            mBody.MoveTo(mBody.Position, Matrix.CreateFromQuaternion(node.Rotation));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the velocity of the entity.
        /// </summary>
        /// <value>The velocity of the entity.</value>
        public Vector3 Velocity
        {
            get { return mBody.Velocity; }
            set { mBody.Velocity = value; }
        }

        /// <summary>
        /// Gets the center of mass of the entity.
        /// </summary>
        /// <value>The center of mass of the entity.</value>
        public Vector3 CenterOfMass
        {
            get { return mCenterOfMass; }
        }

        /// <summary>
        /// Gets or sets the mass of the entity
        /// </summary>
        /// <value>The mass of the entity.</value>
        public float Mass
        {
            get { return mBody.Mass; }
            set
            {
                if (value != mBody.Mass)
                {
                    float volume = mCollisionSkin.GetVolume();
                    this.Density = value / volume;

                    if (MassChanged != null)
                        MassChanged(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Gets or sets the density of the entity.
        /// </summary>
        /// <value>The density of the entity.</value>
        public float Density
        {
            get { return mDensity; }
            set
            {
                if (value != mDensity)
                {
                    mDensity = value;
                    UpdateMass();

                    if (DensityChanged != null)
                        DensityChanged(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is immovable.
        /// </summary>
        /// <value><c>true</c> if immovable; otherwise, <c>false</c>.</value>
        public bool Immovable
        {
            get { return mBody.Immovable; }
            set
            {
                if (value != mBody.Immovable)
                {
                    mBody.Immovable = value;
                    
                    if (ImmovableChanged != null)
                        ImmovableChanged(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Gets or sets the static roughness of the entity.
        /// </summary>
        /// <value>The dynamic roughness of the entity.</value>
        public float StaticRoughness
        {
            get { return mMaterialProperties.StaticRoughness; }
            set
            {
                if (value != mMaterialProperties.StaticRoughness)
                {
                    mMaterialProperties.StaticRoughness = value;
                    UpdateMaterialPropsOnCollisionSkin();

                    if (StaticRoughnessChanged != null)
                        StaticRoughnessChanged(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Gets or sets the dynamic roughness of the entity.
        /// </summary>
        /// <value>The dynamic roughness of the entity.</value>
        public float DynamicRoughness
        {
            get { return mMaterialProperties.DynamicRoughness; }
            set
            {
                if (value != mMaterialProperties.DynamicRoughness)
                {
                    mMaterialProperties.DynamicRoughness = value;
                    UpdateMaterialPropsOnCollisionSkin();

                    if (DynamicRoughnessChanged != null)
                        DynamicRoughnessChanged(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Gets or sets the elasticity of the entity.
        /// </summary>
        /// <value>The elasticity of the entity.</value>
        public float Elasticity
        {
            get { return mMaterialProperties.Elasticity; }
            set
            {
                if (value != mMaterialProperties.Elasticity)
                {
                    mMaterialProperties.Elasticity = value;
                    UpdateMaterialPropsOnCollisionSkin();

                    if (ElasticityChanged != null)
                        ElasticityChanged(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Gets or sets the material properties of the object.
        /// </summary>
        /// <value>The material properties of the object.</value>
        internal MaterialProperties MaterialProperties
        {
            get { return mMaterialProperties; }
            set
            {
                if (!value.Equals(mMaterialProperties))
                {
                    mMaterialProperties = value;
                    UpdateMaterialPropsOnCollisionSkin();

                    if (MaterialPropertiesChanged != null)
                        MaterialPropertiesChanged(this, new EventArgs());
                    if (StaticRoughnessChanged != null)
                        StaticRoughnessChanged(this, new EventArgs());
                    if (DynamicRoughnessChanged != null)
                        DynamicRoughnessChanged(this, new EventArgs());
                    if (ElasticityChanged != null)
                        ElasticityChanged(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Gets or sets the collision skin of the object.
        /// </summary>
        /// <value>The collision skin of the object.</value>
        internal CollisionSkin CollisionSkin
        {
            get { return mCollisionSkin; }
        }

        #endregion

        #region Private/Protected methods

        /// <summary>
        /// Gets the scene node which is to be updated by the physics.
        /// </summary>
        /// <returns></returns>
        protected SceneNode GetSceneNode()
        {
            return mSceneNode;
        }

        /// <summary>
        /// Handles the collision detection.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="collidee">The collidee.</param>
        /// <returns></returns>
        protected bool HandleCollisionDetection(CollisionSkin owner, CollisionSkin collidee)
        {
            // If there are handlers for the CollisionDetected event we trigger it. We use the
            // Owner property of the PhysicsObject to obtain the IPhysicsComponent to which the body
            // belongs. From that we can get the entities that are colliding.
            if (CollisionDetected != null)
            {
                PhysicsBody body1 = owner.Owner as PhysicsBody;
                PhysicsBody body2 = collidee.Owner as PhysicsBody;
                if (body1 != null && body2 != null)
                {
                    IPhysicsComponent component1 = body1.Owner;
                    IPhysicsComponent component2 = body2.Owner;
                    if (component1 != null && component2 != null)
                    {
                        CollisionDetected(component1.Owner, component2.Owner);
                    }
                }
            }

            //We return true because we want the physics engine to handle the collision.
            return true;
        }

        /// <summary>
        /// Set the material properties on the primitives of the collision skin.
        /// </summary>
        protected void UpdateMaterialPropsOnCollisionSkin()
        {
            int count = mCollisionSkin.NumPrimitives;
            for (int i = 0; i < count; ++i)
            {
                mCollisionSkin.SetMaterialProperties(i, mMaterialProperties);
            }
        }

        /// <summary>
        /// Updates the center of mass and the mass properties of the collision skin.
        /// </summary>
        protected void UpdateMass()
        {
            //Sets the mass properties of the object and transforms the collision skin according
            //to the returned center of mass.
            Vector3 newCOM = SetMassProperties(mDensity);

            //We only update the center of mass if it has changed.
            if (!newCOM.Equals(mCenterOfMass))
            {
                mCenterOfMass = newCOM;
                mCollisionSkin.ApplyLocalTransform(new Transform(-mCenterOfMass, Matrix.Identity));

                //The center of mass changed so we trigger the CenterOfMassChanged event.
                if (CenterOfMassChanged != null)
                {
                    CenterOfMassChanged(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Sets the mass properties of the physics object.
        /// </summary>
        /// <param name="mass">The mass of the object.</param>
        /// <returns>The center of mass of the object.</returns>
        protected Vector3 SetMassProperties(float mass)
        {
            PrimitiveProperties primitiveProperties = new PrimitiveProperties(
                PrimitiveProperties.MassDistributionEnum.Solid,
                PrimitiveProperties.MassTypeEnum.Density, mass);

            float junk; Vector3 com; Matrix it, itCoM;

            mCollisionSkin.GetMassProperties(primitiveProperties, out junk, out com, out it, out itCoM);

            mBody.BodyInertia = itCoM;
            mBody.Mass = junk;

            return com;
        }

        /// <summary>
        /// Generates a bounding box collision sking from the model.
        /// </summary>
        /// <param name="model">
        /// The model from which the collision skin should be generated.
        /// </param>
        /// <param name="scale">Scale factors which are to be applied to each vertex.</param>
        protected void GenerateBoundingBoxSkin(Model model, Vector3 scale)
        {
            //Get the vertex and index data from the model.
            List<Vector3> vertexList = new List<Vector3>();
            List<TriangleVertexIndices> indexList = new List<TriangleVertexIndices>();
            ExtractDataFromModel(vertexList, indexList, model, scale);

            float Xmax = vertexList[0].X;
            float Ymax = vertexList[0].Y;
            float Zmax = vertexList[0].Z;
            float Xmin = vertexList[0].X;
            float Ymin = vertexList[0].Y;
            float Zmin = vertexList[0].Z;

            //Determine the maximum coordinates.
            foreach (Vector3 vertexPosition in vertexList)
            {
                if (vertexPosition.X > Xmax) Xmax = vertexPosition.X;
                if (vertexPosition.Y > Ymax) Ymax = vertexPosition.Y;
                if (vertexPosition.Z > Zmax) Zmax = vertexPosition.Z;
            }

            //Determine the minimum coordinates.
            foreach (Vector3 vertexPosition in vertexList)
            {
                if (vertexPosition.X < Xmin) Xmin = vertexPosition.X;
                if (vertexPosition.Y < Ymin) Ymin = vertexPosition.Y;
                if (vertexPosition.Z < Zmin) Zmin = vertexPosition.Z;
            }

            //Create the collision skin.
            Vector3 sideLengths = new Vector3(Xmax - Xmin, Ymax - Ymin, Zmax - Zmin);
            Box collisionBox = new Box(-0.5f * sideLengths, Matrix.Identity, sideLengths);
            mCollisionSkin.RemoveAllPrimitives();
            mCollisionSkin.AddPrimitive(collisionBox, this.MaterialProperties);
        }

        /// <summary>
        /// Generates a bounding sphere collision sking from the model.
        /// </summary>
        /// <param name="model">
        /// The model from which the collision skin should be generated.
        /// </param>
        /// <param name="scale">Scale factors which are to be applied to each vertex.</param>
        protected void GenerateBoundingSphere(Model model, Vector3 scale)
        {
            //Get the vertex and index data from the model.
            List<Vector3> vertexList = new List<Vector3>();
            List<TriangleVertexIndices> indexList = new List<TriangleVertexIndices>();
            ExtractDataFromModel(vertexList, indexList, model, scale);

            //Determine the middle point of the model.
            Vector3 middle = Utils.MiddlePoint(vertexList);

            //Determine the maximum distance from the center point of the model.
            float maxDist = 0;
            foreach (Vector3 vertexPosition in vertexList)
            {
                float dist = Vector3.Distance(vertexPosition, middle);
                if (dist > maxDist) maxDist = dist;
            }

            //Create the collision skin.
            Sphere collisionSphere = new Sphere(middle, maxDist);
            mCollisionSkin.RemoveAllPrimitives();
            mCollisionSkin.AddPrimitive(collisionSphere, this.MaterialProperties);
        }

        /// <summary>
        /// Generates a triangle mesh collision skin from the model.
        /// </summary>
        /// <param name="model">
        /// The model from which the collision skin should be generated.
        /// </param>
        /// <param name="scale">Scale factors which are to be applied to each vertex.</param>
        protected void GenerateTriangleMeshSkin(Model model, Vector3 scale)
        {
            //Get the vertex and index data from the model.
            List<Vector3> vertexList = new List<Vector3>();
            List<TriangleVertexIndices> indexList = new List<TriangleVertexIndices>();
            ExtractDataFromModel(vertexList, indexList, model, scale);

            //Create the triangle mesh collision skin.
            TriangleMesh triangleMesh = new TriangleMesh();
            triangleMesh.CreateMesh(vertexList, indexList, 3, 1.0f);
            mCollisionSkin.RemoveAllPrimitives();
            mCollisionSkin.AddPrimitive(triangleMesh, this.MaterialProperties);
        }

        /// <summary>
        /// Helper method to get the vertex and index lists from the model.
        /// </summary>
        /// <param name="vertices">
        /// List which is to be filled with the vertices of the model.
        /// </param>
        /// <param name="indices">
        /// List which is to be filled with the indices of the model.
        /// </param>
        /// <param name="model">
        /// Model from which the vertices and indices are to be exctracted.
        /// </param>
        /// <param name="scale">Scale factors which are to be applied to each vertex.</param>
        protected void ExtractDataFromModel(List<Vector3> vertices,
            List<TriangleVertexIndices> indices, Model model, Vector3 scale)
        {
            Matrix[] bones = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(bones);
            foreach (ModelMesh mm in model.Meshes)
            {
                Matrix xform = bones[mm.ParentBone.Index];
                foreach (ModelMeshPart mmp in mm.MeshParts)
                {
                    //Get the vertices from the vertex buffer.
                    int offset = vertices.Count;
                    Vector3[] a = new Vector3[mmp.NumVertices];
                    int stride = mmp.VertexBuffer.VertexDeclaration.VertexStride;
                    mmp.VertexBuffer.GetData<Vector3>(mmp.VertexOffset * stride, a, 0,
                        mmp.NumVertices, stride);

                    for (int i = 0; i != a.Length; ++i)
                    {
                        Vector3.Transform(ref a[i], ref xform, out a[i]);
                    }
                    vertices.AddRange(a);

                    if (mmp.IndexBuffer.IndexElementSize == IndexElementSize.SixteenBits)
                    {
                        //Get the indices from the index buffer.
                        short[] rawIndices = new short[mmp.PrimitiveCount * 3];
                        mmp.IndexBuffer.GetData<short>(mmp.StartIndex * 2, rawIndices, 0,
                            mmp.PrimitiveCount * 3);

                        JigLibX.Geometry.TriangleVertexIndices[] tvi =
                            new JigLibX.Geometry.TriangleVertexIndices[mmp.PrimitiveCount];

                        for (int i = 0; i < tvi.Length; ++i)
                        {
                            tvi[i].I0 = rawIndices[i * 3 + 0] + offset;
                            tvi[i].I1 = rawIndices[i * 3 + 1] + offset;
                            tvi[i].I2 = rawIndices[i * 3 + 2] + offset;
                        }
                        indices.AddRange(tvi);
                    }
                    else
                    {
                        //Get the indices from the index buffer.
                        int[] rawIndices = new int[mmp.PrimitiveCount * 3];
                        mmp.IndexBuffer.GetData<int>(mmp.StartIndex * 4, rawIndices, 0,
                            mmp.PrimitiveCount * 3);

                        JigLibX.Geometry.TriangleVertexIndices[] tvi =
                            new JigLibX.Geometry.TriangleVertexIndices[mmp.PrimitiveCount];

                        for (int i = 0; i != tvi.Length; ++i)
                        {
                            tvi[i].I0 = rawIndices[i * 3 + 2] + offset;
                            tvi[i].I1 = rawIndices[i * 3 + 1] + offset;
                            tvi[i].I2 = rawIndices[i * 3 + 0] + offset;
                        }
                        indices.AddRange(tvi);
                    }
                }
            }

            //Scale all the vertices.
            if (!scale.Equals(Vector3.One))
            {
                int count = vertices.Count;
                for (int i = 0; i < count; ++i)
                {
                    vertices[i] = vertices[i] * scale;
                }
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when a collision with this entity is detected.
        /// </summary>
        public event CollisionDetectedDelegate CollisionDetected;

        /// <summary>
        /// Occurs when the value of the CenterOfMass property changes.
        /// </summary>
        public event EventHandler<EventArgs> CenterOfMassChanged;

        /// <summary>
        /// Occurs when the value of the Density property changes.
        /// </summary>
        public event EventHandler<EventArgs> DensityChanged;

        /// <summary>
        /// Occurs when the value of the DynamicRoughness property changes.
        /// </summary>
        public event EventHandler<EventArgs> DynamicRoughnessChanged;

        /// <summary>
        /// Occurs when the value of the Elasticity property changes.
        /// </summary>
        public event EventHandler<EventArgs> ElasticityChanged;

        /// <summary>
        /// Occurs when the value of either the DynamicRoughness, Elasticity or StaticRoughness
        /// properties changes.
        /// </summary>
        public event EventHandler<EventArgs> MaterialPropertiesChanged;

        /// <summary>
        /// Occurs when the value of the Immovable property changes.
        /// </summary>
        public event EventHandler<EventArgs> ImmovableChanged;

        /// <summary>
        /// Occurs when the value of the Mass property changes.
        /// </summary>
        public event EventHandler<EventArgs> MassChanged;

        /// <summary>
        /// Occurs when the value of the StaticRoughness property changes.
        /// </summary>
        public event EventHandler<EventArgs> StaticRoughnessChanged;

        #endregion
    }
}
