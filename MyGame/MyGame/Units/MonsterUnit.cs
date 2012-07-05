using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Helper;

namespace MyGame
{
    /// <summary>
    /// This class represent monster unit that is idle unless attcked or approached by player then it moves toward the 
    /// player attcking him whevener in the vicinity of the player and take damage on colliding with bullets & 
    /// diing when it's hp reaches zero.
    /// </summary>
    public class MonsterUnit:Unit
    {
        private float timeToDie = 5000;
        public bool dead = false;
        public bool moving = false;

        private Vector3 direction;
        public MonsterConstants monsterConstants;

        public MonsterUnit(MyGame game,Vector3 Position, Vector3 Rotation, Vector3 Scale, MonsterConstants monsterConstants)
            : base(game,Position, Rotation, Scale)
        {
            direction = Vector3.Transform(Vector3.Backward,
                Matrix.CreateFromYawPitchRoll(rotation.Y,rotation.X,rotation.Z));

            this.monsterConstants = monsterConstants;
        }

        /// <summary>
        /// Allows the unit to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>>
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
                    position += direction * monsterConstants.MONSTER_SPEED;

                    int num_of_Steps_backward = 0;
                    if(myGame.checkCollisionWithTrees(this, 15))
                    {
                        position = oldPos;
                        for (int i = 0; i < num_of_Steps_backward; i++)
                        {
                            direction = Vector3.Transform(Vector3.Forward,
                            Matrix.CreateFromYawPitchRoll(rotation.Y, rotation.X, rotation.Z));

                            position += direction * monsterConstants.MONSTER_SPEED;
                        }
                        direction = Vector3.Transform(Vector3.Right,
                            Matrix.CreateFromYawPitchRoll(rotation.Y, rotation.X, rotation.Z));

                        position += direction * monsterConstants.MONSTER_SPEED;
                        num_of_Steps_backward++;
                    }

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
