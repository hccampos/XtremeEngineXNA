using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XtremeEngineXNA.Animation
{
    /// <summary>
    /// Interface which defines the methods that an animation manager must provide. The animation
    /// manager is responsible for managing all the animations. When an animation is added to the
    /// animation manager it is automatically updated on each update step if it is not paused or
    /// stopped. The animation manager also allows the user to stop, pause or resume all of the 
    /// animations it manages.
    /// </summary>
    public interface IAnimationManager : IPlugin, IUpdateable
    {
        /// <summary>
        /// Plays all the animations.
        /// </summary>
        void PlayAll();

        /// <summary>
        /// Stops all the animations.
        /// </summary>
        void StopAll();

        /// <summary>
        /// Pauses all the animations.
        /// </summary>
        void PauseAll();

        /// <summary>
        /// Resumes all the paused animations.
        /// </summary>
        void ResumeAll();

        /// <summary>
        /// Restarts all the animations.
        /// </summary>
        void RestartAll();

        /// <summary>
        /// Adds a new animation to the manager.
        /// </summary>
        /// <param name="animation">Animation which is to be added to the manager.</param>
        void AddAnimation(IAnimation animation);

        /// <summary>
        /// Removes an animation from the manager.
        /// </summary>
        /// <param name="animation">Animation which is to be removed from the manager.</param>
        void RemoveAnimation(IAnimation animation);

        /// <summary>
        /// Removes all the animations from the manager.
        /// </summary>
        void RemoveAllAnimations();

        /// <summary>
        /// Gets whether the manager has the specified animation.
        /// </summary>
        /// <param name="animation">Animation which is to be looked for.</param>
        /// <returns>
        /// <c>true</c> if the manager has the specified animation. Otherwise, <c>false</c>.
        /// </returns>
        bool HasAnimation(IAnimation animation);

        /// <summary>
        /// Gets a list with all the animations in the manager.
        /// </summary>
        List<IAnimation> Animations { get; }
    }
}
