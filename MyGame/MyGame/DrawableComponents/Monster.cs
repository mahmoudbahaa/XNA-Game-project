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
    public class Monster : CDrawableComponent
    {
        public Monster(Game1 game, SkinnedModel runSkinnedModel, SkinnedModel dieSkinnedModel, Unit unit)
            : base(game, unit, new MonsterModel(game, runSkinnedModel, dieSkinnedModel))
        {
            foreach (ModelMesh mesh in runSkinnedModel.Model.Meshes)
                foreach (SkinnedEffect effect in mesh.Effects)
                    effect.EnableDefaultLighting();

            foreach (ModelMesh mesh in dieSkinnedModel.Model.Meshes)
                foreach (SkinnedEffect effect in mesh.Effects)
                    effect.EnableDefaultLighting();
        }

        public override void Update(GameTime gameTime)
        {
            ((AnimatedModel)cModel).animationController.Update(gameTime.ElapsedGameTime, Matrix.Identity);

            Vector3 pos = unit.position;
            unit.position.Y = myGame.GetHeightAtPosition(pos.X, pos.Z);
            base.Update(gameTime);
        }
    }
}
