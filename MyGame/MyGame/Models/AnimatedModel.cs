using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DigitalRune.Animation;
using DigitalRune.Animation.Character;
using DigitalRune.Geometry;
using DigitalRune.Mathematics;
using DigitalRune.Mathematics.Algebra;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//using XNAnimation;
//using XNAnimation.Controllers;

namespace MyGame
{
    public class AnimatedModel : CModel
    {
        public Pose _pose = new Pose(Vector3F.Zero);
        public SkeletonPose _skeletonPose;
        protected Skeleton skeleton;
        protected AnimationController _animationController;
        protected IAnimationService AnimationService { get; set; }

        protected Dictionary<string, SkeletonKeyFrameAnimation> theAnimations;
        public AnimatedModel(Game1 game,Model skinnedModel)
            : base(game,skinnedModel)
        {
            AnimationService = (IAnimationService)game.Services.GetService(typeof(IAnimationService));

            //setNewEffect();

            // Create an animation controller and start a clip
            //animationController = new AnimationController(skinnedModel.SkeletonBones);
            //animationController.Speed = 0.5f;

            //animationController.TranslationInterpolation = InterpolationMode.Linear;
            //animationController.OrientationInterpolation = InterpolationMode.Linear;
            //animationController.ScaleInterpolation = InterpolationMode.Linear;

            //animationController.StartClip(skinnedModel.AnimationClips[animation]);


            var additionalData = (Dictionary<string, object>)Model.Tag;

            skeleton = (Skeleton)additionalData["Skeleton"];
            _skeletonPose = SkeletonPose.Create(skeleton);

            // Get the animations from the additional data.
            theAnimations = (Dictionary<string, SkeletonKeyFrameAnimation>)additionalData["Animations"];

            // The Dude model contains only one animation, which is a SkeletonKeyFrameAnimation with 
            // a walk cycle.
            SkeletonKeyFrameAnimation walkAnimation = theAnimations.Values.First();

            // Wrap the walk animation in an animation clip that loops the animation forever.
            AnimationClip<SkeletonPose> loopingAnimation = new AnimationClip<SkeletonPose>(walkAnimation)
            {
                LoopBehavior = LoopBehavior.Cycle,
                Duration = TimeSpan.MaxValue,
            };

            // Start the animation and keep the created AnimationController.
            // We must cast the SkeletonPose to IAnimatableProperty because SkeletonPose implements
            // IAnimatableObject and IAnimatableProperty. We must tell the AnimationService if we want
            // to animate an animatable property of the SkeletonPose (IAnimatableObject), or if we want to
            // animate the whole SkeletonPose (IAnimatableProperty).
            _animationController = AnimationService.StartAnimation(loopingAnimation, (IAnimatableProperty)_skeletonPose);

            // The animation will be applied the next time AnimationManager.ApplyAnimations() is called
            // in the mainloop. ApplyAnimations() is called before this method is called, therefore
            // the model will be rendered in the bind pose in this frame and in the first animation key
            // frame in the next frame - this creates an annoying visual popping effect. 
            // We can avoid this if we call AnimationController.UpdateAndApply(). This will immediately
            // change the model pose to the first key frame pose.
            _animationController.UpdateAndApply();

            // (Optional) Enable Auto-Recycling: 
            // After the animation is stopped, the animation service will recycle all
            // intermediate data structures. 
            _animationController.AutoRecycle();
        }

        public override void Draw(GameTime gameTime)
        {
            myGame.GraphicsDevice.BlendState = BlendState.Opaque;
            myGame.GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            myGame.GraphicsDevice.DepthStencilState = DepthStencilState.Default;


            //GroundModel.Draw(Matrix.Identity, myGame.camera.View, myGame.camera.Projection);
            DrawSkinnedModel(Model, _pose, _skeletonPose);

            base.Draw(gameTime);
            //foreach (ModelMesh mesh in skinnedModel.Model.Meshes)
            //{
            //    foreach (SkinnedEffect effect in mesh.Effects)
            //    {
            //        effect.SetBoneTransforms(animationController.SkinnedBoneTransforms);
            //        effect.World = baseWorld;

            //        effect.View = myGame.camera.View;
            //        effect.Projection = myGame.camera.Projection;
            //    }

            //    mesh.Draw();
            //}
        }

        // Helper method that draws a model with SkinnedEffects.
        protected void DrawSkinnedModel(Model model, Pose pose, SkeletonPose skeletonPose)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (SkinnedEffect effect in mesh.Effects)
                {
                    // SkeletonPose.SkinningMatricesXna provides an array of transformations as needed
                    // by the SkinnedEffect.
                    effect.SetBoneTransforms(skeletonPose.SkinningMatricesXna);

                    // The world space transformation.
                    effect.World = baseWorld * pose;

                    // Camera transformation.
                    effect.View = myGame.camera.View;
                    effect.Projection = myGame.camera.Projection;

                    // Lighting.
                    effect.EnableDefaultLighting();
                    effect.SpecularColor = new Vector3(0.25f);
                    effect.SpecularPower = 16;
                }

                mesh.Draw();
            }
        }
    }
}
