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
    class MonsterModel : AnimatedModel
    {

        readonly string[] animations = new string[] { "Die", "Run" };

        private SkinnedModel dieSkinnedModel;

        public enum monsteranimations
        {
            die = 0,
            run = 1
        }

        public MonsterModel(Game1 game, SkinnedModel runSkinnedModel, SkinnedModel dieSkinnedModel)
            : base(game, runSkinnedModel)
        {
            this.dieSkinnedModel = dieSkinnedModel;
            animationController.StartClip(skinnedModel.AnimationClips["Run"]);
            //animationController.CrossFade(skinnedModel.AnimationClips.Values[0], TimeSpan.FromSeconds(0.05f));
        }

        public void Die()
        {
            skinnedModel = dieSkinnedModel;
            animationController.LoopEnabled = false;
            animationController.StartClip(skinnedModel.AnimationClips[animations[(int)monsteranimations.die]]);
        }
    }
}
