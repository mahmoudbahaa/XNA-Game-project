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
    /// This class represent the Level Screen that is displayed at the beginning of each level showing the level number
    /// and waiting on input from the user to start the actual level.
    /// </summary>
    public class LevelScreen : Screen
    {
        public LevelScreen(MyGame game)
            : base(game,300)
        {
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();
            if (delayedAction.eventHappened(gameTime, keyState.IsKeyDown(Keys.Enter) 
                                                    && !keyState.IsKeyDown(Keys.RightAlt)))
            {
                myGame.mediator.fireEvent(MyEvent.G_StartLevel);
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(backgroundColor);
            spriteBatch.Begin();

            String text1 = "Level " + myGame.currentLevel;
            String text2 = "Press Enter to Continue";
            spriteBatch.DrawString(bigFont, text1, findCenteredPos(text1, bigFont), menuItemColor);
            spriteBatch.DrawString(smallFont, text2, findCenteredPos(text2, smallFont) + 
                new Vector2(0,bigFont.MeasureString(text1).Y), menuItemColor);
            //Vector2 nextPosOffset = Vector2.Zero ;
            //Vector2 pos;
            //for (int i = 0; i < menuItems.Count(); i++)
            //{

            //    pos = findCenteredPos(menuItems[i], mediumFont) + nextPosOffset;
            //    spriteBatch.DrawString(mediumFont, menuItems[i], pos, menuItemColor);
            //    nextPosOffset += new Vector2(0, mediumFont.MeasureString(menuItems[i]).Y);
            //    pos = findCenteredPos(menuItemsDescription[i], smallFont) + nextPosOffset;
            //    spriteBatch.DrawString(smallFont, menuItemsDescription[i], pos, menuItemDescriptionColor);
            //    nextPosOffset += new Vector2(0, smallFont.MeasureString(menuItemsDescription[i]).Y);
            //}

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
