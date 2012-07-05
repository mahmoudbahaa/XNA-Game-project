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
    /// This class represent the ScoreBoard that draws and updates the score
    /// </summary>
    public class ScoreBoard : DrawableGameComponent ,IEvent
    {
        private SpriteBatch spriteBatch;
        public int score = 0;

        protected List<Event> events;
        
        private MyGame myGame;

        //List<CModel> models = new List<CModel>();
        //List<CModel> enemies = new List<CModel>();
        Camera camera;

        public ScoreBoard(MyGame game)
            : base(game)
        {
            this.camera = game.camera;
            myGame = game;
            game.mediator.register(this, MyEvent.M_DIE);
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
                        score+= (int)events[i].args["Score"];
                        if (score >= Constants.LEVEL_SCORES[myGame.currentLevel - 1])
                            myGame.mediator.fireEvent(MyEvent.G_NextLevel);
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
            spriteBatch.DrawString(font, "Score: " + score +"/" + Constants.LEVEL_SCORES[myGame.currentLevel-1], new Vector2(14, 40), Color.Red);
            spriteBatch.End();
            base.Draw(gameTime);
        }


        public void addEvent(Event ev)
        {
            events.Add(ev);
        }

    }
}
