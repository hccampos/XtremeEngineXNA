using System;


namespace XtremeEngineXNA.Scene
{
    /// <summary>
    /// Interface which specifies the methods that an animator must implement. An animator is
    /// responsible for animating a scene node throughout time. For instance we can have an
    /// animator which rotates a scene node at a certain speed.
    /// </summary>
    public interface IAnimator
    {
        /// <summary>
        /// Tells the animator to update a certain node.
        /// </summary>
        /// <param name="elapsedTime">Time that has passed since the last update.</param>
        /// <param name="node">Node which is to be animated.</param>
        void Update(TimeSpan elapsedTime, SceneNode node);
    }
}
