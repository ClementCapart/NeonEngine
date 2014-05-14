using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.Enemies
{
    public class FlyingChase : Chase
    {
        #region Properties
        
        #endregion

        public FlyingChase(Entity entity)
            :base(entity)
        {
            Name = "FlyingChase";
        }

        public override void Init()
        {
            EnemyComponent = entity.GetComponent<EnemyCore>();
            if (EntityToChaseName != "")
                EntityToChase = entity.GameWorld.GetEntityByName(_entityToChaseName);
            if (entity.rigidbody != null)
                entity.rigidbody.body.CollidesWith = FarseerPhysics.Dynamics.Category.Cat16;
            base.Init();
        }

        public override void PreUpdate(GameTime gameTime)
        {
            if (EntityToChase != null)
            {
                switch (EnemyComponent.State)
                {
                    case EnemyState.Idle:
                    case EnemyState.Patrol:
                    case EnemyState.WaitNode:
                        if (DetectionDistance * DetectionDistance > Vector2.DistanceSquared(entity.transform.Position, EntityToChase.transform.Position))
                        {
                            if (entity.rigidbody != null && entity.rigidbody.BodyType == BodyType.Kinematic)
                                entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                            EnemyComponent.State = EnemyState.WaitThreat;
                            _positionToReach = EntityToChase.transform.Position;
                            if (EntityToChase.transform.Position.X < entity.transform.Position.X)
                                EnemyComponent.CurrentSide = Side.Left;
                            else
                                EnemyComponent.CurrentSide = Side.Right;
                        }                        
                        break;

                    case EnemyState.WaitThreat:
                        if (EnemyComponent.WaitThreatTimer >= EnemyComponent.WaitThreatDuration)
                        {
                            EnemyComponent.State = EnemyState.Chase;
                        }
                        else
                        {
                            if (DetectionDistance * DetectionDistance > Vector2.DistanceSquared(entity.transform.Position, EntityToChase.transform.Position))
                            {
                                _positionToReach = EntityToChase.transform.Position;
                                if (entity.transform.Position.X < EntityToChase.transform.Position.X)
                                    EnemyComponent.CurrentSide = Side.Right;
                                else
                                    EnemyComponent.CurrentSide = Side.Left;                                 
                            }
                            else
                            {
                                if (entity.rigidbody != null && entity.rigidbody.BodyType == BodyType.Kinematic)
                                    entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                                EnemyComponent.State = EnemyState.Idle;
                                EnemyComponent.WaitThreatTimer = 0.0f;
                            }
                        }
                        break;

                    case EnemyState.Wait:
                        _waitStopTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                        if (_waitStopTimer >= _waitStopDuration)
                        {
                            _waitStopTimer = 0.0f;
                            if (EnemyComponent.FollowNodes != null)
                                EnemyComponent.State = EnemyState.Patrol;
                            else
                                EnemyComponent.State = EnemyState.Idle;
                            if (entity.rigidbody != null && entity.rigidbody.BodyType == BodyType.Kinematic)
                                entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                        }
                        else
                        {
                            if (DetectionDistance * DetectionDistance > Vector2.DistanceSquared(entity.transform.Position, EntityToChase.transform.Position))
                            {
                                EnemyComponent.State = EnemyState.Chase;
                                _positionToReach = EntityToChase.transform.Position;
                                if (entity.transform.Position.X < EntityToChase.transform.Position.X)
                                    EnemyComponent.CurrentSide = Side.Right;
                                else
                                    EnemyComponent.CurrentSide = Side.Left;

                            }
                        }
                        break;

                    case EnemyState.Chase:
                    case EnemyState.FinishChase:
                        if (DetectionDistance * DetectionDistance > Vector2.DistanceSquared(entity.transform.Position, EntityToChase.transform.Position))
                        {
                            EnemyComponent.State = EnemyState.Chase;
                            _positionToReach = EntityToChase.transform.Position;
                            if (entity.transform.Position.X < EntityToChase.transform.Position.X)
                                EnemyComponent.CurrentSide = Side.Right;
                            else
                                EnemyComponent.CurrentSide = Side.Left;
                        }
                        else
                            EnemyComponent.State = EnemyState.FinishChase;
                           
                        if (EnemyComponent.State == EnemyState.FinishChase && _positionToReach.X + _chasePrecision > entity.transform.Position.X && _positionToReach.X - _chasePrecision < entity.transform.Position.X)
                        {
                            EnemyComponent.State = EnemyState.Wait;
                            if (entity.rigidbody != null && entity.rigidbody.BodyType == BodyType.Kinematic)
                                entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                        }
                        break;

                }

                if (EnemyComponent.Attack == null && EnemyComponent.State != EnemyState.Dying && EnemyComponent.State != EnemyState.Dead)
                    if (entity.hitboxes[0].hitboxRectangle.Intersects(EntityToChase.hitboxes[0].hitboxRectangle))
                        EnemyComponent.State = EnemyState.Wait;
            }
            else
            {
                if (_entityToChaseName != "")
                    EntityToChase = entity.GameWorld.GetEntityByName(_entityToChaseName);
            }
        }

        public override void Update(GameTime gameTime)
        {
            switch (EnemyComponent.State)
            {
                case EnemyState.Chase:
                case EnemyState.FinishChase:
                    this.entity.rigidbody.body.LinearVelocity = Vector2.Normalize(new Vector2(this._positionToReach.X - entity.transform.Position.X, _positionToReach.Y - entity.transform.Position.Y)) * _chaseSpeed;
                    break;

                case EnemyState.Attacking:
                    if (entity.rigidbody != null && entity.rigidbody.BodyType == BodyType.Kinematic)
                        entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                    break;
            }
            base.Update(gameTime);
        }
    }
}
