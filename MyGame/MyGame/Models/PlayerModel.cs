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
        readonly String[] animations = new String[] { "Idle", "Run", "Aim", "Shoot" };

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

        static int RIGHT_HAND_BONE_ID = 15;

        public PlayerModel(Game1 game, SkinnedModel skinnedModel)
            : base(game, skinnedModel)
        {
            game.mediator.register(this, MyEvent.P_RUN, MyEvent.C_ATTACK_BULLET_BEGIN);
            animationController.Speed = 1.2f;
            activeAnimation = PlayerAnimations.Idle;
            playAnimation();    
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

            if (myGame.cameraMode == Game1.CameraMode.thirdPerson)
                base.Draw(gameTime);
            
        }

        private void Attack()
        {
            if (attacking)
            {
                if (animationController.IsPlaying && activeAnimation != PlayerAnimations.Aim &&
                    activeAnimation != PlayerAnimations.Shoot)
                {
                    activeAnimation = PlayerAnimations.Aim ;
                    animationController.LoopEnabled = false;
                    playAnimation();
                }
                else if (!animationController.IsPlaying && activeAnimation == PlayerAnimations.Aim)
                {
                    activeAnimation = PlayerAnimations.Shoot;
                    animationController.LoopEnabled = false;
                    playAnimation();
                }
                else if(!animationController.IsPlaying && activeAnimation == PlayerAnimations.Shoot)
                {
                    attacking = false;
                    shooting = true;
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
                    animationController.LoopEnabled = true;
                    playAnimation();
                }
            }
            else
            {
                if (activeAnimation != PlayerAnimations.Idle && !attacking)
                {
                    activeAnimation = PlayerAnimations.Idle;
                    animationController.LoopEnabled = true;
                    playAnimation();
                }
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
            animationController.StartClip(skinnedModel.AnimationClips[animations[(int)activeAnimation]]);
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
