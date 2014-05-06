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
        InstantiatePrefab,
        PlaySound,
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
        public string GroupName = "NoGroup";
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
        public Vector2 AirImpulse = Vector2.Zero;
        public bool AlwaysStunlock = false;
        public bool AirOnly = false;
        public bool CancelOnGround = false;
        public bool OnlyOnceInAir = false;
        public List<AttackEffect> OnDelaySpecialEffects = new List<AttackEffect>();
        public List<AttackEffect> OnDurationSpecialEffects = new List<AttackEffect>();
        public List<AttackEffect> OnHitSpecialEffects = new List<AttackEffect>();
        public List<AttackEffect> OnGroundCancelSpecialEffects = new List<AttackEffect>();
        public List<AttackEffect> OnFinishSpecialEffects = new List<AttackEffect>();
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
                ai.GroupName = attack.Attribute("Group").Value.ToString();
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
                ai.AirImpulse = Neon.Utils.ParseVector2(attack.Element("AirImpulse").Value);
                ai.AlwaysStunlock = bool.Parse(attack.Element("AlwaysStunlock").Value);

                ai.OnDelaySpecialEffects = new List<AttackEffect>();

                SetAttackEffects(attack.Element("OnDelaySpecialEffects"), ai.OnDelaySpecialEffects);

                ai.OnDurationSpecialEffects = new List<AttackEffect>();

                SetAttackEffects(attack.Element("OnDurationSpecialEffects"), ai.OnDurationSpecialEffects);

                ai.OnHitSpecialEffects = new List<AttackEffect>();

                SetAttackEffects(attack.Element("OnHitSpecialEffects"), ai.OnHitSpecialEffects);

                ai.OnGroundCancelSpecialEffects = new List<AttackEffect>();

                SetAttackEffects(attack.Element("OnGroundCancelSpecialEffects"), ai.OnGroundCancelSpecialEffects);

                ai.OnFinishSpecialEffects = new List<AttackEffect>();

                SetAttackEffects(attack.Element("OnFinishSpecialEffects"), ai.OnFinishSpecialEffects);

                _attacksInformation.Add(ai);
            }
        }

        private static void SetAttackEffects(XElement currentSpecialEffectsData, List<AttackEffect> currentAttackEffects)
        {
            if (currentSpecialEffectsData == null)
                return;

            foreach (XElement currentSpecialEffect in currentSpecialEffectsData.Elements("Effect"))
            {
                SpecialEffect se = (SpecialEffect)Enum.Parse(typeof(SpecialEffect), currentSpecialEffect.Attribute("Type").Value);

                switch (se)
                {
                    case SpecialEffect.Impulse:
                        Vector2 impulseForce = Neon.Utils.ParseVector2(currentSpecialEffect.Element("Parameter").Attribute("Value").Value);
                        bool impulseBool = bool.Parse(currentSpecialEffect.Element("SecondParameter").Attribute("Value").Value);
                        bool notInAir = bool.Parse(currentSpecialEffect.Element("ThirdParameter").Attribute("Value").Value);
                        currentAttackEffects.Add(new AttackEffect(se, new object[] { impulseForce, impulseBool, notInAir }));
                        break;

                    case SpecialEffect.StartAttack:
                        string attackToLaunch = currentSpecialEffect.Element("Parameter").Attribute("Value").Value;
                        currentAttackEffects.Add(new AttackEffect(se, new object[] { attackToLaunch, bool.Parse(currentSpecialEffect.Element("SecondParameter").Attribute("Value").Value), float.Parse(currentSpecialEffect.Element("ThirdParameter").Attribute("Value").Value) }));
                        break;

                    case SpecialEffect.PositionalPulse:
                        Vector2 pulseForce = Neon.Utils.ParseVector2(currentSpecialEffect.Element("Parameter").Attribute("Value").Value);
                        currentAttackEffects.Add(new AttackEffect(se, new object[] { pulseForce }));
                        break;

                    case SpecialEffect.ShootBullet:
                        BulletInfo bi = BulletsManager.GetBulletInfo(currentSpecialEffect.Element("Parameter").Attribute("Value").Value);
                        currentAttackEffects.Add(new AttackEffect(se, new object[] { bi, Neon.Utils.ParseVector2(currentSpecialEffect.Element("SecondParameter").Attribute("Value").Value) }));
                        break;

                    case SpecialEffect.ShootBulletAtTarget:
                        BulletInfo bi2 = BulletsManager.GetBulletInfo(currentSpecialEffect.Element("Parameter").Attribute("Value").Value);
                        currentAttackEffects.Add(new AttackEffect(se, new object[] { bi2, Neon.Utils.ParseVector2(currentSpecialEffect.Element("SecondParameter").Attribute("Value").Value) }));
                        break;

                    case SpecialEffect.Invincible:
                        currentAttackEffects.Add(new AttackEffect(se, new object[] { float.Parse(currentSpecialEffect.Element("Parameter").Attribute("Value").Value, CultureInfo.InvariantCulture) }));
                        break;

                    case SpecialEffect.EffectAnimation:
                        SpriteSheetInfo ssi = AssetManager.GetSpriteSheet(currentSpecialEffect.Element("Parameter").Attribute("Value").Value);
                        float rotation = float.Parse(currentSpecialEffect.Element("SecondParameter").Attribute("Value").Value, CultureInfo.InvariantCulture);
                        Vector2 offset = Neon.Utils.ParseVector2(currentSpecialEffect.Element("ThirdParameter").Attribute("Value").Value);
                        currentAttackEffects.Add(new AttackEffect(se, new object[] { ssi, rotation, offset, bool.Parse(currentSpecialEffect.Element("FourthParameter").Attribute("Value").Value), float.Parse(currentSpecialEffect.Element("FifthParameter").Attribute("Value").Value, CultureInfo.InvariantCulture), float.Parse(currentSpecialEffect.Element("SixthParameter").Attribute("Value").Value), float.Parse(currentSpecialEffect.Element("SeventhParameter").Attribute("Value").Value) }));
                        break;

                    case SpecialEffect.MoveWhileAttacking:
                        currentAttackEffects.Add(new AttackEffect(se, new object[] { float.Parse(currentSpecialEffect.Element("Parameter").Attribute("Value").Value, CultureInfo.InvariantCulture), bool.Parse(currentSpecialEffect.Element("SecondParameter").Attribute("Value").Value) }));
                        break;

                    case SpecialEffect.PercentageDamageBoost:
                        currentAttackEffects.Add(new AttackEffect(se, new object[] { float.Parse(currentSpecialEffect.Element("Parameter").Attribute("Value").Value, CultureInfo.InvariantCulture), float.Parse(currentSpecialEffect.Element("SecondParameter").Attribute("Value").Value, CultureInfo.InvariantCulture) }));
                        break;

                    case SpecialEffect.DamageOverTime:
                        currentAttackEffects.Add(new AttackEffect(se, new object[] { float.Parse(currentSpecialEffect.Element("Parameter").Attribute("Value").Value, CultureInfo.InvariantCulture), float.Parse(currentSpecialEffect.Element("SecondParameter").Attribute("Value").Value, CultureInfo.InvariantCulture), float.Parse(currentSpecialEffect.Element("ThirdParameter").Attribute("Value").Value, CultureInfo.InvariantCulture) }));
                        break;

                    case SpecialEffect.InstantiatePrefab:
                        currentAttackEffects.Add(new AttackEffect(se, new object[] { currentSpecialEffect.Element("Parameter").Attribute("Value").Value, Neon.Utils.ParseVector2(currentSpecialEffect.Element("SecondParameter").Attribute("Value").Value), Neon.Utils.ParseVector2(currentSpecialEffect.Element("ThirdParameter").Attribute("Value").Value) }));
                        break;

                    case SpecialEffect.PlaySound:
                        currentAttackEffects.Add(new AttackEffect(se, new object[] { currentSpecialEffect.Element("Parameter").Attribute("Value").Value, float.Parse(currentSpecialEffect.Element("SecondParameter").Attribute("Value").Value) }));
                        break;
                }
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

        static public Attack StartFreeAttack(string name, Side side, Vector2 Position, bool fromEnemy)
        {
            AttackInfo[] aiList = _attacksInformation.Where(ai => ai.Name == name).ToArray();

            if (aiList.Length == 0)
                return null;
            Attack attack = new Attack(aiList.First(), side, Position, fromEnemy);
            (Neon.World as GameScreen).FreeAttacks.Add(attack);

            return attack;
        }

        static public AttackInfo GetAttackInfo(string name)
        {
            return _attacksInformation.First(ai => ai.Name == name);
        }   
    }
}
