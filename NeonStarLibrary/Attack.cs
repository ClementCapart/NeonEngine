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
        public object Parameters;

        public AttackEffect(SpecialEffect specialEffect, object Parameters)
        {
            this.specialEffect = specialEffect;
            this.Parameters = Parameters;
        }
    }

    public class Attack
    {
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

        private List<AttackEffect> _specialEffects = new List<AttackEffect>();
        private List<AttackEffect> _onHitSpecialEffects  = new List<AttackEffect>();

        public bool Canceled = false;
        private Entity _entity;
        public AttackInfo AttackInfo;
        private MeleeFight _meleeFight;

        public Attack()
        {
        }

        public Attack(AttackInfo attackInfo, Side side, Entity launcher)
        {
            
            AttackInfo = attackInfo;
            _hitboxes = new List<Hitbox>();
            this._side = side;
            this._entity = launcher;
            this.Name = attackInfo.Name;
            this.Type = attackInfo.Type;
            this.Delay = attackInfo.Delay;
            this.DamageOnHit = attackInfo.DamageOnHit;
            this.Cooldown = attackInfo.Cooldown;
            this.Duration = attackInfo.Duration;
            this.AirLock = attackInfo.AirLock;
            this.TargetAirLock = attackInfo.TargetAirLock;
            this.AirOnly = attackInfo.AirOnly;
            this.CancelOnGround = attackInfo.CancelOnGround;
            this.OnlyOnceInAir = attackInfo.OnlyOnceInAir;

            foreach (AttackEffect ae in attackInfo.OnHitSpecialEffects)
            {
                this._onHitSpecialEffects.Add(ae);
            }

            DelayStarted = true;

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

            if (this.Delay <= 0.0f)
                Init();
            if (AirOnly)
                if (this._entity.rigidbody.isGrounded && _meleeFight != null)
                {
                    _meleeFight.CurrentComboHit = _meleeFight.LastComboHit;
                    this.CancelAttack();
                    Canceled = true;
                    return;
                }

            if (AirLock <= 0.0f || _entity.rigidbody.isGrounded)
                AirLockFinished = true;
        }

        public void Init()
        {
            foreach (Rectangle hitbox in AttackInfo.Hitboxes)
            {
                Hitbox hb = Neon.world.HitboxPool.GetAvailableItem();
                hb.Center = _entity.transform.Position;
                hb.Width = hitbox.Width;
                hb.Height = hitbox.Height;
                hb.OffsetX = this._side == Side.Right ? hitbox.X : -hitbox.X;
                hb.OffsetY = hitbox.Y;
                hb.Type = HitboxType.Hit;
                _entity.AddComponent(hb);
                hb.PoolInit(_entity);
                _hitboxes.Add(hb);
            }

            if(_meleeFight.Debug)
                Console.WriteLine(AttackInfo.Name + " launched ! Current Combo -> " + _meleeFight.CurrentComboHit.ToString());

            foreach (AttackEffect ae in AttackInfo.SpecialEffects)
            {
                this._specialEffects.Add(ae);
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
                            Vector2 impulseForce = (Vector2)ae.Parameters;
                            _entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                            _entity.rigidbody.body.ApplyLinearImpulse(new Vector2(_side == Side.Right ? impulseForce.X : -impulseForce.X, impulseForce.Y));
                            break;

                        case SpecialEffect.Boost:
                            break;

                        case SpecialEffect.DamageOverTime:
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
                            if (hb.hitboxRectangle.Intersects(hitbox.hitboxRectangle) && hb.LastIntersects != hitbox)
                            {
                                Effect(hb.entity);
                                if (this.Type == AttackType.MeleeLight || this.Type == AttackType.MeleeSpecial)
                                {
                                    hb.LastIntersects = hitbox;
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
                    CancelAttack();
                }
            }

            if (!_entity.rigidbody.isGrounded)
            {
                if (!AirLocked && !AirLockFinished)
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
            else
            {
                AirLocked = false;
                AirLockFinished = true;
            }

            if (CancelOnGround && _entity.rigidbody.isGrounded)
            {
                for (int i = _hitboxes.Count - 1; i >= 0; i--)
                {
                    _hitboxes[i].Remove();
                }
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

        private void Effect(Entity entity)
        {
            Enemy enemy = entity.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.ChangeHealthPoints(_damageOnHit);
                if(!enemy.entity.rigidbody.isGrounded)
                    enemy.AirLock(TargetAirLock);
                foreach(AttackEffect ae in _onHitSpecialEffects)
                {
                    switch(ae.specialEffect)
                    {
                        case SpecialEffect.Impulse:
                            Vector2 impulseForce = (Vector2)ae.Parameters;                           
                            entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                            entity.rigidbody.GravityScale = entity.rigidbody.InitialGravityScale;
                            entity.rigidbody.body.ApplyLinearImpulse(new Vector2(_side == Side.Right ? impulseForce.X : -impulseForce.X, impulseForce.Y));
                            entity.rigidbody.body.GravityScale = entity.rigidbody.InitialGravityScale;
                            break;

                        case SpecialEffect.Boost:
                            break;

                        case SpecialEffect.DamageOverTime:
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
