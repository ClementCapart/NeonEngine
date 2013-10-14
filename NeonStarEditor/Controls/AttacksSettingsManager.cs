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
        AttackEffect CurrentAttackEffectSelected = null;

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

                XElement airOnly = new XElement("AirOnly", kvp.Value.AirOnly.ToString());
                attack.Add(airOnly);

                XElement cancelOnGround = new XElement("GroundCancel", kvp.Value.CancelOnGround.ToString());
                attack.Add(cancelOnGround);

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
                foreach (AttackEffect effect in kvp.Value.SpecialEffects)
                {
                    specialEffects.Add(CreateEffectText(effect));
                }
                attack.Add(specialEffects);

                XElement onHitSpecialEffects = new XElement("OnHitSpecialEffects");
                foreach (AttackEffect effect in kvp.Value.OnHitSpecialEffects)
                {
                    onHitSpecialEffects.Add(CreateEffectText(effect));
                }
                attack.Add(onHitSpecialEffects);

                attacks.Add(attack);
            }
            xnaContent.Add(attacks);
            document.Add(xnaContent);

            document.Save(@"../Data/Config/Attacks.xml");

            AttacksManager.LoadAttacks();

        }

        private XElement CreateEffectText(AttackEffect effectKvp)
        {
            XElement effect = new XElement("Effect", new XAttribute("Type", effectKvp.specialEffect.ToString()));

            switch(effectKvp.specialEffect)
            {
                case SpecialEffect.Impulse:
                    Vector2 impulseValue = (Vector2)effectKvp.Parameters;
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

        private void InitEffectData()
        {
            EffectsInfoPanel.Controls.Clear();

            if (CurrentAttackEffectSelected != null)
            {
                Label label = new Label();
                label.Text = "Effect Type";
                label.Height = 15;
                label.Location = new System.Drawing.Point(5, 10);
                this.EffectsInfoPanel.Controls.Add(label);

                ComboBox comboBox = new ComboBox();
                foreach (Enum e in Enum.GetValues(typeof(SpecialEffect)))
                    comboBox.Items.Add(e);
                comboBox.SelectedItem = CurrentAttackEffectSelected.specialEffect;
                comboBox.Location = new System.Drawing.Point(5, 10 + label.Height + 5);
                comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                comboBox.SelectedValueChanged += comboBox_SelectedValueChanged;
                this.EffectsInfoPanel.Controls.Add(comboBox);


                switch(CurrentAttackEffectSelected.specialEffect)
                {
                    case SpecialEffect.Impulse:

                        label = new Label();
                        label.Text = "Impulse Power";
                        label.Height = 15;
                        label.Location = new System.Drawing.Point(5, 60);
                        this.EffectsInfoPanel.Controls.Add(label);

                        NumericUpDown ImpulsePower = new NumericUpDown();
                        ImpulsePower.Name = "ImpulsePowerX";
                        ImpulsePower.Maximum = 50000;
                        ImpulsePower.Minimum = -50000;
                        ImpulsePower.Width = 80;
                        ImpulsePower.Value = (decimal)((Vector2)CurrentAttackEffectSelected.Parameters).X;
                        ImpulsePower.Location = new System.Drawing.Point(5, label.Location.Y + label.Height + 5);
                        ImpulsePower.Enter += Numeric_Enter;
                        ImpulsePower.Leave += Numeric_Leave;
                        ImpulsePower.ValueChanged += Numeric_ValueChanged;
                        this.EffectsInfoPanel.Controls.Add(ImpulsePower);

                        ImpulsePower = new NumericUpDown();
                        ImpulsePower.Name = "ImpulsePowerY";
                        ImpulsePower.Maximum = 50000;
                        ImpulsePower.Minimum = -50000;
                        ImpulsePower.Width = 80;
                        ImpulsePower.Value = (decimal)((Vector2)CurrentAttackEffectSelected.Parameters).Y;
                        ImpulsePower.Location = new System.Drawing.Point(ImpulsePower.Width + 10, label.Location.Y + label.Height + 5);
                        ImpulsePower.Enter += Numeric_Enter;
                        ImpulsePower.Leave += Numeric_Leave;
                        ImpulsePower.ValueChanged += Numeric_ValueChanged;
                        this.EffectsInfoPanel.Controls.Add(ImpulsePower);

                        break;
                }
            }
        }

        private void comboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if (CurrentAttackEffectSelected.specialEffect != (SpecialEffect)(sender as ComboBox).SelectedItem)
            {
                CurrentAttackEffectSelected.specialEffect = (SpecialEffect)(sender as ComboBox).SelectedItem;
                InitInformations();
            }
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
            this.AirOnlyCheckbox.Checked = _attackList[AttacksList.SelectedValue.ToString()].AirOnly;
            this.GroundCancelCheckbox.Checked = _attackList[AttacksList.SelectedValue.ToString()].CancelOnGround;

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
            this.SpecialEffectsList.DataSource = null;
            this.SpecialEffectsList.DataSource = _attackList[AttacksList.SelectedValue.ToString()].SpecialEffects;
            this.SpecialEffectsList.DisplayMember = "NameType";
            this.OnHitSpecialEffectsList.DataSource = null;
            this.OnHitSpecialEffectsList.DataSource = _attackList[AttacksList.SelectedValue.ToString()].OnHitSpecialEffects;
            this.OnHitSpecialEffectsList.DisplayMember = "NameType";
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

                case "ImpulsePowerX":
                    Vector2 impulsePowerY = (Vector2)CurrentAttackEffectSelected.Parameters;
                    CurrentAttackEffectSelected.Parameters = new Vector2((float)(sender as NumericUpDown).Value, impulsePowerY.Y);
                    break;

                case "ImpulsePowerY":
                    Vector2 impulsePowerX = (Vector2)CurrentAttackEffectSelected.Parameters;
                    CurrentAttackEffectSelected.Parameters = new Vector2(impulsePowerX.X, (float)(sender as NumericUpDown).Value);
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

        private void AddSpecial_Click(object sender, EventArgs e)
        {
            _attackList[this.AttacksList.SelectedValue.ToString()].SpecialEffects.Add(new AttackEffect(SpecialEffect.Impulse, Vector2.Zero));
            InitInformations();
        }

        private void AddOnHit_Click(object sender, EventArgs e)
        {
            _attackList[this.AttacksList.SelectedValue.ToString()].OnHitSpecialEffects.Add(new AttackEffect(SpecialEffect.Impulse, Vector2.Zero));
            InitInformations();
        }

        private void RemoveSpecial_Click(object sender, EventArgs e)
        {
            if (SpecialEffectsList.SelectedValue != null)
                _attackList[AttacksList.SelectedValue.ToString()].SpecialEffects.RemoveAt(SpecialEffectsList.SelectedIndex);
            InitInformations();
        }

        private void RemoveOnHit_Click(object sender, EventArgs e)
        {
            if (OnHitSpecialEffectsList.SelectedValue != null)
                _attackList[AttacksList.SelectedValue.ToString()].OnHitSpecialEffects.RemoveAt(OnHitSpecialEffectsList.SelectedIndex);
            InitInformations();
        }

        private void SpecialEffectsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((sender as ListBox).SelectedIndex == -1)
            {
                CurrentAttackEffectSelected = null;
            }
            else
            {
                CurrentAttackEffectSelected = _attackList[AttacksList.SelectedValue.ToString()].SpecialEffects[(sender as ListBox).SelectedIndex];
            }           
            InitEffectData();
        }

        private void OnHitSpecialEffectsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((sender as ListBox).SelectedIndex == -1)
            {
                CurrentAttackEffectSelected = null;
            }
            else
            {
                CurrentAttackEffectSelected = _attackList[AttacksList.SelectedValue.ToString()].OnHitSpecialEffects[(sender as ListBox).SelectedIndex];
            }          
            InitEffectData();
        }

        private void GroundCancelCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            _attackList[AttacksList.SelectedValue.ToString()].CancelOnGround = (sender as CheckBox).Checked;
        }

        private void AirOnlyCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            _attackList[AttacksList.SelectedValue.ToString()].AirOnly = (sender as CheckBox).Checked;
        }
    }
}
