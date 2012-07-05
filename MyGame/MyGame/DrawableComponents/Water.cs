using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MyGame
{
    /// <summary>
    /// This class represent Water componenet that reflect the sky 
    /// </summary>
    class Water : CDrawableComponent
    {
        Effect waterEffect;
        public Water(MyGame game, Model model, Unit unit)
            :base(game,unit,new CModel(game, model))
        {
            waterEffect = game.Content.Load<Effect>("WaterEffect");
            cModel.SetModelEffect(waterEffect, false);

            waterEffect.Parameters["viewportWidth"].SetValue(
                game.GraphicsDevice.Viewport.Width);

            waterEffect.Parameters["viewportHeight"].SetValue(
                game.GraphicsDevice.Viewport.Height);

            waterEffect.Parameters["WaterNormalMap"].SetValue(
                game.Content.Load<Texture2D>("water_normal"));

            ((WaterUnit)unit).waterEffect = waterEffect;
        }

    }
}
