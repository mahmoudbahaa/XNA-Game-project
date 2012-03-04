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
            
            game.register(this, MyEvent.M_DIE);
            animator.running = true;
            animator.StartClip(animations[(int)MonsterAnimations.Run], true);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (Event ev in events)
            {
                switch (ev.eventId)
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
            animator.skinningData = dieSkinningData;
            unit.alive = false;
            animator.StartClip(animations[(int)MonsterAnimations.Die], false);
        }
    }
}
