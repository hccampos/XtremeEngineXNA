using System;
using System.Collections.Generic;
using XtremeEngineXNA.Content;

namespace XtremeEngineXNA.Graphics
{
    /// <summary>
    /// Default post-processing effects manager.
    /// </summary>
    internal class DefaultPostProcessManager: PostProcessManagerBase
    {
        #region Attributes

        /// <summary>
        /// List of effect chains in the manager.
        /// </summary>
        private List<PostProcessEffectChain> mEffectChains;

        /// <summary>
        /// List of effect chains that are enabled.
        /// </summary>
        private List<PostProcessEffectChain> mEnabledEffectChains;

        /// <summary>
        /// Whether the list which stores the enabled effect chains has to be updated.
        /// </summary>
        private bool mNeedsUpdate;

        #endregion

        #region DefaultPostProcessManager members

        /// <summary>
        /// Creates a new default post-processing effects manager.
        /// </summary>
        /// <param name="root">Root object to which the new manager belongs.</param>
        /// <param name="name">Name of the new post-processing effects manager.</param>
        public DefaultPostProcessManager(Root root, string name) : base(root, name)
        {
        }

        /// <summary>
        /// Initializes the post-processing effects manager.
        /// </summary>
        public override void Initialize()
        {
            mEffectChains = new List<PostProcessEffectChain>();
            mEnabledEffectChains = new List<PostProcessEffectChain>();
            mNeedsUpdate = false;
        }

        /// <summary>
        /// Destroys the manager.
        /// </summary>
        public override void Destroy()
        {
            if (mEffectChains != null)
            {
                RemoveAllEffectChains();
                mEffectChains.Clear();

                mEnabledEffectChains = null;
            }

            if (mEnabledEffectChains != null)
            {
                mEnabledEffectChains.Clear();
                mEnabledEffectChains = null;
            }
        }

        /// <summary>
        /// Adds a post-process effect chain to the end of the manager's effect chain list.
        /// </summary>
        /// <param name="chain">Effect chain which is to be added.</param>
        public override void AddEffectChain(PostProcessEffectChain chain)
        {
            if (chain == null)
            {
                throw new Exception("DefaultPostProcessManager.AddEffectChain(): null chain.");
            }

            //Add the chain to the manager.
            mEffectChains.Add(chain);
            //Register the OnChainChange method on the chain's ChainChanged event to receive
            //notifications when the state of the chain is changed.
            chain.ChainChanged += new PostProcessEffectChain.ChainChangedHandler(OnChainChange);
            //We have a new chain so we need to update the cache list.
            mNeedsUpdate = true;
        }

        /// <summary>
        /// Inserts a post-process effect chain into the manager's effect chain list.
        /// </summary>
        /// <param name="chain">Effect chain which is to be inserted.</param>
        /// <param name="pos">Position into which the effect chain is to be inserted.</param>
        public override void InsertEffectChain(PostProcessEffectChain chain, int pos)
        {
            if (chain == null)
            {
                throw new Exception("DefaultPostProcessManager.InsertEffectChain(): null chain.");
            }

            if (pos < 0 || pos >= mEffectChains.Count)
            {
                throw new Exception("DefaultPostProcessManager.InsertEffectChain(): invalid insert position.");
            }

            //Add the chain to the manager.
            mEffectChains.Insert(pos, chain);
            //Register the OnChainChange method on the chain's ChainChanged event to receive
            //notifications when the state of the chain is changed.
            chain.ChainChanged += new PostProcessEffectChain.ChainChangedHandler(OnChainChange);
            //We have a new chain so we need to update the cache list.
            mNeedsUpdate = true;
        }

        /// <summary>
        /// Removes the last post-process effect chain in the manager's effect chain list.
        /// </summary>
        public override void RemoveEffectChain()
        {
            int count = mEffectChains.Count;
            if (count > 0)
            {
                --count;
                mEffectChains[count].ChainChanged -= new PostProcessEffectChain.ChainChangedHandler(OnChainChange);
                mEffectChains.RemoveAt(count);
                //Signal that we need to update the cache list.
                mNeedsUpdate = true;
            }
        }

        /// <summary>
        /// Removes a post-process effect chain from the manager's effect chain list.
        /// </summary>
        /// <param name="pos">
        /// Position of the effect chain which is to be removed in the manager's effect chain list.
        /// </param>
        public override void RemoveEffectChain(int pos)
        {
            if (pos < 0 || pos >= mEffectChains.Count)
            {
                throw new Exception("DefaultPostProcessManager.RemoveEffectChain(): invalid remove position.");
            }

            mEffectChains[pos].ChainChanged -= new PostProcessEffectChain.ChainChangedHandler(OnChainChange);
            mEffectChains.RemoveAt(pos);
            //Signal that we need to update the cache list.
            mNeedsUpdate = true;
        }

