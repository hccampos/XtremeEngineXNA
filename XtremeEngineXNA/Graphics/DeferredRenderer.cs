using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XtremeEngineXNA.Scene;
using XtremeEngineXNA.Content;
using XtremeEngineXNA.Gui;

namespace XtremeEngineXNA.Graphics
{
    /// <summary>
    /// Renderer which used deferred lighting to render the scene. It first renders all the
    /// geometry information onto some geometry buffers and then uses that geometry information
    /// to render all the lights.
    /// </summary>
    internal class DeferredRenderer : RendererBase
    {
        #region Attributes

        /// <summary>
        /// Reference to the graphics device used for drawing.
        /// </summary>
        private GraphicsDevice mGraphicsDevice;

        /// <summary>
        /// Reference to the scene manager from which the scene information is retrieved.
        /// </summary>
        private ISceneManager mSceneManager;

        /// <summary>
        /// Reference to the GUI manager from which the gui information is retrieved.
        /// </summary>
        private IGuiManager mGuiManager;

        /// <summary>
        /// Content manager used to load some resources required by the renderer.
        /// </summary>
        private ContentManager mContentManager;

        /// <summary>
        /// Effect used to combine the color map with the light map.
        /// </summary>
        private Effect mCombineFinalEffect;

        /// <summary>
        /// The color that should be used for the background.
        /// </summary>
        private Color mBackgroundColor = Color.Black;

        /// <summary>
        /// Whether the renderer has been loaded or not.
        /// </summary>
        private bool mLoaded = false;

        /// <summary>
        /// Viewport which contains the size of the client window.
        /// </summary>
        private Viewport mViewport;

        /// <summary>
        /// Quality of the shadow maps used to draw shadows.
        /// </summary>
        private ShadowMapQuality mShadowMapQuality = ShadowMapQuality.NORMAL;

        /// <summary>
        /// GUI draw mode.
        /// </summary>
        private GuiDrawMode mGuiDrawMode = GuiDrawMode.FINAL_TEXTURE;

        /// <summary>
        /// Whether the renderer is initialized.
        /// </summary>
        private bool mInitialized;

        #region Textures

        /// <summary>
        /// Render target to which we will render the color of the objects.
        /// </summary>
        private RenderTarget2D mColorGBuffer;

        /// <summary>
        /// Render target to which we will render the normals of the objects.
        /// </summary>
        private RenderTarget2D mNormalGBuffer;

        /// <summary>
        /// Auxiliary geometry buffer to which we will render specular color and shininess.
        /// </summary>
        private RenderTarget2D mAmbientGBuffer;

        /// <summary>
        /// Render target to which we will render the depth of the pixels.
        /// </summary>
        private RenderTarget2D mDepthGBuffer;

        /// <summary>
        /// Final texture which will be rendered to the back buffer or made available to the client
        /// of the renderer.
        /// </summary>
        private Texture2D mFinalTexture;

        /// <summary>
        /// Texture onto which we will render the GUI if the GuiDrawMode 
        /// property is set to SEPARATE_TEXTURE.
        /// </summary>
        private RenderTarget2D mGuiTexture;

        /// <summary>
        /// Render target to which we will render the lights.
        /// </summary>
        private RenderTarget2D mLightMap;

        /// <summary>
        /// Texture with the scene contents before applying any post-processing effects.
        /// </summary>
        private RenderTarget2D mSceneTexture;

        /// <summary>
        /// Texture with the final scene after applying all the post-processing effects.
        /// </summary>
        private RenderTarget2D mPostProcessedTexture;

        /// <summary>
        /// Depth buffer of the previous frame.
        /// </summary>
        private RenderTarget2D mPreviousDepthBuffer;

        /// <summary>
        /// Scene texture of the previous frame before applying any post-processing effects.
        /// </summary>
        private RenderTarget2D mPreviousSceneTexture;

        /// <summary>
        /// Texture which is a copy of the previous frame's back buffer.
        /// </summary>
        private RenderTarget2D mPreviousFrameTexture;

        /// <summary>
        /// Auxiliary render target used to render post-processing effects.
        /// </summary>
        private RenderTarget2D mAuxRT1;

        /// <summary>
        /// Auxiliary render target used to render post-processing effects.
        /// </summary>
        private RenderTarget2D mAuxRT2;

        /// <summary>
        /// Render target whose texture is to be used as input to the current post-proc. effect.
        /// </summary>
        private RenderTarget2D mCurrentInRT;

        /// <summary>
        /// Render target which is to be used as the output for the current post-processing effect.
        /// </summary>
        private RenderTarget2D mCurrentOutRT;

        #endregion

        #region Utilities

        /// <summary>
        /// Quad renderer used to render full screen quads.
        /// </summary>
        private QuadRenderer mQuadRenderer;

        /// <summary>
        /// Sprite batch used to draw debugging information and to draw the final texture to the
        /// back buffer.
        /// </summary>
        private SpriteBatch mSpriteBatch;

        /// <summary>
        /// Effect used to clear the g-buffers.
        /// </summary>
        private Effect mClearGBuffersEffect;

        /// <summary>
        /// Value used to align pixels to texels.
        /// </summary>
        private Vector2 mHalfPixel;

        #endregion

        #region Used for rendering lights

        /// <summary>
        /// Effect used to draw directional lights.
        /// </summary>
        private Effect mDirLightEffect;

        /// <summary>
        /// Effect used to draw shadow maps for directional lights.
        /// </summary>
        private Effect mDirShadowMapEffect;

        /// <summary>
        /// Render target onto which we draw the shadow map for directional lights.
        /// </summary>
        private RenderTarget2D mDirShadowMap;

        /// <summary>
        /// Effect used to draw point lights.
        /// </summary>
        private Effect mPointLightEffect;

        /// <summary>
        /// Effect used to draw spot lights.
        /// </summary>
        private Effect mSpotLightEffect;

        /// <summary>
        /// Sphere used to clip pixels not affected by point lights.
        /// </summary>
        private Model mSphereModel;

