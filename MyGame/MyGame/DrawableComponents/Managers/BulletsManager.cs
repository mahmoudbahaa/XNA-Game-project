using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Helper;
using control;

namespace MyGame
{
    public class BulletsManager : DrawableGameComponent,IEvent
    {
        protected List<Event> events;

        private List<Bullet> bullets;
        private Game1 myGame;

        // Shot variables
        float shotSpeed = 0.5f;

        float bulletRange = 3000;

        public BulletsManager(Game1 game)
            : base(game)
        {
            bullets = new List<Bullet>();
            myGame = game;
            events = new List<Event>();
            game.register(this, MyEvent.C_ATTACK_BULLET);
        }

        public void AddBullet(Vector3 position, Vector3 direction)
        {
            Bullet bullet = new Bullet(myGame, Game.Content.Load<Model>("ammo"),
                new BulletUnit(myGame, position, Vector3.Zero, 10 * Vector3.One, direction));
            bullets.Add(bullet);

        }

        public void addEvent(Event ev)
        {
            events.Add(ev);
        }

        protected void FireShots()
        {
            foreach (Event ev in events)
            {
                switch (ev.EventId)
                {
                    case (int)MyEvent.C_ATTACK_BULLET:
                        Vector3 direction = (myGame.camera.Target - myGame.camera.Position);
                        direction.Y += 25;
                        direction.Normalize();
                        AddBullet((Vector3)ev.args["position"] + new Vector3(0, 40, 0), direction * shotSpeed);
                        break;
                }
            }
            events.Clear();
        }

        protected void UpdateShots(GameTime gameTime)
        {
            // Loop through shots
            for (int i = 0; i < bullets.Count; ++i)
            {
                // Update each shot
                bullets[i].Update(gameTime);

                 //If shot is out of bounds, remove it from game
                //if (!((BulletUnit)(bullets[i].unit)).isInRange(myGame.player.unit.position.X,
                //    myGame.player.unit.position.Z, bulletRange))
                Vector3 pos = bullets[i].unit.position ;
                if(Math.Abs(pos.Length()) > bulletRange || 
                    pos.Y < myGame.GetHeightAtPosition(pos.X,pos.Z))
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
            FireShots();
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