        /// <summary>
        /// Removes a post-process effect chain from the manager's effect chain list.
        /// </summary>
        /// <param name="chain">
        /// Effect chain which is to be removed from the manager's effect chain list.
        /// </param>
        public override void RemoveEffectChain(PostProcessEffectChain chain)
        {
            if (chain == null)
            {
                throw new Exception("DefaultPostProcessManager.RemoveEffectChain(): null chain.");
            }

            if (mEffectChains.Remove(chain))
            {
                chain.ChainChanged -= new PostProcessEffectChain.ChainChangedHandler(OnChainChange);
                //Signal that we need to update the cache list.
                mNeedsUpdate = true;
            }
            else
            {
                throw new Exception("DefaultPostProcessManager.RemoveEffectChain(): chain not found.");
            }
        }

        /// <summary>
        /// Removes all the post-process effect chains from the manager.
        /// </summary>
        public override void RemoveAllEffectChains()
        {
            //Remove all the delegates from the chains.
            foreach (PostProcessEffectChain chain in mEffectChains)
            {
                chain.ChainChanged -= new PostProcessEffectChain.ChainChangedHandler(OnChainChange);
            }
            //Remove all the chains from the manager.
            mEffectChains.Clear();
            //Signal that we need to update the cache list.
            mNeedsUpdate = true;
        }

        /// <summary>
        /// Returns a post-process effect chain from the manager.
        /// </summary>
        /// <param name="pos">Position of the effect chain which is to be returned.</param>
        /// <returns>
        /// The effect chain in the position pos in the manager's effect chain list.
        /// </returns>
        public override PostProcessEffectChain GetEffectChain(int pos)
        {
            return mEffectChains[pos];
        }

        /// <summary>
        /// Enables a post-process effect chain from the manager.
        /// </summary>
        /// <param name="pos">Position of the effect chain which is to be enabled.</param>
        public override void EnableEffectChain(int pos)
        {
            GetEffectChain(pos).Enable();
            mNeedsUpdate = true;
        }

        /// <summary>
        /// Disables a post-process effect chain from the manager.
        /// </summary>
        /// <param name="pos">Position of the effect chain which is to be disabled.</param>
        public override void DisableEffectChain(int pos)
        {
            GetEffectChain(pos).Disable();
            mNeedsUpdate = true;
        }

        /// <summary>
        /// Returns whether a post-processing effect chain is enabled or not.
        /// </summary>
        /// <param name="pos">Position of the effect chain which is to be tested.</param>
        /// <returns>
        /// <c>true</c> if the specified effect chain is enabled and <c>false</c> otherwise.
        /// </returns>
        public override bool IsChainEnabled(int pos)
        {
            return GetEffectChain(pos).Enabled;
        }

        /// <summary>
        /// Called whenever one of the post-processing effect chains in the manager is enabled or
        /// disabled.
        /// </summary>
        /// <param name="chain">Chain which was enabled or disabled.</param>
        /// <param name="value">Whether the chain is now enabled or not.</param>
        public void OnChainChange(PostProcessEffectChain chain, bool value)
        {
            mNeedsUpdate = true;
        }

        /// <summary>
        /// Fills the mEnabledEffectChains list with all the effect chains that are enabled.
        /// </summary>
        private void UpdateCache()
        {
            //Clear the cache list.
            mEnabledEffectChains.Clear();
            //Add all the enabled effect chains to the list.
            foreach (PostProcessEffectChain chain in mEffectChains)
            {
                if (chain.Enabled)
                {
                    mEnabledEffectChains.Add(chain);
                }
            }
            //Set the flag to false because we don't need to update the cache (we just did!).
            mNeedsUpdate = false;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Returns the number of post-process effect chains in the manager.
        /// </summary>
        public override int NumEffectChains
        {
            get { return mEffectChains.Count; }
        }

        /// <summary>
        /// Returns the number of post-process effects in the manager.
        /// </summary>
        public override int NumEffects
        {
            get
            {
                int result = 0;
                foreach (PostProcessEffectChain chain in mEffectChains)
                {
                    result += chain.NumEffects;
                }
                return result;
            }
        }

        /// <summary>
        /// Returns a list with all the post-processing effect chains in the manager.
        /// </summary>
        public override List<PostProcessEffectChain> ChainsList
        {
            get { return mEffectChains;  }
        }

        /// <summary>
        /// Returns a list with all the post-process effect chains that are enabled.
        /// </summary>
        public override List<PostProcessEffectChain> EnabledChainsList
        {
            get
            {
                if (mNeedsUpdate)
                {
                    UpdateCache();
                }

                return mEnabledEffectChains;
            }
        }

        #endregion
    }
}
