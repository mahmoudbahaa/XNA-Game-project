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

        Effect effect;

        public SkyModel(Game1 game, Model model, Unit unit, TextureCube Texture)
            :base(game,model,unit)
        {
            effect = game.Content.Load<Effect>("skysphere_effect");
            effect.Parameters["CubeMap"].SetValue(Texture);
            SetModelEffect(effect, false);
        }

        public override void Draw(GameTime gameTime)
        {
            // Calculate the base transformation by combining translation, rotation, and scaling
            updateBaseWorld(unit.position, unit.rotation, unit.scale, unit.baseWorld);
            Game.GraphicsDevice.DepthStencilState = DepthStencilState.None;
            foreach (ModelMesh mesh in Model.Meshes)
            {
                Matrix localWorld = modelTransforms[mesh.ParentBone.Index] * baseWorld;

                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    Effect effect = meshPart.Effect;                    
                    setEffectParameter(effect, "World", localWorld);
                    setEffectParameter(effect, "View", ((Game1)Game).camera.View);
                    setEffectParameter(effect, "Projection", ((Game1)Game).camera.Projection);
                    setEffectParameter(effect, "CameraPosition", ((Game1)Game).camera.Position);
                    
                }
                mesh.Draw();
            }
            Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
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

        public void SetModelEffect(Effect effect, bool CopyEffect)
        {
            foreach (ModelMesh mesh in Model.Meshes)
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    Effect toSet = effect;

                    // Copy the effect if necessary
                    if (CopyEffect)
                        toSet = effect.Clone();

                    MeshTag tag = ((MeshTag)part.Tag);

                    // If this ModelMeshPart has a texture, set it to the effect
                    if (tag.Texture != null)
                    {
                        setEffectParameter(toSet, "BasicTexture", tag.Texture);
                        setEffectParameter(toSet, "TextureEnabled", true);
                    }
                    else
                    {
                        setEffectParameter(toSet, "TextureEnabled", false);
                    }

                    // Set our remaining parameters to the effect
                    setEffectParameter(toSet, "DiffuseColor", tag.Color);
                    setEffectParameter(toSet, "SpecularPower", tag.SpecularPower);

                    part.Effect = toSet;
                }
        }


    }
}
