using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkinnedModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Helper;
using control;

namespace MyGame
{
    public class Player : CDrawableComponent
    {
        // Shot variables
        int shotDelay = 300;
        int shotCountdown = 0;

        public Player(Game1 game,SkinningData skinningData, Model model, Unit unit)
            : base(game, unit,new PlayerModel (game,skinningData, model))
        {
            //run at first to show to the character otherwise the character dont show
            playerRun();
        }

        public override void Update(GameTime gameTime)
        {

            //Custom Update
            ((ChaseCamera)myGame.camera).Move(unit.position, unit.rotation);

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
                        myGame.fireEvent(MyEvent.C_ATTACK,"position" ,unit.position);

                        // Reset the shot countdown
                        shotCountdown = shotDelay;
                    }
                }
            }
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
