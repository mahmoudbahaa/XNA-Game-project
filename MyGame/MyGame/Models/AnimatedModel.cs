﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XNAnimation;
using XNAnimation.Controllers;

namespace MyGame
{
    public class AnimatedModel : CModel
    {
        protected SkinnedModel skinnedModel;
        public AnimationController animationController;


        public AnimatedModel(Game1 game,SkinnedModel skinnedModel)
            : base(game,skinnedModel.Model)
        {
            this.skinnedModel = skinnedModel;
            //setNewEffect();

            // Create an animation controller and start a clip
            animationController = new AnimationController(skinnedModel.SkeletonBones);
            animationController.Speed = 0.5f;

            animationController.TranslationInterpolation = InterpolationMode.Linear;
            animationController.OrientationInterpolation = InterpolationMode.Linear;
            animationController.ScaleInterpolation = InterpolationMode.Linear;

            //animationController.StartClip(skinnedModel.AnimationClips[animation]);
        }

        public override void Draw(GameTime gameTime)
        {
            myGame.GraphicsDevice.BlendState = BlendState.Opaque;
            myGame.GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            myGame.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            foreach (ModelMesh mesh in skinnedModel.Model.Meshes)
            {
                foreach (SkinnedEffect effect in mesh.Effects)
                {
                    effect.SetBoneTransforms(animationController.SkinnedBoneTransforms);
                    effect.World = baseWorld;

                    effect.View = myGame.camera.View;
                    effect.Projection = myGame.camera.Projection;
                }

                mesh.Draw();
            }

            //foreach (ModelMesh mesh in Model.Meshes)
            //{
            //    foreach (SkinnedEffect effect in mesh.Effects)
            //    {
            //        effect.SetBoneTransforms(animator.SkinTransforms);
            //        effect.View = myGame.camera.View;
            //        effect.Projection = myGame.camera.Projection;
            //        effect.EnableDefaultLighting();
            //        effect.PreferPerPixelLighting = true;
            //    }

            //    mesh.Draw();
            //}
            //base.Draw(gameTime);
            //TimeSpan elapsedTime = gameTime.ElapsedGameTime;

            //animator.Update(elapsedTime, this.baseWorld);
        }


        //protected void setNewEffect()
        //{
        //    foreach (ModelMesh mesh in Model.Meshes)
        //    {
        //        foreach (ModelMeshPart part in mesh.MeshParts)
        //        {
        //            if (!(part.Effect is SkinnedEffect))
        //            {
        //                SkinnedEffect newEffect = new SkinnedEffect(myGame.GraphicsDevice);

        //                newEffect.EnableDefaultLighting();
        //                newEffect.SpecularColor = Color.Black.ToVector3();

        //                BasicEffect oldEffect = ((BasicEffect)part.Effect);
        //                newEffect.AmbientLightColor = oldEffect.AmbientLightColor;
        //                newEffect.DiffuseColor = oldEffect.DiffuseColor;
        //                newEffect.Texture = oldEffect.Texture;

        //                part.Effect = newEffect;
        //            }
        //        }
        //    }

        //}
    }
}
