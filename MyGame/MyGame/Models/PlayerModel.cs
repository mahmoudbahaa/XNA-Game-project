using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DigitalRune.Animation;
using DigitalRune.Animation.Character;
using XNAnimation;
using Helper;

namespace MyGame
{
    public class PlayerModel : AnimatedModel
    {
        readonly String[] animations = new String[] { "Idle", "Run", "Aim", "Shoot" };

        float countdown;

        public enum PlayerAnimations
        {
            Idle = 0,
            Run,
            Aim,
            Shoot,
        }

        private bool running;
        private bool attacking;
        private PlayerAnimations activeAnimation;
        public bool shooting = false;

        //private int activeAnimationClip;

        //static int RIGHT_HAND_BONE_ID = 15;

        public PlayerModel(Game1 game, Model skinnedModel)
            : base(game, skinnedModel)
        {
            game.mediator.register(this, MyEvent.P_RUN, MyEvent.C_ATTACK_BULLET_BEGIN);
            //animationController.Speed = 1.2f;

            activeAnimation = PlayerAnimations.Idle;
            playAnimation(true);    
        }

        public override void Draw(GameTime gameTime)
        {
            running = false;
            foreach (Event ev in events)
            {
                switch (ev.EventId)
                {
                    case (int)MyEvent.P_RUN:
                        running = true;
                        break;
                    case (int) MyEvent.C_ATTACK_BULLET_BEGIN:
                        attacking = true;
                        break;
                    default:
                        break;
                }
            }
            Run();
            Attack(gameTime);
            events.Clear();

            if (myGame.cameraMode == Game1.CameraMode.thirdPerson)
                base.Draw(gameTime);
            
        }

        private void Attack(GameTime gameTime)
        {
            if (attacking)
            {
                if (AnimationService.IsAnimated((IAnimatableProperty)_skeletonPose) && activeAnimation != PlayerAnimations.Aim &&
                    activeAnimation != PlayerAnimations.Shoot)
                {
                    activeAnimation = PlayerAnimations.Aim;
                    playAnimation(false);
                    countdown = theAnimations[animations[(int)activeAnimation]].GetTotalDuration().Milliseconds;
                }
                else if (countdown > 0 && activeAnimation == PlayerAnimations.Aim)
                {
                    countdown -= gameTime.ElapsedGameTime.Milliseconds;
                    if (countdown < 0)
                    {
                        activeAnimation = PlayerAnimations.Shoot;
                        playAnimation(false);
                        countdown = theAnimations[animations[(int)activeAnimation]].GetTotalDuration().Milliseconds;
                    }
                }
                else if (countdown > 0  && activeAnimation == PlayerAnimations.Shoot)
                {
                    countdown -= gameTime.ElapsedGameTime.Milliseconds;
                    if (countdown < 0)
                    {
                        attacking = false;
                        shooting = true;
                    }
                }
            }
        }
       
        private void Run()
        {
            if (running)
            {
                if (activeAnimation != PlayerAnimations.Run && !attacking)
                {
                    activeAnimation = PlayerAnimations.Run;
                    playAnimation(true);
                }
            }
            else
            {
                if (activeAnimation != PlayerAnimations.Idle && !attacking)
                {
                    activeAnimation = PlayerAnimations.Idle;
                    playAnimation(true);
                }
            }
        }

        public Matrix RHandTransformation()
        {
              int handBoneIndex = _skeletonPose.Skeleton.GetIndex("R_Hand2");
              return _skeletonPose.SkinningMatricesXna[handBoneIndex];
        }

