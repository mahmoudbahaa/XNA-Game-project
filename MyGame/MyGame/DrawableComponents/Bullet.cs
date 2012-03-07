using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkinnedModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Helper;
using control;

namespace MyGame
{
    public class Bullet : CDrawableComponent
    {
        public Bullet(Game1 game, Model model, Unit unit)
            : base(game, unit,new CModel(game, model))
        {
        }
    }
}
