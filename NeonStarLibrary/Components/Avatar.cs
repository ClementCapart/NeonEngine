﻿using Microsoft.Xna.Framework;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    public class Avatar : Component
    {
        private bool _debug;

        public bool Debug
        {
            get { return _debug; }
            set { _debug = value; }
        }

        private float _startingHealthPoints;

        public float StartingHealthPoints
        {
            get { return _startingHealthPoints; }
            set { _startingHealthPoints = value; }
        }

        private float _currentHealthPoints;

        public float CurrentHealthPoints
        {
            get { return _currentHealthPoints; }
            set { _currentHealthPoints = value; }
        }

        private float _stunLockDuration;

        public float StunLockDuration
        {
            get { return _stunLockDuration; }
            set { _stunLockDuration = value; }
        }
        private float _airLockDuration;

        public MeleeFight meleeFight;
        public ThirdPersonController thirdPersonController;


        public Avatar(Entity entity)
            :base(entity, "Avatar")
        {
        }

        public override void Init()
        {
            meleeFight = this.entity.GetComponent<MeleeFight>();
            thirdPersonController = this.entity.GetComponent<ThirdPersonController>();
            base.Init();
        }

        public void ChangeHealthPoints(float value)
        {
            _currentHealthPoints += value;
            if (Debug)
            {
                Console.WriteLine(entity.Name + " have lost " + value + " HP(s) -> Now at " + _currentHealthPoints + " HP(s).");
            }
        }

        public void AirLock(float duration)
        {
            _airLockDuration = duration;
            if (_airLockDuration > 0)
            {
                entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                entity.rigidbody.GravityScale = 0.0f;
            }
            else
            {
                entity.rigidbody.GravityScale = entity.rigidbody.InitialGravityScale;
            }
        }

        public void StunLockEffect(float duration)
        {
            _stunLockDuration = duration;
            if (meleeFight != null && meleeFight.CurrentAttack != null)
            {
                meleeFight.CurrentAttack.CancelAttack();
                meleeFight.CurrentAttack = null;
                entity.spritesheets.CurrentPriority = 0;
            }
            if (_stunLockDuration > 0)
            {
                entity.rigidbody.body.LinearVelocity = Vector2.Zero;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (_airLockDuration > 0.0f)
            {
                _airLockDuration -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                entity.rigidbody.GravityScale = 0.0f;
            }
            else
            {
                entity.rigidbody.GravityScale = entity.rigidbody.InitialGravityScale;
                _airLockDuration = 0.0f;
            }

            if (_stunLockDuration > 0)
            {
                _stunLockDuration -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            base.Update(gameTime);
        }
    }
}