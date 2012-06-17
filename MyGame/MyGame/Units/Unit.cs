using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Helper;

namespace MyGame
{
    public class Unit : IEvent
    {
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;
        public bool alive = true;

        //public GameTime gameTime { get; set; }

        //matix hold the base world 
        public Matrix baseWorld;

        protected List<Event> events;

        protected MyGame myGame;

        //attribute holding the bounding sphere for this unit model
        protected BoundingSphere boundingSphere;

        public BoundingSphere BoundingSphere
        {
            get
            {
                // No need for rotation, as this is a sphere
                Matrix worldTransform = baseWorld * Matrix.CreateScale(scale)
                    * Matrix.CreateTranslation(position);

                BoundingSphere transformed = boundingSphere;
                transformed = transformed.Transform(worldTransform);

                return transformed;
            }
            set 
            {
                boundingSphere = value;
            }
        }

        public Unit(MyGame game,Vector3 position,Vector3 rotation,Vector3 scale)
        {
            baseWorld = Matrix.Identity;

            this.position = position;
            this.rotation = rotation;
            this.scale = scale;

            this.myGame = game;

            this.events = new List<Event>();
        }

        public virtual void update(GameTime gameTime) 
        {
        }

        public void addEvent(Event ev)
        {
            events.Add(ev);
        }

        public bool collideWith(Unit otherUnit)
        {
            return (BoundingSphere.Contains(otherUnit.BoundingSphere) != ContainmentType.Disjoint);
        }


    }
}
