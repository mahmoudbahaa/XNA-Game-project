using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Helper;

namespace MyGame
{
    public class ChaseCamera : Camera
    {

        public Vector3 FollowTargetPosition { get; private set; }
        public Vector3 FollowTargetRotation { get; private set; }

        private Vector3 savedTargetOffset;
        private Vector3 savedPositionOffset;

        private Vector3 PositionOffset;
        private Vector3 initialPositionOffset;
        private Vector3 TargetOffset;

        private float sensitivity;
        private float lastTargetOffsetY;

        private MouseState lastMouseState;

        public Vector3 RelativeCameraRotation { get; set; }

        public Vector3 Up;

        public Vector3 Right;

        float springiness = 1f;

        public float Springiness
        {
            get { return springiness; }
            set { springiness = MathHelper.Clamp(value, 0, 1); }
        }

        public ChaseCamera(MyGame game, Vector3 PositionOffset, Vector3 TargetOffset,
            Vector3 RelativeCameraRotation )
            : base(game)
        {
            this.savedTargetOffset = TargetOffset;
            this.savedPositionOffset = PositionOffset;
            initialPositionOffset = PositionOffset;
            this.PositionOffset = PositionOffset;
            this.TargetOffset = TargetOffset;
            this.RelativeCameraRotation = RelativeCameraRotation;
            Mouse.SetPosition(myGame.GraphicsDevice.Viewport.Width / 2, myGame.GraphicsDevice.Viewport.Height / 2);
            lastMouseState = Mouse.GetState();
        }

        public void Move(Vector3 NewFollowTargetPosition,
            Vector3 NewFollowTargetRotation)
        {
            this.FollowTargetPosition = NewFollowTargetPosition;
            this.FollowTargetRotation = NewFollowTargetRotation;
        }

        public void Rotate(Vector3 RotationChange)
        {
            this.RelativeCameraRotation += RotationChange;
        }


        private Matrix recalculatePosition()
        {
            // Sum the rotations of the model and the camera to ensure it 
            // is rotated to the correct position relative to the model's 
            // rotation
            Vector3 combinedRotation = FollowTargetRotation +
                RelativeCameraRotation;

            // Calculate the rotation matrix for the camera
            Matrix rotation = Matrix.CreateFromYawPitchRoll(
                combinedRotation.Y, combinedRotation.X, combinedRotation.Z);

            // Calculate the position the camera would be without the spring
            // value, using the rotation matrix and target position
            Vector3 desiredPosition = FollowTargetPosition +
                Vector3.Transform(PositionOffset, rotation);

            // Interpolate between the current position and desired position
            Position = Vector3.Lerp(Position, desiredPosition, Springiness);

            return rotation;
        }

        public void resetOffsets()
        {
            TargetOffset = savedTargetOffset;
            PositionOffset = savedPositionOffset;
            sensitivity = 1;
        }

        public void setOffsetsFor1stPerson()
        {
            TargetOffset.Z = 0;
            TargetOffset.Y = TargetOffset.Y * 2.5f;
            PositionOffset.Y = PositionOffset.Y * 2.5f;
            PositionOffset.Z = 1;
            sensitivity = 0.5f;
        }

