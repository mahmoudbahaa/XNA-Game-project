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

        Model runModel;
        Model dieModel;
        SkinningData runSkinningData ;
        SkinningData dieSkinningData ;

        public enum MonsterAnimations
        {
            Die = 0,
            Run = 1
        }

        public MonsterModel(Game1 game,SkinningData runSkinningData,SkinningData dieSkinningData, Model runModel, Model dieModel , Unit unit)
            : base(game, runSkinningData, runModel, unit)
        {
            this.runModel = runModel;
            this.dieModel = dieModel;
            this.runSkinningData = runSkinningData;
            this.dieSkinningData = dieSkinningData;
            game.register(this, MyEvent.M_DIE);
            animator.StartClip(animations[(int)MonsterAnimations.Run], false,true);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (int ev in events)
            {
                switch (ev)
                {
                    case MyEvent.M_DIE:
                        Die();
                        break;
                    default:
                        break;
                }
            }
            events.Clear();
            base.Draw(gameTime);
        }

        public void Die()
        {
            animator.StartClip(animations[(int)MonsterAnimations.Die], false);
        }
    }
}
