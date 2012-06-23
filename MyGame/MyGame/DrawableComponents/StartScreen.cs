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
    public class StartScreen : Screen
    {
        private int chosenMenuItem = 0;
        public static bool continueEnabled = false;
        public static Constants.Difficulties Difficulty = Constants.Difficulties.Advanced;
        // Shot variables
        //int keyDelay = 100;
        //int keyCountdown = 100;
        private Color disabledMenuItemColor = Color.Red; 
        
        private Color titleColor = Color.LightGreen;
        private Color shadedMenuItemColor = Color.Yellow;

        //private Vector2 titlePosOffset = new Vector2(0, 150);
        private float preferredtitlePosOffset = 150;
        private const String title1 = "XNA Shooter";
        private const String title2 = "Xterme - Novice - Advanced";

        private String[] menuItems = new String[]{"Continue","Start Game" , "Help" ,"Difficulty <-$$->" , "Exit"};

        private Texture2D emptyTex;

        public StartScreen(MyGame game)
            : base(game,100)
        {
            emptyTex = game.Content.Load<Texture2D>("empty2");
        }

        public override void Update(GameTime gameTime)
        {
            if (checkSilencePeriod(gameTime))
                    return;
            KeyboardState keyState = Keyboard.GetState();
            if (delayedAction.eventHappened(gameTime, (keyState.IsKeyDown(Keys.Down) ||
                                                      keyState.IsKeyDown(Keys.Up) ||
                                                      keyState.IsKeyDown(Keys.Left) ||
                                                      keyState.IsKeyDown(Keys.Right) ||
                                                      (keyState.IsKeyDown(Keys.Enter)) &&
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
                else if (keyState.IsKeyDown(Keys.Left)  && chosenMenuItem == 3)
                {
                    Difficulty--;
                    Difficulty = (Constants.Difficulties)(((int)Difficulty + Constants.DifficultiesString.Count())
                        % Constants.DifficultiesString.Count());
                }
                else if ((keyState.IsKeyDown(Keys.Right) || keyState.IsKeyDown(Keys.Enter)) && chosenMenuItem == 3)
                {
                    Difficulty++;
                    Difficulty = (Constants.Difficulties)((int)Difficulty % Constants.DifficultiesString.Count());
                    
                    //chosenMenuItem = (Difficulty + menuItems.Count()) % menuItems.Count();
                }
                else if (keyState.IsKeyDown(Keys.Enter))
                {
                    switch (chosenMenuItem)
                    {
                        case 0: 
                            if (continueEnabled) 
                            {
                                myGame.paused = false;
                                myGame.resume();
                            }
                            break;
                        case 1: myGame.paused = false; myGame.mediator.fireEvent(MyEvent.G_StartGame); break;
                        case 2: myGame.mediator.fireEvent(MyEvent.G_HelpScreen); break;
                        case 4: myGame.mediator.fireEvent(MyEvent.G_Exit); break;
                    }
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(backgroundColor);
            spriteBatch.Begin();

            Vector2 pos = findCenteredPos2(title1, bigFont);
            Vector2 nextPosOffset = new Vector2(0, Math.Min(preferredtitlePosOffset, pos.Y));
            pos -= nextPosOffset;
            spriteBatch.DrawString(bigFont, title1, pos , titleColor);

            nextPosOffset = nextPosOffset - new Vector2(0, bigFont.MeasureString(title1).Y);
            pos = findCenteredPos2(title2, bigFont) - nextPosOffset;
            spriteBatch.DrawString(bigFont, title2, pos, titleColor);

            nextPosOffset = nextPosOffset - new Vector2(0, bigFont.MeasureString(title2).Y + 30);

            for (int i = 0; i < menuItems.Count(); i++)
            {
                String text = menuItems[i];
                if (i == 3)
                    text = text.Replace("$$", Constants.DifficultiesString[(int)Difficulty]);
                pos =findCenteredPos2(text, mediumFont) - nextPosOffset;
                if (i == chosenMenuItem)
                {
                    Vector2 meausre = mediumFont.MeasureString(text);
                    spriteBatch.Draw(emptyTex, new Rectangle((int)pos.X, (int)pos.Y, (int)meausre.X, (int)meausre.Y),
                        shadedMenuItemColor);
                }

                if (i == 0 && !continueEnabled)
                {
                    spriteBatch.DrawString(mediumFont, text, pos, disabledMenuItemColor);
                } else
                spriteBatch.DrawString(mediumFont,text , pos, menuItemColor);
                nextPosOffset = nextPosOffset - new Vector2(0, mediumFont.MeasureString(text).Y);
                
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
