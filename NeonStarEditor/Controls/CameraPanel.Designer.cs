namespace NeonStarEditor
{
    partial class CameraPanel
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
            this.ClosePanel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.MassNU = new System.Windows.Forms.NumericUpDown();
            this.DampingNU = new System.Windows.Forms.NumericUpDown();
            this.StiffnessNU = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.MassNU)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DampingNU)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StiffnessNU)).BeginInit();
            this.SuspendLayout();
            // 
            // ClosePanel
            // 
            this.ClosePanel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ClosePanel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClosePanel.Location = new System.Drawing.Point(269, 3);
            this.ClosePanel.Name = "ClosePanel";
            this.ClosePanel.Size = new System.Drawing.Size(19, 20);
            this.ClosePanel.TabIndex = 3;
            this.ClosePanel.Text = "X";
            this.ClosePanel.UseVisualStyleBackColor = true;
            this.ClosePanel.Click += new System.EventHandler(this.ClosePanel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Mass";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(109, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Damping";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(185, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Stiffness";
            // 
            // MassNU
            // 
            this.MassNU.Location = new System.Drawing.Point(36, 25);
            this.MassNU.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.MassNU.Name = "MassNU";
            this.MassNU.Size = new System.Drawing.Size(70, 20);
            this.MassNU.TabIndex = 5;
            this.MassNU.ValueChanged += new System.EventHandler(this.MassNU_ValueChanged);
            this.MassNU.Enter += new System.EventHandler(this.Numeric_Enter);
            this.MassNU.Leave += new System.EventHandler(this.Numeric_Leave);
            // 
            // DampingNU
            // 
            this.DampingNU.Location = new System.Drawing.Point(112, 25);
            this.DampingNU.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.DampingNU.Name = "DampingNU";
            this.DampingNU.Size = new System.Drawing.Size(70, 20);
            this.DampingNU.TabIndex = 5;
            this.DampingNU.ValueChanged += new System.EventHandler(this.DampingNU_ValueChanged);
            this.DampingNU.Enter += new System.EventHandler(this.Numeric_Enter);
            this.DampingNU.Leave += new System.EventHandler(this.Numeric_Leave);
            // 
            // StiffnessNU
            // 
            this.StiffnessNU.Location = new System.Drawing.Point(188, 25);
            this.StiffnessNU.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.StiffnessNU.Name = "StiffnessNU";
            this.StiffnessNU.Size = new System.Drawing.Size(70, 20);
            this.StiffnessNU.TabIndex = 5;
            this.StiffnessNU.ValueChanged += new System.EventHandler(this.StiffnessNU_ValueChanged);
            this.StiffnessNU.Enter += new System.EventHandler(this.Numeric_Enter);
            this.StiffnessNU.Leave += new System.EventHandler(this.Numeric_Leave);
            // 
            // CameraPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.StiffnessNU);
            this.Controls.Add(this.DampingNU);
            this.Controls.Add(this.MassNU);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ClosePanel);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.Name = "CameraPanel";
            this.Size = new System.Drawing.Size(291, 48);
            ((System.ComponentModel.ISupportInitialize)(this.MassNU)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DampingNU)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StiffnessNU)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ClosePanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown MassNU;
        private System.Windows.Forms.NumericUpDown DampingNU;
        private System.Windows.Forms.NumericUpDown StiffnessNU;
    }
}
