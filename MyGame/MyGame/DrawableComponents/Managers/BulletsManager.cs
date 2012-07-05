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
    /// <summary>
    /// This class represent the bullet manager that manages creating
    /// and calling updates on bullets, also removing bullet at collision detected.
    /// </summary>
    public class BulletsManager : DrawableGameComponent,IEvent
    {
        protected List<Event> events;

        private List<Bullet> bullets;
        private MyGame myGame;

        // Shot variables
        float shotSpeed = 0.5f;

        float bulletRange = 3000;

        /// <summary>
        /// Constructor that initialize the bullet manager
        /// </summary>
        /// <param name="game">Instance of MyGame this game component is attached to</param>
        public BulletsManager(MyGame game)
            : base(game)
        {
            bullets = new List<Bullet>();
            myGame = game;
            events = new List<Event>();
            game.mediator.register(this, MyEvent.C_ATTACK_BULLET_END);
        }

        /// <summary>
        /// Add a new bullet at the specified position, rotation and direction
        /// </summary>
        /// <param name="position">initial position of the new bullet</param>
        /// <param name="rotation">rotation of the new bullet</param>
        /// <param name="direction">Direction in which the bullet will move</param>
        public void AddBullet(Vector3 position,Vector3 rotation, Vector3 direction)
        {
            Bullet bullet = new Bullet(myGame, Game.Content.Load<Model>("projectile"),
                new BulletUnit(myGame, position, rotation, Constants.BULLET_SCALE, direction));
            bullets.Add(bullet);

        }

        /// <summary>
        /// Add event to the event queue
        /// </summary>
        /// <param name="ev">Event to be added</param>
        public void addEvent(Event ev)
        {
            events.Add(ev);
        }

        /// <summary>
        /// Fire a new bullet if the specified event(C_ATTACK_BULLET_END) was received
        /// </summary>
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

        /// <summary>
        /// update the bullet if the bulltet out of range or collide with the monster remove it
        /// </summary>
        /// <param name="gameTime">The game time</param>
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
                    bullets[i].Dispose();
                    bullets.RemoveAt(i);
                    
                    --i;
                }
            }
        }

        /// <summary>
        /// Allows the component to run logic.
        /// </summary>
        /// <param name="gameTime">The gametime.</param>
        public override void Update(GameTime gameTime)
        {
            if (myGame.paused)
                return;
            FireShots();
            UpdateShots(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This method renders the current state.
        /// </summary>
        /// <param name="gameTime">The elapsed game time.</param>
        public override void Draw(GameTime gameTime)
        {
            foreach (Bullet bullet in bullets)
                bullet.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
