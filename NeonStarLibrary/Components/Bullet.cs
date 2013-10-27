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
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (_lifeTime > 0f)
            {
                switch(MovementStyle)
                {
                    case MovePattern.Linear:
                        entity.transform.Position += (float)gameTime.ElapsedGameTime.TotalSeconds * Direction * Speed;
                        break;

                }
                _lifeTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                BulletsManager.DestroyBullet(this.entity);
            }
            base.Update(gameTime);
        }
    }
}
