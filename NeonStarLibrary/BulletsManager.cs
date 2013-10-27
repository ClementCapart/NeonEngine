using Microsoft.Xna.Framework;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    public class BulletInfo
    {
        public SpriteSheet LivingSpriteSheet;
        public SpriteSheet OnHitSpriteSheet;

        public Rectangle HitboxInfo;

        public MovePattern MovementStyle;

        public Vector2 Direction;
        public float Speed;

        public float LifeTime;

        public float DamageOnHit;

    }

    static public class BulletsManager
    {
        private static GameScreen _world;

        static public void CreateBullet(BulletInfo bulletInfo, GameScreen world)
        {
            _world = world;

            Entity newBullet = _world.BulletsPool.GetAvailableItem();
            Hitbox hitbox;
            Bullet bullet;

            if(newBullet.Components.Count > 1)
            {
                hitbox = newBullet.GetComponent<Hitbox>();
                bullet = newBullet.GetComponent<Bullet>();              
            }
            else
            {
                hitbox = _world.HitboxPool.GetAvailableItem();
                newBullet.AddComponent(hitbox);
                bullet = new Bullet(newBullet);
                newBullet.AddComponent(bullet);
            }

            newBullet.Name = "Bullet";
            
            hitbox.PoolInit(newBullet);
            hitbox.Type = HitboxType.Bullet;
            hitbox.Width = bulletInfo.HitboxInfo.Width;
            hitbox.Height = bulletInfo.HitboxInfo.Height;
            hitbox.OffsetX = bulletInfo.HitboxInfo.X;
            hitbox.OffsetY = bulletInfo.HitboxInfo.Y;
            hitbox.Center = newBullet.transform.Position;

            bullet.LifeTime = 2.0f;

            bullet.Speed = 200f;
            bullet.Direction = new Vector2(1, 0);


            _world.Bullets.Add(newBullet);
        }

        static public void DestroyBullet(Entity entity)
        {
            _world.Bullets.Remove(entity);
            BulletsManager._world.Hitboxes.Remove(entity.hitbox);
            _world.BulletsPool.FlagAvailableItem(entity);
        }
    }
}
