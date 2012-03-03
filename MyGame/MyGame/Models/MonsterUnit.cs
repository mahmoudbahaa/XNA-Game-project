using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MyGame
{
    class MonsterUnit:Unit
    {
        private float MonsterSpeed {get;set;}
        private int Health { get; set; }

        public MonsterUnit(Game1 game,Vector3 Position, Vector3 Rotation, Vector3 Scale)
            : base(game,Position, Rotation, Scale)
        {
            MonsterSpeed = 1.0f;
            Health = 100;
        }

        public override void update(GameTime gameTime)
        {
            base.update(gameTime);
            //TODO: AI movement of the monster
        }
    }
}
