using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MyGame
{
    class SkyCubeUnit:Unit
    {

        public SkyCubeUnit(MyGame game, Vector3 Position, Vector3 Rotation, Vector3 Scale)
            : base(game,Position, Rotation, Scale)
        {
        }

        public override void update(GameTime gameTime)
        {
            position = myGame.camera.Position;


            base.update(gameTime);
        }
    }
}
