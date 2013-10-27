using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    public class EnemyAttack : Component
    {
        public Enemy EnemyComponent;
        public Attack CurrentAttack;

        private string _attackToLaunch;

        public string AttackToLaunch
        {
            get { return _attackToLaunch; }
            set { _attackToLaunch = value; }
        }

        public EnemyAttack(Entity entity)
            :base(entity, "EnemyAttack")
        {
        }

        public override void Init()
        {
            EnemyComponent = entity.GetComponent<Enemy>();
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (EnemyComponent.State == EnemyState.Attack)
            {
                if (CurrentAttack != null)
                {
                    if (CurrentAttack.CooldownFinished && CurrentAttack.AirLockFinished && (entity.spritesheets.CurrentSpritesheet.IsLooped || !entity.spritesheets.CurrentSpritesheet.IsLooped && entity.spritesheets.IsFinished()))
                    {
                        CurrentAttack = null;
                        entity.spritesheets.CurrentPriority = 0;
                    }
                    else
                        CurrentAttack.Update(gameTime);
                }
                else
                    CurrentAttack = AttacksManager.GetAttack(_attackToLaunch, entity.spritesheets.CurrentSide, entity);
            }
            else
            {
                if(CurrentAttack != null ) CurrentAttack.CancelAttack();
                CurrentAttack = null;
            }
            base.Update(gameTime);
        }


    }
}
