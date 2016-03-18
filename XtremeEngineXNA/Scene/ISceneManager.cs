using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XtremeEngineXNA.Scene
{
    /// <summary>
    /// Interface which defines the methods that a scene manager must provide. A scene manager 
    /// contains a scene graph and is responsible for updating the nodes in it, loading/unloading
    /// them and returning the nodes and lights which are to be rendered each frame.
    /// </summary>
    public interface ISceneManager : IPlugin, IUpdateable
    {
        /// <summary>
        /// Sets which camera is active.
        /// </summary>
        /// <param name="camera">Camera which is to be the active camera.</param>
        void SetActiveCamera(CameraNode camera);

        /// <summary>
        /// Gets the root of the scene manager's scene graph.
        /// </summary>
        SceneNode RootSceneNode { get; }

        /// <summary>
        /// Gets the list of nodes which are to be drawn.
        /// </summary>
        List<IDrawableObject> VisibleDrawableNodes { get; }

        /// <summary>
        /// Gets the list of nodes which cast shadows.
        /// </summary>
        List<IDrawableObject> ShadowCastersList { get; }

        /// <summary>
        /// Gets the list of lights which are to be used to draw the scene.
        /// </summary>
        List<LightNode> LightsList { get; }

        /// <summary>
        /// Gets/Sets the camera which is currently active.
        /// </summary>
        CameraNode ActiveCamera { get; set; }

        /// <summary>
        /// Returns a list with all the cameras in the scene.
        /// </summary>
        List<CameraNode> CamerasList { get; }
    }
}
