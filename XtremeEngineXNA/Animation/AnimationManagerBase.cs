using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XtremeEngineXNA.Animation
{
    /// <summary>
    /// Abstract class which implements common methods required by the IAnimationManager interface.
    /// </summary>
    public abstract class AnimationManagerBase : PluginBase, IAnimationManager
    {
        #region Attributes

        /// <summary>
        /// Whether the plugin is enabled (i.e. is updated).
        /// </summary>
        private bool mEnabled;

        /// <summary>
        /// Update order of the plugin.
        /// </summary>
        private int mUpdateOrder;

        #endregion

        #region Public methods

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="root">Root object to which the plugin belongs.</param>
        /// <param name="name">Name of the plugin.</param>
        public AnimationManagerBase(Root root, string name)
            : base(root, name)
        {
            mEnabled = true;
            mUpdateOrder = 0;
        }

        /// <summary>
        /// Plays all the animations.
        /// </summary>
        public abstract void PlayAll();

        /// <summary>
        /// Stops all the animations.
        /// </summary>
        public abstract void StopAll();

        /// <summary>
        /// Pauses all the animations.
        /// </summary>
        public abstract void PauseAll();

        /// <summary>
        /// Resumes all the paused animations.
        /// </summary>
        public abstract void ResumeAll();

        /// <summary>
        /// Restarts all the animations.
        /// </summary>
        public abstract void RestartAll();

        /// <summary>
        /// Adds a new animation to the manager.
        /// </summary>
        /// <param name="animation">Animation which is to be added to the manager.</param>
        public abstract void AddAnimation(IAnimation animation);

        /// <summary>
        /// Removes an animation from the manager.
        /// </summary>
        /// <param name="animation">Animation which is to be removed from the manager.</param>
        public abstract void RemoveAnimation(IAnimation animation);

        /// <summary>
        /// Removes all the animations from the manager.
        /// </summary>
        public abstract void RemoveAllAnimations();

        /// <summary>
        /// Gets whether the manager has the specified animation.
        /// </summary>
        /// <param name="animation">Animation which is to be looked for.</param>
        /// <returns>
        /// <c>true</c> if the manager has the specified animation. Otherwise, <c>false</c>.
        /// </returns>
        public abstract bool HasAnimation(IAnimation animation);

        #endregion

        #region Properties

        /// <summary>
        /// Gets a list with all the animations in the manager.
        /// </summary>
        public abstract List<IAnimation> Animations { get; }

        #endregion

        #region IUpdateable members

        /// <summary>
        /// Gets or sets whether the object is enabled or not.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled
        {
            get { return mEnabled; }
            set
            {
                if (value != mEnabled)
                {
                    mEnabled = value;
                    if (EnabledChanged != null)
                        EnabledChanged(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Updates the manager.
        /// </summary>
        /// <param name="elapsedTime"></param>
        public abstract void Update(TimeSpan elapsedTime);

        /// <summary>
        /// Returns the update order of the scene node.
        /// </summary>
        /// <value>The update order of the object.</value>
        public int UpdateOrder
        {
            get { return mUpdateOrder; }
            set
            {
                if (value != mUpdateOrder)
                {
                    mUpdateOrder = value;

                    if (UpdateOrderChanged != null)
                        UpdateOrderChanged(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Occurs when the Enabled property changed.
        /// </summary>
        public event EventHandler<EventArgs> EnabledChanged;

        /// <summary>
        /// Occurs when the UpdateOrder property changed.
        /// </summary>
        public event EventHandler<EventArgs> UpdateOrderChanged;

        #endregion
    }
}
