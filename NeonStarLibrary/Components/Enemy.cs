using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    public enum EnemyState
    {
        Idle,
        Patrol,
        Chase,
        MustFinishChase,
        FinishChase,
        Wait,
        WaitNode,
        WaitThreat,
        Attacking,
        StunLocked,
        StunLockEnd,
        Dying,
        Dead
    }

    public enum EnemyType
    {
        Ground,
        Flying
    }

    public class Enemy : Component
    {

        #region Properties
        private bool _debug;
        public bool Debug
        {
            get { return _debug; }
            set { _debug = value; }
        }
        private float _startingHealthPoints = 10.0f;
        public float StartingHealthPoints
        {
            get { return _startingHealthPoints; }
            set { _startingHealthPoints = value; }
        }

        private Element _coreElement = Element.Neutral;

        public Element CoreElement
        {
            get { return _coreElement; }
            set { _coreElement = value; }
        }

        private float _currentHealthPoints;

        public float CurrentHealthPoints
        {
            get { return _currentHealthPoints; }
            set { _currentHealthPoints = value; }
        }

        private float _invincibilityDuration = 0.5f;

        public float InvincibilityDuration
        {
            get { return _invincibilityDuration; }
            set { _invincibilityDuration = value; }
        }

        private string _runAnim = "";

        public string RunAnim
        {
            get { return _runAnim; }
            set { _runAnim = value; }
        }

        private string _idleAnim = "";

        public string IdleAnim
        {
            get { return _idleAnim; }
            set { _idleAnim = value; }
        }

        private string _startAttackAnim = "";

        public string StartAttackAnim
        {
            get { return _startAttackAnim; }
            set { _startAttackAnim = value; }
        }

        private string _attackAnim = "";

        public string AttackAnim
        {
            get { return _attackAnim; }
            set { _attackAnim = value; }
        }

        private string _stunlockAnim = "";

        public string StunlockAnim
        {
            get { return _stunlockAnim; }
            set { _stunlockAnim = value; }
        }

        private string _dyingAnim = "";

        public string DyingAnim
        {
            get { return _dyingAnim; }
            set { _dyingAnim = value; }
        }

        private string _hitAnim = "";

        public string HitAnim
        {
            get { return _hitAnim; }
            set { _hitAnim = value; }
        }

        private bool _immuneToStunLock = false;

        public bool ImmuneToStunLock
        {
            get { return _immuneToStunLock; }
            set { _immuneToStunLock = value; }
        }

        private bool _immuneToImpulse = false;

        public bool ImmuneToImpulse
        {
            get { return _immuneToImpulse; }
            set { _immuneToImpulse = value; }
        }

        private bool _immuneToDeath = false;

        public bool ImmuneToDeath
        {
            get { return _immuneToDeath; }
            set { _immuneToDeath = value; }
        }

        private EnemyType _type = EnemyType.Ground;

        public EnemyType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        private bool _triggerOnDamage = false;

        public bool TriggerOnDamage
        {
            get { return _triggerOnDamage; }
            set { _triggerOnDamage = value; }
        }

        private string _componentToTriggerName = "";

        public string ComponentToTriggerName
        {
            get { return _componentToTriggerName; }
            set { _componentToTriggerName = value; }
        }
        #endregion

        public EnemyState State = EnemyState.Idle;

        public FollowNodes FollowNodes;
        public ThreatArea ThreatArea;
        public Chase Chase;
        public EnemyAttack Attack;

        public Side CurrentSide = Side.Right;

        public bool CanMove = true;
        public bool CanTurn = true;
        public bool CanAttack = true;

        public bool IsInvincible = false;
        public bool IsAirLocked = false;
        
        private float _stunLockDuration = 0.0f;
        private float _airLockDuration = 0.0f;
        private float _invincibilityTimer = 0.0f;

        private Attack _damageOverTimeSource = null;
        private float _damageOverTimeValue = 0.0f;
        private float _damageOverTimeTimer = 0.0f;
        private float _damageOverTimeSpeed = 0.0f;
        private float _damageOverTimeTickTimer = 0.0f;

        private bool _opacityGoingDown = true;
        private Component _componentToTrigger = null;

        public Enemy(Entity entity)
            :base(entity, "Enemy")
        {
        }

        public override void Init()
        {
            if (ThreatArea == null)
                ThreatArea = entity.GetComponent<ThreatArea>();
            if (FollowNodes == null)
                FollowNodes = entity.GetComponent<FollowNodes>();
            if (Chase == null)
                Chase = entity.GetComponent<Chase>();
            if (Attack == null)
                Attack = entity.GetComponent<EnemyAttack>();
            if (_componentToTrigger == null && _componentToTriggerName != "")
            {
                _componentToTrigger = entity.GetComponentByName(_componentToTriggerName);
            }
            _currentHealthPoints = _startingHealthPoints;
            base.Init();
        }

        public bool TakeDamage(Attack attack)
        {
            bool tookDamage = TakeDamage(attack.DamageOnHit, attack.StunLock, attack.TargetAirLock, attack.CurrentSide);
            if(tookDamage && _triggerOnDamage)
                if (_componentToTrigger != null)
                    _componentToTrigger.OnTrigger(this.entity, attack.Launcher != null ? attack.Launcher : attack._entity, new object[] { attack });
            return tookDamage;
        }

        public bool TakeDamage(Bullet bullet)
        {
            return TakeDamage(bullet.DamageOnHit, bullet.StunLock, 0.0f, bullet.entity.spritesheets.CurrentSide);
        }

        public bool TakeDamage(float damageValue, float stunLockDuration, float airLockDuration, Side side)
        {
            if (IsInvincible)
                return false;

            if (damageValue >= 0.0f)
                return false;

            _currentHealthPoints += damageValue;

            if (Debug)
            {
                Console.WriteLine(entity.Name + " have lost " + damageValue + " HP(s) -> Now at " + _currentHealthPoints + " HP(s).");
            }

            if (stunLockDuration > 0.0f && !_immuneToStunLock)
            {
                _stunLockDuration = stunLockDuration;
                entity.rigidbody.body.LinearVelocity = Vector2.Zero;

                if (State == EnemyState.Attacking && Attack != null && Attack.CurrentAttack != null)
                {
                    Attack.CurrentAttack.CancelAttack();
                    Attack.CurrentAttack = null;
                }

                State = EnemyState.StunLocked;
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
            if (State == EnemyState.Dying)
            {
                if (entity.spritesheets != null && entity.spritesheets.CurrentSpritesheetName == _dyingAnim && entity.spritesheets.IsFinished())
                    State = EnemyState.Dead;
                else if (entity.spritesheets == null || (entity.spritesheets != null && _dyingAnim == ""))
                    State = EnemyState.Dead;
            }
            else if (State != EnemyState.Dead)
            {
                if (_currentHealthPoints <= 0.0f && !_immuneToDeath)
                {
                    this.State = EnemyState.Dying;

                    if (entity.rigidbody != null)
                    {
                        entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                    }

                    if (entity != null && CoreElement != Element.Neutral)
                        entity.GetComponent<Avatar>().ElementSystem.GetElement(CoreElement);
                }

                if (_airLockDuration > 0.0f && IsAirLocked)
                {
                    _airLockDuration -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    entity.rigidbody.GravityScale = 0.0f;
                    CanMove = false;
                }
                else
                {
                    _airLockDuration = 0.0f;
                    IsAirLocked = true;
                }

                if (_stunLockDuration > 0.0f)
                {
                    _stunLockDuration -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    State = EnemyState.StunLocked;
                    CanMove = false;
                    CanTurn = false;
                    CanAttack = false;
                }
                else if (State == EnemyState.StunLocked)
                    State = EnemyState.Idle;
            }
            base.PreUpdate(gameTime);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (State != EnemyState.Dying && State != EnemyState.Dead)
            {
                if (entity.spritesheets != null)
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
                }           

                if (_damageOverTimeTimer > 0.0f)
                {
                    _damageOverTimeTickTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (_damageOverTimeTickTimer > _damageOverTimeSpeed)
                    {
                        _damageOverTimeTickTimer = 0.0f;
                        TakeDamage(_damageOverTimeValue, 0.0f, 0.0f, _damageOverTimeSource.CurrentSide);
                    }
                    _damageOverTimeTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else
                {
                    _damageOverTimeTickTimer = 0.0f;
                    _damageOverTimeTimer = 0.0f;
                    _damageOverTimeSpeed = 0.0f;
                    _damageOverTimeValue = 0.0f;
                    _damageOverTimeSource = null;
                }
            }           
            base.Update(gameTime);
        }

        public override void PostUpdate(GameTime gameTime)
        {            
            if (State == EnemyState.Dead)
            {
                this.entity.Destroy();
                return;
            }
            else if (State != EnemyState.Dying && State != EnemyState.StunLocked)
            {
                State = EnemyState.Idle;
                CanMove = true;
                CanTurn = true;
                CanAttack = true;
            }
            base.PostUpdate(gameTime);
        }

        public void AfflictDamageOverTime(float damageValue, float damageTimer, float damageSpeed, Attack source)
        {
            _damageOverTimeValue = damageValue;
            _damageOverTimeTimer = damageTimer;
            _damageOverTimeSpeed = damageSpeed;
            _damageOverTimeSource = source;
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
