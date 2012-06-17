using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MyGame
{
    class Sky : CDrawableComponent
    {
        public Sky(MyGame game, Model model, Unit unit, Texture2D Texture)
            :base(game,unit,new SkyModel(game, model,Texture))
        {
        }
    }
}
