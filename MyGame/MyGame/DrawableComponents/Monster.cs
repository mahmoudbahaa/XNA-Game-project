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

        public Monster(Game1 game, SkinnedModel skinnedModel, Unit unit)
            : base(game, unit, new MonsterModel(game, skinnedModel))
        {
            monsterModel = ((MonsterModel)cModel);
            monsterUnit = ((MonsterUnit)unit);

            foreach (ModelMesh mesh in skinnedModel.Model.Meshes)
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
