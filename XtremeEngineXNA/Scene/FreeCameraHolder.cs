using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace XtremeEngineXNA.Scene
{
    /// <summary>
    /// Special type of scene node which can hold a camera and checks for keyboard and gamepad
    /// input to move itself the the camera it holds.
    /// </summary>
    public class FreeCameraHolder : SceneNode
    {
        #region Attributes

        /// <summary>
        /// Speed at which the camera moves forward and backwards.
        /// </summary>
        private float mSpeed;

        /// <summary>
        /// Speed at which the camera moves sideways.
        /// </summary>
        private float mStrafeSpeed;

        /// <summary>
        /// Speed at which the camera rotates.
        /// </summary>
        private float mRotationSpeed;

        /// <summary>
        /// Camera attached to this holder.
        /// </summary>
        CameraNode mCamera;

        #endregion

        #region FreeCamera public methods

        /// <summary>
        /// Creates a new free camera.
        /// </summary>
        /// <param name="root">Root object to which the node belongs.</param>
        /// <param name="fov">Field-of-view of the camera.</param>
        /// <param name="aspect">Aspect ratio of the camera.</param>
        /// <param name="nearDist">Distance to the near clipping plane.</param>
        /// <param name="farDist">Distance to the far clipping plane.</param>
        /// <param name="speed">Speed at which the camera moves forward and backwards.</param>
        /// <param name="strafeSpeed">Speed at which the camera moves sideways.</param>
        /// <param name="rotationSpeed">Speed at which the camera rotates.</param>
        public FreeCameraHolder(Root root, float fov = 1.57f, float aspect = 1.3333f,
            float nearDist = 1.0f, float farDist = 1.0f, float speed = 5.0f,
            float strafeSpeed = 5.0f, float rotationSpeed = 1.0f): base(root)
        {
            mSpeed = speed;
            mStrafeSpeed = strafeSpeed;
            mRotationSpeed = rotationSpeed;

            mCamera = new CameraNode(root, fov, aspect, nearDist, farDist);
            this.AttachChild(mCamera);
        }

        /// <summary>
        /// Updates the camera.
        /// </summary>
        /// <param name="elapsedTime">Time elapsed since the last update.</param>
        public override void Update(TimeSpan elapsedTime)
        {
            double seconds = elapsedTime.TotalSeconds;

            MoveForwardsOrBackwards(seconds);
            Strafe(seconds);
            RotateY(seconds);
            RotateX(seconds);

            base.Update(elapsedTime);
        }

        #endregion

        #region Movement

        /// <summary>
        /// Moves the camera forwards or backwards.
        /// </summary>
        /// <param name="seconds">The seconds elapsed since the last movement.</param>
        private void MoveForwardsOrBackwards(double seconds)
        {
            KeyboardState keyState = Keyboard.GetState();
            float leftThumbY = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y;

            //Calculate the forwards/backwards movement due to the keyboard.
            float forwardAmount = 0;
            if (keyState.IsKeyDown(Keys.W))
            {
                forwardAmount += (float)(mSpeed * seconds);
            }
            if (keyState.IsKeyDown(Keys.S))
            {
                forwardAmount -= (float)(mSpeed * seconds);
            }
            //Calculate the forwards/backwards movement due to the gamepad input and combine it with
            //the movement due to the keyboard input.
            float GamepadForwardAmount = (float)(mSpeed * seconds) * leftThumbY;
            forwardAmount = CombineKeyboardAndGamepad(forwardAmount, GamepadForwardAmount);

            //Translate the camera.
            this.Translate(mCamera.AbsoluteDirection * forwardAmount);
        }

        /// <summary>
        /// Moves the camera perpendicularly to its current direction.
        /// </summary>
        /// <param name="seconds">The seconds elapsed since the last movement.</param>
        private void Strafe(double seconds)
        {
            KeyboardState keyState = Keyboard.GetState();
            float leftThumbX = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X;

            //Calculate the strafe movement due to the keyboard.
            float strafeAmount = 0;
            if (keyState.IsKeyDown(Keys.D))
            {
                strafeAmount += (float)(mStrafeSpeed * seconds);
            }
            if (keyState.IsKeyDown(Keys.A))
            {
                strafeAmount -= (float)(mStrafeSpeed * seconds);
            }

            //Calculate the strafe movement due to the gamepad input and combine it with the 
            //movement due to the keyboard input.
            float GamepadStrafeAmount = (float)(mStrafeSpeed * seconds) * leftThumbX;
            strafeAmount = CombineKeyboardAndGamepad(strafeAmount, GamepadStrafeAmount);

            //Calculate the strafe direction and translate the camera in that direction.
            Vector3 strafeDir = Vector3.Cross(this.Direction, Vector3.UnitY);
            strafeDir.Normalize();
            this.Translate(strafeDir * strafeAmount);
        }

        /// <summary>
        /// Rotates the camera along the Y axis.
        /// </summary>
        /// <param name="seconds">The seconds elapsed since the last rotation.</param>
        private void RotateX(double seconds)
        {
            KeyboardState keyState = Keyboard.GetState();
            float rightThumbY = GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y;

            //Calculate the rotation due to the keyboard input.
            float rotationX = 0;
            if (keyState.IsKeyDown(Keys.Up))
            {
                rotationX += (float)(mRotationSpeed * seconds);
            }
            if (keyState.IsKeyDown(Keys.Down))
            {
                rotationX -= (float)(mRotationSpeed * seconds);
            }

            //Calculate the rotation due to the gamepad input and combine it with the rotation due
            //to the keyboard input.
            float GamepadRotationX = (float)(mSpeed * seconds) * rightThumbY / 2;
            rotationX = CombineKeyboardAndGamepad(rotationX, GamepadRotationX);
            mCamera.Rotate(Vector3.UnitX, rotationX);
        }

        /// <summary>
        /// Rotates the camera along the Y axis.
        /// </summary>
        /// <param name="seconds">The seconds elapsed since the last rotation.</param>
        private void RotateY(double seconds)
        {
            KeyboardState keyState = Keyboard.GetState();
            float rightThumbX = GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X;

            //Calculate the rotation due to the keyboard input.
            float rotationY = 0;
            if (keyState.IsKeyDown(Keys.Left))
            {
                rotationY += (float)(mRotationSpeed * seconds);
            }
            if (keyState.IsKeyDown(Keys.Right))
            {
                rotationY -= (float)(mRotationSpeed * seconds);
            }

            //Calculate the rotation due to the gamepad input and combine it with the rotation due
            //to the keyboard input.
            float GamepadRotationY = (float)(mSpeed * seconds) * -rightThumbX / 2;
            rotationY = CombineKeyboardAndGamepad(rotationY, GamepadRotationY);
            this.Rotate(Vector3.UnitY, rotationY);
        }

        #endregion

        #region Utility methods

        /// <summary>
        /// Combines the keyboard and gamepad inputs. The gamepad input is only added to the final
        /// input if it counteracts the keyboard input or if there is no keyboard input at all.
        /// </summary>
        /// <param name="keyboardInput">The keyboard input.</param>
        /// <param name="gamepadInput">The gamepad input.</param>
        /// <returns>The combined input of the keyboard and the gamepad.</returns>
        private float CombineKeyboardAndGamepad(float keyboardInput, float gamepadInput)
        {
            //We only add the gamepad input if it opposes the movement due to the keyboard input
            //or if there is no keyboard input.
            if (keyboardInput * gamepadInput < 0.0f)
            {
                return keyboardInput + gamepadInput;
            }
            else if (keyboardInput == 0.0f)
            {
                return gamepadInput;
            }
            else
            {
                return keyboardInput;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the speed at which the camera moves forward and backwards.
        /// </summary>
        /// <value>The speed at which the camera moves forwards and backwards.</value>
        public float Speed
        {
            get { return mSpeed; }
            set { mSpeed = value; }
        }

        /// <summary>
        /// Gets or sets the speed at which the camera moves sideways.
        /// </summary>
        /// <value>The speed at which the camera moves sideways.</value>
        public float StrafeSpeed
        {
            get { return mStrafeSpeed; }
            set { mStrafeSpeed = value; }
        }

        /// <summary>
        /// Gets or sets the speed at which the camera rotates.
        /// </summary>
        /// <value>The speed at which the camera rotates.</value>
        public float RotationSpeed
        {
            get { return mRotationSpeed; }
            set { mRotationSpeed = value; }
        }

        /// <summary>
        /// Returns the camera held by the camera holder.
        /// </summary>
        public CameraNode Camera
        {
            get { return mCamera; }
        }

        #endregion
    }
}
