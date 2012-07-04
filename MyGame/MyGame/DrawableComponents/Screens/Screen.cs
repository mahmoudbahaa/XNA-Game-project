using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Helper;
using control;

namespace MyGame
{
    public class Screen : DrawableGameComponent
    {
        protected SpriteBatch spriteBatch;
        protected DelayedAction delayedAction;

        protected Color backgroundColor = Color.Navy;
        protected Color menuItemColor = Color.LightGreen;

        protected SpriteFont smallFont;
        protected SpriteFont mediumFont;
        protected SpriteFont bigFont;

        protected Texture2D background;

        protected float silencePeriod = 500;
        protected MyGame myGame;
        public Screen(MyGame game,int delayedActionDelay)
            : base(game)
        {
            myGame = game;

            spriteBatch = new SpriteBatch(game.GraphicsDevice);
            delayedAction = new DelayedAction(delayedActionDelay);


            background = game.Content.Load<Texture2D>("poster");

            smallFont = Game.Content.Load<SpriteFont>("SpriteFont1");
            mediumFont = Game.Content.Load<SpriteFont>("SpriteFontMedium");
            bigFont = Game.Content.Load<SpriteFont>("SpriteFontLarge");

        }

        public void reInitialize()
        {
            silencePeriod = 500;
        }

        protected Vector2 findCenteredPos1(String text, SpriteFont font)
        {
            Vector2 pos = findCenteredPos2(text, font);
            pos.Y = 0;
            return pos;
        }

        protected Vector2 findCenteredPos2(String text, SpriteFont font)
        {
            Vector2 pos = new Vector2();
            Vector2 textMeasure = font.MeasureString(text);
            pos.X = (Game.GraphicsDevice.Viewport.Width - textMeasure.X) / 2;
            pos.Y = (Game.GraphicsDevice.Viewport.Height - textMeasure.Y) / 2;
            return pos;
        }

        protected bool checkSilencePeriod(GameTime gameTime)
        {
            if (silencePeriod > 0)
            {
                silencePeriod -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (silencePeriod > 0)
                    return true;
            }
            return false;
        }
    }
}