        /// <summary>
        /// Cone used to clip pixels not affected by spot lights.
        /// </summary>
        private Model mConeModel;

        #endregion

        #region Used for commonly accessed values

        CameraNode mCamera;
        Matrix mCameraViewMatrix;
        Matrix mProjectionMatrix;
        Matrix mViewProjectionMatrix;
        Matrix mInverseViewProjection;
        Vector3 mCameraPosition;

        #endregion

        #endregion

        #region Public methods

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="root">Root object to which the new renderer belongs.</param>
        /// <param name="name"></param>
        public DeferredRenderer(Root root, string name) : base(root, name)
        {
            mInitialized = false;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the background color.
        /// </summary>
        public override Color BackgroundColor
        {
            get { return mBackgroundColor; }
            set { mBackgroundColor = value; }
        }

        /// <summary>
        /// Returns a texture which is a copy of the current z-buffer.
        /// </summary>
        /// <value>The depth texture.</value>
        public override Texture2D DepthTexture
        {
            get { return mDepthGBuffer; }
        }

        /// <summary>
        /// Gets the final frame texture (if the renderer is configured to render onto a texture.).
        /// </summary>
        /// <value>
        /// The final frame texture; <c>null</c> if the renderer is not configured to render onto a
        /// texture.
        /// </value>
        public override Texture2D FinalFrameTexture
        {
            get
            {
                return this.DrawToBackBuffer ? null : mFinalTexture;
            }
        }

        /// <summary>
        /// Gets the scene texture.
        /// </summary>
        /// <value>The scene texture.</value>
        public override Texture2D SceneTexture
        {
            get { return mSceneTexture; }
        }

        /// <summary>
        /// Texture onto which we the GUI was rendered if the GuiDrawMode property is set to 
        /// SEPARATE_TEXTURE.
        /// </summary>
        public override Texture2D GuiTexture
        {
            get { return mGuiTexture; }
        }

        /// <summary>
        /// Returns a texture which is a copy of the previous frame's z-buffer.
        /// </summary>
        /// <value>A texture which is a copy of the previous frame's z-buffer.</value>
        public override Texture2D PreviousDepthTexture
        {
            get { return mPreviousDepthBuffer; }
        }

        /// <summary>
        /// Returns a texture which is a copy of the previous frame's back buffer.
        /// </summary>
        /// <value>Texture which is a copy of the previous frame's back buffer.</value>
        public override Texture2D PreviousFrameTexture
        {
            get { return mPreviousFrameTexture; }
        }

        /// <summary>
        /// Returns the render target texture used to apply the previous effect in the 
        /// post-processing effect chain being applied.
        /// </summary>
        /// <value>The previous render target.</value>
        public override Texture2D PreviousRenderTarget
        {
            get { return mCurrentInRT; }
        }

        /// <summary>
        /// Returns a texture with the previous frame's rendered scene.
        /// </summary>
        /// <value>The previous frame's rendered scene.</value>
        public override Texture2D PreviousSceneTexture
        {
            get { return mPreviousSceneTexture; }
        }

        /// <summary>
        /// Gets or sets the quality of the shadow maps. Default value is ShadowMapQuality.NORMAL.
        /// </summary>
        /// <value>
        /// The quality of the shadow maps.
        /// </value>
        /// <remarks>This property must be set before Initialize().</remarks>
        public override ShadowMapQuality ShadowMapQuality
        {
            get { return mShadowMapQuality; }
            set
            {
                if (value != mShadowMapQuality)
                {
                    mShadowMapQuality = value;
                    CreateShadowMapTexture();
                }
            }
        }

        /// <summary>
        /// Gets or sets the GUI draw mode.
        /// </summary>
        public override GuiDrawMode GuiDrawMode
        {
            get { return mGuiDrawMode; }
            set
            {
                if (value != mGuiDrawMode)
                {
                    mGuiDrawMode = value;
                    CreateGuiTexture();
                }
            }
        }

        #endregion

        #region IDrawable members

        /// <summary>
        /// Renders the scene onto the back buffer. In case of a deferred renderer the scene is
        /// first rendered onto geometry buffers and only after that is it rendered onto the back
        /// buffer.
        /// </summary>
        public override void Draw()
        {
            if (!mInitialized)
                return;

            // If the viewport has changed we have to create all the textures again.
            Viewport vp = mGraphicsDevice.Viewport;
            if (mViewport.Width != vp.Width || mViewport.Height != vp.Height)
            {
                mViewport = mGraphicsDevice.Viewport;
                CreateTextures();
            }

            // We're beginning a new frame so we swap the references of the render targets we used 
            // to draw the last frame with the ones which pointed to the frame before the last one.
            // We will then render this new frame onto those render targets whose contents we don't 
            // need anymore.
            SwapMainRenderTargets();

            //----------------------------------------------//
            // DRAW GEOMETRY //
            //----------------------------------------------//
            DrawGeometry();

            //----------------------------------------------//
            // RENDER LIGHTS
            //----------------------------------------------//
            DrawLights();

            //----------------------------------------------//
            // COMBINE COLOR MAP WITH LIGHT MAP
            //----------------------------------------------//
            CombineColorAndLight();

            //----------------------------------------------//
            // POST-PROCESSING EFFECTS
            //----------------------------------------------//
            ApplyPostProcessEffects();

            //----------------------------------------------//
            // GUI -> GuiDrawMode is FINAL_TEXTURE or SEPARATE_TEXTURE
            //----------------------------------------------//
            if (this.GuiDrawMode == Graphics.GuiDrawMode.FINAL_TEXTURE ||
                this.GuiDrawMode == Graphics.GuiDrawMode.SEPARATE_TEXTURE)
            {
                DrawGui();
            }

            //----------------------------------------------//
            // FINAL IMAGE ONTO BACK BUFFER (IF ENABLED)
            //----------------------------------------------//
            // Draw the final frame texture onto the back buffer if the renderer is configured to 
            //do so.
            if (this.DrawToBackBuffer)
            {
                // Set the render target as the back buffer because we're done rendering to the 
                // other auxiliary textures.
                mGraphicsDevice.SetRenderTarget(null);
                
                // Draw the frame onto the back buffer.
                DrawFinalTexture();

                //----------------------------------------------//
                // GUI -> GuiDrawMode is BACK_BUFFER
                //----------------------------------------------//
                DrawGui();
            }

            //----------------------------------------------//
            // DEBUG LAYER
            //----------------------------------------------//
            if (this.DebugLayerEnabled)
            {
                DrawDebugLayer();
            }

            mGraphicsDevice.SetRenderTarget(null);
        }

        #endregion

        #region Plugin members

        /// <summary>
        /// Initializes the renderer. Here we know that all the plug-ins have been created and
        /// added to the root.
        /// </summary>
        public override void Initialize()
        {
            //Save local references of important and frequently accessed objects.
            mGraphicsDevice = Root.GraphicsDevice;
            mSceneManager = Root.SceneManager;
            mGuiManager = Root.GuiManager;
            mContentManager = Root.ContentManager;

            mGraphicsDevice.DeviceReset += new EventHandler<EventArgs>(Renderer_DeviceReset);
            mGraphicsDevice.DeviceLost += new EventHandler<EventArgs>(Renderer_DeviceLost);

            //Save the current viewport which will be used to create the textures.
            mViewport = mGraphicsDevice.Viewport;

            //Create a new quad renderer that will be used to render full screen
            //quads.
            mQuadRenderer = new QuadRenderer(Root);

            //Create a new sprite batch.
            mSpriteBatch = new SpriteBatch(mGraphicsDevice);

            //Save the value of the half pixel to align pixels to texels.
            mHalfPixel.X = 0.5f / (float)mGraphicsDevice.PresentationParameters.BackBufferWidth;
            mHalfPixel.Y = 0.5f / (float)mGraphicsDevice.PresentationParameters.BackBufferHeight;

            if (!mLoaded)
            {
                //Create the textures used for rendering.
                CreateTextures();
                //Load the effects used by the renderer.
                LoadEffects();
                //Load the models used by the renderer.
                LoadModels();

                mLoaded = true;
            }

            mInitialized = true;
        }

        /// <summary>
        /// Destroys all of of the resources used by the object.
        /// </summary>
        public override void Destroy()
        {
            mClearGBuffersEffect = null;
            mCombineFinalEffect = null;
            mDirLightEffect = null;
            mPointLightEffect = null;
            mSpotLightEffect = null;
            mSphereModel = null;
            mConeModel = null;

            DisposeObject(mSpriteBatch);
            DisposeObject(mClearGBuffersEffect);
            DisposeObject(mCombineFinalEffect);
            DisposeObject(mColorGBuffer);
            DisposeObject(mNormalGBuffer);
            DisposeObject(mAmbientGBuffer);
            DisposeObject(mDepthGBuffer);
            DisposeObject(mFinalTexture);
            DisposeObject(mLightMap);
            DisposeObject(mSceneTexture);
            DisposeObject(mPostProcessedTexture);
            DisposeObject(mPreviousDepthBuffer);
            DisposeObject(mPreviousSceneTexture);
            DisposeObject(mPreviousFrameTexture);
            DisposeObject(mAuxRT1);
            DisposeObject(mAuxRT2);
            DisposeObject(mCurrentInRT);
            DisposeObject(mCurrentOutRT);

            mSpriteBatch = null;
            mClearGBuffersEffect = null;
            mCombineFinalEffect = null;
            mColorGBuffer = null;
            mNormalGBuffer = null;
            mAmbientGBuffer = null;
            mDepthGBuffer = null;
            mFinalTexture = null;
            mLightMap = null;
            mSceneTexture = null;
            mPostProcessedTexture = null;
            mPreviousDepthBuffer = null;
            mPreviousSceneTexture = null;
            mPreviousFrameTexture = null;
            mAuxRT1 = null;
            mAuxRT2 = null;
            mCurrentInRT = null;
            mCurrentOutRT = null;

            mInitialized = false;
        }

        #endregion

        #region Private/Protected Methods

        /// <summary>
        /// Safely disposes the specified object.
        /// </summary>
        /// <param name="o"></param>
        private void DisposeObject(IDisposable o)
        {
            if (o != null)
            {
                o.Dispose();
            }
        }

        #region Resource Creation/Loading

        /// <summary>
        /// Creates the textures required by the renderer.
        /// </summary>
        private void CreateTextures()
        {
            int width = mViewport.Width;
            int height = mViewport.Height;
            SurfaceFormat format = SurfaceFormat.Color;
            SurfaceFormat depthBufFormat = SurfaceFormat.Single;

			//Create the render targets used to render the scene.
            mColorGBuffer = new RenderTarget2D(mGraphicsDevice, width, height, false, format, 
                DepthFormat.Depth24, 0, RenderTargetUsage.DiscardContents);
            mNormalGBuffer = new RenderTarget2D(mGraphicsDevice, width, height, false, format, 
                DepthFormat.None, 0, RenderTargetUsage.DiscardContents);
            mAmbientGBuffer = new RenderTarget2D(mGraphicsDevice, width, height, false, format, 
                DepthFormat.None, 0, RenderTargetUsage.DiscardContents);
            mDepthGBuffer = new RenderTarget2D(mGraphicsDevice, width, height, false, 
                depthBufFormat, DepthFormat.None, 0, RenderTargetUsage.DiscardContents);

            //Create the light map.
            mLightMap = new RenderTarget2D(mGraphicsDevice, width, height, false, format, 
                DepthFormat.None, 0, RenderTargetUsage.PreserveContents);

            mPreviousDepthBuffer = new RenderTarget2D(mGraphicsDevice, width, height, false,
                depthBufFormat, DepthFormat.None, 0, RenderTargetUsage.DiscardContents);
            mPreviousSceneTexture = new RenderTarget2D(mGraphicsDevice, width, height, false,
                format, DepthFormat.None, 0, RenderTargetUsage.DiscardContents);
            mPreviousFrameTexture = new RenderTarget2D(mGraphicsDevice, width, height, false,
                format, DepthFormat.None, 0, RenderTargetUsage.DiscardContents);

			//Create the render targets used to render the post-processing effects.
            mAuxRT1 = new RenderTarget2D(mGraphicsDevice, width, height, false, format, 
                DepthFormat.None, 0, RenderTargetUsage.DiscardContents);
            mAuxRT2 = new RenderTarget2D(mGraphicsDevice, width, height, false, format, 
                DepthFormat.None, 0, RenderTargetUsage.DiscardContents);
			mCurrentInRT = mAuxRT1;
			mCurrentOutRT = mAuxRT2;

			//Create a texture onto which we will render the scene.
            mSceneTexture = new RenderTarget2D(mGraphicsDevice, width, height, false, format,
                DepthFormat.None, 0, RenderTargetUsage.DiscardContents);
			//Create a texture onto which we will render the scene with the post-processing effects
            //applied to it.
            mPostProcessedTexture = new RenderTarget2D(mGraphicsDevice, width, height, false, format, 
                DepthFormat.None, 0, RenderTargetUsage.DiscardContents);

            //Create the texture onto which we will draw the shadow map for a directional light.
            CreateShadowMapTexture();
        }

        /// <summary>
        /// Creates the texture onto which the shadow map will be drawn.
        /// </summary>
        private void CreateShadowMapTexture()
        {
            // If we already had a shadow map texture we dispose it because we're not going to need
            // it anymore.
            if (mDirShadowMap != null)
            {
                mDirShadowMap.Dispose();
            }

            //Surface format used for the shadow map.
            SurfaceFormat format = SurfaceFormat.Single;

            switch (this.ShadowMapQuality)
            {
                case Graphics.ShadowMapQuality.LOW:
                    mDirShadowMap = new RenderTarget2D(mGraphicsDevice, 1024, 1024, false,
                        format, DepthFormat.Depth24, 0, RenderTargetUsage.DiscardContents);
                    break;
                case Graphics.ShadowMapQuality.NORMAL:
                    mDirShadowMap = new RenderTarget2D(mGraphicsDevice, 2048, 2048, false,
                        format, DepthFormat.Depth24, 0, RenderTargetUsage.DiscardContents);
                    break;
                case Graphics.ShadowMapQuality.HIGH:
                    mDirShadowMap = new RenderTarget2D(mGraphicsDevice, 4096, 4096, false,
                        format, DepthFormat.Depth24, 0, RenderTargetUsage.DiscardContents);
                    break;
            }
        }

        /// <summary>
        /// Creates the texture onto which the GUI is to be rendered if the GuiDrawMode property is
        /// set to SEPARATE_TEXTURE.
        /// </summary>
        private void CreateGuiTexture()
        {
            // We create the GUI texture if the GuiDrawMode property is set to SEPARATE_TEXTURE.
            if (this.GuiDrawMode == Graphics.GuiDrawMode.SEPARATE_TEXTURE && mGuiTexture == null)
            {
                int width = mViewport.Width;
                int height = mViewport.Height;
                SurfaceFormat format = SurfaceFormat.Color;

                mGuiTexture = new RenderTarget2D(mGraphicsDevice, width, height, false, format,
                    DepthFormat.None, 0, RenderTargetUsage.DiscardContents);
            }
            // If we don't need a GUI texture and we have one, we dispose it.
            else if (mGuiTexture != null)
            {
                mGuiTexture.Dispose();
            }
        }

        /// <summary>
        /// Loads the effects required by the renderer.
        /// </summary>
        private void LoadEffects()
        {
            mClearGBuffersEffect = mContentManager.Load<Effect>("Effects/ClearGBuffers");
            mCombineFinalEffect = mContentManager.Load<Effect>("Effects/CombineFinal");
            mDirLightEffect = mContentManager.Load<Effect>("Effects/DirectionalLight");
            mDirShadowMapEffect = mContentManager.Load<Effect>("Effects/DirectionalShadowMap");
            mPointLightEffect = mContentManager.Load<Effect>("Effects/PointLight");
            mSpotLightEffect = mContentManager.Load<Effect>("Effects/SpotLight");
        }

        /// <summary>
        /// Loads the models required by the renderer.
        /// </summary>
        private void LoadModels()
        {
            mSphereModel = mContentManager.Load<Model>("Models/SphereLowPoly");
            mConeModel = mContentManager.Load<Model>("Models/Cone");
        }

        #endregion

        /// <summary>
        /// Draws the geometry of the scene.
        /// </summary>
        private void DrawGeometry()
        {
            ResetStates();
            SetGBuffers();
            ClearGBuffers();

            // Lists used to store the drawables that are to be drawn after the rest of the scene.
            List<List<IDrawableObject>> layers = new List<List<IDrawableObject>>();

            // Draw the first layer and add drawables belonging to subsequent layers to
            // different lists to be rendered afterwards.
            foreach (IDrawableObject drawable in mSceneManager.VisibleDrawableNodes)
            {
                // If the drawable belongs to layer 0 we can draw it right away.
                if (drawable.Layer == 0)
                {
                    Material mat = drawable.Material;
                    if (mat != null)
                    {
                        drawable.Material.Apply(drawable.NodeWithTransforms);
                        drawable.Draw();
                        ResetStates();
                    }
                }
                // If the drawable belongs to a layer for which we still haven't got a list
                // of drawables we add lists of drawables and then add the drawable to the
                // appropriate list.
                else if (drawable.Layer > layers.Count)
                {
                    int layersToAdd = drawable.Layer - layers.Count;
                    for (int i = 0; i < layersToAdd; ++i)
                    {
                        layers.Add(new List<IDrawableObject>());
                    }
                    layers[drawable.Layer - 1].Add(drawable);
                }
                // If the drawable belongs to a layer for which we already have a list of drawables
                // we simply add it to the list.
                else
                {
                    layers[drawable.Layer - 1].Add(drawable);
                }
            }

            // Draw all the subsequent layers.
            foreach (List<IDrawableObject> layer in layers)
            {
                mGraphicsDevice.Clear(ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);
                foreach (IDrawableObject drawable in layer)
                {
                    drawable.Material.Apply(drawable.NodeWithTransforms);
                    drawable.Draw();
                    ResetStates();
                }
            }
        }

        #region Lighting

        /// <summary>
        /// Draws the lights onto the light map.
        /// </summary>
        private void DrawLights()
        {
            try
		    {
                mCamera = mSceneManager.ActiveCamera;
                mCameraViewMatrix = mCamera.ViewMatrix;
                mProjectionMatrix = mCamera.ProjectionMatrix;
                mViewProjectionMatrix = mCameraViewMatrix * mProjectionMatrix;
                mInverseViewProjection = Matrix.Invert(mViewProjectionMatrix);
                mCameraPosition = mCamera.AbsolutePosition;

                IList<LightNode> lights = Root.SceneManager.LightsList;
                mGraphicsDevice.SetRenderTarget(mLightMap);
                //Clear the light map.
                mGraphicsDevice.Clear(Color.Transparent);

			    //Set the parameters that are common to all the effects.
			    SetCommonParameters();

                List<IDrawableObject> shadowCasters = mSceneManager.ShadowCastersList;

                //Render each light in the list of lights.
                foreach(LightNode light in lights)
                {
				    switch(light.LightType)
				    {
                        case LightType.LIGHT_POINT:
					    {
                            DrawPointLight(light, shadowCasters);
						    break;
					    }
                        case LightType.LIGHT_DIR:
                        {
                            DrawDirLight(light, shadowCasters);
                            break;
                        }
                        case LightType.LIGHT_SPOT:
					    {
                            DrawSpotLight(light, shadowCasters);
						    break;
					    }
					    default:
					    {
						    break;
					    }
				    }
			    }
		    }
		    catch(Exception e)
		    {
			    throw new Exception("DeferredRenderer.RenderLights(): " + e.Message);
		    }
        }

        /// <summary>
        /// Sets parameters that are common to all of the lights.
        /// </summary>
        private void SetCommonParameters()
        {
            //-----------------------------------//
            //DIRECTIONAL LIGHT SHADER PARAMETERS//
            //-----------------------------------//
            mDirLightEffect.Parameters["cameraPosition"].SetValue(mCameraPosition);
            mDirLightEffect.Parameters["InvertViewProjection"].SetValue(mInverseViewProjection);
            mDirLightEffect.Parameters["colorMap"].SetValue(mColorGBuffer);
            mDirLightEffect.Parameters["normalMap"].SetValue(mNormalGBuffer);
            mDirLightEffect.Parameters["depthMap"].SetValue(mDepthGBuffer);
            mDirLightEffect.Parameters["ambientMap"].SetValue(mAmbientGBuffer);
            mDirLightEffect.Parameters["halfPixel"].SetValue(mHalfPixel);

            //-----------------------------//
            //POINT LIGHT SHADER PARAMETERS//
            //-----------------------------//
            mPointLightEffect.Parameters["View"].SetValue(mCameraViewMatrix);
            mPointLightEffect.Parameters["Projection"].SetValue(mProjectionMatrix);
            mPointLightEffect.Parameters["cameraPosition"].SetValue(mCameraPosition);
            mPointLightEffect.Parameters["InvertViewProjection"].SetValue(mInverseViewProjection);
            mPointLightEffect.Parameters["normalMap"].SetValue(mNormalGBuffer);
            mPointLightEffect.Parameters["depthMap"].SetValue(mDepthGBuffer);
            mPointLightEffect.Parameters["ambientMap"].SetValue(mAmbientGBuffer);
            mPointLightEffect.Parameters["halfPixel"].SetValue(mHalfPixel);

            //----------------------------//
            //SPOT LIGHT SHADER PARAMETERS//
            //----------------------------//
            mSpotLightEffect.Parameters["cameraPosition"].SetValue(mCameraPosition);
            mSpotLightEffect.Parameters["InvertViewProjection"].SetValue(mInverseViewProjection);
            mSpotLightEffect.Parameters["normalMap"].SetValue(mNormalGBuffer);
            mSpotLightEffect.Parameters["depthMap"].SetValue(mDepthGBuffer);
            mSpotLightEffect.Parameters["ambientMap"].SetValue(mAmbientGBuffer);
            mSpotLightEffect.Parameters["halfPixel"].SetValue(mHalfPixel);
        }

        /// <summary>
        /// Draws a directional light onto the light map.
        /// </summary>
        /// <param name="light">Light which is to be rendered.</param>
        /// <param name="shadowCasters">List of shadow casters in the scene.</param>
        private void DrawDirLight(LightNode light, List<IDrawableObject> shadowCasters)
        {
            try
		    {
                bool useShadowMap = false;
                Matrix lightViewProjection = Matrix.Identity;

                //If shadows are enabled for the light and there are shadow casters in the scene
                //we must draw a shadow map.
                if (light.ShadowsEnabled && shadowCasters.Count > 0)
                {
                    lightViewProjection = CreateOrthoLightViewProjectionMatrix(light);
                    DrawDirectionalShadowMap(light, shadowCasters, lightViewProjection);
                    useShadowMap = true;
                }
                
                BeginLighting();

                //Set the light properties on the effect.
                mDirLightEffect.Parameters["lightDirection"].SetValue(light.Direction);
                mDirLightEffect.Parameters["lightIntensity"].SetValue(light.Intensity);
                mDirLightEffect.Parameters["lightDiffuseColor"].SetValue(light.DiffuseColor.ToVector3());
                mDirLightEffect.Parameters["lightSpecularColor"].SetValue(light.SpecularColor.ToVector3());

                if (!useShadowMap)
                {
                    mDirLightEffect.CurrentTechnique = mDirLightEffect.Techniques[0];
                    //If there's no shadow map we draw the light as usual.
                    mDirLightEffect.Techniques[0].Passes[0].Apply();
                }
                else
                {
                    //Set the parameters on the effect.
                    mDirLightEffect.Parameters["LightViewProj"].SetValue(lightViewProjection);
                    mDirLightEffect.Parameters["shadowMap"].SetValue(mDirShadowMap);
                    mDirLightEffect.CurrentTechnique = mDirLightEffect.Techniques[1];
                    //If there's a shadow map we use a different technique to exclude from the
                    //lightmap all the pixels which are in shadow.
                    mDirLightEffect.Techniques[1].Passes[0].Apply();
                }

                //Draw a full screen quad to render the light onto the light map.
                mQuadRenderer.Draw();

                EndLighting();
		    }
		    catch(Exception e)
		    {
			    throw new Exception("DeferredRenderer.RenderDirLight(): " + e.Message);
		    }
        }

        /// <summary>
        /// Draws a point light onto the light map.
        /// </summary>
        /// <param name="light">Light which is to be rendered.</param>
        /// /// <param name="shadowCasters">List of shadow casters in the scene.</param>
        private void DrawPointLight(LightNode light, List<IDrawableObject> shadowCasters)
        {
            try
            {
                BeginLighting();

			    //Get the absolute position of the light and create the world matrix for it.
			    Vector3 lightPos = light.AbsolutePosition;
                float r = light.Radius * 1.1f;
			    Matrix worldMatrix = Matrix.CreateScale(r, r, r) * Matrix.CreateTranslation(lightPos);

                mGraphicsDevice.RasterizerState = RasterizerState.CullClockwise;

			    //Set the world matrix on the effect.
			    mPointLightEffect.Parameters["World"].SetValue(worldMatrix);
			    mPointLightEffect.Parameters["lightPosition"].SetValue(lightPos);
                mPointLightEffect.Parameters["lightDiffuseColor"].SetValue(light.DiffuseColor.ToVector3());
                mPointLightEffect.Parameters["lightSpecularColor"].SetValue(light.SpecularColor.ToVector3());
			    mPointLightEffect.Parameters["lightRadius"].SetValue(light.Radius);
			    mPointLightEffect.Parameters["lightIntensity"].SetValue(light.Intensity);

                //Set the effect on each mesh part of the sphere mesh and draw the sphere.
			    foreach (ModelMesh mesh in mSphereModel.Meshes)
                {
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        part.Effect = mPointLightEffect;
                    }
                    mesh.Draw();
                }

			    //Reset the device states.
			    mGraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;

                EndLighting();
		    }
		    catch(Exception e)
		    {
			    throw new Exception("DeferredRenderer.RenderPointLight: " + e.Message);
		    }
        }

