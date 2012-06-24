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
    public class MyGame : Microsoft.Xna.Framework.Game, IEvent
    {
        List<Event> events;

        GraphicsDeviceManager graphics;

        public DifficultyConstants difficultyConstants;
        public Camera camera;
        public Controller controller;
        public Mediator mediator;
        public Player player;
        public bool paused = true;
        public bool canPause = true;
        public bool gameOver = false;

        public CameraMode cameraMode = CameraMode.thirdPerson;

        public enum CameraMode
        {
            thirdPerson = 0,
            firstPersonWithWeapon,
            firstPersonWithoutWeapon,
        }

        Random r = new Random();

        public int currentLevel = 1;

        private SkyCube sky;
        private FirstAidManager firstAidManger;
        //private Terrain[] terrain = new Terrain[Constants.NUM_OF_TERRAINS];
        private Terrain terrain;
        private MonstersManager monsters;
        private DelayedAction delayedAction;
        private DelayedAction delayedAction2;
        private ScoreBoard scoreBoard;
        private Weapon weapon;
        private BulletsManager bullets;
        private StateManager stateManager;
        private AudioManager audioManager;
        private Water water;
        private BillboardSystem grass;
        private BillboardSystem clouds;
        private FrameRateCounter frameRateCounter;
        //private BillboardSystem trees;
        private List<CDrawableComponent> trees = new List<CDrawableComponent>();

        private KinectManager kinectManager;
        private StartScreen startScreen;
        private HelpScreen helpScreen;
        private LevelScreen levelScreen;
        private CreditsScreen creditsScreen;

        SpeechRecognizer speechRecognizer;

        public MyGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            controller = new Controller(Constants.LEFT_HAND);

            //DONT Remove i need this.--Mahmoud Bahaa
            if (System.IO.File.Exists("fbDeprofiler.dll"))
                fbDeprofiler.DeProfiler.Run();

            //TargetElapsedTime = TimeSpan.FromMilliseconds(100);

            Window.AllowUserResizing = true;

            mediator = new Mediator();
            events = new List<Event>();
            delayedAction = new DelayedAction(800);
            delayedAction2 = new DelayedAction();
            mediator.register(this, MyEvent.G_StartGame, MyEvent.G_StartScreen, MyEvent.G_HelpScreen, MyEvent.G_CreditScreen,
                MyEvent.G_StartLevel, MyEvent.G_NextLevel, MyEvent.G_NextLevel_END_OF_MUSIC, MyEvent.G_Exit);
            //mediator.fireEvent(MyEvent.G_StartGame);
        }

        private Player initializePlayer()
        {
            SkinnedModel pmodel = Content.Load<SkinnedModel>(@"model/PlayerMarine");
            //SkinningData skinnedData = pmodel.Tag as SkinningData;
            PlayerUnit playerUnit = new PlayerUnit(this, new Vector3(-5, GetHeightAtPosition(5, -5) + 5, 5),
                new Vector3(0, 0, 0),
                Constants.PLAYER_SCALE);
            Player player = new Player(this, pmodel, playerUnit);
            return player;
        }

        private SkyCube intitializeSky()
        {
            Model skymodel = Content.Load<Model>("skysphere_mesh");
            SkyUnit skyCubeUnit = new SkyUnit(this,Vector3.Zero, Vector3.Zero, new Vector3(10000));
            SkyCube sky = new SkyCube(this,skymodel,skyCubeUnit,Content.Load<TextureCube>("clouds"));

            //Model skyDome = Content.Load<Model>("dome");
            //Texture2D cloudMap = Content.Load<Texture2D>("cloudMap");

            //SkyUnit skyUnit = new SkyUnit(this, Vector3.Zero, Vector3.Zero, new Vector3(10000));
            //Sky sky = new Sky(this, skyDome, skyUnit, cloudMap);

            return sky;
        }

        private void initializeTrees()
        {
            foreach (Vector3 pos in initializePositions(30,Vector2.Zero))
            {
                trees.Add(new CDrawableComponent(this, new Unit(this, pos,
                Vector3.Zero,new Vector3(1f)),
                new CModel(this, Content.Load<Model>("tree2"))));
            }
        }

        private List<Vector3> initializePositions(int numOfBillBoards, Vector2 size)
        {
            // Positions where trees should be drawn
            List<Vector3> positions = new List<Vector3>();

            // Continue until we get 500 trees on the terrain
            for (int i = 0; i < numOfBillBoards; i++) // 500
            {
                // Get X and Z coordinates from the random generator, between
                // [-(terrain width) / 2 * (cell size), (terrain width) / 2 * (cell size)]
                float x = r.Next(-Constants.FIELD_MAX_X_Z, Constants.FIELD_MAX_X_Z);
                float z = r.Next(-Constants.FIELD_MAX_X_Z, Constants.FIELD_MAX_X_Z);

                // Get the height and steepness of this position on the terrain,
                // taking the height of the billboard into account
                //float steepness;
                float y = GetHeightAtPosition(x, z/*, out steepness*/) + size.X / 2;

                // Reject this position if it is too low, high, or steep. Otherwise
                // add it to the list
                if (/*steepness < MathHelper.ToRadians(15) &&*/ y >= Constants.WATER_HEIGHT * 1.2 + size.X / 2 && y <= Constants.WATER_HEIGHT * 3)
                    positions.Add(new Vector3(x, y, z));
                else
                    i--;
            }

            return positions;
        }
        private BillboardSystem initializeBillBoard(int numOfBillBoards, String tex, Boolean EnsureOcclusion,
            Vector2 size,bool flag)
        {

            BillboardSystem billboard = new BillboardSystem(this, Content.Load<Texture2D>(tex), size,
                initializePositions(numOfBillBoards, size).ToArray());

            billboard.Mode = BillboardSystem.BillboardMode.Cylindrical;
            billboard.EnsureOcclusion = EnsureOcclusion;
            return billboard;
        }

        private void initializeClouds()
        {
            List<Vector3> cloudPositions = new List<Vector3>();

            // Create 20 "clusters" of clouds
            for (int i = 0; i < 30; i++)
            {
                Vector3 cloudLoc = new Vector3(
                   r.Next(-Constants.FIELD_MAX_X_Z, Constants.FIELD_MAX_X_Z),
                   r.Next(Constants.TERRAIN_HEIGHT*9/10, Constants.TERRAIN_HEIGHT*3/2),
                   r.Next(-Constants.FIELD_MAX_X_Z, Constants.FIELD_MAX_X_Z));

                // Add 10 cloud billboards around each cluster point
                for (int j = 0; j < 10; j++)
                {
                    cloudPositions.Add(cloudLoc +
                        new Vector3(
                            r.Next(-Constants.FIELD_MAX_X_Z / 4, Constants.FIELD_MAX_X_Z/4),
                            r.Next(-Constants.TERRAIN_HEIGHT / 15, Constants.TERRAIN_HEIGHT / 5),
                            r.Next(-Constants.FIELD_MAX_X_Z / 4, Constants.FIELD_MAX_X_Z/4)));
                }
            }

            clouds = new BillboardSystem(this, Content.Load<Texture2D>("cloud2"), new Vector2(200),
                cloudPositions.ToArray());

            clouds.Mode = BillboardSystem.BillboardMode.Spherical;
            clouds.EnsureOcclusion = false;
        }

        public void pause()
        {
            Components.Remove(startScreen);
            Components.Insert(0, startScreen);
        }

        public void resume()
        {
            Components.Remove(startScreen);
        }

        private void initializeGame2()
        {
            initializeGame1();
            Components.Clear();

            Components.Add(camera);
            Components.Add(sky);
            //for (int i = 0; i < Constants.NUM_OF_TERRAINS; i++)
            //    Components.Add(terrain[i]);
            Components.Add(terrain);
            Components.Add(water);
            foreach (CDrawableComponent tree in trees)
                Components.Add(tree);
            Components.Add(grass);
            Components.Add(clouds);
            Components.Add(monsters);
            Components.Add(firstAidManger);
            Components.Add(bullets);
            Components.Add(weapon);
            Components.Add(player);
            Components.Add(scoreBoard);
            Components.Add(stateManager);
            Components.Add(audioManager);
            Components.Add(frameRateCounter);
            Components.Add(kinectManager);

            Components.Insert(0, levelScreen);
            ((ChaseCamera)camera).resetOffsets();
        }

        private void initializeGame1()
        {
            switch (StartScreen.Difficulty)
            {
                case Constants.Difficulties.Novice: difficultyConstants = new NoviceConstants(); break;
                case Constants.Difficulties.Advanced: difficultyConstants = new AdvancedConstants(); break;
                case Constants.Difficulties.Xtreme: difficultyConstants = new XtremeConstants(); break;
            }
            //camera = new FreeCamera(this, new Vector3(0, 0, 0), 0, 0, 0 , 0);
            //camera = new FreeCamera(new Vector3(400, 600, 400), MathHelper.ToRadians(45), MathHelper.ToRadians(-30), GraphicsDevice);
            camera = new ChaseCamera(this, Constants.CAMERA_POSITION_THIRD_PERSON, Constants.CAMERA_TARGET_THIRD_PERSON, Vector3.Zero);

            //for (int i = 0; i < Constants.NUM_OF_TERRAINS; i++)
            //{
                terrain = new Terrain(this, camera, Content.Load<Texture2D>("terrain"+currentLevel), Constants.TERRAIN_CELL_SIZE,
                    Constants.TERRAIN_HEIGHT, Content.Load<Texture2D>("grass"), Constants.TERRAIN_TEXTURE_TILING, 
                    new Vector3(1, -1, 0)/*,new Vector2(i%2*-1,i/2*-1)*/);
                terrain.WeightMap = Content.Load<Texture2D>("weightMap"+currentLevel);
                terrain.RTexture = Content.Load<Texture2D>("sand");
                terrain.GTexture = Content.Load<Texture2D>("rock");
                terrain.BTexture = Content.Load<Texture2D>("snow");
                terrain.DetailTexture = Content.Load<Texture2D>("noise_texture");
            //}
            player = initializePlayer();
            sky = intitializeSky();
            initializeClouds();

            initializeTrees();
            //trees = initializeBillBoard(100, "tree_billboard", true, Constants.TREE_SIZE,true);
            grass = initializeBillBoard(300, "grass_billboard", false, Constants.GRASS_SIZE,false);


            weapon = new Weapon(this, player, Content.Load<Model>("model//WeaponMachineGun"),
                new Unit(this, Vector3.Zero, Vector3.Zero, Vector3.One));
            bullets = new BulletsManager(this);
            scoreBoard = new ScoreBoard(this);
            monsters = new MonstersManager(this);
            firstAidManger = new FirstAidManager(this);



            //testing = new CDrawableComponent(this, new Unit(this, new Vector3(0,terrain.GetHeightAtPosition(0,0),0),
            //    Vector3.Zero,new Vector3(5f)),
            //    new CModel(this, Content.Load<Model>("firtree1")));

            //waterMesh = new CModel(game.Content.Load<Model>("plane"), position,
            //   Vector3.Zero, new Vector3(size.X, 1, size.Y), game.GraphicsDevice);
            water = new Water(this,Content.Load<Model>("plane"),
                new WaterUnit(this, new Vector3(0,Constants.WATER_HEIGHT,0), Vector3.Zero, new Vector3(10240,1,10240)));
            ((WaterUnit)water.unit).Objects.Add(sky);
            //((WaterUnit)water.unit).Objects.Add(terrain);
            stateManager = new StateManager(this);
            audioManager = new AudioManager(this);

            frameRateCounter = new FrameRateCounter(this);

            kinectManager = new KinectManager(this);
            //CDrawableComponent test = new CDrawableComponent(this,
            //    new Unit(this, new Vector3(0, 80, 0), Vector3.Zero, Vector3.One * .5f),
            //    new CModel(this, Content.Load<Model>(@"model/First Aid Kit2")));
            //mediator.fireEvent(MyEvent.G_StartGame);

        }


        private void initializeStartMenu()
        {
            startScreen.reInitialize();
            Components.Remove(creditsScreen);
            Components.Remove(helpScreen);
            Components.Remove(startScreen);
            Components.Insert(0, startScreen);
        }

        private void initializeHelpScreen()
        {
            helpScreen.reInitialize();
            Components.Remove(helpScreen);
            Components.Insert(0, helpScreen);
        }

        private void initializeCreditsScreen()
        {
            creditsScreen.reInitialize();
            Components.Remove(creditsScreen);
            Components.Insert(0, creditsScreen);
        }

        // Called when the game should load its content
        protected override void LoadContent()
        {
            speechRecognizer = new SpeechRecognizer(this);
            helpScreen = new HelpScreen(this);
            startScreen = new StartScreen(this);
            levelScreen = new LevelScreen(this);
            creditsScreen = new CreditsScreen(this);
            initializeStartMenu();
            //initializeGame1();
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Escape))
                Exit();

            foreach (Event ev in events)
            {
                switch (ev.EventId)
                {
                    case (int)MyEvent.G_Exit: Exit(); break;
                    case (int)MyEvent.G_StartGame:
                        {
                            if (paused)
                                initializeGame2();
                            break;
                        }
                    case (int)MyEvent.G_NextLevel:
                        {
                            paused = true;
                            canPause = false;
                            break;
                        }
                    case (int)MyEvent.G_NextLevel_END_OF_MUSIC:
                        {
                            paused = false;
                            canPause = true;
                            if (currentLevel == Constants.NUM_OF_LEVELS)
                            {
                                initializeCreditsScreen();
                            }
                            else
                            {
                                currentLevel++;
                                initializeGame2();
                            }
                            break;
                        }
                    case (int)MyEvent.G_StartLevel:
                        {
                            if(Components.Contains(levelScreen))
                            {
                                Components.Remove(levelScreen);
                                paused = false;
                            }
                            break;
                        }
                    case (int)MyEvent.G_StartScreen:
                        {
                            paused = true;
                            initializeStartMenu();
                            break;
                        }
                    case (int)MyEvent.G_CreditScreen: initializeCreditsScreen(); break;
                    case (int)MyEvent.G_HelpScreen: initializeHelpScreen(); break;
                }
            }
            events.Clear();

            if (delayedAction.eventHappened(gameTime, keyState.IsKeyDown(Keys.RightAlt) &&
                                                    keyState.IsKeyDown(Keys.Enter)))
            {
                graphics.ToggleFullScreen();
            }

            if (delayedAction2.eventHappened(gameTime, keyState, Keys.C))
            {
                if ((int)cameraMode == 2)
                {
                    cameraMode = CameraMode.thirdPerson;
                    ((ChaseCamera)camera).resetOffsets();
                }
                else
                {
                    cameraMode++;
                    ((ChaseCamera)camera).setOffsetsFor1stPerson();
                }

            }

            base.Update(gameTime);
        }

        public void addEvent(Helper.Event ev)
        {
            events.Add(ev);
        }

        public bool checkCollisionWithBullet(Unit unit)
        {
            return (monsters.checkCollisionWithBullet(unit) || firstAidManger.checkCollisionWithBullet(unit));
        }

        public bool checkCollisionWithTrees(Unit unit,int offset)
        {
            foreach (CDrawableComponent comp in trees)
                if (Math.Abs(comp.unit.position.X - unit.position.X) < offset && Math.Abs(comp.unit.position.Z - unit.position.Z) < offset) 
                    return true;
            return false;
            //return (monsters.checkCollisionWithBullet(unit) || firstAidManger.checkCollisionWithBullet(unit));
        }

        protected override void EndRun()
        {
            GestureManager.running = false;
            speechRecognizer.Dispose();
            base.EndRun();
        }

        //public float GetHeightAtPosition2(float X, float Z)
        //{
        //    if (X > -512 && X < 0 && Z > -512 && Z < 0)
        //        return clamp(terrain[3].GetHeightAtPosition(X, Z));
        //    else if (X > -512 && X < 0 && Z > 0 && Z < 512)
        //        return clamp(terrain[1].GetHeightAtPosition(X, Z));
        //    else if (X > -0 && X < 512 && Z > 0 && Z < 512)
        //        return clamp(terrain[0].GetHeightAtPosition(X, Z));
        //    else 
        //        return clamp(terrain[2].GetHeightAtPosition(X, Z));
        //}

        public float GetHeightAtPosition(float X, float Z)
        {
            float steepness;
            //if (Constants.NUM_OF_TERRAINS == 1) 
                return clamp(terrain.GetHeightAtPosition(X, Z, out steepness));
            //if (X > -512 * Constants.TERRAIN_CELL_SIZE && X < 0 &&
            //    Z > -512 * Constants.TERRAIN_CELL_SIZE && Z < 0)
            //    return clamp(terrain[3].GetHeightAtPosition(X, Z, out steepness));
            //else if (X > -512 * Constants.TERRAIN_CELL_SIZE && X < 0 &&
            //         Z > 0 && Z < 512 * Constants.TERRAIN_CELL_SIZE)
            //    return clamp(terrain[1].GetHeightAtPosition(X, Z, out steepness));
            //else if (X >= 0 && X < 512 * Constants.TERRAIN_CELL_SIZE &&
            //         Z >= 0 && Z < 512 * Constants.TERRAIN_CELL_SIZE)
            //    return clamp(terrain[0].GetHeightAtPosition(X, Z, out steepness));
            //else if (X >= 0 && X < 512 * Constants.TERRAIN_CELL_SIZE &&
            //         Z > -512 * Constants.TERRAIN_CELL_SIZE && Z < 0)
            //    return clamp(terrain[2].GetHeightAtPosition(X, Z, out steepness));
            //else
            //    return 0;
            //return clamp(terrain[0].GetHeightAtPosition(X, Z, out steepness)) ;
        }

        private float clamp(float h)
        {
            if (h < (Constants.WATER_HEIGHT - 5))
                h = (Constants.WATER_HEIGHT - 5);
            return h;
        }
    }
}
