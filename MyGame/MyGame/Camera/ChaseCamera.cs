using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MyGame
{
    public class ChaseCamera : Camera
    {

        public Vector3 FollowTargetPosition { get; private set; }
        public Vector3 FollowTargetRotation { get; private set; }

        public Vector3 PositionOffset { get; set; }
        public Vector3 TargetOffset { get; set; }

        public Vector3 RelativeCameraRotation { get; set; }

        float springiness = .15f;

        //MouseState lastMouseState;

        public float Springiness
        {
            get { return springiness; }
            set { springiness = MathHelper.Clamp(value, 0, 1); }
        }

        public ChaseCamera(Game game, Vector3 PositionOffset, Vector3 TargetOffset,
            Vector3 RelativeCameraRotation )
            : base(game)
        {
            this.PositionOffset = PositionOffset;
            this.TargetOffset = TargetOffset;
            this.RelativeCameraRotation = RelativeCameraRotation;
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

        public override void  Update(GameTime gameTime)
        {
            //// Get the new keyboard and mouse state
            //MouseState mouseState = Mouse.GetState();
            //KeyboardState keyState = Keyboard.GetState();

            //// Determine how much the camera should turn
            //float deltaX = (float)lastMouseState.X - (float)mouseState.X;
            //float deltaY = (float)lastMouseState.Y - (float)mouseState.Y;

            //// Rotate the camera
            //Rotate(new Vector3(-deltaY * .0005f, -deltaX * .0005f, 0));

            Vector3 translation = Vector3.Zero;

            // Determine in which direction to move the camera

            // Move 4 units per millisecond, independent of frame rate
            //translation *= 0.5f *
            //    (float)gameTime.ElapsedGameTime.TotalMilliseconds;

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

            if (Position.Y < 10) Position.Y = 10 ;

            // Calculate the new target using the rotation matrix
            Target = FollowTargetPosition +
                Vector3.Transform(TargetOffset, rotation);

            // Obtain the up vector from the matrix
            Vector3 up = Vector3.Transform(Vector3.Up, rotation);

            // Recalculate the view matrix
            View = Matrix.CreateLookAt(Position, Target, up);
            //lastMouseState = mouseState;
            base.Update(gameTime);
        }
    }
}
