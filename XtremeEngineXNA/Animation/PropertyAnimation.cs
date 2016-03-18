using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace XtremeEngineXNA.Animation
{
    /// <summary>
    /// Animation which animates a property of an object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PropertyAnimation<T> : IAnimation
    {
        #region Attributes

        /// <summary>
        /// Object which is animated by the animation.
        /// </summary>
        private object mTarget;

        /// <summary>
        /// Name of the property which is to be animated.
        /// </summary>
        private string mPropertyName;

        /// <summary>
        /// Initial value of the property which is animated.
        /// </summary>
        private T mInitialValue;

        /// <summary>
        /// Final value of the property which is animated.
        /// </summary>
        private T mFinalValue;

        /// <summary>
        /// Whether the animation is being played.
        /// </summary>
        private bool mPlaying;

        /// <summary>
        /// Whether the animation is paused.
        /// </summary>
        private bool mPaused;

        /// <summary>
        /// Position of the animation (0: beginning -> 1: end);
        /// </summary>
        private double mPosition;

        /// <summary>
        /// Position of the animation after the easing function is applied.
        /// </summary>
        private double mEasedPosition;

        /// <summary>
        /// Duration of the animation in milliseconds.
        /// </summary>
        private int mDuration;

        /// <summary>
        /// Whether the animation should be removed from the animation manager upon completion.
        /// </summary>
        private bool mRemoveOnCompletion;

        /// <summary>
        /// Function used to ease the animation.
        /// </summary>
        private EasingFunctionDelegate mEasingFunction;

        /// <summary>
        /// Function used to interpolate the property.
        /// </summary>
        private InterpolationFunctionDelegate<T> mInterpolationFunction;

        #endregion

        #region Public methods

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="target">Object on which a property is to be animated.</param>
        /// <param name="property">Property which is to be animated.</param>
        /// <param name="initialValue">Initial value of the property.</param>
        /// <param name="finalValue">Final value of the property.</param>
        /// <param name="duration">Duration of the animation (in milliseconds).</param>
        /// <param name="easingFunction">Function used to ease the animation.</param>
        /// <param name="removeOnCompletion">
        /// Whether the animation should be removed from the animation manager upon completion.
        /// </param>
        public PropertyAnimation(object target, string property, T initialValue, T finalValue,
            int duration, EasingFunctionDelegate easingFunction = null, bool removeOnCompletion = false)
        {
            mTarget = target;
            mPropertyName = property;
            mInitialValue = initialValue;
            mFinalValue = finalValue;

            mPlaying = false;
            mPaused = false;
            mPosition = 0.0f;
            mEasedPosition = 0.0f;
            mDuration = duration;
            mEasingFunction = easingFunction != null ? easingFunction : EasingFunctions.EaseInOutCubic;
            mRemoveOnCompletion = removeOnCompletion;
        }

        /// <summary>
        /// Updates the animation state.
        /// </summary>
        /// <param name="elapsedTime">Time elapsed since the last step.</param>
        public void AnimationStep(TimeSpan elapsedTime)
        {
            if (this.Playing)
            {
                // Advance the position of the animation.
                this.Position += elapsedTime.TotalMilliseconds / mDuration;

                // If the animation is complete... stay in the end position.
                if (this.Complete)
                {
                    this.Position = 1.0;
                    Pause();
                }
            }
        }

        /// <summary>
        /// Plays the animation from the beginning if it is not playing.
        /// </summary>
        public void Play()
        {
            if (!mPlaying)
            {
                mPlaying = true;
                mPaused = false;
                this.Position = 0.0;
            }
        }

        /// <summary>
        /// Stops the animation and goes back to the beginning.
        /// </summary>
        public void Stop()
        {
            if (mPlaying)
            {
                mPlaying = false;
                mPaused = false;
                this.Position = 0.0;
            }
        }

        /// <summary>
        /// Pauses the animation.
        /// </summary>
        public void Pause()
        {
            mPaused = true;
        }

        /// <summary>
        /// Resumes the animation if it is paused.
        /// </summary>
        public void Resume()
        {
            mPaused = false;
        }

        /// <summary>
        /// Stops the animation and starts playing it from the beginning.
        /// </summary>
        public void Restart()
        {
            mPaused = false;
            this.Position = 0.0;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the target of the animation, i.e. the object which is animated.
        /// </summary>
        public object Target
        {
            get { return mTarget; }
            set { mTarget = value; }
        }

        /// <summary>
        /// Sets the name of the property which is to be animated.
        /// </summary>
        public string PropertyName
        {
            get { return mPropertyName; }
            set
            {
                mPropertyName = value;
                UpdateProperty();
            }
        }

        /// <summary>
        /// Initial value of the property which is animated.
        /// </summary>
        public T InitialValue
        {
            get { return mInitialValue; }
            set
            {
                mInitialValue = value;
                UpdateProperty();
            }
        }

        /// <summary>
        /// Final value of the property which is animated.
        /// </summary>
        public T FinalValue
        {
            get { return mFinalValue; }
            set
            {
                mFinalValue = value;
                UpdateProperty();
            }
        }

        /// <summary>
        /// Gets or sets the function used to ease the animation.
        /// </summary>
        public EasingFunctionDelegate EasingFunction
        {
            get { return mEasingFunction; }
            set
            {
                mEasingFunction = value;
                UpdateProperty();
            }
        }

        /// <summary>
        /// Gets or sets the function used to interpolate between values of the property.
        /// </summary>
        public InterpolationFunctionDelegate<T> InterpolationFunction
        {
            get { return mInterpolationFunction; }
            set
            {
                mInterpolationFunction = value;
                UpdateProperty();
            }
        }

        /// <summary>
        /// Gets or sets the duration of the animation (in milliseconds).
        /// </summary>
        public int Duration
        {
            get { return mDuration; }
            set { mDuration = value; }
        }

        /// <summary>
        /// Gets the current position in the animation (0: beginning -> 1: end).
        /// </summary>
        public double Position
        {
            get { return mPosition; }
            set
            {
                mPosition = value;
                UpdateProperty();
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IAnimation"/> is playing.
        /// </summary>
        /// <value>
        ///   <c>true</c> if playing; otherwise, <c>false</c>.
        /// </value>
        public bool Playing
        {
            get { return mPlaying && !mPaused; }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IAnimation"/> is paused.
        /// </summary>
        /// <value>
        ///   <c>true</c> if paused; otherwise, <c>false</c>.
        /// </value>
        public bool Paused
        {
            get { return !Playing; }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IAnimation"/> is complete.
        /// </summary>
        /// <value>
        ///   <c>true</c> if complete; otherwise, <c>false</c>.
        /// </value>
        public bool Complete
        {
            get { return this.Position >= 1.0f; }
        }

        /// <summary>
        /// Gets or sets whether the animation is to be removed from the animation manager after it
        /// is completed.
        /// </summary>
        public bool RemoveOnCompletion
        {
            get { return mRemoveOnCompletion; }
            set { mRemoveOnCompletion = value; }
        }

        #endregion

        #region Private/Protected methods

        /// <summary>
        /// Updates the property on the target object according to the current position of the 
        /// animation.
        /// </summary>
        protected void UpdateProperty()
        {
            // Ease the animation.
            mEasedPosition = mEasingFunction != null ? mEasingFunction(this.Position) : mPosition;

            if (mInterpolationFunction == null)
                return;

            try
            {
                // Calculate the interpolated value of the property.
                T propertyValue = mInterpolationFunction(mEasedPosition, mInitialValue, mFinalValue);

                PropertyInfo info = mTarget.GetType().GetProperty(mPropertyName);
                info.SetValue(mTarget, propertyValue, null);
            }
            catch (Exception e)
            {
                throw new Exception("PropertyAnimation: error animating the property " + this.PropertyName + ". Error: " + e.Message);
            }
        }

        #endregion
    }
}
