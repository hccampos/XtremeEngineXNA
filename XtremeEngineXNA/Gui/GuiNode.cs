using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XtremeEngineXNA.Scene;

namespace XtremeEngineXNA.Gui
{
    /// <summary>
    /// Class which represents a node used to control the position, rotation and scale of GUI 
    /// elements.
    /// </summary>
    public class GuiNode : Node
    {
        #region Attributes

        /// <summary>
        /// Position of the node.
        /// </summary>
        private Vector2 mPosition;

        /// <summary>
        /// Rotation of the node.
        /// </summary>
        private float mRotation;

        /// <summary>
        /// Scale of the node.
        /// </summary>
        private Vector2 mScale;

        /// <summary>
        /// Global transformation of the node.
        /// </summary>
        private Matrix mWorldTransformMatrix;

        /// <summary>
        /// Local transformation of the node.
        /// </summary>
        private Matrix mLocalTransformMatrix;

        /// <summary>
        /// Whether the node's matrices have to be updated or not.
        /// </summary>
        private bool mNeedsUpdate;

        #endregion

        #region Public methods

        /// <summary>
        /// Initializes a new instance of the <see cref="GuiNode"/> class.
        /// </summary>
        /// <param name="root">Root object to which the object being created belongs.</param>
        public GuiNode(Root root)
            : base(root)
        {
            mPosition = Vector2.Zero;
            mRotation = 0.0f;
            mScale = Vector2.One;
            mWorldTransformMatrix = Matrix.Identity;
            mLocalTransformMatrix = Matrix.Identity;
            mNeedsUpdate = false;
        }

        /// <summary>
        /// Creates a new GUI node and attaches it to this node.
        /// </summary>
        /// <returns>The GUI node which was created.</returns>
        public GuiNode CreateChild()
        {
            try
            {
                //Create the new node.
                GuiNode node = new GuiNode(Root);
                //Attach the new node to this node.
                AttachChild(node);
                //return the newly created node.
                return node;
            }
            catch (Exception e)
            {
                throw new Exception("GuiNode.createChild (): " + e.Message);
            }
        }