        /// <summary>
        /// Draws a spot light onto the light map.
        /// </summary>
        /// <param name="light">Light which is to be rendered.</param>
        /// /// <param name="shadowCasters">List of shadow casters in the scene.</param>
        private void DrawSpotLight(LightNode light, List<IDrawableObject> shadowCasters)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Prepares the device to render one light.
        /// </summary>
        private void BeginLighting()
        {
            //Set the light map as the current render target.
            mGraphicsDevice.SetRenderTarget(mLightMap);
            //Disable the depth buffer.
            mGraphicsDevice.DepthStencilState = DepthStencilState.None;
            //Enable blending
            mGraphicsDevice.BlendState = BlendState.AlphaBlend;
        }

        /// <summary>
        /// Ends the rendering of one light.
        /// </summary>
        private void EndLighting()
        {
            mGraphicsDevice.BlendState = BlendState.Opaque;
            mGraphicsDevice.DepthStencilState = DepthStencilState.Default;
        }

        /// <summary>
        /// Draws the a shadow map for a directional light.
        /// </summary>
        /// <param name="light">The light for which a shadow map is to be generated.</param>
        /// <param name="shadowCasters">The shadow casters in the scene.</param>
        /// <param name="lightViewProjection">Matrix to transform points to light space.</param>
        private void DrawDirectionalShadowMap(LightNode light,
            List<IDrawableObject> shadowCasters, Matrix lightViewProjection)
        {
            mGraphicsDevice.BlendState = BlendState.Opaque;
            mGraphicsDevice.DepthStencilState = DepthStencilState.Default;
            mGraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;

            //Set the parameters needed to draw the shadow map.
            mDirShadowMapEffect.Parameters["LightViewProj"].SetValue(lightViewProjection);

            mGraphicsDevice.SetRenderTarget(mDirShadowMap);
            mGraphicsDevice.Clear(Color.White);

            //Draw each shadow caster onto the shadow map.
            foreach (IDrawableObject shadowCaster in shadowCasters)
            {
                mDirShadowMapEffect.Parameters["LightViewProj"].SetValue(lightViewProjection);
                mDirShadowMapEffect.Parameters["World"].SetValue(shadowCaster.NodeWithTransforms.WorldTransformMatrix);
                mDirShadowMapEffect.Techniques[0].Passes[0].Apply();
                shadowCaster.Draw();
            }
        }

