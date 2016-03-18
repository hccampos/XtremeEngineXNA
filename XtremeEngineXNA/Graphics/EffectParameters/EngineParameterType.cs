
namespace XtremeEngineXNA.Graphics.EffectParameters
{
    /// <summary>
    /// Types of engine-specific effect parameters.
    /// </summary>
    public enum EngineParameterType
    {
        /// <summary>Auto-updates the parameter with the node's world matrix.</summary>
        PARAM_WORLD_MATRIX,
        /// <summary>Auto-updates the parameter with the camera's view matrix.</summary>
        PARAM_VIEW_MATRIX,
        /// <summary>Auto-updates the parameter with the camera's projection matrix.</summary>
        PARAM_PROJECTION_MATRIX,
        /// <summary>Auto-updates the parameter with the world-view matrix.</summary>
        PARAM_WORLD_VIEW_MATRIX,
        /// <summary>Auto-updates the parameter with the world-view-projection matrix.</summary>
        PARAM_WORLD_VIEW_PROJECTION_MATRIX,
        /// <summary>Auto-updates the parameter with the camera's position.</summary>
        PARAM_CAMERA_POSITION,
        /// <summary>Auto-updates the parameter with the camera's rotation.</summary>
        PARAM_CAMERA_ROTATION_QUATERNION,
        /// <summary>Auto-updates the parameter with the camera's rotation.</summary>
        PARAM_CAMERA_ROTATION_EULER,
        /// <summary>Auto-updates the parameter with the camera's direction.</summary>
        PARAM_CAMERA_DIRECTION,
        /// <summary>Auto-updates the parameter with the camera's aspect ratio.</summary>
        PARAM_CAMERA_ASPECT_RATIO,
        /// <summary>Auto-updates the parameter with the camera's near plane.</summary>
        PARAM_CAMERA_NEAR_PLANE,
        /// <summary>Auto-updates the parameter with the camera's far plane.</summary>
        PARAM_CAMERA_FAR_PLANE,
        /// <summary>Auto-updates the parameter with the camera's field-of-view.</summary>
        PARAM_CAMERA_FOV,
        /// <summary>Auto-updates the parameter with the node's position.</summary>
        PARAM_NODE_POSITION,
        /// <summary>Auto-updates the parameter with the node's rotation.</summary>
        PARAM_NODE_ROTATION_QUATERNION,
        /// <summary>Auto-updates the parameter with the node's rotation.</summary>
        PARAM_NODE_ROTATION_EULER,
        /// <summary>Auto-updates the parameter with the node's direction.</summary>
        PARAM_NODE_DIRECTION,
        /// <summary>Auto-updates the parameter with the node's scale.</summary>
        PARAM_NODE_SCALE,
        /// <summary>
        /// Auto-updates the parameter with the background color set in the renderer.
        /// </summary>
        PARAM_BACKGROUND_COLOR,
        /// <summary>
        /// Auto-updates the parameter with a texture with the contents of the z-buffer. Used by
        /// post-processing effects.
        /// </summary>
        PARAM_DEPTH_TEXTURE,
        /// <summary>
        /// Auto-updates the parameter with a texture with the rendered scene. Used by
        /// post-processing effects.
        /// </summary>
        PARAM_SCENE_TEXTURE,
        /// <summary>
        /// Auto-updates the parameter with a texture with the contents of the z-buffer from the
        /// previous frame. Used by post-processing effects.
        /// </summary>
        PARAM_PREVIOUS_DEPTH_TEXTURE,
        /// <summary>
        /// Auto-updates the parameter with a texture with the Returns a texture which is a copy of
        /// the previous frame's back buffer.
        /// </summary>
        PARAM_PREVIOUS_FRAME_TEXTURE,
        /// <summary>
        /// Auto-updates the parameter with the render target texture used to apply the previous 
        /// effect in the post-processing effect chain being applied.
        /// </summary>
        PARAM_PREVIOUS_RT_TEXTURE,
        /// <summary>
        /// Auto-updates the parameter with a texture with the rendered scene from the previous
        /// frame. Used by post-processing effects.
        /// </summary>
        PARAM_PREVIOUS_SCENE_TEXTURE,
        /// <summary>Undefined parameter.</summary>
        PARAM_UNDEFINED
    };
}
