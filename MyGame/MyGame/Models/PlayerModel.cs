using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XNAnimation;
using Helper;

namespace MyGame
{
    public class PlayerModel : AnimatedModel
    {
        private bool running;
        private bool attacking;
        public bool shooting = false;

        //private int activeAnimationClip;

        static int RIGHT_HAND_BONE_ID = 15;

        SkinnedModel idleSkinnedModel;
        SkinnedModel runSkinnedModel;
        SkinnedModel aimSkinnedModel;
        SkinnedModel shootSkinnedModel;

        public PlayerModel(Game1 game, SkinnedModel idleSkinnedModel, SkinnedModel runSkinnedModel,
                                    SkinnedModel aimSkinnedModel, SkinnedModel shootSkinnedModel)
            : base(game, idleSkinnedModel)
        {
            this.idleSkinnedModel = idleSkinnedModel;
            this.runSkinnedModel = runSkinnedModel;
            this.aimSkinnedModel = aimSkinnedModel;
            this.shootSkinnedModel = shootSkinnedModel;

            game.mediator.register(this, MyEvent.P_RUN, MyEvent.C_ATTACK_BULLET_BEGIN);
            animationController.Speed = 1.2f;
            animationController.StartClip(skinnedModel.AnimationClips.Values[0]);
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
            Attack();
            events.Clear();

            if (!myGame.firstPerson)
                base.Draw(gameTime);
            
        }

        private void Attack()
        {
            if (attacking)
            {
                if (animationController.IsPlaying && skinnedModel != aimSkinnedModel 
                    && skinnedModel != shootSkinnedModel)
                {
                    skinnedModel = aimSkinnedModel;
                    Model = aimSkinnedModel.Model;
                    animationController.LoopEnabled = false;
                    playAnimation();
                }
                else if (!animationController.IsPlaying && skinnedModel == aimSkinnedModel)
                {
                    skinnedModel = shootSkinnedModel;
                    Model = shootSkinnedModel.Model;
                    animationController.LoopEnabled = false;
                    playAnimation();
                }
                else if(!animationController.IsPlaying && skinnedModel == shootSkinnedModel)
                {
                    attacking = false;
                    shooting = true;
                }
            }
        }

        //public void AttackByAxe(PlayerAnimations playerAnimation)
        //{
        //    animationController.LoopEnabled = false;
        //    activeAnimationClip = (int)playerAnimation;
        //    playAnimation();
        //}
       
        private void Run()
        {
            if (running)
            {
                if (skinnedModel != runSkinnedModel && !attacking)
                {
                    skinnedModel = runSkinnedModel;
                    Model = runSkinnedModel.Model;
                    animationController.LoopEnabled = true;
                    playAnimation();
                }
                //if (activeAnimationClip != (int)PlayerAnimations.RunWithPistol)
                //{
                //    animationController.LoopEnabled = true;
                //    activeAnimationClip = (int)PlayerAnimations.RunWithPistol;
                //    playAnimation();
                //}
            }
            else
            {
                if (skinnedModel != idleSkinnedModel && !attacking)
                {
                    skinnedModel = idleSkinnedModel;
                    Model = idleSkinnedModel.Model;
                    animationController.LoopEnabled = true;
                    playAnimation();
                }
                //if (activeAnimationClip == (int)PlayerAnimations.Run || !animationController.IsPlaying)
                //{
                //    //animationController.LoopEnabled = false;
                //    //animationController.LoopEnabled
                //    activeAnimationClip = (int)PlayerAnimations.LookBlowKiss;
                //    playAnimation();

                //}
            }
        }

        public Matrix RHandTransformation()
        {
            return Model.Bones["R_Hand"].Transform
                * animationController.SkinnedBoneTransforms[RIGHT_HAND_BONE_ID];
               //* animationController.SkinnedBoneTransforms[0];
        }

        private void playAnimation()
        {
            animationController.StartClip(skinnedModel.AnimationClips.Values[0]);
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

        public enum PlayerAnimations
        {
            LookBlowKiss = 1,
            ReadyWithRifle,
            ReadyWithPistol,
            SlashWithBlade,
            Walk,
            WalkWithWeapon,
            SneakWithRifle,
            StalkWithPistol,
            OverheadSlashWithBlade,
            Run,
            RunWithRifleSafe,
            RunWithRifleAimed,
            RunWithPistol,
            RunningSlashWithBlade,
            Crouch,
            CrouchedRifle,
            CrouchedPistol,
            CrouchedStabWithBlade,
            CrouchedStalk,
            CrouchedStalkWithRifle,
            CrouchedStalkWithPistol,
            CrouchedStalkStabWithBlade,
            FreeFall,
            FreeFallWithPistol,
            FreeFallWithRifle,
            FreeFallSlashWithBlade,
            ClimbLadder,
            JumpLand,
            DieStanding,
            DieFalling,
            DieCrouched
        }

        //readonly String[] animations = new String[] { "Animation0" };

        //public enum PlayAnimations
        //{
        //    Run = 0
        //}

    }
}
