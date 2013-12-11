namespace NeonStarEditor
{
    partial class EntityListControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EntityListControl));
            this.EntityListBox = new System.Windows.Forms.ListBox();
            this.EntityList = new System.Windows.Forms.Label();
            this.SavePrefabDialog = new System.Windows.Forms.SaveFileDialog();
            this.OrderList = new System.Windows.Forms.Button();
            this.duplicateButton = new System.Windows.Forms.Button();
            this.RemoveEntityButton = new System.Windows.Forms.Button();
            this.AddEntityButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // EntityListBox
            // 
            this.EntityListBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.EntityListBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.EntityListBox.FormattingEnabled = true;
            this.EntityListBox.Location = new System.Drawing.Point(21, 34);
            this.EntityListBox.Name = "EntityListBox";
            this.EntityListBox.Size = new System.Drawing.Size(280, 121);
            this.EntityListBox.TabIndex = 1;
            this.EntityListBox.SelectedIndexChanged += new System.EventHandler(this.EntityListBox_SelectedIndexChanged);
            // 
            // EntityList
            // 
            this.EntityList.AutoSize = true;
            this.EntityList.Font = new System.Drawing.Font("Agency FB", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EntityList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.EntityList.Location = new System.Drawing.Point(130, 2);
            this.EntityList.Name = "EntityList";
            this.EntityList.Size = new System.Drawing.Size(67, 25);
            this.EntityList.TabIndex = 2;
            this.EntityList.Text = "Entity List";
            // 
            // SavePrefabDialog
            // 
            this.SavePrefabDialog.DefaultExt = "prefab";
            this.SavePrefabDialog.Filter = "Prefab|*.prefab";
            // 
            // OrderList
            // 
            this.OrderList.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.OrderList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OrderList.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OrderList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.OrderList.Location = new System.Drawing.Point(349, 41);
            this.OrderList.Name = "OrderList";
            this.OrderList.Size = new System.Drawing.Size(31, 30);
            this.OrderList.TabIndex = 3;
            this.OrderList.Text = "ABC";
            this.OrderList.UseVisualStyleBackColor = true;
            this.OrderList.Click += new System.EventHandler(this.OrderList_Click);
            // 
            // duplicateButton
            // 
            this.duplicateButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("duplicateButton.BackgroundImage")));
            this.duplicateButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.duplicateButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.duplicateButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.duplicateButton.Location = new System.Drawing.Point(313, 114);
            this.duplicateButton.Name = "duplicateButton";
            this.duplicateButton.Size = new System.Drawing.Size(30, 30);
            this.duplicateButton.TabIndex = 3;
            this.duplicateButton.UseVisualStyleBackColor = true;
            this.duplicateButton.Click += new System.EventHandler(this.duplicateButton_Click);
            // 
            // RemoveEntityButton
            // 
            this.RemoveEntityButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("RemoveEntityButton.BackgroundImage")));
            this.RemoveEntityButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.RemoveEntityButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RemoveEntityButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.RemoveEntityButton.Location = new System.Drawing.Point(313, 78);
            this.RemoveEntityButton.Name = "RemoveEntityButton";
            this.RemoveEntityButton.Size = new System.Drawing.Size(30, 30);
            this.RemoveEntityButton.TabIndex = 3;
            this.RemoveEntityButton.UseVisualStyleBackColor = true;
            this.RemoveEntityButton.Click += new System.EventHandler(this.RemoveEntityButton_Click);
            // 
            // AddEntityButton
            // 
            this.AddEntityButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("AddEntityButton.BackgroundImage")));
            this.AddEntityButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.AddEntityButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddEntityButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.AddEntityButton.Location = new System.Drawing.Point(313, 41);
            this.AddEntityButton.Name = "AddEntityButton";
            this.AddEntityButton.Size = new System.Drawing.Size(30, 30);
            this.AddEntityButton.TabIndex = 3;
            this.AddEntityButton.UseVisualStyleBackColor = true;
            this.AddEntityButton.Click += new System.EventHandler(this.AddEntityButton_Click);
            // 
            // EntityListControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Controls.Add(this.duplicateButton);
            this.Controls.Add(this.RemoveEntityButton);
            this.Controls.Add(this.OrderList);
            this.Controls.Add(this.AddEntityButton);
            this.Controls.Add(this.EntityList);
            this.Controls.Add(this.EntityListBox);
            this.MaximumSize = new System.Drawing.Size(400, 160);
            this.MinimumSize = new System.Drawing.Size(400, 35);
            this.Name = "EntityListControl";
            this.Size = new System.Drawing.Size(400, 158);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ListBox EntityListBox;
        private System.Windows.Forms.Label EntityList;
        private System.Windows.Forms.Button AddEntityButton;
        private System.Windows.Forms.Button RemoveEntityButton;
        private System.Windows.Forms.SaveFileDialog SavePrefabDialog;
        private System.Windows.Forms.Button duplicateButton;
        private System.Windows.Forms.Button OrderList;
    }
}
