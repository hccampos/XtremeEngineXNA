using System.Collections.Generic;
using XtremeEngineXNA.Content;

namespace XtremeEngineXNA.Graphics
{
    /// <summary>
    /// Interface which defines which methods a post-process manager must implement. A 
    /// post-processing manager is responsible for managing all the post-process effects which are 
    /// to be applied to a rendered scene. After the scene is rendered the rendered queries the
    /// post-process manager for the currently active effects and then applies them.
    /// </summary>
    public interface IPostProcessManager : IPlugin
    {
        /// <summary>
		/// Adds a post-process effect chain to the end of the manager's effect chain list.
		/// </summary>
		/// <param name="chain">Effect chain which is to be added.</param>
		void AddEffectChain(PostProcessEffectChain chain);

		/// <summary>
		/// Inserts a post-process effect chain into the manager's effect chain list.
		/// </summary>
		/// <param name="chain">Effect chain which is to be inserted.</param>
		/// <param name="pos">Position into which the effect chain is to be inserted.</param>
        void InsertEffectChain(PostProcessEffectChain chain, int pos);

		/// <summary>
		/// Removes the last post-process effect chain in the manager's effect chain list.
		/// </summary>
        void RemoveEffectChain();

		/// <summary>
		/// Removes a post-process effect chain from the manager's effect chain list.
		/// </summary>
		/// <param name="pos">
		/// Position of the effect chain which is to be removed in the manager's effect chain list.
		/// </param>
        void RemoveEffectChain(int pos);

		/// <summary>
		/// Removes a post-process effect chain from the manager's effect chain list.
		/// </summary>
		/// <param name="chain">
		/// Effect chain which is to be removed from the manager's effect chain list.
		/// </param>
        void RemoveEffectChain(PostProcessEffectChain chain);

		/// <summary>
        /// Removes all the post-process effect chains from the manager.
        /// </summary>
        void RemoveAllEffectChains();

        /// <summary>
        /// Returns a post-process effect chain from the manager.
        /// </summary>
        /// <param name="pos">Position of the effect chain which is to be returned.</param>
        /// <returns>
        /// The effect chain in the position pos in the manager's effect chain list.
        /// </returns>
        PostProcessEffectChain GetEffectChain(int pos);

        /// <summary>
        /// Enables a post-process effect chain from the manager.
        /// </summary>
        /// <param name="pos">Position of the effect chain which is to be enabled.</param>
        void EnableEffectChain(int pos);

        /// <summary>
        /// Disables a post-process effect chain from the manager.
        /// </summary>
        /// <param name="pos">Position of the effect chain which is to be disabled.</param>
        void DisableEffectChain(int pos);

        /// <summary>
        /// Returns whether a post-processing effect chain is enabled or not.
        /// </summary>
        /// <param name="pos">Position of the effect chain which is to be tested.</param>
        /// <returns>
        /// <c>true</c> if the specified effect chain is enabled and <c>false</c> otherwise.
        /// </returns>
        bool IsChainEnabled(int pos);

        /// <summary>
        /// Returns the number of post-process effect chains in the manager.
        /// </summary>
        int NumEffectChains { get; }

		/// <summary>
        /// Returns the number of post-process effects in the manager.
        /// </summary>
        int NumEffects { get; }

        /// <summary>
        /// Returns a list with all the post-processing effect chains in the manager.
        /// </summary>
        List<PostProcessEffectChain> ChainsList { get; }

		/// <summary>
        /// Returns a list with all the post-process effect chains that are enabled.
        /// </summary>
        List<PostProcessEffectChain> EnabledChainsList { get; }
    }
}
