using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MyGame
{
    class Tree : CDrawableComponent
    {
        public Tree(MyGame game, Model model, Unit unit, Texture2D Texture)
            :base(game,unit,new CModel(game, model))
        {
        }
    }
}
