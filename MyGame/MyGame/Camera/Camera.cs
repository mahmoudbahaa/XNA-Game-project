using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame
{
    /// <summary>
    /// This class represent an abstract Camera 
    /// </summary>
    public abstract class Camera : Microsoft.Xna.Framework.GameComponent
    {
        Matrix view;
        Matrix projection;
        //Vector3 position;

        public Vector3 Position;

        public Vector3 Target;

        protected MyGame myGame; 

        public Matrix Projection
        {
            get { return projection; }
            protected set
            {
                projection = value;
                generateFrustum();
            }
        }

        public Matrix View
        {
            get { return view; }
            protected set
            {
                view = value;
                generateFrustum();
            }
        }

        public BoundingFrustum Frustum { get; private set; }
        
        public Camera(MyGame game)
            : base(game)
        {
            generatePerspectiveProjectionMatrix(MathHelper.PiOver4);
            myGame = game;
        }

        private void generatePerspectiveProjectionMatrix(float FieldOfView)
        {
            this.Projection = Matrix.CreatePerspectiveFieldOfView(
                FieldOfView, Game.GraphicsDevice.Viewport.AspectRatio, 0.1f, 1000000.0f);
        }


        private void generateFrustum()
        {
            Matrix viewProjection = View * Projection;
            Frustum = new BoundingFrustum(viewProjection);
        }

        public bool BoundingVolumeIsInView(BoundingSphere sphere)
        {
            return (Frustum.Contains(sphere) != ContainmentType.Disjoint);
        }

        public bool BoundingVolumeIsInView(BoundingBox box)
        {
            return (Frustum.Contains(box) != ContainmentType.Disjoint);
        }
    }
}
