namespace NeonStarEditor
{
    partial class SpawnPointPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpawnPointPanel));
            this.InfoBox = new System.Windows.Forms.GroupBox();
            this.ToggleDisplayAll = new System.Windows.Forms.Button();
            this.NodeLists = new System.Windows.Forms.ListBox();
            this.RemoveSpawnButton = new System.Windows.Forms.Button();
            this.AddSpawnButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // InfoBox
            // 
            this.InfoBox.Location = new System.Drawing.Point(148, 4);
            this.InfoBox.Name = "InfoBox";
            this.InfoBox.Size = new System.Drawing.Size(289, 184);
            this.InfoBox.TabIndex = 11;
            this.InfoBox.TabStop = false;
            this.InfoBox.Text = "Info";
            // 
            // ToggleDisplayAll
            // 
            this.ToggleDisplayAll.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ToggleDisplayAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ToggleDisplayAll.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ToggleDisplayAll.Location = new System.Drawing.Point(78, 158);
            this.ToggleDisplayAll.Name = "ToggleDisplayAll";
            this.ToggleDisplayAll.Size = new System.Drawing.Size(64, 30);
            this.ToggleDisplayAll.TabIndex = 9;
            this.ToggleDisplayAll.Text = "Show All";
            this.ToggleDisplayAll.UseVisualStyleBackColor = true;
            this.ToggleDisplayAll.Click += new System.EventHandler(this.ToggleDisplayAll_Click);
            // 
            // NodeLists
            // 
            this.NodeLists.FormattingEnabled = true;
            this.NodeLists.Location = new System.Drawing.Point(6, 5);
            this.NodeLists.Name = "NodeLists";
            this.NodeLists.Size = new System.Drawing.Size(136, 147);
            this.NodeLists.TabIndex = 7;
            // 
            // RemoveSpawnButton
            // 
            this.RemoveSpawnButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("RemoveSpawnButton.BackgroundImage")));
            this.RemoveSpawnButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.RemoveSpawnButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RemoveSpawnButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.RemoveSpawnButton.Location = new System.Drawing.Point(42, 158);
            this.RemoveSpawnButton.Name = "RemoveSpawnButton";
            this.RemoveSpawnButton.Size = new System.Drawing.Size(30, 30);
            this.RemoveSpawnButton.TabIndex = 10;
            this.RemoveSpawnButton.UseVisualStyleBackColor = true;
            // 
            // AddSpawnButton
            // 
            this.AddSpawnButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("AddSpawnButton.BackgroundImage")));
            this.AddSpawnButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.AddSpawnButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddSpawnButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.AddSpawnButton.Location = new System.Drawing.Point(6, 158);
            this.AddSpawnButton.Name = "AddSpawnButton";
            this.AddSpawnButton.Size = new System.Drawing.Size(30, 30);
            this.AddSpawnButton.TabIndex = 8;
            this.AddSpawnButton.UseVisualStyleBackColor = true;
            // 
            // SpawnPointPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Controls.Add(this.InfoBox);
            this.Controls.Add(this.ToggleDisplayAll);
            this.Controls.Add(this.RemoveSpawnButton);
            this.Controls.Add(this.AddSpawnButton);
            this.Controls.Add(this.NodeLists);
            this.Name = "SpawnPointPanel";
            this.Size = new System.Drawing.Size(448, 197);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox InfoBox;
        private System.Windows.Forms.Button ToggleDisplayAll;
        private System.Windows.Forms.Button RemoveSpawnButton;
        private System.Windows.Forms.Button AddSpawnButton;
        public System.Windows.Forms.ListBox NodeLists;
    }
}
