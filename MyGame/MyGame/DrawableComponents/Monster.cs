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
    public class Monster : CDrawableComponent
    {
        public int health = 100;

        private MonsterModel monsterModel;
        public MonsterUnit monsterUnit;

        public MonsterModel.MonsterAnimations ActiveAnimation
        {
            get 
            {
                return monsterModel.activeAnimation;
            }
        }

        public Monster(Game1 game, SkinnedModel idleSkinnedModel, SkinnedModel runSkinnedModel,
                        SkinnedModel biteSkinnedModel, SkinnedModel takeDamageSkinnedModel,
                        SkinnedModel dieSkinnedModel, Unit unit)
            : base(game, unit, new MonsterModel(game, idleSkinnedModel, runSkinnedModel, biteSkinnedModel,
                                                takeDamageSkinnedModel, dieSkinnedModel))
        {
            monsterModel = ((MonsterModel)cModel);
            monsterUnit = ((MonsterUnit)unit);

            foreach (ModelMesh mesh in runSkinnedModel.Model.Meshes)
                foreach (SkinnedEffect effect in mesh.Effects)
                    effect.EnableDefaultLighting();

            foreach (ModelMesh mesh in idleSkinnedModel.Model.Meshes)
                foreach (SkinnedEffect effect in mesh.Effects)
                    effect.EnableDefaultLighting();

            foreach (ModelMesh mesh in biteSkinnedModel.Model.Meshes)
                foreach (SkinnedEffect effect in mesh.Effects)
                    effect.EnableDefaultLighting();

            foreach (ModelMesh mesh in takeDamageSkinnedModel.Model.Meshes)
                foreach (SkinnedEffect effect in mesh.Effects)
                    effect.EnableDefaultLighting();


            foreach (ModelMesh mesh in dieSkinnedModel.Model.Meshes)
                foreach (SkinnedEffect effect in mesh.Effects)
                    effect.EnableDefaultLighting();
        }

        public override void Update(GameTime gameTime)
        {
            monsterModel.animationController.Update(gameTime.ElapsedGameTime, Matrix.Identity);

            if ((monsterModel.activeAnimation == MonsterModel.MonsterAnimations.TakeDamage ||
                monsterModel.activeAnimation == MonsterModel.MonsterAnimations.Bite)&&
                !monsterModel.animationController.IsPlaying)
            {
                if (monsterModel.isRunning)
                {
                    monsterUnit.moving = true;
                    monsterModel.Run();
                }
                else
                {
                    monsterUnit.moving = false;
                    monsterModel.Idle();
                }
            }

            Vector3 pos = unit.position;
            unit.position.Y = myGame.GetHeightAtPosition(pos.X, pos.Z);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public void Idle()
        {
            monsterModel.Idle();
        }

        public void Run()
        {
            monsterModel.Run();
        }

        public void Bite()
        {
            monsterModel.Bite();
        }

        public void TakeDamage()
        {
            monsterModel.TakeDamage();
        }

        public void Die()
        {
            monsterModel.Die();
        }
    }
}
