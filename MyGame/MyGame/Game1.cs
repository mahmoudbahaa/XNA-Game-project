using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using System.Collections;
using System;
using Helper;
using SkinnedModel;
using control;

namespace MyGame
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;

        public Camera camera;
        public Controller controller;

        private Terrain terrain;
        private Player player;
        private MonstersManager monsters;

        private ScoreBoard scoreBoard;
        //assal

        Hashtable hash;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            hash = new Hashtable();
            controller = new Controller(Constants.LEFT_HAND);

            //DONT Remove i need this.--Mahmoud Bahaa
            if (System.IO.File.Exists("fbDeprofiler.dll"))
                fbDeprofiler.DeProfiler.Run();
        }

        private Player initializePlayer()
        {
            Model pmodel = Content.Load<Model>("dude");
            SkinningData skinnedData = pmodel.Tag as SkinningData;
            PlayerUnit playerUnit = new PlayerUnit(this, new Vector3(0, 5, 0), Vector3.Zero, new Vector3(1f));
            Player player= new Player(this, skinnedData, pmodel, playerUnit);
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
            terrain = new Terrain(this, camera, Content.Load<Texture2D>("terrain"), 10, 1000,
               Content.Load<Texture2D>("grass"), 100, new Vector3(1, -1, 0));
            BulletsManager bullets = new BulletsManager(this);
            scoreBoard = new ScoreBoard(this);
            monsters = new MonstersManager(this);

            //CDrawableComponent test = new CDrawableComponent(this,
            //    new Unit(this, new Vector3(0,100,0), Vector3.Zero, Vector3.One*100),
            //    new CModel(this,Content.Load<Model>(@"model/Dwarf")));

            Components.Add(camera);
            Components.Add(sky);
            Components.Add(terrain);
            Components.Add(monsters);
            Components.Add(bullets);
            Components.Add(player);
            Components.Add(scoreBoard);
            //Components.Add(test);
        }

        public bool checkCollisionWithBullet(BulletUnit bulletUnit)
        {
            return (monsters.checkCollisionWithBullet(bulletUnit));
        }

        public void register(IEvent ie,params int[] eventKey)
        {
            foreach (int ev in eventKey)
            {
                if (hash[ev] != null)
                {
                    ((List<IEvent>)hash[ev]).Add(ie);
                }
                else
                {
                    List<IEvent> list = new List<IEvent>();
                    list.Add(ie);
                    hash[ev] = list;
                }
            }
        }

        public void fireEvent(int ev,params Object[] param)
        {
            if (hash[ev] == null) return;
            List<IEvent> list = (List<IEvent>)hash[ev];
            Event eve = new Event(ev,param);
            foreach (IEvent ie in list)
            {
                ie.addEvent(eve);
            }
        }

        public void controlPointer(float deltaX)
        {
            fireEvent(MyEvent.C_Pointer,"deltaX", deltaX);
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
