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

        readonly String[] animations = new String[] { "Idle" };

        public enum MonsterAnimations
        {
            Die = 0
        }

        public MonsterModel(Game1 game,SkinningData skinningData, Model model,Unit unit)
            : base(game, skinningData, model, unit)
        {
            game.register(this, MyEvent.M_DIE);
            animator.StartClip(animations[(int)MonsterAnimations.Die], true);
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
            animator.StartClip(animations[(int)MonsterAnimations.Die], false);
        }
    }
}
