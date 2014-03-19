namespace NeonStarEditor
{
    partial class WindPanel
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
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.AttackToLaunch = new System.Windows.Forms.TextBox();
            this.TimedGaugeComsuption = new System.Windows.Forms.NumericUpDown();
            this.GaugeCost = new System.Windows.Forms.NumericUpDown();
            this.ImpulseDuration = new System.Windows.Forms.NumericUpDown();
            this.AirVerticalMaxVelocity = new System.Windows.Forms.NumericUpDown();
            this.AirVerticalVelocity = new System.Windows.Forms.NumericUpDown();
            this.AirControlSpeed = new System.Windows.Forms.NumericUpDown();
            this.AirVerticalImpulse = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TimedGaugeComsuption)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GaugeCost)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImpulseDuration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AirVerticalMaxVelocity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AirVerticalVelocity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AirControlSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AirVerticalImpulse)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Agency FB", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(274, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 28);
            this.label2.TabIndex = 2;
            this.label2.Text = "Wind";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.AttackToLaunch);
            this.groupBox1.Controls.Add(this.TimedGaugeComsuption);
            this.groupBox1.Controls.Add(this.GaugeCost);
            this.groupBox1.Controls.Add(this.ImpulseDuration);
            this.groupBox1.Controls.Add(this.AirVerticalMaxVelocity);
            this.groupBox1.Controls.Add(this.AirVerticalVelocity);
            this.groupBox1.Controls.Add(this.AirControlSpeed);
            this.groupBox1.Controls.Add(this.AirVerticalImpulse);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(15, 42);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(601, 153);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Common Settings";
            // 
            // AttackToLaunch
            // 
            this.AttackToLaunch.Location = new System.Drawing.Point(496, 15);
            this.AttackToLaunch.Name = "AttackToLaunch";
            this.AttackToLaunch.Size = new System.Drawing.Size(99, 20);
            this.AttackToLaunch.TabIndex = 2;
            this.AttackToLaunch.Enter += new System.EventHandler(this.textBox_Enter);
            this.AttackToLaunch.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // TimedGaugeComsuption
            // 
            this.TimedGaugeComsuption.Location = new System.Drawing.Point(141, 48);
            this.TimedGaugeComsuption.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.TimedGaugeComsuption.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.TimedGaugeComsuption.Name = "TimedGaugeComsuption";
            this.TimedGaugeComsuption.Size = new System.Drawing.Size(53, 20);
            this.TimedGaugeComsuption.TabIndex = 1;
            this.TimedGaugeComsuption.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
            this.TimedGaugeComsuption.Enter += new System.EventHandler(this.numericUpDown_Enter);
            this.TimedGaugeComsuption.Leave += new System.EventHandler(this.numericUpDown_Leave);
            // 
            // GaugeCost
            // 
            this.GaugeCost.Location = new System.Drawing.Point(142, 16);
            this.GaugeCost.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.GaugeCost.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.GaugeCost.Name = "GaugeCost";
            this.GaugeCost.Size = new System.Drawing.Size(53, 20);
            this.GaugeCost.TabIndex = 1;
            this.GaugeCost.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
            this.GaugeCost.Enter += new System.EventHandler(this.numericUpDown_Enter);
            this.GaugeCost.Leave += new System.EventHandler(this.numericUpDown_Leave);
            // 
            // ImpulseDuration
            // 
            this.ImpulseDuration.DecimalPlaces = 2;
            this.ImpulseDuration.Location = new System.Drawing.Point(342, 16);
            this.ImpulseDuration.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.ImpulseDuration.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.ImpulseDuration.Name = "ImpulseDuration";
            this.ImpulseDuration.Size = new System.Drawing.Size(53, 20);
            this.ImpulseDuration.TabIndex = 1;
            this.ImpulseDuration.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
            this.ImpulseDuration.Enter += new System.EventHandler(this.numericUpDown_Enter);
            this.ImpulseDuration.Leave += new System.EventHandler(this.numericUpDown_Leave);
            // 
            // AirVerticalMaxVelocity
            // 
            this.AirVerticalMaxVelocity.DecimalPlaces = 2;
            this.AirVerticalMaxVelocity.Location = new System.Drawing.Point(369, 109);
            this.AirVerticalMaxVelocity.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.AirVerticalMaxVelocity.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.AirVerticalMaxVelocity.Name = "AirVerticalMaxVelocity";
            this.AirVerticalMaxVelocity.Size = new System.Drawing.Size(53, 20);
            this.AirVerticalMaxVelocity.TabIndex = 1;
            this.AirVerticalMaxVelocity.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
            this.AirVerticalMaxVelocity.Enter += new System.EventHandler(this.numericUpDown_Enter);
            this.AirVerticalMaxVelocity.Leave += new System.EventHandler(this.numericUpDown_Leave);
            // 
            // AirVerticalVelocity
            // 
            this.AirVerticalVelocity.DecimalPlaces = 2;
            this.AirVerticalVelocity.Location = new System.Drawing.Point(369, 79);
            this.AirVerticalVelocity.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.AirVerticalVelocity.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.AirVerticalVelocity.Name = "AirVerticalVelocity";
            this.AirVerticalVelocity.Size = new System.Drawing.Size(53, 20);
            this.AirVerticalVelocity.TabIndex = 1;
            this.AirVerticalVelocity.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
            this.AirVerticalVelocity.Enter += new System.EventHandler(this.numericUpDown_Enter);
            this.AirVerticalVelocity.Leave += new System.EventHandler(this.numericUpDown_Leave);
            // 
            // AirControlSpeed
            // 
            this.AirControlSpeed.DecimalPlaces = 2;
            this.AirControlSpeed.Location = new System.Drawing.Point(342, 48);
            this.AirControlSpeed.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.AirControlSpeed.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.AirControlSpeed.Name = "AirControlSpeed";
            this.AirControlSpeed.Size = new System.Drawing.Size(53, 20);
            this.AirControlSpeed.TabIndex = 1;
            this.AirControlSpeed.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
            this.AirControlSpeed.Enter += new System.EventHandler(this.numericUpDown_Enter);
            this.AirControlSpeed.Leave += new System.EventHandler(this.numericUpDown_Leave);
            // 
            // AirVerticalImpulse
            // 
            this.AirVerticalImpulse.Location = new System.Drawing.Point(141, 79);
            this.AirVerticalImpulse.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.AirVerticalImpulse.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.AirVerticalImpulse.Name = "AirVerticalImpulse";
            this.AirVerticalImpulse.Size = new System.Drawing.Size(53, 20);
            this.AirVerticalImpulse.TabIndex = 1;
            this.AirVerticalImpulse.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
            this.AirVerticalImpulse.Enter += new System.EventHandler(this.numericUpDown_Enter);
            this.AirVerticalImpulse.Leave += new System.EventHandler(this.numericUpDown_Leave);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(205, 111);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(157, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Air Vertical Charge Max Velocity";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(206, 81);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(156, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Air Vertical Charge Acceleration";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(206, 48);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(89, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "Air Control Speed";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Air Vertical Impulse";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(206, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Impulse Duration";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(398, 18);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(93, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Attack To Launch";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 50);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(129, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Timed Gauge Comsuption";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Gauge Cost";
            // 
            // WindPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.Name = "WindPanel";
            this.Size = new System.Drawing.Size(630, 540);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TimedGaugeComsuption)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GaugeCost)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImpulseDuration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AirVerticalMaxVelocity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AirVerticalVelocity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AirControlSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AirVerticalImpulse)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox AttackToLaunch;
        private System.Windows.Forms.NumericUpDown GaugeCost;
        private System.Windows.Forms.NumericUpDown ImpulseDuration;
        private System.Windows.Forms.NumericUpDown AirVerticalImpulse;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown TimedGaugeComsuption;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown AirVerticalMaxVelocity;
        private System.Windows.Forms.NumericUpDown AirVerticalVelocity;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown AirControlSpeed;
        private System.Windows.Forms.Label label9;

    }
}
