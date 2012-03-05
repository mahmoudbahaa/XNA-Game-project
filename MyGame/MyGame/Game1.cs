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
        //Terrain terrain;
        MouseState lastMouseState;
        CModelManager modelManager;
        //assal

        Hashtable hash;

        Controller control;

       // PlayerModel playerModel;
        
        // Shot variables
        float shotSpeed = 0.5f;
        int shotDelay = 300;
        int shotCountdown = 0;

        Random r = new Random();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            hash = new Hashtable();
            control = new Controller(Constants.LEFT_HAND);

            //DONT Remove i need this.--Mahmoud Bahaa
            //if(System.IO.File.Exists("fbDeprofiler.dll"))
            //    fbDeprofiler.DeProfiler.Run();
        }

        // Called when the game should load its content
        protected override void LoadContent()
        {
            //camera = new FreeCamera(this, new Vector3(0, 0, 0), 0, 0, 0 , 0);
            //camera = new FreeCamera(new Vector3(400, 600, 400), MathHelper.ToRadians(45), MathHelper.ToRadians(-30), GraphicsDevice);
            camera = new ChaseCamera(this, new Vector3(0, 20, 200), new Vector3(0, 50, 0), new Vector3(0, 0, 0));

            //Model pmodel = Content.Load<Model>(@"Textures\EnemyBeast");
            //SkinningData skinnedData = pmodel.Tag as SkinningData;
            //PlayerUnit playerUnit = new PlayerUnit(this, new Vector3(0, 10, 0), Vector3.Zero, new Vector3(5f));
            //playerModel = new PlayerModel(this, skinnedData, pmodel, playerUnit);
           // playerModel.Run();

            //terrain = new Terrain(this, camera, Content.Load<Texture2D>("terrain"), 150, 0,
            //    Content.Load<Texture2D>("grass"), 100, new Vector3(1, -1, 0));

            //SkySphere sky = new SkySphere(this,camera, Content.Load<TextureCube>("clouds"));


            modelManager = new CModelManager(this);

            lastMouseState = Mouse.GetState();
            Components.Add(camera);
            //Components.Add(sky);
            //Components.Add(terrain);
            Components.Add(modelManager);

            //run at first to show to the character otherwise the character dont show
            playerRun();
        }

        // Called when the game should update itself
        protected override void Update(GameTime gameTime)
        {
            
            updatePlayer();
            updateCamera(modelManager.player);
            FireShots(gameTime, modelManager.player.unit.position);

            base.Update(gameTime);
        }


        public void updateCamera(CModel player)
        {

            ((ChaseCamera)camera).Move(player.unit.position, player.unit.rotation);

            // Get the new keyboard and mouse state
            MouseState mouseState = Mouse.GetState();
            KeyboardState keyState = Keyboard.GetState();

            // Determine how much the camera should turn
            float deltaX ;
            
            float deltaY;


            if (System.Windows.Forms.Control.IsKeyLocked(System.Windows.Forms.Keys.CapsLock))
            {
                Vector2 d = control.getPointer();
                deltaX = d.X;
                deltaY = d.Y;
            }
            else
            {
                deltaX = -((float)lastMouseState.X - (float)mouseState.X) * 15;
                deltaY = ((float)lastMouseState.Y - (float)mouseState.Y) * 15;
            }

            ChaseCamera chaseCamera = (ChaseCamera)camera;
            // Rotate the camera
            chaseCamera.Rotate(new Vector3(deltaY * .0005f, 0, 0));
            //player.unit.position.Y = terrain.GetHeightAtPosition(player.unit.position.X,
              //  player.unit.position.Z);
            controlPointer(- deltaX * .0005f);
            lastMouseState = mouseState;
        }

        private void updatePlayer()
        {
            KeyboardState keyBoard = Keyboard.GetState();
            if (keyBoard.IsKeyDown(Keys.Up) || keyBoard.IsKeyDown(Keys.W)||control.isActive(Controller.FORWARD))
            {
                playerRun();
                controlForward();
            }
            if (keyBoard.IsKeyDown(Keys.Down) || keyBoard.IsKeyDown(Keys.S) || control.isActive(Controller.BACKWARD))
            {
                playerRun();
                controlBackward();
            }
            if (keyBoard.IsKeyDown(Keys.Left) || keyBoard.IsKeyDown(Keys.A) || control.isActive(Controller.LEFT))
            {
                playerRun();
                controlLeft();
            }
            if (keyBoard.IsKeyDown(Keys.Right) || keyBoard.IsKeyDown(Keys.D) || control.isActive(Controller.RIGHT))
            {
                playerRun();
                controlRight();
            }
        }

        protected void FireShots(GameTime gameTime,Vector3 position)
        {
            shotCountdown -= gameTime.ElapsedGameTime.Milliseconds;
            if (shotCountdown <= 0)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Space) ||
                        Mouse.GetState().LeftButton == ButtonState.Pressed ||
                        control.isActive(Controller.RIGHT_HAND_STR))
                {
                    {
                        Vector3 direction = (camera.Target - camera.Position);
                        direction.Y += 25;
                        direction.Normalize();
                        modelManager.AddBullet(position + new Vector3(0, 40, 0), direction * shotSpeed);

                        // Reset the shot countdown
                        shotCountdown = shotDelay;
                    }
                }
            }
                
        }


        // Called when the game should draw itself
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            //playerModel.Draw(gameTime);
            base.Draw(gameTime);
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

        public void playerRun()
        {
            fireEvent(MyEvent.P_RUN);
        }

        public void playerStopRun()
        {
            fireEvent(MyEvent.P_STOP);
        }

        public void controlForward()
        {
            fireEvent(MyEvent.C_FORWARD);
        }

        public void controlBackward()
        {
            fireEvent(MyEvent.C_BACKWARD);
        }

        public void controlLeft()
        {
            fireEvent(MyEvent.C_LEFT);
        }

        public void controlRight()
        {
            fireEvent(MyEvent.C_RIGHT);
        }

        public void controlAttack()
        {
            fireEvent(MyEvent.C_ATTACK);
        }

        public void controlPointer(float deltaX)
        {
            fireEvent(MyEvent.C_Pointer,"deltaX", deltaX);
        }


    }
}