        public override void  Update(GameTime gameTime)
        {
            if (myGame.paused)
                return;

            // Get the new keyboard and mouse state
            MouseState mouseState = Mouse.GetState();
            KeyboardState keyState = Keyboard.GetState();

            // Determine how much the camera should turn
            float deltaX;
            float deltaY;


            if (System.Windows.Forms.Control.IsKeyLocked(System.Windows.Forms.Keys.CapsLock))
            {
                if (myGame.controller.isActive(control.Controller.POINTER))
                {
                    //savedPositionOffset = PositionOffset;
                    //savedTargetOffset = TargetOffset;
                    myGame.cameraMode = MyGame.CameraMode.firstPersonWithoutWeapon;
                    Vector2 d = myGame.controller.getPointer();
                    deltaX = d.X *.5f;
                    deltaY = d.Y * .5f;
                }
                else
                {
                    myGame.cameraMode = MyGame.CameraMode.thirdPerson;
                    //PositionOffset = savedPositionOffset;
                    //TargetOffset = savedTargetOffset;
                    float diff = myGame.controller.getShoulderDiff();
                    deltaX = diff * 200;
                    if (Math.Abs(deltaX) < 10f)
                        deltaX = 0;
                    deltaY = 0;
                }
            }
            else
            {
                deltaX = -((float)lastMouseState.X - (float)mouseState.X) * 15;
                deltaY = ((float)lastMouseState.Y - (float)mouseState.Y) * 15;
            }

            Matrix rotation;
            if (TargetOffset.Y > savedTargetOffset.Y && deltaY < 0)
            {
                TargetOffset.Y += 5 * deltaY * .0005f * sensitivity;
                if (TargetOffset.Y < savedTargetOffset.Y)
                    TargetOffset.Y = savedTargetOffset.Y;
                rotation = recalculatePosition();
            }
            else
            {
                if (TargetOffset.Y < savedTargetOffset.Y)
                    TargetOffset.Y = savedTargetOffset.Y;
                // Rotate the camera
                Rotate(new Vector3(deltaY * .0005f * sensitivity, 0, 0));


                myGame.mediator.fireEvent(MyEvent.C_Pointer, "deltaX", -deltaX * .0005f * sensitivity);//controlPointer(-deltaX * .0005f);

                //Natural Chase Camera Update
                //Vector3 translation = Vector3.Zero;

                rotation = recalculatePosition();

                if (Position.Y < myGame.GetHeightAtPosition(myGame.player.unit.position.X, myGame.player.unit.position.Z) + 10)
                {
                    //Rotate(new Vector3(-deltaY * .0005f, 0, 0));
                    //TargetOffset.Y = savedTargetOffset.Y;
                    //rotation = recalculatePosition();
                    //if (Position.Y < myGame.GetHeightAtPosition(myGame.player.unit.position.X, myGame.player.unit.position.Z) + 10)
                    //{
                    Rotate(new Vector3(-deltaY * .0005f * sensitivity, 0, 0));
                    lastTargetOffsetY = TargetOffset.Y += 5 * deltaY * .0005f * sensitivity;
                    rotation = recalculatePosition();
                    //if (Position.Y < myGame.GetHeightAtPosition(Position.X, Position.Z) + 10)
                    //{
                    //    Rotate(new Vector3(-deltaY * .0005f * sensitivity, 0, 0));
                    //    rotation = recalculatePosition();
                    //}
                    //}
                }
            }
            //int i = 0;
            //while (Position.Y < myGame.GetHeightAtPosition(myGame.player.unit.position.X, myGame.player.unit.position.Z) +2)
            //{
            //    if (i++ > 1000)
            //        break;
            //    //if (combinedRotation.X >= -MathHelper.PiOver2)
            //    combinedRotation.X -= 0.01f;
            //    //else
            //        //combinedRotation.X += 0.001f;

            //    // Calculate the rotation matrix for the camera
            //    rotation = Matrix.CreateFromYawPitchRoll(
            //        combinedRotation.Y, combinedRotation.X, combinedRotation.Z);

            //    // Calculate the position the camera would be without the spring
            //    // value, using the rotation matrix and target position
            //    desiredPosition = FollowTargetPosition +
            //        Vector3.Transform(PositionOffset, rotation);

            //    // Interpolate between the current position and desired position
            //    Position = Vector3.Lerp(Position, desiredPosition, Springiness);
            //}

            // Calculate the new target using the rotation matrix
            Target = FollowTargetPosition +
                Vector3.Transform(TargetOffset, rotation);
            // Obtain the up vector from the matrix
            Vector3 up = Vector3.Transform(Vector3.Up, rotation);

            // Recalculate the view matrix
            View = Matrix.CreateLookAt(Position, Target, up);

            // Calculate the new target
            Vector3 forward = Vector3.Transform(Vector3.Forward, rotation);

            this.Up = up;
            this.Right = Vector3.Cross(forward, up);

            int scrollWheelValue = mouseState.ScrollWheelValue - lastMouseState.ScrollWheelValue;

            if (keyState.IsKeyDown(Keys.Up) && TargetOffset.Y < 80) 
                TargetOffset.Y += 1;
            if (keyState.IsKeyDown(Keys.Down) && TargetOffset.Y > 0)
                TargetOffset.Y -= 1;
            if (keyState.IsKeyDown(Keys.Right) && TargetOffset.X < 50)
                TargetOffset.X += 1;
            if (keyState.IsKeyDown(Keys.Left) && TargetOffset.X > -50)
                TargetOffset.X -= 1;

            PositionOffset.Z -= scrollWheelValue / 2;
            if ((PositionOffset.Z - TargetOffset.Z) < 10)
                PositionOffset.Z += scrollWheelValue/2;

            //else if ((Position.Z - Target.Z) < scrollWheelValue / 2)
            //    PositionOffset.Z += scrollWheelValue/2 ;

            if (keyState.IsKeyDown(Keys.LeftControl))
            {
                lastMouseState = mouseState;
            }
            else
            {
                Mouse.SetPosition(myGame.GraphicsDevice.Viewport.Width / 2, myGame.GraphicsDevice.Viewport.Height / 2);
                lastMouseState = Mouse.GetState();
            }

            base.Update(gameTime);
        }
    }
}
