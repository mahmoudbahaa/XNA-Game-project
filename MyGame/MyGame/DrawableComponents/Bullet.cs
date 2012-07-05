using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Helper;
using control;

namespace MyGame
{
    /// <summary>
    /// This class represent a bullet
    /// </summary>
    public class Bullet : CDrawableComponent
    {
        public Bullet(MyGame game, Model model, Unit unit)
            : base(game, unit,new CModel(game, model))
        {
        }
    }
}
