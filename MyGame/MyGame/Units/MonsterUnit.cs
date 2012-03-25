using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Helper;

namespace MyGame
{
    public class MonsterUnit:Unit
    {
        private float timeToDie = 5000;
        public bool dead = false;
        public bool moving = false;

        private Vector3 direction;

        public MonsterUnit(Game1 game,Vector3 Position, Vector3 Rotation, Vector3 Scale)
            : base(game,Position, Rotation, Scale)
        {
            direction = Vector3.Transform(Vector3.Backward,
                Matrix.CreateFromYawPitchRoll(rotation.Y,rotation.X,rotation.Z));

        }

        public override void update(GameTime gameTime)
        {
            if (alive)
            {
                Vector3 relPos = position - myGame.player.unit.position;

                rotation = new Vector3(0, (float)Math.Atan2(relPos.X, relPos.Z)+MathHelper.Pi, 0);

                if (moving)
                {
                    direction = Vector3.Transform(Vector3.Backward,
                   Matrix.CreateFromYawPitchRoll(rotation.Y, rotation.X, rotation.Z)); 

                    position += direction * Constants.MONSTER_SPEED;

                    //if (position.X < Constants.FIELD_MIN_X_Z || position.X > Constants.FIELD_MAX_X_Z ||
                    //    position.Z < Constants.FIELD_MIN_X_Z || position.Z > Constants.FIELD_MAX_X_Z)
                    //{
                    //    rotation = new Vector3(rotation.X, MathHelper.Pi + rotation.Y, rotation.Z);
                    //    direction = -direction;
                    //}
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
