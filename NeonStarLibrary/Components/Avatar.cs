using Microsoft.Xna.Framework;
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

        private float _invincibilityBlinkSpeed = 15.0f;

        public float InvincibilityBlinkSpeed
        {
            get { return _invincibilityBlinkSpeed; }
            set { _invincibilityBlinkSpeed = value; }
        }

        private string _hitAnim = "";

        public string HitAnim
        {
            get { return _hitAnim; }
            set { _hitAnim = value; }
        }

        private string _hitGuardAnim = "";

        public string HitGuardAnim
        {
            get { return _hitGuardAnim; }
            set { _hitGuardAnim = value; }
        }

        private string _hitGuardFX = "";

        public string HitGuardFX
        {
            get { return _hitGuardFX; }
            set { _hitGuardFX = value; }
        }
        #endregion       

        public AvatarState State = AvatarState.Idle;

        public AvatarAnimationManager AvatarAnimationManager;
        public MeleeFight MeleeFight;
        public ThirdPersonController ThirdPersonController;
        public Guard Guard;
        public ElementSystem ElementSystem;
        public EnergySystem EnergySystem;

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
        
        private bool _opacityGoingDown = true;
        private SpriteSheetInfo _hitGuardSpritesheet = null;


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
            EnergySystem = this.entity.GetComponent<EnergySystem>();
            _hitGuardSpritesheet = AssetManager.GetSpriteSheet(_hitGuardFX);

            base.Init();
        }

        public bool TakeDamage(Attack attack)
        {
            bool takeDamage = TakeDamage(attack.DamageOnHit, attack.StunLock, attack.TargetAirLock, attack.CurrentSide);

            if (!takeDamage && State == AvatarState.Guarding)
            {
                if (attack.Launcher != null)
                {
                    Enemy e = attack.Launcher.GetComponent<Enemy>();
                    if (e != null)
                    {
                        if (Guard != null)
                            e.StunLock(Guard.GuardLockDuration);
                    }
                }
                else if (attack._entity != null)
                {
                    Enemy e = attack._entity.GetComponent<Enemy>();
                    if (e != null)
                    {
                        if (Guard != null)
                            e.StunLock(Guard.GuardLockDuration);
                    }
                }
            }

            return takeDamage;
            
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
            {                
                entity.spritesheets.ChangeAnimation(this._hitGuardAnim, true, 0, true, false, false);
                EffectsManager.GetEffect(_hitGuardSpritesheet, CurrentSide, entity.transform.Position, 0.0f, Vector2.Zero, 2.0f, 0.9f);
                return false;
            }

            _currentHealthPoints += damageValue;
            entity.spritesheets.ChangeAnimation(this._hitAnim, true, 0, true, false, false);

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

            IsInvincible = true;
            _invincibilityTimer = _invincibilityDuration;

            return true;
        }

        public void AirLock(float duration)
        {
            if (!entity.rigidbody.isGrounded)
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
                IsInvincible = true;
                _invincibilityTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                OpacityBlink(_invincibilityBlinkSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
            else
            {
                _opacityGoingDown = true;
                IsInvincible = false;
                _invincibilityTimer = 0.0f;
                entity.spritesheets.CurrentSpritesheet.opacity = 1f;
            }
            base.Update(gameTime);
        }

        public override void PostUpdate(GameTime gameTime)
        {
            CanMove = true;
            CanTurn = true;
            CanAttack = true;
            CanUseElement = true;
            if (Debug) Console.WriteLine("LiOn State -> " + State);
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