        /// <summary>
        /// Creates the WorldViewProjection matrix from the perspective of the 
        /// light using the cameras bounding frustum to determine what is visible 
        /// in the scene.
        /// </summary>
        /// <param name="light">Light for which the matrix is to be calculated.</param>
        /// <returns>The WorldViewProjection for the light</returns>
        private Matrix CreateOrthoLightViewProjectionMatrix(LightNode light)
        {
            // Matrix with that will rotate in points the direction of the light
            Matrix lightRotation = Matrix.CreateLookAt(Vector3.Zero,
                                                       -light.Direction,
                                                       Vector3.Up);

            // Get the corners of the frustum
            Vector3[] frustumCorners = mCamera.Frustum.GetCorners();

            // Transform the positions of the corners into the direction of the light
            for (int i = 0; i < frustumCorners.Length; i++)
            {
                frustumCorners[i] = Vector3.Transform(frustumCorners[i], lightRotation);
            }

            // Find the smallest box around the points
            BoundingBox lightBox = BoundingBox.CreateFromPoints(frustumCorners);

            Vector3 boxSize = lightBox.Max - lightBox.Min;
            Vector3 halfBoxSize = boxSize * 0.5f;

            // The position of the light should be in the center of the back
            // pannel of the box. 
            Vector3 lightPosition = lightBox.Min + halfBoxSize;
            lightPosition.Z = lightBox.Min.Z;

            // We need the position back in world coordinates so we transform 
            // the light position by the inverse of the lights rotation
            lightPosition = Vector3.Transform(lightPosition,
                                              Matrix.Invert(lightRotation));

            // Create the view matrix for the light
            Matrix lightView = Matrix.CreateLookAt(lightPosition,
                                                   lightPosition - light.Direction,
                                                   Vector3.Up);

            // Create the projection matrix for the light
            // The projection is orthographic since we are using a directional light
            Matrix lightProjection = Matrix.CreateOrthographic(boxSize.X, boxSize.Y,
                                                               -boxSize.Z, boxSize.Z);

            return lightView * lightProjection;
        }

