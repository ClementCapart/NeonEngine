using Microsoft.Xna.Framework;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
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

        private bool _active;
        public bool Active
        {
            get { return _active; }
            set { _active = value; }
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

        private bool _activated = false;

        public bool Activated
        {
            get { return _activated; }
            set { _activated = value; }
        }

        private Dictionary<SpecialEffect, object> _specialEffects  = new Dictionary<SpecialEffect, object>();
        private Dictionary<SpecialEffect, object> _onHitSpecialEffects  = new Dictionary<SpecialEffect, object>();

        private Entity _entity;
        private AttackInfo _attackInfo;

        public Attack()
        {
        }

        public Attack(AttackInfo attackInfo, Side side, Entity launcher)
        {
            _attackInfo = attackInfo;
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
            
            foreach (KeyValuePair<SpecialEffect, object> kvp in attackInfo.OnHitSpecialEffects)
            {
                this._onHitSpecialEffects.Add(kvp.Key, kvp.Value);
            }

            if (this.Delay <= 0.0f)
                Init();
        }

        public void Init()
        {
            foreach (Rectangle hitbox in _attackInfo.Hitboxes)
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

            foreach (KeyValuePair<SpecialEffect, object> kvp in _attackInfo.SpecialEffects)
            {
                this._specialEffects.Add(kvp.Key, kvp.Value);
            }
            this._activated = true;
            this._active = true;
        }

        public void Update(GameTime gameTime)
        {
            if (_active)
            {
                for(int i = _specialEffects.Count - 1; i >= 0; i--)
                {
                    KeyValuePair<SpecialEffect, object> kvp = _specialEffects.ElementAt(i);
                    switch (kvp.Key)
                    {
                        case SpecialEffect.Impulse:
                            Vector2 impulseForce = (Vector2)kvp.Value;
                            _entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                            _entity.rigidbody.body.ApplyLinearImpulse(new Vector2(_side == Side.Right ? impulseForce.X : -impulseForce.X, impulseForce.Y));
                            break;

                        case SpecialEffect.Boost:
                            break;

                        case SpecialEffect.DamageOverTime:
                            break;
                    }
                    _specialEffects.Remove(kvp.Key);
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
                                if (this.Type == AttackType.Melee)
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
                    CancelAttack();
                }
            }
            else if (!_activated && this.Delay > 0.0f)
            {
                Delay -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (!_activated)
            {
                Init();
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
                foreach(KeyValuePair<SpecialEffect, object> kvp in _onHitSpecialEffects)
                {
                    switch(kvp.Key)
                    {
                        case SpecialEffect.Impulse:
                            Vector2 impulseForce = (Vector2)kvp.Value;                           
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
            this.Active = false;
        }
    }
}
