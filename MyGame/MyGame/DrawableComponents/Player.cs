using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Helper;
using control;
using XNAnimation;

namespace MyGame
{
    public class Player : CDrawableComponent
    {
        // Shot variables
        int shotDelay = 300;
        int shotCountdown = 0;


        public Player(Game1 game, SkinnedModel idleSkinnedModel, SkinnedModel runSkinnedModel,
                        SkinnedModel aimSkinnedModel, SkinnedModel shootSkinnedModel, Unit unit)
            : base(game, unit, new PlayerModel(game, idleSkinnedModel, runSkinnedModel,
                                                aimSkinnedModel, shootSkinnedModel))
        {
            // THe dwarf model is slightly broken so we set the textures manually.
            //Texture2D axe = game.Content.Load<Texture2D>("Textures\\axe");
            //Texture2D dwarf = game.Content.Load<Texture2D>("Textures\\dwarf");

            foreach (ModelMesh mesh in idleSkinnedModel.Model.Meshes)
            {
                foreach (SkinnedEffect effect in mesh.Effects)
                {
                    //effect.Texture = dwarf;

                    //if (mesh.Name == "axe")
                    //{
                    //    effect.Texture = axe;
                    //}

                    effect.EnableDefaultLighting();

                    //effect.SpecularColor = new Vector3(0.25f);
                    //effect.SpecularPower = 16;
                }
            }

            foreach (ModelMesh mesh in runSkinnedModel.Model.Meshes)
                foreach (SkinnedEffect effect in mesh.Effects)
                    effect.EnableDefaultLighting();

            foreach (ModelMesh mesh in aimSkinnedModel.Model.Meshes)
                foreach (SkinnedEffect effect in mesh.Effects)
                    effect.EnableDefaultLighting();

            foreach (ModelMesh mesh in shootSkinnedModel.Model.Meshes)
                foreach (SkinnedEffect effect in mesh.Effects)
                    effect.EnableDefaultLighting();

            //run at first to show to the character otherwise the character dont show
            playerRun();
        }

        public override void Update(GameTime gameTime)
        {
            ((AnimatedModel)cModel).animationController.Update(gameTime.ElapsedGameTime, Matrix.Identity);
            //Custom Update
            ((ChaseCamera)myGame.camera).Move(unit.position,  unit.rotation + new Vector3(0,MathHelper.Pi,0));

            KeyboardState keyBoard = Keyboard.GetState();
            if (keyBoard.IsKeyDown(Keys.Up) || keyBoard.IsKeyDown(Keys.W) || myGame.controller.isActive(Controller.FORWARD))
            {
                playerRun();
                controlForward();
            }
            if (keyBoard.IsKeyDown(Keys.Down) || keyBoard.IsKeyDown(Keys.S) || myGame.controller.isActive(Controller.BACKWARD))
            {
                playerRun();
                controlBackward();
            }
            if (keyBoard.IsKeyDown(Keys.Left) || keyBoard.IsKeyDown(Keys.A) || myGame.controller.isActive(Controller.LEFT))
            {
                playerRun();
                controlLeft();
            }
            if (keyBoard.IsKeyDown(Keys.Right) || keyBoard.IsKeyDown(Keys.D) || myGame.controller.isActive(Controller.RIGHT))
            {
                playerRun();
                controlRight();
            }

            unit.position.Y = myGame.GetHeightAtPosition(unit.position.X,
                unit.position.Z) + 5;

            FireShots(gameTime);



            base.Update(gameTime);
        }

        

        protected void FireShots(GameTime gameTime)
        {
            shotCountdown -= gameTime.ElapsedGameTime.Milliseconds;
            if (shotCountdown <= 0)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Space) ||
                        Mouse.GetState().LeftButton == ButtonState.Pressed ||
                        myGame.controller.isActive(Controller.RIGHT_HAND_STR))
                {
                    {
                        myGame.fireEvent(MyEvent.C_ATTACK_BULLET,"position" ,unit.position);

                        // Reset the shot countdown
                        shotCountdown = shotDelay;
                    }
                }

                if (Keyboard.GetState().IsKeyDown(Keys.D1))
                {
                    myGame.fireEvent(MyEvent.C_ATTACK_AXE);
                    myGame.fireEvent(MyEvent.P_ATTACK_AXE1);
                }
                if (Keyboard.GetState().IsKeyDown(Keys.D2))
                {
                    myGame.fireEvent(MyEvent.C_ATTACK_AXE);
                    myGame.fireEvent(MyEvent.P_ATTACK_AXE2);
                }
                if (Keyboard.GetState().IsKeyDown(Keys.D3))
                {
                    myGame.fireEvent(MyEvent.C_ATTACK_AXE);
                    myGame.fireEvent(MyEvent.P_ATTACK_AXE3);
                }
                if (Keyboard.GetState().IsKeyDown(Keys.D4))
                {
                    myGame.fireEvent(MyEvent.C_ATTACK_AXE);
                    myGame.fireEvent(MyEvent.P_ATTACK_AXE4);
                }
                if (Keyboard.GetState().IsKeyDown(Keys.D5))
                {
                    myGame.fireEvent(MyEvent.C_ATTACK_AXE);
                    myGame.fireEvent(MyEvent.P_ATTACK_AXE5);
                }

            }
        }

        // Helper used by the Update method to refresh the WorldTransforms data.
        public Matrix RHandTransformation()
        {
            return ((PlayerModel)cModel).RHandTransformation();
        }

        public void playerRun()
        {
            myGame.fireEvent(MyEvent.P_RUN);
        }

        public void playerStopRun()
        {
            myGame.fireEvent(MyEvent.P_STOP);
        }

        public void controlForward()
        {
            myGame.fireEvent(MyEvent.C_FORWARD);
        }

        public void controlBackward()
        {
            myGame.fireEvent(MyEvent.C_BACKWARD);
        }

        public void controlLeft()
        {
            myGame.fireEvent(MyEvent.C_LEFT);
        }

        public void controlRight()
        {
            myGame.fireEvent(MyEvent.C_RIGHT);
        }


    }
}
