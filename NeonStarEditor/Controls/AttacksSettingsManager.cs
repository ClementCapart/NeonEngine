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
                AttacksManager._attacksInformation = AttacksManager._attacksInformation.OrderBy(ai => ai.Name).ToList<AttackInfo>();
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
                AttacksManager._attacksInformation.Remove(_attackList[(string)AttacksList.SelectedValue]);
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
            AttacksManager._attacksInformation.Add(_attackList["Attack" + i]);
            InitializeData(false);
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

                XElement onlyOnceInAir = new XElement("OnlyOnceInAir", kvp.Value.OnlyOnceInAir.ToString());
                attack.Add(onlyOnceInAir);

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

                XElement airFactor = new XElement("AirFactor", kvp.Value.AirFactor.ToString("G", CultureInfo.InvariantCulture));
                attack.Add(airFactor);

                XElement stunLock = new XElement("StunLock", kvp.Value.StunLock.ToString("G", CultureInfo.InvariantCulture));
                attack.Add(stunLock);

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

                XElement onGroundCancelSpecialEffects = new XElement("OnGroundCancelSpecialEffects");
                foreach (AttackEffect effect in kvp.Value.OnGroundCancelSpecialEffects)
                {
                    onGroundCancelSpecialEffects.Add(CreateEffectText(effect));
                }
                attack.Add(onGroundCancelSpecialEffects);

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
                    Vector2 impulseValue = (Vector2)effectKvp.Parameters[0];

                    XElement parameter = new XElement("Parameter", new XAttribute("Value", "{X:" + impulseValue.X + " Y:" + impulseValue.Y + "}"));
                    XElement boolPulse = new XElement("SecondParameter", new XAttribute("Value", ((bool)effectKvp.Parameters[1]).ToString()));
                    effect.Add(parameter);
                    effect.Add(boolPulse);
                    break;

                case SpecialEffect.DamageOverTime:
                    break;

                case SpecialEffect.Boost:
                    break;

                case SpecialEffect.PositionalPulse:
                    Vector2 pulseValue = (Vector2)effectKvp.Parameters[0];
                    XElement parameterPulse = new XElement("Parameter", new XAttribute("Value", "{X:" + pulseValue.X + " Y:" + pulseValue.Y + "}"));

                    effect.Add(parameterPulse);
                    break;

                case SpecialEffect.StartAttack:
                    string attackValue = (string)effectKvp.Parameters[0];
                    XElement parameterString = new XElement("Parameter", new XAttribute("Value", attackValue));
                    effect.Add(parameterString);
                    break;

                case SpecialEffect.ShootBullet:
                    string bulletName = (string)(effectKvp.Parameters[0] as BulletInfo).Name;
                    XElement parameterBullet = new XElement("Parameter", new XAttribute("Value", bulletName));
                    effect.Add(parameterBullet);
                    break;

                case SpecialEffect.ShootBulletAtTarget:
                    string bulletName2 = (string)(effectKvp.Parameters[0] as BulletInfo).Name;
                    XElement parameterBullet2 = new XElement("Parameter", new XAttribute("Value", bulletName2));
                    effect.Add(parameterBullet2);
                    break;

                case SpecialEffect.Invincible:
                    XElement parameterInvincibility = new XElement("Parameter", new XAttribute("Value", ((float)effectKvp.Parameters[0]).ToString("G", CultureInfo.InvariantCulture)));
                    effect.Add(parameterInvincibility);
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

                        NumericUpDown impulsePower = new NumericUpDown();
                        impulsePower.Name = "ImpulsePowerX";
                        impulsePower.Maximum = 50000;
                        impulsePower.Minimum = -50000;
                        impulsePower.Width = 80;
                        impulsePower.Value = (decimal)((Vector2)CurrentAttackEffectSelected.Parameters[0]).X;
                        impulsePower.Location = new System.Drawing.Point(5, label.Location.Y + label.Height + 5);
                        impulsePower.Enter += Numeric_Enter;
                        impulsePower.Leave += Numeric_Leave;
                        impulsePower.ValueChanged += Numeric_ValueChanged;
                        this.EffectsInfoPanel.Controls.Add(impulsePower);

                        impulsePower = new NumericUpDown();
                        impulsePower.Name = "ImpulsePowerY";
                        impulsePower.Maximum = 50000;
                        impulsePower.Minimum = -50000;
                        impulsePower.Width = 80;
                        impulsePower.Value = (decimal)((Vector2)CurrentAttackEffectSelected.Parameters[0]).Y;
                        impulsePower.Location = new System.Drawing.Point(impulsePower.Width + 10, label.Location.Y + label.Height + 5);
                        impulsePower.Enter += Numeric_Enter;
                        impulsePower.Leave += Numeric_Leave;
                        impulsePower.ValueChanged += Numeric_ValueChanged;
                        this.EffectsInfoPanel.Controls.Add(impulsePower);

                        CheckBox checkBox = new CheckBox();
                        checkBox.Text = "Must stop on target";
                        checkBox.Name = "TargetStop";
                        checkBox.Checked = (bool)CurrentAttackEffectSelected.Parameters[1];
                        checkBox.Location = new System.Drawing.Point(label.Location.X, impulsePower.Location.Y + impulsePower.Height + 5);
                        checkBox.CheckedChanged += checkBox_CheckedChanged;
                        EffectsInfoPanel.Controls.Add(checkBox);

                        break;

                    case SpecialEffect.PositionalPulse:
                        label = new Label();
                        label.Text = "Pulse Power";
                        label.Height = 15;
                        label.Location = new System.Drawing.Point(5, 60);
                        this.EffectsInfoPanel.Controls.Add(label);

                        NumericUpDown pulsePower = new NumericUpDown();
                        pulsePower.Name = "ImpulsePowerX";
                        pulsePower.Maximum = 50000;
                        pulsePower.Minimum = -50000;
                        pulsePower.Width = 80;
                        pulsePower.Value = (decimal)((Vector2)CurrentAttackEffectSelected.Parameters[0]).X;
                        pulsePower.Location = new System.Drawing.Point(5, label.Location.Y + label.Height + 5);
                        pulsePower.Enter += Numeric_Enter;
                        pulsePower.Leave += Numeric_Leave;
                        pulsePower.ValueChanged += Numeric_ValueChanged;
                        this.EffectsInfoPanel.Controls.Add(pulsePower);

                        pulsePower = new NumericUpDown();
                        pulsePower.Name = "ImpulsePowerY";
                        pulsePower.Maximum = 50000;
                        pulsePower.Minimum = -50000;
                        pulsePower.Width = 80;
                        pulsePower.Value = (decimal)((Vector2)CurrentAttackEffectSelected.Parameters[0]).Y;
                        pulsePower.Location = new System.Drawing.Point(pulsePower.Width + 10, label.Location.Y + label.Height + 5);
                        pulsePower.Enter += Numeric_Enter;
                        pulsePower.Leave += Numeric_Leave;
                        pulsePower.ValueChanged += Numeric_ValueChanged;
                        this.EffectsInfoPanel.Controls.Add(pulsePower);
                        break;

                    case SpecialEffect.StartAttack:
                        label = new Label();
                        label.Text = "Attack to launch";
                        label.Height = 15;
                        label.Location = new System.Drawing.Point(5, 60);
                        this.EffectsInfoPanel.Controls.Add(label);

                        TextBox textBox = new TextBox();
                        textBox.Name = "AttackName";
                        textBox.Location = new System.Drawing.Point(5, label.Location.Y + label.Height + 5);
                        textBox.Width = 150;
                        textBox.Enter += textBox_Enter;
                        textBox.Leave += textBox_Leave;
                        textBox.Text = (string)CurrentAttackEffectSelected.Parameters[0];
                        this.EffectsInfoPanel.Controls.Add(textBox);
                        break;

                    case SpecialEffect.ShootBullet:
                    case SpecialEffect.ShootBulletAtTarget:
                        label = new Label();
                        label.Text = "Bullet to launch";
                        label.Height = 15;
                        label.Location = new System.Drawing.Point(5, 60);
                        this.EffectsInfoPanel.Controls.Add(label);

                        TextBox textBox2 = new TextBox();
                        textBox2.Name = "BulletName";
                        textBox2.Location = new System.Drawing.Point(5, label.Location.Y + label.Height + 5);
                        textBox2.Width = 150;
                        textBox2.Enter += textBox_Enter;
                        textBox2.Leave += textBox_Leave;
                        textBox2.Text = (CurrentAttackEffectSelected.Parameters[0] as BulletInfo) != null ? (string)(CurrentAttackEffectSelected.Parameters[0] as BulletInfo).Name : "";
                        this.EffectsInfoPanel.Controls.Add(textBox2);
                        break;

                    case SpecialEffect.Invincible:
                        label = new Label();
                        label.Text = "Duration";
                        label.Height = 15;
                        label.Location = new System.Drawing.Point(5, 60);
                        this.EffectsInfoPanel.Controls.Add(label);

                        NumericUpDown duration = new NumericUpDown();
                        duration.Name = "InvincibleDuration";
                        duration.Maximum = 50000;
                        duration.Minimum = -50000;
                        duration.Width = 80;
                        duration.DecimalPlaces = 2;
                        duration.Value = (decimal)((float)CurrentAttackEffectSelected.Parameters[0]);
                        duration.Location = new System.Drawing.Point(5, label.Location.Y + label.Height + 5);
                        duration.Enter += Numeric_Enter;
                        duration.Leave += Numeric_Leave;
                        duration.ValueChanged += Numeric_ValueChanged;
                        this.EffectsInfoPanel.Controls.Add(duration);
                        break;
                }
            }
        }

        void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            CurrentAttackEffectSelected.Parameters[1] = (bool)(sender as CheckBox).Checked;
        }

        void textBox_Leave(object sender, EventArgs e)
        {
            if((sender as TextBox).Name == "BulletName")
                CurrentAttackEffectSelected.Parameters[0] = BulletsManager.GetBulletInfo((sender as TextBox).Text);
            else
                CurrentAttackEffectSelected.Parameters[0] = (sender as TextBox).Text;
            (Neon.world as EditorScreen).FocusedTextBox = null;
        }

        void textBox_Enter(object sender, EventArgs e)
        {
            (Neon.world as EditorScreen).FocusedTextBox = sender as TextBox;
        }

        private void comboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if (CurrentAttackEffectSelected.specialEffect != (SpecialEffect)(sender as ComboBox).SelectedItem)
            {
                CurrentAttackEffectSelected.specialEffect = (SpecialEffect)(sender as ComboBox).SelectedItem;
                CurrentAttackEffectSelected.Parameters = null;
                switch(CurrentAttackEffectSelected.specialEffect)
                {
                    case SpecialEffect.PositionalPulse:
                        CurrentAttackEffectSelected.Parameters = new object[] { new Vector2() };
                        break;

                    case SpecialEffect.Impulse:
                        CurrentAttackEffectSelected.Parameters = new object[] { new Vector2(), false };
                        break;

                    case SpecialEffect.Invincible:
                        CurrentAttackEffectSelected.Parameters = new object[] { 0.0f };
                        break;

                    case SpecialEffect.ShootBullet:
                        CurrentAttackEffectSelected.Parameters = new object[] { BulletsManager._bulletsInformation[0] };
                        break;

                    case SpecialEffect.ShootBulletAtTarget:
                        CurrentAttackEffectSelected.Parameters = new object[] { BulletsManager._bulletsInformation[0] };
                        break;

                    case SpecialEffect.StartAttack:
                        CurrentAttackEffectSelected.Parameters = new object[] { "" };
                        break;
                }
                this.InitEffectData();
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
            this.OnlyOnceInAir.Checked = _attackList[AttacksList.SelectedValue.ToString()].OnlyOnceInAir;
            this.AirFactorNU.Value = (decimal)_attackList[AttacksList.SelectedValue.ToString()].AirFactor;
            this.StunLockNumeric.Value = (decimal)_attackList[AttacksList.SelectedValue.ToString()].StunLock;

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
            this.OnGroundCancelSpecialEffectList.DataSource = null;
            this.OnGroundCancelSpecialEffectList.DataSource = _attackList[AttacksList.SelectedValue.ToString()].OnGroundCancelSpecialEffects;
            this.OnGroundCancelSpecialEffectList.DisplayMember = "NameType";
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
                    Vector2 impulsePowerY = (Vector2)CurrentAttackEffectSelected.Parameters[0];
                    CurrentAttackEffectSelected.Parameters[0] = new Vector2((float)(sender as NumericUpDown).Value, impulsePowerY.Y);
                    break;

                case "ImpulsePowerY":
                    Vector2 impulsePowerX = (Vector2)CurrentAttackEffectSelected.Parameters[0];
                    CurrentAttackEffectSelected.Parameters[0] = new Vector2(impulsePowerX.X, (float)(sender as NumericUpDown).Value);
                    break;

                case "AirFactorNU":
                    _attackList[AttacksList.SelectedValue.ToString()].AirFactor = (float)(sender as NumericUpDown).Value;
                    break;

                case "StunLockNumeric":
                    _attackList[AttacksList.SelectedValue.ToString()].StunLock = (float)(sender as NumericUpDown).Value;
                    break;

                case "InvincibleDuration":
                    CurrentAttackEffectSelected.Parameters[0] = (float)(sender as NumericUpDown).Value;
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
            _attackList[this.AttacksList.SelectedValue.ToString()].SpecialEffects.Add(new AttackEffect(SpecialEffect.Impulse, new object[] { Vector2.Zero }));
            InitInformations();
        }

        private void AddOnHit_Click(object sender, EventArgs e)
        {
            _attackList[this.AttacksList.SelectedValue.ToString()].OnHitSpecialEffects.Add(new AttackEffect(SpecialEffect.Impulse, new object[] { Vector2.Zero }));
            InitInformations();
        }

        private void AddOnGround_Click(object sender, EventArgs e)
        {
            _attackList[this.AttacksList.SelectedValue.ToString()].OnGroundCancelSpecialEffects.Add(new AttackEffect(SpecialEffect.Impulse, new object[] { Vector2.Zero }));
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

        private void RemoveOnGround_Click(object sender, EventArgs e)
        {
            if (OnGroundCancelSpecialEffectList.SelectedValue != null)
                _attackList[AttacksList.SelectedValue.ToString()].OnGroundCancelSpecialEffects.RemoveAt(OnGroundCancelSpecialEffectList.SelectedIndex);
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

        private void OnGroundCancelSpecialEffectsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((sender as ListBox).SelectedIndex == -1)
            {
                CurrentAttackEffectSelected = null;
            }
            else
            {
                CurrentAttackEffectSelected = _attackList[AttacksList.SelectedValue.ToString()].OnGroundCancelSpecialEffects[(sender as ListBox).SelectedIndex];
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

        private void OnlyOnceInAir_CheckedChanged(object sender, EventArgs e)
        {
            _attackList[AttacksList.SelectedValue.ToString()].OnlyOnceInAir = (sender as CheckBox).Checked;
        }
    }
}
