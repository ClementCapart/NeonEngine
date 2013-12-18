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

        public override void PreUpdate(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if(entity.spritesheets != null)
                entity.spritesheets.ChangeSide(EnemyComponent.CurrentSide);
            base.PreUpdate(gameTime);
        }

        public override void PostUpdate(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if(entity.spritesheets != null)
            {
                entity.spritesheets.ChangeSide(EnemyComponent.CurrentSide);

                switch (EnemyComponent.State)
                {
                    case EnemyState.Attacking:
                        if (LastAttackHashCode != EnemyComponent.Attack.CurrentAttack.GetHashCode())
                        {
                            if (EnemyComponent.Attack.CurrentAttack.Name == EnemyComponent.Attack.AttackToLaunchOne)
                            {
                                entity.spritesheets.ChangeAnimation(EnemyComponent.Attack.AttackOneAnimation, true, 0, true, false, true);
                            }
                            else if (EnemyComponent.Attack.CurrentAttack.Name == EnemyComponent.Attack.AttackToLaunchTwo)
                            {
                                entity.spritesheets.ChangeAnimation(EnemyComponent.Attack.AttackTwoAnimation, true, 0, true, false, true);
                            }
                            else if (EnemyComponent.Attack.CurrentAttack.Name == EnemyComponent.Attack.AttackToLaunchThree)
                            {
                                entity.spritesheets.ChangeAnimation(EnemyComponent.Attack.AttackThreeAnimation, true, 0, true, false, true);
                            }
                            else if (EnemyComponent.Attack.CurrentAttack.Name == EnemyComponent.Attack.AttackToLaunchFour)
                            {
                                entity.spritesheets.ChangeAnimation(EnemyComponent.Attack.AttackFourAnimation, true, 0, true, false, true);
                            }
                            else if (EnemyComponent.Attack.CurrentAttack.Name == EnemyComponent.Attack.AttackToLaunchFive)
                            {
                                entity.spritesheets.ChangeAnimation(EnemyComponent.Attack.AttackFiveAnimation, true, 0, true, false, true);
                            }
                        }
                        

                        LastAttackHashCode = EnemyComponent.Attack.CurrentAttack.GetHashCode();
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
                        entity.spritesheets.ChangeAnimation(EnemyComponent.HitAnim, true, 0, true, false, false);
                        break;
                }
            }
            

            base.PostUpdate(gameTime);
        }
    }
}
