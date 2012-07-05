using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Helper
{
    /// <summary>
    /// Class used to allaw delay between successive key event.
    /// </summary>
    public class DelayedAction
    {
        // Shot variables
        /// <summary>amount of delay between 2 successive event. </summary>
        private int keyDelay ;

        /// <summary> time elapsed from last event occurrence.</summary>
        private int keyCountdown ;

        /// <summary>
        /// Constractor of DelayedAction class.
        /// </summary>
        /// <param name="keyDelay">amount of delay between 2 successive events</param>
        public DelayedAction(int keyDelay = 300)
        {
            keyCountdown = this.keyDelay = keyDelay;
        }

        /// <summary>
        /// indicate either the event happened again after the keyDelay has passed. (keyborad version)
        /// </summary>
        /// <param name="gameTime">the game time</param>
        /// <param name="keyState">the keyboard state</param>
        /// <param name="keys">array of keys to check if they are pressed or not.</param>
        /// <returns>boolean indicate either the event happened again.</returns>
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

        /// <summary>
        /// indicate either the event happened again after the keyDelay has passed. (general version)
        /// </summary>
        /// <param name="gameTime">the game time</param>
        /// <param name="condition">boolean indicating either the condition of the event is true or not.</param>
        /// <returns>boolean indicate either the event happened again.</returns>
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
