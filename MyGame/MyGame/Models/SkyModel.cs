using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MyGame
{
    class SkyModel : CModel
    {

        //Effect effect;
        Texture2D cloudMap;

        public SkyModel(MyGame game, Model model, Texture2D cloudMap)
            :base(game,model)
        {
            this.cloudMap = cloudMap;
            model.Meshes[0].MeshParts[0].Effect = myGame.Content.Load<Effect>("Series4Effects");
            //effect = game.Content.Load<Effect>("skysphere_effect");
            //effect.Parameters["CubeMap"].SetValue(Texture);
            //SetModelEffect(effect, false);
        }

        public override void Draw(GameTime gameTime)
        {
            myGame.GraphicsDevice.DepthStencilState = DepthStencilState.None;

            Matrix[] modelTransforms = new Matrix[Model.Bones.Count];
            Model.CopyAbsoluteBoneTransformsTo(modelTransforms);

            Matrix wMatrix = Matrix.CreateTranslation(0, -0.3f, 0) * Matrix.CreateScale(10) * baseWorld;
            foreach (ModelMesh mesh in Model.Meshes)
            {
                foreach (Effect currentEffect in mesh.Effects)
                {
                    Matrix worldMatrix = modelTransforms[mesh.ParentBone.Index] * wMatrix;
                    currentEffect.CurrentTechnique = currentEffect.Techniques["Textured"];
                    currentEffect.Parameters["xWorld"].SetValue(worldMatrix);
                    currentEffect.Parameters["xView"].SetValue(myGame.camera.View);
                    currentEffect.Parameters["xProjection"].SetValue(myGame.camera.Projection);
                    currentEffect.Parameters["xTexture"].SetValue(cloudMap);
                    currentEffect.Parameters["xEnableLighting"].SetValue(false);
                }
                mesh.Draw();
            }

            

            //base.Draw(gameTime);

            myGame.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
        }
        // Sets the specified effect parameter to the given effect, if it has that parameter
        void setEffectParameter(Effect effect, string paramName, object val)
        {
            if (effect.Parameters[paramName] == null)
                return;

            if (val is Vector3)
                effect.Parameters[paramName].SetValue((Vector3)val);
            else if (val is bool)
                effect.Parameters[paramName].SetValue((bool)val);
            else if (val is Matrix)
                effect.Parameters[paramName].SetValue((Matrix)val);
            else if (val is Texture2D)
                effect.Parameters[paramName].SetValue((Texture2D)val);
        }

        //public void SetModelEffect(Effect effect, bool CopyEffect)
        //{
        //    foreach (ModelMesh mesh in Model.Meshes)
        //        foreach (ModelMeshPart part in mesh.MeshParts)
        //        {
        //            Effect toSet = effect;

        //            // Copy the effect if necessary
        //            if (CopyEffect)
        //                toSet = effect.Clone();

        //            MeshTag tag = ((MeshTag)part.Tag);

        //            // If this ModelMeshPart has a texture, set it to the effect
        //            if (tag.Texture != null)
        //            {
        //                setEffectParameter(toSet, "BasicTexture", tag.Texture);
        //                setEffectParameter(toSet, "TextureEnabled", true);
        //            }
        //            else
        //            {
        //                setEffectParameter(toSet, "TextureEnabled", false);
        //            }

        //            // Set our remaining parameters to the effect
        //            setEffectParameter(toSet, "DiffuseColor", tag.Color);
        //            setEffectParameter(toSet, "SpecularPower", tag.SpecularPower);

        //            part.Effect = toSet;
        //        }
        //}


    }
}
