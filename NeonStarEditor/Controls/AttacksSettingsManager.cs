using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NeonEngine;
using NeonStarLibrary;
using System.Xml.Linq;
using System.Globalization;
using Microsoft.Xna.Framework;

namespace NeonStarEditor
{
    public partial class AttacksSettingsManager : UserControl
    {
        Dictionary<string, AttackInfo> _attackList = new Dictionary<string, AttackInfo>();

        public AttacksSettingsManager()
        {
            InitializeComponent();
            this.Location = new System.Drawing.Point(Neon.graphicsDevice.PresentationParameters.BackBufferWidth / 2 - this.Width / 2, Neon.graphicsDevice.PresentationParameters.BackBufferHeight / 2 - this.Height / 2);
            foreach (Enum e in Enum.GetValues(typeof(AttackType)))
                TypeComboBox.Items.Add(e);
        }

        public void InitializeData(bool fromGame = true)
        {
            if (fromGame)
            {
                _attackList.Clear();
                foreach (AttackInfo ai in AttacksManager._attacksInformation)
                {
                    _attackList.Add(ai.Name, ai);
                }
            }
            AttacksList.DataSource = _attackList.Keys.ToList();
            
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            if (AttacksList.SelectedValue != null)
            {
                _attackList.Remove((string)AttacksList.SelectedValue);
                InitializeData(false);
            }
        }

        private void AddNew_Click(object sender, EventArgs e)
        {
            int i = 1;
            while(_attackList.ContainsKey("Attack"+i))
                i++;
            
            AttackInfo ai = new AttackInfo();
            ai.Name = "Attack"+i;

            _attackList.Add("Attack" + i, ai);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            int i = 0;
            
            XDocument document = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
            XElement xnaContent = new XElement("XnaContent");
            XElement attacks = new XElement("Attacks");

            foreach (KeyValuePair<string, AttackInfo> kvp in _attackList)
            {
                XElement attack = new XElement("Attack", new XAttribute("ID", i), 
                                                new XAttribute("Name", kvp.Key),
                                                new XAttribute("Type", kvp.Value.Type.ToString()));

                XElement hitboxes = new XElement("Hitboxes");
                foreach(Microsoft.Xna.Framework.Rectangle rectangle in kvp.Value.Hitboxes)
                {
                    XElement hitbox = new XElement("Hitbox", new XAttribute("OffsetX", rectangle.X),
                                                    new XAttribute("OffsetY", rectangle.Y),
                                                    new XAttribute("Width", rectangle.Width),
                                                    new XAttribute("Height", rectangle.Height));

                    hitboxes.Add(hitbox);
                }

                attack.Add(hitboxes);

                XElement damageOnHit = new XElement("DamageOnHit", kvp.Value.DamageOnHit.ToString("G", CultureInfo.InvariantCulture));
                attack.Add(damageOnHit);

                XElement delay = new XElement("Delay", kvp.Value.Delay.ToString("G", CultureInfo.InvariantCulture));
                attack.Add(delay);

                XElement cooldown = new XElement("Cooldown", kvp.Value.Cooldown.ToString("G", CultureInfo.InvariantCulture));
                attack.Add(cooldown);

                XElement duration = new XElement("Duration", kvp.Value.Duration.ToString("G", CultureInfo.InvariantCulture));
                attack.Add(duration);

                XElement airLock = new XElement("AirLock", kvp.Value.AirLock.ToString("G", CultureInfo.InvariantCulture));
                attack.Add(airLock);

                XElement targetAirLock = new XElement("TargetAirLock", kvp.Value.TargetAirLock.ToString("G", CultureInfo.InvariantCulture));
                attack.Add(targetAirLock);

                XElement specialEffects = new XElement("SpecialEffects");
                foreach (KeyValuePair<SpecialEffect, object> effectKvp in kvp.Value.SpecialEffects)
                {
                    specialEffects.Add(CreateEffectText(effectKvp));
                }
                attack.Add(specialEffects);

                XElement onHitSpecialEffects = new XElement("OnHitSpecialEffects");
                foreach (KeyValuePair<SpecialEffect, object> effectKvp in kvp.Value.OnHitSpecialEffects)
                {
                    onHitSpecialEffects.Add(CreateEffectText(effectKvp));
                }
                attack.Add(onHitSpecialEffects);

                attacks.Add(attack);
            }
            xnaContent.Add(attacks);
            document.Add(xnaContent);

            document.Save(@"../Data/Config/Attacks.xml");

            AttacksManager.LoadAttacks();

        }

