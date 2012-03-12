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
    public class MonstersManager : DrawableGameComponent
    {
        private List<Monster> monsters;

        private Random rnd;
        private float spawnTime = 300;
        private float reaminingTimeToNextSpawn = 0;

        SkinnedModel runSkinnedModel;
        SkinnedModel dieSkinnedModel;

        private Game1 myGame;
        public MonstersManager(Game1 game)
            : base(game)
        {
            monsters = new List<Monster>();
            myGame = game;

            rnd = new Random();

            //skinnedModel = Game.Content.Load<SkinnedModel>(@"Textures\EnemyBeast");
        }

        public bool checkCollisionWithBullet(Unit unit)
        {
            // If shot is still in play, check for collisions
            for (int j = 0; j < monsters.Count; ++j)
            {

                if (((MonsterUnit)monsters[j].unit).dead)
                {
                    monsters.Remove(monsters[j]);
                    Game.Components.Remove(monsters[j]);
                }
                else if (monsters[j].unit.alive && unit.collideWith(monsters[j].unit))
                {
                    ((MonsterModel)monsters[j].cModel).Die();
                    monsters[j].unit.alive = false;
                    return true;
                }
            }
            return false;
        }


        private void addEnemy()
        {
            runSkinnedModel = Game.Content.Load<SkinnedModel>(@"model\EnemyBeast");
            dieSkinnedModel = Game.Content.Load<SkinnedModel>(@"model\EnemyBeastDie");


            Vector3 pos = new Vector3((float)(rnd.NextDouble() * 4700 - Constants.FIELD_MAX_X_Z),
                5, (float)(rnd.NextDouble() * 4700 - Constants.FIELD_MAX_X_Z));
            Vector3 rot = new Vector3(0, (float)(rnd.NextDouble() * MathHelper.TwoPi), 0);
            MonsterUnit monsterUnit = new MonsterUnit(myGame, pos, rot, new Vector3(.5f));
            Monster monster = new Monster(myGame, runSkinnedModel ,dieSkinnedModel, monsterUnit);

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
            foreach (Monster monster in monsters)
                monster.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (Monster monster in monsters)
                monster.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
