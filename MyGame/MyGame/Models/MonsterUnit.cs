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

        private Vector3 direction;

        public MonsterUnit(Game1 game,Vector3 Position, Vector3 Rotation, Vector3 Scale)
            : base(game,Position, Rotation, Scale)
        {
            MonsterSpeed = 3f;
            Health = 100;
            direction = Vector3.Transform(Vector3.Backward,
                Matrix.CreateFromYawPitchRoll(rotation.Y,rotation.X,rotation.Z));

        }

        public override void update(GameTime gameTime)
        {
            position += direction * MonsterSpeed;

            if (position.X < -2350 || position.X > 2350 || position.Z < -2350 || position.Z > 2350)
            {
                rotation = new Vector3(rotation.X, MathHelper.Pi + rotation.Y, rotation.Z);
                direction = -direction;
            }

            base.update(gameTime);
        }
    }
}
