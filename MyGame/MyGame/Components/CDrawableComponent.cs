using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MyGame
{
    public class CDrawableComponent : DrawableGameComponent
    {
        public CModel cModel;

        public CDrawableComponent(Game1 game)
            :base(game)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            cModel.Draw(gameTime);
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            cModel.unit.update(gameTime);
            base.Update(gameTime);
        }

    }
}
