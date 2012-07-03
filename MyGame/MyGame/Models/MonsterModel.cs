using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public bool isRunning = true;

        public enum MonsterAnimations
        {
            Idle = 0,
            Run ,
            Bite,
            TakeDamage,
            Die,

        }

        public MonsterModel(MyGame game, SkinnedModel skinnedModel)
            : base(game, skinnedModel)
        {

            animationController.StartClip(skinnedModel.AnimationClips[animations[(int)MonsterAnimations.Idle]]);
            //animationController.CrossFade(skinnedModel.AnimationClips.Values[0], TimeSpan.FromSeconds(0.05f));
        }

        public override void reinitialize(SkinnedModel skinnedModel)
        {
 	         base.reinitialize(skinnedModel);
             animationController.StartClip(skinnedModel.AnimationClips[animations[(int)MonsterAnimations.Idle]]);
        }

        public void Idle()
        {
            DoAction(true,MonsterAnimations.Idle);
        }

        public void Run()
        {
            DoAction(true, MonsterAnimations.Run);
        }

        public void Bite()
        {
            DoAction(false, MonsterAnimations.Bite);
        }

        public void TakeDamage()
        {
            DoAction(false, MonsterAnimations.TakeDamage);
        }

        public void Die()
        {
            DoAction(false, MonsterAnimations.Die);
        }

        private void DoAction(bool LoopEnabled,MonsterAnimations anim)
        {
            animationController.LoopEnabled = LoopEnabled;
            animationController.StartClip(skinnedModel.AnimationClips[animations[(int)anim]]);
            activeAnimation = anim;

        }
    }
}
