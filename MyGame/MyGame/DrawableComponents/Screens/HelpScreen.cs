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
    /// <summary>
    /// This class represent the Help Screen that show some info on how to play the game
    /// </summary>
    public class HelpScreen : Screen
    {
        // Shot variables
        //private const int keyDelay = 100;

        private Color menuItemDescriptionColor = Color.Yellow;
        private float preferredtitlePosOffset = 200;
        private Color titleColor = Color.Red;

        private String title = "Help";
        private String[] menuItems = new String[] { "Movement", "Attack", "Camera", "Music", "FullScreen" };
        private String[] menuItemsDescription = new String[] { 
            "Press W/A/S/D for movement",
            "Press spacebar or left mouse button to attack", 
            "Press Up/Left/Down/Right/mouse wheel for moving the camera\n"+
            "Press C to toggle camera mode (1st person/3rd person)",
            "Press 'M' to toggle Music on/off",
            "Press 'rightAlt'+'Enter' to toggle full screen"};

        public HelpScreen(MyGame game)
            : base(game,300)
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (checkSilencePeriod(gameTime))
                return;
            KeyboardState keyState = Keyboard.GetState();
            if (delayedAction.eventHappened(gameTime, keyState.IsKeyDown(Keys.Enter) 
                                                    && !keyState.IsKeyDown(Keys.RightAlt)))
            {
                myGame.mediator.fireEvent(MyEvent.G_StartScreen);
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(backgroundColor);
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Rectangle(0, 0, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height), Color.White);


            Vector2 pos = findCenteredPos(title, bigFont);
            Vector2 nextPosOffset = new Vector2(0, Math.Min(preferredtitlePosOffset, pos.Y));
            pos -= nextPosOffset;
            spriteBatch.DrawString(bigFont, title, pos, titleColor);

            nextPosOffset = nextPosOffset - new Vector2(0, bigFont.MeasureString(title).Y);
            for (int i = 0; i < menuItems.Count(); i++)
            {
                pos = findCenteredPos(menuItems[i], mediumFont) - nextPosOffset;
                spriteBatch.DrawString(mediumFont, menuItems[i], pos, menuItemColor);
                nextPosOffset = nextPosOffset - new Vector2(0, mediumFont.MeasureString(menuItems[i]).Y);
                pos = findCenteredPos(menuItemsDescription[i], smallFont) - nextPosOffset;
                spriteBatch.DrawString(smallFont, menuItemsDescription[i], pos, menuItemDescriptionColor);
                nextPosOffset = nextPosOffset - new Vector2(0, smallFont.MeasureString(menuItemsDescription[i]).Y);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
