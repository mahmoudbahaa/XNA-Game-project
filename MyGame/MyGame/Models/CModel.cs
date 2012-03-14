using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Helper;


namespace MyGame
{
    public class CModel: IEvent
    {
        public Model Model { get; set; }

        protected Matrix baseWorld { get; set; }

        protected Matrix[] modelTransforms;

        protected List<Event> events;

        protected Game1 myGame;

        public CModel(Game1 game, Model Model)
        {
            this.Model = Model;

            baseWorld = Matrix.Identity;


            modelTransforms = new Matrix[Model.Bones.Count];
            Model.CopyAbsoluteBoneTransformsTo(modelTransforms);

            generateTags();

            myGame = game;

            events = new List<Event>();
        }

        public void updateBaseWorld(Vector3 position, Vector3 rotation, Vector3 scale, Matrix baseWorld)
        {
            this.baseWorld = baseWorld * Matrix.CreateScale(scale)
                * Matrix.CreateFromYawPitchRoll(rotation.Y, rotation.X, rotation.Z)
                * Matrix.CreateTranslation(position);
        }
        public virtual void Draw(GameTime game)
        {
            foreach (ModelMesh mesh in Model.Meshes)
            {
                Matrix localWorld = modelTransforms[mesh.ParentBone.Index]
                    * this.baseWorld;

                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    Effect effect = meshPart.Effect;
                    if (effect is BasicEffect)
                    {
                        ((BasicEffect)effect).World = localWorld;
                        ((BasicEffect)effect).View = myGame.camera.View;
                        ((BasicEffect)effect).Projection = myGame.camera.Projection;
                        ((BasicEffect)effect).EnableDefaultLighting();
                    }
                    else
                    {
                        setEffectParameter(effect, "World", localWorld);
                        setEffectParameter(effect, "View", myGame.camera.View);
                        setEffectParameter(effect, "Projection", myGame.camera.Projection);
                        setEffectParameter(effect, "CameraPosition", myGame.camera.Position);
                    }
                }

                mesh.Draw();
            }
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

        public void addEvent(Event ev)
        {
            events.Add(ev);
        }

        public BoundingSphere buildBoundingSphere()
        {
            BoundingSphere sphere = new BoundingSphere(Vector3.Zero, 0);

            // Merge all the model's built in bounding spheres
            foreach (ModelMesh mesh in Model.Meshes)
            {
                BoundingSphere transformed = mesh.BoundingSphere.Transform(
                    modelTransforms[mesh.ParentBone.Index]);

                sphere = BoundingSphere.CreateMerged(sphere, transformed);
            }
            return sphere;
        }

        private void generateTags()
        {
            foreach (ModelMesh mesh in Model.Meshes)
                foreach (ModelMeshPart part in mesh.MeshParts)
                    if (part.Effect is BasicEffect)
                    {
                        BasicEffect effect = (BasicEffect)part.Effect;
                        MeshTag tag = new MeshTag(effect.DiffuseColor,
                            effect.Texture, effect.SpecularPower);
                        part.Tag = tag;
                    }
        }
    }

 

    public class MeshTag
    {
        public Vector3 Color;
        public Texture2D Texture;
        public float SpecularPower;
        public Effect CachedEffect = null;

        public MeshTag(Vector3 Color, Texture2D Texture, float SpecularPower)
        {
            this.Color = Color;
            this.Texture = Texture;
            this.SpecularPower = SpecularPower;
        }
    }
}
