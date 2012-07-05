using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MyGame
{
    /// <summary>
    /// This class represent a Tree
    /// </summary>
    class Tree : CDrawableComponent
    {
        public Tree(MyGame game, Model model, Unit unit, Texture2D Texture)
            :base(game,unit,new CModel(game, model))
        {
        }
    }
}
