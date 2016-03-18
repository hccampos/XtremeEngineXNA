using System.Collections.Generic;
using XtremeEngineXNA.Content;

namespace XtremeEngineXNA.Graphics
{
    /// <summary>
    /// Abstract class which implements the common methods required by the IPostProcessManager 
    /// interface.
    /// </summary>
    public abstract class PostProcessManagerBase: PluginBase, IPostProcessManager
    {
        #region PostProcessManager Members

        /// <summary>
        /// Initializes a new post process manager.
        /// </summary>
        /// <param name="root"></param>
        /// <param name="name"></param>
        public PostProcessManagerBase(Root root, string name) : base(root, name) { }

        /// <summary>
		/// Adds a post-process effect chain to the end of the manager's effect chain list.
		/// </summary>
		/// <param name="chain">Effect chain which is to be added.</param>
		public abstract void AddEffectChain(PostProcessEffectChain chain);

		/// <summary>
		/// Inserts a post-process effect chain into the manager's effect chain list.
		/// </summary>
		/// <param name="chain">Effect chain which is to be inserted.</param>
		/// <param name="pos">Position into which the effect chain is to be inserted.</param>
        public abstract void InsertEffectChain(PostProcessEffectChain chain, int pos);

		/// <summary>
		/// Removes the last post-process effect chain in the manager's effect chain list.
		/// </summary>
        public abstract void RemoveEffectChain();

		/// <summary>
		/// Removes a post-process effect chain from the manager's effect chain list.
		/// </summary>
		/// <param name="pos">
		/// Position of the effect chain which is to be removed in the manager's effect chain list.
		/// </param>
        public abstract void RemoveEffectChain(int pos);

		/// <summary>
		/// Removes a post-process effect chain from the manager's effect chain list.
		/// </summary>
		/// <param name="chain">
		/// Effect chain which is to be removed from the manager's effect chain list.
		/// </param>
        public abstract void RemoveEffectChain(PostProcessEffectChain chain);

		/// <summary>
        /// Removes all the post-process effect chains from the manager.
        /// </summary>
        public abstract void RemoveAllEffectChains();

        /// <summary>
        /// Returns a post-process effect chain from the manager.
        /// </summary>
        /// <param name="pos">Position of the effect chain which is to be returned.</param>
        /// <returns>
        /// The effect chain in the position pos in the manager's effect chain list.
        /// </returns>
        public abstract PostProcessEffectChain GetEffectChain(int pos);

        /// <summary>
        /// Enables a post-process effect chain from the manager.
        /// </summary>
        /// <param name="pos">Position of the effect chain which is to be enabled.</param>
        public abstract void EnableEffectChain(int pos);

        /// <summary>
        /// Disables a post-process effect chain from the manager.
        /// </summary>
        /// <param name="pos">Position of the effect chain which is to be disabled.</param>
        public abstract void DisableEffectChain(int pos);

        /// <summary>
        /// Returns whether a post-processing effect chain is enabled or not.
        /// </summary>
        /// <param name="pos">Position of the effect chain which is to be tested.</param>
        /// <returns>
        /// <c>true</c> if the specified effect chain is enabled and <c>false</c> otherwise.
        /// </returns>
        public abstract bool IsChainEnabled(int pos);

        #endregion

        #region Properties

        /// <summary>
        /// Returns the number of post-process effect chains in the manager.
        /// </summary>
        public abstract int NumEffectChains
        {
            get;
        }

		/// <summary>
        /// Returns the number of post-process effects in the manager.
        /// </summary>
        public abstract int NumEffects
        {
            get;
        }

        /// <summary>
        /// Returns a list with all the post-processing effect chains in the manager.
        /// </summary>
        public abstract List<PostProcessEffectChain> ChainsList
        {
            get;
        }

		/// <summary>
        /// Returns a list with all the post-process effect chains that are enabled.
        /// </summary>
        public abstract List<PostProcessEffectChain> EnabledChainsList
        {
            get;
        }

        #endregion
    }
}
