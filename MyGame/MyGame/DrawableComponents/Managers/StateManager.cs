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
    public class StateManager : DrawableGameComponent ,IEvent
    {
        private SpriteBatch spriteBatch;

        private List<Event> events = new List<Event>();
        private MyGame myGame;
        private DelayedAction delayedAction;

        // Pause variables
        //int pauseDelay = 300;
        //int pauseCountdown = 0;

        public StateManager(MyGame game)
            : base(game)
        {
            myGame = game;

            spriteBatch = new SpriteBatch(game.GraphicsDevice);
            delayedAction = new DelayedAction();
            myGame.mediator.register(this, MyEvent.G_PAUSE,MyEvent.G_RESUME);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            foreach (Event ev in events)
            {
                switch (ev.EventId)
                {
                    case (int)MyEvent.G_PAUSE:
                        {
                            myGame.paused = true;
                            myGame.pause();
                            StartScreen.continueEnabled = true;
                            break;
                        }
                    case (int)MyEvent.G_RESUME:
                        {
                            myGame.paused = false;
                            myGame.resume();
                            break;
                        }
                }
            }
            events.Clear();

            if (myGame.gameOver || !myGame.canPause)
                return;
            KeyboardState keyState = Keyboard.GetState();
            if (delayedAction.eventHappened(gameTime, keyState, Keys.P, Keys.Escape))
            {
                myGame.paused = !myGame.paused;
                if (myGame.paused)
                {
                    myGame.pause();
                    StartScreen.continueEnabled = true;
                }
                else
                    myGame.resume();

            }

        }

        public override void Draw(GameTime gameTime)
        {
            //if (myGame.paused)
            //{
                if(myGame.gameOver)
                    Draw("Game  ver  ");
                //else
                //    Draw("PAUSED");
            //}
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


        public void addEvent(Event ev)
        {
            events.Add(ev);
        }
    }
}
