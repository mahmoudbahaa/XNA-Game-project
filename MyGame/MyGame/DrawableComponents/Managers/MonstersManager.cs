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
        private HPBillboardSystem hpBillBoardSystem;

        private Random rnd;
        private float spawnTime = 300;
        private float reaminingTimeToNextSpawn = 0;


        SkinnedModel skinnedModel;

        private MyGame myGame;
        public MonstersManager(MyGame game)
            : base(game)
        {
            monsters = new List<Monster>();
            myGame = game;

            rnd = new Random();


            hpBillBoardSystem = new HPBillboardSystem(game.GraphicsDevice, game.Content, Constants.HP_SIZE, monsters);
            //skinnedModel = Game.Content.Load<SkinnedModel>(@"Textures\EnemyBeast");

            skinnedModel = Game.Content.Load<SkinnedModel>(@"model\EnemyBeast");
        }

        public bool checkCollisionWithBullet(Unit unit)
        {
            // If shot is still in play, check for collisions
            for (int j = 0; j < monsters.Count; ++j)
            {
                if (monsters[j].unit.alive && unit.collideWith(monsters[j].unit))
                {
                    monsters[j].health -= myGame.difficultyConstants.MONSTER_HEALTH_PER_BULLET;
                    hpBillBoardSystem.setTexture(j);

                    if (monsters[j].health <= 0)
                    {
                        monsters[j].Die();
                        monsters[j].unit.alive = false;
                        myGame.mediator.fireEvent(MyEvent.M_DIE);
                    }
                    else
                    {
                        monsters[j].TakeDamage();
                        ((MonsterUnit)monsters[j].unit).moving = true;
                    }
                    return true;
                }
            }
            return false;
        }


        private void addEnemy()
        {
            float x=0,z=0;
            float y = Constants.TERRAIN_HEIGHT;
            //while(y > .5 * Constants.TERRAIN_HEIGHT)
            //{
                x = (float)(rnd.NextDouble() * Constants.FIELD_MAX_X_Z * 2 - Constants.FIELD_MAX_X_Z);
                z = (float)(rnd.NextDouble() * Constants.FIELD_MAX_X_Z * 2 - Constants.FIELD_MAX_X_Z);
                y = myGame.GetHeightAtPosition(x, z);
            //}
            Vector3 pos = new Vector3(x, y, z);
            Vector3 rot = new Vector3(0, (float)(rnd.NextDouble() * MathHelper.TwoPi), 0);
            MonsterUnit monsterUnit = new MonsterUnit(myGame, pos, rot, Constants.MONSTER_SCALE);
            Monster monster = new Monster(myGame, skinnedModel, monsterUnit);

            monsters.Add(monster);
            hpBillBoardSystem.monstersTextures.Add(HPBillboardSystem.getTexture(monster.health));
            //billBoardSystem.monsters.Add(monster);
        }

        public override void Update(GameTime gameTime)
        {
            if (myGame.paused)
                return;

            reaminingTimeToNextSpawn -= gameTime.ElapsedGameTime.Milliseconds;
            if (reaminingTimeToNextSpawn < 0 && monsters.Count < myGame.difficultyConstants.NUM_MONSTERS_IN_FIELD)
            {
                reaminingTimeToNextSpawn = spawnTime;
                addEnemy();
            }
            for (int j = 0; j < monsters.Count; j++)// Monster monster in monsters)
            {
                monsters[j].Update(gameTime);

                if (monsters[j].unit.alive && myGame.player.unit.collideWith(monsters[j].unit))
                {
                    monsters[j].monsterUnit.moving = false;
                    if (monsters[j].ActiveAnimation != MonsterModel.MonsterAnimations.Bite)
                    {
                        monsters[j].Bite();
                        myGame.mediator.fireEvent(MyEvent.M_BITE);
                    }
                }
                else if (monsters[j].monsterUnit.dead)
                {
                    monsters.RemoveAt(j);
                    hpBillBoardSystem.monstersTextures.RemoveAt(j);
                    j--;
                }
            }

            if (monsters.Count != 0) 
                hpBillBoardSystem.generateParticles();
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (Monster monster in monsters)
                monster.Draw(gameTime);

            if (monsters.Count != 0) 
                hpBillBoardSystem.Draw(myGame.camera.View, myGame.camera.Projection,
                    ((ChaseCamera)myGame.camera).Up, ((ChaseCamera)myGame.camera).Right);
            base.Draw(gameTime);
        }
    }
}
