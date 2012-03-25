using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Helper;

namespace MyGame
{
    public class BulletUnit : Unit
    {
        Vector3 Direction { get; set; }

        public BulletUnit(Game1 game,Vector3 Position, Vector3 Rotation, Vector3 Scale,Vector3 Direction)
            : base(game,Position, Rotation, Scale)
        {
            this.Direction = Direction;            
        }

        public override void update(GameTime gameTime)
        {
            // Move bullet
            position += Direction * Constants.BULLET_SPEED;
            base.update(gameTime);
        }
    }
}
