using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MyGame
{
    public class FrameRateCounter : DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;

        int frameRate = 0;
        int frameCounter = 0;
        TimeSpan elapsedTime = TimeSpan.Zero;

        private MyGame myGame;


        public FrameRateCounter(Game game)
            : base(game)
        {
            myGame = (MyGame)game;
        }


        protected override void  LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteFont = Game.Content.Load<SpriteFont>("SpriteFont1");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }
        }


        public override void Draw(GameTime gameTime)
        {
            frameCounter++;

            string fps = string.Format("fps: {0}", frameRate);

            spriteBatch.Begin();

            Vector2 pos;
            pos.X = myGame.GraphicsDevice.Viewport.Width - 250 - spriteFont.MeasureString(fps).X;
            spriteBatch.DrawString(spriteFont, fps, new Vector2(pos.X, 5), Color.White);
            spriteBatch.DrawString(spriteFont, fps, new Vector2(pos.X, 4), Color.Black);

            spriteBatch.End();
        }
    }
}
