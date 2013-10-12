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
            this.TargetAirLockNumeric = new System.Windows.Forms.NumericUpDown();
            this.AirLockNumeric = new System.Windows.Forms.NumericUpDown();
            this.CooldownNumeric = new System.Windows.Forms.NumericUpDown();
            this.DelayNumeric = new System.Windows.Forms.NumericUpDown();
            this.DurationNumeric = new System.Windows.Forms.NumericUpDown();
            this.DamageNumeric = new System.Windows.Forms.NumericUpDown();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.TypeComboBox = new System.Windows.Forms.ComboBox();
            this.AttackName = new System.Windows.Forms.TextBox();
            this.AttackInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TargetAirLockNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AirLockNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CooldownNumeric)).BeginInit();
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
            this.Title.Location = new System.Drawing.Point(214, 12);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(155, 34);
            this.Title.TabIndex = 0;
            this.Title.Text = "Attacks Manager";
            // 
            // AttacksList
            // 
            this.AttacksList.FormattingEnabled = true;
            this.AttacksList.Location = new System.Drawing.Point(17, 68);
            this.AttacksList.Name = "AttacksList";
            this.AttacksList.Size = new System.Drawing.Size(175, 355);
            this.AttacksList.TabIndex = 1;
            this.AttacksList.SelectedIndexChanged += new System.EventHandler(this.AttacksList_SelectedIndexChanged);
            // 
            // RemoveButton
            // 
            this.RemoveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RemoveButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.RemoveButton.Location = new System.Drawing.Point(24, 445);
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
            this.AddNew.Location = new System.Drawing.Point(110, 445);
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
            this.SaveButton.Location = new System.Drawing.Point(63, 474);
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
            this.ClosePanel.Location = new System.Drawing.Point(543, 3);
            this.ClosePanel.Name = "ClosePanel";
            this.ClosePanel.Size = new System.Drawing.Size(27, 26);
            this.ClosePanel.TabIndex = 2;
            this.ClosePanel.Text = "X";
            this.ClosePanel.UseVisualStyleBackColor = true;
            this.ClosePanel.Click += new System.EventHandler(this.ClosePanel_Click);
            // 
            // AttackInfo
            // 
            this.AttackInfo.Controls.Add(this.AttackName);
            this.AttackInfo.Controls.Add(this.TargetAirLockNumeric);
            this.AttackInfo.Controls.Add(this.AirLockNumeric);
            this.AttackInfo.Controls.Add(this.CooldownNumeric);
            this.AttackInfo.Controls.Add(this.DelayNumeric);
            this.AttackInfo.Controls.Add(this.DurationNumeric);
            this.AttackInfo.Controls.Add(this.DamageNumeric);
            this.AttackInfo.Controls.Add(this.panel3);
            this.AttackInfo.Controls.Add(this.panel2);
            this.AttackInfo.Controls.Add(this.panel1);
            this.AttackInfo.Controls.Add(this.label8);
            this.AttackInfo.Controls.Add(this.label7);
            this.AttackInfo.Controls.Add(this.label6);
            this.AttackInfo.Controls.Add(this.label5);
            this.AttackInfo.Controls.Add(this.label10);
            this.AttackInfo.Controls.Add(this.label4);
            this.AttackInfo.Controls.Add(this.label9);
            this.AttackInfo.Controls.Add(this.label3);
            this.AttackInfo.Controls.Add(this.label2);
            this.AttackInfo.Controls.Add(this.label1);
            this.AttackInfo.Controls.Add(this.TypeComboBox);
            this.AttackInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.AttackInfo.Location = new System.Drawing.Point(201, 49);
            this.AttackInfo.Name = "AttackInfo";
            this.AttackInfo.Size = new System.Drawing.Size(362, 448);
            this.AttackInfo.TabIndex = 3;
            this.AttackInfo.TabStop = false;
            this.AttackInfo.Text = "Attack Info";
            // 
            // TargetAirLockNumeric
            // 
            this.TargetAirLockNumeric.DecimalPlaces = 2;
            this.TargetAirLockNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.TargetAirLockNumeric.Location = new System.Drawing.Point(237, 132);
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
            this.AirLockNumeric.Location = new System.Drawing.Point(162, 132);
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
            // CooldownNumeric
            // 
            this.CooldownNumeric.DecimalPlaces = 2;
            this.CooldownNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.CooldownNumeric.Location = new System.Drawing.Point(89, 132);
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
            // DelayNumeric
            // 
            this.DelayNumeric.DecimalPlaces = 2;
            this.DelayNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.DelayNumeric.Location = new System.Drawing.Point(18, 132);
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
            // DurationNumeric
            // 
            this.DurationNumeric.DecimalPlaces = 2;
            this.DurationNumeric.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.DurationNumeric.Location = new System.Drawing.Point(267, 84);
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
            this.DamageNumeric.Location = new System.Drawing.Point(162, 84);
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
            // panel3
            // 
            this.panel3.AutoScroll = true;
            this.panel3.Location = new System.Drawing.Point(12, 361);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(338, 70);
            this.panel3.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.Location = new System.Drawing.Point(12, 271);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(338, 70);
            this.panel2.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Location = new System.Drawing.Point(12, 182);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(338, 70);
            this.panel1.TabIndex = 2;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(234, 116);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(74, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "TargetAirLock";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(159, 116);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(43, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "AirLock";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(86, 116);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(54, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Cooldown";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(264, 67);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Duration";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(16, 345);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(102, 13);
            this.label10.TabIndex = 1;
            this.label10.Text = "SpecialEffectsOnHit";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 116);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Delay";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(16, 255);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(75, 13);
            this.label9.TabIndex = 1;
            this.label9.Text = "SpecialEffects";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(159, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "DamageOnHit";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 166);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Hitboxes";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Type";
            // 
            // TypeComboBox
            // 
            this.TypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TypeComboBox.FormattingEnabled = true;
            this.TypeComboBox.Location = new System.Drawing.Point(19, 84);
            this.TypeComboBox.Name = "TypeComboBox";
            this.TypeComboBox.Size = new System.Drawing.Size(121, 21);
            this.TypeComboBox.TabIndex = 0;
            this.TypeComboBox.SelectedIndexChanged += new System.EventHandler(this.TypeComboBox_SelectedIndexChanged);
            // 
            // AttackName
            // 
            this.AttackName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.AttackName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.AttackName.Font = new System.Drawing.Font("Agency FB", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AttackName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.AttackName.Location = new System.Drawing.Point(41, 19);
            this.AttackName.Name = "AttackName";
            this.AttackName.Size = new System.Drawing.Size(286, 36);
            this.AttackName.TabIndex = 4;
            this.AttackName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.AttackName.Enter += new System.EventHandler(this.AttackName_Enter);
            this.AttackName.Leave += new System.EventHandler(this.AttackName_Leave);
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
            this.Size = new System.Drawing.Size(573, 511);
            this.AttackInfo.ResumeLayout(false);
            this.AttackInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TargetAirLockNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AirLockNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CooldownNumeric)).EndInit();
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
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
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
    }
}
