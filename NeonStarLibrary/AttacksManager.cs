using Microsoft.Xna.Framework;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace NeonStarLibrary
{
    public enum SpecialEffect
    {
        Impulse,
        PositionalPulse,
        DamageOverTime,
        PercentageDamageBoost,
        StartAttack,
        ShootBullet,
        ShootBulletAtTarget,
        Invincible,
        EffectAnimation,
        MoveWhileAttacking,
    }

    public enum AttackType
    {
        MeleeLight,
        MeleeSpecial,
        Range,
        Special
    }

    public class AttackInfo
    {
        public string Name;
        public AttackType Type = AttackType.MeleeLight;
        public List<Rectangle> Hitboxes = new List<Rectangle>();
        public float DamageOnHit = 1.0f;
        public float Delay = 0.0f;
        public float Cooldown = 1.0f;
        public float LocalCooldown = 0.0f;
        public float Duration = 1.0f;
        public float AirLock = 1.0f;
        public float TargetAirLock = 1.0f;
        public float AirFactor = 1.0f;
        public float StunLock = 0.0f;
        public bool AirOnly = false;
        public bool CancelOnGround = false;
        public bool OnlyOnceInAir = false;
        public List<AttackEffect> SpecialEffects = new List<AttackEffect>();
        public List<AttackEffect> OnHitSpecialEffects = new List<AttackEffect>();
        public List<AttackEffect> OnGroundCancelSpecialEffects = new List<AttackEffect>();
    }

    static public class AttacksManager
    {
        static public List<AttackInfo> _attacksInformation = new List<AttackInfo>();

        static public void LoadAttacks()
        {
            _attacksInformation.Clear();

            XDocument document = XDocument.Load(@"../Data/Config/Attacks.xml");
            XElement attacks = document.Element("XnaContent").Element("Attacks");

            foreach (XElement attack in attacks.Elements("Attack"))
            {
                AttackInfo ai = new AttackInfo();
                ai.Name = attack.Attribute("Name").Value.ToString();
                ai.Type = (AttackType)Enum.Parse(typeof(AttackType), attack.Attribute("Type").Value);
                ai.Hitboxes = new List<Rectangle>();

                foreach (XElement hitbox in attack.Element("Hitboxes").Elements("Hitbox"))
                {
                    ai.Hitboxes.Add(new Rectangle(int.Parse(hitbox.Attribute("OffsetX").Value), int.Parse(hitbox.Attribute("OffsetY").Value), 
                        int.Parse(hitbox.Attribute("Width").Value), int.Parse(hitbox.Attribute("Height").Value)));
                }

                ai.AirOnly = bool.Parse(attack.Element("AirOnly").Value);
                ai.CancelOnGround = bool.Parse(attack.Element("GroundCancel").Value);
                ai.OnlyOnceInAir = bool.Parse(attack.Element("OnlyOnceInAir").Value);
                ai.DamageOnHit = float.Parse(attack.Element("DamageOnHit").Value, CultureInfo.InvariantCulture);
                ai.Delay = float.Parse(attack.Element("Delay").Value, CultureInfo.InvariantCulture);
                ai.Cooldown = float.Parse(attack.Element("Cooldown").Value, CultureInfo.InvariantCulture);
                ai.LocalCooldown = float.Parse(attack.Element("LocalCooldown").Value, CultureInfo.InvariantCulture);
                ai.Duration = float.Parse(attack.Element("Duration").Value, CultureInfo.InvariantCulture);
                ai.AirLock = float.Parse(attack.Element("AirLock").Value, CultureInfo.InvariantCulture);
                ai.TargetAirLock = float.Parse(attack.Element("TargetAirLock").Value, CultureInfo.InvariantCulture);
                ai.AirFactor = float.Parse(attack.Element("AirFactor").Value, CultureInfo.InvariantCulture);
                ai.StunLock = float.Parse(attack.Element("StunLock").Value, CultureInfo.InvariantCulture);
                ai.SpecialEffects = new List<AttackEffect>();
                ai.OnGroundCancelSpecialEffects = new List<AttackEffect>();

                foreach (XElement specialEffect in attack.Element("SpecialEffects").Elements("Effect"))
                {
                    SpecialEffect se = (SpecialEffect)Enum.Parse(typeof(SpecialEffect), specialEffect.Attribute("Type").Value);

                    switch(se)
                    {
                        case SpecialEffect.Impulse:
                            Vector2 impulseForce = Neon.utils.ParseVector2(specialEffect.Element("Parameter").Attribute("Value").Value);
                            bool impulseBool = bool.Parse(specialEffect.Element("SecondParameter").Attribute("Value").Value);
                            ai.SpecialEffects.Add(new AttackEffect(se, new object[] { impulseForce, impulseBool }));
                            break;

                        case SpecialEffect.StartAttack:
                            string attackToLaunch = specialEffect.Element("Parameter").Attribute("Value").Value;
                            ai.SpecialEffects.Add(new AttackEffect(se, new object[] { attackToLaunch }));
                            break;

                        case SpecialEffect.PositionalPulse:
                            Vector2 pulseForce = Neon.utils.ParseVector2(specialEffect.Element("Parameter").Attribute("Value").Value);
                            ai.SpecialEffects.Add(new AttackEffect(se, new object[] { pulseForce }));
                            break;

                        case SpecialEffect.ShootBullet:
                            BulletInfo bi = BulletsManager.GetBulletInfo(specialEffect.Element("Parameter").Attribute("Value").Value);
                            ai.SpecialEffects.Add(new AttackEffect(se, new object[] { bi }));
                            break;

                        case SpecialEffect.ShootBulletAtTarget:
                            BulletInfo bi2 = BulletsManager.GetBulletInfo(specialEffect.Element("Parameter").Attribute("Value").Value);
                            ai.SpecialEffects.Add(new AttackEffect(se, new object[] { bi2 }));
                            break;
                            
                        case SpecialEffect.Invincible:
                            ai.SpecialEffects.Add(new AttackEffect(se, new object[] { float.Parse(specialEffect.Element("Parameter").Attribute("Value").Value, CultureInfo.InvariantCulture) }));
                            break;

                        case SpecialEffect.EffectAnimation:
                            SpriteSheetInfo ssi = AssetManager.GetSpriteSheet(specialEffect.Element("Parameter").Attribute("Value").Value);
                            float rotation = float.Parse(specialEffect.Element("SecondParameter").Attribute("Value").Value, CultureInfo.InvariantCulture);
                            Vector2 offset = Neon.utils.ParseVector2(specialEffect.Element("ThirdParameter").Attribute("Value").Value);
                            ai.SpecialEffects.Add(new AttackEffect(se, new object[] { ssi, rotation, offset }));
                            break;

                        case SpecialEffect.MoveWhileAttacking:
                            ai.SpecialEffects.Add(new AttackEffect(se, new object[] { float.Parse(specialEffect.Element("Parameter").Attribute("Value").Value, CultureInfo.InvariantCulture) }));
                            break;

                        case SpecialEffect.PercentageDamageBoost:
                            ai.SpecialEffects.Add(new AttackEffect(se, new object[] { float.Parse(specialEffect.Element("Parameter").Attribute("Value").Value, CultureInfo.InvariantCulture), float.Parse(specialEffect.Element("SecondParameter").Attribute("Value").Value, CultureInfo.InvariantCulture) }));
                            break;
                    }         
                }

                ai.OnHitSpecialEffects = new List<AttackEffect>();

                foreach (XElement onHitSpecialEffect in attack.Element("OnHitSpecialEffects").Elements("Effect"))
                {
                    SpecialEffect se = (SpecialEffect)Enum.Parse(typeof(SpecialEffect), onHitSpecialEffect.Attribute("Type").Value);

                    switch(se)
                    {
                        case SpecialEffect.Impulse:
                            Vector2 impulseForce = Neon.utils.ParseVector2(onHitSpecialEffect.Element("Parameter").Attribute("Value").Value);
                            bool impulseBool = bool.Parse(onHitSpecialEffect.Element("SecondParameter").Attribute("Value").Value);
                            ai.OnHitSpecialEffects.Add(new AttackEffect(se, new object[] { impulseForce, impulseBool }));
                            break;

                        case SpecialEffect.StartAttack:
                            string attackToLaunch = onHitSpecialEffect.Element("Parameter").Attribute("Value").Value;
                            ai.OnHitSpecialEffects.Add(new AttackEffect(se, new object[] { attackToLaunch }));
                            break;

                        case SpecialEffect.PositionalPulse:
                            Vector2 pulseForce = Neon.utils.ParseVector2(onHitSpecialEffect.Element("Parameter").Attribute("Value").Value);
                            ai.OnHitSpecialEffects.Add(new AttackEffect(se, new object[] { pulseForce }));
                            break;

                        case SpecialEffect.ShootBullet:
                            BulletInfo bi = BulletsManager.GetBulletInfo(onHitSpecialEffect.Element("Parameter").Attribute("Value").Value);
                            ai.OnHitSpecialEffects.Add(new AttackEffect(se, new object[] { bi }));
                            break;

                        case SpecialEffect.ShootBulletAtTarget:
                            BulletInfo bi2 = BulletsManager.GetBulletInfo(onHitSpecialEffect.Element("Parameter").Attribute("Value").Value);
                            ai.OnHitSpecialEffects.Add(new AttackEffect(se, new object[] { bi2 }));
                            break;

                        case SpecialEffect.Invincible:
                            ai.OnHitSpecialEffects.Add(new AttackEffect(se, new object[] { onHitSpecialEffect.Element("Parameter").Attribute("Value").Value }));
                            break;

                        case SpecialEffect.EffectAnimation:
                            SpriteSheetInfo ssi = AssetManager.GetSpriteSheet(onHitSpecialEffect.Element("Parameter").Attribute("Value").Value);
                            float rotation = float.Parse(onHitSpecialEffect.Element("SecondParameter").Attribute("Value").Value, CultureInfo.InvariantCulture);
                            Vector2 offset = Neon.utils.ParseVector2(onHitSpecialEffect.Element("ThirdParameter").Attribute("Value").Value);
                            ai.OnHitSpecialEffects.Add(new AttackEffect(se, new object[] { ssi, rotation, offset }));
                            break;

                        case SpecialEffect.MoveWhileAttacking:
                            ai.OnHitSpecialEffects.Add(new AttackEffect(se, new object[] { float.Parse(onHitSpecialEffect.Element("Parameter").Attribute("Value").Value) }));
                            break;

                        case SpecialEffect.PercentageDamageBoost:
                            ai.OnHitSpecialEffects.Add(new AttackEffect(se, new object[] { float.Parse(onHitSpecialEffect.Element("Parameter").Attribute("Value").Value, CultureInfo.InvariantCulture), float.Parse(onHitSpecialEffect.Element("SecondParameter").Attribute("Value").Value, CultureInfo.InvariantCulture)}));
                            break;
                    }         
                }

                ai.OnGroundCancelSpecialEffects = new List<AttackEffect>();

                foreach (XElement onGroundCancelSpecialEffect in attack.Element("OnGroundCancelSpecialEffects").Elements("Effect"))
                {
                    SpecialEffect se = (SpecialEffect)Enum.Parse(typeof(SpecialEffect), onGroundCancelSpecialEffect.Attribute("Type").Value);

                    switch (se)
                    {
                        case SpecialEffect.Impulse:
                            Vector2 impulseForce = Neon.utils.ParseVector2(onGroundCancelSpecialEffect.Element("Parameter").Attribute("Value").Value);
                            bool impulseBool = bool.Parse(onGroundCancelSpecialEffect.Element("ParameterTwo").Attribute("Value").Value);
                            ai.OnGroundCancelSpecialEffects.Add(new AttackEffect(se, new object[] { impulseForce, impulseBool }));
                            break;

                        case SpecialEffect.StartAttack:
                            string attackToLaunch = onGroundCancelSpecialEffect.Element("Parameter").Attribute("Value").Value;
                            ai.OnGroundCancelSpecialEffects.Add(new AttackEffect(se, new object[] { attackToLaunch }));
                            break;

                        case SpecialEffect.PositionalPulse:
                            Vector2 pulseForce = Neon.utils.ParseVector2(onGroundCancelSpecialEffect.Element("Parameter").Attribute("Value").Value);
                            ai.OnGroundCancelSpecialEffects.Add(new AttackEffect(se, new object[] { pulseForce }));
                            break;

                        case SpecialEffect.ShootBullet:
                            BulletInfo bi = BulletsManager.GetBulletInfo(onGroundCancelSpecialEffect.Element("Parameter").Attribute("Value").Value);
                            ai.OnGroundCancelSpecialEffects.Add(new AttackEffect(se, new object[] { bi }));
                            break;

                        case SpecialEffect.ShootBulletAtTarget:
                            BulletInfo bi2 = BulletsManager.GetBulletInfo(onGroundCancelSpecialEffect.Element("Parameter").Attribute("Value").Value);
                            ai.OnGroundCancelSpecialEffects.Add(new AttackEffect(se, new object[] { bi2 }));
                            break;

                        case SpecialEffect.Invincible:
                            ai.OnGroundCancelSpecialEffects.Add(new AttackEffect(se, new object[] { onGroundCancelSpecialEffect.Element("Parameter").Attribute("Value").Value }));
                            break;

                        case SpecialEffect.EffectAnimation:
                            SpriteSheetInfo ssi = AssetManager.GetSpriteSheet(onGroundCancelSpecialEffect.Element("Parameter").Attribute("Value").Value);
                            float rotation = float.Parse(onGroundCancelSpecialEffect.Element("SecondParameter").Attribute("Value").Value, CultureInfo.InvariantCulture);
                            Vector2 offset = Neon.utils.ParseVector2(onGroundCancelSpecialEffect.Element("ThirdParameter").Attribute("Value").Value);
                            ai.OnGroundCancelSpecialEffects.Add(new AttackEffect(se, new object[] { ssi, rotation, offset }));
                            break;

                        case SpecialEffect.MoveWhileAttacking:
                            ai.OnGroundCancelSpecialEffects.Add(new AttackEffect(se, new object[] { float.Parse(onGroundCancelSpecialEffect.Element("Parameter").Attribute("Value").Value) }));
                            break;

                        case SpecialEffect.PercentageDamageBoost:
                            ai.OnGroundCancelSpecialEffects.Add(new AttackEffect(se, new object[] { float.Parse(onGroundCancelSpecialEffect.Element("Parameter").Attribute("Value").Value, CultureInfo.InvariantCulture), float.Parse(onGroundCancelSpecialEffect.Element("SecondParameter").Attribute("Value").Value, CultureInfo.InvariantCulture)}));
                            break;
                    }
                }

                _attacksInformation.Add(ai);
            }
        }

        static public Attack GetAttack(string name, Side side, Entity launcher, Entity target = null , bool isEnemy = false)
        {
            if (name == "" || name == null)
                return null;
            AttackInfo[] aiList = _attacksInformation.Where(ai => ai.Name == name).ToArray();
            
            if (aiList.Length == 0)
                return null;

            Attack attack = new Attack(aiList.First(), side, launcher, target, isEnemy);

            return attack;
        }

        static public Attack StartFreeAttack(string name, Side side, Vector2 Position)
        {
            AttackInfo[] aiList = _attacksInformation.Where(ai => ai.Name == name).ToArray();

            if (aiList.Length == 0)
                return null;
            Attack attack = new Attack(aiList.First(), side, Position);
            (Neon.world as GameScreen).FreeAttacks.Add(attack);

            return attack;
        }

        static public AttackInfo GetAttackInfo(string name)
        {
            return _attacksInformation.First(ai => ai.Name == name);
        }   
    }
}