        #endregion

        /// <summary>
        /// Combines the color g-buffer with the light map to produce the final scene.
        /// </summary>
        private void CombineColorAndLight()
        {
            try
		    {
                //The final scene image will be rendered onto the mSceneTexture render target.
                mGraphicsDevice.SetRenderTarget(mSceneTexture);
                //Clear the scene texture before rendering onto it.
                mGraphicsDevice.Clear(mBackgroundColor);

			    //Set the color map, the light map and the depth map on the effect.
			    mCombineFinalEffect.Parameters["colorMap"].SetValue(mColorGBuffer);
			    mCombineFinalEffect.Parameters["lightMap"].SetValue(mLightMap);
			    mCombineFinalEffect.Parameters["depthMap"].SetValue(mDepthGBuffer);
                mCombineFinalEffect.Parameters["ambientMap"].SetValue(mAmbientGBuffer);
                mCombineFinalEffect.Parameters["halfPixel"].SetValue(mHalfPixel);

                //Draw a full screen quad to combine the color, depth and light maps into the final
                //scene image.
                foreach (EffectPass pass in mCombineFinalEffect.Techniques[0].Passes)
                {
                    pass.Apply();
                    mQuadRenderer.Draw();
                }
		    }
		    catch(Exception e)
		    {
			    throw new Exception("DeferredRenderer.CombineColorAndLight(): " + e.Message);
		    }
        }

