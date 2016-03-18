using System;

namespace XtremeEngineXNA.Animation
{
    /// <summary>
    /// Interface which describes the methods that an animation must implement. An animation updates
    /// a numeric property on an object.
    /// </summary>
    public interface IAnimation
    {
        /// <summary>
        /// Updates the animation state.
        /// </summary>
        /// <param name="elapsedTime">Time elapsed since the last step.</param>
        void AnimationStep(TimeSpan elapsedTime);

        /// <summary>
        /// Plays the animation from the beginning if it is not playing.
        /// </summary>
        void Play();

        /// <summary>
        /// Stops the animation and goes back to the beginning.
        /// </summary>
        void Stop();

        /// <summary>
        /// Pauses the animation.
        /// </summary>
        void Pause();

        /// <summary>
        /// Resumes the animation if it is paused.
        /// </summary>
        void Resume();

        /// <summary>
        /// Stops the animation and starts playing it from the beginning.
        /// </summary>
        void Restart();

        /// <summary>
        /// Gets or sets the duration of the animation (in milliseconds).
        /// </summary>
        int Duration { get; set; }

        /// <summary>
        /// Gets the current position in the animation (0: beginning -> 1: end).
        /// </summary>
        double Position { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IAnimation"/> is playing.
        /// </summary>
        /// <value>
        ///   <c>true</c> if playing; otherwise, <c>false</c>.
        /// </value>
        bool Playing { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IAnimation"/> is paused.
        /// </summary>
        /// <value>
        ///   <c>true</c> if paused; otherwise, <c>false</c>.
        /// </value>
        bool Paused { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IAnimation"/> is complete.
        /// </summary>
        /// <value>
        ///   <c>true</c> if complete; otherwise, <c>false</c>.
        /// </value>
        bool Complete { get; }

        /// <summary>
        /// Gets or sets whether the animation is to be removed from the animation manager after it
        /// is completed.
        /// </summary>
        bool RemoveOnCompletion { get; set; }
    }
}
