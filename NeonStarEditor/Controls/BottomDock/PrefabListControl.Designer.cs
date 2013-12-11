namespace NeonStarEditor
{
    partial class PrefabListControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrefabListControl));
            this.PrefabListBox = new System.Windows.Forms.ListBox();
            this.PrefabList = new System.Windows.Forms.Label();
            this.AddPrefabButton = new System.Windows.Forms.Button();
            this.SavePrefabDialog = new System.Windows.Forms.SaveFileDialog();
            this.SaveAsPrefabButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // PrefabListBox
            // 
            this.PrefabListBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.PrefabListBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.PrefabListBox.FormattingEnabled = true;
            this.PrefabListBox.Location = new System.Drawing.Point(21, 33);
            this.PrefabListBox.Name = "PrefabListBox";
            this.PrefabListBox.Size = new System.Drawing.Size(280, 121);
            this.PrefabListBox.TabIndex = 1;
            // 
            // PrefabList
            // 
            this.PrefabList.AutoSize = true;
            this.PrefabList.Font = new System.Drawing.Font("Agency FB", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PrefabList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.PrefabList.Location = new System.Drawing.Point(127, 2);
            this.PrefabList.Name = "PrefabList";
            this.PrefabList.Size = new System.Drawing.Size(75, 25);
            this.PrefabList.TabIndex = 2;
            this.PrefabList.Text = "Prefab List";
            // 
            // AddPrefabButton
            // 
            this.AddPrefabButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("AddPrefabButton.BackgroundImage")));
            this.AddPrefabButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.AddPrefabButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddPrefabButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.AddPrefabButton.Location = new System.Drawing.Point(310, 37);
            this.AddPrefabButton.Name = "AddPrefabButton";
            this.AddPrefabButton.Size = new System.Drawing.Size(30, 30);
            this.AddPrefabButton.TabIndex = 3;
            this.AddPrefabButton.UseVisualStyleBackColor = true;
            this.AddPrefabButton.Click += new System.EventHandler(this.AddPrefabButton_Click);
            // 
            // SavePrefabDialog
            // 
            this.SavePrefabDialog.DefaultExt = "prefab";
            this.SavePrefabDialog.Filter = "Prefab|*.prefab";
            // 
            // SaveAsPrefabButton
            // 
            this.SaveAsPrefabButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("SaveAsPrefabButton.BackgroundImage")));
            this.SaveAsPrefabButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.SaveAsPrefabButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SaveAsPrefabButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.SaveAsPrefabButton.Location = new System.Drawing.Point(310, 74);
            this.SaveAsPrefabButton.Name = "SaveAsPrefabButton";
            this.SaveAsPrefabButton.Size = new System.Drawing.Size(30, 30);
            this.SaveAsPrefabButton.TabIndex = 4;
            this.SaveAsPrefabButton.UseVisualStyleBackColor = true;
            this.SaveAsPrefabButton.Click += new System.EventHandler(this.SaveAsPrefabButton_Click);
            // 
            // PrefabListControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Controls.Add(this.SaveAsPrefabButton);
            this.Controls.Add(this.AddPrefabButton);
            this.Controls.Add(this.PrefabList);
            this.Controls.Add(this.PrefabListBox);
            this.MaximumSize = new System.Drawing.Size(400, 160);
            this.MinimumSize = new System.Drawing.Size(400, 35);
            this.Name = "PrefabListControl";
            this.Size = new System.Drawing.Size(400, 158);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ListBox PrefabListBox;
        private System.Windows.Forms.Label PrefabList;
        private System.Windows.Forms.Button AddPrefabButton;
        private System.Windows.Forms.SaveFileDialog SavePrefabDialog;
        private System.Windows.Forms.Button SaveAsPrefabButton;
    }
}