        /// <summary>
        /// Updates the matrices of this node and all of its children.
        /// </summary>
        /// <param name="matrix">Transformation matrix inherited from the node's parent.</param>
        public virtual void UpdateMatrices(Matrix matrix)
        {
            if (this.NeedsUpdate)
            {
                Matrix T, sMatrix, rMatrix, tMatrix;
                T = Matrix.Identity;

                sMatrix = Matrix.CreateScale(mScale.X, mScale.Y, 1.0f);
                rMatrix = Matrix.CreateRotationZ(mRotation);
                tMatrix = Matrix.CreateTranslation(mPosition.X, mPosition.Y, 1.0f);

                // Update the node's local transformation matrix.
                mLocalTransformMatrix = sMatrix * rMatrix * tMatrix;

                // We've just updated the node's transform so we don't need to update it again
                // until the node has been moved, rotated or scaled.
                this.NeedsUpdate = false;
            }

            // Update the node's global transformation matrix.
            mWorldTransformMatrix = mLocalTransformMatrix * matrix;

            // Update all the children.
            foreach (GuiNode node in mNodes)
            {
                node.UpdateMatrices(mWorldTransformMatrix);
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
        /// Translation vector which describes the distance by which the node is to be translated 
        /// along each axis.
        /// </param>
        public void Translate(Vector2 dist)
        {
            this.Position = this.Position + dist;
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
        public void Translate(float x, float y)
        {
            Translate(new Vector2(x, y));
        }

        /// <summary>
        /// Rotates the node by the specified number of radians.
        /// </summary>
        /// <param name="radians">By how many radians the node is to be rotated.</param>
        public void Rotate(float radians)
        {
            this.Rotation = this.Rotation + radians;
        }

        /// <summary>
        /// Scales the node.
        /// </summary>
        /// <param name="factor">The factor by which the node is to be scaled.</param>
        public void Scale(float factor)
        {
            Scale(factor, factor);
        }

        /// <summary>
        /// Scales the node.
        /// </summary>
        /// <param name="s">Scaling factors by which the node is to be scaled.</param>
        public void Scale(Vector2 s)
        {
            this.ScalingFactors = this.ScalingFactors * s;
        }

        /// <summary>
        /// Scales the node.
        /// </summary>
        /// <param name="sx">Scaling along the X axis.</param>
        /// <param name="sy">Scaling along the Y axis.</param>
        public void Scale(float sx, float sy)
        {
            Scale(new Vector2(sx, sy));
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
        public void SetPosition(float x, float y)
        {
            this.Position = new Vector2(x, y);
        }

        /// <summary>
        /// Sets the rotation of the node.
        /// </summary>
        /// <param name="radians">New rotation of the node (in radians).</param>
        public void SetRotation(float radians)
        {
            this.Rotation = radians;
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
        public void SetDirection(float x, float y)
        {
            this.Direction = new Vector2(x, y);
        }

        /// <summary>
        /// Sets the scale of the node.
        /// </summary>
        /// <param name="sx">Scaling along the X axis.</param>
        /// <param name="sy">Scaling along the Y axis.</param>
        public void SetScale(float sx, float sy)
        {
            this.ScalingFactors = new Vector2(sx, sy);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the position of the node.
        /// </summary>
        public virtual Vector2 Position
        {
            get { return mPosition; }
            set
            {
                mPosition = value;
                this.NeedsUpdate = true;
                OnPositionChanged();
            }
        }

        /// <summary>
        /// Gets or sets the rotation of the node.
        /// </summary>
        public virtual float Rotation
        {
            get { return mRotation; }
            set 
            {
                mRotation = value;
                this.NeedsUpdate = true;
                OnRotationChanged();
            }
        }

        /// <summary>
        /// Gets or sets the scaling factors of the node.
        /// </summary>
        public virtual Vector2 ScalingFactors
        {
            get { return mScale; }
            set
            {
                mScale = value;
                this.NeedsUpdate = true;
                OnScaleChanged();
            }
        }

        /// <summary>
        /// Gets the direction of the node or rotates it so that it faces a certain direction.
        /// </summary>
        public Vector2 Direction
        {
            get { return new Vector2(1, (float)Math.Tan(mRotation)); }
            set
            {
                this.Rotation = (float)Math.Atan2(value.Y, value.X);
            }
        }

        /// <summary>
        /// Gets the absolute position of the node in relation to the screen.
        /// </summary>
        public Vector2 AbsolutePosition
        {
            get
            {
                return Vector2.Transform(Vector2.Zero, WorldTransformMatrix);
            }
        }

        /// <summary>
        /// Gets the absolute rotation of the node in relation to the screen.
        /// </summary>
        public float AbsoluteRotation
        {
            get
            {
                GuiNode parent = this.Parent as GuiNode;
                if (parent != null)
                {
                    return parent.AbsoluteRotation + this.Rotation;
                }
                else
                {
                    return this.Rotation;
                }
            }
        }

        /// <summary>
        /// Gets the direction the node is facing in world coordinates.
        /// </summary>
        public Vector2 AbsoluteDirection
        {
            get
            {
                return new Vector2(1, (float)Math.Tan(this.AbsoluteRotation));
            }
        }

        /// <summary>
        /// Gets the absolute scaling factors of the node in relation to the screen.
        /// </summary>
        public Vector2 AbsoluteScale
        {
            get
            {
                GuiNode parent = this.Parent as GuiNode;
                if (parent != null)
                {
                    return parent.AbsoluteScale * this.ScalingFactors;
                }
                else
                {
                    return this.ScalingFactors;
                }
            }
        }

        /// <summary>
        /// Gets or sets the local transform matrix.
        /// </summary>
        public Matrix LocalTransformMatrix
        {
            get { return mLocalTransformMatrix; }
            set 
            {
                mLocalTransformMatrix = value;
                this.NeedsUpdate = true;
            }
        }

        /// <summary>
        /// Gets or sets the world transform matrix.
        /// </summary>
        public Matrix WorldTransformMatrix
        {
            get { return mWorldTransformMatrix; }
            set 
            {
                mWorldTransformMatrix = value;
                this.NeedsUpdate = true;
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

        #region Events

        /// <summary>
        /// Event which is triggered when the position of the node is changed.
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
        /// Event which is triggered when the rotation of the node is changed.
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
        /// Event which is triggered when the scale of the node is changed.
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
    }
}
