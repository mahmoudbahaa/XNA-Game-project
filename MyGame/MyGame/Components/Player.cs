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
        public Player(Game1 game,SkinningData skinningData, Model model, Unit unit)
            : base(game, unit)
        {
            cModel =  new PlayerModel (game,skinningData, model);
            unit.BoundingSphere = cModel.buildBoundingSphere();
            //run at first to show to the character otherwise the character dont show
            playerRun();
        }

        public override void Update(GameTime gameTime)
        {
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

            base.Update(gameTime);
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