        /// <summary>
        /// Applies all the post-processing effects in the post-processing effects manager.
        /// </summary>
        private void ApplyPostProcessEffects()
        {
            IPostProcessManager postProcMgr = Root.PostProcessManager;

		    foreach(PostProcessEffectChain chain in postProcMgr.EnabledChainsList)
		    {
			    //Get all the effects in the the effect chain.
			    List<PostProcessEffect> effects = chain.EffectsList;

			    int numEffects = effects.Count;
			    int lastEffectIndex = numEffects - 1;
			    for(int effect_i = 0; effect_i < numEffects; ++effect_i)
			    {
				    //If we're rendering the last effect of the chain we render it to the 
                    //mFinalTexture render target.
				    if(effect_i == lastEffectIndex)
				    {
                        mGraphicsDevice.SetRenderTarget(mPostProcessedTexture);
				    }
				    //If we're not rendering the last effect of the chain we render it to on one of
                    //the auxiliary render targets.
				    else
				    {
					    mGraphicsDevice.SetRenderTarget(mCurrentOutRT);
				    }

				    //Get the next effect.
				    PostProcessEffect effect = effects[effect_i];

				    //Set the effect parameters, states and pixel shader on the graphics device.
                    effect.Apply();
				    //Render the full screen quad.
				    mQuadRenderer.Draw();
				    //Swap mCurrentInTR and mCurrentOutRT: the output becomes the new input and the
                    //input becomes the new output.
				    SwapAuxRenderTargets();				
			    }
		    }

            // Make sure se set the correct final texture.
            int numChains = Root.PostProcessManager.EnabledChainsList.Count;
            mFinalTexture = numChains == 0 ? mSceneTexture : mPostProcessedTexture;
        }

