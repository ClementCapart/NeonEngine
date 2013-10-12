namespace NeonStarEditor.Controls.LeftDock
{
    partial class Toolbar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Toolbar));
            this.SaveCurrentMap = new System.Windows.Forms.Button();
            this.ReloadButton = new System.Windows.Forms.Button();
            this.PausePlayButton = new System.Windows.Forms.Button();
            this.CreateRectangle = new System.Windows.Forms.Button();
            this.Selection = new System.Windows.Forms.Button();
            this.ReloadScript = new System.Windows.Forms.Button();
            this.ReloadAssetsButton = new System.Windows.Forms.Button();
            this.AttackManagerButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // SaveCurrentMap
            // 
            this.SaveCurrentMap.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("SaveCurrentMap.BackgroundImage")));
            this.SaveCurrentMap.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.SaveCurrentMap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SaveCurrentMap.Location = new System.Drawing.Point(3, 7);
            this.SaveCurrentMap.Name = "SaveCurrentMap";
            this.SaveCurrentMap.Size = new System.Drawing.Size(30, 30);
            this.SaveCurrentMap.TabIndex = 1;
            this.SaveCurrentMap.UseVisualStyleBackColor = true;
            this.SaveCurrentMap.Click += new System.EventHandler(this.SaveCurrentMap_Click);
            // 
            // ReloadButton
            // 
            this.ReloadButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ReloadButton.BackgroundImage")));
            this.ReloadButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ReloadButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ReloadButton.Location = new System.Drawing.Point(3, 42);
            this.ReloadButton.Name = "ReloadButton";
            this.ReloadButton.Size = new System.Drawing.Size(30, 30);
            this.ReloadButton.TabIndex = 3;
            this.ReloadButton.UseVisualStyleBackColor = true;
            this.ReloadButton.Click += new System.EventHandler(this.ReloadButton_Click);
            // 
            // PausePlayButton
            // 
            this.PausePlayButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.PausePlayButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PausePlayButton.Location = new System.Drawing.Point(3, 97);
            this.PausePlayButton.Name = "PausePlayButton";
            this.PausePlayButton.Size = new System.Drawing.Size(30, 30);
            this.PausePlayButton.TabIndex = 3;
            this.PausePlayButton.UseVisualStyleBackColor = true;
            this.PausePlayButton.Click += new System.EventHandler(this.PausePlayButton_Click);
            // 
            // CreateRectangle
            // 
            this.CreateRectangle.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("CreateRectangle.BackgroundImage")));
            this.CreateRectangle.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.CreateRectangle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CreateRectangle.Location = new System.Drawing.Point(3, 215);
            this.CreateRectangle.Name = "CreateRectangle";
            this.CreateRectangle.Size = new System.Drawing.Size(30, 30);
            this.CreateRectangle.TabIndex = 4;
            this.CreateRectangle.UseVisualStyleBackColor = true;
            this.CreateRectangle.Click += new System.EventHandler(this.CreateRectangle_Click);
            // 
            // Selection
            // 
            this.Selection.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Selection.BackgroundImage")));
            this.Selection.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Selection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Selection.Location = new System.Drawing.Point(3, 179);
            this.Selection.Name = "Selection";
            this.Selection.Size = new System.Drawing.Size(30, 30);
            this.Selection.TabIndex = 5;
            this.Selection.UseVisualStyleBackColor = true;
            this.Selection.Click += new System.EventHandler(this.Selection_Click);
            // 
            // ReloadScript
            // 
            this.ReloadScript.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ReloadScript.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ReloadScript.Location = new System.Drawing.Point(4, 589);
            this.ReloadScript.Name = "ReloadScript";
            this.ReloadScript.Size = new System.Drawing.Size(30, 30);
            this.ReloadScript.TabIndex = 4;
            this.ReloadScript.Text = "S";
            this.ReloadScript.UseVisualStyleBackColor = true;
            this.ReloadScript.Click += new System.EventHandler(this.ReloadScript_Click);
            // 
            // ReloadAssetsButton
            // 
            this.ReloadAssetsButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ReloadAssetsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ReloadAssetsButton.Location = new System.Drawing.Point(4, 553);
            this.ReloadAssetsButton.Name = "ReloadAssetsButton";
            this.ReloadAssetsButton.Size = new System.Drawing.Size(30, 30);
            this.ReloadAssetsButton.TabIndex = 4;
            this.ReloadAssetsButton.Text = "D";
            this.ReloadAssetsButton.UseVisualStyleBackColor = true;
            this.ReloadAssetsButton.Click += new System.EventHandler(this.ReloadAssetsButton_Click);
            // 
            // AttackManagerButton
            // 
            this.AttackManagerButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.AttackManagerButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AttackManagerButton.Location = new System.Drawing.Point(3, 517);
            this.AttackManagerButton.Name = "AttackManagerButton";
            this.AttackManagerButton.Size = new System.Drawing.Size(30, 30);
            this.AttackManagerButton.TabIndex = 4;
            this.AttackManagerButton.Text = "A";
            this.AttackManagerButton.UseVisualStyleBackColor = true;
            this.AttackManagerButton.Click += new System.EventHandler(this.AttackManagerButton_Click);
            // 
            // Toolbar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Controls.Add(this.Selection);
            this.Controls.Add(this.AttackManagerButton);
            this.Controls.Add(this.ReloadAssetsButton);
            this.Controls.Add(this.ReloadScript);
            this.Controls.Add(this.CreateRectangle);
            this.Controls.Add(this.PausePlayButton);
            this.Controls.Add(this.ReloadButton);
            this.Controls.Add(this.SaveCurrentMap);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.Name = "Toolbar";
            this.Size = new System.Drawing.Size(38, 624);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button SaveCurrentMap;
        private System.Windows.Forms.Button ReloadButton;
        private System.Windows.Forms.Button PausePlayButton;
        private System.Windows.Forms.Button CreateRectangle;
        private System.Windows.Forms.Button Selection;
        private System.Windows.Forms.Button ReloadScript;
        private System.Windows.Forms.Button ReloadAssetsButton;
        private System.Windows.Forms.Button AttackManagerButton;
    }
}
