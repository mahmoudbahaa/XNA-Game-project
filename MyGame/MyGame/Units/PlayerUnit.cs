using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Helper;

namespace MyGame
{
    public class PlayerUnit : Unit
    {
        //here goes the player attribute like speed health etc ...
        public int health;

        public PlayerUnit(Game1 game,Vector3 Position, Vector3 Rotation, Vector3 Scale)
            : base(game,Position, Rotation, Scale)
        {
            game.mediator.register(this, MyEvent.C_FORWARD, MyEvent.C_BACKWARD, MyEvent.C_LEFT,
                MyEvent.C_RIGHT, MyEvent.C_Pointer, MyEvent.M_BITE);
            health = 100;
        }

        public override void update(GameTime gameTime)
        {
            //rotation += new Vector3(0, MathHelper.Pi, 0);
            float leftRight = 0;
            float forwardBackward = 0;

            foreach (Event ev in events)
            {
                switch (ev.EventId)
                {
                    case  (int)MyEvent.C_LEFT:
                        leftRight -= 1;
                        break;
                    case (int)MyEvent.C_RIGHT:
                        leftRight += 1;
                        break;
                    case (int)MyEvent.C_FORWARD:
                        forwardBackward = -1;
                        break;
                    case (int)MyEvent.C_BACKWARD:
                        forwardBackward = 1;
                        break;
                    case (int)MyEvent.C_Pointer:
                        float deltaX = (float)ev.args["deltaX"];
                        rotation += new Vector3(0, deltaX, 0);
                        break;
                    case (int)MyEvent.M_BITE:
                        decreaseHealth();
                        break;
                }
            }
            events.Clear(); 
   
            //rotation += new Vector3(0, rotY * .025f, 0);
            Matrix rot = Matrix.CreateFromYawPitchRoll(rotation.Y, rotation.X, rotation.Z);
            Vector3 oldPos = position;
            position += Vector3.Transform(new Vector3(-leftRight, 0, -forwardBackward), rot) *
               (float)gameTime.ElapsedGameTime.TotalMilliseconds * Constants.PLAYER_SPEED;
            //position = Vector3.Clamp(position, new Vector3(-Constants.FIELD_MAX_X_Z), new Vector3(Constants.FIELD_MAX_X_Z));
            position.Y = myGame.GetHeightAtPosition2(position.X, position.Z);
            if (position.Y > .7 * Constants.TERRAIN_HEIGHT)
            {
                position = oldPos;
            }


            base.update(gameTime);
        }

        public void decreaseHealth()
        {
            health -= myGame.difficultyConstants.PLAYER_HEALTH_DECREASE;
            if (health <= 0)
            {
                myGame.gameOver = true;
                myGame.paused = true;
                myGame.mediator.fireEvent(MyEvent.G_GameOver);
            }

        }

       
    }
}
