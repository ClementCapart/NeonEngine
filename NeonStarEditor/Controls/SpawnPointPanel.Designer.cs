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
            this.label2 = new System.Windows.Forms.Label();
            this.sideComboBox = new System.Windows.Forms.ComboBox();
            this.ToggleDisplayAll = new System.Windows.Forms.Button();
            this.SpawnPointList = new System.Windows.Forms.ListBox();
            this.RemoveSpawnButton = new System.Windows.Forms.Button();
            this.AddSpawnButton = new System.Windows.Forms.Button();
            this.EntityList = new System.Windows.Forms.Label();
            this.ClosePanel = new System.Windows.Forms.Button();
            this.InfoBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // InfoBox
            // 
            this.InfoBox.Controls.Add(this.label2);
            this.InfoBox.Controls.Add(this.sideComboBox);
            this.InfoBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.InfoBox.Location = new System.Drawing.Point(148, 33);
            this.InfoBox.Name = "InfoBox";
            this.InfoBox.Size = new System.Drawing.Size(289, 155);
            this.InfoBox.TabIndex = 11;
            this.InfoBox.TabStop = false;
            this.InfoBox.Text = "Info";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.label2.Location = new System.Drawing.Point(6, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Spawn Side";
            // 
            // sideComboBox
            // 
            this.sideComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sideComboBox.FormattingEnabled = true;
            this.sideComboBox.Location = new System.Drawing.Point(6, 48);
            this.sideComboBox.Name = "sideComboBox";
            this.sideComboBox.Size = new System.Drawing.Size(67, 21);
            this.sideComboBox.TabIndex = 12;
            this.sideComboBox.SelectedIndexChanged += new System.EventHandler(this.sideComboBox_SelectedIndexChanged);
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
            // SpawnPointList
            // 
            this.SpawnPointList.FormattingEnabled = true;
            this.SpawnPointList.Location = new System.Drawing.Point(6, 5);
            this.SpawnPointList.Name = "SpawnPointList";
            this.SpawnPointList.Size = new System.Drawing.Size(136, 147);
            this.SpawnPointList.TabIndex = 7;
            this.SpawnPointList.SelectedIndexChanged += new System.EventHandler(this.SpawnPointList_SelectedIndexChanged);
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
            this.RemoveSpawnButton.Click += new System.EventHandler(this.RemoveSpawnButton_Click);
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
            this.AddSpawnButton.Click += new System.EventHandler(this.AddSpawnButton_Click);
            // 
            // EntityList
            // 
            this.EntityList.AutoSize = true;
            this.EntityList.Font = new System.Drawing.Font("Agency FB", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EntityList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.EntityList.Location = new System.Drawing.Point(258, 5);
            this.EntityList.Name = "EntityList";
            this.EntityList.Size = new System.Drawing.Size(92, 25);
            this.EntityList.TabIndex = 14;
            this.EntityList.Text = "Spawn Points";
            // 
            // ClosePanel
            // 
            this.ClosePanel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ClosePanel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClosePanel.Location = new System.Drawing.Point(418, 4);
            this.ClosePanel.Name = "ClosePanel";
            this.ClosePanel.Size = new System.Drawing.Size(27, 26);
            this.ClosePanel.TabIndex = 15;
            this.ClosePanel.Text = "X";
            this.ClosePanel.UseVisualStyleBackColor = true;
            this.ClosePanel.Click += new System.EventHandler(this.ClosePanel_Click);
            // 
            // SpawnPointPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Controls.Add(this.ClosePanel);
            this.Controls.Add(this.EntityList);
            this.Controls.Add(this.InfoBox);
            this.Controls.Add(this.ToggleDisplayAll);
            this.Controls.Add(this.RemoveSpawnButton);
            this.Controls.Add(this.AddSpawnButton);
            this.Controls.Add(this.SpawnPointList);
            this.Name = "SpawnPointPanel";
            this.Size = new System.Drawing.Size(448, 197);
            this.InfoBox.ResumeLayout(false);
            this.InfoBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox InfoBox;
        private System.Windows.Forms.Button ToggleDisplayAll;
        private System.Windows.Forms.Button RemoveSpawnButton;
        private System.Windows.Forms.Button AddSpawnButton;
        public System.Windows.Forms.ListBox SpawnPointList;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox sideComboBox;
        private System.Windows.Forms.Label EntityList;
        private System.Windows.Forms.Button ClosePanel;
    }
}
