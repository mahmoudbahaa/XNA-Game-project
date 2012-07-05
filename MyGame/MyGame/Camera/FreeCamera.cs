using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MyGame
{
    /// <summary>
    /// This class represent a Free Camera that move and rotate freely 
    /// </summary>
    public class FreeCamera : Camera
    {
        public float Yaw { get; set; }
        public float Pitch { get; set; }


        private Vector3 translation;
        public Terrain terrain;
        private float maxHeight;
        private float minHeight;

        MouseState lastMouseState;

        public FreeCamera(MyGame game, Vector3 Position, float Yaw, float Pitch,
            float minHeight, float maxHeight)
            : base(game)
        {
            this.Position = Position;
            this.Yaw = Yaw;
            this.Pitch = Pitch;
            this.minHeight = minHeight;
            this.maxHeight = maxHeight;

            translation = Vector3.Zero;
        }

        public void Rotate(float YawChange, float PitchChange)
        {
            this.Yaw += YawChange;
            this.Pitch += PitchChange;
        }

        public void Move(Vector3 Translation)
        {
            this.translation += Translation;
        }

        /// <summary>
        /// Allows the component to run logic.
        /// </summary>
        /// <param name="gameTime">The gametime.</param>
        public override void Update(GameTime gameTime)
        {
            // Get the new keyboard and mouse state
            MouseState mouseState = Mouse.GetState();
            KeyboardState keyState = Keyboard.GetState();
            // Determine how much the camera should turn
            float deltaX = (float)lastMouseState.X - (float)mouseState.X;
            float deltaY = (float)lastMouseState.Y - (float)mouseState.Y;

            // TODO: Add your update code here
            // Calculate the rotation matrix
            Rotate(deltaX * .01f, deltaY * .01f);
            Matrix rotation = Matrix.CreateFromYawPitchRoll(Yaw, Pitch, 0);

            Vector3 translation = Vector3.Zero;

            // Determine in which direction to move the camera
            if (keyState.IsKeyDown(Keys.W)) translation +=  Vector3.Forward/100;
            if (keyState.IsKeyDown(Keys.S)) translation +=  Vector3.Backward/100;
            if (keyState.IsKeyDown(Keys.A)) translation +=  Vector3.Left/100;
            if (keyState.IsKeyDown(Keys.D)) translation +=  Vector3.Right/100;
            // Move 3 units per millisecond, independent of frame rate
            translation *= 3 * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            // Update the mouse state
            lastMouseState = mouseState;

            // Offset the position and reset the translation
            translation = Vector3.Transform(translation, rotation);
            Position += translation;

            //float height = terrain.GetHeightAtPosition(Position.X, Position.Z);
            //if (Position.Y < height + minHeight)
            //    Position.Y = height + minHeight;
            //else if (Position.Y > height + maxHeight)
            //    Position.Y = height + maxHeight;
            //MathHelper.Clamp(Position.Y, height+500f, height + maxHeight);

            // Calculate the new target
            Vector3 forward = Vector3.Transform(Vector3.Forward, rotation);
            Target = Position + forward;

            // Calculate the up vector
            Vector3 up = Vector3.Transform(Vector3.Up, rotation);

            // Calculate the view matrix
            View = Matrix.CreateLookAt(Position, Target, up);
            base.Update(gameTime);
        }
    }
}
