using Microsoft.Xna.Framework;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    public class Bullet : Component
    {
        public bool EnemyBullet = false;

        public Entity launcher;

        private MovePattern _movementStyle;

        public MovePattern MovementStyle
        {
            get { return _movementStyle; }
            set { _movementStyle = value; }
        }

        private float _speed;

        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        private Vector2 _direction;

        public Vector2 Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        private float _lifeTime;
        private float _initialLifeTime;

        public float LifeTime
        {
            get { return _lifeTime; }
            set { _lifeTime = value; }
        }

        private float _damageOnHit;

        public float DamageOnHit
        {
            get { return _damageOnHit; }
            set { _damageOnHit = value; }
        }

        private float _stunLock;

        public float StunLock
        {
            get { return _stunLock; }
            set { _stunLock = value; }
        }

       
        private List<AttackEffect> _onHitSpecialEffects;

        public List<AttackEffect> OnHitSpecialEffects
        {
            get { return _onHitSpecialEffects; }
            set { _onHitSpecialEffects = value; }
        }

        private SpriteSheetInfo _livingSpriteSheetInfo;

        public SpriteSheetInfo LivingSpriteSheetInfo
        {
            get { return _livingSpriteSheetInfo; }
            set { _livingSpriteSheetInfo = value; }
        }

        private SpriteSheetInfo _onHitSpriteSheetInfo;

        public SpriteSheetInfo OnHitSpriteSheetInfo
        {
            get { return _onHitSpriteSheetInfo; }
            set { _onHitSpriteSheetInfo = value; }
        }

        public Bullet(Entity entity)
            :base(entity, "Bullet")
        {
        }

        public override void Init()
        {
            _initialLifeTime = LifeTime;
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            entity.spritesheets.RotationOffset = (float)Math.Atan2(Direction.Y, Direction.X);
            if (entity.spritesheets.RotationOffset < -Math.PI / 2)
                entity.spritesheets.ChangeSide(Side.Left);
            else
                entity.spritesheets.ChangeSide(Side.Right);
            if (entity.spritesheets.CurrentSide == Side.Left)
            {
                entity.spritesheets.RotationOffset += (float)Math.PI;
            }

            if (_lifeTime > 0f)
            {
                foreach (Hitbox hb in entity.containerWorld.Hitboxes)
                {
                    if (hb != launcher.hitboxes[0] && hb != entity.hitboxes[0] && hb.Type != HitboxType.Hit && hb.Type != HitboxType.Bullet && hb.Type != HitboxType.Invincible && hb.Type != HitboxType.None)
                    {
                        if (hb.hitboxRectangle.Intersects(entity.hitboxes[0].hitboxRectangle))
                        {
                            if (hb.Type == HitboxType.Solid)
                            {
                                LifeTime = 0f;
                                entity.spritesheets.ChangeAnimation("hit", true, 0, true, true, false);
                                return;
                            }

                            if (EnemyBullet)
                            {
                                if (hb.Type == HitboxType.Main)
                                {
                                    Avatar avatar = hb.entity.GetComponent<Avatar>();
                                    if (avatar != null)
                                    {
                                       if(avatar.TakeDamage(this))
                                       {
                                            LifeTime = 0f;
                                            entity.spritesheets.ChangeAnimation("hit", true, 0, true, true, false);
                                            return;
                                       }
                                       else
                                       {
                                            launcher = avatar.entity;
                                            EnemyBullet = false;
                                            LifeTime = _initialLifeTime;
                                            Direction = -Direction;
                                        }                              
                                    }
                                }
                            }
                            else
                            {
                                if (hb.Type == HitboxType.Main)
                                {
                                    Enemy enemy = hb.entity.GetComponent<Enemy>();
                                    if (enemy != null)
                                    {
                                        enemy.ChangeHealthPoints(DamageOnHit, launcher);
                                        LifeTime = 0f;
                                        entity.spritesheets.ChangeAnimation("hit", true, 0, true, true, false);
                                        return;
                                    }
                                }
                            }
                        }                                 
                    }
                }

                switch (MovementStyle)
                {
                    case MovePattern.Linear:
                        entity.transform.Position += (float)gameTime.ElapsedGameTime.TotalSeconds * Direction * Speed;
                        break;
                }
                _lifeTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                if (entity.spritesheets.CurrentSpritesheetName != "hit")
                    entity.spritesheets.ChangeAnimation("hit", true, 0, true, true, false);
                else
                {
                    if (entity.spritesheets.IsFinished())
                    {
                        BulletsManager.DestroyBullet(this.entity);
                    }
                }           
            }
            base.Update(gameTime);
        }
    }
}
