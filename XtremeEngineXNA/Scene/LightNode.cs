using System;
using Microsoft.Xna.Framework;

namespace XtremeEngineXNA.Scene
{
    /// <summary>
    /// Class which represents a Light. A LightNode is a special type of node 
    /// which represents a light. A light node can be rotated, moved, etc just
    /// like any other node and also provides methods which let the user set the
    /// properties and type of the light.
    /// </summary>
    public class LightNode : SceneNode
    {
        #region Attributes

        /// <summary>
        /// Type of the light.
        /// </summary>
        private LightType mLightType;

        /// <summary>
        /// Diffuse color of the light.
        /// </summary>
        private Color mDiffuse;

        /// <summary>
        /// Specular color of the light.
        /// </summary>
        private Color mSpecular;

        /// <summary>
        /// Intensity of the light.
        /// </summary>
        private float mIntensity;

        /// <summary>
        /// How far the light reaches.
        /// </summary>
        private float mRadius;

        /// <summary>
        /// Falloff factor for the spotlight.
        /// </summary>
        private float mSpotLightFalloff;

        /// <summary>
        /// Angle from the direction vector of the spot light to the edge of the
        /// inner spot light cone.
        /// </summary>
        private float mSpotLightInnerAngle;

        /// <summary>
        /// Angle from the direction vector of the spot light to the edge of the
        /// outer spot light cone.
        /// </summary>
        private float mSpotLightOutterAngle;

        /// <summary>
        /// Co-sine of the angle from the direction vector of the spot light to 
        /// the edge of the inner spot light cone.
        /// </summary>
        private float mSpotLightInnerAngleCos;

        /// <summary>
        /// Co-sine of the angle from the direction vector of the spot light to
        /// the edge of the outer spot light cone.
        /// </summary>
        private float mSpotLightOutterAngleCos;

        /// <summary>
        /// Whether the light is visible or not.
        /// </summary>
        private bool mOn;

        /// <summary>
        /// Whether objects illuminated by this light cast shadows.
        /// </summary>
        private bool mShadowEnabled;

        #endregion

        #region LightNode members

        /// <summary>
        /// Creates a new light node with default parameters.
        /// </summary>
        /// <param name="root">Root object to which the node belongs.</param>
        public LightNode(Root root) : base(root)
        {
            mLightType = LightType.LIGHT_POINT;
            mDiffuse = Color.White;
            mSpecular = Color.White;
            mIntensity = 1.0f;
            mRadius = 1.0f;
            mSpotLightFalloff = 1.0f;
            mSpotLightInnerAngle = 0.52f;
            mSpotLightInnerAngleCos = (float)Math.Cos(mSpotLightInnerAngle);
            mSpotLightOutterAngle = 0.7f;
            mSpotLightOutterAngleCos = (float)Math.Cos(mSpotLightOutterAngle);
            mOn = true;
            mShadowEnabled = false;
        }

        /// <summary>
        /// Creates a new light node.
        /// </summary>
        /// <param name="root">Root object to which the node belongs.</param>
        /// <param name="type">Type of the new light.</param>
        /// <param name="diffuse">Diffuse color of the new light.</param>
        /// <param name="specular">Specular color of the new light.</param>
        /// <param name="intensity">Intensity of the new light.</param>
        /// <param name="radius">How far the light reaches.</param>
        /// <param name="spotFalloff">Falloff factor for the spotlight.</param>
        /// <param name="spotInnerAngle">
        /// Angle between the spot light direction vector and the edge of the 
        /// inner spot light cone.
        /// </param>
        /// <param name="spotOutterAngle">
        /// Angle between the spot light direction vector and the edge of the 
        /// outer spot light cone.
        /// </param>
        /// <param name="shadowsEnabled">
        /// Whether objects illuminated by this light cast shadows.
        /// </param>
        public LightNode(Root root, LightType type, Color diffuse,
            Color specular, float intensity, float radius, float spotFalloff, float spotInnerAngle,
            float spotOutterAngle, bool shadowsEnabled)
            : base(root)
        {
            mLightType = type;
            mDiffuse = diffuse;
            mSpecular = specular;
            mIntensity = intensity;
            mRadius = radius;
            mSpotLightFalloff = spotFalloff;
            mSpotLightInnerAngle = spotInnerAngle;
            mSpotLightInnerAngleCos = (float)Math.Cos(mSpotLightInnerAngle);
            mSpotLightOutterAngle = spotOutterAngle;
            mSpotLightOutterAngleCos = (float)Math.Cos(mSpotLightOutterAngle);
            mOn = true;
            mShadowEnabled = shadowsEnabled;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets/Sets the type of the light.
        /// </summary>
        public LightType LightType
        {
            get { return mLightType; }
            set { mLightType = value; }
        }

        /// <summary>
        /// Gets/Sets the diffuse color of the light.
        /// </summary>
        public Color DiffuseColor
        {
            get { return mDiffuse; }
            set { mDiffuse = value; }
        }

        /// <summary>
        /// Gets/Sets the specular color of the light.
        /// </summary>
        public Color SpecularColor
        {
            get { return mSpecular; }
            set { mSpecular = value; }
        }

        /// <summary>
        /// Gets/Sets the intensity of the light.
        /// </summary>
        public float Intensity
        {
            get { return mIntensity; }
            set { mIntensity = value; }
        }

        /// <summary>
        /// Gets/Sets a value which indicates how far the light reaches.
        /// </summary>
        public float Radius
        {
            get { return mRadius; }
            set { mRadius = value; }
        }

        /// <summary>
        /// Gets/Sets the spotlight falloff factor.
        /// </summary>
        public float SpotLightFalloff
        {
            get { return mSpotLightFalloff; }
            set { mSpotLightFalloff = value; }
        }

        /// <summary>
        /// Gets/Sets the angle between the spot light direction vector and the 
        /// edge of the inner spot light cone.
        /// </summary>
        public float SpotLightInnerAngle
        {
            get { return mSpotLightInnerAngle; }
            set
            {
                mSpotLightInnerAngle = value;
                mSpotLightInnerAngleCos = (float)Math.Cos(mSpotLightInnerAngle);
            }
        }

        /// <summary>
        /// Gets the co-sine of the angle between the spot light direction 
        /// vector and the edge of the inner spot light cone.
        /// </summary>
        public float SpotLightInnerAngleCos
        {
            get { return mSpotLightInnerAngleCos; }
        }

        /// <summary>
        /// Gets/Sets the angle between the spot light direction vector and the 
        /// edge of the outer spot light cone.
        /// </summary>
        public float SpotLightOutterAngle
        {
            get { return mSpotLightOutterAngle; }
            set
            {
                mSpotLightOutterAngle = value;
                mSpotLightOutterAngleCos = (float)Math.Cos(mSpotLightOutterAngle);
            }
        }

        /// <summary>
        /// Gets/Sets the co-sine of the angle between the spot light direction 
        /// vector and the edge of the outer spot light cone.
        /// </summary>
        public float SpotLightOutterAngleCos
        {
            get { return mSpotLightOutterAngleCos; }
        }

        /// <summary>
        /// Gets/Sets the flag which indicates whether the light is on or not.
        /// </summary>
        public bool IsOn
        {
            get { return mOn; }
            set { mOn = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether objects illuminated by this light cast shadows.
        /// </summary>
        /// <value>
        /// <c>true</c> if objects illuminated by this light cast shadows; otherwise, <c>false</c>.
        /// </value>
        public bool ShadowsEnabled
        {
            get { return mShadowEnabled; }
            set { mShadowEnabled = value; }
        }

        #endregion
    }
}
