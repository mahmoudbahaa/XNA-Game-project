using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Helper;

namespace MyGame
{
    class MonsterUnit:Unit
    {
        private float MonsterSpeed {get;set;}
        private int Health { get; set; }

        private float timeToDie = 5000;
        public bool dead = false;

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
            if (alive)
            {
                position += direction * MonsterSpeed;

                if (position.X < Constants.FIELD_MIN_X_Z || position.X > Constants.FIELD_MAX_X_Z ||
                    position.Z < Constants.FIELD_MIN_X_Z || position.Z > Constants.FIELD_MAX_X_Z)
                {
                    rotation = new Vector3(rotation.X, MathHelper.Pi + rotation.Y, rotation.Z);
                    direction = -direction;
                }
            }

            else
            {
                timeToDie -= gameTime.ElapsedGameTime.Milliseconds;
                if (timeToDie < 0)
                    dead = true;
            }

            base.update(gameTime);
        }
    }
}
