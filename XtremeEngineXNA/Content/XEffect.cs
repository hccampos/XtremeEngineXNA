using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using XtremeEngineXNA.Graphics.EffectParameters;
using XtremeEngineXNA.Scene;

namespace XtremeEngineXNA.Content
{
    /// <summary>
    /// Class which represents an XtremeEngine effect. An XtremeEngine effect contains an XNA
    /// effect and a number of effect parameters which are automatically set on the XNA effect
    /// when we call Apply().
    /// </summary>
    public class XEffect : Base
    {
        #region Attributes

        /// <summary>
        /// XNA Effect used by the effect.
        /// </summary>
        private Effect mEffect;

        /// <summary>
        /// Name of the technique used by this effect.
        /// </summary>
        private string mTechniqueName;

        /// <summary>
        /// Effects parameters which are to be set when the effect is applied.
        /// </summary>
        private Dictionary<string, IEffectParameter> mParameters;

        /// <summary>
        /// BlendState used to draw object with the effect.
        /// </summary>
        private BlendState mBlendState;

        /// <summary>
        /// DepthStencilState used to draw object with the effect.
        /// </summary>
        private DepthStencilState mDepthStencilState;

        /// <summary>
        /// RasterizerState used to draw object with the effect.
        /// </summary>
        private RasterizerState mRasterizerState;

        #endregion

        #region XEffect public members

        /// <summary>
        /// Creates a new XEffect.
        /// </summary>
        /// <param name="root">Root object to which the effect belongs.</param>
        public XEffect(Root root) :
            base(root)
        {
            mEffect = null;
            mTechniqueName = "";
            mParameters = new Dictionary<string, IEffectParameter>();
            mBlendState = null;
            mDepthStencilState = null;
            mRasterizerState = null;
        }

        /// <summary>
        /// Creates a new XEffect.
        /// </summary>
        /// <param name="root">Root object to which the effect belongs.</param>
        /// <param name="effect">XNA Effect used by the effect.</param>
        /// <param name="technique">Name of the technique used by the effect.</param>
        /// <param name="parameters">
        /// List of effect parameters which are to be set when the effect is applied.
        /// </param>
        public XEffect(Root root, Effect effect, string technique, List<IEffectParameter> parameters) 
            : base(root)
        {
            try
            {
                mEffect = effect;
                mTechniqueName = technique;
                mParameters = new Dictionary<string, IEffectParameter>(parameters.Count);
                mBlendState = null;
                mDepthStencilState = null;
                mRasterizerState = null;

                //If the current technique could not be set we throw an exception because if the
                //user provides an effect to the constructor, he must also provide a valid
                //technique name.
                if (!SetCurrentTechnique())
                {
                    throw new Exception("invalid technique.");
                }

                //Add all the parameters to the parameters dictionary.
                foreach (IEffectParameter p in parameters)
                {
                    mParameters.Add(p.Name, p);
                }

                //Set the effect on all the parameters.
                SetEffectOnParameters();
            }
            catch (Exception e)
            {
                throw new Exception("XEffect.XEffect(): " + e.Message);
            }
        }

        /// <summary>
        /// Creates a new XEffect.
        /// </summary>
        /// <param name="root">Root object to which the effect belongs.</param>
        /// <param name="effect">XNA Effect used by the effect.</param>
        /// <param name="technique">Name of the technique used by the effect.</param>
        /// <param name="parameters">
        /// List of effect parameters which are to be set when the effect is applied.
        /// </param>
        /// <param name="blendState">BlendState used by the effect.</param>
        /// <param name="depthStencilState">DepthStencilState used by the effect.</param>
        /// <param name="rasterizerState">RasterizerState used by the effect.</param>
        public XEffect(Root root, Effect effect, string technique, 
            List<IEffectParameter> parameters, BlendState blendState, 
            DepthStencilState depthStencilState, RasterizerState rasterizerState) 
            : this(root, effect, technique, parameters)
        {
            mBlendState = blendState;
            mDepthStencilState = depthStencilState;
            mRasterizerState = rasterizerState;
        }

        /// <summary>
        /// Prepares the graphics device to draw objects with this effect.
        /// </summary>
        public void Apply()
        {
            Apply(null);
        }

        /// <summary>
        /// Prepares the graphics device to draw objects with this effect.
        /// </summary>
        /// <param name="node">Node from which some parameters may retrieve information.</param>
        public void Apply(SceneNode node)
        {
#if DEBUG
            try
            {
                // If the effect is null we throw an exception.
                if (mEffect == null)
                {
                    throw new InvalidOperationException("null effect.");
                }

                // If the effect has no current technique, we throw an exception.
                if (mEffect.CurrentTechnique == null)
                {
                    throw new InvalidOperationException("no technique is set.");
                }
#endif
                // Set the effect parameters on the effect.
                foreach (KeyValuePair<string, IEffectParameter> kvp in mParameters)
                {
                    kvp.Value.SetOnEffect(node);
                }

                // Apply the first pass of the current technique. Only 1 pass is supported.
                mEffect.CurrentTechnique.Passes[0].Apply();

                // Set the device states on the graphics device.
                SetStates();
#if DEBUG
            }
            catch (Exception e)
            {
                throw new Exception("XEffect.Apply(): " + e.Message);
            }
#endif
        }

