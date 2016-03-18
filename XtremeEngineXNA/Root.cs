using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XtremeEngineXNA.EntityComponent;
using XtremeEngineXNA.Graphics;
using XtremeEngineXNA.Physics;
using XtremeEngineXNA.Scene;
using XtremeEngineXNA.Animation;
using XtremeEngineXNA.Gui;

namespace XtremeEngineXNA
{
    /// <summary>
    /// Main class of XtremeEngine. It is responsible for managing all the managers used to display
    /// graphics on the screen.
    /// </summary>
    public class Root
    {
        #region Attributes

        /// <summary>
        /// Service provider used to locate the implementations of the services required by the
        /// engine. For instance, this service provider is used to locate the implementation of
        /// the IGraphicsDeviceService which in turn is used to get the graphics device that the
        /// engine should use for rendering.
        /// </summary>
        private IServiceProvider mServices;

        /// <summary>
        /// Graphics device used for drawing.
        /// </summary>
        private GraphicsDevice mGraphicsDevice;

        /// <summary>
        /// Default content manager. This is the content manager which is returned by the property
        /// ContentManager.
        /// </summary>
        private ContentManager mDefaultContentManager;

        /// <summary>
        /// Dictionary with all the content managers created by the root.
        /// </summary>
        private Dictionary<string, ContentManager> mCustomContentManagers;

        /// <summary>
        /// Scene manager used by the root.
        /// </summary>
        private ISceneManager mSceneManager;
        
        /// <summary>
        /// Post-processing effects manager used by the root.
        /// </summary>
        private IPostProcessManager mPostProcessManager;

        /// <summary>
        /// Renderer used by the root.
        /// </summary>
        private IRenderer mRenderer;

        /// <summary>
        /// Physics manager used to simulate the physics of the objects in the engine.
        /// </summary>
        private IPhysicsManager mPhysicsManager;

        /// <summary>
        /// Manager which manages all the entities of a game.
        /// </summary>
        private IEntityManager mEntityManager;

        /// <summary>
        /// Manager which manages all the animations.
        /// </summary>
        private IAnimationManager mAnimationManager;

        /// <summary>
        /// Manager which manages all the GUI elements.
        /// </summary>
        private IGuiManager mGuiManager;

        /// <summary>
        /// Dictionary with all the plugins in the engine.
        /// </summary>
        private Dictionary<string, IPlugin> mPlugins;

        /// <summary>
        /// List with all the updateable plugins in the engine.
        /// </summary>
        private List<IUpdateable> mUpdateablePlugins;

        /// <summary>
        /// List with all the drawable plugins in the engine. This is used to speed up drawing.
        /// </summary>
        private List<IDrawable> mDrawablePlugins;

        /// <summary>
        /// Whether the engine has been initialized.
        /// </summary>
        private bool mInitialized;
        
        #endregion //Attributes

        #region Root members

        /// <summary>
        /// Creates a new Root object. Private because of singleton pattern.
        /// </summary>
        public Root()
        {
            //Create the dictionaries used to store references to the content managers and plugins.
            mCustomContentManagers = new Dictionary<string, ContentManager>();
            mPlugins = new Dictionary<string, IPlugin>();
            mUpdateablePlugins = new List<IUpdateable>();
            mDrawablePlugins = new List<IDrawable>();

            mInitialized = false;
        }

        /// <summary>
        /// Initializes the root object.
        /// </summary>
        /// <param name="serviceProvider">
        /// IServiceProvider used to locate the IGraphicsDeviceService implementation.
        /// </param>
        public void Initialize(IServiceProvider serviceProvider)
        {
            try
            {
                //Save the service provider.
                Services = serviceProvider;

                //Get the graphics device from the service provider.
                IGraphicsDeviceService service = Services.GetService(typeof(IGraphicsDeviceService)) as IGraphicsDeviceService;
                if (service == null)
                {
                    string msg = "could not locate the IGraphicsDeviceService implementation.";
                    throw new NullReferenceException(msg);
                }

                //Get the graphics device from the graphics device service.
                mGraphicsDevice = service.GraphicsDevice;

                //Create the default content manager.
                mDefaultContentManager = new ContentManager(serviceProvider, "Content");

                //Create all the other core plug-ins.
                mSceneManager = new DefaultSceneManager(this, "sceneManager");
                mPostProcessManager = new DefaultPostProcessManager(this, "postProcessManager");
                mRenderer = new DeferredRenderer(this, "deferred_renderer");
                mPhysicsManager = new JigLibXPhysicsManager(this, "physicsManager");
                mEntityManager = new DefaultEntityManager(this, "entityManager");
                mAnimationManager = new DefaultAnimationManager(this, "animationManager");
                mGuiManager = new DefaultGuiManager(this, "guiManager");

                //Update the physics before updating the scene so that the physics objects
                //(which are nodes) can update their positions based on the physics world.
                mPhysicsManager.UpdateOrder = 0;
                mSceneManager.UpdateOrder = 1;

                mRenderer.Layer = 0;

                //Add all the core plug-ins to the plug-ins dictionary.
                AddPlugin(mSceneManager);
                AddPlugin(mPostProcessManager);
                AddPlugin(mRenderer);
                AddPlugin(mPhysicsManager);
                AddPlugin(mEntityManager);
                AddPlugin(mAnimationManager);
                AddPlugin(mGuiManager);

                //Initialize all the plugins.
                foreach(KeyValuePair<string, IPlugin> kvp in mPlugins)
                {
                    kvp.Value.Initialize();
                }

                mInitialized = true;
            }
            catch (Exception e)
            {
                throw new Exception("Root.Initialize(): " + e.Message);
            }
        }

