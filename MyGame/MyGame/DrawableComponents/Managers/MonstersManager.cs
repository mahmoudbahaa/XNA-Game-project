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
    public class MonstersManager : DrawableGameComponent, IEvent
    {
        List<Event> events = new List<Event>();

        private List<Monster> deadMonsters;
        private List<Monster> monsters;
        private HPBillboardSystem hpBillBoardSystem;

        private Random[] rnd = new Random[2];
        private float spawnTime = 300;
        private float reaminingTimeToNextSpawn = 0;

        //Texture2D[] monsterTexture = new Texture2D[3];
        MonsterConstants[] monsterConstants = new MonsterConstants[4];
        SkinnedModel[] skinnedModel = new SkinnedModel[4];
        //SkinnedModel level2skinnedModel;
        //SkinnedModel level3skinnedModel;

        private MyGame myGame;
        public MonstersManager(MyGame game)
            : base(game)
        {
            monsters = new List<Monster>();
            deadMonsters = new List<Monster>();
            myGame = game;

            rnd[0] = new Random();
            rnd[1] = new Random();


            hpBillBoardSystem = new HPBillboardSystem(game.GraphicsDevice, game.Content, Constants.HP_SIZE, monsters);
            //skinnedModel = Game.Content.Load<SkinnedModel>(@"Textures\EnemyBeast");

            reinitializeMonstersTypes();

            myGame.mediator.register(this, MyEvent.G_NextLevel_END_OF_MUSIC);
        }

        public bool checkCollisionWithBullet(Unit unit)
        {
            // If shot is still in play, check for collisions
            for (int j = 0; j < monsters.Count; ++j)
            {
                if (monsters[j].unit.alive && unit.collideWith(monsters[j].unit))
                {
                    myGame.mediator.fireEvent(MyEvent.M_HIT);
                    monsters[j].health -= monsters[j].monsterUnit.monsterConstants.MONSTER_HEALTH_PER_BULLET;
                    hpBillBoardSystem.setTexture(j);

                    if (monsters[j].health <= 0)
                    {
                        monsters[j].Die();
                        monsters[j].unit.alive = false;
                        myGame.mediator.fireEvent(MyEvent.M_DIE,"Score",monsters[j].getScore());
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
            float x = 0, z = 0;
            float y = Constants.TERRAIN_HEIGHT;
            //while(y > .5 * Constants.TERRAIN_HEIGHT)
            //{
            bool flag = true;
            while (flag)
            {
                x = (float)(rnd[0].NextDouble() * Constants.FIELD_MAX_X_Z * 2 - Constants.FIELD_MAX_X_Z);
                z = (float)(rnd[0].NextDouble() * Constants.FIELD_MAX_X_Z * 2 - Constants.FIELD_MAX_X_Z);
                //x = (float)(rnd[0].NextDouble() * 50);
                //z = (float)(rnd[0].NextDouble() * 50);
                if (Math.Abs(x - myGame.player.unit.position.X)>20 && Math.Abs(z-myGame.player.unit.position.Z)>20)
                    flag = false;
            }
            y = myGame.GetHeightAtPosition(x, z);
            //}
            Vector3 pos = new Vector3(x, y, z);
            Vector3 rot = new Vector3(0, (float)(rnd[0].NextDouble() * MathHelper.TwoPi), 0);
            int choice = (int)(rnd[1].NextDouble() * 4);

            MonsterUnit monsterUnit = new MonsterUnit(myGame, pos, rot, Constants.MONSTER_SCALE, monsterConstants[choice]);

            Monster monster;
            //if(deadMonsters.Count == 0)
                monster = new Monster(myGame,new MonsterModel(myGame,skinnedModel[choice]) , monsterUnit);
            //else
            //{
            //    monster = deadMonsters[0];
            //    deadMonsters.RemoveAt(0);
            //    monster.monsterUnit = monsterUnit;
            //    monster.unit = monsterUnit;
            //    monster.health = 100;
            //    ((MonsterModel)monster.cModel).reinitialize(skinnedModel[choice]);
            //}

            monsters.Add(monster);
            hpBillBoardSystem.monstersTextures.Add(HPBillboardSystem.getTexture(monster.health));
            //billBoardSystem.monsters.Add(monster);
        }

        public void reinitializeMonstersTypes()
        {
            switch (myGame.currentLevel)
            {
                case 1:
                    {
                        monsterConstants[0] = new MonsterLevel1Constants();
                        monsterConstants[3] = new MonsterLevel2Constants();

                        skinnedModel[0] = Game.Content.Load<SkinnedModel>(@"monsters\1\model\EnemyBeast");
                        skinnedModel[3] = Game.Content.Load<SkinnedModel>(@"monsters\2\model\EnemyBeast");
                        break;
                    }
                case 2:
                    {
                        monsterConstants[0] = new MonsterLevel2Constants();
                        monsterConstants[3] = new MonsterLevel3Constants();

                        skinnedModel[0] = Game.Content.Load<SkinnedModel>(@"monsters\2\model\EnemyBeast");
                        skinnedModel[3] = Game.Content.Load<SkinnedModel>(@"monsters\3\model\EnemyBeast");
                        break;
                    }

                case 3:
                    {
                        monsterConstants[0] = new MonsterLevel3Constants();
                        monsterConstants[3] = new MonsterLevel4Constants();

                        skinnedModel[0] = Game.Content.Load<SkinnedModel>(@"monsters\3\model\EnemyBeast");
                        skinnedModel[3] = Game.Content.Load<SkinnedModel>(@"monsters\4\model\EnemyBeast");
                        break;
                    }
            }


            for (int i = 0; i < 5; i+=3)
            {
                foreach (ModelMesh mesh in skinnedModel[i].Model.Meshes)
                    foreach (SkinnedEffect effect in mesh.Effects)
                    {
                        effect.EnableDefaultLighting();
                    }
            }

            if (myGame.difficultyConstants is NoviceConstants)
            {
                monsterConstants[1] = monsterConstants[0];
                monsterConstants[2] = monsterConstants[0];
                skinnedModel[1] = skinnedModel[0];
                skinnedModel[2] = skinnedModel[0];
            }
            else if (myGame.difficultyConstants is AdvancedConstants)
            {
                monsterConstants[1] = monsterConstants[0];
                monsterConstants[2] = monsterConstants[3];
                skinnedModel[1] = skinnedModel[0];
                skinnedModel[2] = skinnedModel[3];
            }
            else
            {
                monsterConstants[1] = monsterConstants[3];
                monsterConstants[2] = monsterConstants[3];
                skinnedModel[1] = skinnedModel[3];
                skinnedModel[2] = skinnedModel[3];
            }

        }

        public override void Update(GameTime gameTime)
        {
            if (myGame.paused)
                return;

            foreach (Event ev in events)
            {
                switch (ev.EventId)
                {
                    case (int)MyEvent.G_NextLevel_END_OF_MUSIC: reinitializeMonstersTypes(); break;
                }
            }
            events.Clear();

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
                        myGame.mediator.fireEvent(MyEvent.M_BITE, "decreasedHealth",
                            monsters[j].monsterUnit.monsterConstants.PLAYER_HEALTH_DECREASE);
                    }
                }
                else if (monsters[j].monsterUnit.dead)
                {
                    deadMonsters.Add(monsters[j]);
                    monsters.RemoveAt(j);
                    hpBillBoardSystem.monstersTextures.RemoveAt(j);
                    monsters[j].Dispose();
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

        public void addEvent(Event ev)
        {
            events.Add(ev);
        }
    }
}
