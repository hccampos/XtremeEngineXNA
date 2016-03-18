using Microsoft.Xna.Framework;

namespace XtremeEngineXNA.Scene
{
    /// <summary>
    /// Class which represents a Camera. A CameraNode is a special type of node 
    /// which represents a camera. A camera node can be rotated, moved, etc just
    /// like any other node and also provides methods which let the user set the
    /// field-of-view, the aspect-ratio and others. The engine uses camera nodes
    /// to render scenes properly.
    /// </summary>
    public class CameraNode : SceneNode
    {
        #region Attributes

        /// <summary>
        /// Camera's field-of-view.
        /// </summary>
        private float mFOV;

        /// <summary>
        /// Camera's aspect ratio.
        /// </summary>
        private float mAspect;

        /// <summary>
        /// Distance to the near clipping plane.
        /// </summary>
        private float mNear;

        /// <summary>
        /// Distance to the far clipping plane.
        /// </summary>
        private float mFar;

        /// <summary>
        /// Camera's view matrix.
        /// </summary>
        private Matrix mView;

        /// <summary>
        /// Camera's projection matrix.
        /// </summary>
        private Matrix mProjection;

        /// <summary>
        /// Bounding frustum of the camera.
        /// </summary>
        private BoundingFrustum mFrustum;

        /// <summary>
        /// Scene node at which the camera looks.
        /// </summary>
        private SceneNode mTarget;

        #endregion

        #region CameraNode members

        /// <summary>
        /// Creates a new camera node.
        /// </summary>
        /// <param name="root">Root object to which the node belongs.</param>
        /// <param name="fov">Field-of-view of the camera.</param>
        /// <param name="aspect">Aspect ratio of the camera.</param>
        /// <param name="nearDist">Distance to the near clipping plane.</param>
        /// <param name="farDist">Distance to the far clipping plane.</param>
        public CameraNode(Root root, float fov = 1.57f, float aspect = 1.3333f, 
            float nearDist = 1.0f, float farDist = 1.0f) : base(root)
        {
            mFOV = fov;
            mAspect = aspect;
            mNear = nearDist;
            mFar = farDist;
            mView = Matrix.Identity;
            mTarget = null;
            UpdateProjection();
            mFrustum = new BoundingFrustum(mView * mProjection);
        }

        /// <summary>
        /// Updates the view matrix according to the node's position and rotation.
        /// </summary>
        /// <param name="matrix">Matrix of the camera node's parent.</param>
        public override void UpdateMatrices(Matrix matrix)
        {
            base.UpdateMatrices(matrix);

            Vector3 camPos = this.AbsolutePosition;
            if (mTarget != null)
            {
                Vector3 targetPos = mTarget.AbsolutePosition;
                mView = Matrix.CreateLookAt(camPos, targetPos, Vector3.UnitY);
            }
            else
            {
                Matrix translation, rotation;
                translation = Matrix.CreateTranslation(-camPos);
                rotation = Matrix.CreateFromQuaternion(Quaternion.Inverse(AbsoluteRotation));
                mView = translation* rotation;
            }

            mFrustum.Matrix = mView * mProjection;
        }

        /// <summary>
        /// Updates the projection matrix according to the camera's parameters.
        /// </summary>
        private void UpdateProjection()
        {
            mProjection = Matrix.CreatePerspectiveFieldOfView(mFOV, mAspect, mNear, mFar);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets/Sets the field-of-view of the camera.
        /// </summary>
        public float FOV
        {
            get { return mFOV; }
            set { mFOV = value; UpdateProjection(); }
        }

        /// <summary>
        /// Gets/Sets the aspect ratio of the camera.
        /// </summary>
        public float AspectRatio
        {
            get { return mAspect; }
            set { mAspect = value; UpdateProjection(); }
        }

        /// <summary>
        /// Gets/Sets the near viewing/clipping plane of the camera.
        /// </summary>
        public float NearPlaneDist
        {
            get { return mNear; }
            set { mNear = value; UpdateProjection(); }
        }

        /// <summary>
        /// Gets/Sets the far viewing/clipping plane of the camera.
        /// </summary>
        public float FarPlaneDist
        {
            get { return mFar; }
            set { mFar = value; UpdateProjection(); }
        }

        /// <summary>
        /// Gets the frustum of the camera.
        /// </summary>
        /// <value>The frustum of the camera.</value>
        public BoundingFrustum Frustum
        {
            get { return mFrustum; }
        }

        /// <summary>
        /// Gets the view matrix of the camera.
        /// </summary>
        public Matrix ViewMatrix
        {
            get { return mView; }
        }

        /// <summary>
        /// Gets the projection matrix of the camera.
        /// </summary>
        public Matrix ProjectionMatrix
        {
            get { return mProjection; }
        }

        /// <summary>
        /// Gets/Sets the target at which the camera always points.
        /// </summary>
        public SceneNode Target
        {
            get { return mTarget; }
            set { mTarget = value; }
        }

        #endregion
    }
}
