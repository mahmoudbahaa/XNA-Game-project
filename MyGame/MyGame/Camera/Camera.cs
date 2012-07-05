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

        /// <summary>
        /// Constructor that initiliaze the projection matrix
        /// </summary>
        /// <param name="game">The instance of MyGame the game component is attached to</param>
        public Camera(MyGame game)
            : base(game)
        {
            this.Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4, Game.GraphicsDevice.Viewport.AspectRatio, 0.1f, 1000000.0f);
            myGame = game;
        }

        /// <summary>
        /// Generate the camera frustrum from the view projection matrix
        /// </summary>
        private void generateFrustum()
        {
            Matrix viewProjection = View * Projection;
            Frustum = new BoundingFrustum(viewProjection);
        }

        /// <summary>
        /// Checks if the bounding sphere is in the camera frustrum or not
        /// </summary>
        /// <param name="sphere">the Bounding sphere to check</param>
        public bool BoundingVolumeIsInView(BoundingSphere sphere)
        {
            return (Frustum.Contains(sphere) != ContainmentType.Disjoint);
        }

        /// <summary>
        /// Checks if the bounding box is in the camera frustrum or not
        /// </summary>
        /// <param name="box">the Bounding box to check</param>
        public bool BoundingVolumeIsInView(BoundingBox box)
        {
            return (Frustum.Contains(box) != ContainmentType.Disjoint);
        }
    }
}
