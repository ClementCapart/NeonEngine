using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NeonEngine;
using NeonStarLibrary.Components.Enemies;
using NeonStarLibrary.Private;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NeonStarLibrary.Components;
using NeonStarLibrary.Components.Graphics2D;

namespace NeonStarLibrary.Components.Avatar
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
        UsingElement,
        Dying,
        FastRespawning,
        Respawning,
        Saving,
        FinishSaving,
        ChangingLevel
    }

    public enum DamageResult
    {
        Effective,
        Missed,
        Guarded
    }

    public class AvatarCore : Component
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

        private string _respawnAnimation = "";

        public string RespawnAnimation
        {
            get { return _respawnAnimation; }
            set { _respawnAnimation = value; }
        }
        #endregion       

        public static int AvatarDeath = 0;
        public static float TotalGameTime = 0.0f;
        public static float TimeSinceLastCompletion = 0.0f;

        public static float TimeSinceLastDeath = 0.0f;
        public static List<float> TimeBeforeDeaths = new List<float>();
        
        public static float HealedHealthPointsSinceLastDeath = 0.0f;
        public static float TotalHealedHealthPoints = 0.0f;
        public static List<float> HealedHealthPointsBeforeDeaths = new List<float>();

        public static int NumberOfGameCompleted = 0;
        public static List<float> CompletionTime = new List<float>();       

        public AvatarState State = AvatarState.Idle;

        public AvatarAnimationManager AvatarAnimationManager;
        public MeleeFight MeleeFight;
        public ThirdPersonController ThirdPersonController;
        public Guard Guard;
        public ElementSystem ElementSystem;
        public EnergySystem EnergySystem;
        public DamageDisplayer DamageDisplayer;

        public Side CurrentSide = Side.Right;

        public bool CanMove = true;
        public bool CanTurn = true;
        public bool CanAttack = true;
        public bool CanUseElement = true;
        public bool CanRoll = true;

        public bool IsInvincible = false;
        public bool IsAirLocked = false;

        private float _stunLockDuration = 0.0f;
        private float _invincibilityTimer = 0.0f;
        private float _airLockDuration = 0.0f;

        public float IdleTimer = 0.0f;
        
        private bool _opacityGoingDown = true;
        private SpriteSheetInfo _hitGuardSpritesheet = null;

        public AvatarCore(Entity entity)
            :base(entity, "AvatarCore")
        {
        }

        public override void Init()
        {
            AvatarAnimationManager = this.entity.GetComponent<AvatarAnimationManager>();
            MeleeFight = this.entity.GetComponent<MeleeFight>();
            ThirdPersonController = this.entity.GetComponent<ThirdPersonController>();
            DamageDisplayer = this.entity.GetComponent<DamageDisplayer>();
            Guard = this.entity.GetComponent<Guard>();
            ElementSystem = this.entity.GetComponent<ElementSystem>();
            EnergySystem = this.entity.GetComponent<EnergySystem>();
            _hitGuardSpritesheet = AssetManager.GetSpriteSheet(_hitGuardFX);
            base.Init();
        }

        public DamageResult TakeDamage(Attack attack)
        {
            DamageResult takeDamage = TakeDamage(attack.DamageOnHit, attack.StunLock, attack.TargetAirLock, attack.CurrentSide, attack.Type);

            if (takeDamage == DamageResult.Guarded)
            {
                if (attack.Launcher != null)
                {
                    EnemyCore e = attack.Launcher.GetComponent<EnemyCore>();
                    if (e != null)
                    {
                        if (Guard != null)
                        {
                            e.StunLock(Guard.GuardLockDuration);
                            if (e.entity.hitboxes.Count > 0)
                                e.entity.hitboxes[0].Type = NeonEngine.Components.CollisionDetection.HitboxType.Main;
                        }
                    }
                }
                else if (attack._entity != null)
                {
                    EnemyCore e = attack._entity.GetComponent<EnemyCore>();
                    if (e != null)
                    {
                        if (Guard != null)
                        {
                            e.StunLock(Guard.GuardLockDuration);
                            if (e.entity.hitboxes.Count > 0)
                                e.entity.hitboxes[0].Type = NeonEngine.Components.CollisionDetection.HitboxType.Main;
                        }
                    }
                }
            }

            return takeDamage;
            
        }

        public DamageResult TakeDamage(Bullet bullet)
        {
            return TakeDamage(bullet.DamageOnHit, bullet.StunLock, 0.0f, bullet.entity.spritesheets.CurrentSide);
        }

        public DamageResult TakeDamage(float damageValue, float stunLockDuration, float airLockDuration, Side side, AttackType attackType = AttackType.MeleeLight)
        {
            if (IsInvincible)
                return DamageResult.Missed;

            if (State == AvatarState.Guarding && attackType != AttackType.Special)
            {
                damageValue = 0;
                if (damageValue >= 0.0f)
                {
                    entity.spritesheets.ChangeAnimation(this._hitGuardAnim, true, 0, true, false, false);
                    EffectsManager.GetEffect(_hitGuardSpritesheet, CurrentSide, entity.transform.Position, 0.0f, Vector2.Zero, 2.0f, 0.9f);
                    if (DamageDisplayer != null)
                    {
                        DamageDisplayer.DisplayDamage(0);
                    }
                    return DamageResult.Guarded;
                }
            }     

            _currentHealthPoints += damageValue;
            if(damageValue < 0)
                entity.spritesheets.ChangeAnimation(this._hitAnim, true, 0, true, true, false);

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

            if (damageValue < 0)
            {
                IsInvincible = true;
                _invincibilityTimer = _invincibilityDuration;
                if (DamageDisplayer != null)
                {
                    DamageDisplayer.DisplayDamage(damageValue);
                }
                return DamageResult.Effective;
            }

            return DamageResult.Effective;
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
            TotalGameTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            TimeSinceLastDeath += (float)gameTime.ElapsedGameTime.TotalSeconds;
            TimeSinceLastCompletion += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (State != AvatarState.Idle || !entity.rigidbody.isGrounded)
                IdleTimer = 0.0f;
            else
                IdleTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (State == AvatarState.FastRespawning)
            {
                CanMove = false;
                CanAttack = false;
                CanTurn = false;
                CanUseElement = false;
                CanRoll = false;
                if (entity.rigidbody != null)
                    entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                if (entity.spritesheets.CurrentSpritesheetName == RespawnAnimation && entity.spritesheets.IsFinished())
                    State = AvatarState.Idle;
            }
            else if (State == AvatarState.ChangingLevel)
            {
                CanMove = false;
                CanAttack = false;
                CanTurn = false;
                CanUseElement = false;
                CanRoll = false;
               
            }
            if (State != AvatarState.Dying && State != AvatarState.FastRespawning)
            {
                if (CurrentHealthPoints <= 0 && State != AvatarState.Dying)
                {
                    State = AvatarState.Dying;
                    if (entity.rigidbody != null)
                        entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                    
                    ///////////////////
                    AvatarDeath++;
                    TimeBeforeDeaths.Add(TimeSinceLastDeath);
                    TimeSinceLastDeath = 0.0f;
                    HealedHealthPointsBeforeDeaths.Add(HealedHealthPointsSinceLastDeath);
                    HealedHealthPointsSinceLastDeath = 0.0f;
                    //////////////////

                    (this.entity.GameWorld as GameScreen).Respawn();
                }
                else if (_airLockDuration > 0.0f && IsAirLocked)
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

                if (_stunLockDuration > 0.0f && State != AvatarState.Dying)
                {
                    _stunLockDuration -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    State = AvatarState.Stunlocked;
                    CanMove = false;
                    CanTurn = false;
                    CanAttack = false;
                    CanUseElement = false;
                    CanRoll = false;
                }
                else
                {
                    _stunLockDuration = 0.0f;
                }
            }
            

            base.PreUpdate(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            if (State != AvatarState.Dying && State != AvatarState.FastRespawning && State != AvatarState.Saving && State != AvatarState.FinishSaving)
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
                    if (entity.spritesheets != null)
                        entity.spritesheets.CurrentSpritesheet.Opacity = 1f;
                }
            }
            base.Update(gameTime);
        }

        public override void PostUpdate(GameTime gameTime)
        {
            CanMove = true;
            CanTurn = true;
            CanAttack = true;
            CanUseElement = true;
            CanRoll = true;
            if (Debug) Console.WriteLine("LiOn State -> " + State);
            base.PostUpdate(gameTime);
        }

        public void OpacityBlink(float value)
        {
            if (_opacityGoingDown)
            {
                if (entity.spritesheets.CurrentSpritesheet.Opacity > 0)
                {
                    entity.spritesheets.ChangeOpacity(-value);
                }
                else
                {
                    _opacityGoingDown = false;
                    entity.spritesheets.CurrentSpritesheet.Opacity = 0.0f;
                }                  
            }
            else
            {
                if (entity.spritesheets.CurrentSpritesheet.Opacity < 1)
                {
                    entity.spritesheets.ChangeOpacity(value);
                }
                else
                {
                    _opacityGoingDown = true;
                    entity.spritesheets.CurrentSpritesheet.Opacity = 1.0f;
                }
            }
        }
    }
}
