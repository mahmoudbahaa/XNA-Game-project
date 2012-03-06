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
        private const int updateDelay = 1000;
        //private int sinceLastUpdate = 0;

        public SkyUnit(Game1 game,Vector3 Position, Vector3 Rotation, Vector3 Scale)
            : base(game,Position, Rotation, Scale)
        {
            //RotationSpeed = 20f;
        }

        public override void update(GameTime gameTime)
        {
            base.update(gameTime);
            // Move the model with the sphere
            //position = game.camera.Position;
            //if(sinceLastUpdate > updateDelay)
            //{
            //    rotation.Y += RotationSpeed*.025f;
            //    Matrix rot = Matrix.CreateFromYawPitchRoll(rotation.X, rotation.Y, rotation.Z) 
            //        * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            //    position += Vector3.Transform(Vector3.Zero, rot);
            //    sinceLastUpdate = 0;
            //}
            //else
            //{
            //    sinceLastUpdate += gameTime.ElapsedGameTime.Milliseconds;
            //}
        }
    }
}
