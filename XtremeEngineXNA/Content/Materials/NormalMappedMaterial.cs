using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XtremeEngineXNA.Graphics.EffectParameters;

namespace XtremeEngineXNA.Content.Materials
{
    /// <summary>
    /// Normal mapped material which supports diffuse, noraml and specular textures.
    /// </summary>
    public class NormalMappedMaterial : Material
    {
        #region Attributes

        /// <summary>
        /// Effect parameter with the ambient color of the material.
        /// </summary>
        private ColorParameter mAmbientColorParam;

        /// <summary>
        /// Effect parameter with the diffuse texture of the material.
        /// </summary>
        private TextureParameter mDiffuseTextureParam;

        /// <summary>
        /// Effect parameter with the normal texture of the material.
        /// </summary>
        private TextureParameter mNormalTextureParam;

        /// <summary>
        /// Effect parameter with the specular texture of the material.
        /// </summary>
        private TextureParameter mSpecularTextureParam;

        /// <summary>
        /// Effect parameter which controls bumpiness of the material.
        /// </summary>
        private FloatParameter mBumpinessParam;

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

        #region NormalMappedMaterial public members

        /// <summary>
        /// Creates a new material with no textures. Textures must be set before rendering any
        /// object with the material.
        /// </summary>
        /// <param name="root">Root object to which the material belongs.</param>
        public NormalMappedMaterial(Root root) : base(root)
        {
            mAmbientColorParam = new ColorParameter(Root, "ambientColor", Color.Black);
            mDiffuseTextureParam = new TextureParameter(Root, "txDiffuse", null);
            mNormalTextureParam = new TextureParameter(Root, "txNormal", null);
            mSpecularTextureParam = new TextureParameter(Root, "txSpecular", null);
            mBumpinessParam = new FloatParameter(Root, "bumpiness", 1.0f);
            mReceiveShadowsParam = new FloatParameter(root, "receiveShadows", 1.0f);

            Setup();          
        }

        /// <summary>
        /// Creates a new material with all the textures set.
        /// </summary>
        /// <param name="root">Root object to which the material belongs.</param>
        /// <param name="diffuse">Diffuse texture of the new material.</param>
        /// <param name="normal">Normal map of the new material.</param>
        /// <param name="specular">Specular map of the new material.</param>
        /// <param name="ambient">Ambient color of the new material.</param>
        /// <param name="ambientIntensity">Intensity of the ambient light.</param>
        /// <param name="bumpiness">Bumpiness of the material.</param>
        /// <param name="receiveShadows">
        /// Whether objects drawn with this material receive shadows.
        /// </param>
        public NormalMappedMaterial(Root root, Texture2D diffuse, Texture2D normal,
            Texture2D specular, Color ambient, float ambientIntensity = 0.2f, 
            float bumpiness = 1.0f, bool receiveShadows = true)
            : base(root)
        {
            mAmbientIntensity = ambientIntensity;
            Color multipliedAmbient = Color.Multiply(ambient, ambientIntensity);
            mAmbientColorParam = new ColorParameter(Root, "ambientColor", multipliedAmbient);
            mDiffuseTextureParam = new TextureParameter(Root, "txDiffuse", diffuse);
            mNormalTextureParam = new TextureParameter(Root, "txNormal", normal);
            mSpecularTextureParam = new TextureParameter(Root, "txSpecular", specular);
            mBumpinessParam = new FloatParameter(Root, "bumpiness", bumpiness);
            mReceiveShadowsParam = new FloatParameter(root, "receiveShadows", BoolToFloat(receiveShadows));
            mReceiveShadows = receiveShadows;

            Setup();
        }

