﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkinnedModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame
{
    public class AnimatedModel : CModel
    {
        protected SkinnedAnimationPlayer animator;


        public AnimatedModel(Game1 game,SkinningData skinningData, Model model,Unit unit)
            : base(game,model,unit)
        {
            animator = new SkinnedAnimationPlayer(skinningData);
            setNewEffect();
        }

        public override void Draw(GameTime gameTime)
        {
            updateBaseWorld(unit.position, unit.rotation, unit.scale, unit.baseWorld);

            foreach (ModelMesh mesh in Model.Meshes)
            {
                foreach (SkinnedEffect effect in mesh.Effects)
                {
                    effect.SetBoneTransforms(animator.SkinTransforms);
                    effect.View = ((Game1)Game).camera.View;
                    effect.Projection = ((Game1)Game).camera.Projection;
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                }

                mesh.Draw();
            }
            base.Draw(gameTime);
            TimeSpan elapsedTime = gameTime.ElapsedGameTime;

            animator.Update(elapsedTime, this.baseWorld);
        }


        protected void setNewEffect()
        {
            foreach (ModelMesh mesh in Model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    if (!(part.Effect is SkinnedEffect))
                    {
                        SkinnedEffect newEffect = new SkinnedEffect(Game.GraphicsDevice);

                        newEffect.EnableDefaultLighting();
                        newEffect.SpecularColor = Color.Black.ToVector3();

                        BasicEffect oldEffect = ((BasicEffect)part.Effect);
                        newEffect.AmbientLightColor = oldEffect.AmbientLightColor;
                        newEffect.DiffuseColor = oldEffect.DiffuseColor;
                        newEffect.Texture = oldEffect.Texture;

                        part.Effect = newEffect;
                    }
                }
            }

        }
    }
}
