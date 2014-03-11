using NeonEngine;
using NeonEngine.Components.Graphics2D;
using NeonStarLibrary.Components.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.Enemies
{
    public class EnemyAnimationManager : Component
    {
        #region Properties

        private bool _canChangeSide = true;

        public bool CanChangeSide
        {
            get { return _canChangeSide; }
            set { _canChangeSide = value; }
        }
        #endregion

        public EnemyCore EnemyComponent = null;
        public int LastAttackHashCode = 0;

        public EnemyAnimationManager(Entity entity)
            : base(entity, "EnemyAnimationManager")
        {
            RequiredComponents = new Type[] { typeof(EnemyCore), typeof(SpritesheetManager) };
        }

        public override void Init()
        {
            EnemyComponent = entity.GetComponent<EnemyCore>();
            base.Init();
        }

        public override void PreUpdate(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if(entity.spritesheets != null && _canChangeSide)
                entity.spritesheets.ChangeSide(EnemyComponent.CurrentSide);
            else if (entity.spritesheets != null)
            {
                entity.spritesheets.ChangeSide(Side.Right);
            }
            base.PreUpdate(gameTime);
        }

        public override void PostUpdate(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if(entity.spritesheets != null)
            {
                if (_canChangeSide)
                    entity.spritesheets.ChangeSide(EnemyComponent.CurrentSide);
                else
                    entity.spritesheets.ChangeSide(Side.Right);

                switch (EnemyComponent.State)
                {
                    case EnemyState.Attacking:
                        if (EnemyComponent.Attack != null && EnemyComponent.Attack.CurrentAttack != null /*&& LastAttackHashCode != EnemyComponent.Attack.CurrentAttack.GetHashCode()*/)
                        {                        
                            if (EnemyComponent.Attack.CurrentAttack.Name == EnemyComponent.Attack.AttackToLaunchOne)
                            {
                                if (EnemyComponent.Attack.CurrentAttack.DelayStarted && !EnemyComponent.Attack.CurrentAttack.DelayFinished)
                                {
                                    entity.spritesheets.ChangeAnimation(EnemyComponent.Attack.AttackOneDelayAnimation, false, 0, true, false, false);
                                }
                                else if (EnemyComponent.Attack.CurrentAttack.DurationStarted && !EnemyComponent.Attack.CurrentAttack.DurationFinished)
                                {
                                    entity.spritesheets.ChangeAnimation(EnemyComponent.Attack.AttackOneDurationAnimation, false, 0, true, false, true);
                                }
                                else if (EnemyComponent.Attack.CurrentAttack.CooldownStarted && !EnemyComponent.Attack.CurrentAttack.CooldownFinished)
                                {
                                    entity.spritesheets.ChangeAnimation(EnemyComponent.Attack.AttackOneCooldownAnimation, false, 0, true, false, false);
                                }                             
                            }
                            else if (EnemyComponent.Attack.CurrentAttack.Name == EnemyComponent.Attack.AttackToLaunchTwo)
                            {
                                if (EnemyComponent.Attack.CurrentAttack.DelayStarted && !EnemyComponent.Attack.CurrentAttack.DelayFinished)
                                {
                                    entity.spritesheets.ChangeAnimation(EnemyComponent.Attack.AttackTwoDelayAnimation, false, 0, true, false, false);
                                }
                                else if (EnemyComponent.Attack.CurrentAttack.DurationStarted && !EnemyComponent.Attack.CurrentAttack.DurationFinished)
                                {
                                    entity.spritesheets.ChangeAnimation(EnemyComponent.Attack.AttackTwoDurationAnimation, false, 0, true, false, true);
                                }
                                else if (EnemyComponent.Attack.CurrentAttack.CooldownStarted && !EnemyComponent.Attack.CurrentAttack.CooldownFinished)
                                {
                                    entity.spritesheets.ChangeAnimation(EnemyComponent.Attack.AttackTwoCooldownAnimation, false, 0, true, false, false);
                                }                                           
                            }
                            else if (EnemyComponent.Attack.CurrentAttack.Name == EnemyComponent.Attack.AttackToLaunchThree)
                            {
                                if (EnemyComponent.Attack.CurrentAttack.DelayStarted && !EnemyComponent.Attack.CurrentAttack.DelayFinished)
                                {
                                    entity.spritesheets.ChangeAnimation(EnemyComponent.Attack.AttackThreeDelayAnimation, false, 0, true, false, false);
                                }
                                else if (EnemyComponent.Attack.CurrentAttack.DurationStarted && !EnemyComponent.Attack.CurrentAttack.DurationFinished)
                                {
                                    entity.spritesheets.ChangeAnimation(EnemyComponent.Attack.AttackThreeDurationAnimation, false, 0, true, false, true);
                                }
                                else if (EnemyComponent.Attack.CurrentAttack.CooldownStarted && !EnemyComponent.Attack.CurrentAttack.CooldownFinished)
                                {
                                    entity.spritesheets.ChangeAnimation(EnemyComponent.Attack.AttackThreeCooldownAnimation, false, 0, true, false, false);
                                }              
                            }
                        }

                        if(EnemyComponent.Attack != null && EnemyComponent.Attack.CurrentAttack != null)
                            LastAttackHashCode = EnemyComponent.Attack.CurrentAttack.GetHashCode();

                        break;

                    case EnemyState.Wait:
                    case EnemyState.WaitNode:
                    case EnemyState.WaitThreat:
                    case EnemyState.Idle:
                        if(EnemyComponent.IdleAnim != "")
                            entity.spritesheets.ChangeAnimation(EnemyComponent.IdleAnim, false);
                        else
                            entity.spritesheets.ChangeAnimation(EnemyComponent.RunAnim, false);
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
