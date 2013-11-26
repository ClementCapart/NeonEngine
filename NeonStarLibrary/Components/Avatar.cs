﻿using Microsoft.Xna.Framework;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    public enum AvatarState
    {
        Idle, 
        Moving,
        Attacking,
        Guarding,
        Rolling,
        AirDashing,
        Stunlocked,
        UsingElement
    }

    public class Avatar : Component
    {
        #region Properties
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

        private float _invincibilityDuration = 0.0f;

        public float InvincibilityDuration
        {
            get { return _invincibilityDuration; }
            set { _invincibilityDuration = value; }
        }

        private string _hitAnim = "";

        public string HitAnim
        {
            get { return _hitAnim; }
            set { _hitAnim = value; }
        }
        #endregion       

        public AvatarState State = AvatarState.Idle;

        public AvatarAnimationManager AvatarAnimationManager;
        public MeleeFight MeleeFight;
        public ThirdPersonController ThirdPersonController;
        public Guard Guard;
        public ElementSystem ElementSystem;

        public Side CurrentSide = Side.Right;

        public bool CanMove = true;
        public bool CanTurn = true;
        public bool CanAttack = true;
        public bool CanUseElement = true;

        public bool IsInvincible = false;
        public bool IsAirLocked = false;

        private float _stunLockDuration = 0.0f;
        private float _invincibilityTimer = 0.0f;
        private float _airLockDuration = 0.0f;
        
        private bool _opacityGoingDown = false;

        public Avatar(Entity entity)
            :base(entity, "Avatar")
        {
        }

        public override void Init()
        {
            AvatarAnimationManager = this.entity.GetComponent<AvatarAnimationManager>();
            MeleeFight = this.entity.GetComponent<MeleeFight>();
            ThirdPersonController = this.entity.GetComponent<ThirdPersonController>();
            Guard = this.entity.GetComponent<Guard>();
            ElementSystem = this.entity.GetComponent<ElementSystem>();

            base.Init();
        }

        public bool TakeDamage(Attack attack)
        {
            return TakeDamage(attack.DamageOnHit, attack.StunLock, attack.TargetAirLock, attack.CurrentSide);
        }

        public bool TakeDamage(Bullet bullet)
        {
            return TakeDamage(bullet.DamageOnHit, bullet.StunLock, 0.0f, bullet.entity.spritesheets.CurrentSide);
        }

        public bool TakeDamage(float damageValue, float stunLockDuration, float airLockDuration, Side side)
        {
            if (IsInvincible)
                return false;

            if (State == AvatarState.Guarding && CurrentSide != side)
            {
                damageValue = Math.Min(damageValue + Guard.GuardDamageReduce, 0);              
            }

            if (damageValue >= 0.0f)
                return false;

            _currentHealthPoints += damageValue;

            if (Debug)
            {
                Console.WriteLine(entity.Name + " have lost " + damageValue + " HP(s) -> Now at " + _currentHealthPoints + " HP(s).");
            }

            if (stunLockDuration > 0.0f)
            {
                _stunLockDuration = stunLockDuration;
                entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                
                if (State == AvatarState.Attacking && MeleeFight != null && MeleeFight.CurrentAttack != null)
                {
                    MeleeFight.CurrentAttack.CancelAttack();
                    MeleeFight.CurrentAttack = null;
                    entity.spritesheets.CurrentPriority = 0;
                }
                
                if (State == AvatarState.UsingElement && ElementSystem.CurrentElementEffect != null)
                {
                    ElementSystem.CurrentElementEffect.End();
                }

                State = AvatarState.Stunlocked;
            }
            
            if (!entity.rigidbody.isGrounded)
            {
                AirLock(airLockDuration);
            }
            return true;
        }

        public void AirLock(float duration)
        {
            if (duration > 0.0f)
            {
                IsAirLocked = true;
                _airLockDuration = duration;
                entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                entity.rigidbody.GravityScale = 0.0f;
            }
        }

        public override void PreUpdate(GameTime gameTime)
        {
            if (_airLockDuration > 0.0f && IsAirLocked)
            {
                _airLockDuration -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                entity.rigidbody.GravityScale = 0.0f;
                CanMove = false;
            }
            else
            {
                _airLockDuration = 0.0f;
                IsAirLocked = false;
            }

            if (_stunLockDuration > 0.0f)
            {
                _stunLockDuration -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                State = AvatarState.Stunlocked;
                CanMove = false;
                CanTurn = false;
                CanAttack = false;
                CanUseElement = false;
            }
            else
            {
                _stunLockDuration = 0.0f;
            }

            base.PreUpdate(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            if (_invincibilityTimer > 0.0f)
            {
                _invincibilityTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                OpacityBlink(15 * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
            else
            {
                _invincibilityTimer = 0.0f;
                entity.spritesheets.CurrentSpritesheet.opacity = 1.0f;
            }
            base.Update(gameTime);
        }

        public override void PostUpdate(GameTime gameTime)
        {
            CanMove = true;
            CanTurn = true;
            CanAttack = true;
            CanUseElement = true;
            State = AvatarState.Idle;
            base.PostUpdate(gameTime);
        }

        public void OpacityBlink(float value)
        {
            if (_opacityGoingDown)
            {
                if (entity.spritesheets.CurrentSpritesheet.opacity > 0)
                {
                    entity.spritesheets.ChangeOpacity(-value);
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
                    entity.spritesheets.ChangeOpacity(value);
                }
                else
                {
                    _opacityGoingDown = true;
                    entity.spritesheets.CurrentSpritesheet.opacity = 1.0f;
                }
            }
        }
    }
}
