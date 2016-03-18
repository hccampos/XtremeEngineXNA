using System.Collections.Generic;
using System;

namespace XtremeEngineXNA.Content
{
    /// <summary>
    /// Class which represents a post-processing effect chain. An effect chain consists of a group 
    /// of one or more effects in which the output of one effect is the input of the next effect in
    /// the chain.
    /// </summary>
    public class PostProcessEffectChain : Base
    {
        #region Attributes

        /// <summary>
        /// List with all the effects in the post-processing effect chain.
        /// </summary>
        private List<PostProcessEffect> mEffects;

        /// <summary>
        /// Whether the post-processing effect chain is enabled or not.
        /// </summary>
        private bool mIsEnabled;

        #endregion

        #region PostProcessEffect Membersendregion

        /// <summary>
        /// Creates a new post-processing effect chain with no effects. We must add effects to the
        /// chain before it can be used.
        /// </summary>
        /// <param name="root">Root object to which the effect chain belongs.</param>
        public PostProcessEffectChain(Root root) : base(root)
        {
            mEffects = new List<PostProcessEffect>();
            mIsEnabled = true;
        }

        /// <summary>
        /// Creates a new post-processing effect chain from a list of post-processing effects.
        /// </summary>
        /// <param name="root">Root object to which the effect chain belongs.</param>
        /// <param name="effects">
        /// List of post-processing effect parts that make up the new post-processing effect.
        /// </param>
        public PostProcessEffectChain(Root root, List<PostProcessEffect> effects) : base(root)
        {
            mEffects = new List<PostProcessEffect>(effects);
            mIsEnabled = true;
        }

        /// <summary>
        /// Adds an effect to the end of the post-processing effect chain.
        /// </summary>
        /// <param name="effect">Effect which is to be added.</param>
        public void AddEffect(PostProcessEffect effect)
        {
            if (effect == null)
            {
                throw new Exception("PostProcessEffectChain.AddEffect(): null effect.");
            }

            mEffects.Add(effect);
        }

        /// <summary>
        /// Inserts an effect into the post-processing effect chain.
        /// </summary>
        /// <param name="effect">Effect which is to be inserted.</param>
        /// <param name="pos">Position into which the effect is to be inserted.</param>
        public void InsertEffect(PostProcessEffect effect, int pos)
        {
            if (effect == null)
            {
                throw new Exception("PostProcessEffectChain.InsertEffect(): null effect");
            }

            if (pos < 0 || pos >= mEffects.Count)
            {
                throw new Exception("PostProcessEffectChain.InsertEffect(): invalid index");
            }

            mEffects.Insert(pos, effect);
        }

        /// <summary>
        /// Removes the last effect in the post-processing effect chain.
        /// </summary>
        public void RemoveEffect()
        {
            if (mEffects.Count > 0)
            {
                mEffects.RemoveAt(mEffects.Count - 1);
            }
        }

        /// <summary>
        /// Removes an effect from the post-processing effect chain.
        /// </summary>
        /// <param name="pos">Position of the effect which is to be removed.</param>
        public void RemoveEffect(int pos)
        {
            if (pos < 0 || pos >= mEffects.Count)
            {
                throw new Exception("PostProcessEffectChain.RemoveEffect(): invalid index");
            }

            mEffects.RemoveAt(pos);
        }

        /// <summary>
        /// Removes an effect from the post-processing effect chain.
        /// </summary>
        /// <param name="effect">Effect which is to be removed.</param>
        public void RemoveEffect(PostProcessEffect effect)
        {
            if (!mEffects.Remove(effect))
            {
                throw new Exception("PostProcessEffectChain.RemoveEffect(): effect not found.");
            }
        }

        /// <summary>
        /// Removes all the effects from the post-processing effect chain.
        /// </summary>
        public void RemoveAllEffects()
        {
            mEffects.Clear();
        }

        /// <summary>
        /// Enables this post-processing effect chain.
        /// </summary>
        public void Enable()
        {
            if (mIsEnabled != true)
            {
                mIsEnabled = true;
                OnChainChanged(this, true);
            }
        }

        /// <summary>
        /// Disables this post-processing effect chain.
        /// </summary>
        public void Disable()
        {
            if (mIsEnabled != false)
            {
                mIsEnabled = false;
                OnChainChanged(this, false);
            }
        }

        /// <summary>
        /// Returns an effect from the post-processing effect chain.
        /// </summary>
        /// <param name="pos">Position of the effect which is to be returned.</param>
        /// <returns>The effect in the position pos of the effect chain.</returns>
        public PostProcessEffect GetEffect(int pos)
        {
            return mEffects[pos];
        }

        /// <summary>
        /// Delegate type for the ChainChanged events.
        /// </summary>
        /// <param name="sender">Chain which triggered the event.</param>
        /// <param name="isEnabled">Whether the chain is enabled or disabled.</param>
        public delegate void ChainChangedHandler(PostProcessEffectChain sender, bool isEnabled);

        /// <summary>
        /// Event which is triggered when the effect chain is enabled.
        /// </summary>
        public event ChainChangedHandler ChainChanged;

        /// <summary>
        /// Invokes the ChainChanged event.
        /// </summary>
        /// <param name="sender">Chain which triggered the event.</param>
        /// <param name="isEnabled">Whether the chain is enabled or disabled.</param>
        public void OnChainChanged(PostProcessEffectChain sender, bool isEnabled)
        {
            if (ChainChanged != null)
            {
                ChainChanged(sender, isEnabled);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets whether the post-processing effect chain is enabled or not.
        /// </summary>
        public bool Enabled
        {
            get { return mIsEnabled; }
            set
            {
                if (mIsEnabled != value)
                {
                    mIsEnabled = value;
                    OnChainChanged(this, value);
                }
            }
        }

        /// <summary>
        /// Returns the number of effects in the post-processing effect chain.
        /// </summary>
        public int NumEffects
        {
            get { return mEffects.Count; }
        }

        /// <summary>
        /// Returns a list with all the effects that make up the post-processing effect chain.
        /// </summary>
        public List<PostProcessEffect> EffectsList
        {
            get { return mEffects; }
        }

        #endregion
    }
}