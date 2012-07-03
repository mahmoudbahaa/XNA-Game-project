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
        private MyGame myGame;

        // Shot variables
        float shotSpeed = 0.5f;

        float bulletRange = 30000;

        public BulletsManager(MyGame game)
            : base(game)
        {
            bullets = new List<Bullet>();
            myGame = game;
            events = new List<Event>();
            game.mediator.register(this, MyEvent.C_ATTACK_BULLET_END);
        }

        public void AddBullet(Vector3 position,Vector3 rotation, Vector3 direction)
        {
            Bullet bullet = new Bullet(myGame, Game.Content.Load<Model>("projectile"),
                new BulletUnit(myGame, position, rotation, Constants.BULLET_SCALE, direction));
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
                    case (int)MyEvent.C_ATTACK_BULLET_END:
                        Vector3 direction = Vector3.Normalize(myGame.camera.Target - myGame.camera.Position);
                        Vector3 rotation = (Vector3)ev.args["rotation"];
                        Vector3 rotatedDir = Vector3.Transform(direction, Matrix.CreateRotationY(-rotation.Y));
                    //direction.Y += 25;
                        float rotX = (float)Math.Atan2(rotatedDir.Y, rotatedDir.Z);
                        AddBullet((Vector3)ev.args["position"] + Constants.BULLET_OFFSET,
                            rotation + new Vector3(-rotX, 0, 0), direction * shotSpeed);
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
                Vector3 pos = bullets[i].unit.position ;
                if(Math.Abs(pos.Length()) > bulletRange || 
                    myGame.checkCollisionWithBullet(bullets[i].unit) //||
                    /*pos.Y < myGame.GetHeightAtPosition(pos.X,pos.Z)*/ )
                {
                    bullets.RemoveAt(i);
                    --i;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (myGame.paused)
                return;
            FireShots();
            UpdateShots(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (Bullet bullet in bullets)
                bullet.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
