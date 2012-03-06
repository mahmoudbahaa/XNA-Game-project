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
        public Unit unit;
        protected Game1 myGame;

        public CDrawableComponent(Game1 game,Unit unit)
            :base(game)
        {
            myGame = game;
            this.unit = unit;
        }

        public override void Draw(GameTime gameTime)
        {
            cModel.Draw(gameTime);
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            // Calculate the base transformation by combining translation, rotation, and scaling
            cModel.updateBaseWorld(unit.position, unit.rotation, unit.scale, unit.baseWorld);

            unit.update(gameTime);
            base.Update(gameTime);
        }

    }
}
