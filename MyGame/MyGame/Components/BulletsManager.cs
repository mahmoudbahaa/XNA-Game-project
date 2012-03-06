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
    public class BulletsManager : DrawableGameComponent
    {
        private List<Bullet> bullets;
        private Game1 myGame;

        // Shot variables
        float shotSpeed = 0.5f;
        int shotDelay = 300;
        float bulletRange = 3000;
        int shotCountdown = 0;

        public BulletsManager(Game1 game)
            : base(game)
        {
            bullets = new List<Bullet>();
            myGame = game;
        }

        public void AddBullet(Vector3 position, Vector3 direction)
        {
            Bullet bullet = new Bullet(myGame, Game.Content.Load<Model>("ammo"),
                new BulletUnit(myGame, position, Vector3.Zero, 10 * Vector3.One, direction));
            bullets.Add(bullet);

        }

        protected void FireShots(GameTime gameTime, Vector3 position)
        {
            shotCountdown -= gameTime.ElapsedGameTime.Milliseconds;
            if (shotCountdown <= 0)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Space) ||
                        Mouse.GetState().LeftButton == ButtonState.Pressed ||
                        myGame.controller.isActive(Controller.RIGHT_HAND_STR))
                {
                    {
                        Vector3 direction = (myGame.camera.Target - myGame.camera.Position);
                        direction.Y += 25;
                        direction.Normalize();
                        AddBullet(position + new Vector3(0, 40, 0), direction * shotSpeed);

                        // Reset the shot countdown
                        shotCountdown = shotDelay;
                    }
                }
            }
        }

        protected void UpdateShots(GameTime gameTime)
        {
            // Loop through shots
            for (int i = 0; i < bullets.Count; ++i)
            {
                // Update each shot
                bullets[i].Update(gameTime);

                // If shot is out of bounds, remove it from game
                if (!((BulletUnit)(bullets[i].unit)).isInRange(myGame.player.unit.position.X,
                    myGame.player.unit.position.Z, bulletRange))
                {
                    bullets.RemoveAt(i);
                    --i;
                }
                else
                {
                    if (myGame.checkCollisionWithBullet((BulletUnit)bullets[i].unit))
                    {
                        myGame.fireEvent(MyEvent.M_DIE);
                        bullets.RemoveAt(i);
                        --i;
                        break;
                    }
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            FireShots(gameTime, myGame.player.unit.position);
            UpdateShots(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (Bullet bullet in bullets)
                //if (camera.BoundingVolumeIsInView(skModel.unit.BoundingSphere))
                bullet.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