        /// <summary>
        /// Destroys all the plugins the makes sure the root is ready to be garbage collected.
        /// </summary>
        public void Destroy()
        {
            // Destroy all the plugins that are installed the engine.
            Dictionary<string, IPlugin> plugins = new Dictionary<string, IPlugin>(mPlugins);
            foreach (IPlugin plugin in plugins.Values)
            {
                UninstallPlugin(plugin);
            }

            mInitialized = false;
        }

        /// <summary>
        /// Installs the specified plug-in in the engine.
        /// </summary>
        /// <param name="plugin">Plug-in which is to be installed.</param>
        public void InstallPlugin(IPlugin plugin)
        {
            if (plugin == null)
            {
                throw new ArgumentNullException("Root.InstallPlugin(): null plugin.");
            }

            AddPlugin(plugin);

            // If the engine has already been initialized, we have to initialize the plug-in as
            // well. If not, it will be initialize when the engine is initialized.
            if (mInitialized)
            {
                plugin.Initialize();
            }
        }

        /// <summary>
        /// Uninstalls the specified plug-in from the engine.
        /// </summary>
        /// <param name="plugin">Plug-in which is to be uninstalled.</param>
        public void UninstallPlugin(IPlugin plugin)
        {
            if (plugin == null)
            {
                throw new ArgumentNullException("Root.UninstallPlugin(): null plugin.");
            }

            plugin.Destroy();
            RemovePlugin(plugin.Name);
        }

        /// <summary>
        /// Creates a new custom content manager.
        /// </summary>
        /// <param name="name">Name of the new content manager.</param>
        /// <param name="folder">Folder where the content manager should look for content.</param>
        public void CreateCustomContentManager(string name, string folder = "Content")
        {
            try
            {
                ContentManager manager = new ContentManager(Services, folder);
                mCustomContentManagers.Add(name, manager);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("Root.CreateContentManager(): duplicate name.");
            }
        }

        /// <summary>
        /// Gets the content manager identified by the parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The content manager identified by the parameter.</returns>
        public ContentManager GetCustomContentManager(string name)
        {
            ContentManager manager;
            //If the content manager was not found, return null.
            if (!mCustomContentManagers.TryGetValue(name, out manager))
            {
                return null;
            }
            //If the animator was found, return it.
            else
            {
                return manager;
            }
        }

        /// <summary>
        /// Removes the content manager passed as a parameter from the engine.
        /// </summary>
        /// <param name="manager">Content manager which is to be removed from the engine.</param>
        public void RemoveCustomContentManager(ContentManager manager)
        {
            foreach (KeyValuePair<string, ContentManager> kvp in mCustomContentManagers)
            {
                if (kvp.Value == manager)
                {
                    mCustomContentManagers.Remove(kvp.Key);
                    return;
                }
            }

            string msg = "Root.RemoveContentManager(): no such content manager.";
            throw new ArgumentException(msg);
        }

        /// <summary>
        /// Removes a content manager from the engine.
        /// </summary>
        /// <param name="name">Name of the content manager which is to be removed.</param>
        public void RemoveContentManager(string name)
        {
            //Throw an exception if no name was passed to the method.
            if (name == null || name.Length == 0)
            {
                string msg = "Root.RemoveContentManager: no name specified ";
                throw new ArgumentException(msg);
            }

            ContentManager manager;
            //Throw an exception if the animator could not be found.
            if (!mCustomContentManagers.TryGetValue(name, out manager))
            {
                string msg = "Root.RemoveContentManager: no such content manager.";
                throw new ArgumentException(msg);
            }
            //If the animator was found, remove it.
            else
            {
                mCustomContentManagers.Remove(name);
            }
        }

        /// <summary>
        /// Draws the scene defined in the scene manager, applies any post-processing effects that
        /// have been added to the post-processing effect manager and draws any required gui
        /// elements.
        /// </summary>
        public void Draw()
        {
            //Draws all the drawable plug-ins.
            foreach (IDrawable drawablePlugin in mDrawablePlugins)
            {
                if (drawablePlugin.Visible)
                {
                    drawablePlugin.Draw();
                }
            }
        }
        
        /// <summary>
        /// Updates the root object and the managers it manages.
        /// </summary>
        /// <param name="elapsedTime">Time elapsed since the last update.</param>
        public void Update(TimeSpan elapsedTime)
        {
            // Updates all the plug-ins that are updateable.
            foreach (IUpdateable updateable in mUpdateablePlugins)
            {
                if (updateable.Enabled)
                {
                    updateable.Update(elapsedTime);
                }
            }
        } 

