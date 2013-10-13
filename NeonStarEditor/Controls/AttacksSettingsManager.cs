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
        string InitialName = "";

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

        private void InitInformations()
        {
            this.AttackName.Text = _attackList[AttacksList.SelectedValue.ToString()].Name;
            this.TypeComboBox.SelectedIndex = (int)_attackList[AttacksList.SelectedValue.ToString()].Type;
            this.DamageNumeric.Value = (decimal)_attackList[AttacksList.SelectedValue.ToString()].DamageOnHit;
            this.DelayNumeric.Value = (decimal)_attackList[AttacksList.SelectedValue.ToString()].Delay;
            this.DurationNumeric.Value = (decimal)_attackList[AttacksList.SelectedValue.ToString()].Duration;
            this.CooldownNumeric.Value = (decimal)_attackList[AttacksList.SelectedValue.ToString()].Cooldown;
            this.AirLockNumeric.Value = (decimal)_attackList[AttacksList.SelectedValue.ToString()].AirLock;
            this.TargetAirLockNumeric.Value = (decimal)_attackList[AttacksList.SelectedValue.ToString()].TargetAirLock;

            int yPosition = 5;
            int rectangleIndex = 0;
            this.HitboxesPanel.Controls.Clear();

            foreach (Microsoft.Xna.Framework.Rectangle rectangle in _attackList[AttacksList.SelectedValue.ToString()].Hitboxes)
            {
                Label label = new Label();
                label.Text = "X";
                label.Location = new System.Drawing.Point(5, yPosition + 4);
                label.Width = 10;
                this.HitboxesPanel.Controls.Add(label);

                NumericUpDown numericUpDown = new NumericUpDown();
                numericUpDown.Name = "X_" + rectangleIndex;
                numericUpDown.Width = 50;
                numericUpDown.Minimum = -1000;
                numericUpDown.Maximum = 1000;
                numericUpDown.Location = new System.Drawing.Point(label.Location.X + label.Width + 5, yPosition);
                numericUpDown.Value = rectangle.X;
                numericUpDown.Enter += Numeric_Enter;
                numericUpDown.Leave += Numeric_Leave;
                numericUpDown.ValueChanged += hitboxesNumericUpDown_ValueChanged;
                this.HitboxesPanel.Controls.Add(numericUpDown);

                label = new Label();
                label.Text = "Y";
                label.Location = new System.Drawing.Point(numericUpDown.Location.X + numericUpDown.Width + 5, yPosition + 4);
                label.Width = 10;
                this.HitboxesPanel.Controls.Add(label);

                numericUpDown = new NumericUpDown();
                numericUpDown.Name = "Y_" + rectangleIndex;
                numericUpDown.Width = 50;
                numericUpDown.Minimum = -1000;
                numericUpDown.Maximum = 1000;
                numericUpDown.Location = new System.Drawing.Point(label.Location.X + label.Width + 5, yPosition);
                numericUpDown.Value = rectangle.Y;
                numericUpDown.Enter += Numeric_Enter;
                numericUpDown.Leave += Numeric_Leave;
                numericUpDown.ValueChanged += hitboxesNumericUpDown_ValueChanged;
                this.HitboxesPanel.Controls.Add(numericUpDown);

                label = new Label();
                label.Text = "Width";
                label.Location = new System.Drawing.Point(numericUpDown.Location.X + numericUpDown.Width + 5, yPosition + 4);
                label.Width = 35;
                this.HitboxesPanel.Controls.Add(label);

                numericUpDown = new NumericUpDown();
                numericUpDown.Name = "W_" + rectangleIndex;
                numericUpDown.Width = 50;
                numericUpDown.Minimum = -1000;
                numericUpDown.Maximum = 1000;
                numericUpDown.Location = new System.Drawing.Point(label.Location.X + label.Width + 5, yPosition);
                numericUpDown.Value = rectangle.Width;
                numericUpDown.Enter += Numeric_Enter;
                numericUpDown.Leave += Numeric_Leave;
                numericUpDown.ValueChanged += hitboxesNumericUpDown_ValueChanged;
                this.HitboxesPanel.Controls.Add(numericUpDown);

                label = new Label();
                label.Text = "Height";
                label.Location = new System.Drawing.Point(numericUpDown.Location.X + numericUpDown.Width + 5, yPosition + 4);
                label.Width = 37;
                this.HitboxesPanel.Controls.Add(label);

                numericUpDown = new NumericUpDown();
                numericUpDown.Name = "H_" + rectangleIndex;
                numericUpDown.Width = 50;
                numericUpDown.Minimum = -1000;
                numericUpDown.Maximum = 1000;
                numericUpDown.Location = new System.Drawing.Point(label.Location.X + label.Width + 5, yPosition);
                numericUpDown.Value = rectangle.Height;
                numericUpDown.Enter += Numeric_Enter;
                numericUpDown.Leave += Numeric_Leave;
                numericUpDown.ValueChanged += hitboxesNumericUpDown_ValueChanged;
                this.HitboxesPanel.Controls.Add(numericUpDown);

                yPosition += numericUpDown.Height + 5;
                rectangleIndex++;
            }

            if (rectangleIndex > 0)
            {
                Button buttonRemove = new Button();
                buttonRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                buttonRemove.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
                buttonRemove.Location = new System.Drawing.Point(5, yPosition + 5);
                buttonRemove.Size = new System.Drawing.Size(100, 23);
                buttonRemove.Text = "Remove Last";
                buttonRemove.Click += buttonRemove_Click;
                buttonRemove.UseVisualStyleBackColor = true;

                this.HitboxesPanel.Controls.Add(buttonRemove);
            }

            Button buttonAdd = new Button();
            buttonAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonAdd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            buttonAdd.Location = new System.Drawing.Point(115, yPosition + 5);
            buttonAdd.Size = new System.Drawing.Size(100, 23);
            buttonAdd.Text = "Add New";
            buttonAdd.UseVisualStyleBackColor = true;
            buttonAdd.Click += buttonAdd_Click;

            this.HitboxesPanel.Controls.Add(buttonAdd);
        }

        void buttonAdd_Click(object sender, EventArgs e)
        {
            _attackList[AttacksList.SelectedValue.ToString()].Hitboxes.Add(new Microsoft.Xna.Framework.Rectangle(0, 0, 0, 0));
            InitInformations();
        }

        private void AttacksList_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitInformations();
        }

        void buttonRemove_Click(object sender, EventArgs e)
        {
            _attackList[AttacksList.SelectedValue.ToString()].Hitboxes.RemoveAt(_attackList[AttacksList.SelectedValue.ToString()].Hitboxes.Count - 1);
            InitInformations();
        }

        private void hitboxesNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown numericUpDown = sender as NumericUpDown;
            string[] nameInformation = numericUpDown.Name.Split('_');

            string name = nameInformation[0];
            Microsoft.Xna.Framework.Rectangle rectangle = _attackList[AttacksList.SelectedValue.ToString()].Hitboxes[int.Parse(nameInformation[1])];      

            switch(name)
            {
                case "X":
                    _attackList[AttacksList.SelectedValue.ToString()].Hitboxes[int.Parse(nameInformation[1])] = new Microsoft.Xna.Framework.Rectangle((int)numericUpDown.Value, rectangle.Y, rectangle.Width, rectangle.Height);
                    break;

                case "Y":
                    _attackList[AttacksList.SelectedValue.ToString()].Hitboxes[int.Parse(nameInformation[1])] = new Microsoft.Xna.Framework.Rectangle(rectangle.X, (int)numericUpDown.Value, rectangle.Width, rectangle.Height);
                    break;

                case "W":
                    _attackList[AttacksList.SelectedValue.ToString()].Hitboxes[int.Parse(nameInformation[1])] = new Microsoft.Xna.Framework.Rectangle(rectangle.X, rectangle.Y, (int)numericUpDown.Value, rectangle.Height);
                    break;

                case "H":
                    _attackList[AttacksList.SelectedValue.ToString()].Hitboxes[int.Parse(nameInformation[1])] = new Microsoft.Xna.Framework.Rectangle(rectangle.X, rectangle.Y, rectangle.Width, (int)numericUpDown.Value);
                    break;
            }
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

        private void AttackName_Enter(object sender, EventArgs e)
        {
            InitialName = (sender as TextBox).Text;
            (Neon.world as EditorScreen).FocusedTextBox = sender as TextBox;
        }

        private void AttackName_Leave(object sender, EventArgs e)
        {

            (Neon.world as EditorScreen).FocusedTextBox = null;
            AttackInfo ai = _attackList[InitialName];
            ai.Name = (sender as TextBox).Text;
            _attackList.Remove(InitialName);
            _attackList.Add((sender as TextBox).Text, ai);
            InitializeData(false);
        }
    }
}
