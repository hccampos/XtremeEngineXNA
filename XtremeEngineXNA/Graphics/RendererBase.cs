using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XtremeEngineXNA.Graphics
{
    /// <summary>
    /// Abstract class which implements the common methods required by the IRenderer interface.
    /// </summary>
    public abstract class RendererBase : DrawablePluginBase, IRenderer
    {
        #region Attributes

        /// <summary>
        /// Whether the renderer should draw a debug layer or not.
        /// </summary>
        private bool mDebugLayerEnabled;

        /// <summary>
        /// Whether the renderer should draw to a texture or not. If false, the renderer will
        /// draw everything on the back buffer.
        /// </summary>
        private bool mDrawToTexture;

        #endregion

        #region Renderer Members

        /// <summary>
        /// Initializes a Renderer object.
        /// </summary>
        /// <param name="root">Root object to which the new renderer belongs.</param>
        /// <param name="name">Name of the new renderer.</param>
        public RendererBase(Root root, string name)
            : base(root, name)
        {
            mDebugLayerEnabled = false;
            mDrawToTexture = false;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the background color.
        /// </summary>
        /// <value>Color used to draw the background of the scene.</value>
        public abstract Color BackgroundColor
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to draw a debug layer.
        /// </summary>
        /// <value>
        /// <c>true</c> if the renderer is to draw a debug layer; otherwise, <c>false</c>.
        /// </value>
        public bool DebugLayerEnabled
        {
            get { return mDebugLayerEnabled; }
            set { mDebugLayerEnabled = value; }
        }

        /// <summary>
        /// Returns a texture which is a copy of the current z-buffer.
        /// </summary>
        /// <value>The depth texture.</value>
        public abstract Texture2D DepthTexture
        {
            get;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the renderer should draw to a texture.
        /// </summary>
        /// <value>
        /// <c>true</c> if the renderer should draw to a texture]; otherwise, <c>false</c>.
        /// </value>
        public virtual bool DrawToTexture
        {
            get { return mDrawToTexture; }
            set { mDrawToTexture = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the renderer should draw to the back buffer.
        /// </summary>
        /// <value>
        /// <c>true</c> if the renderer should draw to the back buffer; otherwise, <c>false</c>.
        /// </value>
        public virtual bool DrawToBackBuffer
        {
            get { return !this.DrawToTexture; }
            set { this.DrawToTexture = !value; }
        }

        /// <summary>
        /// Gets or sets the GUI draw mode.
        /// </summary>
        public abstract GuiDrawMode GuiDrawMode { get; set; }

        /// <summary>
        /// Gets the final frame texture (if the renderer is configured to render onto a texture.).
        /// </summary>
        /// <value>
        /// The final frame texture; <c>null</c> if the renderer is not configured to render onto a
        /// texture.
        /// </value>
        public abstract Texture2D FinalFrameTexture
        {
            get;
        }

        /// <summary>
        /// Gets the scene texture.
        /// </summary>
        public abstract Texture2D SceneTexture
        {
            get;
        }

        /// <summary>
        /// Texture onto which we the GUI was rendered if the GuiDrawMode property is set to 
        /// SEPARATE_TEXTURE.
        /// </summary>
        public abstract Texture2D GuiTexture
        {
            get;
        }

        /// <summary>
        /// Returns a texture which is a copy of the previous frame's z-buffer.
        /// </summary>
        /// <value>A texture which is a copy of the previous frame's z-buffer.</value>
        public abstract Texture2D PreviousDepthTexture
        {
            get;
        }

        /// <summary>
        /// Returns a texture which is a copy of the previous frame's back buffer.
        /// </summary>
        /// <value>Texture which is a copy of the previous frame's back buffer.</value>
        public abstract Texture2D PreviousFrameTexture
        {
            get;
        }

        /// <summary>
        /// Returns the render target texture used to apply the previous effect in the 
        /// post-processing effect chain being applied.
        /// </summary>
        /// <value>The previous render target.</value>
        public abstract Texture2D PreviousRenderTarget
        {
            get;
        }

        /// <summary>
        /// Returns a texture with the previous frame's rendered scene.
        /// </summary>
        /// <value>The previous frame's rendered scene.</value>
        public abstract Texture2D PreviousSceneTexture
        {
            get;
        }

        /// <summary>
        /// Gets or sets the quality of the shadow maps. Default value is ShadowMapQuality.NORMAL.
        /// </summary>
        /// <value>
        /// The quality of the shadow maps.
        /// </value>
        /// <remarks>This property must be set before Initialize().</remarks>
        public abstract ShadowMapQuality ShadowMapQuality
        {
            get;
            set;
        }

        #endregion
    }
}
