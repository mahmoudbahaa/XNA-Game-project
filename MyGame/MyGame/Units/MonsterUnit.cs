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

                    Vector3 oldPos = position ;
                    position += direction * myGame.difficultyConstants.MONSTER_SPEED;

                    float y = myGame.GetHeightAtPosition(position.X, position.Z);
                    //if (y > .7 * Constants.TERRAIN_HEIGHT)
                    //{
                    //    position = oldPos;
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
