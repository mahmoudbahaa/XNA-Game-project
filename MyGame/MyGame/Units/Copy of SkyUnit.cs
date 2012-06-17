using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MyGame
{
    class SkyUnit:Unit
    {
        private float RotationSpeed {get;set;}
        //private const int updateDelay = 1000;
        //private int sinceLastUpdate = 0;

        public SkyUnit(MyGame game,Vector3 Position, Vector3 Rotation, Vector3 Scale)
            : base(game,Position, Rotation, Scale)
        {
            RotationSpeed = MathHelper.PiOver4;
        }

        public override void update(GameTime gameTime)
        {
            // //Move the model with the sphere
            ////position = game.camera.Position;
            //sinceLastUpdate += gameTime.ElapsedGameTime.Milliseconds;
            //rotation += new Vector3(RotationSpeed);
            //if(sinceLastUpdate > updateDelay)
            //{
                
            //    //Matrix rot = Matrix.CreateFromYawPitchRoll(rotation.X, rotation.Y, rotation.Z) 
            //    //    * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            //    //position += Vector3.Transform(Vector3.Zero, rot);
            //    sinceLastUpdate = 0;
            //}


            base.update(gameTime);
        }
    }
}
