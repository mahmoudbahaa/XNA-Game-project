using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using System.Collections;
using System;
using Helper;
using control;
using XNAnimation;

namespace MyGame
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;

        public Camera camera;
        public Controller controller;
        public Mediator mediator;
        public Player player;
        public bool paused = false;
        public bool gameOver = false;

        private Terrain terrain;
        private MonstersManager monsters;

        private ScoreBoard scoreBoard;
        //assal

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            controller = new Controller(Constants.LEFT_HAND);

            //DONT Remove i need this.--Mahmoud Bahaa
            if (System.IO.File.Exists("fbDeprofiler.dll"))
                fbDeprofiler.DeProfiler.Run();

            mediator = new Mediator();
        }

        private Player initializePlayer()
        {
            SkinnedModel pmodelIdle = Content.Load<SkinnedModel>(@"model/PlayerMarineIdle");
            SkinnedModel pmodelRun  = Content.Load<SkinnedModel>(@"model/PlayerMarineRun");
            SkinnedModel pmodelAim  = Content.Load<SkinnedModel>(@"model/PlayerMarineAim");
            SkinnedModel pmodelShoot = Content.Load<SkinnedModel>(@"model/PlayerMarineShoot");
            //SkinningData skinnedData = pmodel.Tag as SkinningData;
            PlayerUnit playerUnit = new PlayerUnit(this, new Vector3(0, 50, 0),
                new Vector3(0, 0, 0),
                new Vector3(2f));
            Player player = new Player(this, pmodelIdle, pmodelRun ,pmodelAim,pmodelShoot, playerUnit);
            return player;
        }

        private Sky intitializeSky()
        {
            TextureCube tc = Content.Load<TextureCube>("clouds");
            Model pmodel = Content.Load<Model>("skysphere_mesh");
            SkyUnit skyUnit = new SkyUnit(this, Vector3.Zero, Vector3.Zero, new Vector3(10000));
            Sky sky = new Sky(this, pmodel, skyUnit, tc);

            return sky;
        }

        // Called when the game should load its content
        protected override void LoadContent()
        {
            //camera = new FreeCamera(this, new Vector3(0, 0, 0), 0, 0, 0 , 0);
            //camera = new FreeCamera(new Vector3(400, 600, 400), MathHelper.ToRadians(45), MathHelper.ToRadians(-30), GraphicsDevice);
            camera  = new ChaseCamera(this, new Vector3(0, 20, 200), new Vector3(0, 50, 0), new Vector3(0, 0, 0));
            player  = initializePlayer();
            Sky sky = intitializeSky();

            Weapon weapon = new Weapon(this, player, Content.Load<Model>("model//WeaponMachineGun"),
                new Unit(this, Vector3.Zero, Vector3.Zero,Vector3.One));
            terrain = new Terrain(this, camera, Content.Load<Texture2D>("terrain"), 10, 100,
               Content.Load<Texture2D>("grass"), 100, new Vector3(1, -1, 0));
            BulletsManager bullets = new BulletsManager(this);
            scoreBoard = new ScoreBoard(this);
            monsters = new MonstersManager(this);

            StateManager stateManager = new StateManager(this);
            AudioManager audioManager = new AudioManager(this);
            //CDrawableComponent test = new CDrawableComponent(this,
            //    new Unit(this, new Vector3(0, 100, 0), Vector3.Zero, Vector3.One * 100),
            //    new CModel(this, Content.Load<Model>(@"hp")));

            Components.Add(camera);
            Components.Add(sky);
            Components.Add(terrain);
            Components.Add(monsters);
            Components.Add(bullets);
            Components.Add(weapon);
            Components.Add(player);
            Components.Add(scoreBoard);
            Components.Add(stateManager);
            Components.Add(audioManager);
            //Components.Add(test);
        }

        public bool checkCollisionWithBullet(Unit unit)
        {
            return (monsters.checkCollisionWithBullet(unit));
        }

        protected override void  EndRun()
        {
            GestureManager.running = false;
 	         base.EndRun();
        }

        public float GetHeightAtPosition(float X, float Z)
        {
            return terrain.GetHeightAtPosition(X, Z);
        }

        public float GetHeightAtPosition2(float X, float Z)
        {
            float steepness ;
            return terrain.GetHeightAtPosition(X, Z,out steepness);
        }
    }
}
