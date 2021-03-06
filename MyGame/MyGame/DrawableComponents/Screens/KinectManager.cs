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
    /// <summary>
    /// This class represent the Kinect Manager that manages the state of the kinect sensoring each on or off 
    /// and draw its status on screen
    /// </summary>
    public class KinectManager : Screen
    {
        // Shot variables
        //private const int keyDelay = 100;

        public KinectManager(MyGame game)
            : base(game,300)
        {
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (checkSilencePeriod(gameTime))
                return;
            KeyboardState keyState = Keyboard.GetState();
            if (delayedAction.eventHappened(gameTime, keyState.IsKeyDown(Keys.D0)))
            {
                GestureManager.paused = !GestureManager.paused ;
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This method renders the current state.
        /// </summary>
        /// <param name="gameTime">The elapsed game time.</param>
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            string kinectStatus = string.Format("kinect: {0}", GestureManager.paused?"Off":"On");
            Vector2 pos;
            pos.X = myGame.GraphicsDevice.Viewport.Width - smallFont.MeasureString(kinectStatus).X;
            spriteBatch.DrawString(smallFont, kinectStatus, new Vector2(pos.X, 5), Color.White);
            spriteBatch.DrawString(smallFont, kinectStatus, new Vector2(pos.X, 4), Color.Black);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
