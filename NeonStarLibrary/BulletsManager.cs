﻿using Microsoft.Xna.Framework;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace NeonStarLibrary
{
    public class BulletInfo
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public SpriteSheetInfo LivingSpriteSheet;
        public SpriteSheetInfo OnHitSpriteSheet;

        public Rectangle HitboxInfo;

        public MovePattern MovementStyle;

        public Vector2 Direction;
        public float Speed;

        public float LifeTime;

        public float DamageOnHit;

        public List<AttackEffect> OnHitSpecialEffects = new List<AttackEffect>();

    }

    static public class BulletsManager
    {
        private static GameScreen _world;
        static public List<BulletInfo> _bulletsInformation = new List<BulletInfo>();

        static public void LoadBullets()
        {
            _bulletsInformation.Clear();

            XDocument document = XDocument.Load(@"../Data/Config/Bullets.xml");
            XElement bullets = document.Element("XnaContent").Element("Bullets");

            foreach(XElement bullet in bullets.Elements("Bullet"))
            {
                BulletInfo bi = new BulletInfo();
                bi.Name = bullet.Attribute("Name").Value.ToString();
                bi.HitboxInfo = new Rectangle();
                bi.HitboxInfo.X = int.Parse(bullet.Element("Hitbox").Attribute("OffsetX").Value);
                bi.HitboxInfo.Y = int.Parse(bullet.Element("Hitbox").Attribute("OffsetY").Value);
                bi.HitboxInfo.Width = int.Parse(bullet.Element("Hitbox").Attribute("Width").Value);
                bi.HitboxInfo.Height = int.Parse(bullet.Element("Hitbox").Attribute("Height").Value);

                bi.LivingSpriteSheet = AssetManager.GetSpriteSheet(bullet.Element("LivingSpriteSheet").Value != "" ? bullet.Element("LivingSpriteSheet").Value : null);
                bi.OnHitSpriteSheet = AssetManager.GetSpriteSheet(bullet.Element("OnHitSpriteSheet").Value != "" ? bullet.Element("OnHitSpriteSheet").Value : null);

                bi.MovementStyle = (MovePattern)Enum.Parse(typeof(MovePattern), bullet.Element("MovePattern").Value);
                bi.Direction = Neon.utils.ParseVector2(bullet.Element("Direction").Value);

                bi.Speed = float.Parse(bullet.Element("Speed").Value, CultureInfo.InvariantCulture);
                bi.LifeTime = float.Parse(bullet.Element("LifeTime").Value, CultureInfo.InvariantCulture);
                bi.DamageOnHit = float.Parse(bullet.Element("DamageOnHit").Value, CultureInfo.InvariantCulture);

                bi.OnHitSpecialEffects = new List<AttackEffect>();

                foreach (XElement onHitSpecialEffect in bullet.Element("OnHitSpecialEffects").Elements("Effect"))
                {
                    SpecialEffect se = (SpecialEffect)Enum.Parse(typeof(SpecialEffect), onHitSpecialEffect.Attribute("Type").Value);

                    switch (se)
                    {
                        case SpecialEffect.Impulse:
                            Vector2 impulseForce = Neon.utils.ParseVector2(onHitSpecialEffect.Element("Parameter").Attribute("Value").Value);
                            bi.OnHitSpecialEffects.Add(new AttackEffect(se, impulseForce));
                            break;

                        case SpecialEffect.StartAttack:
                            string attackToLaunch = onHitSpecialEffect.Element("Parameter").Attribute("Value").Value;
                            bi.OnHitSpecialEffects.Add(new AttackEffect(se, attackToLaunch));
                            break;

                        case SpecialEffect.PositionalPulse:
                            Vector2 pulseForce = Neon.utils.ParseVector2(onHitSpecialEffect.Element("Parameter").Attribute("Value").Value);
                            bi.OnHitSpecialEffects.Add(new AttackEffect(se, pulseForce));
                            break;
                    }
                }

                _bulletsInformation.Add(bi);
            }
        }

        static public BulletInfo GetBulletInfo(string name)
        {
            if (name == "")
                return null;
            return _bulletsInformation.First(bi => bi.Name == name);
        }

        static public void CreateBullet(BulletInfo bulletInfo, Side side, Vector2 newDirection, Entity shooter, GameScreen world, bool isEnemy = true)
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
            hitbox.OffsetX = side == Side.Right ? bulletInfo.HitboxInfo.X : -bulletInfo.HitboxInfo.X;
            hitbox.OffsetY = side == Side.Right ? bulletInfo.HitboxInfo.Y : -bulletInfo.HitboxInfo.Y;
            hitbox.Center = newBullet.transform.Position;

            bullet.LifeTime = bulletInfo.LifeTime;

            bullet.Speed = bulletInfo.Speed;
            bullet.Direction = newDirection != Vector2.Zero ? newDirection : new Vector2(side == Side.Right ? bulletInfo.Direction.X : -bulletInfo.Direction.X, bulletInfo.Direction.Y);
            bullet.DamageOnHit = bulletInfo.DamageOnHit;
            bullet.LivingSpriteSheetInfo = bulletInfo.LivingSpriteSheet;
            bullet.OnHitSpriteSheetInfo = bulletInfo.OnHitSpriteSheet;
            bullet.EnemyBullet = isEnemy;
            bullet.OnHitSpecialEffects = new List<AttackEffect>();

            foreach (AttackEffect ae in bulletInfo.OnHitSpecialEffects)
            {
                bullet.OnHitSpecialEffects.Add(ae);
            }

            newBullet.transform.Position = shooter.transform.Position;
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