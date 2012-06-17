using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MyGame
{
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
        }

        public override void Draw(GameTime gameTime)
        {
            if(myGame.camera.BoundingVolumeIsInView(unit.BoundingSphere))
                cModel.Draw(gameTime);
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            // Calculate the base transformation by combining translation, rotation, and scaling
            cModel.updateBaseWorld(unit.position, unit.rotation, unit.scale, unit.baseWorld);

            unit.update(gameTime);
            base.Update(gameTime);
        }

    }
}
