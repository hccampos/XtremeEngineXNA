﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XtremeEngineXNA.Graphics.EffectParameters;

namespace XtremeEngineXNA.Content.Materials
{
    /// <summary>
    /// Material which supports diffuse, specular and ambient colors as well as a shininess
    /// parameter.
    /// </summary>
    public class ColoredMaterial : Material
    {
        #region Attributes

        /// <summary>
        /// Effect parameter with the ambient color of the material.
        /// </summary>
        private ColorParameter mAmbientColorParam;

        /// <summary>
        /// Effect parameter with the diffuse color of the material.
        /// </summary>
        private ColorParameter mDiffuseColorParam;

        /// <summary>
        /// Effect parameter with the specular intensity of the material.
        /// </summary>
        private FloatParameter mSpecularIntensityParam;

        /// <summary>
        /// Effect parameter with the shininess of the material.
        /// </summary>
        private FloatParameter mShininessParam;

        /// <summary>
        /// Effect parameter with the value which indicates whether objects drawn with this
        /// material get shadowed or not.
        /// </summary>
        private FloatParameter mReceiveShadowsParam;

        /// <summary>
        /// Intensity of the ambient light for the material.
        /// </summary>
        private float mAmbientIntensity = 0.0f;

        /// <summary>
        /// Whether objects drawn with this material get shadowed or not.
        /// </summary>
        private bool mReceiveShadows = true;

        #endregion

        #region ColoredMaterial public members

        /// <summary>
        /// Initializes a new instance of the <see cref="ColoredMaterial"/> class.
        /// </summary>
        /// <param name="root">Root object to which the material belongs.</param>
        public ColoredMaterial(Root root) : base(root)
        {
            mAmbientColorParam = new ColorParameter(root, "ambientColor", Color.Black);
            mDiffuseColorParam = new ColorParameter(root, "diffuseColor", Color.Black);
            mSpecularIntensityParam = new FloatParameter(root, "specularIntensity", 0.0f);
            mShininessParam = new FloatParameter(root, "shininess", 1.0f);
            mReceiveShadowsParam = new FloatParameter(root, "receiveShadows", 1.0f);

            Setup();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColoredMaterial"/> class.
        /// </summary>
        /// <param name="root">Root object to which the material belongs.</param>
        /// <param name="diffuse">Diffuse color of the new material.</param>
        /// <param name="specularIntensity">Specular intensity of the new material.</param>
        /// <param name="shininess">Shininess of the new material.</param>
        /// <param name="ambient">Ambient color of the new material.</param>
        /// <param name="ambientIntensity">Intensity of the ambient color. Each component of the ambient color will be multiplied
        /// by this factor.</param>
        /// <param name="receiveShadows">if set to <c>true</c> [receive shadows].</param>
        public ColoredMaterial(Root root, Color diffuse, float specularIntensity, 
            float shininess, Color ambient, float ambientIntensity = 0.2f, 
            bool receiveShadows = true)
            : base(root)
        {
            mAmbientIntensity = ambientIntensity; 
            Color multipliedAmbient = Color.Multiply(ambient, ambientIntensity);
            mAmbientColorParam = new ColorParameter(Root, "ambientColor", multipliedAmbient);
            mDiffuseColorParam = new ColorParameter(Root, "diffuseColor", diffuse);
            mSpecularIntensityParam = new FloatParameter(Root, "specularIntensity", specularIntensity);
            mShininessParam = new FloatParameter(root, "shininess", shininess);
            mReceiveShadowsParam = new FloatParameter(root, "receiveShadows", BoolToFloat(receiveShadows));
            mReceiveShadows = receiveShadows;

            Setup();
        }

        #endregion

        #region Private/Protected members

        /// <summary>
        /// Adds the required effect parameters to the material and sets the technique and effect
        /// to be used when drawing objects with this material.
        /// </summary>
        private void Setup()
        {
            AddParameter(new EngineParameter(Root, "View", EngineParameterType.PARAM_VIEW_MATRIX));
            AddParameter(new EngineParameter(Root, "Projection", EngineParameterType.PARAM_PROJECTION_MATRIX));
            AddParameter(new EngineParameter(Root, "World", EngineParameterType.PARAM_WORLD_MATRIX));
            AddParameter(mAmbientColorParam);
            AddParameter(mDiffuseColorParam);
            AddParameter(mSpecularIntensityParam);
            AddParameter(mShininessParam);
            AddParameter(mReceiveShadowsParam);

            this.TechniqueName = "Render";
            this.Effect = Root.ContentManager.Load<Effect>("Effects/ColoredMaterial");
        }

        /// <summary>
        /// Converts a bool value to a float.
        /// </summary>
        /// <param name="value">Value to be converted.</param>
        /// <returns>1.0f if set to <c>true</c>; 0.0f otherwise.</returns>
        private float BoolToFloat(bool value)
        {
            if (value)
                return 1.0f;
            else
                return 0.0f;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the ambient color of the material.
        /// </summary>
        /// <value>The the ambient color of the material.</value>
        public Color AmbientColor
        {
            get { return mAmbientColorParam.Value; }
            set { mAmbientColorParam.Value = Color.Multiply(value, mAmbientIntensity); }
        }

        /// <summary>
        /// Gets or sets the intensity of ambient light for the material.
        /// </summary>
        /// <value>The intensity of the ambient light for the material.</value>
        public float AmbientIntensity
        {
            get { return mAmbientIntensity; }
            set
            {
                mAmbientIntensity = value;
                mAmbientColorParam.Value = Color.Multiply(mAmbientColorParam.Value, mAmbientIntensity);
            }
        }

        /// <summary>
        /// Gets or sets the diffuse color of the material.
        /// </summary>
        /// <value>The diffuse color of the material.</value>
        public Color DiffuseColor
        {
            get { return mDiffuseColorParam.Value; }
            set { mDiffuseColorParam.Value = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether objects drawn with this material get shadowed
        /// or not.
        /// </summary>
        /// <value>
        /// <c>true</c> if objects drawn with this material get shadowed; otherwise, <c>false</c>.
        /// </value>
        public bool ReceiveShadows
        {
            get { return mReceiveShadows; }
            set
            {
                mReceiveShadows = value;
                mReceiveShadowsParam.Value = BoolToFloat(value);
            }
        }

        /// <summary>
        /// Gets or sets the specular intensity of the material.
        /// </summary>
        /// <value>The specular intensity of the material.</value>
        public float SpecularColor
        {
            get { return mSpecularIntensityParam.Value; }
            set { mSpecularIntensityParam.Value = value; }
        }

        /// <summary>
        /// Gets or sets the shininess of the material.
        /// </summary>
        /// <value>The shininess of the material.</value>
        public float Shininess
        {
            get { return mShininessParam.Value; }
            set { mShininessParam.Value = value; }
        }

        #endregion
    }
}
