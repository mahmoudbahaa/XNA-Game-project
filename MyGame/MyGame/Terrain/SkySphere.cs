//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework;

//namespace MyGame
//{
//    public class SkySphere : DrawableGameComponent
//    {
//        CModel model;
//        Effect effect;
//        Camera camera;

//        public SkySphere(Game game,Camera camera, TextureCube Texture):base(game)
//        {
//            model = new CModel((Game1)game,Game.Content.Load<Model>("skysphere_mesh"),
//                Vector3.Zero, Vector3.Zero, Vector3.One);

//            this.camera = camera;
//            effect = Game.Content.Load<Effect>("skysphere_effect");
//            effect.Parameters["CubeMap"].SetValue(Texture);

//            model.SetModelEffect(effect, false);
//        }

//        public override void Draw(GameTime gameTime)
//        {
//            // Disable the depth buffer
//            Game.GraphicsDevice.DepthStencilState = DepthStencilState.None;

//            // Move the model with the sphere
//            model.position = camera.Position;

//            model.Draw(camera.View, camera.Projection, camera.Position);

//            Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

//            base.Draw(gameTime);
//        }
//    }
//}
