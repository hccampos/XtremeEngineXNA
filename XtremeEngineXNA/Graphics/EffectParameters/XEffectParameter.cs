using System;
using Microsoft.Xna.Framework.Graphics;
using XtremeEngineXNA.Scene;

namespace XtremeEngineXNA.Graphics.EffectParameters
{
    /// <summary>
    /// Generic class which represents an effect parameter which stores a value of a certain type.
    /// An effect parameter is used to hold a certain value to be set on an effect. Each Material 
    /// has a collection of effect parameters and before drawing an object, all of the effect 
    /// parameters in the Material are set on the effect used by the Material (the Material calls
    /// <c>setOnEffect()</c> for each shader).
    /// </summary>
    /// <typeparam name="T">Type of the value held by the effect parameter.</typeparam>
    public abstract class XEffectParameter<T> : Base, IEffectParameter
    {
        #region Attributes

        /// <summary>
        /// Name of the effect parameter.
        /// </summary>
        private string mName;

        /// <summary>
        /// XNA effect parameter.
        /// </summary>
        private EffectParameter mParameter;

        /// <summary>
        /// Effect to which this parameter belongs.
        /// </summary>
        private Effect mEffect;

        /// <summary>
        /// Value held by the effect parameter.
        /// </summary>
        private T mValue;

        #endregion Attributes

        #region EffectParameter Members

        /// <summary>
        /// Creates a new EffectParameter.
        /// </summary>
        /// <param name="root">Root object to which the parameter belongs.</param>
        /// <param name="name">Name of the effect parameter.</param>
        /// <param name="value">Value of the effect parameter.</param>
        public XEffectParameter(Root root, string name, T value) : base(root)
        {
            try
            {
                if (name == null || name.Length == 0)
                {
                    throw new ArgumentNullException("the effect parameter must have a name.");
                }

                //Save the name of the parameter.
                mName = name;
                //Save the value of the parameter.
                mValue = value;
                //Save the XNA effect of the parameter.
                mEffect = null;
            }
            catch (Exception e)
            {
                throw new Exception("XEffectParameter.XEffectParameter(): " + e.Message);
            }
        }

        /// <summary>
        /// Creates a new EffectParameter.
        /// </summary>
        /// <param name="root">Root object to which the parameter belongs.</param>
        /// <param name="name">Name of the effect parameter.</param>
        /// <param name="value">Value of the effect parameter.</param>
        /// <param name="effect">Effect to which the parameter belongs.</param>
        public XEffectParameter(Root root, string name, T value, Effect effect): base(root)
        {
            try
            {
                if (name == null || name.Length == 0)
                {
                    throw new ArgumentNullException("the effect parameter must have a name.");
                }

                //Save the name of the parameter.
                mName = name;
                //Save the value of the parameter.
                mValue = value;
                //Save the XNA effect of the parameter.
                mEffect = effect;
                //Try to retrieve the XNA effect parameter from it.
                RetrieveParameter();
            }
            catch (Exception e)
            {
                throw new Exception("XEffectParameter.XEffectParameter(): " + e.Message);
            }
        }

        /// <summary>
        /// Sets the value of the parameter on its effect.
        /// </summary>
        /// <param name="node">Node from which information can be retrieved.</param>
        public abstract void SetOnEffect(SceneNode node);

        /// <summary>
        /// Gets/Sets the effect to which this parameter belongs.
        /// </summary>
        /// <value>The effect to which this parameter belongs</value>
        public Effect Effect
        {
            get { return mEffect; }
            set
            {
                mEffect = value;
                RetrieveParameter();
            }
        }

        /// <summary>
        /// Gets the name of the effect parameter.
        /// </summary>
        public string Name
        {
            get { return mName; }
        }

        /// <summary>
        /// Gets the XNA effect parameter.
        /// </summary>
        public EffectParameter Parameter
        {
            get { return mParameter; }
        }

        /// <summary>
        /// Gets/Sets the value of the effect parameter.
        /// </summary>
        public T Value
        {
            get { return mValue; }
            set { mValue = value; }
        }

        #endregion

        #region XEffectParameter private members

        /// <summary>
        /// Tries to find the XNA effect parameter in the effect.
        /// </summary>
        private void RetrieveParameter()
        {
            if (mEffect != null)
            {
                //Get the XNA EffectParameter from the effect.
                mParameter = mEffect.Parameters[mName];

                //If the parameter could not be found in the effect we throw an exception.
                if (mParameter == null)
                {
                    throw new Exception(mName + " could not be found in " + mEffect.Name);
                }
            }
            else
            {
                mParameter = null;
            }
        }

        #endregion
    }
}