        #endregion //Root public methods

        #region Properties

        /// <summary>
        /// Gets the animation manager.
        /// </summary>
        public IAnimationManager AnimationManager
        {
            get { return mAnimationManager; }
        }

        /// <summary>
        /// Gets the content manager.
        /// </summary>
        public ContentManager ContentManager
        {
            get { return mDefaultContentManager; }
        }

        /// <summary>
        /// Gets all the custom content managers.
        /// </summary>
        public Dictionary<string, ContentManager> CustomContentManagers
        {
            get { return new Dictionary<string, ContentManager>(mCustomContentManagers); }
        }

        /// <summary>
        /// Gets or sets the entity manager.
        /// </summary>
        public IEntityManager EntityManager
        {
            get { return mEntityManager; }
        }

        /// <summary>
        /// Gets the graphics device.
        /// </summary>
        public GraphicsDevice GraphicsDevice
        {
            get { return mGraphicsDevice; }
        }

        /// <summary>
        /// Gets the GUI manager.
        /// </summary>
        public IGuiManager GuiManager
        {
            get { return mGuiManager; }
        }

        /// <summary>
        /// Gets the post process manager.
        /// </summary>
        public IPostProcessManager PostProcessManager
        {
            get { return mPostProcessManager; }
        }

        /// <summary>
        /// Gets the physics manager.
        /// </summary>
        public IPhysicsManager PhysicsManager
        {
            get { return mPhysicsManager; }
        }

        /// <summary>
        /// Gets the renderer.
        /// </summary>
        public IRenderer Renderer
        {
            get { return mRenderer; }
        }

        /// <summary>
        /// Gets the scene manager.
        /// </summary>
        public ISceneManager SceneManager
        {
            get { return mSceneManager; }
        }

        /// <summary>
        /// Gets or sets the IServiceProvider used to locate the implementations of the services 
        /// needed by the engine.
        /// </summary>
        /// <value>
        /// The IServiceProvider used to locate the implementations of the services needed by the 
        /// engine.
        /// </value>
        public IServiceProvider Services
        {
            get { return mServices; }
            set { mServices = value; }
        }
        
        #endregion

        #region Private methods

        /// <summary>
        /// Adds a plug-in to the engine.
        /// </summary>
        /// <param name="plugin">The plug-in which is to be added.</param>
        private void AddPlugin(IPlugin plugin)
        {
            try
            {
                mPlugins.Add(plugin.Name, plugin);

                //If the plug-in is updateable, we add it to the list of updateable plug-ins.
                IUpdateable updateable = plugin as IUpdateable;
                if (updateable != null)
                {
                    mUpdateablePlugins.Add(updateable);
                    //Order the plug-ins according to their update order.
                    mUpdateablePlugins.Sort((a, b) => a.UpdateOrder.CompareTo(b.UpdateOrder));
                }

                //If the plug-in is drawable, we add it to the list of drawable plug-ins.
                IDrawable drawable = plugin as IDrawable;
                if (drawable != null)
                {
                    mDrawablePlugins.Add(drawable);
                    mDrawablePlugins.Sort((a, b) => a.Layer.CompareTo(b.Layer));
                }
            }
            catch (ArgumentException)
            {
                string msg = "Root.AddPlugin(): " + plugin.Name + " has already been added to ";
                msg += "XtremeEngine.";
                throw new Exception(msg);
            }
        }

        /// <summary>
        /// Removes a plug-in from the engine.
        /// </summary>
        /// <param name="name">Name of the plug-in which is to be removed.</param>
        private void RemovePlugin(string name)
        {
            if (!mPlugins.Remove(name))
            {
                throw new Exception("Root.RemovePlugin(): " + name + " could not be found.");
            }

            // Remove the plug-in from the updateable plug-ins list.
            int count = mUpdateablePlugins.Count;
            for (int i = 0; i < count; ++i)
            {
                IPlugin plugin = mUpdateablePlugins[i] as IPlugin;
                if (plugin.Name.Equals(name))
                {
                    mUpdateablePlugins.RemoveAt(i);
                    // Order the plug-ins according to their update order.
                    mUpdateablePlugins.Sort((a, b) => a.UpdateOrder.CompareTo(b.UpdateOrder));
                    // Break out of the loop because we already found the plugin.
                    break;
                }
            }

            // Remove the plug-in from the drawable plug-ins list.
            count = mDrawablePlugins.Count;
            for (int i = 0; i < count; ++i)
            {
                IPlugin plugin = mDrawablePlugins[i] as IPlugin;
                if (plugin.Name.Equals(name))
                {
                    mDrawablePlugins.RemoveAt(i);
                    // Order the plug-ins according to their draw order.
                    mDrawablePlugins.Sort((a, b) => a.Layer.CompareTo(b.Layer));
                    // Break out of the loop because we already found the plugin.
                    break;
                }
            }
        }

        #endregion
    }
}
