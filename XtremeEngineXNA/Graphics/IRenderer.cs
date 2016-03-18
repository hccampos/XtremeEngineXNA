using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XtremeEngineXNA.Graphics
{
    /// <summary>
    /// Interface which defines the methods that a renderer must provide. A renderer is an
    /// object which is responsible for using the graphics device to render the scene geometry. It
    /// can use any approach to do so. It can be a deferred renderer which creates geometry buffers
    /// and uses them to render the scene or it can be a forward renderer which simply draws
    /// primitives onto the back buffer. The renderer is also responsible for using the root's
    /// post-processing effects manager to render the post-process effects stored in it.
    /// </summary>
    public interface IRenderer : IDrawablePlugin
    {
        /// <summary>
        /// Gets or sets the background color.
        /// </summary>
        /// <value>Color used to draw the background of the scene.</value>
        Color BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to draw a debug layer.
        /// </summary>
        /// <value>
        /// <c>true</c> if the renderer is to draw a debug layer; otherwise, <c>false</c>.
        /// </value>
        bool DebugLayerEnabled { get; set; }

        /// <summary>
        /// Returns a texture which is a copy of the current z-buffer.
        /// </summary>
        /// <value>The depth texture.</value>
        Texture2D DepthTexture { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the renderer should draw to a texture.
        /// </summary>
        /// <value>
        /// <c>true</c> if the renderer should draw to a texture]; otherwise, <c>false</c>.
        /// </value>
        bool DrawToTexture { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the renderer should draw to the back buffer.
        /// </summary>
        /// <value>
        /// <c>true</c> if the renderer should draw to the back buffer; otherwise, <c>false</c>.
        /// </value>
        bool DrawToBackBuffer { get; set; }

        /// <summary>
        /// Gets or sets the GUI draw mode.
        /// </summary>
        GuiDrawMode GuiDrawMode { get; set; }

        /// <summary>
        /// Gets the final frame texture (if the renderer is configured to render onto a texture).
        /// </summary>
        /// <value>
        /// The final frame texture; <c>null</c> if the renderer is not configured to render onto a
        /// texture.
        /// </value>
        Texture2D FinalFrameTexture { get; }

        /// <summary>
        /// Gets the scene texture.
        /// </summary>
        Texture2D SceneTexture { get; }

        /// <summary>
        /// Texture onto which we the GUI was rendered if the GuiDrawMode property is set to 
        /// SEPARATE_TEXTURE.
        /// </summary>
        Texture2D GuiTexture { get; }

        /// <summary>
        /// Returns a texture which is a copy of the previous frame's z-buffer.
        /// </summary>
        Texture2D PreviousDepthTexture { get; }

        /// <summary>
        /// Returns a texture which is a copy of the previous frame's back buffer.
        /// </summary>
        Texture2D PreviousFrameTexture { get; }

        /// <summary>
        /// Returns the render target texture used to apply the previous effect in the 
        /// post-processing effect chain being applied.
        /// </summary>
        Texture2D PreviousRenderTarget { get; }

        /// <summary>
        /// Returns a texture with the previous frame's rendered scene.
        /// </summary>
        Texture2D PreviousSceneTexture { get; }

        /// <summary>
        /// Gets or sets the quality of the shadow maps. Default value is ShadowMapQuality.NORMAL.
        /// </summary>
        /// <remarks>This property must be set before Initialize().</remarks>
        ShadowMapQuality ShadowMapQuality { get; set; }
    }
}
