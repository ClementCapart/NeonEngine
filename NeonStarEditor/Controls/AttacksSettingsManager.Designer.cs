namespace NeonStarEditor
{
    partial class AttacksSettingsManager
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Title = new System.Windows.Forms.Label();
            this.AttacksList = new System.Windows.Forms.ListBox();
            this.RemoveButton = new System.Windows.Forms.Button();
            this.AddNew = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.ClosePanel = new System.Windows.Forms.Button();
            this.AttackInfo = new System.Windows.Forms.GroupBox();
            this.RemoveDelayEffect = new System.Windows.Forms.Button();
            this.AddDelayEffect = new System.Windows.Forms.Button();
            this.OnDelaySpecialEffectsList = new System.Windows.Forms.ListBox();
            this.label14 = new System.Windows.Forms.Label();
            this.OnlyOnceInAir = new System.Windows.Forms.CheckBox();
            this.GroundCancelCheckbox = new System.Windows.Forms.CheckBox();
            this.AirOnlyCheckbox = new System.Windows.Forms.CheckBox();
            this.AttackName = new System.Windows.Forms.TextBox();
            this.TargetAirLockNumeric = new System.Windows.Forms.NumericUpDown();
            this.AirLockNumeric = new System.Windows.Forms.NumericUpDown();
            this.LocalCooldownNumeric = new System.Windows.Forms.NumericUpDown();
            this.MultiHitDelayNU = new System.Windows.Forms.NumericUpDown();
            this.CooldownNumeric = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.AirFactorNU = new System.Windows.Forms.NumericUpDown();
            this.RemoveOnGround = new System.Windows.Forms.Button();
            this.RemoveOnHit = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.StunLockNumeric = new System.Windows.Forms.NumericUpDown();
            this.AddOnGround = new System.Windows.Forms.Button();
            this.AddOnHit = new System.Windows.Forms.Button();
            this.RemoveSpecial = new System.Windows.Forms.Button();
            this.AddSpecial = new System.Windows.Forms.Button();
            this.DelayNumeric = new System.Windows.Forms.NumericUpDown();
            this.OnGroundCancelSpecialEffectList = new System.Windows.Forms.ListBox();
            this.OnHitSpecialEffectsList = new System.Windows.Forms.ListBox();
            this.OnDurationSpecialEffectsList = new System.Windows.Forms.ListBox();
            this.DurationNumeric = new System.Windows.Forms.NumericUpDown();
            this.DamageNumeric = new System.Windows.Forms.NumericUpDown();
            this.EffectsInfoPanel = new System.Windows.Forms.Panel();
            this.HitboxesPanel = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.LocalCooldown = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label111 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Element = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ElementCombobox = new System.Windows.Forms.ComboBox();
            this.TypeComboBox = new System.Windows.Forms.ComboBox();
            this.AttackInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TargetAirLockNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AirLockNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LocalCooldownNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MultiHitDelayNU)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CooldownNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AirFactorNU)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StunLockNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DelayNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DurationNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DamageNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // Title
            // 
            this.Title.AutoSize = true;
            this.Title.Font = new System.Drawing.Font("Agency FB", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.Title.Location = new System.Drawing.Point(297, 12);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(155, 34);
            this.Title.TabIndex = 0;
            this.Title.Text = "Attacks Manager";
            // 
            // AttacksList
            // 
            this.AttacksList.FormattingEnabled = true;
            this.AttacksList.Location = new System.Drawing.Point(17, 99);
            this.AttacksList.Name = "AttacksList";
            this.AttacksList.Size = new System.Drawing.Size(175, 355);
            this.AttacksList.TabIndex = 1;
            this.AttacksList.SelectedIndexChanged += new System.EventHandler(this.AttacksList_SelectedIndexChanged);
            // 
            // RemoveButton
            // 
            this.RemoveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RemoveButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.RemoveButton.Location = new System.Drawing.Point(24, 476);
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Size = new System.Drawing.Size(75, 23);
            this.RemoveButton.TabIndex = 2;
            this.RemoveButton.Text = "Remove";
            this.RemoveButton.UseVisualStyleBackColor = true;
            this.RemoveButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // AddNew
            // 
            this.AddNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddNew.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.AddNew.Location = new System.Drawing.Point(110, 476);
            this.AddNew.Name = "AddNew";
            this.AddNew.Size = new System.Drawing.Size(75, 23);
            this.AddNew.TabIndex = 2;
            this.AddNew.Text = "Add New";
            this.AddNew.UseVisualStyleBackColor = true;
            this.AddNew.Click += new System.EventHandler(this.AddNew_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SaveButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.SaveButton.Location = new System.Drawing.Point(63, 505);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 23);
            this.SaveButton.TabIndex = 2;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // ClosePanel
            // 
            this.ClosePanel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ClosePanel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClosePanel.Location = new System.Drawing.Point(682, 2);
            this.ClosePanel.Name = "ClosePanel";
            this.ClosePanel.Size = new System.Drawing.Size(27, 26);
            this.ClosePanel.TabIndex = 2;
            this.ClosePanel.Text = "X";
            this.ClosePanel.UseVisualStyleBackColor = true;
            this.ClosePanel.Click += new System.EventHandler(this.ClosePanel_Click);
            // 
            // AttackInfo
            // 
            this.AttackInfo.Controls.Add(this.RemoveDelayEffect);
            this.AttackInfo.Controls.Add(this.AddDelayEffect);
            this.AttackInfo.Controls.Add(this.OnDelaySpecialEffectsList);
            this.AttackInfo.Controls.Add(this.label14);
            this.AttackInfo.Controls.Add(this.OnlyOnceInAir);
            this.AttackInfo.Controls.Add(this.GroundCancelCheckbox);
            this.AttackInfo.Controls.Add(this.AirOnlyCheckbox);
            this.AttackInfo.Controls.Add(this.AttackName);
            this.AttackInfo.Controls.Add(this.TargetAirLockNumeric);
            this.AttackInfo.Controls.Add(this.AirLockNumeric);
            this.AttackInfo.Controls.Add(this.LocalCooldownNumeric);
            this.AttackInfo.Controls.Add(this.MultiHitDelayNU);
            this.AttackInfo.Controls.Add(this.CooldownNumeric);
            this.AttackInfo.Controls.Add(this.label11);
            this.AttackInfo.Controls.Add(this.AirFactorNU);
            this.AttackInfo.Controls.Add(this.RemoveOnGround);
            this.AttackInfo.Controls.Add(this.RemoveOnHit);
            this.AttackInfo.Controls.Add(this.label12);
            this.AttackInfo.Controls.Add(this.StunLockNumeric);
            this.AttackInfo.Controls.Add(this.AddOnGround);
            this.AttackInfo.Controls.Add(this.AddOnHit);
            this.AttackInfo.Controls.Add(this.RemoveSpecial);
            this.AttackInfo.Controls.Add(this.AddSpecial);
            this.AttackInfo.Controls.Add(this.DelayNumeric);
            this.AttackInfo.Controls.Add(this.OnGroundCancelSpecialEffectList);
            this.AttackInfo.Controls.Add(this.OnHitSpecialEffectsList);
            this.AttackInfo.Controls.Add(this.OnDurationSpecialEffectsList);
            this.AttackInfo.Controls.Add(this.DurationNumeric);
            this.AttackInfo.Controls.Add(this.DamageNumeric);
            this.AttackInfo.Controls.Add(this.EffectsInfoPanel);
            this.AttackInfo.Controls.Add(this.HitboxesPanel);
            this.AttackInfo.Controls.Add(this.label8);
            this.AttackInfo.Controls.Add(this.label7);
            this.AttackInfo.Controls.Add(this.LocalCooldown);
            this.AttackInfo.Controls.Add(this.label13);
            this.AttackInfo.Controls.Add(this.label6);
            this.AttackInfo.Controls.Add(this.label111);
            this.AttackInfo.Controls.Add(this.label5);
            this.AttackInfo.Controls.Add(this.label10);
            this.AttackInfo.Controls.Add(this.label4);
            this.AttackInfo.Controls.Add(this.label9);
            this.AttackInfo.Controls.Add(this.label3);
            this.AttackInfo.Controls.Add(this.label2);
            this.AttackInfo.Controls.Add(this.Element);
            this.AttackInfo.Controls.Add(this.label1);
            this.AttackInfo.Controls.Add(this.ElementCombobox);
            this.AttackInfo.Controls.Add(this.TypeComboBox);
            this.AttackInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.AttackInfo.Location = new System.Drawing.Point(201, 49);
            this.AttackInfo.Name = "AttackInfo";
            this.AttackInfo.Size = new System.Drawing.Size(508, 524);
            this.AttackInfo.TabIndex = 3;
            this.AttackInfo.TabStop = false;
            this.AttackInfo.Text = "Attack Info";
            // 
            // RemoveDelayEffect
            // 
            this.RemoveDelayEffect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RemoveDelayEffect.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.RemoveDelayEffect.Location = new System.Drawing.Point(137, 302);
            this.RemoveDelayEffect.Name = "RemoveDelayEffect";
            this.RemoveDelayEffect.Size = new System.Drawing.Size(31, 23);
            this.RemoveDelayEffect.TabIndex = 8;
            this.RemoveDelayEffect.Text = "-";
            this.RemoveDelayEffect.UseVisualStyleBackColor = true;
            this.RemoveDelayEffect.Click += new System.EventHandler(this.RemoveDelayEffect_Click);
            // 
            // AddDelayEffect
            // 
            this.AddDelayEffect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddDelayEffect.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.AddDelayEffect.Location = new System.Drawing.Point(137, 274);
            this.AddDelayEffect.Name = "AddDelayEffect";
            this.AddDelayEffect.Size = new System.Drawing.Size(31, 23);
            this.AddDelayEffect.TabIndex = 9;
            this.AddDelayEffect.Text = "+";
            this.AddDelayEffect.UseVisualStyleBackColor = true;
            this.AddDelayEffect.Click += new System.EventHandler(this.AddDelayEffect_Click);
            // 
            // OnDelaySpecialEffectsList
            // 
            this.OnDelaySpecialEffectsList.FormattingEnabled = true;
            this.OnDelaySpecialEffectsList.Location = new System.Drawing.Point(18, 278);
            this.OnDelaySpecialEffectsList.Name = "OnDelaySpecialEffectsList";
            this.OnDelaySpecialEffectsList.Size = new System.Drawing.Size(113, 43);
            this.OnDelaySpecialEffectsList.TabIndex = 6;
            this.OnDelaySpecialEffectsList.SelectedIndexChanged += new System.EventHandler(this.OnDelaySpecialEffectsList_SelectedIndexChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(16, 262);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(122, 13);
            this.label14.TabIndex = 7;
            this.label14.Text = "On Delay SpecialEffects";
            // 
            // OnlyOnceInAir
            // 
            this.OnlyOnceInAir.AutoSize = true;
            this.OnlyOnceInAir.Location = new System.Drawing.Point(378, 159);
            this.OnlyOnceInAir.Name = "OnlyOnceInAir";
            this.OnlyOnceInAir.Size = new System.Drawing.Size(78, 17);
            this.OnlyOnceInAir.TabIndex = 5;
            this.OnlyOnceInAir.Text = "Once in Air";
            this.OnlyOnceInAir.UseVisualStyleBackColor = true;
            this.OnlyOnceInAir.CheckedChanged += new System.EventHandler(this.OnlyOnceInAir_CheckedChanged);
            // 
            // GroundCancelCheckbox
            // 
            this.GroundCancelCheckbox.AutoSize = true;
            this.GroundCancelCheckbox.Location = new System.Drawing.Point(378, 138);
            this.GroundCancelCheckbox.Name = "GroundCancelCheckbox";
            this.GroundCancelCheckbox.Size = new System.Drawing.Size(94, 17);
            this.GroundCancelCheckbox.TabIndex = 5;
            this.GroundCancelCheckbox.Text = "GroundCancel";
            this.GroundCancelCheckbox.UseVisualStyleBackColor = true;
            this.GroundCancelCheckbox.CheckedChanged += new System.EventHandler(this.GroundCancelCheckbox_CheckedChanged);
            // 
            // AirOnlyCheckbox
            // 
            this.AirOnlyCheckbox.AutoSize = true;
            this.AirOnlyCheckbox.Location = new System.Drawing.Point(378, 117);
            this.AirOnlyCheckbox.Name = "AirOnlyCheckbox";
            this.AirOnlyCheckbox.Size = new System.Drawing.Size(59, 17);
            this.AirOnlyCheckbox.TabIndex = 5;
            this.AirOnlyCheckbox.Text = "AirOnly";
            this.AirOnlyCheckbox.UseVisualStyleBackColor = true;
            this.AirOnlyCheckbox.CheckedChanged += new System.EventHandler(this.AirOnlyCheckbox_CheckedChanged);
            // 
            // AttackName
            // 
            this.AttackName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.AttackName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.AttackName.Font = new System.Drawing.Font("Agency FB", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AttackName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.AttackName.Location = new System.Drawing.Point(123, 19);
            this.AttackName.Name = "AttackName";
            this.AttackName.Size = new System.Drawing.Size(286, 36);
            this.AttackName.TabIndex = 4;
            this.AttackName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.AttackName.Enter += new System.EventHandler(this.AttackName_Enter);
            this.AttackName.Leave += new System.EventHandler(this.AttackName_Leave);
            // 
            // TargetAirLockNumeric
            // 
            this.TargetAirLockNumeric.DecimalPlaces = 2;
            this.TargetAirLockNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.TargetAirLockNumeric.Location = new System.Drawing.Point(279, 135);
            this.TargetAirLockNumeric.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.TargetAirLockNumeric.Name = "TargetAirLockNumeric";
            this.TargetAirLockNumeric.Size = new System.Drawing.Size(60, 20);
            this.TargetAirLockNumeric.TabIndex = 3;
            this.TargetAirLockNumeric.ValueChanged += new System.EventHandler(this.Numeric_ValueChanged);
            this.TargetAirLockNumeric.Enter += new System.EventHandler(this.Numeric_Enter);
            this.TargetAirLockNumeric.Leave += new System.EventHandler(this.Numeric_Leave);
            // 
            // AirLockNumeric
            // 
            this.AirLockNumeric.DecimalPlaces = 2;
            this.AirLockNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.AirLockNumeric.Location = new System.Drawing.Point(211, 135);
            this.AirLockNumeric.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.AirLockNumeric.Name = "AirLockNumeric";
            this.AirLockNumeric.Size = new System.Drawing.Size(60, 20);
            this.AirLockNumeric.TabIndex = 3;
            this.AirLockNumeric.ValueChanged += new System.EventHandler(this.Numeric_ValueChanged);
            this.AirLockNumeric.Enter += new System.EventHandler(this.Numeric_Enter);
            this.AirLockNumeric.Leave += new System.EventHandler(this.Numeric_Leave);
            // 
            // LocalCooldownNumeric
            // 
            this.LocalCooldownNumeric.DecimalPlaces = 2;
            this.LocalCooldownNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.LocalCooldownNumeric.Location = new System.Drawing.Point(438, 84);
            this.LocalCooldownNumeric.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.LocalCooldownNumeric.Name = "LocalCooldownNumeric";
            this.LocalCooldownNumeric.Size = new System.Drawing.Size(60, 20);
            this.LocalCooldownNumeric.TabIndex = 3;
            this.LocalCooldownNumeric.ValueChanged += new System.EventHandler(this.Numeric_ValueChanged);
            this.LocalCooldownNumeric.Enter += new System.EventHandler(this.Numeric_Enter);
            this.LocalCooldownNumeric.Leave += new System.EventHandler(this.Numeric_Leave);
            // 
            // MultiHitDelayNU
            // 
            this.MultiHitDelayNU.DecimalPlaces = 2;
            this.MultiHitDelayNU.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.MultiHitDelayNU.Location = new System.Drawing.Point(12, 135);
            this.MultiHitDelayNU.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.MultiHitDelayNU.Name = "MultiHitDelayNU";
            this.MultiHitDelayNU.Size = new System.Drawing.Size(60, 20);
            this.MultiHitDelayNU.TabIndex = 3;
            this.MultiHitDelayNU.ValueChanged += new System.EventHandler(this.Numeric_ValueChanged);
            this.MultiHitDelayNU.Enter += new System.EventHandler(this.Numeric_Enter);
            this.MultiHitDelayNU.Leave += new System.EventHandler(this.Numeric_Leave);
            // 
            // CooldownNumeric
            // 
            this.CooldownNumeric.DecimalPlaces = 2;
            this.CooldownNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.CooldownNumeric.Location = new System.Drawing.Point(373, 85);
            this.CooldownNumeric.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.CooldownNumeric.Name = "CooldownNumeric";
            this.CooldownNumeric.Size = new System.Drawing.Size(60, 20);
            this.CooldownNumeric.TabIndex = 3;
            this.CooldownNumeric.ValueChanged += new System.EventHandler(this.Numeric_ValueChanged);
            this.CooldownNumeric.Enter += new System.EventHandler(this.Numeric_Enter);
            this.CooldownNumeric.Leave += new System.EventHandler(this.Numeric_Leave);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(76, 119);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(49, 13);
            this.label11.TabIndex = 1;
            this.label11.Text = "AirFactor";
            // 
            // AirFactorNU
            // 
            this.AirFactorNU.DecimalPlaces = 2;
            this.AirFactorNU.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.AirFactorNU.Location = new System.Drawing.Point(78, 135);
            this.AirFactorNU.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.AirFactorNU.Name = "AirFactorNU";
            this.AirFactorNU.Size = new System.Drawing.Size(60, 20);
            this.AirFactorNU.TabIndex = 3;
            this.AirFactorNU.ValueChanged += new System.EventHandler(this.Numeric_ValueChanged);
            this.AirFactorNU.Enter += new System.EventHandler(this.Numeric_Enter);
            this.AirFactorNU.Leave += new System.EventHandler(this.Numeric_Leave);
            // 
            // RemoveOnGround
            // 
            this.RemoveOnGround.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RemoveOnGround.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.RemoveOnGround.Location = new System.Drawing.Point(136, 490);
            this.RemoveOnGround.Name = "RemoveOnGround";
            this.RemoveOnGround.Size = new System.Drawing.Size(31, 23);
            this.RemoveOnGround.TabIndex = 2;
            this.RemoveOnGround.Text = "-";
            this.RemoveOnGround.UseVisualStyleBackColor = true;
            this.RemoveOnGround.Click += new System.EventHandler(this.RemoveOnGround_Click);
            // 
            // RemoveOnHit
            // 
            this.RemoveOnHit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RemoveOnHit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.RemoveOnHit.Location = new System.Drawing.Point(137, 429);
            this.RemoveOnHit.Name = "RemoveOnHit";
            this.RemoveOnHit.Size = new System.Drawing.Size(31, 23);
            this.RemoveOnHit.TabIndex = 2;
            this.RemoveOnHit.Text = "-";
            this.RemoveOnHit.UseVisualStyleBackColor = true;
            this.RemoveOnHit.Click += new System.EventHandler(this.RemoveOnHit_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(141, 118);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(53, 13);
            this.label12.TabIndex = 1;
            this.label12.Text = "StunLock";
            // 
            // StunLockNumeric
            // 
            this.StunLockNumeric.DecimalPlaces = 2;
            this.StunLockNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.StunLockNumeric.Location = new System.Drawing.Point(144, 135);
            this.StunLockNumeric.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.StunLockNumeric.Name = "StunLockNumeric";
            this.StunLockNumeric.Size = new System.Drawing.Size(60, 20);
            this.StunLockNumeric.TabIndex = 3;
            this.StunLockNumeric.ValueChanged += new System.EventHandler(this.Numeric_ValueChanged);
            this.StunLockNumeric.Enter += new System.EventHandler(this.Numeric_Enter);
            this.StunLockNumeric.Leave += new System.EventHandler(this.Numeric_Leave);
            // 
            // AddOnGround
            // 
            this.AddOnGround.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddOnGround.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.AddOnGround.Location = new System.Drawing.Point(136, 463);
            this.AddOnGround.Name = "AddOnGround";
            this.AddOnGround.Size = new System.Drawing.Size(31, 23);
            this.AddOnGround.TabIndex = 2;
            this.AddOnGround.Text = "+";
            this.AddOnGround.UseVisualStyleBackColor = true;
            this.AddOnGround.Click += new System.EventHandler(this.AddOnGround_Click);
            // 
            // AddOnHit
            // 
            this.AddOnHit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddOnHit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.AddOnHit.Location = new System.Drawing.Point(137, 401);
            this.AddOnHit.Name = "AddOnHit";
            this.AddOnHit.Size = new System.Drawing.Size(31, 23);
            this.AddOnHit.TabIndex = 2;
            this.AddOnHit.Text = "+";
            this.AddOnHit.UseVisualStyleBackColor = true;
            this.AddOnHit.Click += new System.EventHandler(this.AddOnHit_Click);
            // 
            // RemoveSpecial
            // 
            this.RemoveSpecial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RemoveSpecial.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.RemoveSpecial.Location = new System.Drawing.Point(137, 364);
            this.RemoveSpecial.Name = "RemoveSpecial";
            this.RemoveSpecial.Size = new System.Drawing.Size(31, 23);
            this.RemoveSpecial.TabIndex = 2;
            this.RemoveSpecial.Text = "-";
            this.RemoveSpecial.UseVisualStyleBackColor = true;
            this.RemoveSpecial.Click += new System.EventHandler(this.RemoveSpecial_Click);
            // 
            // AddSpecial
            // 
            this.AddSpecial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddSpecial.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.AddSpecial.Location = new System.Drawing.Point(137, 336);
            this.AddSpecial.Name = "AddSpecial";
            this.AddSpecial.Size = new System.Drawing.Size(31, 23);
            this.AddSpecial.TabIndex = 2;
            this.AddSpecial.Text = "+";
            this.AddSpecial.UseVisualStyleBackColor = true;
            this.AddSpecial.Click += new System.EventHandler(this.AddSpecial_Click);
            // 
            // DelayNumeric
            // 
            this.DelayNumeric.DecimalPlaces = 2;
            this.DelayNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.DelayNumeric.Location = new System.Drawing.Point(242, 85);
            this.DelayNumeric.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.DelayNumeric.Name = "DelayNumeric";
            this.DelayNumeric.Size = new System.Drawing.Size(60, 20);
            this.DelayNumeric.TabIndex = 3;
            this.DelayNumeric.ValueChanged += new System.EventHandler(this.Numeric_ValueChanged);
            this.DelayNumeric.Enter += new System.EventHandler(this.Numeric_Enter);
            this.DelayNumeric.Leave += new System.EventHandler(this.Numeric_Leave);
            // 
            // OnGroundCancelSpecialEffectList
            // 
            this.OnGroundCancelSpecialEffectList.FormattingEnabled = true;
            this.OnGroundCancelSpecialEffectList.Location = new System.Drawing.Point(18, 467);
            this.OnGroundCancelSpecialEffectList.Name = "OnGroundCancelSpecialEffectList";
            this.OnGroundCancelSpecialEffectList.Size = new System.Drawing.Size(112, 43);
            this.OnGroundCancelSpecialEffectList.TabIndex = 1;
            this.OnGroundCancelSpecialEffectList.SelectedIndexChanged += new System.EventHandler(this.OnGroundCancelSpecialEffectsList_SelectedIndexChanged);
            // 
            // OnHitSpecialEffectsList
            // 
            this.OnHitSpecialEffectsList.FormattingEnabled = true;
            this.OnHitSpecialEffectsList.Location = new System.Drawing.Point(19, 404);
            this.OnHitSpecialEffectsList.Name = "OnHitSpecialEffectsList";
            this.OnHitSpecialEffectsList.Size = new System.Drawing.Size(112, 43);
            this.OnHitSpecialEffectsList.TabIndex = 1;
            this.OnHitSpecialEffectsList.SelectedIndexChanged += new System.EventHandler(this.OnHitSpecialEffectsList_SelectedIndexChanged);
            // 
            // OnDurationSpecialEffectsList
            // 
            this.OnDurationSpecialEffectsList.FormattingEnabled = true;
            this.OnDurationSpecialEffectsList.Location = new System.Drawing.Point(18, 340);
            this.OnDurationSpecialEffectsList.Name = "OnDurationSpecialEffectsList";
            this.OnDurationSpecialEffectsList.Size = new System.Drawing.Size(113, 43);
            this.OnDurationSpecialEffectsList.TabIndex = 1;
            this.OnDurationSpecialEffectsList.SelectedIndexChanged += new System.EventHandler(this.SpecialEffectsList_SelectedIndexChanged);
            // 
            // DurationNumeric
            // 
            this.DurationNumeric.DecimalPlaces = 2;
            this.DurationNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.DurationNumeric.Location = new System.Drawing.Point(308, 85);
            this.DurationNumeric.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.DurationNumeric.Name = "DurationNumeric";
            this.DurationNumeric.Size = new System.Drawing.Size(60, 20);
            this.DurationNumeric.TabIndex = 3;
            this.DurationNumeric.ValueChanged += new System.EventHandler(this.Numeric_ValueChanged);
            this.DurationNumeric.Enter += new System.EventHandler(this.Numeric_Enter);
            this.DurationNumeric.Leave += new System.EventHandler(this.Numeric_Leave);
            // 
            // DamageNumeric
            // 
            this.DamageNumeric.Location = new System.Drawing.Point(164, 85);
            this.DamageNumeric.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.DamageNumeric.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.DamageNumeric.Name = "DamageNumeric";
            this.DamageNumeric.Size = new System.Drawing.Size(71, 20);
            this.DamageNumeric.TabIndex = 3;
            this.DamageNumeric.ValueChanged += new System.EventHandler(this.Numeric_ValueChanged);
            this.DamageNumeric.Enter += new System.EventHandler(this.Numeric_Enter);
            this.DamageNumeric.Leave += new System.EventHandler(this.Numeric_Leave);
            // 
            // EffectsInfoPanel
            // 
            this.EffectsInfoPanel.AutoScroll = true;
            this.EffectsInfoPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.EffectsInfoPanel.Location = new System.Drawing.Point(174, 258);
            this.EffectsInfoPanel.Name = "EffectsInfoPanel";
            this.EffectsInfoPanel.Size = new System.Drawing.Size(326, 260);
            this.EffectsInfoPanel.TabIndex = 2;
            // 
            // HitboxesPanel
            // 
            this.HitboxesPanel.AutoScroll = true;
            this.HitboxesPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.HitboxesPanel.Location = new System.Drawing.Point(12, 182);
            this.HitboxesPanel.Name = "HitboxesPanel";
            this.HitboxesPanel.Size = new System.Drawing.Size(490, 70);
            this.HitboxesPanel.TabIndex = 2;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(276, 119);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(74, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "TargetAirLock";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(208, 119);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(43, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "AirLock";
            // 
            // LocalCooldown
            // 
            this.LocalCooldown.AutoSize = true;
            this.LocalCooldown.Location = new System.Drawing.Point(426, 67);
            this.LocalCooldown.Name = "LocalCooldown";
            this.LocalCooldown.Size = new System.Drawing.Size(80, 13);
            this.LocalCooldown.TabIndex = 1;
            this.LocalCooldown.Text = "LocalCooldown";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(9, 118);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(69, 13);
            this.label13.TabIndex = 1;
            this.label13.Text = "MultiHitDelay";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(369, 68);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(54, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Cooldown";
            // 
            // label111
            // 
            this.label111.AutoSize = true;
            this.label111.Location = new System.Drawing.Point(14, 451);
            this.label111.Name = "label111";
            this.label111.Size = new System.Drawing.Size(124, 13);
            this.label111.TabIndex = 1;
            this.label111.Text = "SpecialEffectsOnGround";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(305, 67);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Duration";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(15, 388);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(102, 13);
            this.label10.TabIndex = 1;
            this.label10.Text = "SpecialEffectsOnHit";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(239, 68);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Delay";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(16, 324);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(135, 13);
            this.label9.TabIndex = 1;
            this.label9.Text = "On Duration SpecialEffects";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(161, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "DamageOnHit";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 166);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Hitboxes";
            // 
            // Element
            // 
            this.Element.AutoSize = true;
            this.Element.Location = new System.Drawing.Point(83, 67);
            this.Element.Name = "Element";
            this.Element.Size = new System.Drawing.Size(45, 13);
            this.Element.TabIndex = 1;
            this.Element.Text = "Element";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Type";
            // 
            // ElementCombobox
            // 
            this.ElementCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ElementCombobox.FormattingEnabled = true;
            this.ElementCombobox.Location = new System.Drawing.Point(86, 84);
            this.ElementCombobox.Name = "ElementCombobox";
            this.ElementCombobox.Size = new System.Drawing.Size(71, 21);
            this.ElementCombobox.TabIndex = 0;
            this.ElementCombobox.SelectedIndexChanged += new System.EventHandler(this.ElementComboBox_SelectedIndexChanged);
            // 
            // TypeComboBox
            // 
            this.TypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TypeComboBox.FormattingEnabled = true;
            this.TypeComboBox.Location = new System.Drawing.Point(9, 84);
            this.TypeComboBox.Name = "TypeComboBox";
            this.TypeComboBox.Size = new System.Drawing.Size(72, 21);
            this.TypeComboBox.TabIndex = 0;
            this.TypeComboBox.SelectedIndexChanged += new System.EventHandler(this.TypeComboBox_SelectedIndexChanged);
            // 
            // AttacksSettingsManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.AttackInfo);
            this.Controls.Add(this.ClosePanel);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.AddNew);
            this.Controls.Add(this.RemoveButton);
            this.Controls.Add(this.AttacksList);
            this.Controls.Add(this.Title);
            this.Name = "AttacksSettingsManager";
            this.Size = new System.Drawing.Size(712, 579);
            this.AttackInfo.ResumeLayout(false);
            this.AttackInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TargetAirLockNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AirLockNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LocalCooldownNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MultiHitDelayNU)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CooldownNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AirFactorNU)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StunLockNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DelayNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DurationNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DamageNumeric)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.ListBox AttacksList;
        private System.Windows.Forms.Button RemoveButton;
        private System.Windows.Forms.Button AddNew;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button ClosePanel;
        private System.Windows.Forms.GroupBox AttackInfo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox TypeComboBox;
        private System.Windows.Forms.NumericUpDown DamageNumeric;
        private System.Windows.Forms.Panel EffectsInfoPanel;
        private System.Windows.Forms.Panel HitboxesPanel;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown AirLockNumeric;
        private System.Windows.Forms.NumericUpDown CooldownNumeric;
        private System.Windows.Forms.NumericUpDown DelayNumeric;
        private System.Windows.Forms.NumericUpDown DurationNumeric;
        private System.Windows.Forms.NumericUpDown TargetAirLockNumeric;
        private System.Windows.Forms.TextBox AttackName;
        private System.Windows.Forms.ListBox OnHitSpecialEffectsList;
        private System.Windows.Forms.ListBox OnDurationSpecialEffectsList;
        private System.Windows.Forms.Button AddSpecial;
        private System.Windows.Forms.Button RemoveOnHit;
        private System.Windows.Forms.Button AddOnHit;
        private System.Windows.Forms.Button RemoveSpecial;
        private System.Windows.Forms.CheckBox GroundCancelCheckbox;
        private System.Windows.Forms.CheckBox AirOnlyCheckbox;
        private System.Windows.Forms.CheckBox OnlyOnceInAir;
        private System.Windows.Forms.Button RemoveOnGround;
        private System.Windows.Forms.Button AddOnGround;
        private System.Windows.Forms.ListBox OnGroundCancelSpecialEffectList;
        private System.Windows.Forms.Label label111;
        private System.Windows.Forms.NumericUpDown AirFactorNU;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown StunLockNumeric;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown LocalCooldownNumeric;
        private System.Windows.Forms.Label LocalCooldown;
        private System.Windows.Forms.Label Element;
        private System.Windows.Forms.ComboBox ElementCombobox;
        private System.Windows.Forms.NumericUpDown MultiHitDelayNU;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button RemoveDelayEffect;
        private System.Windows.Forms.Button AddDelayEffect;
        private System.Windows.Forms.ListBox OnDelaySpecialEffectsList;
        private System.Windows.Forms.Label label14;
    }
}
