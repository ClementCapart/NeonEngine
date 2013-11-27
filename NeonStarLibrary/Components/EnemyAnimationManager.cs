using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    public class EnemyAnimationManager : Component
    {
        public Enemy EnemyComponent = null;
        public int LastAttackHashCode = 0;

        public EnemyAnimationManager(Entity entity)
            : base(entity, "EnemyAnimationManager")
        {
        }

        public override void Init()
        {
            EnemyComponent = entity.GetComponent<Enemy>();
            base.Init();
        }

        public override void PostUpdate(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if(entity.spritesheets != null)
            {
                entity.spritesheets.ChangeSide(EnemyComponent.CurrentSide);

                switch (EnemyComponent.State)
                {
                    case EnemyState.Attacking:
                        break;

                    case EnemyState.Wait:
                    case EnemyState.WaitNode:
                    case EnemyState.WaitThreat:
                    case EnemyState.Idle:
                        entity.spritesheets.ChangeAnimation(EnemyComponent.IdleAnim, false);
                        break;

                    case EnemyState.Patrol:
                    case EnemyState.Chase:
                    case EnemyState.FinishChase:
                    case EnemyState.MustFinishChase:
                        entity.spritesheets.ChangeAnimation(EnemyComponent.RunAnim);
                        break;

                    case EnemyState.Dying:
                        break;

                    case EnemyState.StunLocked:
                        break;
                }
            }
            

            base.PostUpdate(gameTime);
        }
    }
}