        private XElement CreateEffectText(KeyValuePair<SpecialEffect, object> effectKvp)
        {
            XElement effect = new XElement("Effect", new XAttribute("Type", effectKvp.Key.ToString()));

            switch(effectKvp.Key)
            {
                case SpecialEffect.Impulse:
                    Vector2 impulseValue = (Vector2)effectKvp.Value;
                    XElement parameter = new XElement("Parameter", new XAttribute("Value", "{X:" + impulseValue.X + " Y:" + impulseValue.Y + "}"));
                    effect.Add(parameter);
                    break;

                case SpecialEffect.DamageOverTime:
                    break;

                case SpecialEffect.Boost:
                    break;
            }

            return effect;
        }

        private void ClosePanel_Click(object sender, EventArgs e)
        {
            (Neon.world as EditorScreen).ToggleAttackManager();
        }

        private void AttacksList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.TypeComboBox.SelectedIndex = (int)_attackList[AttacksList.SelectedValue.ToString()].Type;
            this.DamageNumeric.Value = (decimal)_attackList[AttacksList.SelectedValue.ToString()].DamageOnHit;
            this.DelayNumeric.Value = (decimal)_attackList[AttacksList.SelectedValue.ToString()].Delay;
            this.DurationNumeric.Value = (decimal)_attackList[AttacksList.SelectedValue.ToString()].Duration;
            this.CooldownNumeric.Value = (decimal)_attackList[AttacksList.SelectedValue.ToString()].Cooldown;
            this.AirLockNumeric.Value = (decimal)_attackList[AttacksList.SelectedValue.ToString()].AirLock;
            this.TargetAirLockNumeric.Value = (decimal)_attackList[AttacksList.SelectedValue.ToString()].TargetAirLock;
        }

        private void TypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _attackList[AttacksList.SelectedValue.ToString()].Type = (AttackType)this.TypeComboBox.SelectedIndex;
        }

        private void Numeric_ValueChanged(object sender, EventArgs e)
        {
            switch((sender as NumericUpDown).Name)
            {
                case "DamageNumeric":
                    _attackList[AttacksList.SelectedValue.ToString()].DamageOnHit = (float)(sender as NumericUpDown).Value;
                    break;

                case "DelayNumeric":
                    _attackList[AttacksList.SelectedValue.ToString()].Delay = (float)(sender as NumericUpDown).Value;
                    break;

                case "DurationNumeric":
                    _attackList[AttacksList.SelectedValue.ToString()].Duration = (float)(sender as NumericUpDown).Value;
                    break;

                case "CooldownNumeric":
                    _attackList[AttacksList.SelectedValue.ToString()].Cooldown = (float)(sender as NumericUpDown).Value;
                    break;

                case "AirLockNumeric":
                    _attackList[AttacksList.SelectedValue.ToString()].AirLock = (float)(sender as NumericUpDown).Value;
                    break;

                case "TargetAirLockNumeric":
                    _attackList[AttacksList.SelectedValue.ToString()].TargetAirLock = (float)(sender as NumericUpDown).Value;
                    break;
            }
        }

        private void Numeric_Enter(object sender, EventArgs e)
        {
            (Neon.world as EditorScreen).FocusedNumericUpDown = sender as NumericUpDown;
        }

        private void Numeric_Leave(object sender, EventArgs e)
        {
            (Neon.world as EditorScreen).FocusedNumericUpDown = null;
        }
    }
}