        /// <summary>
        /// Creates a new material by loading the required textures.
        /// </summary>
        /// <param name="root">Root object to which the material belongs.</param>
        /// <param name="diffuseFile">File with the diffuse texture of the material.</param>
        /// <param name="normalFile">File with the normal map of the material.</param>
        /// <param name="specularFile">File with the specular map of the material.</param>
        /// <param name="ambient">Ambient color of the material.</param>
        /// <param name="ambientIntensity">Intensity of the ambient light.</param>
        /// /// <param name="bumpiness">Bumpiness of the material.</param>
        public NormalMappedMaterial(Root root, string diffuseFile, string normalFile,
            string specularFile, Color ambient, float ambientIntensity = 0.2f, 
            float bumpiness = 1.0f)
            : base(root)
        {
            mAmbientIntensity = ambientIntensity;
            Color multipliedAmbient = Color.Multiply(ambient, ambientIntensity);
            mAmbientColorParam = new ColorParameter(Root, "ambientColor", multipliedAmbient);

            //Load the textures of the material.
            Texture2D diffuse = Root.ContentManager.Load<Texture2D>(diffuseFile);
            Texture2D normal = Root.ContentManager.Load<Texture2D>(normalFile);
            Texture2D specular = Root.ContentManager.Load<Texture2D>(specularFile);

            mDiffuseTextureParam = new TextureParameter(Root, "txDiffuse", diffuse);
            mNormalTextureParam = new TextureParameter(Root, "txNormal", normal);
            mSpecularTextureParam = new TextureParameter(Root, "txSpecular", specular);
            mBumpinessParam = new FloatParameter(Root, "bumpiness", bumpiness);

            Setup();
        }

        /// <summary>
        /// Loads a texture and sets it as the Material's diffuse texture.
        /// </summary>
        /// <param name="filename">File with the new diffuse texture of the material.</param>
        public void SetDiffuseTexture(string filename)
        {
            mDiffuseTextureParam.Value = Root.ContentManager.Load<Texture2D>(filename);
        }

        /// <summary>
        /// Loads a texture and sets it as the Material's normal texture.
        /// </summary>
        /// <param name="filename">File with the new normal texture of the material.</param>
        public void SetNormalTexture(string filename)
        {
            mNormalTextureParam.Value = Root.ContentManager.Load<Texture2D>(filename);
        }

        /// <summary>
        /// Loads a texture and sets it as the Material's specular texture.
        /// </summary>
        /// <param name="filename">File with the new specular texture of the material.</param>
        public void SetSpecularTexture(string filename)
        {
            mSpecularTextureParam.Value = Root.ContentManager.Load<Texture2D>(filename);
        }

        #endregion

        #region NormalMappedMaterial private members

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
            AddParameter(mDiffuseTextureParam);
            AddParameter(mNormalTextureParam);
            AddParameter(mSpecularTextureParam);
            AddParameter(mBumpinessParam);

            this.TechniqueName = "Render";
            this.Effect = Root.ContentManager.Load<Effect>("Effects/NormalMapped");
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
        /// Gets or sets the bumpiness of the material.
        /// </summary>
        /// <value>The bumpiness of the material.</value>
        public float Bumpiness
        {
            get { return mBumpinessParam.Value; }
            set { mBumpinessParam.Value = value; }
        }

        /// <summary>
        /// Gets or sets the diffuse texture of the material.
        /// </summary>
        /// <value>The diffuse texture of the material.</value>
        public Texture2D DiffuseTexture
        {
            get { return (Texture2D)mDiffuseTextureParam.Value; }
            set { mDiffuseTextureParam.Value = value; }
        }

        /// <summary>
        /// Gets or sets the normal texture of the material.
        /// </summary>
        /// <value>The normal texture of the material.</value>
        public Texture2D NormalTexture
        {
            get { return (Texture2D)mNormalTextureParam.Value; }
            set { mNormalTextureParam.Value = value; }
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
        /// Gets or sets the specular texture of the material.
        /// </summary>
        /// <value>The specular texture of the material.</value>
        public Texture2D SpecularTexture
        {
            get { return (Texture2D)mSpecularTextureParam.Value; }
            set { mSpecularTextureParam.Value = value; }
        }

        #endregion
    }
}
