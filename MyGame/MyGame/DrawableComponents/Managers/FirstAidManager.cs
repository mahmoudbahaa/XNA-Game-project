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

        private MyGame myGame;
        public FirstAidManager(MyGame game)
            : base(game)
        {
            firstAidKits = new List<FirstAid>();
            myGame = game;

            rnd = new Random();
        }

        private void addFirstAidKit()
        {
            float y = Constants.TERRAIN_HEIGHT;
            float x = 0, z = 0;
            while (y > .7 * Constants.TERRAIN_HEIGHT)
            {
                x = (float)(rnd.NextDouble() * Constants.FIELD_MAX_X_Z * 2 - Constants.FIELD_MAX_X_Z);
                z = (float)(rnd.NextDouble() * Constants.FIELD_MAX_X_Z * 2 - Constants.FIELD_MAX_X_Z);
                y = myGame.GetHeightAtPosition(x, z);
            }
            Vector3 pos = new Vector3(x, y, z) + Constants.MEDKIT_OFFSET;
            Unit unit = new Unit(myGame, pos, Vector3.Zero, Constants.MEDKIT_SCALE);
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
            myGame.player.health += myGame.difficultyConstants.INCREASED_HEALTH_BY_MEDKIT;
            if (myGame.player.health > 100)
                myGame.player.health = 100;
            firstAidKits.RemoveAt(j);
        }

        public override void Update(GameTime gameTime)
        {
            if (myGame.paused)
                return;

            reaminingTimeToNextSpawn -= gameTime.ElapsedGameTime.Milliseconds;
            if (reaminingTimeToNextSpawn < 0 && firstAidKits.Count < myGame.difficultyConstants.NUM_MEDKITS_IN_FIELD)
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
