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
using SkinnedModel;


namespace MyGame
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class CModelManager : DrawableGameComponent
    {
        public CModel player;
        private List<CModel> monsters;
        private List<CModel> bullets;
        private SkyModel sky;
        private Terrain terrain;

        //List<CModel> models = new List<CModel>();
        //List<CModel> enemies = new List<CModel>();
        Camera camera;
        float bulletRange = 3000;

        public CModelManager(Game1 game)
            : base(game)
        {
            this.camera = game.camera;
            Initialize();
        }

        public void addEnemy(CModel monster)
        {
            monsters.Add(monster);
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            monsters = new List<CModel>();
            bullets = new List<CModel>();

            addEnemy(intilizeMonster());
            player = initializePlayer();

            sky = intitializeSky();

            terrain = new Terrain(Game, camera, Game.Content.Load<Texture2D>("terrain"), 10, 100,
                Game.Content.Load<Texture2D>("grass"), 100, new Vector3(1, -1, 0));

            base.Initialize();
        }

        private SkyModel intitializeSky()
        {
            TextureCube tc = Game.Content.Load<TextureCube>("clouds");
            Effect effect = Game.Content.Load<Effect>("skysphere_effect");
            effect.Parameters["CubeMap"].SetValue(tc);
            Model pmodel = Game.Content.Load<Model>("skysphere_mesh");
            SkyUnit skyUnit = new SkyUnit(((Game1)Game),new Vector3(0,-300,0),Vector3.Zero,Vector3.One*100);
            SkyModel skyModel = new SkyModel((Game1)Game,  pmodel,skyUnit ,tc);

            return skyModel;
        }

        private PlayerModel initializePlayer()
        {
            Model pmodel = Game.Content.Load<Model>("dude");
            SkinningData skinnedData = pmodel.Tag as SkinningData;
            PlayerUnit playerUnit = new PlayerUnit((Game1)Game, new Vector3(0, 5, 0), Vector3.Zero, new Vector3(1f));
            PlayerModel playerModel = new PlayerModel((Game1)Game,skinnedData, pmodel,playerUnit);
            return playerModel;
        }

        private MonsterModel intilizeMonster()
        {
            Model dieModel= Game.Content.Load<Model>(@"Textures\EnemyBeastDie");
            Model runModel = Game.Content.Load<Model>(@"Textures\EnemyBeast");
            SkinningData runSkinnedData = runModel.Tag as SkinningData;
            SkinningData dieSkinnedData = dieModel.Tag as SkinningData;
            MonsterUnit monsterUnit = new MonsterUnit((Game1)Game, new Vector3(50, 5, 100), Vector3.Zero, new Vector3(.5f));
            MonsterModel monsterModel = new MonsterModel((Game1)Game,runSkinnedData,dieSkinnedData, runModel,dieModel, monsterUnit);

            return monsterModel;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }
        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            foreach (CModel skModel in monsters)
                skModel.Update(gameTime);
            player.Update(gameTime);
            UpdateShots(gameTime);
            sky.Update(gameTime);

            

            base.Update(gameTime);
        }

        public void AddBullet(Vector3 position, Vector3 direction)
        {
            bullets.Add(new CModel((Game1)Game, Game.Content.Load<Model>("ammo"),
                new BulletUnit((Game1)Game,position, Vector3.Zero, 10*Vector3.One, direction)));
 
        }

        protected void UpdateShots(GameTime gameTime)
        {
            // Loop through shots
            for (int i = 0; i < bullets.Count; ++i)
            {
                // Update each shot
                bullets[i].Update(gameTime);

                // If shot is out of bounds, remove it from game
                if (!((BulletUnit)(bullets[i].unit)).isInRange(player.unit.position.X,
                    player.unit.position.Z,bulletRange))
                {
                    bullets.RemoveAt(i);
                    --i;
                }
                else
                {
                    // If shot is still in play, check for collisions
                    for (int j = 0; j < monsters.Count; ++j)
                    {
                        if (bullets[i].unit.collideWith(monsters[j].unit))
                        {

                            //TODO: Collision! add an explosion.

                            // Collision! Remove the ship and the shot. 
                            //monsters.RemoveAt(j);
                            ((Game1)Game).monsterDie();
                            bullets.RemoveAt(i);
                            --i;
                            break;
                        }
                    }
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            sky.Draw(gameTime);

            terrain.Draw(gameTime);

            foreach (CModel skModel in monsters)
                //if (camera.BoundingVolumeIsInView(skModel.unit.BoundingSphere))
                skModel.Draw(gameTime);

            foreach (CModel skModel in bullets)
                //if (camera.BoundingVolumeIsInView(skModel.unit.BoundingSphere))
                skModel.Draw(gameTime);

            player.Draw(gameTime);

            



            base.Draw(gameTime);
        }


    }
}
