using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Helper;

namespace MyGame
{
    class PlayerUnit : Unit
    {
        //here goes the player attribute like speed health etc ...
        private float PlayerSpeed { get; set; }
        private int Health { get; set; }

        public PlayerUnit(Game1 game,Vector3 Position, Vector3 Rotation, Vector3 Scale)
            : base(game,Position, Rotation, Scale)
        {
            game.register(this, MyEvent.C_FORWARD, MyEvent.C_BACKWARD, MyEvent.C_LEFT, MyEvent.C_RIGHT);
            PlayerSpeed = .1f;
            Health = 100;
        }

        public override void update(GameTime gameTime)
        {
            base.update(gameTime);
            float leftRight = 0;
            float forwardBackward = 0;

            foreach (int ev in events)
            {
                switch (ev)
                {
                    case  MyEvent.C_LEFT:
                        leftRight -= 1f;
                        break;
                    case MyEvent.C_RIGHT:
                        leftRight += 1f;
                        break;
                    case MyEvent.C_FORWARD:
                        forwardBackward = -1f;
                        break;
                    case MyEvent.C_BACKWARD:
                        forwardBackward = 1f;
                        break;
                }
            }
            events.Clear(); 
   
            //rotation += new Vector3(0, rotY * .025f, 0);
            Matrix rot = Matrix.CreateFromYawPitchRoll(rotation.Y, rotation.X, rotation.Z);
            position += Vector3.Transform(new Vector3(leftRight, 0, forwardBackward), rot) *
               (float)gameTime.ElapsedGameTime.TotalMilliseconds * PlayerSpeed;

        }
    }
}
