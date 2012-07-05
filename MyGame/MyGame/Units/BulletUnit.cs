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
    /// This class represent bullet unit which represent the logic of the bullet 
    /// that is it move with constant speed in a certain direction
    /// </summary>
    public class BulletUnit : Unit
    {
        Vector3 Direction { get; set; }

        public BulletUnit(MyGame game,Vector3 Position, Vector3 Rotation, Vector3 Scale,Vector3 Direction)
            : base(game,Position, Rotation, Scale)
        {
            this.Direction = Direction;            
        }

        public override void update(GameTime gameTime)
        {
            // Move bullet
            position += Direction * Constants.BULLET_SPEED;
            base.update(gameTime);
        }
    }
}
