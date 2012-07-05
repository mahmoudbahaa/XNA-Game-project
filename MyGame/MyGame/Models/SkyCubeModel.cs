using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace MyGame
{
    /// <summary>
    /// This class represent sky cube model, it implement IRenderable in order to be reflected by the water 
    /// </summary>
    public class SkyCubeModel : CModel,IRenderable
    {
        public Effect effect;

        public SkyCubeModel(MyGame game, Model model, TextureCube Texture)
            :base(game,model)
        {
            effect = game.Content.Load<Effect>("skysphere_effect");
            effect.Parameters["CubeMap"].SetValue(Texture);

            SetModelEffect(effect, false);
        }

        public override void Draw(GameTime gameTime)
        {
            // Disable the depth buffer
            myGame.GraphicsDevice.DepthStencilState = DepthStencilState.None;

            //// Move the model with the sphere
            //model.Position = CameraPosition;

            base.Draw(gameTime);

            myGame.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
        }

        public void Draw()
        {
            Draw(null);
        }
        public void SetClipPlane(Vector4? Plane)
        {
            effect.Parameters["ClipPlaneEnabled"].SetValue(Plane.HasValue);
            
            if (Plane.HasValue)
                effect.Parameters["ClipPlane"].SetValue(Plane.Value);
        }
    }
}
