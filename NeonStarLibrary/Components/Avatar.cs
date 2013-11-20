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

        private float _invincibilityDuration = 0.0f;

        public float InvincibilityDuration
        {
            get { return _invincibilityDuration; }
            set { _invincibilityDuration = value; }
        }
        private float _invincibilityTimer = 0.0f;

        private float _airLockDuration;

        private string _hitAnim = "";

        public string HitAnim
        {
            get { return _hitAnim; }
            set { _hitAnim = value; }
        }

        public MeleeFight meleeFight;
        public ThirdPersonController thirdPersonController;
        public Guard guard;
        public ElementSystem elementSystem;

        private bool _opacityGoingDown = false;

        public Avatar(Entity entity)
            :base(entity, "Avatar")
        {
        }

        public override void Init()
        {
            meleeFight = this.entity.GetComponent<MeleeFight>();
            thirdPersonController = this.entity.GetComponent<ThirdPersonController>();
            guard = this.entity.GetComponent<Guard>();
            elementSystem = this.entity.GetComponent<ElementSystem>();
            base.Init();
        }

        public bool ChangeHealthPoints(float value, Attack attack = null)
        {
            if (_invincibilityTimer <= 0)
            {
                _currentHealthPoints += value;
                if (value < 0)
                {
                    entity.spritesheets.ChangeAnimation(_hitAnim, 1, true, false, false);
                    this._invincibilityTimer = this._invincibilityDuration;
                }
                if (Debug)
                {
                    Console.WriteLine(entity.Name + " have lost " + value + " HP(s) -> Now at " + _currentHealthPoints + " HP(s).");
                }
                return true;
            }
            else
                return false;
        }

        public void AirLock(float duration)
        {
            _airLockDuration = duration;
            if (_airLockDuration > 0)
            {
                entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                entity.rigidbody.GravityScale = 0.0f;
            }
        }

        public void StunLockEffect(float duration)
        {
            _stunLockDuration = duration;
            if (_stunLockDuration > 0)
            {
                entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                if (meleeFight != null && meleeFight.CurrentAttack != null)
                {
                    meleeFight.CurrentAttack.CancelAttack();
                    meleeFight.CurrentAttack = null;
                    entity.spritesheets.CurrentPriority = 0;
                }
                if (elementSystem.CurrentElementEffect != null)
                {
                    elementSystem.CurrentElementEffect.End();
                }
            }        
        }

        public override void Update(GameTime gameTime)
        {
            

            if (_invincibilityTimer > 0.0f)
            {
                _invincibilityTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_opacityGoingDown)
                {
                    if (entity.spritesheets.CurrentSpritesheet.opacity > 0)
                    {
                        entity.spritesheets.ChangeOpacity(-15 * (float)gameTime.ElapsedGameTime.TotalSeconds);
                    }
                    else
                    {
                        _opacityGoingDown = false;
                        entity.spritesheets.CurrentSpritesheet.opacity = 0.0f;
                    }
                    
                }
                else
                {
                    if (entity.spritesheets.CurrentSpritesheet.opacity < 1)
                    {
                        entity.spritesheets.ChangeOpacity(15 * (float)gameTime.ElapsedGameTime.TotalSeconds);
                    }
                    else
                    {
                        _opacityGoingDown = true;
                        entity.spritesheets.CurrentSpritesheet.opacity = 1.0f;
                    }
                }
            }
            else
            {
                _invincibilityTimer = 0.0f;
                _opacityGoingDown = true;
                entity.spritesheets.CurrentSpritesheet.opacity = 1f;
            }

            if (_airLockDuration > 0.0f)
            {
                _airLockDuration -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                entity.rigidbody.GravityScale = 0.0f;
            }
            else
            {
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
