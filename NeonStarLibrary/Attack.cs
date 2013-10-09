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

        private float _damageOnHit = 1.0f;
        public float DamageOnHit
        {
            get { return _damageOnHit; }
            set { _damageOnHit = value; }
        }

        List<Hitbox> _hitboxes;

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

        private Dictionary<SpecialEffect, object> _specialEffects  = new Dictionary<SpecialEffect, object>();
        private Dictionary<SpecialEffect, object> _onHitSpecialEffects  = new Dictionary<SpecialEffect, object>();

        private Entity _entity;

        public Attack()
        {
        }

        public Attack(AttackInfo attackInfo, Side side, Entity launcher)
        {
            _hitboxes = new List<Hitbox>();
            this._side = side;
            this._entity = launcher;
            this.Name = attackInfo.Name;
            this.DamageOnHit = attackInfo.DamageOnHit;
            this.Cooldown = attackInfo.Cooldown;
            foreach (Rectangle hitbox in attackInfo.Hitboxes)
            {
                Hitbox hb = Neon.world.HitboxPool.GetAvailableItem();
                hb.Center = launcher.transform.Position;
                hb.Width = hitbox.Width;
                hb.Height = hitbox.Height;
                hb.OffsetX = this._side == Side.Right ? hitbox.X : -hitbox.X;
                hb.OffsetY = hitbox.Y;
                hb.Type = HitboxType.Hit;
                launcher.AddComponent(hb);
                hb.PoolInit(launcher);
                _hitboxes.Add(hb);
            }
            this._specialEffects = attackInfo.SpecialEffects;
            this._onHitSpecialEffects = attackInfo.OnHitSpecialEffects;
            this._active = true;
        }

        public void Init()
        {
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
                            if (hb.hitboxRectangle.Intersects(hitbox.hitboxRectangle))
                            {
                                Effect(hb.entity);
                                hitbox.Remove();
                                _hitboxes.Remove(hitbox);
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
        }

        private void Effect(Entity entity)
        {
            Enemy enemy = entity.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.ChangeHealthPoints(_damageOnHit);

                foreach(KeyValuePair<SpecialEffect, object> kvp in _onHitSpecialEffects)
                {
                    switch(kvp.Key)
                    {
                        case SpecialEffect.Impulse:
                            Vector2 impulseForce = (Vector2)kvp.Value;
                            entity.rigidbody.body.ApplyLinearImpulse(new Vector2(_side == Side.Right ? impulseForce.X : -impulseForce.X, impulseForce.Y));
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
            this._active = false;
        }
    }
}
