using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkinnedModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Helper;

namespace MyGame
{
    public class Monsters : DrawableGameComponent
    {
        private List<CModel> monsters;

        private Random rnd;
        private float spawnTime = 300;
        private float reaminingTimeToNextSpawn = 0;

        Model dieModel;
        Model runModel;
        SkinningData runSkinnedData;
        SkinningData dieSkinnedData;

        private Game1 myGame;
        public Monsters(Game1 game)
            : base(game)
        {
            monsters = new List<CModel>();
            myGame = game;

            rnd = new Random();

            dieModel = Game.Content.Load<Model>(@"Textures\EnemyBeastDie");
            runModel = Game.Content.Load<Model>(@"Textures\EnemyBeast");
            runSkinnedData = runModel.Tag as SkinningData;
            dieSkinnedData = dieModel.Tag as SkinningData;

            addEnemy();
        }

        public bool checkCollisionWithBullet(BulletUnit bulletUnit)
        {
            // If shot is still in play, check for collisions
            for (int j = 0; j < monsters.Count; ++j)
            {

                if (((MonsterUnit)monsters[j].unit).dead)
                {
                    monsters.Remove(monsters[j]);
                    Game.Components.Remove(monsters[j]);
                }
                else if (monsters[j].unit.alive && bulletUnit.collideWith(monsters[j].unit))
                {
                    ((MonsterModel)monsters[j]).Die();
                    monsters[j].unit.alive = false;
                    return true;
                }
            }
            return false;
        }
        

        private void addEnemy()
        {
            dieModel = Game.Content.Load<Model>(@"Textures\EnemyBeastDie");
            runModel = Game.Content.Load<Model>(@"Textures\EnemyBeast");
            runSkinnedData = runModel.Tag as SkinningData;
            dieSkinnedData = dieModel.Tag as SkinningData;
            Vector3 pos = new Vector3((float)(rnd.NextDouble() * 4700 - Constants.FIELD_MAX_X_Z),
                5, (float)(rnd.NextDouble() * 4700 - Constants.FIELD_MAX_X_Z));
            Vector3 rot = new Vector3(0, (float)(rnd.NextDouble() * MathHelper.TwoPi), 0);
            MonsterUnit monsterUnit = new MonsterUnit(myGame, pos, rot, new Vector3(.5f));
            MonsterModel monster = new MonsterModel(myGame, runSkinnedData, dieSkinnedData, runModel, monsterUnit);

            monsters.Add(monster);
        }

        public override void Update(GameTime gameTime)
        {
            reaminingTimeToNextSpawn -= gameTime.ElapsedGameTime.Milliseconds;
            if (reaminingTimeToNextSpawn < 0 && monsters.Count < 30)
            {
                reaminingTimeToNextSpawn = spawnTime;
                addEnemy();
            }

            foreach (CModel monster in monsters)
            {
                Vector3 pos = monster.unit.position;
                monster.unit.position.Y = myGame.terrain.GetHeightAtPosition(pos.X, pos.Z);

                monster.Update(gameTime);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (CModel monster in monsters)
                monster.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
