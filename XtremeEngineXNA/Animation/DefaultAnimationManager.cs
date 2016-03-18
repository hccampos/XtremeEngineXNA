using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XtremeEngineXNA.Animation
{
    /// <summary>
    /// Default implementation of the IAnimationManager interface.
    /// </summary>
    public class DefaultAnimationManager : AnimationManagerBase
    {
        #region Attributes

        /// <summary>
        /// Animations managed by the manager.
        /// </summary>
        private List<IAnimation> mAnimations;

        #endregion

        #region Public methods

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="root">Root object to which the manager belongs.</param>
        /// <param name="name">Name of the manager.</param>
        public DefaultAnimationManager(Root root, string name)
            : base(root, name)
        {
            mAnimations = new List<IAnimation>();
        }

        /// <summary>
        /// Updates all the animations in the manager.
        /// </summary>
        /// <param name="elapsedTime"></param>
        public override void Update(TimeSpan elapsedTime)
        {
            // We have to create a copy of the list here because we are going to iterate over it
            // but we may need to remove elements from it. The copy will be used for iteration 
            // purposes while the original can be changed.
            List<IAnimation> animationsCopy = new List<IAnimation>(mAnimations);

            foreach (IAnimation a in animationsCopy)
            {
                a.AnimationStep(elapsedTime);

                if (a.Complete && a.RemoveOnCompletion)
                {
                    mAnimations.Remove(a);
                }
            }
        }

        /// <summary>
        /// Plays all the animations.
        /// </summary>
        public override void PlayAll()
        {
            foreach (IAnimation a in mAnimations)
            {
                a.Play();
            }
        }

        /// <summary>
        /// Stops all the animations.
        /// </summary>
        public override void StopAll()
        {
            foreach (IAnimation a in mAnimations)
            {
                a.Stop();
            }
        }

        /// <summary>
        /// Pauses all the animations.
        /// </summary>
        public override void PauseAll()
        {
            foreach (IAnimation a in mAnimations)
            {
                a.Pause();
            }
        }

        /// <summary>
        /// Resumes all the paused animations.
        /// </summary>
        public override void ResumeAll()
        {
            foreach (IAnimation a in mAnimations)
            {
                a.Resume();
            }
        }

        /// <summary>
        /// Restarts all the animations.
        /// </summary>
        public override void RestartAll()
        {
            foreach (IAnimation a in mAnimations)
            {
                a.Restart();
            }
        }

        /// <summary>
        /// Adds a new animation to the manager.
        /// </summary>
        /// <param name="animation">Animation which is to be added to the manager.</param>
        public override void AddAnimation(IAnimation animation)
        {
            if (animation == null)
            {
                throw new ArgumentNullException("DefaultAnimationManager.AddAnimation(): null animation.");
            }

            mAnimations.Add(animation);
        }

        /// <summary>
        /// Removes an animation from the manager.
        /// </summary>
        /// <param name="animation">Animation which is to be removed from the manager.</param>
        public override void RemoveAnimation(IAnimation animation)
        {
            if (animation == null)
            {
                throw new ArgumentNullException("DefaultAnimationManager.RemoveAnimation(): null animation.");
            }

            if (!mAnimations.Remove(animation))
            {
                throw new ArgumentException("DefaultAnimationManager.RemoveAnimation(): animation not found.");
            }
        }

        /// <summary>
        /// Removes all the animations from the manager.
        /// </summary>
        public override void RemoveAllAnimations()
        {
            mAnimations.Clear();
        }

        /// <summary>
        /// Gets whether the manager has the specified animation.
        /// </summary>
        /// <param name="animation">Animation which is to be looked for.</param>
        /// <returns>
        /// <c>true</c> if the manager has the specified animation. Otherwise, <c>false</c>.
        /// </returns>
        public override bool HasAnimation(IAnimation animation)
        {
            return mAnimations.Contains(animation);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a list with all the animations in the manager.
        /// </summary>
        public override List<IAnimation> Animations
        {
            get { return mAnimations; }
        }

        #endregion
    }
}
