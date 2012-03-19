using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Helper;
using XNAnimation;

namespace MyGame
{
    public class FirstAidManager : DrawableGameComponent
    {
        private List<FirstAid> firstAidKits;

        private Random rnd;
        private float spawnTime = 5000;
        private float reaminingTimeToNextSpawn = 0;

        private Game1 myGame;
        public FirstAidManager(Game1 game)
            : base(game)
        {
            firstAidKits = new List<FirstAid>();
            myGame = game;

            rnd = new Random();
        }

        private void addFirstAidKit()
        {
            float x = (float)(rnd.NextDouble() * 4700 - Constants.FIELD_MAX_X_Z);
            float z = (float)(rnd.NextDouble() * 4700 - Constants.FIELD_MAX_X_Z);
            Vector3 pos = new Vector3(x, myGame.GetHeightAtPosition(x, z) + 30, z);
            Unit unit = new Unit(myGame, pos, Vector3.Zero, new Vector3(.5f));
            FirstAid firstAid = new FirstAid(myGame, myGame.Content.Load<Model>(@"model/First Aid Kit2"), unit);

            firstAidKits.Add(firstAid);
        }

        public bool checkCollisionWithBullet(Unit unit)
        {
            // If shot is still in play, check for collisions
            for (int j = 0; j < firstAidKits.Count; ++j)
            {
                if (unit.collideWith(firstAidKits[j].unit))
                {
                    addHealth(j);
                    return true;
                }
            }
            return false;
        }

        private void addHealth(int j)
        {
            myGame.player.health += 50;
            if (myGame.player.health > 100)
                myGame.player.health = 100;
            firstAidKits.RemoveAt(j);
        }

        public override void Update(GameTime gameTime)
        {
            if (myGame.paused)
                return;

            reaminingTimeToNextSpawn -= gameTime.ElapsedGameTime.Milliseconds;
            if (reaminingTimeToNextSpawn < 0 && firstAidKits.Count < 5)
            {
                reaminingTimeToNextSpawn = spawnTime;
                addFirstAidKit();
            }
            for (int j = 0; j < firstAidKits.Count; j++)// Monster monster in monsters)
            {
                firstAidKits[j].Update(gameTime);

                if (myGame.player.unit.collideWith(firstAidKits[j].unit))
                {
                    addHealth(j);
                    j--;
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (FirstAid firstAid in firstAidKits)
                firstAid.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
