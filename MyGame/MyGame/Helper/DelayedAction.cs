using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Helper
{
    public class DelayedAction
    {
        // Shot variables
        private int keyDelay ;
        private int keyCountdown ;
        public DelayedAction(int keyDelay = 300)
        {
            keyCountdown = this.keyDelay = keyDelay;
        }

        public bool eventHappened(GameTime gameTime, KeyboardState keyState, params Keys[] keys)
        {
            keyCountdown -= gameTime.ElapsedGameTime.Milliseconds;
            if (keyCountdown <= 0)
            {
                foreach (Keys key in keys)
                {
                    if (keyState.IsKeyDown(key))
                    {
                        keyCountdown = keyDelay;
                        return true;
                    }
                }
                keyCountdown = 0;
            }

            return false;
        }

        public bool eventHappened(GameTime gameTime, bool condition)
        {
            keyCountdown += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (keyCountdown >= keyDelay)
            {
                if (condition)
                {
                    keyCountdown = 0;
                    return true;
                }
                keyCountdown = keyDelay;
            }

            return false;
        }
    }
}
