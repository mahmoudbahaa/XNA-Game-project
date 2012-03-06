using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkinnedModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Helper;

namespace MyGame
{
    class MonsterModel : AnimatedModel
    {

        readonly String[] animations = new String[] { "Die" , "Run"};

        //SkinningData runSkinningData ;
        SkinningData dieSkinningData ;

        public enum MonsterAnimations
        {
            Die = 0,
            Run = 1
        }

        public MonsterModel(Game1 game,SkinningData runSkinningData,SkinningData dieSkinningData, Model model, Unit unit)
            : base(game, runSkinningData, model, unit)
        {
            //this.runSkinningData = runSkinningData;
            this.dieSkinningData = dieSkinningData;
            
            
            animator.running = true;
            animator.StartClip(animations[(int)MonsterAnimations.Run], true);
        }

        public void Die()
        {
            animator.skinningData = dieSkinningData;
            unit.alive = false;
            animator.StartClip(animations[(int)MonsterAnimations.Die], false);
        }
    }
}
