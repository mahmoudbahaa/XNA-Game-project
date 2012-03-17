using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Helper;


namespace MyGame
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class StateManager : DrawableGameComponent 
    {
        private SpriteBatch spriteBatch;
        
        private Game1 myGame;
        private DelayedAction delayedAction;

        // Pause variables
        //int pauseDelay = 300;
        //int pauseCountdown = 0;

        public StateManager(Game1 game)
            : base(game)
        {
            myGame = game;

            spriteBatch = new SpriteBatch(game.GraphicsDevice);
            delayedAction = new DelayedAction();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (myGame.gameOver)
                return;
            KeyboardState keyState = Keyboard.GetState();
            if(delayedAction.eventHappened(gameTime,keyState,Keys.P))
                myGame.paused = !myGame.paused;

        }

        public override void Draw(GameTime gameTime)
        {
            if (myGame.paused)
            {
                if(myGame.gameOver)
                    Draw("Game Over");
                else
                    Draw("PAUSED");
            }
        }

        private void Draw(String text)
        {
            spriteBatch.Begin();
            SpriteFont font = myGame.Content.Load<SpriteFont>("SpriteFontLarge");
            Vector2 measure = font.MeasureString(text);
            float x = (myGame.GraphicsDevice.Viewport.Width - measure.X) / 2;
            float y = (myGame.GraphicsDevice.Viewport.Height - measure.Y) / 2;
            spriteBatch.DrawString(font, text, new Vector2(x, y), Color.Red);
            spriteBatch.End();
        }

    }
}
