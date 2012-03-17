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
        //readonly string[] animations = new string[] { "Idle", "Run", "Bite", "Take Damage", "Die", };

        SkinnedModel idleSkinnedModel;
        SkinnedModel runSkinnedModel;
        SkinnedModel biteSkinnedModel;
        SkinnedModel takeDamageSkinnedModel;
        SkinnedModel dieSkinnedModel;

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

        public MonsterModel(Game1 game, SkinnedModel idleSkinnedModel, SkinnedModel runSkinnedModel,
            SkinnedModel biteSkinnedModel, SkinnedModel takeDamageSkinnedModel, SkinnedModel dieSkinnedModel)
            : base(game, idleSkinnedModel)
        {
            this.idleSkinnedModel       = idleSkinnedModel;
            this.runSkinnedModel        = runSkinnedModel;
            this.biteSkinnedModel       = biteSkinnedModel;
            this.takeDamageSkinnedModel = takeDamageSkinnedModel;
            this.dieSkinnedModel        = dieSkinnedModel;

            animationController.StartClip(skinnedModel.AnimationClips.Values[0]);
            //animationController.CrossFade(skinnedModel.AnimationClips.Values[0], TimeSpan.FromSeconds(0.05f));
        }

        public void Idle()
        {
            DoAction(idleSkinnedModel, true,MonsterAnimations.Idle);
        }

        public void Run()
        {
            DoAction(runSkinnedModel, true, MonsterAnimations.Run);
        }

        public void Bite()
        {
            DoAction(biteSkinnedModel, false, MonsterAnimations.Bite);
        }

        public void TakeDamage()
        {
            DoAction(takeDamageSkinnedModel, false, MonsterAnimations.TakeDamage);
        }

        public void Die()
        {
            DoAction(dieSkinnedModel, false, MonsterAnimations.Die);
        }

        private void DoAction(SkinnedModel newSkinnedModel, bool LoopEnabled,MonsterAnimations anim)
        {
            skinnedModel = newSkinnedModel;
            animationController.LoopEnabled = LoopEnabled;
            animationController.StartClip(skinnedModel.AnimationClips.Values[0]);
            activeAnimation = anim;

        }
    }
}
