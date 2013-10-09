using Microsoft.Xna.Framework;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    public enum SpecialEffect
    {
        Impulse,
        DamageOverTime,
        Boost
    }

    public struct AttackInfo
    {
        public string Name;
        public List<Rectangle> Hitboxes;
        public float DamageOnHit;
        public float Cooldown;
        public float Duration;
        public Dictionary<SpecialEffect, object> SpecialEffects;
        public Dictionary<SpecialEffect, object> OnHitSpecialEffects;
    }

    static public class AttacksManager
    {
        static private List<AttackInfo> _attacksInformation;

        static public void Init()
        {
            _attacksInformation = new List<AttackInfo>();

            AttackInfo ai = new AttackInfo();
            ai.Name = "TestAttack";
            ai.Hitboxes = new List<Rectangle>();
            ai.Hitboxes.Add(new Rectangle(50, -30, 50, 80));
            ai.Cooldown = 1.0f;
            ai.Duration = 1.0f;
            ai.DamageOnHit = 5.0f;
            ai.OnHitSpecialEffects = new Dictionary<SpecialEffect, object>();
            ai.OnHitSpecialEffects.Add(SpecialEffect.Impulse, new Vector2(3000, 0));
            ai.SpecialEffects = new Dictionary<SpecialEffect, object>();
            ai.SpecialEffects.Add(SpecialEffect.Impulse, new Vector2(0, 300));
            _attacksInformation.Add(ai);
        }

        static public Attack GetAttack(string name, Side side, Entity launcher)
        {

            AttackInfo attackInfo = _attacksInformation.First(ai => ai.Name == name);
            Attack attack = new Attack(attackInfo, side, launcher);

            return attack;
        }
    }
}
