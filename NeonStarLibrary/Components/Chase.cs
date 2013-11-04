using Microsoft.Xna.Framework;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    public class Chase : Component
    {
        public Enemy EnemyComponent;
        public Vector2 LastTargetPosition;
        public bool HavingTarget = false;

        private float _speed;

        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        private float _waitBeforeChasingDelay = 0.0f;
        public bool HasToWait = true;
        private float _timerBeforeChasing = 0.0f;

        public float WaitBeforeChasingDelay
        {
            get { return _waitBeforeChasingDelay; }
            set { _waitBeforeChasingDelay = value; }
        }
        
        private float _waitDelay = 1.0f;

        public float WaitDelay
        {
            get { return _waitDelay; }
            set { _waitDelay = value; }
        }
        private float _waitTimer = 0.0f;


        public Chase(Entity entity)
            :base(entity, "Chase")
        {
        }

        public override void Init()
        {
            EnemyComponent = entity.GetComponent<Enemy>();
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (EnemyComponent.State == EnemyState.MustFinishChase)
                EnemyComponent.State = EnemyState.FinishChase;

            if (EnemyComponent.State == EnemyState.FinishChase)
            {
                if (HavingTarget)
                {
                    if (LastTargetPosition.X < this.entity.transform.Position.X)
                    {
                        if (entity.rigidbody.beacon.CheckLeftGround() && entity.rigidbody.beacon.CheckLeftSide(0) == null)
                        {
                            if (this.entity.spritesheets != null)
                            {
                                entity.spritesheets.ChangeSide(Side.Left);
                            }
                            this.entity.rigidbody.body.LinearVelocity = new Microsoft.Xna.Framework.Vector2(-_speed, this.entity.rigidbody.body.LinearVelocity.Y);
                        }
                        else
                        {
                            this.entity.rigidbody.body.LinearVelocity = new Microsoft.Xna.Framework.Vector2(0, this.entity.rigidbody.body.LinearVelocity.Y);
                            HavingTarget = false;
                            EnemyComponent._threatArea.ShouldDetectAgain = false;
                            EnemyComponent.State = EnemyState.Wait;
                            _waitTimer = _waitDelay;
                        }
                    }
                    else
                    {
                        if (entity.rigidbody.beacon.CheckRightGround() && entity.rigidbody.beacon.CheckRightSide(0) == null)
                        {
                            if (this.entity.spritesheets != null)
                            {
                                entity.spritesheets.ChangeSide(Side.Right);
                            }
                            this.entity.rigidbody.body.LinearVelocity = new Microsoft.Xna.Framework.Vector2(_speed, this.entity.rigidbody.body.LinearVelocity.Y);
                        }
                        else
                        {
                            this.entity.rigidbody.body.LinearVelocity = new Microsoft.Xna.Framework.Vector2(0, this.entity.rigidbody.body.LinearVelocity.Y);
                            EnemyComponent._threatArea.ShouldDetectAgain = false;
                            EnemyComponent.State = EnemyState.Wait;
                            _waitTimer = _waitDelay;
                            HavingTarget = false;
                        }
                    }

                    if (LastTargetPosition.X + _speed > entity.transform.Position.X && LastTargetPosition.X - _speed < entity.transform.Position.X)
                    {
                        this.entity.rigidbody.body.LinearVelocity = new Microsoft.Xna.Framework.Vector2(0, this.entity.rigidbody.body.LinearVelocity.Y);
                        HavingTarget = false;
                        EnemyComponent._threatArea.ShouldDetectAgain = false;
                        EnemyComponent.State = EnemyState.Wait;
                        _waitTimer = _waitDelay;
                    }
                }
                else
                {
                    EnemyComponent.State = EnemyState.Wait;
                    EnemyComponent._threatArea.ShouldDetectAgain = false;
                    _waitTimer = _waitDelay;
                }
            }
            else if (EnemyComponent.State == EnemyState.Wait)
            {
                if (_waitTimer > 0.0f)
                {
                    _waitTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else
                {
                    HasToWait = true;
                    EnemyComponent.State = EnemyState.Idle;
                }
            }
            else if (EnemyComponent.State == EnemyState.Chase)
            {
                HavingTarget = true;
                LastTargetPosition = new Vector2(EnemyComponent._threatArea.EntityFollowed.transform.Position.X, EnemyComponent._threatArea.EntityFollowed.transform.Position.Y);

                if (HasToWait)
                {
                    HasToWait = false;
                    _timerBeforeChasing = _waitBeforeChasingDelay;
                }
                else if (_timerBeforeChasing > 0.0f)
                {
                    _timerBeforeChasing -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else if (LastTargetPosition.X < this.entity.transform.Position.X)
                {
                    _timerBeforeChasing = 0.0f;
                    if (entity.rigidbody.beacon.CheckLeftGround() && entity.rigidbody.beacon.CheckLeftSide(0) == null)
                    {
                        if (this.entity.spritesheets != null)
                        {
                            entity.spritesheets.ChangeSide(Side.Left);
                        }
                        if(EnemyComponent.Type == EnemyType.Ground && entity.rigidbody.isGrounded || EnemyComponent.Type == EnemyType.Flying)
                            this.entity.rigidbody.body.LinearVelocity = new Microsoft.Xna.Framework.Vector2(-_speed, this.entity.rigidbody.body.LinearVelocity.Y);
                    }
                    else
                    {
                        this.entity.rigidbody.body.LinearVelocity = new Microsoft.Xna.Framework.Vector2(0, this.entity.rigidbody.body.LinearVelocity.Y);
                        _waitTimer = _waitDelay;
                        EnemyComponent.State = EnemyState.MustFinishChase;
                    }
                }
                else
                {
                    if (entity.rigidbody.beacon.CheckRightGround() && entity.rigidbody.beacon.CheckRightSide(0) == null)
                    {
                        if (this.entity.spritesheets != null)
                        {
                            entity.spritesheets.ChangeSide(Side.Right);
                        }
                        if (EnemyComponent.Type == EnemyType.Ground && entity.rigidbody.isGrounded || EnemyComponent.Type == EnemyType.Flying)
                            this.entity.rigidbody.body.LinearVelocity = new Microsoft.Xna.Framework.Vector2(_speed, this.entity.rigidbody.body.LinearVelocity.Y);
                    }
                    else
                    {
                        this.entity.rigidbody.body.LinearVelocity = new Microsoft.Xna.Framework.Vector2(0, this.entity.rigidbody.body.LinearVelocity.Y);
                        _waitTimer = _waitDelay;
                        EnemyComponent.State = EnemyState.MustFinishChase;
                    }
                }
            }
            base.Update(gameTime);
        }
    }
}
