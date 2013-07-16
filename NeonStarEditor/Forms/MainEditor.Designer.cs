namespace NeonStarEditor
{
    partial class MainEditor
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainEditor));
            this.PlayButton = new System.Windows.Forms.CheckBox();
            this.PauseButton = new System.Windows.Forms.CheckBox();
            this.SaveCurrentMap = new System.Windows.Forms.Button();
            this.AddEntity = new System.Windows.Forms.Button();
            this.Selection = new System.Windows.Forms.Button();
            this.CreateRectangle = new System.Windows.Forms.Button();
            this.ReloadButton = new System.Windows.Forms.Button();
            this.ReloadScript = new System.Windows.Forms.Button();
            this.SavePrefab = new System.Windows.Forms.Button();
            this.SavePrefabDialog = new System.Windows.Forms.SaveFileDialog();
            this.AddPrefab = new System.Windows.Forms.Button();
            this.Prefabs = new System.Windows.Forms.GroupBox();
            this.PrefabList = new System.Windows.Forms.ListBox();
            this.Prefabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // PlayButton
            // 
            this.PlayButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.PlayButton.AutoSize = true;
            this.PlayButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("PlayButton.BackgroundImage")));
            this.PlayButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.PlayButton.Checked = true;
            this.PlayButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.PlayButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PlayButton.Location = new System.Drawing.Point(105, 55);
            this.PlayButton.MaximumSize = new System.Drawing.Size(30, 30);
            this.PlayButton.MinimumSize = new System.Drawing.Size(30, 30);
            this.PlayButton.Name = "PlayButton";
            this.PlayButton.Size = new System.Drawing.Size(30, 30);
            this.PlayButton.TabIndex = 1;
            this.PlayButton.UseVisualStyleBackColor = true;
            this.PlayButton.CheckedChanged += new System.EventHandler(this.PlayButton_CheckedChanged);
            // 
            // PauseButton
            // 
            this.PauseButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.PauseButton.AutoSize = true;
            this.PauseButton.BackgroundImage = global::NeonStarEditor.Icones.PauseButton;
            this.PauseButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.PauseButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PauseButton.Location = new System.Drawing.Point(69, 55);
            this.PauseButton.MaximumSize = new System.Drawing.Size(30, 30);
            this.PauseButton.MinimumSize = new System.Drawing.Size(30, 30);
            this.PauseButton.Name = "PauseButton";
            this.PauseButton.Size = new System.Drawing.Size(30, 30);
            this.PauseButton.TabIndex = 1;
            this.PauseButton.UseVisualStyleBackColor = true;
            this.PauseButton.CheckedChanged += new System.EventHandler(this.PauseButton_CheckedChanged);
            // 
            // SaveCurrentMap
            // 
            this.SaveCurrentMap.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("SaveCurrentMap.BackgroundImage")));
            this.SaveCurrentMap.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.SaveCurrentMap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SaveCurrentMap.Location = new System.Drawing.Point(12, 12);
            this.SaveCurrentMap.Name = "SaveCurrentMap";
            this.SaveCurrentMap.Size = new System.Drawing.Size(30, 30);
            this.SaveCurrentMap.TabIndex = 0;
            this.SaveCurrentMap.UseVisualStyleBackColor = true;
            this.SaveCurrentMap.Click += new System.EventHandler(this.SaveCurrentMap_Click);
            // 
            // AddEntity
            // 
            this.AddEntity.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("AddEntity.BackgroundImage")));
            this.AddEntity.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.AddEntity.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddEntity.Location = new System.Drawing.Point(78, 96);
            this.AddEntity.Name = "AddEntity";
            this.AddEntity.Size = new System.Drawing.Size(50, 50);
            this.AddEntity.TabIndex = 0;
            this.AddEntity.UseVisualStyleBackColor = true;
            this.AddEntity.Click += new System.EventHandler(this.AddEntity_Click);
            // 
            // Selection
            // 
            this.Selection.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Selection.BackgroundImage")));
            this.Selection.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Selection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Selection.Location = new System.Drawing.Point(12, 96);
            this.Selection.Name = "Selection";
            this.Selection.Size = new System.Drawing.Size(50, 50);
            this.Selection.TabIndex = 0;
            this.Selection.UseVisualStyleBackColor = true;
            this.Selection.Click += new System.EventHandler(this.Selection_Click);
            // 
            // CreateRectangle
            // 
            this.CreateRectangle.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("CreateRectangle.BackgroundImage")));
            this.CreateRectangle.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.CreateRectangle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CreateRectangle.Location = new System.Drawing.Point(12, 152);
            this.CreateRectangle.Name = "CreateRectangle";
            this.CreateRectangle.Size = new System.Drawing.Size(50, 50);
            this.CreateRectangle.TabIndex = 0;
            this.CreateRectangle.UseVisualStyleBackColor = true;
            this.CreateRectangle.Click += new System.EventHandler(this.CreateRectangle_Click);
            // 
            // ReloadButton
            // 
            this.ReloadButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ReloadButton.BackgroundImage")));
            this.ReloadButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ReloadButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ReloadButton.Location = new System.Drawing.Point(32, 55);
            this.ReloadButton.Name = "ReloadButton";
            this.ReloadButton.Size = new System.Drawing.Size(30, 30);
            this.ReloadButton.TabIndex = 0;
            this.ReloadButton.UseVisualStyleBackColor = true;
            this.ReloadButton.Click += new System.EventHandler(this.ReloadButton_Click);
            // 
            // ReloadScript
            // 
            this.ReloadScript.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ReloadScript.BackgroundImage")));
            this.ReloadScript.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ReloadScript.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ReloadScript.Location = new System.Drawing.Point(218, 404);
            this.ReloadScript.Name = "ReloadScript";
            this.ReloadScript.Size = new System.Drawing.Size(30, 28);
            this.ReloadScript.TabIndex = 0;
            this.ReloadScript.UseVisualStyleBackColor = true;
            this.ReloadScript.Click += new System.EventHandler(this.ReloadScript_Click);
            // 
            // SavePrefab
            // 
            this.SavePrefab.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.SavePrefab.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SavePrefab.Location = new System.Drawing.Point(50, 402);
            this.SavePrefab.Name = "SavePrefab";
            this.SavePrefab.Size = new System.Drawing.Size(94, 30);
            this.SavePrefab.TabIndex = 0;
            this.SavePrefab.Text = "Save Prefab";
            this.SavePrefab.UseVisualStyleBackColor = true;
            this.SavePrefab.Click += new System.EventHandler(this.SavePrefab_Click);
            // 
            // SavePrefabDialog
            // 
            this.SavePrefabDialog.DefaultExt = "prefab";
            this.SavePrefabDialog.Filter = "Prefab|*.prefab";
            // 
            // AddPrefab
            // 
            this.AddPrefab.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.AddPrefab.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddPrefab.Location = new System.Drawing.Point(38, 131);
            this.AddPrefab.Name = "AddPrefab";
            this.AddPrefab.Size = new System.Drawing.Size(94, 28);
            this.AddPrefab.TabIndex = 0;
            this.AddPrefab.Text = "Add Prefab";
            this.AddPrefab.UseVisualStyleBackColor = true;
            this.AddPrefab.Click += new System.EventHandler(this.AddPrefab_Click);
            // 
            // Prefabs
            // 
            this.Prefabs.Controls.Add(this.PrefabList);
            this.Prefabs.Controls.Add(this.AddPrefab);
            this.Prefabs.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Prefabs.Location = new System.Drawing.Point(12, 218);
            this.Prefabs.Name = "Prefabs";
            this.Prefabs.Size = new System.Drawing.Size(169, 165);
            this.Prefabs.TabIndex = 2;
            this.Prefabs.TabStop = false;
            this.Prefabs.Text = "Prefabs";
            // 
            // PrefabList
            // 
            this.PrefabList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.PrefabList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.PrefabList.FormattingEnabled = true;
            this.PrefabList.ItemHeight = 18;
            this.PrefabList.Location = new System.Drawing.Point(6, 31);
            this.PrefabList.Name = "PrefabList";
            this.PrefabList.Size = new System.Drawing.Size(157, 94);
            this.PrefabList.TabIndex = 1;
            // 
            // MainEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(259, 444);
            this.Controls.Add(this.Prefabs);
            this.Controls.Add(this.PlayButton);
            this.Controls.Add(this.ReloadButton);
            this.Controls.Add(this.PauseButton);
            this.Controls.Add(this.SavePrefab);
            this.Controls.Add(this.SaveCurrentMap);
            this.Controls.Add(this.ReloadScript);
            this.Controls.Add(this.AddEntity);
            this.Controls.Add(this.Selection);
            this.Controls.Add(this.CreateRectangle);
            this.Font = new System.Drawing.Font("Calibri", 8.25F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Location = new System.Drawing.Point(500, 500);
            this.MaximizeBox = false;
            this.Name = "MainEditor";
            this.ShowIcon = false;
            this.Text = "NeonStar Editor";
            this.Prefabs.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CreateRectangle;
        private System.Windows.Forms.CheckBox PauseButton;
        private System.Windows.Forms.CheckBox PlayButton;
        private System.Windows.Forms.Button SaveCurrentMap;
        private System.Windows.Forms.Button Selection;
        private System.Windows.Forms.Button AddEntity;
        private System.Windows.Forms.Button ReloadButton;
        private System.Windows.Forms.Button ReloadScript;
        private System.Windows.Forms.Button SavePrefab;
        private System.Windows.Forms.SaveFileDialog SavePrefabDialog;
        private System.Windows.Forms.Button AddPrefab;
        private System.Windows.Forms.GroupBox Prefabs;
        private System.Windows.Forms.ListBox PrefabList;
    }
}