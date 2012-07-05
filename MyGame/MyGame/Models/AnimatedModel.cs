using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XNAnimation;
using XNAnimation.Controllers;

namespace MyGame
{
    /// <summary>
    /// This class represent animated model (A model that has animation(s))
    /// </summary>
    public class AnimatedModel : CModel
    {
        protected SkinnedModel skinnedModel;
        public AnimationController animationController;


        public AnimatedModel(MyGame game,SkinnedModel skinnedModel)
            : base(game,skinnedModel.Model)
        {
            reinitialize2(skinnedModel);
        }

        private void reinitialize2(SkinnedModel skinnedModel)
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

        public virtual void reinitialize(SkinnedModel skinnedModel)
        {
            base.reinitialize(skinnedModel.Model);
            reinitialize2(skinnedModel);
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
        }
    }
}
