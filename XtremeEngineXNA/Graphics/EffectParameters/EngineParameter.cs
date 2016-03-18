using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XtremeEngineXNA.Scene;

namespace XtremeEngineXNA.Graphics.EffectParameters
{
    /// <summary>
    /// Class which represents an effect parameter which can set engine-specific values on an
    /// effect.
    /// </summary>
    public class EngineParameter : XEffectParameter<EngineParameterType>
    {
        #region EngineParameter Members

        /// <summary>
        /// Creates a new engine effect parameter.
        /// </summary>
        /// <param name="root">Root object to which the parameter belongs.</param>
        /// <param name="name">Name of the effect parameter.</param>
        /// <param name="type">Type of the effect parameter.</param>
        public EngineParameter(Root root, string name, EngineParameterType type) :
            base(root, name, type) { }
        
        /// <summary>
        /// Creates a new engine effect parameter.
        /// </summary>
        /// <param name="root">Root object to which the parameter belongs.</param>
        /// <param name="name">Name of the effect parameter.</param>
        /// <param name="type">Type of the effect parameter.</param>
        /// <param name="effect">Effect to which the parameter belongs.</param>
        public EngineParameter(Root root, string name, EngineParameterType type, Effect effect)
            : base(root, name, type, effect) { }

        /// <summary>
        /// Sets the value of the parameter on its effect.
        /// </summary>
        /// <param name="node">Node from which information can be retrieved.</param>
        public override void SetOnEffect(SceneNode node)
        {
#if DEBUG
            try
            {
#endif
                //Set the engine parameter depending on the type of the parameter.
                switch (this.Value)
                {
                    case EngineParameterType.PARAM_WORLD_MATRIX:
                        Parameter.SetValue(node.WorldTransformMatrix);
                        break;
                    case EngineParameterType.PARAM_VIEW_MATRIX:
                        Parameter.SetValue(Root.SceneManager.ActiveCamera.ViewMatrix);
                        break;
                    case EngineParameterType.PARAM_PROJECTION_MATRIX:
                        Parameter.SetValue(Root.SceneManager.ActiveCamera.ProjectionMatrix);
                        break;
                    case EngineParameterType.PARAM_WORLD_VIEW_MATRIX:
                        {
                            Matrix worldMatrix = node.WorldTransformMatrix;
                            Matrix viewMatrix = Root.SceneManager.ActiveCamera.ViewMatrix;
                            Parameter.SetValue(worldMatrix * viewMatrix);
                            break;
                        }
                    case EngineParameterType.PARAM_WORLD_VIEW_PROJECTION_MATRIX:
                        {
                            Matrix worldMatrix = node.WorldTransformMatrix;
                            Matrix viewMatrix = Root.SceneManager.ActiveCamera.ViewMatrix;
                            Matrix projMatrix = Root.SceneManager.ActiveCamera.ProjectionMatrix;
                            Parameter.SetValue(worldMatrix * viewMatrix * projMatrix);
                            break;
                        }
                    case EngineParameterType.PARAM_CAMERA_POSITION:
                        Parameter.SetValue(Root.SceneManager.ActiveCamera.AbsolutePosition);
                        break;
                    case EngineParameterType.PARAM_CAMERA_ROTATION_QUATERNION:
                        Parameter.SetValue(Root.SceneManager.ActiveCamera.AbsoluteRotation);
                        break;
                    case EngineParameterType.PARAM_CAMERA_ROTATION_EULER:
                        Parameter.SetValue(Root.SceneManager.ActiveCamera.AbsoluteRotationEuler);
                        break;
                    case EngineParameterType.PARAM_CAMERA_DIRECTION:
                        Parameter.SetValue(Root.SceneManager.ActiveCamera.Direction);
                        break;
                    case EngineParameterType.PARAM_CAMERA_ASPECT_RATIO:
                        Parameter.SetValue(Root.SceneManager.ActiveCamera.AspectRatio);
                        break;
                    case EngineParameterType.PARAM_CAMERA_NEAR_PLANE:
                        Parameter.SetValue(Root.SceneManager.ActiveCamera.NearPlaneDist);
                        break;
                    case EngineParameterType.PARAM_CAMERA_FAR_PLANE:
                        Parameter.SetValue(Root.SceneManager.ActiveCamera.FarPlaneDist);
                        break;
                    case EngineParameterType.PARAM_CAMERA_FOV:
                        Parameter.SetValue(Root.SceneManager.ActiveCamera.FOV);
                        break;
                    case EngineParameterType.PARAM_NODE_POSITION:
                        Parameter.SetValue(node.AbsolutePosition);
                        break;
                    case EngineParameterType.PARAM_NODE_ROTATION_QUATERNION:
                        Parameter.SetValue(node.AbsoluteRotation);
                        break;
                    case EngineParameterType.PARAM_NODE_ROTATION_EULER:
                        Parameter.SetValue(node.AbsoluteRotationEuler);
                        break;
                    case EngineParameterType.PARAM_NODE_DIRECTION:
                        Parameter.SetValue(node.AbsoluteDirection);
                        break;
                    case EngineParameterType.PARAM_NODE_SCALE:
                        Parameter.SetValue(node.ScalingFactors);
                        break;
                    case EngineParameterType.PARAM_BACKGROUND_COLOR:
                        Parameter.SetValue(Root.Renderer.BackgroundColor.ToVector4());
                        break;
                    case EngineParameterType.PARAM_DEPTH_TEXTURE:
                        Parameter.SetValue(Root.Renderer.DepthTexture);
                        break;
                    case EngineParameterType.PARAM_SCENE_TEXTURE:
                        Parameter.SetValue(Root.Renderer.SceneTexture);
                        break;
                    case EngineParameterType.PARAM_PREVIOUS_DEPTH_TEXTURE:
                        Parameter.SetValue(Root.Renderer.PreviousDepthTexture);
                        break;
                    case EngineParameterType.PARAM_PREVIOUS_FRAME_TEXTURE:
                        Parameter.SetValue(Root.Renderer.PreviousFrameTexture);
                        break;
                    case EngineParameterType.PARAM_PREVIOUS_RT_TEXTURE:
                        Parameter.SetValue(Root.Renderer.PreviousRenderTarget);
                        break;
                    case EngineParameterType.PARAM_PREVIOUS_SCENE_TEXTURE:
                        Parameter.SetValue(Root.Renderer.PreviousSceneTexture);
                        break;
                    case EngineParameterType.PARAM_UNDEFINED:
                        break;
                }
#if DEBUG
            }
            catch (Exception e)
            {
                throw new Exception("EngineParameter.SetOnEffect(): " + e.Message);
            }

#endif
        }

        #endregion
    }
}
