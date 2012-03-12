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
    public class ScoreBoard : DrawableGameComponent ,IEvent
    {
        private SpriteBatch spriteBatch;
        public int score = 0;

        protected List<Event> events;
        
        private Game1 myGame;

        //List<CModel> models = new List<CModel>();
        //List<CModel> enemies = new List<CModel>();
        Camera camera;

        public ScoreBoard(Game1 game)
            : base(game)
        {
            this.camera = game.camera;
            myGame = game;
            game.register(this, MyEvent.M_DIE);
            events = new List<Event>();

            spriteBatch = new SpriteBatch(game.GraphicsDevice);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < events.Count; i++) 
            {
                switch (events[i].EventId)
                {
                    case (int)MyEvent.M_DIE:
                        score++;
                        events.Remove(events[i]);
                        i--;
                        break;
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            SpriteFont font = Game.Content.Load<SpriteFont>("SpriteFont1");
            spriteBatch.DrawString(font, "Score: " + score, new Vector2(5, 5), Color.Red);
            spriteBatch.End();
            base.Draw(gameTime);
        }


        public void addEvent(Event ev)
        {
            events.Add(ev);
        }

    }
}
