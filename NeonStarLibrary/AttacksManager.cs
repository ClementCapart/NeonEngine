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
        DamageOverTime,
        Boost
    }

    public enum AttackType
    {
        Melee,
        Range,
        Special
    }

    public struct AttackInfo
    {
        public string Name;
        public AttackType Type;
        public List<Rectangle> Hitboxes;
        public float DamageOnHit;
        public float Cooldown;
        public float Duration;
        public Dictionary<SpecialEffect, object> SpecialEffects;
        public Dictionary<SpecialEffect, object> OnHitSpecialEffects;
    }

    static public class AttacksManager
    {
        static private List<AttackInfo> _attacksInformation = new List<AttackInfo>();

        static public void LoadAttacks()
        {
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

                ai.DamageOnHit = float.Parse(attack.Element("DamageOnHit").Value, CultureInfo.InvariantCulture);
                ai.Cooldown = float.Parse(attack.Element("Cooldown").Value, CultureInfo.InvariantCulture);
                ai.Duration = float.Parse(attack.Element("Duration").Value, CultureInfo.InvariantCulture);

                ai.SpecialEffects = new Dictionary<SpecialEffect, object>();

                foreach (XElement specialEffect in attack.Element("SpecialEffects").Elements("Effect"))
                {
                    SpecialEffect se = (SpecialEffect)Enum.Parse(typeof(SpecialEffect), specialEffect.Attribute("Type").Value);

                    switch(se)
                    {
                        case SpecialEffect.Impulse:
                            Vector2 impulseForce = Neon.utils.ParseVector2(specialEffect.Element("Parameter").Attribute("Value").Value);
                            ai.SpecialEffects.Add(se, impulseForce);
                            break;
                    }         
                }

                ai.OnHitSpecialEffects = new Dictionary<SpecialEffect, object>();

                foreach (XElement onHitSpecialEffect in attack.Element("OnHitSpecialEffects").Elements("Effect"))
                {
                    SpecialEffect se = (SpecialEffect)Enum.Parse(typeof(SpecialEffect), onHitSpecialEffect.Attribute("Type").Value);

                    switch(se)
                    {
                        case SpecialEffect.Impulse:
                            Vector2 impulseForce = Neon.utils.ParseVector2(onHitSpecialEffect.Element("Parameter").Attribute("Value").Value);
                            ai.OnHitSpecialEffects.Add(se, impulseForce);
                            break;
                    }         
                }

                _attacksInformation.Add(ai);
            }
        }

        static public Attack GetAttack(string name, Side side, Entity launcher)
        {
            AttackInfo attackInfo = _attacksInformation.First(ai => ai.Name == name);
            Attack attack = new Attack(attackInfo, side, launcher);

            return attack;
        }
    }
}
