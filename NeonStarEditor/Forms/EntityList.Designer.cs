namespace NeonStarEditor
{
    partial class EntityList
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
            this.EntityListBox = new System.Windows.Forms.ListBox();
            this.Inspector = new System.Windows.Forms.Panel();
            this.ComponentList = new System.Windows.Forms.ComboBox();
            this.AddComponent = new System.Windows.Forms.Button();
            this.OpenScript = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // EntityListBox
            // 
            this.EntityListBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.EntityListBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.EntityListBox.FormattingEnabled = true;
            this.EntityListBox.Location = new System.Drawing.Point(12, 12);
            this.EntityListBox.Name = "EntityListBox";
            this.EntityListBox.Size = new System.Drawing.Size(269, 121);
            this.EntityListBox.TabIndex = 0;
            this.EntityListBox.SelectedIndexChanged += new System.EventHandler(this.EntityListBox_SelectedIndexChanged);
            // 
            // Inspector
            // 
            this.Inspector.AutoScroll = true;
            this.Inspector.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.Inspector.Location = new System.Drawing.Point(12, 139);
            this.Inspector.Name = "Inspector";
            this.Inspector.Size = new System.Drawing.Size(269, 563);
            this.Inspector.TabIndex = 1;
            // 
            // ComponentList
            // 
            this.ComponentList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComponentList.FormattingEnabled = true;
            this.ComponentList.Location = new System.Drawing.Point(12, 716);
            this.ComponentList.Name = "ComponentList";
            this.ComponentList.Size = new System.Drawing.Size(194, 21);
            this.ComponentList.TabIndex = 2;
            // 
            // AddComponent
            // 
            this.AddComponent.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.AddComponent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddComponent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.AddComponent.Location = new System.Drawing.Point(212, 716);
            this.AddComponent.Name = "AddComponent";
            this.AddComponent.Size = new System.Drawing.Size(69, 23);
            this.AddComponent.TabIndex = 3;
            this.AddComponent.Text = "Add";
            this.AddComponent.UseVisualStyleBackColor = true;
            this.AddComponent.Click += new System.EventHandler(this.AddComponent_Click);
            // 
            // OpenScript
            // 
            this.OpenScript.Filter = "\"Fichiers NeonStar C#|*.nscs";
            // 
            // EntityList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(293, 749);
            this.Controls.Add(this.AddComponent);
            this.Controls.Add(this.ComponentList);
            this.Controls.Add(this.Inspector);
            this.Controls.Add(this.EntityListBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "EntityList";
            this.Text = "Entity List";
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.ListBox EntityListBox;
        private System.Windows.Forms.Panel Inspector;
        private System.Windows.Forms.Button AddComponent;
        private System.Windows.Forms.OpenFileDialog OpenScript;
        public System.Windows.Forms.ComboBox ComponentList;
    }
}