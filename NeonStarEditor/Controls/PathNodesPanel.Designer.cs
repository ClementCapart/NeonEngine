namespace NeonStarEditor
{
    partial class PathNodesPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PathNodesPanel));
            this.NodeLists = new System.Windows.Forms.ListBox();
            this.AddPathButton = new System.Windows.Forms.Button();
            this.RemovePathButton = new System.Windows.Forms.Button();
            this.InfoBox = new System.Windows.Forms.GroupBox();
            this.PathName = new System.Windows.Forms.TextBox();
            this.TypeComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.InfoBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // NodeLists
            // 
            this.NodeLists.FormattingEnabled = true;
            this.NodeLists.Location = new System.Drawing.Point(3, 3);
            this.NodeLists.Name = "NodeLists";
            this.NodeLists.Size = new System.Drawing.Size(136, 147);
            this.NodeLists.TabIndex = 0;
            this.NodeLists.SelectedIndexChanged += new System.EventHandler(this.NodeLists_SelectedIndexChanged);
            // 
            // AddPathButton
            // 
            this.AddPathButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("AddPathButton.BackgroundImage")));
            this.AddPathButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.AddPathButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddPathButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.AddPathButton.Location = new System.Drawing.Point(3, 156);
            this.AddPathButton.Name = "AddPathButton";
            this.AddPathButton.Size = new System.Drawing.Size(30, 30);
            this.AddPathButton.TabIndex = 4;
            this.AddPathButton.UseVisualStyleBackColor = true;
            this.AddPathButton.Click += new System.EventHandler(this.AddPathButton_Click);
            // 
            // RemovePathButton
            // 
            this.RemovePathButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("RemovePathButton.BackgroundImage")));
            this.RemovePathButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.RemovePathButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RemovePathButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.RemovePathButton.Location = new System.Drawing.Point(39, 156);
            this.RemovePathButton.Name = "RemovePathButton";
            this.RemovePathButton.Size = new System.Drawing.Size(30, 30);
            this.RemovePathButton.TabIndex = 5;
            this.RemovePathButton.UseVisualStyleBackColor = true;
            this.RemovePathButton.Click += new System.EventHandler(this.RemovePathButton_Click);
            // 
            // InfoBox
            // 
            this.InfoBox.Controls.Add(this.label1);
            this.InfoBox.Controls.Add(this.TypeComboBox);
            this.InfoBox.Controls.Add(this.PathName);
            this.InfoBox.Location = new System.Drawing.Point(145, 2);
            this.InfoBox.Name = "InfoBox";
            this.InfoBox.Size = new System.Drawing.Size(289, 184);
            this.InfoBox.TabIndex = 6;
            this.InfoBox.TabStop = false;
            this.InfoBox.Text = "Info";
            // 
            // PathName
            // 
            this.PathName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.PathName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PathName.Font = new System.Drawing.Font("Agency FB", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PathName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.PathName.Location = new System.Drawing.Point(68, 19);
            this.PathName.Name = "PathName";
            this.PathName.Size = new System.Drawing.Size(155, 36);
            this.PathName.TabIndex = 7;
            this.PathName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.PathName.Enter += new System.EventHandler(this.PathName_Enter);
            this.PathName.Leave += new System.EventHandler(this.PathName_Leave);
            // 
            // TypeComboBox
            // 
            this.TypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TypeComboBox.FormattingEnabled = true;
            this.TypeComboBox.Location = new System.Drawing.Point(6, 80);
            this.TypeComboBox.Name = "TypeComboBox";
            this.TypeComboBox.Size = new System.Drawing.Size(81, 21);
            this.TypeComboBox.TabIndex = 8;
            this.TypeComboBox.SelectedIndexChanged += new System.EventHandler(this.TypeComboBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Type";
            // 
            // PathNodesPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.InfoBox);
            this.Controls.Add(this.RemovePathButton);
            this.Controls.Add(this.AddPathButton);
            this.Controls.Add(this.NodeLists);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.Name = "PathNodesPanel";
            this.Size = new System.Drawing.Size(442, 197);
            this.InfoBox.ResumeLayout(false);
            this.InfoBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox NodeLists;
        private System.Windows.Forms.Button AddPathButton;
        private System.Windows.Forms.Button RemovePathButton;
        private System.Windows.Forms.GroupBox InfoBox;
        private System.Windows.Forms.TextBox PathName;
        private System.Windows.Forms.ComboBox TypeComboBox;
        private System.Windows.Forms.Label label1;
    }
}
