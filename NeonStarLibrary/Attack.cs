using Microsoft.Xna.Framework;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    public class AttackEffect
    {
        public SpecialEffect specialEffect = SpecialEffect.Impulse;
        public string NameType
        {
            get { return specialEffect.ToString(); }
        }
        public object[] Parameters;

        public AttackEffect(SpecialEffect specialEffect, object[] Parameters)
        {
            this.specialEffect = specialEffect;
            this.Parameters = Parameters;
        }
    }

    public class Attack
    {
        private bool _hit = false;
        private bool _mustStopAtTargetSight = false;

        string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        AttackType _type;
        public AttackType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        private bool _onlyOnceInAir = false;

        public bool OnlyOnceInAir
        {
            get { return _onlyOnceInAir; }
            set { _onlyOnceInAir = value; }
        }

        private bool _airOnly = false;

        public bool AirOnly
        {
            get { return _airOnly; }
            set { _airOnly = value; }
        }

        private bool _cancelOnGround = false;

        public bool CancelOnGround
        {
            get { return _cancelOnGround; }
            set { _cancelOnGround = value; }
        }


        private float _damageOnHit = 1.0f;
        public float DamageOnHit
        {
            get { return _damageOnHit; }
            set { _damageOnHit = value; }
        }

        List<Hitbox> _hitboxes;

        private float _delay = 1.0f;
        public float Delay
        {
            get { return _delay; }
            set { _delay = value; }
        }

        private float _cooldown = 1.0f;
        public float Cooldown
        {
            get { return _cooldown; }
            set { _cooldown = value; }
        }

        private float _duration = 1.0f;
        public float Duration
        {
            get { return _duration; }
            set { _duration = value; }
        }

        private float _airLock = 0.0f;
        public float AirLock
        {
            get { return _airLock; }
            set { _airLock = value; }
        }

        private float _stunLock = 0.0f;

        public float StunLock
        {
            get { return _stunLock; }
            set { _stunLock = value; }
        }

        private Side _side = Side.Right;
        public Side CurrentSide
        {
          get { return _side; }
          set { _side = value; }
        }

        private float _targetAirLock = 0.0f;
        public float TargetAirLock
        {
            get { return _targetAirLock; }
            set { _targetAirLock = value; }
        }

        private bool _delayStarted = false;

        public bool DelayStarted
        {
            get { return _delayStarted; }
            set { _delayStarted = value; }
        }

        private bool _durationStarted = false;

        public bool DurationStarted
        {
            get { return _durationStarted; }
            set { _durationStarted = value; }
        }

        private bool _cooldownStarted = false;

        public bool CooldownStarted
        {
            get { return _cooldownStarted; }
            set { _cooldownStarted = value; }
        }

        private bool _delayFinished = false;

        public bool DelayFinished
        {
            get { return _delayFinished; }
            set { _delayFinished = value; }
        }

        private bool _durationFinished = false;

        public bool DurationFinished
        {
            get { return _durationFinished; }
            set { _durationFinished = value; }
        }

        private bool _cooldownFinished = false;

        public bool CooldownFinished
        {
            get { return _cooldownFinished; }
            set { _cooldownFinished = value; }
        }

        private bool _airLockFinished = false;

        private bool _airLocked = false;

        public bool AirLocked
        {
          get { return _airLocked; }
          set { _airLocked = value; }
        }

        public bool AirLockFinished
        {
            get { return _airLockFinished; }
            set { _airLockFinished = value; }
        }

        private float _airFactor = 1.0f;

        public float AirFactor
        {
            get { return _airFactor; }
            set { _airFactor = value; }
        }

        private List<AttackEffect> _specialEffects = new List<AttackEffect>();

        private List<AttackEffect> _onHitSpecialEffects  = new List<AttackEffect>();
        private List<AttackEffect> _onGroundCancelSpecialEffects = new List<AttackEffect>();

        private List<Hitbox> _alreadyTouched = new List<Hitbox>();

        public bool Canceled = false;
        public Entity _entity;
        public AttackInfo AttackInfo;
        private MeleeFight _meleeFight;
        private bool _fromEnemy = false;
        private Entity _target;

        public Attack()
        {
        }

        public Attack(AttackInfo attackInfo, Side side, Entity launcher, Entity target, bool FromEnemy = false)
            : this(attackInfo, side, launcher, FromEnemy)
        {
            this._target = target;
        }

        public Attack(AttackInfo attackInfo, Side side, Vector2 Position)
            :this(attackInfo, side, null)
        {
            _entity = new Entity(Neon.world);
            _entity.Name = "AttackHolder";
            _entity.transform.Position = Position;
        }

        public Attack(AttackInfo attackInfo, Side side, Entity launcher, bool FromEnemy = false)
        {           
            AttackInfo = attackInfo;
            _hitboxes = new List<Hitbox>();
            this._side = side;
            if(launcher != null) this._entity = launcher;
            this.Name = attackInfo.Name;
            this.Type = attackInfo.Type;
            this.Delay = attackInfo.Delay;
            this.DamageOnHit = attackInfo.DamageOnHit;
            this.Cooldown = attackInfo.Cooldown;
            this.Duration = attackInfo.Duration;
            this.AirLock = attackInfo.AirLock;
            this.TargetAirLock = attackInfo.TargetAirLock;
            this.StunLock = attackInfo.StunLock;
            this.AirOnly = attackInfo.AirOnly;
            this.CancelOnGround = attackInfo.CancelOnGround;
            this.OnlyOnceInAir = attackInfo.OnlyOnceInAir;
            this._airFactor = attackInfo.AirFactor;
            this._fromEnemy = FromEnemy;

            foreach (AttackEffect ae in attackInfo.OnHitSpecialEffects)
            {
                this._onHitSpecialEffects.Add(ae);
            }

            DelayStarted = true;


            if (_entity != null && _entity.Name != "AttackHolder")
            {
                _meleeFight = _entity.GetComponent<MeleeFight>();

                if (OnlyOnceInAir && !_entity.rigidbody.isGrounded)
                {
                    if (_meleeFight != null)
                    {
                        if (_meleeFight.AttacksWhileInAir.Contains(attackInfo.Name) || _meleeFight.AttacksWhileInAir.Contains(attackInfo.Name.Remove(attackInfo.Name.Length - 6)))
                        {
                            _meleeFight.CurrentComboHit = _meleeFight.LastComboHit;
                            this.CancelAttack();
                            Canceled = true;
                            _entity.spritesheets.CurrentPriority = 0;
                            return;
                        }
                        else
                        {
                            _meleeFight.AttacksWhileInAir.Add(attackInfo.Name);
                            if (attackInfo.Name.Substring(attackInfo.Name.Length - 6) == "Finish")
                            {
                                _meleeFight.AttacksWhileInAir.Add(attackInfo.Name);
                                _meleeFight.AttacksWhileInAir.Add(attackInfo.Name.Remove(attackInfo.Name.Length - 6));
                            }
                            else
                            {
                                _meleeFight.AttacksWhileInAir.Add(attackInfo.Name);
                                _meleeFight.AttacksWhileInAir.Add(attackInfo.Name + "Finish");
                            }
                        }
                    }
                }
            }

            if (this.Delay <= 0.0f && _entity != null)
                Init();

            if (_entity != null && _entity.Name != "AttackHolder")
            {
                if (AirOnly)
                    if (this._entity.rigidbody.isGrounded && _meleeFight != null)
                    {
                        _meleeFight.CurrentComboHit = _meleeFight.LastComboHit;
                        this.CancelAttack();
                        Canceled = true;
                        return;
                    }

                if (AirLock <= 0.0f || (_entity.rigidbody.isGrounded && ((_meleeFight != null && _meleeFight.ThirdPersonController != null && !_meleeFight.ThirdPersonController.StartJumping) || _meleeFight == null)))
                    AirLockFinished = true;
            }

            if (_type == AttackType.MeleeSpecial && _meleeFight != null)
            {
                _meleeFight.CurrentComboHit = ComboSequence.None;
            }
        }

        public void Init()
        {
            foreach (Rectangle hitbox in AttackInfo.Hitboxes)
            {
                Hitbox hb = Neon.world.HitboxPool.GetAvailableItem();
                hb.Type = HitboxType.Hit;
                hb.PoolInit(_entity);
                hb.Width = hitbox.Width;
                hb.Height = hitbox.Height;
                hb.Center = _entity.transform.Position;

                hb.OffsetX = this._side == Side.Right ? hitbox.X : -hitbox.X;
                hb.OffsetY = hitbox.Y;
                
                _entity.AddComponent(hb);
                _hitboxes.Add(hb);
            }

            if(_entity.Name != "AttackHolder" && (_meleeFight != null && _meleeFight.Debug))
                Console.WriteLine(AttackInfo.Name + " launched ! Current Combo -> " + _meleeFight.CurrentComboHit.ToString());

            foreach (AttackEffect ae in AttackInfo.SpecialEffects)
            {
                this._specialEffects.Add(ae);
            }

            foreach (AttackEffect ae in AttackInfo.OnGroundCancelSpecialEffects)
            {
                this._onGroundCancelSpecialEffects.Add(ae);
            }

            this.DelayFinished = true;
            this.DurationStarted = true;
        }

        public void Update(GameTime gameTime)
        {
            if (DelayStarted && !DelayFinished)
            {
                if (Delay > 0.0f)
                    Delay -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                else
                {
                    Delay = 0.0f;
                    Init();
                }
            }
            else if (DurationStarted && !DurationFinished)
            {
                for(int i = _specialEffects.Count - 1; i >= 0; i--)
                {
                    AttackEffect ae = _specialEffects.ElementAt(i);
                    switch (ae.specialEffect)
                    {
                        case SpecialEffect.Impulse:
                            if (_entity != null)
                            {
                                Vector2 impulseForce = (Vector2)ae.Parameters[0] * (_entity.rigidbody.isGrounded ? 1 : AirFactor);
                                _mustStopAtTargetSight = (bool)ae.Parameters[1];
                                _entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                                _entity.rigidbody.body.ApplyLinearImpulse(new Vector2(_side == Side.Right ? impulseForce.X : -impulseForce.X, impulseForce.Y));
                            }
                            break;

                        case SpecialEffect.Boost:
                            break;

                        case SpecialEffect.DamageOverTime:
                            break;

                        case SpecialEffect.StartAttack:
                            string attackName = (string)(ae.Parameters[0]);
                            AttacksManager.StartFreeAttack(attackName, _side, _entity.transform.Position);
                            break;

                        case SpecialEffect.ShootBullet:
                            BulletInfo bi = (BulletInfo)ae.Parameters[0];
                            BulletsManager.CreateBullet(bi, _side,  Vector2.Zero, _entity, (GameScreen)Neon.world, _fromEnemy);
                            break;

                        case SpecialEffect.ShootBulletAtTarget:
                            BulletInfo bi2 = (BulletInfo)ae.Parameters[0];
                            if(_target != null)
                                BulletsManager.CreateBullet(bi2, _side, Vector2.Normalize(_target.transform.Position - _entity.transform.Position), _entity, (GameScreen)Neon.world, _fromEnemy);
                            break;

                        case SpecialEffect.Invincible:
                            _entity.hitbox.SwitchType(HitboxType.Invincible, (float)(ae.Parameters[0]));
                            break;

                        case SpecialEffect.EffectAnimation:
                            break;

                    }
                    _specialEffects.Remove(ae);
                }

                for(int i = _hitboxes.Count - 1; i >= 0; i --)
                {
                    Hitbox hitbox = _hitboxes[i];
                    for(int j = Neon.world.Hitboxes.Count - 1; j >= 0; j--)
                    {
                        Hitbox hb = Neon.world.Hitboxes[j];
                        if (hb.Type == HitboxType.Main && hb.entity != this._entity)
                        {
                            if (hb.hitboxRectangle.Intersects(hitbox.hitboxRectangle) && !_alreadyTouched.Contains(hb))
                            {
                                Effect(hb.entity);
                                if (this.Type == AttackType.MeleeLight || this.Type == AttackType.MeleeSpecial)
                                {
                                    _alreadyTouched.Add(hb);
                                }
                                else
                                {
                                    hitbox.Remove();
                                    _hitboxes.Remove(hitbox);
                                }
                            }
                        }                      
                    }
                }

                Duration -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (Duration <= 0)
                {
                    DurationFinished = true;
                    CooldownStarted = true;
                    for (int i = _hitboxes.Count - 1; i >= 0; i--)
                    {
                        _hitboxes[i].Remove();
                    }
                    this._hitboxes.Clear();
                }
            }
            else if(CooldownStarted && !CooldownFinished)
            {
                if (Cooldown > 0.0f)
                {
                    Cooldown -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else
                {
                    Cooldown = 0.0f;
                    CooldownFinished = true;
                }
            }
            if (_entity.Name != "AttackHolder")
            {
                if (!_entity.rigidbody.isGrounded)
                {
                    if (!AirLocked && !AirLockFinished && (_type != AttackType.MeleeLight || _type == AttackType.MeleeLight && _hit))
                    {
                        _entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                        AirLocked = true;
                    }
                    else if (AirLocked)
                    {
                        AirLock -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }

                    if (AirLock <= 0.0f)
                    {
                        AirLocked = false;
                        AirLockFinished = true;
                    }
                }

                if (CancelOnGround && _entity.rigidbody.isGrounded && !Canceled)
                {
                    for (int i = _onGroundCancelSpecialEffects.Count - 1; i >= 0; i--)
                    {
                        AttackEffect ae = _onGroundCancelSpecialEffects.ElementAt(i);
                        switch (ae.specialEffect)
                        {
                            case SpecialEffect.Impulse:
                                Vector2 impulseForce = (Vector2)ae.Parameters[0] * (_entity.rigidbody.isGrounded ? 1 : AirFactor);
                                _entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                                _entity.rigidbody.body.ApplyLinearImpulse(new Vector2(_side == Side.Right ? impulseForce.X : -impulseForce.X, impulseForce.Y));
                                break;

                            case SpecialEffect.Boost:
                                break;

                            case SpecialEffect.DamageOverTime:
                                break;

                            case SpecialEffect.StartAttack:
                                string attackName = (string)ae.Parameters[0];
                                AttacksManager.StartFreeAttack(attackName, _side, _entity.transform.Position);
                                break;

                            case SpecialEffect.ShootBullet:
                                BulletInfo bi = (BulletInfo)ae.Parameters[0];
                                BulletsManager.CreateBullet(bi, _side, Vector2.Zero, _entity, (GameScreen)Neon.world, _fromEnemy);
                                break;

                            case SpecialEffect.ShootBulletAtTarget:
                                BulletInfo bi2 = (BulletInfo)ae.Parameters[0];
                                if (_target != null)
                                    BulletsManager.CreateBullet(bi2, _side, Vector2.Normalize(_target.transform.Position - _entity.transform.Position), _entity, (GameScreen)Neon.world, _fromEnemy);
                                break;

                            case SpecialEffect.Invincible:
                                _entity.hitbox.SwitchType(HitboxType.Invincible, (float)ae.Parameters[0]);
                                break;
                        }
                        _onGroundCancelSpecialEffects.Remove(ae);
                    }

                    for (int i = _hitboxes.Count - 1; i >= 0; i--)
                    {
                        _hitboxes[i].Remove();
                    }
                    _hitboxes.Clear();
                    this.DurationStarted = true;
                    this.DurationFinished = true;
                    this.DelayStarted = true;
                    this.DelayStarted = true;
                    this.CooldownStarted = true;
                    this.AirLocked = false;
                    this.AirLockFinished = true;
                    this.Canceled = true;
                }
            }

            if (_mustStopAtTargetSight)
            {
                if(_side == Side.Left)
                    if (_target.hitbox.Type != HitboxType.Invincible && _entity.rigidbody.beacon.CheckLeftSide(Math.Abs(_entity.rigidbody.body.LinearVelocity.X) * 4, true) == _target)
                        _entity.rigidbody.body.LinearVelocity = Vector2.Zero;

                if (_side == Side.Right)
                    if (_target.hitbox.Type != HitboxType.Invincible &&  _entity.rigidbody.beacon.CheckRightSide(Math.Abs(_entity.rigidbody.body.LinearVelocity.X) * 4, true) == _target)
                        _entity.rigidbody.body.LinearVelocity = Vector2.Zero;
            }
        }

        private void Effect(Entity entity)
        {
            bool validTarget = false;

            if (!_fromEnemy)
            {
                Enemy enemy = entity.GetComponent<Enemy>();
                if (enemy != null)
                {
                    validTarget = true;
                    _hit = true;
                    enemy.ChangeHealthPoints(_damageOnHit);
                    enemy.StunLockEffect(_stunLock);
                    if (!enemy.entity.rigidbody.isGrounded)
                        enemy.AirLock(TargetAirLock);
                }
            }
            else
            {
                Avatar avatar = entity.GetComponent<Avatar>();
                if(avatar != null)
                {
                    validTarget = true;
                    _hit = true;
                    avatar.ChangeHealthPoints(_damageOnHit);
                    avatar.StunLockEffect(_stunLock);
                    if(!avatar.entity.rigidbody.IsGround)
                        avatar.AirLock(TargetAirLock);
                }
            }

            if (validTarget)
            {
                bool velocityReset = false;

                foreach (AttackEffect ae in _onHitSpecialEffects)
                {
                    switch (ae.specialEffect)
                    {
                        case SpecialEffect.Impulse:
                            Vector2 impulseForce = (Vector2)ae.Parameters[0];
                            if (!velocityReset) entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                            entity.rigidbody.body.ApplyLinearImpulse(new Vector2(_side == Side.Right ? impulseForce.X : -impulseForce.X, impulseForce.Y) * (entity.rigidbody.isGrounded ? 1 : AirFactor));
                            velocityReset = true;
                            break;

                        case SpecialEffect.PositionalPulse:
                            Vector2 pulseForce = (Vector2)ae.Parameters[0];
                            if (!velocityReset) entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                            entity.rigidbody.body.ApplyLinearImpulse(new Vector2(pulseForce.X * (_entity.transform.Position.X < entity.transform.Position.X ? 1 : -1), pulseForce.Y * (_entity.transform.Position.Y < entity.transform.Position.Y ? 1 : -1)));
                            velocityReset = true;
                            break;

                        case SpecialEffect.Boost:
                            break;

                        case SpecialEffect.DamageOverTime:
                            break;

                        case SpecialEffect.StartAttack:
                            string attackName = (string)ae.Parameters[0];
                            AttacksManager.StartFreeAttack(attackName, _side, _entity.transform.Position);
                            break;

                        case SpecialEffect.ShootBullet:
                            BulletInfo bi = (BulletInfo)ae.Parameters[0];
                            BulletsManager.CreateBullet(bi, _side, Vector2.Zero, _entity, (GameScreen)Neon.world, _fromEnemy);
                            break;

                        case SpecialEffect.ShootBulletAtTarget:
                            BulletInfo bi2 = (BulletInfo)ae.Parameters[0];
                            if (_target != null)
                                BulletsManager.CreateBullet(bi2, _side, Vector2.Normalize(_target.transform.Position - _entity.transform.Position), _entity, (GameScreen)Neon.world, _fromEnemy);
                            break;

                        case SpecialEffect.Invincible:
                            entity.hitbox.SwitchType(HitboxType.Invincible, (float)ae.Parameters[0]);
                            break;
                    }
                }
            }

            
        }

        public void CancelAttack()
        {
            for (int i = _hitboxes.Count - 1; i >= 0; i--)
            {
                _hitboxes[i].Remove();
            }
            this._hitboxes.Clear();
            this.CooldownFinished = true;
            this.CooldownStarted = true;
            this.DurationStarted = true;
            this.DurationFinished =true;
            this.DelayStarted = true;
            this.DelayFinished = true;
            this.AirLocked = false;
            this.AirLockFinished = true;
        }
    }
}
