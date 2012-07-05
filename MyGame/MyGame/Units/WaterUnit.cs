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
    /// This class represent water unit that reflect all those that implement IRenderable (like sky)
    /// </summary>
    public interface IRenderable
    {
        void Draw();
        void SetClipPlane(Vector4? Plane);
    }

    class WaterUnit:Unit
    {
        public RenderTarget2D reflectionTarg;
        public Effect waterEffect;
        public List<IRenderable> Objects = new List<IRenderable>();

        public WaterUnit(MyGame game,Vector3 Position, Vector3 Rotation, Vector3 Scale)
            : base(game,Position, Rotation, Scale)
        {
            reflectionTarg = new RenderTarget2D(game.GraphicsDevice, game.GraphicsDevice.Viewport.Width,
               game.GraphicsDevice.Viewport.Height, false, SurfaceFormat.Color,
               DepthFormat.Depth24);
        }

        public void renderReflection(GameTime gameTime)
        {
            // Reflect the camera's properties across the water plane
            Vector3 reflectedCameraPosition = myGame.camera.Position;
            reflectedCameraPosition.Y = -reflectedCameraPosition.Y
                + position.Y * 2;

            Vector3 reflectedCameraTarget = myGame.camera.Target;
            reflectedCameraTarget.Y = -reflectedCameraTarget.Y
                + position.Y * 2;

            // Create a temporary camera to render the reflected scene
            TargetCamera reflectionCamera = new TargetCamera(myGame,reflectedCameraPosition,reflectedCameraTarget);

            reflectionCamera.Update();

            // Set the reflection camera's view matrix to the water effect
            waterEffect.Parameters["ReflectedView"].SetValue(reflectionCamera.View);

            // Create the clip plane
            Vector4 clipPlane = new Vector4(0, 1, 0, -position.Y);

            // Set the render target
            myGame.GraphicsDevice.SetRenderTarget(reflectionTarg);
            myGame.GraphicsDevice.Clear(Color.Black);

            // Draw all objects with clip plane
            Camera oldCamera = myGame.camera;
            myGame.camera = reflectionCamera;
            foreach (IRenderable renderable in Objects)
            {
                renderable.SetClipPlane(clipPlane);

                renderable.Draw();
                //reflectionCamera.View,
                //    reflectionCamera.Projection,
                //    reflectedCameraPosition);
                
                renderable.SetClipPlane(null);
            }
            myGame.camera = oldCamera;

            myGame.GraphicsDevice.SetRenderTarget(null);

            // Set the reflected scene to its effect parameter in
            // the water effect
            waterEffect.Parameters["ReflectionMap"].SetValue(reflectionTarg);
        }

        /// <summary>
        /// Allows the unit to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>>
        public override void update(GameTime gameTime)
        {
            renderReflection(gameTime);
            waterEffect.Parameters["Time"].SetValue((float)gameTime.TotalGameTime.TotalSeconds);
            base.update(gameTime);
        }
    }
}