        /// <summary>
        /// Draws the final texture onto the backbuffer.
        /// </summary>
        private void DrawFinalTexture()
        {
            // Set the viewport because the previous steps probably changed the viewport in order to
            // draw to the full textures and not just to part of them.
            mGraphicsDevice.Viewport = mViewport;

            mSpriteBatch.Begin(0, BlendState.Opaque, SamplerState.PointClamp, null, null);

            //If we applied some post-processing effects we draw the processed scene texture.
            mSpriteBatch.Draw(mFinalTexture, new Rectangle(0, 0, mViewport.Width,
                mViewport.Height), Color.White);

            mSpriteBatch.End();
        }

        /// <summary>
        /// Draws the GUI onto the back buffer, a separate texture or the final frame texture.
        /// </summary>
        private void DrawGui()
        {
            // Set the viewport because the previous steps probably changed the viewport in order to
            // draw to the full textures and not just to part of them.
            mGraphicsDevice.Viewport = mViewport;

            // Set up the correct render target.
            switch(this.GuiDrawMode)
            {
                case Graphics.GuiDrawMode.BACK_BUFFER:
                    mGraphicsDevice.SetRenderTarget(null);
                    break;
                case Graphics.GuiDrawMode.SEPARATE_TEXTURE:
                    mGraphicsDevice.SetRenderTarget(mGuiTexture);
                    break;
                default:
                    break;
            }

            // Draw all of the drawable GUI nodes.
            foreach (IDrawable node in mGuiManager.VisibleDrawableNodes)
            {
                node.Draw();
            }

            //---------------------------------------------------------------------------------//

            // Lists used to store the GUI drawable nodes that are to be drawn after the others.
            List<List<IDrawable>> layers = new List<List<IDrawable>>();

            // Draw the first layer and add drawables belonging to subsequent layers to
            // layer lists to be rendered afterwards.
            foreach (IDrawable drawable in mGuiManager.VisibleDrawableNodes)
            {
                // If the drawable belongs to layer 0 we can draw it right away (no need to waste
                // time adding it to a list).
                if (drawable.Layer == 0)
                {
                    drawable.Draw();
                }
                // If the drawable belongs to a layer for which we still haven't got a list
                // we add new lists and then add the drawable to the appropriate list.
                else if (drawable.Layer > layers.Count)
                {
                    int layersToAdd = drawable.Layer - layers.Count;

                    // Add the missing layers.
                    for (int i = 0; i < layersToAdd; ++i)
                    {
                        layers.Add(new List<IDrawable>());
                    }

                    // Add the drawable to the correct layer.
                    layers[drawable.Layer - 1].Add(drawable);
                }
                // If the drawable belongs to a layer for which we already have a list we simply 
                // add it to the list.
                else
                {
                    layers[drawable.Layer - 1].Add(drawable);
                }
            }

            // Draw all the subsequent layers.
            foreach (List<IDrawable> layer in layers)
            {
                mGraphicsDevice.Clear(ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);

                // Draw all the GUI drawable nodes in the layer.
                foreach (IDrawable drawable in layer)
                {
                    drawable.Draw();
                }
            }
        }

