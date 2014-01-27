using Microsoft.Xna.Framework;
using NeonEngine;
using NeonEngine.Components.CollisionDetection;
using NeonStarLibrary.Components.Avatar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.Enemies
{
    class SupervisorChase : Chase
    {
        #region Properties
        private float _detectionWidth;

        public float DetectionWidth
        {
            get { return _detectionWidth; }
            set { _detectionWidth = value; }
        }

        private float _detectionHeight;

        public float DetectionHeight
        {
            get { return _detectionHeight; }
            set { _detectionHeight = value; }
        }

        private Vector2 _detectionBoxOffset;

        public Vector2 DetectionBoxOffset
        {
            get { return _detectionBoxOffset; }
            set { _detectionBoxOffset = value; }
        }

        private float _attackRangeWidth;

        public float AttackRangeWidth
        {
            get { return _attackRangeWidth; }
            set { _attackRangeWidth = value; }
        }

        private float _attackRangeHeight;

        public float AttackRangeHeight
        {
            get { return _attackRangeHeight; }
            set { _attackRangeHeight = value; }
        }

        private Vector2 _attackRangeOffset;

        public Vector2 AttackRangeOffset
        {
            get { return _attackRangeOffset; }
            set { _attackRangeOffset = value; }
        }

        private PathNodeList _pathNodeBounds;

        public PathNodeList PathNodeBounds
        {
            get { return _pathNodeBounds; }
            set { _pathNodeBounds = value; }
        }
        #endregion

        private bool _onDuty = false;
        private AvatarCore _avatar;

        private Vector2 _lastChasePosition;
        private float _leftBound = float.MaxValue;
        private float _rightBound = float.MinValue;

        public SupervisorChase(Entity entity)
            :base(entity)
        {
            Name = "SupervisorChase";
            
        }

        public override void Init()
        {
            EnemyComponent = entity.GetComponent<EnemyCore>();
            this.EntityToChase = entity.containerWorld.GetEntityByName(_entityToChaseName);
            if (EntityToChase != null)
                _avatar = EntityToChase.GetComponent<AvatarCore>();
            if (_pathNodeBounds != null)
            {
                foreach (Node n in _pathNodeBounds.Nodes)
                {
                    if (n.Position.X < _leftBound)
                        _leftBound = n.Position.X;
                    if (n.Position.X > _rightBound)
                    {
                        _rightBound = n.Position.X;
                    }
                }
            }
            base.Init();
        }

        public override void PreUpdate(GameTime gameTime)
        {
            if (_avatar != null)
            {
                switch (EnemyComponent.State)
                {
                    case EnemyState.Idle:
                    case EnemyState.Patrol:
                    case EnemyState.WaitNode:
                    case EnemyState.Wait:
                        if (_onDuty)
                        {
                            Rectangle detectionHitbox = new Rectangle((int)(entity.transform.Position.X + _detectionBoxOffset.X - _detectionWidth / 2), (int)(entity.transform.Position.Y + _detectionBoxOffset.Y - _detectionHeight / 2), (int)_detectionWidth, (int)_detectionHeight);
                            if (detectionHitbox.Intersects(EntityToChase.hitboxes[0].hitboxRectangle))
                            {
                                EnemyComponent.State = EnemyState.WaitThreat;
                            }
                        }
                        break;

                    case EnemyState.Chase:
                        if (_onDuty)
                        {
                            Rectangle detectionHitbox = new Rectangle((int)(entity.transform.Position.X + _detectionBoxOffset.X - _detectionWidth / 2), (int)(entity.transform.Position.Y + _detectionBoxOffset.Y - _detectionHeight / 2), (int)_detectionWidth, (int)_detectionHeight);
                            if (!detectionHitbox.Intersects(EntityToChase.hitboxes[0].hitboxRectangle))
                            {
                                EnemyComponent.State = EnemyState.FinishChase;
                            }
                            else
                            {
                                this._lastChasePosition = EntityToChase.transform.Position;
                            }
                            
                            detectionHitbox.X = (int)(entity.transform.Position.X + _attackRangeOffset.X - _attackRangeWidth / 2);
                            detectionHitbox.Y = (int)(entity.transform.Position.Y + _attackRangeOffset.Y - _attackRangeHeight / 2);
                            detectionHitbox.Width = (int)_attackRangeWidth;
                            detectionHitbox.Height = (int)_attackRangeHeight;
                            
                            if (detectionHitbox.Intersects(EntityToChase.hitboxes[0].hitboxRectangle))
                            {
                                EnemyComponent.State = EnemyState.Attacking;
                                if (entity.rigidbody != null)
                                    entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                            }
                            if (entity.transform.Position.X > _rightBound || entity.transform.Position.X < _leftBound)
                            {
                                EnemyComponent.State = EnemyState.Wait;
                            }
                        }
                        break;

                    case EnemyState.FinishChase:
                        if (_onDuty)
                        {
                            if (this.entity.transform.Position.X > _lastChasePosition.X - _chasePrecision && this.EntityToChase.transform.Position.X < _lastChasePosition.X + _chasePrecision)
                            {
                                EnemyComponent.State = EnemyState.Wait;
                            }

                            if (entity.transform.Position.X > _rightBound || entity.transform.Position.X < _leftBound)
                            {
                                EnemyComponent.State = EnemyState.Wait;
                            }
                        }
                        break;

                    case EnemyState.Attacking:

                        break;
                }
            }
            
            base.PreUpdate(gameTime);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (_avatar != null)
            {
                switch (EnemyComponent.State)
                {
                    case EnemyState.Idle:
                    case EnemyState.Patrol:
                    case EnemyState.WaitNode:
                        if (!_onDuty)
                        {
                            Rectangle detectionHitbox = new Rectangle((int)(entity.transform.Position.X + _detectionBoxOffset.X - _detectionWidth / 2), (int)(entity.transform.Position.Y + _detectionBoxOffset.Y - _detectionHeight / 2), (int)_detectionWidth, (int)_detectionHeight);
                            if (detectionHitbox.Intersects(EntityToChase.hitboxes[0].hitboxRectangle))
                            {
                                foreach (Entity e in entity.containerWorld.Entities)
                                {
                                    if (e != _avatar.entity && e != entity && e.hitboxes.Count > 0 && e.hitboxes[0].hitboxRectangle.Intersects(detectionHitbox) && e.hitboxes[0].Type == HitboxType.Main)
                                    {
                                        EnemyCore enemy = e.GetComponent<EnemyCore>();
                                        if (enemy != null)
                                            if (enemy.TookDamageThisFrame)
                                            {
                                                _onDuty = true;
                                                EnemyComponent.State = EnemyState.WaitThreat;
                                                if (entity.rigidbody != null)
                                                    entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                                            }
                                    }
                                }
                            }
                        }
                        break;

                    case EnemyState.Chase:
                        if (_avatar.entity.transform.Position.X + this._chasePrecision < entity.transform.Position.X)
                        {
                            if (entity.rigidbody != null)
                                entity.rigidbody.body.LinearVelocity = new Vector2(-this._chaseSpeed, 0);
                        }
                        else if (_avatar.entity.transform.Position.X - this._chasePrecision > entity.transform.Position.X)
                        {

                            if (entity.rigidbody != null)
                                entity.rigidbody.body.LinearVelocity = new Vector2(this._chaseSpeed, 0);
                        }
                        else
                        {
                            if (entity.rigidbody != null)
                            {
                                entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                            }
                        }
                        break;

                    case EnemyState.FinishChase:
                        if (this._lastChasePosition.X + this._chasePrecision < entity.transform.Position.X)
                        {
                            if (entity.rigidbody != null)
                                entity.rigidbody.body.LinearVelocity = new Vector2(-this._chaseSpeed, 0);
                        }
                        else if (this._lastChasePosition.X - this._chasePrecision > entity.transform.Position.X)
                        {

                            if (entity.rigidbody != null)
                                entity.rigidbody.body.LinearVelocity = new Vector2(this._chaseSpeed, 0);
                        }
                        break;

                    case EnemyState.Wait:
                        if (entity.rigidbody != null)
                            entity.rigidbody.body.LinearVelocity = Vector2.Zero;

                        _waitStopTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                        if (_waitStopTimer >= _waitStopDuration)
                        {
                            _waitStopTimer = 0.0f;
                            if (EnemyComponent.FollowNodes != null)
                                EnemyComponent.State = EnemyState.Patrol;
                            else
                                EnemyComponent.State = EnemyState.Idle;
                        }
                        
                        /*if (_onDuty)
                        {
                            Rectangle detectionHitbox = new Rectangle((int)(entity.transform.Position.X + _detectionBoxOffset.X - _detectionWidth / 2), (int)(entity.transform.Position.Y + _detectionBoxOffset.Y - _detectionHeight / 2), (int)_detectionWidth, (int)_detectionHeight);
                            if (detectionHitbox.Intersects(EntityToChase.hitboxes[0].hitboxRectangle) && !(entity.transform.Position.X > _rightBound || entity.transform.Position.X < _leftBound))
                            {
                                EnemyComponent.State = EnemyState.Chase;
                            }
                        }*/
                        break;

                    case EnemyState.WaitThreat:
                        if (EnemyComponent.WaitThreatTimer >= EnemyComponent.WaitThreatDuration)
                        {
                            EnemyComponent.State = EnemyState.Chase;
                        }
                        if (_onDuty)
                        {
                            Rectangle detectionHitbox = new Rectangle((int)(entity.transform.Position.X + _detectionBoxOffset.X - _detectionWidth / 2), (int)(entity.transform.Position.Y + _detectionBoxOffset.Y - _detectionHeight / 2), (int)_detectionWidth, (int)_detectionHeight);
                            if (!detectionHitbox.Intersects(EntityToChase.hitboxes[0].hitboxRectangle))
                            {
                                EnemyComponent.State = EnemyState.Idle;
                            }
                        }
                        break;
                }  
            }           
            base.Update(gameTime);
        }


    }
}
