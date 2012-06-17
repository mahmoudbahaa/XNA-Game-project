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
        private SpriteBatch spriteBatch;
        private Texture2D crossHairTex;
        private DelayedAction delayedAction;

        public int health
        {
            get
            {
                return ((PlayerUnit)unit).health;
            }

            set
            {
                ((PlayerUnit)unit).health = value;
            }
        }


        public Player(MyGame game, SkinnedModel skinnedModel, Unit unit)
            : base(game, unit, new PlayerModel(game, skinnedModel))
        {
            foreach (ModelMesh mesh in skinnedModel.Model.Meshes)
                foreach (SkinnedEffect effect in mesh.Effects)
                    effect.EnableDefaultLighting();

            spriteBatch = new SpriteBatch(game.GraphicsDevice);
            crossHairTex = game.Content.Load<Texture2D>("crosshair");
            delayedAction = new DelayedAction(800);
            //run at first to show to the character otherwise the character dont show
            playerRun();
        }

        public override void Update(GameTime gameTime)
        {
            if (myGame.paused)
                return;
            ((AnimatedModel)cModel).animationController.Update(gameTime.ElapsedGameTime, Matrix.Identity);
            //Custom Update
            ((ChaseCamera)myGame.camera).Move(unit.position,  unit.rotation + new Vector3(0,MathHelper.Pi,0));

            KeyboardState keyBoard = Keyboard.GetState();
            if (keyBoard.IsKeyDown(Keys.W) || myGame.controller.isActive(Controller.FORWARD))
            {
                playerRun();
                controlForward();
            }
            if (keyBoard.IsKeyDown(Keys.S) || myGame.controller.isActive(Controller.BACKWARD))
            {
                playerRun();
                controlBackward();
            }
            if (keyBoard.IsKeyDown(Keys.A) || myGame.controller.isActive(Controller.LEFT))
            {
                playerRun();
                controlLeft();
            }
            if (keyBoard.IsKeyDown(Keys.D) || myGame.controller.isActive(Controller.RIGHT))
            {
                playerRun();
                controlRight();
            }

            FireShots(gameTime);
   
            base.Update(gameTime);
        }

        protected void FireShots(GameTime gameTime)
        {
            if (((PlayerModel)cModel).shooting)
            {
                //Vector3 dir = Vector3.Normalize(myGame.camera.Target - myGame.camera.Position);
                //float rotz = (float)Math.Atan2(dir.Y, dir.X);
                myGame.mediator.fireEvent(MyEvent.C_ATTACK_BULLET_END, "position", unit.position,
                    "rotation", new Vector3(0, unit.rotation.Y, 0));
                ((PlayerModel)cModel).shooting = false;
            }
            if (delayedAction.eventHappened(gameTime, Keyboard.GetState().IsKeyDown(Keys.Space) ||
                                            Mouse.GetState().LeftButton == ButtonState.Pressed ||
                                            myGame.controller.isActive(Controller.RIGHT_HAND_STR)))
            {
                myGame.mediator.fireEvent(MyEvent.C_ATTACK_BULLET_BEGIN);
            }
        }

        // Helper used by the Update method to refresh the WorldTransforms data.
        public Matrix RHandTransformation()
        {
            return ((PlayerModel)cModel).RHandTransformation();
        }

        public void playerRun()
        {
            myGame.mediator.fireEvent(MyEvent.P_RUN);
        }

        public void controlForward()
        {
            myGame.mediator.fireEvent(MyEvent.C_FORWARD);
        }

        public void controlBackward()
        {
            myGame.mediator.fireEvent(MyEvent.C_BACKWARD);
        }

        public void controlLeft()
        {
            myGame.mediator.fireEvent(MyEvent.C_LEFT);
        }

        public void controlRight()
        {
            myGame.mediator.fireEvent(MyEvent.C_RIGHT);
        }


        private void DrawCrossHair()
        {
            int x = (Game.GraphicsDevice.Viewport.Width - 50) / 2;
            int y = (Game.GraphicsDevice.Viewport.Height - 50) / 2;
            Rectangle rect = new Rectangle(x, y, 50, 50);
            spriteBatch.Draw(crossHairTex, rect, Color.White);
        }

        private void DrawHP()
        {
            Rectangle rect = new Rectangle(0, 0, 300, 50);
            spriteBatch.Draw(HPBillboardSystem.getTexture(health), rect , Color.White);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            DrawHP();
            DrawCrossHair();
            spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
