using NeonEngine;
using NeonStarLibrary.Components.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.Enemies
{
    public class WeaknessDuringAttack : Component
    {
        #region Properties
        private string _weaknessAttack = "";

        public string WeaknessAttack
        {
            get { return _weaknessAttack; }
            set { _weaknessAttack = value; }
        }

        private string _attackWeakTo = "";

        public string AttackWeakTo
        {
            get { return _attackWeakTo; }
            set { _attackWeakTo = value; }
        }

        private float _damageModifier = 2.0f;

        public float DamageModifier
        {
            get { return _damageModifier; }
            set { _damageModifier = value; }
        }
        #endregion

        EnemyCore EnemyComponent;
        public bool CurrentlyWeak = false;

        public WeaknessDuringAttack(Entity entity)
            : base(entity, "WeaknessDuringAttack")
        {
        }

        public override void Init()
        {
            EnemyComponent = entity.GetComponent<EnemyCore>();
            base.Init();
        }

        public override void PreUpdate(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (EnemyComponent != null && EnemyComponent.Attack != null && EnemyComponent.Attack.CurrentAttack != null)
            {
                if (EnemyComponent.Attack.CurrentAttack.Name == _weaknessAttack)
                {
                    if (EnemyComponent.Attack.CurrentAttack.CooldownStarted && !EnemyComponent.Attack.CurrentAttack.CooldownFinished)
                    {
                        CurrentlyWeak = true;
                    }
                    else
                        CurrentlyWeak = false;
                }
                else
                    CurrentlyWeak = false;
            }
            else
                CurrentlyWeak = false;
            base.PreUpdate(gameTime);
        }
    }
}
