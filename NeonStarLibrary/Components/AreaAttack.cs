using Microsoft.Xna.Framework;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    public class AreaAttack : EnemyAttack
    {
        #region Properties
        private float _maximumAngleDifference = 360.0f;

        public float MaximumAngleDifference
        {
            get { return _maximumAngleDifference; }
            set { _maximumAngleDifference = value; }
        }
        #endregion

        public AreaAttack(Entity entity)
            :base(entity)
        {
            Name = "AreaAttack";
        }

        public override void PreUpdate(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (EntityToAttack != null)
            {
                switch(EnemyComponent.State)
                {
                    case EnemyState.Chase:
                    case EnemyState.FinishChase:
                    case EnemyState.Wait:
                    case EnemyState.WaitNode:
                        if (CheckIfEntityIsInRange())
                        {
                            EnemyComponent.State = EnemyState.Attacking;
                            if (EntityToAttack.transform.Position.X < entity.transform.Position.X)
                                EnemyComponent.CurrentSide = Side.Left;
                            else
                                EnemyComponent.CurrentSide = Side.Right;
                        }
                        break;

                    case EnemyState.Idle:
                    case EnemyState.Patrol:
                        if (CheckIfEntityIsInRange())
                        {
                            EnemyComponent.State = EnemyState.WaitThreat;
                            if (EntityToAttack.transform.Position.X < entity.transform.Position.X)
                                EnemyComponent.CurrentSide = Side.Left;
                            else
                                EnemyComponent.CurrentSide = Side.Right;
                        }
                        break;

                    case EnemyState.WaitThreat:
                        if (CheckIfEntityIsInRange())
                        {
                            if (EnemyComponent.WaitThreatTimer >= EnemyComponent.WaitThreatDuration)
                            {
                                EnemyComponent.State = EnemyState.Attacking;
                            }
                            if (EntityToAttack.transform.Position.X < entity.transform.Position.X)
                                EnemyComponent.CurrentSide = Side.Left;
                            else
                                EnemyComponent.CurrentSide = Side.Right;
                        }
                        else
                        {
                            if (EnemyComponent.Chase != null) EnemyComponent.State = EnemyState.Idle;
                        }
                        break;
                }
            }
            base.PreUpdate(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            if (CurrentAttack != null)
            {
                CurrentAttack.Update(gameTime);
                if (CurrentAttack.CooldownFinished)
                {
                    CurrentAttack = null;
                }
            }
            else
            {
                switch (EnemyComponent.State)
                {
                    case EnemyState.Attacking:
                        ChooseAttack();
                        break;
                }
            }
            base.Update(gameTime);
        }

        public override void PostUpdate(GameTime gameTime)
        {
            if (CurrentAttack == null && EnemyComponent.State == EnemyState.Attacking)
                EnemyComponent.State = EnemyState.Wait;

            if (EnemyComponent.State != EnemyState.Attacking && CurrentAttack != null)
            {
                CurrentAttack.CancelAttack();
                CurrentAttack = null;
            }
            base.PostUpdate(gameTime);
        }

        public bool CheckIfEntityIsInRange()
        {
            float distanceSquared = Vector2.DistanceSquared(EntityToAttack.transform.Position, entity.transform.Position);
            if (distanceSquared < _rangeForAttackOne * _rangeForAttackOne)
            {
                float angle = (Neon.utils.AngleBetween(entity.transform.Position, EntityToAttack.transform.Position) - entity.transform.rotation);
                if (angle < 0)
                    angle = angle + (float)Math.PI * 2;
                while (angle > Math.PI * 2)
                    angle = angle - (float)Math.PI * 2;
                float lowerAngle = (-_maximumAngleDifference) % 360;
                lowerAngle = lowerAngle < 0 ? lowerAngle + 360 : lowerAngle;
                lowerAngle = MathHelper.ToRadians(lowerAngle);
                if (lowerAngle <= angle && Math.PI * 2 >= angle)
                    return true;
            }

            return false;
        }

        public void ChooseAttack()
        {
            if (_rangeForAttackFive != 0.0f && Vector2.DistanceSquared(entity.transform.Position, EntityToAttack.transform.Position) < _rangeForAttackFive * _rangeForAttackFive)
            {
                if (_attackToLaunchFive == "Chase")
                {
                    EnemyComponent.State = EnemyState.Chase;
                }
                else
                {
                    bool inLocalCooldown = false;
                    foreach (Attack a in LocalAttacksInCooldown)
                    {
                        if (a.Name == _attackToLaunchFive)
                            inLocalCooldown = true;
                    }

                    if (!inLocalCooldown)
                        CurrentAttack = AttacksManager.GetAttack(_attackToLaunchFive, EnemyComponent.CurrentSide, entity, EntityToAttack, true);
                }
            }
            else if (_rangeForAttackFour != 0.0f && Vector2.DistanceSquared(entity.transform.Position, EntityToAttack.transform.Position) < _rangeForAttackFour * _rangeForAttackFour)
            {
                if (_attackToLaunchFour == "Chase")
                {
                    EnemyComponent.State = EnemyState.Chase;
                }
                else
                {
                    bool inLocalCooldown = false;
                    foreach (Attack a in LocalAttacksInCooldown)
                    {
                        if (a.Name == _attackToLaunchFour)
                            inLocalCooldown = true;
                    }

                    if (!inLocalCooldown)
                        CurrentAttack = AttacksManager.GetAttack(_attackToLaunchFour, EnemyComponent.CurrentSide, entity, EntityToAttack, true);
                }
            }
            else if (_rangeForAttackThree != 0.0f && Vector2.DistanceSquared(entity.transform.Position, EntityToAttack.transform.Position) < _rangeForAttackThree * _rangeForAttackThree)
            {
                if (_attackToLaunchThree == "Chase")
                {
                    EnemyComponent.State = EnemyState.Chase;
                }
                else
                {
                    bool inLocalCooldown = false;
                    foreach (Attack a in LocalAttacksInCooldown)
                    {
                        if (a.Name == _attackToLaunchThree)
                            inLocalCooldown = true;
                    }

                    if (!inLocalCooldown)
                        CurrentAttack = AttacksManager.GetAttack(_attackToLaunchThree, EnemyComponent.CurrentSide, entity, EntityToAttack, true);
                }
            }
            else if (_rangeForAttackTwo != 0.0f && Vector2.DistanceSquared(entity.transform.Position, EntityToAttack.transform.Position) < _rangeForAttackTwo * _rangeForAttackTwo)
            {
                if (_attackToLaunchTwo == "Chase")
                {
                    EnemyComponent.State = EnemyState.Chase;
                }
                else
                {
                    bool inLocalCooldown = false;
                    foreach (Attack a in LocalAttacksInCooldown)
                    {
                        if (a.Name == _attackToLaunchTwo)
                            inLocalCooldown = true;
                    }

                    if (!inLocalCooldown)
                        CurrentAttack = AttacksManager.GetAttack(_attackToLaunchTwo, EnemyComponent.CurrentSide, entity, EntityToAttack, true);
                }
            }
            else
            {
                if (_attackToLaunchOne == "Chase")
                {
                    EnemyComponent.State = EnemyState.Chase;
                }
                else
                {
                    bool inLocalCooldown = false;
                    foreach (Attack a in LocalAttacksInCooldown)
                    {
                        if (a.Name == _attackToLaunchOne)
                            inLocalCooldown = true;
                    }

                    if (!inLocalCooldown)
                        CurrentAttack = AttacksManager.GetAttack(_attackToLaunchOne, EnemyComponent.CurrentSide, entity, EntityToAttack, true);
                }
            }
        }


    }


}
