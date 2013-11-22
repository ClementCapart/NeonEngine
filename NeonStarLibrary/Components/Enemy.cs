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
        Attack,
        StunLock,
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

        public EnemyState State;

        private float _currentHealthPoints;

        public float CurrentHealthPoints
        {
            get { return _currentHealthPoints; }
            set { _currentHealthPoints = value; }
        }

        private float _airLockDuration = 0.0f;

        public FollowNodes _followNodes;
        public ThreatArea _threatArea;
        public Chase _chase;
        public EnemyAttack _attack;
        public bool CanMove = true;
        private float _stunLockDuration = 0.0f;

        private Entity _damageOverTimeSource = null;
        private float _damageOverTimeValue = 0.0f;
        private float _damageOverTimeTimer = 0.0f;
        private float _damageOverTimeSpeed = 0.0f;
        private float _damageOverTimeTickTimer = 0.0f;

        private float _invincibilityDuration = 0.5f;

        public float InvincibilityDuration
        {
            get { return _invincibilityDuration; }
            set { _invincibilityDuration = value; }
        }

        private float _invincibilityTimer = 0.0f;


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

        private bool _opacityGoingDown = true;
        private Component _componentToTrigger = null;

        public Enemy(Entity entity)
            :base(entity, "Enemy")
        {
        }

        public override void Init()
        {
            if (_threatArea == null)
                _threatArea = entity.GetComponent<ThreatArea>();
            if (_followNodes == null)
                _followNodes = entity.GetComponent<FollowNodes>();
            if (_chase == null)
                _chase = entity.GetComponent<Chase>();
            if (_attack == null)
                _attack = entity.GetComponent<EnemyAttack>();
            if (_componentToTrigger == null && _componentToTriggerName != "")
            {
                _componentToTrigger = entity.GetComponentByName(_componentToTriggerName);
            }

            _currentHealthPoints = _startingHealthPoints;
            base.Init();
        }

        public bool ChangeHealthPoints(float value, Entity entity, Attack attack = null)
        {
            if (State != EnemyState.Dying && State != EnemyState.Dead && _invincibilityTimer <= 0.0f)
            {
                _currentHealthPoints += value;
                _invincibilityTimer = _invincibilityDuration;
                if (value < 0)
                    this.entity.spritesheets.ChangeAnimation(_hitAnim, true, 0, true, true, false);
                if (_triggerOnDamage && value < 0)
                {
                    if (_componentToTrigger != null)
                        _componentToTrigger.OnTrigger(this.entity, attack.Launcher != null ? attack.Launcher : attack._entity, new object[] { attack });
                }
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
                if (Debug)
                {
                    Console.WriteLine(this.entity.Name + " have lost " + value + " HP(s) -> Now at " + _currentHealthPoints + " HP(s).");
                }
                return true;
            }
            else
                return false;
 
        }

        public void StunLockEffect(float duration)
        {
            if (State != EnemyState.Dying && State != EnemyState.Dead && _invincibilityTimer <= 0.0f)
            {
                if (!_immuneToStunLock)
                {
                    _stunLockDuration = duration;
                    if (_stunLockDuration > 0)
                    {
                        entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                        State = EnemyState.StunLock;
                    }
                }
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
        }

        public override void PreUpdate(GameTime gameTime)
        {
            if (State == EnemyState.Dying)
            {
                if (entity.spritesheets != null && entity.spritesheets.CurrentSpritesheetName == _dyingAnim && entity.spritesheets.IsFinished())
                    State = EnemyState.Dead;
                else if (entity.spritesheets == null || (entity.spritesheets != null && _dyingAnim == ""))
                    State = EnemyState.Dead;

                entity.spritesheets.ChangeAnimation(_dyingAnim, true, 0, true, false, false);
                return;
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

            if (_stunLockDuration > 0 && State == EnemyState.StunLock)
            {
                _stunLockDuration -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                State = EnemyState.StunLock;
            }
            else if (State == EnemyState.StunLock)
            {
                State = EnemyState.StunLockEnd;
                entity.spritesheets.CurrentPriority = 0;
            }
            else
            {
                if (State == EnemyState.Idle && _followNodes != null)
                {
                    State = EnemyState.Patrol;
                }
            }

            if (entity.spritesheets != null && (entity.spritesheets.CurrentSpritesheet.IsLooped || entity.spritesheets.CurrentSpritesheet.IsFinished))
            {
                switch (State)
                {
                    case EnemyState.Patrol:
                        entity.spritesheets.ChangeAnimation(_runAnim);
                        break;

                    case EnemyState.Chase:
                        entity.spritesheets.ChangeAnimation(_runAnim);
                        break;

                    case EnemyState.FinishChase:
                        entity.spritesheets.ChangeAnimation(_runAnim);
                        break;

                    case EnemyState.Wait:
                    case EnemyState.WaitNode:
                    case EnemyState.WaitThreat:
                        entity.spritesheets.ChangeAnimation(_idleAnim);
                        break;

                    case EnemyState.Idle:
                        entity.spritesheets.ChangeAnimation(_idleAnim);
                        break;

                    case EnemyState.Attack:
                        if (_attack.CurrentAttack != null && _attack.CurrentAttack.CooldownStarted && !_attack.CurrentAttack.CooldownFinished && entity.spritesheets.CurrentSpritesheetName == _attackAnim)
                            entity.spritesheets.ChangeAnimation(_idleAnim);
                        else if ((entity.spritesheets.CurrentSpritesheetName == _startAttackAnim && entity.spritesheets.IsFinished() && _attackAnim != ""))
                            entity.spritesheets.ChangeAnimation(_attackAnim);
                        else if (entity.spritesheets.CurrentSpritesheetName != _attackAnim && (entity.spritesheets.CurrentSpritesheetName != _startAttackAnim || (entity.spritesheets.CurrentSpritesheetName == _startAttackAnim && entity.spritesheets.IsFinished())))
                            entity.spritesheets.ChangeAnimation(_startAttackAnim, true, 0, true, true, false, 0);
                        break;
                    
                    case EnemyState.Dying:
                        entity.spritesheets.ChangeAnimation(_dyingAnim, true, 0, true, false, false);
                        break;
                }
            } 
            base.PreUpdate(gameTime);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (_invincibilityTimer > 0.0f)
            {
                _invincibilityTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_opacityGoingDown)
                {
                    if (entity.spritesheets.CurrentSpritesheet.opacity > 0)
                    {
                        entity.spritesheets.ChangeOpacity(-25 * (float)gameTime.ElapsedGameTime.TotalSeconds);
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
                        entity.spritesheets.ChangeOpacity(25 * (float)gameTime.ElapsedGameTime.TotalSeconds);
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
            base.Update(gameTime);
        }

        public override void PostUpdate(GameTime gameTime)
        {
            if (_damageOverTimeTimer > 0.0f)
            {
                _damageOverTimeTickTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_damageOverTimeTickTimer > _damageOverTimeSpeed)
                {
                    _damageOverTimeTickTimer = 0.0f;
                    ChangeHealthPoints(_damageOverTimeValue, _damageOverTimeSource);
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

            if (State == EnemyState.Dead)
                this.entity.Destroy();
            base.PostUpdate(gameTime);
        }

        public void AfflictDamageOverTime(float damageValue, float damageTimer, float damageSpeed, Entity source)
        {
            _damageOverTimeValue = damageValue;
            _damageOverTimeTimer = damageTimer;
            _damageOverTimeSpeed = damageSpeed;
            _damageOverTimeSource = source;
        }
    }
}
