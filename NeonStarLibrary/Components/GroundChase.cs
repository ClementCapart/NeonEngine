using Microsoft.Xna.Framework;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    public class GroundChase : Chase
    {
        #region Properties
        private bool _checkBothSide = true;

        public bool CheckBothSide
        {
            get { return _checkBothSide; }
            set { _checkBothSide = value; }
        }

        private Vector2 _raycastOffset = Vector2.Zero;

        public Vector2 RaycastOffset
        {
            get { return _raycastOffset; }
            set { _raycastOffset = value; }
        }
        #endregion

        public GroundChase(Entity entity)
            :base(entity)
        {
            Name = "GroundChase";
        }

        public override void Init()
        {
            EnemyComponent = entity.GetComponent<Enemy>();
            if (EntityToChaseName != "")
                EntityToChase = Neon.world.GetEntityByName(_entityToChaseName);
            base.Init();
        }

        public override void PreUpdate(GameTime gameTime)
        {
            if (EntityToChase != null)
            {
                if (entity.rigidbody.isGrounded)
                {
                    switch (EnemyComponent.State)
                    {
                        case EnemyState.Idle:
                        case EnemyState.Patrol:
                        case EnemyState.WaitNode:
                            if (_checkBothSide)
                            {
                                Entity[] hitEntities = EnemyComponent.UniqueRaycast(_raycastOffset, _detectionDistance, true);
                                if(hitEntities[0] == EntityToChase)
                                {
                                    EnemyComponent.State = EnemyState.WaitThreat;
                                    _positionToReach = EntityToChase.transform.Position;
                                    EnemyComponent.CurrentSide = Side.Right;
                                }
                                else if (hitEntities[1] == EntityToChase)
                                {
                                    EnemyComponent.State = EnemyState.WaitThreat;
                                    _positionToReach = EntityToChase.transform.Position;
                                    EnemyComponent.CurrentSide = Side.Left;
                                }
                            }
                            else
                            {
                                Entity[] hitEntities = EnemyComponent.UniqueRaycast(_raycastOffset, _detectionDistance, false);
                                if (hitEntities[0] == EntityToChase)
                                {
                                    EnemyComponent.State = EnemyState.WaitThreat;
                                    _positionToReach = EntityToChase.transform.Position;
                                }
                            }
                            break;

                        case EnemyState.WaitThreat:
                            if (EnemyComponent.WaitThreatTimer >= EnemyComponent.WaitThreatDuration)
                            {
                                EnemyComponent.State = EnemyState.Chase;
                            }
                            else
                            {
                                if (_checkBothSide)
                                {
                                    Entity[] hitEntities = EnemyComponent.UniqueRaycast(_raycastOffset, _detectionDistance, true);
                                    if(hitEntities[0] == EntityToChase)
                                    {
                                        _positionToReach = EntityToChase.transform.Position;
                                        EnemyComponent.CurrentSide = Side.Right;
                                    }
                                    else if (hitEntities[1] == EntityToChase)
                                    {
                                        _positionToReach = EntityToChase.transform.Position;
                                        EnemyComponent.CurrentSide = Side.Left;
                                    }
                                    else if (entity.hitboxes[0].hitboxRectangle.Intersects(EntityToChase.hitboxes[0].hitboxRectangle))
                                    {
                                        if (entity.transform.Position.X < EntityToChase.transform.Position.X)
                                            EnemyComponent.CurrentSide = Side.Right;
                                        else
                                            EnemyComponent.CurrentSide = Side.Left;
                                    }
                                    else
                                    {
                                        EnemyComponent.State = EnemyState.Idle;
                                        EnemyComponent.WaitThreatTimer = 0.0f;
                                    }
                                }
                                else
                                {
                                    Entity[] hitEntities = EnemyComponent.UniqueRaycast(_raycastOffset, _detectionDistance, false);
                                    if (hitEntities[0] == EntityToChase)
                                    {
                                        _positionToReach = EntityToChase.transform.Position;
                                    }
                                    else if (entity.hitboxes[0].hitboxRectangle.Intersects(EntityToChase.hitboxes[0].hitboxRectangle))
                                    {
                                        if (entity.transform.Position.X < EntityToChase.transform.Position.X)
                                            EnemyComponent.CurrentSide = Side.Right;
                                        else
                                            EnemyComponent.CurrentSide = Side.Left;
                                    }
                                    else
                                    {
                                        EnemyComponent.State = EnemyState.Idle;
                                        EnemyComponent.WaitThreatTimer = 0.0f;
                                    }
                                }
                            }
                            break;

                        case EnemyState.Wait:
                            _waitStopTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                            if (_waitStopTimer >= _waitStopDuration)
                            {
                                _waitStopTimer = 0.0f;
                                EnemyComponent.State = EnemyState.Idle;
                            }
                            else
                            {
                                if (_checkBothSide)
                                {
                                    Entity[] hitEntities = EnemyComponent.UniqueRaycast(_raycastOffset, _detectionDistance, true);
                                    if(hitEntities[0] == EntityToChase)
                                    {
                                        if (entity.rigidbody.beacon.CheckRightGround())
                                        {
                                            EnemyComponent.State = EnemyState.Chase;
                                            _positionToReach = EntityToChase.transform.Position;
                                        }
                                        else
                                        {
                                            _waitStopTimer = 0.0f;
                                        }
                                        EnemyComponent.CurrentSide = Side.Right;
                                    }
                                    else if (hitEntities[1] == EntityToChase)
                                    {
                                        if (entity.rigidbody.beacon.CheckLeftGround())
                                        {
                                            EnemyComponent.State = EnemyState.Chase;
                                            _positionToReach = EntityToChase.transform.Position;
                                        }
                                        else
                                        {
                                            _waitStopTimer = 0.0f;
                                        }
                                        EnemyComponent.CurrentSide = Side.Left;
                                    }
                                    else if (entity.hitboxes[0].hitboxRectangle.Intersects(EntityToChase.hitboxes[0].hitboxRectangle))
                                    {
                                        if (entity.transform.Position.X < EntityToChase.transform.Position.X)
                                            EnemyComponent.CurrentSide = Side.Right;
                                        else
                                            EnemyComponent.CurrentSide = Side.Left;
                                    }
                                }
                                else
                                {
                                    Entity[] hitEntities = EnemyComponent.UniqueRaycast(_raycastOffset, _detectionDistance, false);
                                    if(hitEntities[0] == EntityToChase)
                                    {
                                        switch (EnemyComponent.CurrentSide)
                                        {
                                            case Side.Left:
                                                if (entity.rigidbody.beacon.CheckLeftGround())
                                                {
                                                    EnemyComponent.State = EnemyState.Chase;
                                                    _positionToReach = EntityToChase.transform.Position;
                                                }
                                                else
                                                    _waitStopTimer = 0.0f;
                                                break;

                                            case Side.Right:
                                                if (entity.rigidbody.beacon.CheckRightGround())
                                                {
                                                    EnemyComponent.State = EnemyState.Chase;
                                                    _positionToReach = EntityToChase.transform.Position;
                                                }
                                                else
                                                    _waitStopTimer = 0.0f;
                                                break;
                                        }
                                        _positionToReach = EntityToChase.transform.Position;

                                    }
                                }
                            }
                            break;

                        case EnemyState.Chase:
                        case EnemyState.FinishChase:
                            if (EnemyComponent.CurrentSide == Side.Right)
                            {
                                if (!entity.rigidbody.beacon.CheckRightGround())
                                {
                                    EnemyComponent.State = EnemyState.Wait;
                                }
                            }
                            else if (EnemyComponent.CurrentSide == Side.Left)
                            {
                                if (!entity.rigidbody.beacon.CheckLeftGround())
                                {
                                    EnemyComponent.State = EnemyState.Wait;
                                }
                            }

                            if (_checkBothSide)
                            {
                                Entity[] hitEntities = EnemyComponent.UniqueRaycast(_raycastOffset, _detectionDistance, true);
                                if(hitEntities[0] == EntityToChase)
                                {
                                    EnemyComponent.CurrentSide = Side.Right;
                                    EnemyComponent.State = EnemyState.Chase;
                                    _positionToReach = EntityToChase.transform.Position;
                                }
                                else if(hitEntities[1] == EntityToChase)
                                {
                                    EnemyComponent.CurrentSide = Side.Left;
                                    EnemyComponent.State = EnemyState.Chase;
                                    _positionToReach = EntityToChase.transform.Position;
                                }
                                else
                                    EnemyComponent.State = EnemyState.FinishChase;
                            }
                            else
                            {
                                Entity[] hitEntities = EnemyComponent.UniqueRaycast(_raycastOffset, _detectionDistance, false);
                                if (hitEntities[0] == EntityToChase)
                                {
                                    EnemyComponent.State = EnemyState.Chase;
                                    _positionToReach = EntityToChase.transform.Position;
                                }
                                else
                                    EnemyComponent.State = EnemyState.FinishChase;
                            }

                            if (EnemyComponent.State == EnemyState.FinishChase && _positionToReach.X + _chasePrecision > entity.transform.Position.X && _positionToReach.X - _chasePrecision < entity.transform.Position.X)
                            {
                                EnemyComponent.State = EnemyState.Wait;
                            }
                            break;

                    }

                    if(EnemyComponent.Attack == null)
                        if (entity.hitboxes[0].hitboxRectangle.Intersects(EntityToChase.hitboxes[0].hitboxRectangle))
                            EnemyComponent.State = EnemyState.Wait;
                }
                else
                {
                    if (_entityToChaseName != "")
                        EntityToChase = Neon.world.GetEntityByName(_entityToChaseName);
                }
            }
            else
            {
                EnemyComponent.State = EnemyState.Idle;
            }
                
        }

        public override void Update(GameTime gameTime)
        {
            switch(EnemyComponent.State)
            {
                case EnemyState.Chase:
                case EnemyState.FinishChase:
                    if (EnemyComponent.CurrentSide == Side.Right)
                    {
                        this.entity.rigidbody.body.LinearVelocity = new Microsoft.Xna.Framework.Vector2(_chaseSpeed, 0);
                    }
                    else if (EnemyComponent.CurrentSide == Side.Left)
                    {
                        this.entity.rigidbody.body.LinearVelocity = new Microsoft.Xna.Framework.Vector2(-_chaseSpeed, 0);
                    }
                    break;
            }
            base.Update(gameTime);
        }
    }
}