        private void playAnimation(bool loop)
        {
            // The Dude model contains only one animation, which is a SkeletonKeyFrameAnimation with 
            // a walk cycle.
            SkeletonKeyFrameAnimation animation = theAnimations[animations[(int)activeAnimation]];

            // Wrap the walk animation in an animation clip that loops the animation forever.
            AnimationClip<SkeletonPose> loopingAnimation = new AnimationClip<SkeletonPose>(animation)
            {
                LoopBehavior = loop?LoopBehavior.Cycle:LoopBehavior.Constant,
                Duration = loop ? TimeSpan.MaxValue : animation.GetTotalDuration(),
            };

            if (activeAnimation == PlayerAnimations.Shoot || activeAnimation == PlayerAnimations.Aim)
            {
                // The SkeletonKeyFrameAnimations allows to set a weight for each bone channel. 
                // For the 'Shoot' animation, we set the weight to 0 for all bones that are 
                // not descendents of the second spine bone (bone index 2). That means, the 
                // animation affects only the upper body bones and is disabled on the lower 
                // body bones.
                for (int i = 0; i < skeleton.NumberOfBones; i++)
                {
                    if(i>15 || i==1)
                    //if (!SkeletonHelper.IsAncestorOrSelf(_skeletonPose,_skeletonPose.Skeleton.GetIndex("spine2") , i))
                        animation.SetWeight(i, 0);
                }

                AnimationService.StartAnimation(animation, (IAnimatableProperty)_skeletonPose,
                                        AnimationTransitions.Compose()).AutoRecycle();
            }

            else
            {
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
            //animationController.StartClip(skinnedModel.AnimationClips[animations[(int)activeAnimation]]);
            //animationController.CrossFade(skinnedModel.AnimationClips.Values[activeAnimationClip], TimeSpan.FromSeconds(0.5f));
        }


        //readonly String[] animations = new String[] {   "Take 001" ,
        //                                                "Walk",
        //                                                "Run",
        //                                                "Jump",
        //                                                "Jump2",
        //                                                "CrouchDown",
        //                                                "Crouch",
        //                                                "GetUp",
        //                                                "BattleIdle1",
        //                                                "BattleIdle2",
        //                                                "Attack1",
        //                                                "Attack2",
        //                                                "Attack3",
        //                                                "Attack4",
        //                                                "Attack5",
        //                                                "Block",
        //                                                "Die1",
        //                                                "Die2",
        //                                                "Yes",
        //                                                "No",
        //                                                "Idle1",
        //                                                "Idle2"
        //};

        //public enum PlayerAnimations
        //{
        //    Take001 = 0,
        //    Walk,
        //    Run,
        //    Jump,
        //    Jump2,
        //    CrouchDown,
        //    Crouch,
        //    GetUp,
        //    BattleIdle1,
        //    BattleIdle2,
        //    Attack1,
        //    Attack2,
        //    Attack3,
        //    Attack4,
        //    Attack5,
        //    Block,
        //    Die1,
        //    Die2,
        //    Yes,
        //    No,
        //    Idle1,
        //    Idle2
        //}

        //readonly String[] animations = new String[] {   "",
        //                                                "Look Blow Kiss",
        //                                                "Ready With Rifle",
        //                                                "Ready With Pistol",
        //                                                "Slash With Blade",
        //                                                "Walk",
        //                                                "Walk With Weapon",
        //                                                "Sneak With Rifle",
        //                                                "Stalk With Pistol",
        //                                                "Overhead Slash With Blade",
        //                                                "Run",
        //                                                "Run With Rifle Safe",
        //                                                "Run With Rifle Aimed",
        //                                                "Run With Pistol",
        //                                                "Running Slash With Blade",
        //                                                "Crouch",
        //                                                "Crouched Rifle",
        //                                                "Crouched Pistol",
        //                                                "Crouched Stab With Blade",
        //                                                "Crouched Stalk",
        //                                                "Crouched Stalk With Rifle",
        //                                                "Crouched Stalk With Pistol",
        //                                                "Crouched Stalk Stab With Blade",
        //                                                "Free Fall",
        //                                                "Free Fall With Pistol",
        //                                                "Free Fall With Rifle",
        //                                                "Free Fall Slash With Blade",
        //                                                "Climb Ladder",
        //                                                "Jump Land",
        //                                                "Die Standing",
        //                                                "Die Falling",
        //                                                "Die Crouched"
        //                                    };

        //public enum PlayerAnimations
        //{
        //    LookBlowKiss = 1,
        //    ReadyWithRifle,
        //    ReadyWithPistol,
        //    SlashWithBlade,
        //    Walk,
        //    WalkWithWeapon,
        //    SneakWithRifle,
        //    StalkWithPistol,
        //    OverheadSlashWithBlade,
        //    Run,
        //    RunWithRifleSafe,
        //    RunWithRifleAimed,
        //    RunWithPistol,
        //    RunningSlashWithBlade,
        //    Crouch,
        //    CrouchedRifle,
        //    CrouchedPistol,
        //    CrouchedStabWithBlade,
        //    CrouchedStalk,
        //    CrouchedStalkWithRifle,
        //    CrouchedStalkWithPistol,
        //    CrouchedStalkStabWithBlade,
        //    FreeFall,
        //    FreeFallWithPistol,
        //    FreeFallWithRifle,
        //    FreeFallSlashWithBlade,
        //    ClimbLadder,
        //    JumpLand,
        //    DieStanding,
        //    DieFalling,
        //    DieCrouched
        //}

        //readonly String[] animations = new String[] { "Animation0" };

        //public enum PlayAnimations
        //{
        //    Run = 0
        //}

    }
}
