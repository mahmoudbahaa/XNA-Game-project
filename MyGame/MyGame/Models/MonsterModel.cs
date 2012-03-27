using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DigitalRune.Animation;
using DigitalRune.Animation.Character;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Helper;
using XNAnimation;

namespace MyGame
{
    public class MonsterModel : AnimatedModel
    {
        readonly string[] animations = new string[] { "Idle", "Run", "Bite", "Take Damage", "Die", };

        public MonsterAnimations activeAnimation;
        public float countDown;
        public bool isRunning = true;

        public enum MonsterAnimations
        {
            Idle = 0,
            Run,
            Bite,
            TakeDamage,
            Die,

        }

        public MonsterModel(Game1 game, Model skinnedModel)
            : base(game, skinnedModel)
        {

            //animationController.StartClip(skinnedModel.AnimationClips[animations[(int)MonsterAnimations.Idle]]);
            //animationController.CrossFade(skinnedModel.AnimationClips.Values[0], TimeSpan.FromSeconds(0.05f));
        }

        public void Idle()
        {
            DoAction(true, MonsterAnimations.Idle);
        }

        public void Run()
        {
            DoAction(true, MonsterAnimations.Run);
        }

        public void Bite()
        {
            DoAction(false, MonsterAnimations.Bite);
            countDown = 5 * theAnimations[animations[(int)activeAnimation]].GetTotalDuration().Milliseconds;
        }

        public void TakeDamage()
        {
            DoAction(false, MonsterAnimations.TakeDamage);
            countDown = 5 * theAnimations[animations[(int)activeAnimation]].GetTotalDuration().Milliseconds;
        }

        public void Die()
        {
            DoAction(false, MonsterAnimations.Die);
        }

        private void DoAction(bool loop, MonsterAnimations anim)
        {
            activeAnimation = anim;

            // The Dude model contains only one animation, which is a SkeletonKeyFrameAnimation with 
            // a walk cycle.
            SkeletonKeyFrameAnimation animation = theAnimations[animations[(int)activeAnimation]];
            // Wrap the walk animation in an animation clip that loops the animation forever.
            AnimationClip<SkeletonPose> loopingAnimation = new AnimationClip<SkeletonPose>(animation)
            {
                LoopBehavior = loop ? LoopBehavior.Cycle : LoopBehavior.Constant,
                Duration = loop ? TimeSpan.MaxValue : animation.GetTotalDuration(),
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
            //if(countDown)
            base.Draw(gameTime);
        }
    }
}
