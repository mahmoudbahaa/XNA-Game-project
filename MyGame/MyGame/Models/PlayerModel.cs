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
    class PlayerModel : AnimatedModel
    {
        private bool running;

        //readonly String[] animations = new String[] { "Take 001" };
        readonly String[] animations = new String[] { "run1" };

        public enum PlayAnimations
        {
            Run = 0
        }

        public PlayerModel(Game1 game,SkinningData skinningData, Model model, Unit unit)
            : base(game,skinningData, model, unit)
        {
            game.register(this,MyEvent.P_RUN);
            running = false;
            animator.StartClip(animations[(int)PlayAnimations.Run], true);
        }

        public override void Draw(GameTime gameTime)
        {
            running = false;
            foreach (Event ev in events)
            {
                switch (ev.eventId)
                {
                    case MyEvent.P_RUN:
                        running = true;
                        break;
                    default:
                        break;
                }
            }
            Run();
            events.Clear();
            base.Draw(gameTime);
            
        }
       

       
        public void Run()
        {
            if (running)
            {
                animator.loop = true;
                //animator.StartClip(animations[(int)PlayAnimations.Run],true);
                
            }
            else
            {
                animator.loop = false;
                //animator.StartClip(animations[(int)PlayAnimations.Run],false,true);
            }
        }


    }
}
