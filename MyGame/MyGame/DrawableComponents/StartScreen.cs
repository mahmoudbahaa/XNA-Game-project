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
    public class StartScreen : DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private int chosenMenuItem = 0;
        private DelayedAction delayedAction;
        // Shot variables
        //int keyDelay = 100;
        //int keyCountdown = 100;

        private Color backgroundColor = Color.Navy;
        private Color titleColor = Color.Green;
        private Color menuItemColor = Color.Green;
        private Color shadedMenuItemColor = Color.Yellow;

        //private Vector2 titlePosOffset = new Vector2(0, 150);
        private float preferredtitlePosOffset = 150;
        private const String title1 = "XNA Shooter";
        private const String title2 = "Xterme - Novice - Advanced";

        private String[] menuItems = new String[]{"Start Game" , "Help" , "Exit"};

        private Texture2D emptyTex;

        private Game1 myGame;
        public StartScreen(Game1 game)
            : base(game)
        {
            myGame = game;

            spriteBatch = new SpriteBatch(game.GraphicsDevice);
            emptyTex = game.Content.Load<Texture2D>("empty2");
            delayedAction = new DelayedAction(100);
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();
            if (delayedAction.eventHappened(gameTime, keyState.IsKeyDown(Keys.Down) ||
                                                      keyState.IsKeyDown(Keys.Up) ||
                                                      (keyState.IsKeyDown(Keys.Enter) &&
                                                      !keyState.IsKeyDown(Keys.RightAlt))))
            {
                if (keyState.IsKeyDown(Keys.Down))
                {
                    chosenMenuItem++;
                    chosenMenuItem = chosenMenuItem % menuItems.Count();
                }
                else if (keyState.IsKeyDown(Keys.Up))
                {
                    chosenMenuItem--;
                    chosenMenuItem = (chosenMenuItem + menuItems.Count()) % menuItems.Count();
                }
                else if (keyState.IsKeyDown(Keys.Enter))
                {
                    switch (chosenMenuItem)
                    {
                        case 0: myGame.mediator.fireEvent(MyEvent.G_StartGame); break;
                        case 1: myGame.mediator.fireEvent(MyEvent.G_HelpScreen); break;
                        case 2: myGame.mediator.fireEvent(MyEvent.G_Exit); break;
                    }
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(backgroundColor);
            spriteBatch.Begin();

            SpriteFont bigFont = Game.Content.Load<SpriteFont>("SpriteFontLarge");
            SpriteFont mediumFont = Game.Content.Load<SpriteFont>("SpriteFontMedium");

            Vector2 pos = findCenteredPos(title1, bigFont);
            Vector2 nextPosOffset = new Vector2(0, Math.Min(preferredtitlePosOffset, pos.Y));
            pos -= nextPosOffset;
            spriteBatch.DrawString(bigFont, title1, pos , titleColor);

            nextPosOffset = nextPosOffset - new Vector2(0, bigFont.MeasureString(title1).Y);
            pos = findCenteredPos(title2, bigFont) - nextPosOffset;
            spriteBatch.DrawString(bigFont, title2, pos, titleColor);

            nextPosOffset = nextPosOffset - new Vector2(0, bigFont.MeasureString(title2).Y + 30);

            for (int i = 0; i < menuItems.Count(); i++)
            {
                pos =findCenteredPos(menuItems[i], mediumFont) - nextPosOffset;
                if (i == chosenMenuItem)
                {
                    Vector2 meausre = mediumFont.MeasureString(menuItems[i]);
                    spriteBatch.Draw(emptyTex, new Rectangle((int)pos.X, (int)pos.Y, (int)meausre.X, (int)meausre.Y),
                        shadedMenuItemColor);
                }
                spriteBatch.DrawString(mediumFont, menuItems[i], pos, menuItemColor);
                nextPosOffset = nextPosOffset - new Vector2(0, mediumFont.MeasureString(menuItems[i]).Y);
                
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        Vector2 findCenteredPos(String text,SpriteFont font)
        {
            Vector2 pos = new Vector2();
            Vector2 textMeasure = font.MeasureString(text);
            pos.X = (Game.GraphicsDevice.Viewport.Width - textMeasure.X ) / 2;
            pos.Y = (Game.GraphicsDevice.Viewport.Height - textMeasure.Y) / 2;
            return pos;
        }
    }
}
