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
        Boost,
        StartAttack
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
                            ai.SpecialEffects.Add(new AttackEffect(se, impulseForce));
                            break;

                        case SpecialEffect.StartAttack:
                            string attackToLaunch = specialEffect.Element("Parameter").Attribute("Value").Value;
                            ai.SpecialEffects.Add(new AttackEffect(se, attackToLaunch));
                            break;

                        case SpecialEffect.PositionalPulse:
                            Vector2 pulseForce = Neon.utils.ParseVector2(specialEffect.Element("Parameter").Attribute("Value").Value);
                            ai.SpecialEffects.Add(new AttackEffect(se, pulseForce));
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
                            ai.OnHitSpecialEffects.Add(new AttackEffect(se, impulseForce));
                            break;

                        case SpecialEffect.StartAttack:
                            string attackToLaunch = onHitSpecialEffect.Element("Parameter").Attribute("Value").Value;
                            ai.OnHitSpecialEffects.Add(new AttackEffect(se, attackToLaunch));
                            break;

                        case SpecialEffect.PositionalPulse:
                            Vector2 pulseForce = Neon.utils.ParseVector2(onHitSpecialEffect.Element("Parameter").Attribute("Value").Value);
                            ai.OnHitSpecialEffects.Add(new AttackEffect(se, pulseForce));
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
                            ai.OnGroundCancelSpecialEffects.Add(new AttackEffect(se, impulseForce));
                            break;

                        case SpecialEffect.StartAttack:
                            string attackToLaunch = onGroundCancelSpecialEffect.Element("Parameter").Attribute("Value").Value;
                            ai.OnGroundCancelSpecialEffects.Add(new AttackEffect(se, attackToLaunch));
                            break;

                        case SpecialEffect.PositionalPulse:
                            Vector2 pulseForce = Neon.utils.ParseVector2(onGroundCancelSpecialEffect.Element("Parameter").Attribute("Value").Value);
                            ai.OnGroundCancelSpecialEffects.Add(new AttackEffect(se, pulseForce));
                            break;
                    }
                }

                _attacksInformation.Add(ai);
            }
        }

        static public Attack GetAttack(string name, Side side, Entity launcher)
        {
            if (name == "")
                return null;
            AttackInfo attackInfo = _attacksInformation.First(ai => ai.Name == name);
            Attack attack = new Attack(attackInfo, side, launcher);

            return attack;
        }

        static public Attack StartFreeAttack(string name, Side side, Vector2 Position)
        {
            AttackInfo attackInfo = _attacksInformation.First(ai => ai.Name == name);
            Attack attack = new Attack(attackInfo, side, Position);
            (Neon.world as GameScreen).FreeAttacks.Add(attack);

            return attack;
        }

        static public AttackInfo GetAttackInfo(string name)
        {
            return _attacksInformation.First(ai => ai.Name == name);
        }   
    }
}