        /// <summary>
        /// Adds an effect parameter to the effect.
        /// </summary>
        /// <param name="parameter">The parameter which is to be added to the effect.</param>
        public void AddParameter(IEffectParameter parameter)
        {
            mParameters.Add(parameter.Name, parameter);
            parameter.Effect = mEffect;
        }

        /// <summary>
        /// Removes a parameter from the effect.
        /// </summary>
        /// <param name="parameterName">Name of the parameter which is to be removed.</param>
        public void RemoveParameter(string parameterName)
        {
            mParameters.Remove(parameterName);
        }

        /// <summary>
        /// Sets the device states on the graphics device.
        /// </summary>
        public void SetStates()
        {
            if (mBlendState != null)
            {
                mEffect.GraphicsDevice.BlendState = mBlendState;
            }

            if (mDepthStencilState != null)
            {
                mEffect.GraphicsDevice.DepthStencilState = mDepthStencilState;
            }

            if (mRasterizerState != null)
            {
                mEffect.GraphicsDevice.RasterizerState = mRasterizerState;
            }
        }        

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the BlendState.
        /// </summary>
        /// <value>
        /// The BlendState used by the effect.
        /// </value>
        public BlendState BlendState
        {
            get { return mBlendState; }
            set { mBlendState = value; }
        }

        /// <summary>
        /// Gets or sets the DepthStencilState.
        /// </summary>
        /// <value>
        /// The DepthStencilState used by the effect.
        /// </value>
        public DepthStencilState DepthStencilState
        {
            get { return mDepthStencilState; }
            set { mDepthStencilState = value; }
        }

        /// <summary>
        /// Gets or sets the XNA effect used by the material.
        /// </summary>
        public Effect Effect
        {
            get { return mEffect; }
            set
            {
                if (value != mEffect)
                {
                    mEffect = value;
                    SetEffectOnParameters();
                    SetCurrentTechnique();
                }
            }
        }

        /// <summary>
        /// Gets/Sets all of the effect parameters in the effect as a list.
        /// </summary>
        public List<IEffectParameter> ParameterList
        {
            get
            {
                List<IEffectParameter> parameters = new List<IEffectParameter>(mParameters.Count);
                foreach (KeyValuePair<string, IEffectParameter> kvp in mParameters)
                {
                    parameters.Add(kvp.Value);
                }
                return parameters;
            }
            set
            {
                mParameters.Clear();
                //Add all the parameters to the parameters dictionary.
                foreach (IEffectParameter p in value)
                {
                    mParameters.Add(p.Name, p);
                }
                //Set the effect on all the parameters.
                SetEffectOnParameters();
            }
        }

        /// <summary>
        /// Gets/Sets all of the effect parameters in the effect as a dictionary.
        /// </summary>
        public Dictionary<string, IEffectParameter> ParameterDictionary
        {
            get { return mParameters; }
            set
            {
                //Create a new parameter dictionary which is a copy of the argument.
                mParameters = new Dictionary<string, IEffectParameter>(value);
                //Set the effect on all the parameters.
                SetEffectOnParameters();
            }
        }

        /// <summary>
        /// Gets or sets the RasterizerState.
        /// </summary>
        /// <value>
        /// The RasterizerState used by the effect.
        /// </value>
        public RasterizerState RasterizerState
        {
            get { return mRasterizerState; }
            set { mRasterizerState = value; }
        }

        /// <summary>
        /// Gets or sets the name of the technique that should be used by the effect.
        /// </summary>
        public string TechniqueName
        {
            get { return mTechniqueName; }
            set
            {
                if (!value.Equals(mTechniqueName))
                {
                    mTechniqueName = value;
                    SetCurrentTechnique();
                }
            }
        }     

        #endregion

        #region XEffect private members

        /// <summary>
        /// Sets the effect that is set on this XEffect on all the effect parameters it contains.
        /// </summary>
        private void SetEffectOnParameters()
        {
            foreach (KeyValuePair<string, IEffectParameter> kvp in mParameters)
            {
                kvp.Value.Effect = mEffect;
            }
        }

        /// <summary>
        /// Tries to find the technique defined in this XEffect and if the technique is found, sets
        /// it as the current technique in the effect.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the current technique was set and <c>false</c> otherwise.
        /// </returns>
        private bool SetCurrentTechnique()
        {
            if (mEffect != null)
            {
                //Try to get the technique from the effect.
                EffectTechnique newTechnique = mEffect.Techniques[mTechniqueName];

                //If the technique was found in the effect we set it as the current technique.
                if (newTechnique != null)
                {
                    mEffect.CurrentTechnique = newTechnique;
                    return true;
                }
                //If the technique could not be found we set the current technique to null.
                else
                {
                    mEffect.CurrentTechnique = null;
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        #endregion
    }
}
