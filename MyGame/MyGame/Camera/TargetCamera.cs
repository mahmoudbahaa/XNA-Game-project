using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame
{
    /// <summary>
    /// This class represent a Target camera that is a basic camera
    /// </summary>
    public class TargetCamera : Camera
    {
        public TargetCamera(MyGame game,Vector3 Position, Vector3 Target)
            : base(game)
        {
            this.Position = Position;
            this.Target = Target;
        }

        /// <summary>
        /// Allows the component to run logic.
        /// </summary>
        /// <param name="gameTime">The gametime.</param>
        public void Update()
        {
            this.View = Matrix.CreateLookAt(Position, Target, Vector3.Up);
        }
    }
}
