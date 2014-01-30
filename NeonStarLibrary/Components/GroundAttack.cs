using Microsoft.Xna.Framework;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.Enemies
{
    public class GroundAttack : EnemyAttack
    {
        #region Properties
        private bool _checkBothSide = true;

        public bool CheckBothSide
        {
            get { return _checkBothSide; }
            set { _checkBothSide = value; }
        }

        private Vector2 _detectionOffset = Vector2.Zero;

        public Vector2 DetectionOffset
        {
          get { return _detectionOffset; }
          set { _detectionOffset = value; }
        }
        #endregion

        public GroundAttack(Entity entity)
            :base(entity)
        {
            Name = "GroundAttack";
        }

        public override void PreUpdate(Microsoft.Xna.Framework.GameTime gameTime)
        { 
            if(EntityToAttack != null && entity.rigidbody.isGrounded)
            {
                switch (EnemyComponent.State)
                {
                    case EnemyState.Chase:
                    case EnemyState.FinishChase:
                    case EnemyState.Wait:
                    case EnemyState.WaitNode:
                        if (_checkBothSide)
                        {
                            Entity[] hitEntities;
                            if (EnemyComponent.Chase != null)
                            {
                                hitEntities = EnemyComponent.UniqueRaycast(_detectionOffset, EnemyComponent.Chase.DetectionDistance, true);
                            }
                            else
                            {
                                hitEntities = EnemyComponent.UniqueRaycast(_detectionOffset, _rangeForAttackOne, true);
                            }

                            if (hitEntities[0] == EntityToAttack && Math.Abs(EntityToAttack.transform.Position.X - entity.transform.Position.X) < _rangeForAttackOne)
                            {
                                EnemyComponent.State = EnemyState.Attacking;
                                EnemyComponent.CurrentSide = Side.Right;
                            }
                            else if (hitEntities[1] == EntityToAttack && Math.Abs(EntityToAttack.transform.Position.X - entity.transform.Position.X) < _rangeForAttackOne)
                            {
                                EnemyComponent.State = EnemyState.Attacking;
                                EnemyComponent.CurrentSide = Side.Left;
                            }
                            else if (entity.hitboxes[0].hitboxRectangle.Intersects(EntityToAttack.hitboxes[0].hitboxRectangle))
                            {
                                EnemyComponent.State = EnemyState.Attacking;
                                if (entity.transform.Position.X < EntityToAttack.transform.Position.X)
                                    EnemyComponent.CurrentSide = Side.Right;
                                else
                                    EnemyComponent.CurrentSide = Side.Left;
                            }
                        }
                        else
                        {
                            Entity[] hitEntities;
                            if (EnemyComponent.Chase != null)
                            {
                                hitEntities = EnemyComponent.UniqueRaycast(_detectionOffset, EnemyComponent.Chase.DetectionDistance, false);
                            }
                            else
                            {
                                hitEntities = EnemyComponent.UniqueRaycast(_detectionOffset, _rangeForAttackOne, false);
                            }
                            if (hitEntities[0] == EntityToAttack && Math.Abs(EntityToAttack.transform.Position.X - entity.transform.Position.X) < _rangeForAttackOne)
                            {
                                EnemyComponent.State = EnemyState.Attacking;
                            }
                            else if (entity.hitboxes[0].hitboxRectangle.Intersects(EntityToAttack.hitboxes[0].hitboxRectangle))
                            {
                                EnemyComponent.State = EnemyState.Attacking;
                                if (entity.transform.Position.X < EntityToAttack.transform.Position.X)
                                    EnemyComponent.CurrentSide = Side.Right;
                                else
                                    EnemyComponent.CurrentSide = Side.Left;
                            }
                        }
                        break;

                    case EnemyState.Idle:
                    case EnemyState.Patrol:
                        if (_checkBothSide)
                        {
                            Entity[] hitEntities;
                            if (EnemyComponent.Chase != null)
                            {
                                hitEntities = EnemyComponent.UniqueRaycast(_detectionOffset, EnemyComponent.Chase.DetectionDistance, true);
                            }
                            else
                            {
                                hitEntities = EnemyComponent.UniqueRaycast(_detectionOffset, _rangeForAttackOne, true);
                            }

                            if (hitEntities[0] == EntityToAttack && Math.Abs(EntityToAttack.transform.Position.X - entity.transform.Position.X) < _rangeForAttackOne)
                            {
                                EnemyComponent.State = EnemyState.WaitThreat;
                                EnemyComponent.CurrentSide = Side.Right;
                            }
                            else if (hitEntities[1] == EntityToAttack && Math.Abs(EntityToAttack.transform.Position.X - entity.transform.Position.X) < _rangeForAttackOne)
                            {
                                EnemyComponent.State = EnemyState.WaitThreat;
                                EnemyComponent.CurrentSide = Side.Left;
                            }
                            else if (entity.hitboxes[0].hitboxRectangle.Intersects(EntityToAttack.hitboxes[0].hitboxRectangle))
                            {
                                EnemyComponent.State = EnemyState.WaitThreat;
                                if (entity.transform.Position.X < EntityToAttack.transform.Position.X)
                                    EnemyComponent.CurrentSide = Side.Right;
                                else
                                    EnemyComponent.CurrentSide = Side.Left;
                            }
                        }
                        else
                        {
                            Entity[] hitEntities;
                            if (EnemyComponent.Chase != null)
                            {
                                hitEntities = EnemyComponent.UniqueRaycast(_detectionOffset, EnemyComponent.Chase.DetectionDistance, false);
                            }
                            else
                            {
                                hitEntities = EnemyComponent.UniqueRaycast(_detectionOffset, _rangeForAttackOne, false);
                            }
                            if (hitEntities[0] == EntityToAttack && Math.Abs(EntityToAttack.transform.Position.X - entity.transform.Position.X) < _rangeForAttackOne)
                            {
                                EnemyComponent.State = EnemyState.WaitThreat;
                            }
                            else if (entity.hitboxes[0].hitboxRectangle.Intersects(EntityToAttack.hitboxes[0].hitboxRectangle))
                            {
                                EnemyComponent.State = EnemyState.WaitThreat;
                                if (entity.transform.Position.X < EntityToAttack.transform.Position.X)
                                    EnemyComponent.CurrentSide = Side.Right;
                                else
                                    EnemyComponent.CurrentSide = Side.Left;
                            }
                        }
                        break;

                    case EnemyState.WaitThreat:
                        if (EnemyComponent.WaitThreatTimer > EnemyComponent.WaitThreatDuration)
                        {
                            if (_checkBothSide)
                            {
                                Entity[] hitEntities;
                                if (EnemyComponent.Chase != null)
                                {
                                    hitEntities = EnemyComponent.UniqueRaycast(_detectionOffset, EnemyComponent.Chase.DetectionDistance, true);
                                }
                                else
                                {
                                    hitEntities = EnemyComponent.UniqueRaycast(_detectionOffset, _rangeForAttackOne, true);
                                }

                                if (hitEntities[0] == EntityToAttack && Math.Abs(EntityToAttack.transform.Position.X - entity.transform.Position.X) < _rangeForAttackOne)
                                {
                                    EnemyComponent.State = EnemyState.Attacking;
                                    EnemyComponent.CurrentSide = Side.Right;
                                }
                                else if (hitEntities[1] == EntityToAttack && Math.Abs(EntityToAttack.transform.Position.X - entity.transform.Position.X) < _rangeForAttackOne)
                                {
                                    EnemyComponent.State = EnemyState.Attacking;
                                    EnemyComponent.CurrentSide = Side.Left;
                                }
                                else if (entity.hitboxes[0].hitboxRectangle.Intersects(EntityToAttack.hitboxes[0].hitboxRectangle))
                                {
                                    EnemyComponent.State = EnemyState.Attacking;
                                    if (entity.transform.Position.X < EntityToAttack.transform.Position.X)
                                        EnemyComponent.CurrentSide = Side.Right;
                                    else
                                        EnemyComponent.CurrentSide = Side.Left;
                                }
                            }
                            else
                            {
                                Entity[] hitEntities;
                                if (EnemyComponent.Chase != null)
                                {
                                    hitEntities = EnemyComponent.UniqueRaycast(_detectionOffset, EnemyComponent.Chase.DetectionDistance, false);
                                }
                                else
                                {
                                    hitEntities = EnemyComponent.UniqueRaycast(_detectionOffset, _rangeForAttackOne, false);
                                }
                                if (hitEntities[0] == EntityToAttack && Math.Abs(EntityToAttack.transform.Position.X - entity.transform.Position.X) < _rangeForAttackOne)
                                {
                                    EnemyComponent.State = EnemyState.Attacking;
                                }
                                else if (entity.hitboxes[0].hitboxRectangle.Intersects(EntityToAttack.hitboxes[0].hitboxRectangle))
                                {
                                    EnemyComponent.State = EnemyState.Attacking;
                                    if (entity.transform.Position.X < EntityToAttack.transform.Position.X)
                                        EnemyComponent.CurrentSide = Side.Right;
                                    else
                                        EnemyComponent.CurrentSide = Side.Left;
                                }
                            }
                        }
                        else if(_checkBothSide)
                        {
                            Entity[] hitEntities;
                            if (EnemyComponent.Chase != null)
                            {
                                hitEntities = EnemyComponent.UniqueRaycast(_detectionOffset, EnemyComponent.Chase.DetectionDistance, true);
                            }
                            else
                            {
                                hitEntities = EnemyComponent.UniqueRaycast(_detectionOffset, _rangeForAttackOne, true);
                            }

                            if (hitEntities[0] == EntityToAttack && Math.Abs(EntityToAttack.transform.Position.X - entity.transform.Position.X) < _rangeForAttackOne)
                            {
                                EnemyComponent.CurrentSide = Side.Right;
                            }
                            else if (hitEntities[1] == EntityToAttack && Math.Abs(EntityToAttack.transform.Position.X - entity.transform.Position.X) < _rangeForAttackOne)
                            {
                                EnemyComponent.CurrentSide = Side.Left;
                            }
                            else if (entity.hitboxes[0].hitboxRectangle.Intersects(EntityToAttack.hitboxes[0].hitboxRectangle))
                            {
                                if (entity.transform.Position.X < EntityToAttack.transform.Position.X)
                                    EnemyComponent.CurrentSide = Side.Right;
                                else
                                    EnemyComponent.CurrentSide = Side.Left;
                            }
                            else if (EnemyComponent.Chase == null)
                                EnemyComponent.State = EnemyState.Idle;
                        }
                        else
                        {
                            Entity[] hitEntities;
                            if (EnemyComponent.Chase != null)
                            {
                                hitEntities = EnemyComponent.UniqueRaycast(_detectionOffset, EnemyComponent.Chase.DetectionDistance, false);
                            }
                            else
                            {
                                hitEntities = EnemyComponent.UniqueRaycast(_detectionOffset, _rangeForAttackOne, false);
                            }
                            if (hitEntities[0] != EntityToAttack || Math.Abs(EntityToAttack.transform.Position.X - entity.transform.Position.X) > _rangeForAttackOne && !entity.hitboxes[0].hitboxRectangle.Intersects(EntityToAttack.hitboxes[0].hitboxRectangle))
                            {
                                if (EnemyComponent.Chase != null)
                                    EnemyComponent.State = EnemyState.Idle;
                            }
                        }
                        break;
                }
            }
            
            base.PreUpdate(gameTime);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (CurrentAttack != null)
            {
                CurrentAttack.Update(gameTime);
                if (CurrentAttack != null && CurrentAttack.CooldownFinished)
                {
                    CurrentAttack = null;
                    EnemyComponent.State = EnemyState.Wait;
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
            if (EnemyComponent.State != EnemyState.Dying && EnemyComponent.State != EnemyState.Dead)
            {
                if ((CurrentAttack == null && EnemyComponent.State == EnemyState.Attacking) || !entity.rigidbody.isGrounded)
                    EnemyComponent.State = EnemyState.Wait;

                if (EnemyComponent.State != EnemyState.Attacking && CurrentAttack != null)
                {
                    CurrentAttack.CancelAttack();
                    CurrentAttack = null;
                }
            }
            
            base.PostUpdate(gameTime);
        }

        private void ChooseAttack()
        {
            if (_rangeForAttackThree != 0.0f && Vector2.DistanceSquared(entity.transform.Position, EntityToAttack.transform.Position) < _rangeForAttackThree * _rangeForAttackThree)
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
