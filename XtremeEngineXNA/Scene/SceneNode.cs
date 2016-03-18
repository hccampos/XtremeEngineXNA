using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace XtremeEngineXNA.Scene
{
    /// <summary>
    /// Class which represents a node of a scene graph. A scene node has 
    /// information about the node's position, rotation, scale, etc and can be 
    /// attached to the scene manager's root scene node.
    /// </summary>
    public class SceneNode : Node, IUpdateable
    {
        #region Attributes

        /// <summary>
        /// Map which stores all of the node's controllers.
        /// </summary>
        private List<IAnimator> mAnimators;

        /// <summary>
        /// Position of the node.
        /// </summary>
        private Vector3 mPosition;

        /// <summary>
        /// Rotation of the node.
        /// </summary>
        private Quaternion mRotation;

        /// <summary>
        /// Scale of the node.
        /// </summary>
        private Vector3 mScale;

        /// <summary>
        /// Global transformation of the node.
        /// </summary>
        private Matrix mWorldTransformMatrix;

        /// <summary>
        /// Local transformation of the node.
        /// </summary>
        private Matrix mLocalTransformMatrix;

        /// <summary>
        /// Whether the node is enabled or not.
        /// </summary>
        private bool mEnabled;

        /// <summary>
        /// Whether the node's matrices have to be updated or not.
        /// </summary>
        private bool mNeedsUpdate;

        /// <summary>
        /// Order of update of the node.
        /// </summary>
        private int mUpdateOrder;

        #endregion

        #region SceneNode public members

        /// <summary>
        /// Creates a new scene node.
        /// </summary>
        /// <param name="root">Root object to which the scene node belongs.</param>
        public SceneNode(Root root) : base(root)
        {
            mAnimators = new List<IAnimator>();
            mPosition = Vector3.Zero;
            mRotation = Quaternion.Identity;
            mScale = Vector3.One;
            mWorldTransformMatrix = Matrix.Identity;
            mLocalTransformMatrix = Matrix.Identity;
            mEnabled = true;
            mNeedsUpdate = false;
            mUpdateOrder = 1;
        }

        /// <summary>
        /// Creates a new node and attaches it to this node.
        /// </summary>
        /// <returns>The node which was created.</returns>
        public SceneNode CreateChild()
        {
            try
            {
                //Create the new node.
                SceneNode node = new SceneNode(Root);
                //Attach the new node to this node.
                AttachChild(node);
                //return the newly created node.
                return node;
            }
            catch (Exception e)
            {
                throw new Exception("SceneNode.createChild (): " + e.Message);
            }
        }

        /// <summary>
        /// Adds an animator to the scene node.
        /// </summary>
        /// <param name="animator">Animator which is to be added.</param>
        public void AddAnimator(IAnimator animator)
        {
            //Throw an exception if no animator was passed to the method.
            if (animator == null)
            {
                throw new Exception("SceneNode.AddAnimator(): null animator.");
            }

            mAnimators.Add(animator);
        }

        /// <summary>
        /// Removes an animator from the scene node.
        /// </summary>
        /// <param name="animator">Animator which is to be removed.</param>
        public void RemoveAnimator(IAnimator animator)
        {
            if (animator == null)
            {
                throw new Exception("Node.RemoveAnimator(): null animator.");
            }
            else
            {
                if (!mAnimators.Remove(animator))
                {
                    throw new Exception("Node.RemoveAnimator(): animator not found.");
                }
            }
        }

        /// <summary>
        /// Updates the matrices of this node and all of its children.
        /// </summary>
        /// <param name="matrix">Transformation matrix inherited from the node's parent.</param>
        public virtual void UpdateMatrices(Matrix matrix)
        {
            if (NeedsUpdate)
            {
                Matrix T, sMatrix, rMatrix, tMatrix;
                T = Matrix.Identity;

                sMatrix = Matrix.CreateScale(mScale);
                rMatrix = Matrix.CreateFromQuaternion(mRotation);
                tMatrix = Matrix.CreateTranslation(mPosition);

                // Update the node's local transformation matrix.
                mLocalTransformMatrix = sMatrix * rMatrix * tMatrix;

                // We've just updated the node's transform so we don't need to update it again
                // until the node has been moved, rotated or scaled.
                NeedsUpdate = false;
            }

            // Update the node's global transformation matrix.
            mWorldTransformMatrix = mLocalTransformMatrix * matrix;

            // Update all the children.
            foreach (SceneNode node in mNodes)
            {
                if (node.Enabled)
                {
                    node.UpdateMatrices(mWorldTransformMatrix);
                }
            }
        }

        /// <summary>
        /// Translates the node along the direction it is facing.
        /// </summary>
        /// <param name="dist">
        /// Distance by which the node is to be translated.
        /// </param>
        public void Translate(float dist)
        {
            Translate(Direction * dist);
        }

        /// <summary>
        /// Translates the node according to a translation vector.
        /// </summary>
        /// <param name="dist">
        /// Translation vector which describes the distance by which the node is
        /// to be translated along each axis.
        /// </param>
        public void Translate(Vector3 dist)
        {
            Position = Position + dist;
        }

        /// <summary>
        /// Translates the node according to a translation vector.
        /// </summary>
        /// <param name="x">
        /// Distance by which the node is to be translated along the X axis.
        /// </param>
        /// <param name="y">
        /// Distance by which the node is to be translated along the Y axis.
        /// </param>
        /// <param name="z">
        /// Distance by which the node is to be translated along the Z axis.
        /// </param>
        public void Translate(float x, float y, float z)
        {
            Translate(new Vector3(x, y, z));
        }

        /// <summary>
        /// Rotates the node according to a quaternion.
        /// </summary>
        /// <param name="rot">Quaternion which describes the rotation.</param>
        public void Rotate(Quaternion rot)
        {
            Rotation = rot * Rotation;
        }

        /// <summary>
        /// Rotates the node around a certain axis.
        /// </summary>
        /// <param name="axis">
        /// Axis around which the node is to be rotated.
        /// </param>
        /// <param name="angle">
        /// Angle by which the node is to be rotated around the specified axis.
        /// </param>
        public void Rotate(Vector3 axis, float angle)
        {
            Rotate(Quaternion.CreateFromAxisAngle(axis, angle));
        }

        /// <summary>
        /// Rotates the node around a certain axis.
        /// </summary>
        /// <param name="x">
        /// X component of the axis around which the node is to be rotated.
        /// </param>
        /// <param name="y">
        /// Y component of the axis around which the node is to be rotated.
        /// </param>
        /// <param name="z">
        /// Z component of the axis around which the node is to be rotated.
        /// </param>
        /// <param name="angle">
        /// Angle by which the node is to be rotated around the specified axis.
        /// </param>
        public void Rotate(float x, float y, float z, float angle)
        {
            Rotate(new Vector3(x, y, z), angle);
        }

        /// <summary>
        /// Rotates the node according to the Euler angles stored in a vector.
        /// </summary>
        /// <param name="eulerAngles">
        /// Vector with the Euler angles which describe the rotation.
        /// </param>
        public void Rotate(Vector3 eulerAngles)
        {
            Rotate(Quaternion.CreateFromYawPitchRoll(eulerAngles.Y, eulerAngles.X, eulerAngles.Z));
        }

        /// <summary>
        /// Rotates the node according to the Euler angles passed to the method.
        /// </summary>
        /// <param name="rx">Rotation around the X axis.</param>
        /// <param name="ry">Rotation around the Y axis.</param>
        /// <param name="rz">Rotation around the Z axis.</param>
        public void Rotate(float rx, float ry, float rz)
        {
            Rotate(Quaternion.CreateFromYawPitchRoll(ry, rx, rz));
        }

        /// <summary>
        /// Scales the node.
        /// </summary>
        /// <param name="factor">The factor by which the node is to be scaled.</param>
        public void Scale(float factor)
        {
            Scale(factor, factor, factor);
        }

        /// <summary>
        /// Scales the node.
        /// </summary>
        /// <param name="s">Scaling factors by which the node is to be scaled.</param>
        public void Scale(Vector3 s)
        {
            ScalingFactors = ScalingFactors * s;
        }

        /// <summary>
        /// Scales the node.
        /// </summary>
        /// <param name="sx">Scaling along the X axis.</param>
        /// <param name="sy">Scaling along the Y axis.</param>
        /// <param name="sz">Scaling along the Z axis.</param>
        public void Scale(float sx, float sy, float sz)
        {
            Scale(new Vector3(sx, sy, sz));
        }

        /// <summary>
        /// Changes the position of the node.
        /// </summary>
        /// <param name="x">
        /// X coordinate of the new position vector of the node.
        /// </param>
        /// <param name="y">
        /// Y coordinate of the new position vector of the node.
        /// </param>
        /// <param name="z">
        /// Z coordinate of the new position vector of the node.
        /// </param>
        public void SetPosition(float x, float y, float z)
        {
            Position = new Vector3(x, y, z);
        }

        /// <summary>
        /// Sets the rotation of the node.
        /// </summary>
        /// <param name="rotation">New rotation of the node.</param>
        public void SetRotation(Quaternion rotation)
        {
            Rotation = rotation;
        }

        /// <summary>
        /// Sets the rotation of the node according to the Euler angles passed to the method.
        /// </summary>
        /// <param name="eulerAngles">
        /// Vector with the Euler angles of the new rotation of the node.
        /// </param>
        public void SetRotation(Vector3 eulerAngles)
        {
            Rotation = Quaternion.CreateFromYawPitchRoll(eulerAngles.Y, eulerAngles.X, eulerAngles.Z);
        }

        /// <summary>
        /// Sets the rotation of the node according to the Euler angles passed 
        /// to the method.
        /// </summary>
        /// <param name="rx">Rotation around the X axis.</param>
        /// <param name="ry">Rotation around the Y axis.</param>
        /// <param name="rz">Rotation around the Z axis.</param>
        public void SetRotation(float rx, float ry, float rz)
        {
            SetRotation(new Vector3(rx, ry, rz));
        }

        /// <summary>
        /// Rotates the node so that it faces a certain direction.
        /// </summary>
        /// <param name="x">
        /// X coordinate of the new direction of the node.
        /// </param>
        /// <param name="y">
        /// Y coordinate of the new direction of the node.
        /// </param>
        /// <param name="z">
        /// Z coordinate of the new direction of the node.
        /// </param>
        public void SetDirection(float x, float y, float z)
        {
            Direction = new Vector3(x, y, z);
        }

        /// <summary>
        /// Sets the scale of the node.
        /// </summary>
        /// <param name="sx">Scaling along the X axis.</param>
        /// <param name="sy">Scaling along the Y axis.</param>
        /// <param name="sz">Scaling along the Z axis.</param>
        public void SetScale(float sx, float sy, float sz)
        {
            ScalingFactors = new Vector3(sx, sy, sz);
        }

        #endregion

        #region SceneNode events and delegates

        /// <summary>
        /// Event which is triggered when the position of the scene node is changed.
        /// </summary>
        public event EventHandler<EventArgs> PositionChanged;

        /// <summary>
        /// Invokes the PositionChanged event.
        /// </summary>
        public void OnPositionChanged()
        {
            if (PositionChanged != null)
                PositionChanged(this, new EventArgs());
        }

        /// <summary>
        /// Event which is triggered when the rotation of the scene node is changed.
        /// </summary>
        public event EventHandler<EventArgs> RotationChanged;

        /// <summary>
        /// Invokes the RotationChanged event.
        /// </summary>
        public void OnRotationChanged()
        {
            if (RotationChanged != null)
                RotationChanged(this, new EventArgs());
        }

        /// <summary>
        /// Event which is triggered when the scale of the scene node is changed.
        /// </summary>
        public event EventHandler<EventArgs> ScaleChanged;

        /// <summary>
        /// Invokes the ScaleChanged event.
        /// </summary>
        public void OnScaleChanged()
        {
            if (ScaleChanged != null)
                ScaleChanged(this, new EventArgs());
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets/Sets the position of the node.
        /// </summary>
        public virtual Vector3 Position
        {
            get { return mPosition; }
            set
            {
                mPosition = value;
                NeedsUpdate = true;
                OnPositionChanged();
            }
        }

        /// <summary>
        /// Gets/Sets the rotation of the node.
        /// </summary>
        public virtual Quaternion Rotation
        {
            get { return mRotation; }
            set 
            {
                mRotation = value;
                NeedsUpdate = true;
                OnRotationChanged();
            }
        }

        /// <summary>
        /// Gets/Sets the rotation of the node in the form of the three Euler angles.
        /// </summary>
        public Vector3 RotationEuler
        {
            get
            {
                float x = Rotation.X;
                float y = Rotation.Y;
                float z = Rotation.Z;
                float w = Rotation.W;
                float sqx = x * x;
                float sqy = y * y;
                float sqz = z * z;

                Vector3 euler;
                euler.X = (float)Math.Atan2(2.0f * (y * z + x * w), 1 - 2 * (sqx + sqy));
                euler.Y = (float)Math.Asin(-2.0f * (x * z - y * w));
                euler.Z = (float)Math.Atan2(2.0f * (x * y + z * w), 1 - 2 * (sqy + sqz));
                return euler;
            }
            set
            {
                Rotation = Quaternion.CreateFromYawPitchRoll(value.Y, value.X, value.Z);
            }
        }

        /// <summary>
        /// Gets/Sets the scaling factors of the node.
        /// </summary>
        public virtual Vector3 ScalingFactors
        {
            get { return mScale; }
            set
            {
                mScale = value;
                NeedsUpdate = true;
                OnScaleChanged();
            }
        }

        /// <summary>
        /// Gets the direction of the node or rotates it so that it faces a certain direction.
        /// </summary>
        public Vector3 Direction
        {
            get { return Vector3.Transform(-Vector3.UnitZ, Rotation); }
            set
            {
                Matrix lookAt = Matrix.CreateLookAt(Vector3.Zero, value, Vector3.UnitY);
                Quaternion rotation = Quaternion.CreateFromRotationMatrix(lookAt);
                Rotation = Quaternion.Inverse(rotation);
            }
        }

        /// <summary>
        /// Returns the absolute position of the node in relation to the root scene node.
        /// </summary>
        public Vector3 AbsolutePosition
        {
            get
            {
                return Vector3.Transform(Vector3.Zero, WorldTransformMatrix);
            }
        }

        /// <summary>
        /// Returns the absolute rotation of the node in relation to the root scene node.
        /// </summary>
        public Quaternion AbsoluteRotation
        {
            get
            {
                SceneNode parent = this.Parent as SceneNode;
                if (parent != null)
                {
                    return parent.AbsoluteRotation * Rotation;
                }
                else
                {
                    return Rotation;
                }
            }
        }

        /// <summary>
        /// Returns the absolute rotation of the node in relation the root scene node in the form
        /// of the three Euler angles.
        /// </summary>
        public Vector3 AbsoluteRotationEuler
        {
            get
            {
                Quaternion absoluteRotation = AbsoluteRotation;
                float x = absoluteRotation.X;
                float y = absoluteRotation.Y;
                float z = absoluteRotation.Z;
                float w = absoluteRotation.W;
                float sqx = x * x;
                float sqy = y * y;
                float sqz = z * z;

                Vector3 euler;
                euler.X = (float)Math.Atan2(2.0f * (y * z + x * w), 1 - 2 * (sqx + sqy));
                euler.Y = (float)Math.Asin(-2.0f * (x * z - y * w));
                euler.Z = (float)Math.Atan2(2.0f * (x * y + z * w), 1 - 2 * (sqy + sqz));
                return euler;
            }
        }

        /// <summary>
        /// Returns the direction the node is facing in world coordinates.
        /// </summary>
        public Vector3 AbsoluteDirection
        {
            get
            {
                return Vector3.Transform(-Vector3.UnitZ, AbsoluteRotation);
            }
        }

        /// <summary>
        /// Returns the absolute scaling factors of the node in relation to the
        /// root scene node.
        /// </summary>
        public Vector3 AbsoluteScale
        {
            get
            {
                SceneNode parent = this.Parent as SceneNode;
                if (parent != null)
                {
                    return parent.AbsoluteScale * ScalingFactors;
                }
                else
                {
                    return ScalingFactors;
                }
            }
        }

        /// <summary>
        /// Gets/Sets the local transform matrix.
        /// </summary>
        public Matrix LocalTransformMatrix
        {
            get { return mLocalTransformMatrix; }
            set 
            {
                mLocalTransformMatrix = value;
                NeedsUpdate = true;
            }
        }

        /// <summary>
        /// Gets/Sets the world transform matrix.
        /// </summary>
        public Matrix WorldTransformMatrix
        {
            get { return mWorldTransformMatrix; }
            set 
            {
                mWorldTransformMatrix = value;
                NeedsUpdate = true;
            }
        }

        /// <summary>
        /// Gets or sets whether the node's matrices need to be updated.
        /// </summary>
        public bool NeedsUpdate
        {
            get { return mNeedsUpdate; }
            set { mNeedsUpdate = value; }
        }

        #endregion

        #region IUpdateable Members

        /// <summary>
        /// Gets or sets whether the scene node is enabled or not.
        /// </summary>
        public bool Enabled
        {
            get { return mEnabled; }
            set
            {
                if(value != mEnabled)
                {
                    mEnabled = value;
                    if (EnabledChanged != null)
                    {
                        EnabledChanged(this, new EventArgs());
                    }
                }
            }
        }

        /// <summary>
        /// Calls the update method of each animator added to the node.
        /// </summary>
        /// <param name="elapsedTime">Time elapsed since the last update.</param>
        public virtual void Update(TimeSpan elapsedTime)
        {
            //Update each animator in the node.
            foreach (IAnimator animator in mAnimators)
            {
                animator.Update(elapsedTime, this);
            }
        }

        /// <summary>
        /// Returns the update order of the scene node.
        /// </summary>
        public int UpdateOrder
        {
            get { return mUpdateOrder; }
            set
            {
                if (value != mUpdateOrder)
                {
                    mUpdateOrder = value;
                    if (UpdateOrderChanged != null)
                    {
                        UpdateOrderChanged(this, new EventArgs());
                    }
                }
            }
        }

        /// <summary>
        /// No description.
        /// </summary>
        public event EventHandler<EventArgs> EnabledChanged;

        /// <summary>
        /// No description.
        /// </summary>
        public event EventHandler<EventArgs> UpdateOrderChanged;

        #endregion
    }
}
