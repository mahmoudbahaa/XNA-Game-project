using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Helper;
using control;
using XNAnimation;

namespace MyGame
{
    public class Weapon : CDrawableComponent
    {
        Player player;

        public Weapon(Game1 game,Player player, Model model, Unit unit)
            : base(game, unit, new CModel(game, model))
        {
            this.player = player;
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);
        }

        public override void Update(GameTime gameTime)
        {
            
            //cModel.baseWorld = player.unit.baseWorld * player.RHandTransformation();
            //Vector3 transform = Vector3.Transform(Vector3.Zero, player.RHandTransformation());

            Vector3 cameraDirection = myGame.camera.Target - myGame.camera.Position;
            Vector3 perp =  Vector3.Normalize(Vector3.Cross(cameraDirection, Vector3.Up));
            unit.baseWorld = player.unit.baseWorld * player.RHandTransformation();
            unit.position = player.unit.position - 2 * player.unit.scale * perp;
            unit.rotation = player.unit.rotation;// +Matrix.Invert(player.RHandTransformation()).Translation;
            //unit.baseWorld = player.RHandTransformation();
            unit.scale = player.unit.scale;//new Vector3(2f);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if(myGame.cameraMode != Game1.CameraMode.firstPersonWithoutWeapon)
                base.Draw(gameTime);
        }
    }
}
