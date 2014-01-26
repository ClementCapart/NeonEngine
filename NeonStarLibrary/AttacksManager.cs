using Microsoft.Xna.Framework;
using NeonEngine;
using NeonStarLibrary.Components.Avatar;
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
        public AttackType Type = AttackType.MeleeSpecial;
        public Element AttackElement = Element.Neutral;
        public List<Rectangle> Hitboxes = new List<Rectangle>();
        public float DamageOnHit = 1.0f;
        public float Delay = 0.0f;
        public float Cooldown = 1.0f;
        public float LocalCooldown = 0.0f;
        public float Duration = 1.0f;
        public float AirLock = 0.0f;
        public float TargetAirLock = 0.0f;
        public float AirFactor = 1.0f;
        public float StunLock = 0.0f;
        public float MultiHitDelay = 0.0f;
        public bool AirOnly = false;
        public bool CancelOnGround = false;
        public bool OnlyOnceInAir = false;
        public List<AttackEffect> OnDelaySpecialEffects = new List<AttackEffect>();
        public List<AttackEffect> OnDurationSpecialEffects = new List<AttackEffect>();
        public List<AttackEffect> OnHitSpecialEffects = new List<AttackEffect>();
        public List<AttackEffect> OnGroundCancelSpecialEffects = new List<AttackEffect>();
    }

    static public class AttacksManager
    {
        static public List<AttackInfo> _attacksInformation = new List<AttackInfo>();

        static public void LoadAttacks()
        {
            _attacksInformation = new List<AttackInfo>();

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

                if (attack.Element("AttackElement") != null)
                    ai.AttackElement = (Element)Enum.Parse(typeof(Element), attack.Element("AttackElement").Value);
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
                ai.MultiHitDelay = float.Parse(attack.Element("MultiHitDelay").Value, CultureInfo.InvariantCulture);

                ai.OnDelaySpecialEffects = new List<AttackEffect>();

                foreach (XElement onDelaySpecialEffect in attack.Element("OnDelaySpecialEffects").Elements("Effect"))
                {
                    SpecialEffect se = (SpecialEffect)Enum.Parse(typeof(SpecialEffect), onDelaySpecialEffect.Attribute("Type").Value);

                    switch (se)
                    {
                        case SpecialEffect.Impulse:
                            Vector2 impulseForce = Neon.Utils.ParseVector2(onDelaySpecialEffect.Element("Parameter").Attribute("Value").Value);
                            bool impulseBool = bool.Parse(onDelaySpecialEffect.Element("SecondParameter").Attribute("Value").Value);
                            bool notInAir = bool.Parse(onDelaySpecialEffect.Element("ThirdParameter").Attribute("Value").Value);
                            ai.OnDelaySpecialEffects.Add(new AttackEffect(se, new object[] { impulseForce, impulseBool, notInAir }));
                            break;

                        case SpecialEffect.StartAttack:
                            string attackToLaunch = onDelaySpecialEffect.Element("Parameter").Attribute("Value").Value;
                            ai.OnDelaySpecialEffects.Add(new AttackEffect(se, new object[] { attackToLaunch }));
                            break;

                        case SpecialEffect.PositionalPulse:
                            Vector2 pulseForce = Neon.Utils.ParseVector2(onDelaySpecialEffect.Element("Parameter").Attribute("Value").Value);
                            ai.OnDelaySpecialEffects.Add(new AttackEffect(se, new object[] { pulseForce }));
                            break;

                        case SpecialEffect.ShootBullet:
                            BulletInfo bi = BulletsManager.GetBulletInfo(onDelaySpecialEffect.Element("Parameter").Attribute("Value").Value);
                            ai.OnDelaySpecialEffects.Add(new AttackEffect(se, new object[] { bi, Neon.Utils.ParseVector2(onDelaySpecialEffect.Element("SecondParameter").Attribute("Value").Value) }));
                            break;

                        case SpecialEffect.ShootBulletAtTarget:
                            BulletInfo bi2 = BulletsManager.GetBulletInfo(onDelaySpecialEffect.Element("Parameter").Attribute("Value").Value);
                            ai.OnDelaySpecialEffects.Add(new AttackEffect(se, new object[] { bi2, Neon.Utils.ParseVector2(onDelaySpecialEffect.Element("SecondParameter").Attribute("Value").Value) }));
                            break;

                        case SpecialEffect.Invincible:
                            ai.OnDelaySpecialEffects.Add(new AttackEffect(se, new object[] { float.Parse(onDelaySpecialEffect.Element("Parameter").Attribute("Value").Value, CultureInfo.InvariantCulture) }));
                            break;

                        case SpecialEffect.EffectAnimation:
                            SpriteSheetInfo ssi = AssetManager.GetSpriteSheet(onDelaySpecialEffect.Element("Parameter").Attribute("Value").Value);
                            float rotation = float.Parse(onDelaySpecialEffect.Element("SecondParameter").Attribute("Value").Value, CultureInfo.InvariantCulture);
                            Vector2 offset = Neon.Utils.ParseVector2(onDelaySpecialEffect.Element("ThirdParameter").Attribute("Value").Value);
                            ai.OnDelaySpecialEffects.Add(new AttackEffect(se, new object[] { ssi, rotation, offset, bool.Parse(onDelaySpecialEffect.Element("FourthParameter").Attribute("Value").Value), float.Parse(onDelaySpecialEffect.Element("FifthParameter").Attribute("Value").Value, CultureInfo.InvariantCulture) }));
                            break;

                        case SpecialEffect.MoveWhileAttacking:
                            ai.OnDelaySpecialEffects.Add(new AttackEffect(se, new object[] { float.Parse(onDelaySpecialEffect.Element("Parameter").Attribute("Value").Value, CultureInfo.InvariantCulture) }));
                            break;

                        case SpecialEffect.PercentageDamageBoost:
                            ai.OnDelaySpecialEffects.Add(new AttackEffect(se, new object[] { float.Parse(onDelaySpecialEffect.Element("Parameter").Attribute("Value").Value, CultureInfo.InvariantCulture), float.Parse(onDelaySpecialEffect.Element("SecondParameter").Attribute("Value").Value, CultureInfo.InvariantCulture) }));
                            break;

                        case SpecialEffect.DamageOverTime:
                            ai.OnDelaySpecialEffects.Add(new AttackEffect(se, new object[] { float.Parse(onDelaySpecialEffect.Element("Parameter").Attribute("Value").Value, CultureInfo.InvariantCulture), float.Parse(onDelaySpecialEffect.Element("SecondParameter").Attribute("Value").Value, CultureInfo.InvariantCulture), float.Parse(onDelaySpecialEffect.Element("ThirdParameter").Attribute("Value").Value, CultureInfo.InvariantCulture) }));
                            break;
                    }
                }

                ai.OnDurationSpecialEffects = new List<AttackEffect>();

                foreach (XElement onDurationSpecialEffect in attack.Element("OnDurationSpecialEffects").Elements("Effect"))
                {
                    SpecialEffect se = (SpecialEffect)Enum.Parse(typeof(SpecialEffect), onDurationSpecialEffect.Attribute("Type").Value);

                    switch(se)
                    {
                        case SpecialEffect.Impulse:
                            Vector2 impulseForce = Neon.Utils.ParseVector2(onDurationSpecialEffect.Element("Parameter").Attribute("Value").Value);
                            bool impulseBool = bool.Parse(onDurationSpecialEffect.Element("SecondParameter").Attribute("Value").Value);
                            bool notInAir = bool.Parse(onDurationSpecialEffect.Element("ThirdParameter").Attribute("Value").Value);
                            ai.OnDurationSpecialEffects.Add(new AttackEffect(se, new object[] { impulseForce, impulseBool, notInAir }));
                            break;

                        case SpecialEffect.StartAttack:
                            string attackToLaunch = onDurationSpecialEffect.Element("Parameter").Attribute("Value").Value;
                            ai.OnDurationSpecialEffects.Add(new AttackEffect(se, new object[] { attackToLaunch }));
                            break;

                        case SpecialEffect.PositionalPulse:
                            Vector2 pulseForce = Neon.Utils.ParseVector2(onDurationSpecialEffect.Element("Parameter").Attribute("Value").Value);
                            ai.OnDurationSpecialEffects.Add(new AttackEffect(se, new object[] { pulseForce }));
                            break;

                        case SpecialEffect.ShootBullet:
                            BulletInfo bi = BulletsManager.GetBulletInfo(onDurationSpecialEffect.Element("Parameter").Attribute("Value").Value);
                            ai.OnDurationSpecialEffects.Add(new AttackEffect(se, new object[] { bi, Neon.Utils.ParseVector2(onDurationSpecialEffect.Element("SecondParameter").Attribute("Value").Value) }));
                            break;

                        case SpecialEffect.ShootBulletAtTarget:
                            BulletInfo bi2 = BulletsManager.GetBulletInfo(onDurationSpecialEffect.Element("Parameter").Attribute("Value").Value);
                            ai.OnDurationSpecialEffects.Add(new AttackEffect(se, new object[] { bi2, Neon.Utils.ParseVector2(onDurationSpecialEffect.Element("SecondParameter").Attribute("Value").Value) }));
                            break;
                            
                        case SpecialEffect.Invincible:
                            ai.OnDurationSpecialEffects.Add(new AttackEffect(se, new object[] { float.Parse(onDurationSpecialEffect.Element("Parameter").Attribute("Value").Value, CultureInfo.InvariantCulture) }));
                            break;

                        case SpecialEffect.EffectAnimation:
                            SpriteSheetInfo ssi = AssetManager.GetSpriteSheet(onDurationSpecialEffect.Element("Parameter").Attribute("Value").Value);
                            float rotation = float.Parse(onDurationSpecialEffect.Element("SecondParameter").Attribute("Value").Value, CultureInfo.InvariantCulture);
                            Vector2 offset = Neon.Utils.ParseVector2(onDurationSpecialEffect.Element("ThirdParameter").Attribute("Value").Value);
                            ai.OnDurationSpecialEffects.Add(new AttackEffect(se, new object[] { ssi, rotation, offset, bool.Parse(onDurationSpecialEffect.Element("FourthParameter").Attribute("Value").Value), float.Parse(onDurationSpecialEffect.Element("FifthParameter").Attribute("Value").Value, CultureInfo.InvariantCulture) }));
                            break;

                        case SpecialEffect.MoveWhileAttacking:
                            ai.OnDurationSpecialEffects.Add(new AttackEffect(se, new object[] { float.Parse(onDurationSpecialEffect.Element("Parameter").Attribute("Value").Value, CultureInfo.InvariantCulture) }));
                            break;

                        case SpecialEffect.PercentageDamageBoost:
                            ai.OnDurationSpecialEffects.Add(new AttackEffect(se, new object[] { float.Parse(onDurationSpecialEffect.Element("Parameter").Attribute("Value").Value, CultureInfo.InvariantCulture), float.Parse(onDurationSpecialEffect.Element("SecondParameter").Attribute("Value").Value, CultureInfo.InvariantCulture) }));
                            break;

                        case SpecialEffect.DamageOverTime:
                            ai.OnDurationSpecialEffects.Add(new AttackEffect(se, new object[] { float.Parse(onDurationSpecialEffect.Element("Parameter").Attribute("Value").Value, CultureInfo.InvariantCulture), float.Parse(onDurationSpecialEffect.Element("SecondParameter").Attribute("Value").Value, CultureInfo.InvariantCulture), float.Parse(onDurationSpecialEffect.Element("ThirdParameter").Attribute("Value").Value, CultureInfo.InvariantCulture) }));
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
                            Vector2 impulseForce = Neon.Utils.ParseVector2(onHitSpecialEffect.Element("Parameter").Attribute("Value").Value);
                            bool impulseBool = bool.Parse(onHitSpecialEffect.Element("SecondParameter").Attribute("Value").Value);
                            bool notInAir = bool.Parse(onHitSpecialEffect.Element("ThirdParameter").Attribute("Value").Value);
                            ai.OnHitSpecialEffects.Add(new AttackEffect(se, new object[] { impulseForce, impulseBool, notInAir }));
                            break;

                        case SpecialEffect.StartAttack:
                            string attackToLaunch = onHitSpecialEffect.Element("Parameter").Attribute("Value").Value;
                            ai.OnHitSpecialEffects.Add(new AttackEffect(se, new object[] { attackToLaunch }));
                            break;

                        case SpecialEffect.PositionalPulse:
                            Vector2 pulseForce = Neon.Utils.ParseVector2(onHitSpecialEffect.Element("Parameter").Attribute("Value").Value);
                            ai.OnHitSpecialEffects.Add(new AttackEffect(se, new object[] { pulseForce }));
                            break;

                        case SpecialEffect.ShootBullet:
                            BulletInfo bi = BulletsManager.GetBulletInfo(onHitSpecialEffect.Element("Parameter").Attribute("Value").Value);
                            ai.OnHitSpecialEffects.Add(new AttackEffect(se, new object[] { bi, Neon.Utils.ParseVector2(onHitSpecialEffect.Element("SecondParameter").Attribute("Value").Value) }));
                            break;

                        case SpecialEffect.ShootBulletAtTarget:
                            BulletInfo bi2 = BulletsManager.GetBulletInfo(onHitSpecialEffect.Element("Parameter").Attribute("Value").Value);
                            ai.OnHitSpecialEffects.Add(new AttackEffect(se, new object[] { bi2, Neon.Utils.ParseVector2(onHitSpecialEffect.Element("SecondParameter").Attribute("Value").Value) }));
                            break;

                        case SpecialEffect.Invincible:
                            ai.OnHitSpecialEffects.Add(new AttackEffect(se, new object[] { onHitSpecialEffect.Element("Parameter").Attribute("Value").Value }));
                            break;

                        case SpecialEffect.EffectAnimation:
                            SpriteSheetInfo ssi = AssetManager.GetSpriteSheet(onHitSpecialEffect.Element("Parameter").Attribute("Value").Value);
                            float rotation = float.Parse(onHitSpecialEffect.Element("SecondParameter").Attribute("Value").Value, CultureInfo.InvariantCulture);
                            Vector2 offset = Neon.Utils.ParseVector2(onHitSpecialEffect.Element("ThirdParameter").Attribute("Value").Value);
                            ai.OnHitSpecialEffects.Add(new AttackEffect(se, new object[] { ssi, rotation, offset, bool.Parse(onHitSpecialEffect.Element("FourthParameter").Attribute("Value").Value), float.Parse(onHitSpecialEffect.Element("FifthParameter").Attribute("Value").Value, CultureInfo.InvariantCulture) }));
                            break;

                        case SpecialEffect.MoveWhileAttacking:
                            ai.OnHitSpecialEffects.Add(new AttackEffect(se, new object[] { float.Parse(onHitSpecialEffect.Element("Parameter").Attribute("Value").Value, CultureInfo.InvariantCulture) }));
                            break;

                        case SpecialEffect.PercentageDamageBoost:
                            ai.OnHitSpecialEffects.Add(new AttackEffect(se, new object[] { float.Parse(onHitSpecialEffect.Element("Parameter").Attribute("Value").Value, CultureInfo.InvariantCulture), float.Parse(onHitSpecialEffect.Element("SecondParameter").Attribute("Value").Value, CultureInfo.InvariantCulture)}));
                            break;

                        case SpecialEffect.DamageOverTime:
                            ai.OnHitSpecialEffects.Add(new AttackEffect(se, new object[] { float.Parse(onHitSpecialEffect.Element("Parameter").Attribute("Value").Value, CultureInfo.InvariantCulture), float.Parse(onHitSpecialEffect.Element("SecondParameter").Attribute("Value").Value, CultureInfo.InvariantCulture), float.Parse(onHitSpecialEffect.Element("ThirdParameter").Attribute("Value").Value, CultureInfo.InvariantCulture) }));
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
                            Vector2 impulseForce = Neon.Utils.ParseVector2(onGroundCancelSpecialEffect.Element("Parameter").Attribute("Value").Value);
                            bool impulseBool = bool.Parse(onGroundCancelSpecialEffect.Element("ParameterTwo").Attribute("Value").Value);
                            bool notInAir = bool.Parse(onGroundCancelSpecialEffect.Element("ThirdParameter").Attribute("Value").Value);
                            ai.OnGroundCancelSpecialEffects.Add(new AttackEffect(se, new object[] { impulseForce, impulseBool, notInAir }));
                            break;

                        case SpecialEffect.StartAttack:
                            string attackToLaunch = onGroundCancelSpecialEffect.Element("Parameter").Attribute("Value").Value;
                            ai.OnGroundCancelSpecialEffects.Add(new AttackEffect(se, new object[] { attackToLaunch }));
                            break;

                        case SpecialEffect.PositionalPulse:
                            Vector2 pulseForce = Neon.Utils.ParseVector2(onGroundCancelSpecialEffect.Element("Parameter").Attribute("Value").Value);
                            ai.OnGroundCancelSpecialEffects.Add(new AttackEffect(se, new object[] { pulseForce }));
                            break;

                        case SpecialEffect.ShootBullet:
                            BulletInfo bi = BulletsManager.GetBulletInfo(onGroundCancelSpecialEffect.Element("Parameter").Attribute("Value").Value);
                            ai.OnGroundCancelSpecialEffects.Add(new AttackEffect(se, new object[] { bi, Neon.Utils.ParseVector2(onGroundCancelSpecialEffect.Element("SecondParameter").Attribute("Value").Value) }));
                            break;

                        case SpecialEffect.ShootBulletAtTarget:
                            BulletInfo bi2 = BulletsManager.GetBulletInfo(onGroundCancelSpecialEffect.Element("Parameter").Attribute("Value").Value);
                            ai.OnGroundCancelSpecialEffects.Add(new AttackEffect(se, new object[] { bi2, Neon.Utils.ParseVector2(onGroundCancelSpecialEffect.Element("SecondParameter").Attribute("Value").Value) }));
                            break;

                        case SpecialEffect.Invincible:
                            ai.OnGroundCancelSpecialEffects.Add(new AttackEffect(se, new object[] { onGroundCancelSpecialEffect.Element("Parameter").Attribute("Value").Value }));
                            break;

                        case SpecialEffect.EffectAnimation:
                            SpriteSheetInfo ssi = AssetManager.GetSpriteSheet(onGroundCancelSpecialEffect.Element("Parameter").Attribute("Value").Value);
                            float rotation = float.Parse(onGroundCancelSpecialEffect.Element("SecondParameter").Attribute("Value").Value, CultureInfo.InvariantCulture);
                            Vector2 offset = Neon.Utils.ParseVector2(onGroundCancelSpecialEffect.Element("ThirdParameter").Attribute("Value").Value);
                            ai.OnGroundCancelSpecialEffects.Add(new AttackEffect(se, new object[] { ssi, rotation, offset, bool.Parse(onGroundCancelSpecialEffect.Element("FourthParameter").Attribute("Value").Value), float.Parse(onGroundCancelSpecialEffect.Element("FifthParameter").Attribute("Value").Value, CultureInfo.InvariantCulture) }));
                            break;

                        case SpecialEffect.MoveWhileAttacking:
                            ai.OnGroundCancelSpecialEffects.Add(new AttackEffect(se, new object[] { float.Parse(onGroundCancelSpecialEffect.Element("Parameter").Attribute("Value").Value) }));
                            break;

                        case SpecialEffect.PercentageDamageBoost:
                            ai.OnGroundCancelSpecialEffects.Add(new AttackEffect(se, new object[] { float.Parse(onGroundCancelSpecialEffect.Element("Parameter").Attribute("Value").Value, CultureInfo.InvariantCulture), float.Parse(onGroundCancelSpecialEffect.Element("SecondParameter").Attribute("Value").Value, CultureInfo.InvariantCulture)}));
                            break;

                        case SpecialEffect.DamageOverTime:
                            ai.OnGroundCancelSpecialEffects.Add(new AttackEffect(se, new object[] { float.Parse(onGroundCancelSpecialEffect.Element("Parameter").Attribute("Value").Value, CultureInfo.InvariantCulture), float.Parse(onGroundCancelSpecialEffect.Element("SecondParameter").Attribute("Value").Value, CultureInfo.InvariantCulture), float.Parse(onGroundCancelSpecialEffect.Element("ThirdParameter").Attribute("Value").Value, CultureInfo.InvariantCulture) }));
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
            (Neon.World as GameScreen).FreeAttacks.Add(attack);

            return attack;
        }

        static public AttackInfo GetAttackInfo(string name)
        {
            return _attacksInformation.First(ai => ai.Name == name);
        }   
    }
}
