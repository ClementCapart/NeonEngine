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
            this.AddEntityButton = new System.Windows.Forms.Button();
            this.RemoveEntityButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // EntityListBox
            // 
            this.EntityListBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.EntityListBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.EntityListBox.FormattingEnabled = true;
            this.EntityListBox.Location = new System.Drawing.Point(21, 34);
            this.EntityListBox.Name = "EntityListBox";
            this.EntityListBox.Size = new System.Drawing.Size(436, 121);
            this.EntityListBox.TabIndex = 1;
            // 
            // EntityList
            // 
            this.EntityList.AutoSize = true;
            this.EntityList.Font = new System.Drawing.Font("Agency FB", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EntityList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.EntityList.Location = new System.Drawing.Point(202, 2);
            this.EntityList.Name = "EntityList";
            this.EntityList.Size = new System.Drawing.Size(67, 25);
            this.EntityList.TabIndex = 2;
            this.EntityList.Text = "Entity List";
            // 
            // AddEntityButton
            // 
            this.AddEntityButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("AddEntityButton.BackgroundImage")));
            this.AddEntityButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.AddEntityButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddEntityButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.AddEntityButton.Location = new System.Drawing.Point(463, 35);
            this.AddEntityButton.Name = "AddEntityButton";
            this.AddEntityButton.Size = new System.Drawing.Size(30, 28);
            this.AddEntityButton.TabIndex = 3;
            this.AddEntityButton.UseVisualStyleBackColor = true;
            this.AddEntityButton.Click += new System.EventHandler(this.AddEntityButton_Click);
            // 
            // RemoveEntityButton
            // 
            this.RemoveEntityButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("RemoveEntityButton.BackgroundImage")));
            this.RemoveEntityButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.RemoveEntityButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RemoveEntityButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.RemoveEntityButton.Location = new System.Drawing.Point(463, 69);
            this.RemoveEntityButton.Name = "RemoveEntityButton";
            this.RemoveEntityButton.Size = new System.Drawing.Size(30, 28);
            this.RemoveEntityButton.TabIndex = 3;
            this.RemoveEntityButton.UseVisualStyleBackColor = true;
            this.RemoveEntityButton.Click += new System.EventHandler(this.RemoveEntityButton_Click);
            // 
            // EntityListControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Controls.Add(this.RemoveEntityButton);
            this.Controls.Add(this.AddEntityButton);
            this.Controls.Add(this.EntityList);
            this.Controls.Add(this.EntityListBox);
            this.MaximumSize = new System.Drawing.Size(640, 160);
            this.MinimumSize = new System.Drawing.Size(640, 35);
            this.Name = "EntityListControl";
            this.Size = new System.Drawing.Size(640, 158);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ListBox EntityListBox;
        private System.Windows.Forms.Label EntityList;
        private System.Windows.Forms.Button AddEntityButton;
        private System.Windows.Forms.Button RemoveEntityButton;
    }
}
