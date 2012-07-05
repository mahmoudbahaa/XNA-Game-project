using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Helper;
using control;
using XNAnimation;

namespace MyGame
{
    /// <summary>
    /// This class represent a first aid item that rotates with time
    /// </summary>
    public class FirstAid : CDrawableComponent
    {
        public FirstAid(MyGame game, Model model, Unit unit)
            : base(game, unit, new CModel(game,model))
        {
        }

        public override void Update(GameTime gameTime)
        {
            unit.rotation += new Vector3(0, MathHelper.Pi / 80, 0);

            base.Update(gameTime);
        }
    }
}
