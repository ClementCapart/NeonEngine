using NeonEngine;
using NeonStarLibrary.Components.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.Triggers
{
    public class DamageOnTrigger : Component
    {
        #region Properties
        private string _enemyToDamageName = "";

        public string EnemyToDamageName
        {
            get { return _enemyToDamageName; }
            set { _enemyToDamageName = value; }
        }

        private float _damageToInflict = 1.0f;

        public float DamageToInflict
        {
            get { return _damageToInflict; }
            set { _damageToInflict = value; }
        }

        #endregion

        private Entity _enemyToDamage;
        private EnemyCore _enemyComponent;

        public DamageOnTrigger(Entity entity)
            :base(entity, "DamageOnTrigger")
        {

        }

        public override void Init()
        {
            _enemyToDamage = entity.GameWorld.GetEntityByName(_enemyToDamageName);
            if (_enemyToDamage != null)
                _enemyComponent = _enemyToDamage.GetComponent<EnemyCore>();
            base.Init();
        }

        public override void OnTrigger(Entity trigger, Entity triggeringEntity, object[] parameters = null)
        {
            if (_enemyComponent != null && _enemyComponent.State != EnemyState.Dying && _enemyComponent.State != EnemyState.Dead)
                _enemyComponent.TakeDamage(_damageToInflict * -1, 0.0f, false, 0.0f, Side.Right);
            base.OnTrigger(trigger, triggeringEntity, parameters);
        }

    }
}
