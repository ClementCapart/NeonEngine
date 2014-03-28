namespace NeonStarEditor
{
    partial class LeftDock
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LeftDock));
            this.MainToolbar = new NeonStarEditor.Controls.LeftDock.Toolbar();
            this.magnetismValue = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.magnetismValue)).BeginInit();
            this.SuspendLayout();
            // 
            // MinimizeButton
            // 
            this.MinimizeButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("MinimizeButton.BackgroundImage")));
            this.MinimizeButton.Location = new System.Drawing.Point(3, 3);
            // 
            // MainToolbar
            // 
            this.MainToolbar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.MainToolbar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.MainToolbar.Location = new System.Drawing.Point(-1, 39);
            this.MainToolbar.Name = "MainToolbar";
            this.MainToolbar.Size = new System.Drawing.Size(76, 681);
            this.MainToolbar.TabIndex = 1;
            // 
            // magnetismValue
            // 
            this.magnetismValue.Location = new System.Drawing.Point(36, 361);
            this.magnetismValue.Name = "magnetismValue";
            this.magnetismValue.Size = new System.Drawing.Size(43, 20);
            this.magnetismValue.TabIndex = 2;
            this.magnetismValue.ValueChanged += new System.EventHandler(this.magnetismValue_ValueChanged);
            this.magnetismValue.Enter += new System.EventHandler(this.magnetismValue_Enter);
            this.magnetismValue.Leave += new System.EventHandler(this.magnetismValue_Leave);
            // 
            // LeftDock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Controls.Add(this.magnetismValue);
            this.Controls.Add(this.MainToolbar);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.MaximumSize = new System.Drawing.Size(80, 685);
            this.MinimumSize = new System.Drawing.Size(35, 685);
            this.Name = "LeftDock";
            this.Size = new System.Drawing.Size(78, 683);
            this.Controls.SetChildIndex(this.MinimizeButton, 0);
            this.Controls.SetChildIndex(this.MainToolbar, 0);
            this.Controls.SetChildIndex(this.magnetismValue, 0);
            ((System.ComponentModel.ISupportInitialize)(this.magnetismValue)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.LeftDock.Toolbar MainToolbar;
        private System.Windows.Forms.NumericUpDown magnetismValue;



    }
}
