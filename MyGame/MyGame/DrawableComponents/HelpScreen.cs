﻿using System;
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
    public class HelpScreen : DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private DelayedAction delayedAction;

        // Shot variables
        //private const int keyDelay = 100;

        private Color backgroundColor = Color.Navy;
        private Color menuItemColor = Color.Green;
        private Color menuItemDescriptionColor = Color.Yellow;

        private String[] menuItems = new String[] { "Movement", "Attack", "Camera", "Music", "FullScreen" };
        private String[] menuItemsDescription = new String[] { 
            "Press W/A/S/D for movement",
            "Press spacebar or left mouse button to attack", 
            "Press Up/Left/Down/Right/mouse wheel for moving the camera\n"+
            "Press C to toggle camera mode (1st person/3rd person)",
            "Press 'M' to toggle Music on/off",
            "Press 'rightAlt'+'Enter' to toggle full screen"};

        private MyGame myGame;
        public HelpScreen(MyGame game)
            : base(game)
        {
            myGame = game;

            spriteBatch = new SpriteBatch(game.GraphicsDevice);
            delayedAction = new DelayedAction();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();
            if (delayedAction.eventHappened(gameTime, keyState.IsKeyDown(Keys.Enter) 
                                                    && !keyState.IsKeyDown(Keys.RightAlt)))
            {
                if(keyState.IsKeyDown(Keys.Enter)) {
                    myGame.mediator.fireEvent(MyEvent.G_StartScreen);
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(backgroundColor);
            spriteBatch.Begin();

            SpriteFont smallFont = Game.Content.Load<SpriteFont>("SpriteFont1");
            SpriteFont mediumFont = Game.Content.Load<SpriteFont>("SpriteFontMedium");

            Vector2 nextPosOffset = Vector2.Zero ;
            Vector2 pos;
            for (int i = 0; i < menuItems.Count(); i++)
            {

                pos = findCenteredPos(menuItems[i], mediumFont) + nextPosOffset;
                spriteBatch.DrawString(mediumFont, menuItems[i], pos, menuItemColor);
                nextPosOffset += new Vector2(0, mediumFont.MeasureString(menuItems[i]).Y);
                pos = findCenteredPos(menuItemsDescription[i], smallFont) + nextPosOffset;
                spriteBatch.DrawString(smallFont, menuItemsDescription[i], pos, menuItemDescriptionColor);
                nextPosOffset += new Vector2(0, smallFont.MeasureString(menuItemsDescription[i]).Y);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        Vector2 findCenteredPos(String text,SpriteFont font)
        {
            Vector2 pos = new Vector2(0);
            Vector2 textMeasure = font.MeasureString(text);
            pos.X = (Game.GraphicsDevice.Viewport.Width - textMeasure.X ) / 2;
            return pos;
        }
    }
}
