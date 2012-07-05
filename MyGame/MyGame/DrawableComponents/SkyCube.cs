using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MyGame
{
    /// <summary>
    /// This class represent Sky Cube that is used to render the sky
    /// </summary>
    class SkyCube : CDrawableComponent, IRenderable
    {
        public SkyCube(MyGame game, Model model, Unit unit, TextureCube Texture)
            :base(game,unit,new SkyCubeModel(game, model,Texture))
        {
        }

        public void SetClipPlane(Vector4? Plane)
        {
            ((SkyCubeModel)cModel).effect.Parameters["ClipPlaneEnabled"].SetValue(Plane.HasValue);

            if (Plane.HasValue)
                ((SkyCubeModel)cModel).effect.Parameters["ClipPlane"].SetValue(Plane.Value);
        }

        public void Draw()
        {
            Draw(null);
        }

    }
}
