using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MyGame
{
    /// <summary>
    /// This class represent a custom Drawable Component that has a CModel that handle drawing and a Unit that hanlde updating
    /// of the Drawable game component.
    /// </summary>
    public class CDrawableComponent : DrawableGameComponent
    {
        public CModel cModel;
        public Unit unit;
        protected MyGame myGame;

        public CDrawableComponent(MyGame game,Unit unit, CModel model)
            :base(game)
        {
            myGame = game;
            this.unit = unit;
            this.cModel = model;
            unit.BoundingSphere = cModel.buildBoundingSphere();
            unit.BoundingBox = cModel.buildBoundingBox();
        }

        /// <summary>
        /// This method renders the current state.
        /// </summary>
        /// <param name="gameTime">The elapsed game time.</param>
        public override void Draw(GameTime gameTime)
        {
            //if (myGame.camera.BoundingVolumeIsInView(unit.BoundingSphere))
            if (myGame.camera.BoundingVolumeIsInView(unit.BoundingBox))
            {
                cModel.Draw(gameTime);
                base.Draw(gameTime);
            }
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // Calculate the base transformation by combining translation, rotation, and scaling
            cModel.updateBaseWorld(unit.position, unit.rotation, unit.scale, unit.baseWorld);

            unit.update(gameTime);
            base.Update(gameTime);
        }

    }
}
