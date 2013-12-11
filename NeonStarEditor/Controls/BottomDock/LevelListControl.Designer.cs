namespace NeonStarEditor.Controls.BottomDock
{
    partial class LevelListControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LevelListControl));
            this.EntityList = new System.Windows.Forms.Label();
            this.levelListTreeView = new System.Windows.Forms.TreeView();
            this.LoadLevel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // EntityList
            // 
            this.EntityList.AutoSize = true;
            this.EntityList.Font = new System.Drawing.Font("Agency FB", 15.5F);
            this.EntityList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.EntityList.Location = new System.Drawing.Point(92, 5);
            this.EntityList.Name = "EntityList";
            this.EntityList.Size = new System.Drawing.Size(64, 25);
            this.EntityList.TabIndex = 3;
            this.EntityList.Text = "Level List";
            // 
            // levelListTreeView
            // 
            this.levelListTreeView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.levelListTreeView.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.levelListTreeView.Location = new System.Drawing.Point(12, 37);
            this.levelListTreeView.Name = "levelListTreeView";
            this.levelListTreeView.Size = new System.Drawing.Size(216, 116);
            this.levelListTreeView.TabIndex = 4;
            // 
            // LoadLevel
            // 
            this.LoadLevel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("LoadLevel.BackgroundImage")));
            this.LoadLevel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.LoadLevel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LoadLevel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.LoadLevel.Location = new System.Drawing.Point(235, 38);
            this.LoadLevel.Name = "LoadLevel";
            this.LoadLevel.Size = new System.Drawing.Size(30, 30);
            this.LoadLevel.TabIndex = 5;
            this.LoadLevel.UseVisualStyleBackColor = true;
            this.LoadLevel.Click += new System.EventHandler(this.LoadLevel_Click);
            // 
            // LevelListControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Controls.Add(this.LoadLevel);
            this.Controls.Add(this.levelListTreeView);
            this.Controls.Add(this.EntityList);
            this.Name = "LevelListControl";
            this.Size = new System.Drawing.Size(284, 165);
            this.Load += new System.EventHandler(this.LevelListControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label EntityList;
        private System.Windows.Forms.TreeView levelListTreeView;
        private System.Windows.Forms.Button LoadLevel;
    }
}