        /// <summary>
        /// Draws the debug layer.
        /// </summary>
        private void DrawDebugLayer()
        {
            mSpriteBatch.Begin(0, BlendState.Opaque, SamplerState.PointClamp, null, null);

            mSpriteBatch.Draw(mColorGBuffer, new Rectangle(0, 0, 200, 150), Color.White);
            mSpriteBatch.Draw(mNormalGBuffer, new Rectangle(200, 0, 200, 150), Color.White);
            mSpriteBatch.Draw(mDepthGBuffer, new Rectangle(400, 0, 200, 150), Color.White);
            //mSpriteBatch.Draw(mLightMap, new Rectangle(600, 0, 200, 150), Color.White);
            mSpriteBatch.Draw(mDirShadowMap, new Rectangle(600, 0, 200, 150), Color.White);

            mSpriteBatch.End();
        }

        #region Utilities

        /// <summary>
        /// Resets the device states.
        /// </summary>
        private void ResetStates()
        {
            //Reset the device states.
            mGraphicsDevice.BlendState = BlendState.Opaque;
            mGraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            mGraphicsDevice.DepthStencilState = DepthStencilState.Default;
        }

        /// <summary>
        /// Sets the G-buffers on the graphics device as render targets.
        /// </summary>
        private void SetGBuffers()
        {
            mGraphicsDevice.SetRenderTargets(mColorGBuffer, mNormalGBuffer, mDepthGBuffer, mAmbientGBuffer);
        }

        /// <summary>
        /// Clears the G-buffers.
        /// </summary>
        private void ClearGBuffers()
        {
            mGraphicsDevice.DepthStencilState = DepthStencilState.None;
            mClearGBuffersEffect.Techniques[0].Passes[0].Apply();
            SamplerState state = new SamplerState();
            state.Filter = TextureFilter.Point;
            mGraphicsDevice.SamplerStates[0] = state;
            mGraphicsDevice.SamplerStates[1] = state;
            mGraphicsDevice.SamplerStates[2] = state;
            mGraphicsDevice.SamplerStates[3] = state;
            mGraphicsDevice.SamplerStates[4] = state;
            mGraphicsDevice.SamplerStates[5] = state;
            mGraphicsDevice.SamplerStates[6] = state;
            mGraphicsDevice.SamplerStates[7] = state;
            mGraphicsDevice.SamplerStates[8] = state;
            mGraphicsDevice.SamplerStates[9] = state;
            mGraphicsDevice.SamplerStates[10] = state;
            mGraphicsDevice.SamplerStates[11] = state;
            mGraphicsDevice.SamplerStates[12] = state;
            mGraphicsDevice.SamplerStates[13] = state;
            mGraphicsDevice.SamplerStates[14] = state;
            mGraphicsDevice.SamplerStates[15] = state;
            mQuadRenderer.Draw();
            mGraphicsDevice.DepthStencilState = DepthStencilState.Default;
        }

        /// <summary>
        /// Swaps the newly created render targets with the previous frame ones.
        /// </summary>
        private void SwapMainRenderTargets()
        {
            RenderTarget2D depthTemp, sceneTemp, finalTemp;
            depthTemp = mDepthGBuffer;
            sceneTemp = mSceneTexture;
            finalTemp = mPostProcessedTexture;

            mDepthGBuffer = mPreviousDepthBuffer;
            mSceneTexture = mPreviousSceneTexture;
            mPostProcessedTexture = mPreviousFrameTexture;

            mPreviousDepthBuffer = depthTemp;
            mPreviousSceneTexture = sceneTemp;
            mPreviousFrameTexture = finalTemp;
        }

        /// <summary>
        /// Swaps the references to the auxiliary render targets used to render post-processing 
        /// effects.
        /// </summary>
        private void SwapAuxRenderTargets()
	    {
		    RenderTarget2D temp = mCurrentInRT;
		    mCurrentInRT = mCurrentOutRT;
		    mCurrentOutRT = temp;
	    }

        /// <summary>
        /// Creates a new render target with the same properties as the one passed as an argument.
        /// </summary>
        /// <param name="rt">Render target whose properties are to be copied.</param>
        /// <returns>
        /// A render target which has the same properties as the one passed as an argument.
        /// </returns>
        private RenderTarget2D CreateCompatible(RenderTarget2D rt)
        {
            return new RenderTarget2D(mGraphicsDevice, rt.Width, rt.Height, false, rt.Format,
               rt.DepthStencilFormat, rt.MultiSampleCount, rt.RenderTargetUsage);
        }

        #endregion

        /// <summary>
        /// Handles the DeviceReset event of the Renderer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Renderer_DeviceReset(object sender, EventArgs args)
        {
            try
            {
                CreateTextures();
                LoadEffects();
                LoadModels();
            }
            catch (Exception e)
            {
                System.Console.Out.WriteLine("Error reseting the device: " + e.Message);
            }
        }

        /// <summary>
        /// Handles the DeviceLost event of the mGraphicsDevice control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Renderer_DeviceLost(object sender, EventArgs args)
        {
        }

        #endregion
    }
}
