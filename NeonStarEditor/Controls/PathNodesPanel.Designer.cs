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
            this.NodeInfo = new System.Windows.Forms.GroupBox();
            this.NodeTypeCombobox = new System.Windows.Forms.ComboBox();
            this.TypeLabel = new System.Windows.Forms.Label();
            this.Align = new System.Windows.Forms.Button();
            this.DeleteNode = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SelectionButton = new System.Windows.Forms.Button();
            this.AddPathNode = new System.Windows.Forms.Button();
            this.TypeComboBox = new System.Windows.Forms.ComboBox();
            this.PathName = new System.Windows.Forms.TextBox();
            this.ToggleDisplayAll = new System.Windows.Forms.Button();
            this.ClosePanel = new System.Windows.Forms.Button();
            this.label = new System.Windows.Forms.Label();
            this.InfoBox.SuspendLayout();
            this.NodeInfo.SuspendLayout();
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
            this.InfoBox.Controls.Add(this.NodeInfo);
            this.InfoBox.Controls.Add(this.label1);
            this.InfoBox.Controls.Add(this.SelectionButton);
            this.InfoBox.Controls.Add(this.AddPathNode);
            this.InfoBox.Controls.Add(this.TypeComboBox);
            this.InfoBox.Controls.Add(this.PathName);
            this.InfoBox.Location = new System.Drawing.Point(145, 30);
            this.InfoBox.Name = "InfoBox";
            this.InfoBox.Size = new System.Drawing.Size(289, 183);
            this.InfoBox.TabIndex = 6;
            this.InfoBox.TabStop = false;
            this.InfoBox.Text = "Info";
            // 
            // NodeInfo
            // 
            this.NodeInfo.Controls.Add(this.NodeTypeCombobox);
            this.NodeInfo.Controls.Add(this.TypeLabel);
            this.NodeInfo.Controls.Add(this.Align);
            this.NodeInfo.Controls.Add(this.DeleteNode);
            this.NodeInfo.Location = new System.Drawing.Point(79, 61);
            this.NodeInfo.Name = "NodeInfo";
            this.NodeInfo.Size = new System.Drawing.Size(204, 117);
            this.NodeInfo.TabIndex = 10;
            this.NodeInfo.TabStop = false;
            this.NodeInfo.Text = "Selected Node";
            // 
            // NodeTypeCombobox
            // 
            this.NodeTypeCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.NodeTypeCombobox.FormattingEnabled = true;
            this.NodeTypeCombobox.Location = new System.Drawing.Point(12, 31);
            this.NodeTypeCombobox.Name = "NodeTypeCombobox";
            this.NodeTypeCombobox.Size = new System.Drawing.Size(67, 21);
            this.NodeTypeCombobox.TabIndex = 8;
            this.NodeTypeCombobox.SelectedIndexChanged += new System.EventHandler(this.NodeTypeCombobox_SelectedIndexChanged);
            // 
            // TypeLabel
            // 
            this.TypeLabel.AutoSize = true;
            this.TypeLabel.Location = new System.Drawing.Point(9, 15);
            this.TypeLabel.Name = "TypeLabel";
            this.TypeLabel.Size = new System.Drawing.Size(31, 13);
            this.TypeLabel.TabIndex = 9;
            this.TypeLabel.Text = "Type";
            // 
            // Align
            // 
            this.Align.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Align.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Align.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.Align.Location = new System.Drawing.Point(12, 81);
            this.Align.Name = "Align";
            this.Align.Size = new System.Drawing.Size(51, 30);
            this.Align.TabIndex = 5;
            this.Align.Text = "V-Align";
            this.Align.UseVisualStyleBackColor = true;
            this.Align.Click += new System.EventHandler(this.Align_Click);
            // 
            // DeleteNode
            // 
            this.DeleteNode.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("DeleteNode.BackgroundImage")));
            this.DeleteNode.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.DeleteNode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DeleteNode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.DeleteNode.Location = new System.Drawing.Point(168, 81);
            this.DeleteNode.Name = "DeleteNode";
            this.DeleteNode.Size = new System.Drawing.Size(30, 30);
            this.DeleteNode.TabIndex = 5;
            this.DeleteNode.UseVisualStyleBackColor = true;
            this.DeleteNode.Click += new System.EventHandler(this.DeleteNode_Click);
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
            // SelectionButton
            // 
            this.SelectionButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.SelectionButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SelectionButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.SelectionButton.Location = new System.Drawing.Point(9, 148);
            this.SelectionButton.Name = "SelectionButton";
            this.SelectionButton.Size = new System.Drawing.Size(64, 30);
            this.SelectionButton.TabIndex = 5;
            this.SelectionButton.Text = "Selection";
            this.SelectionButton.UseVisualStyleBackColor = true;
            this.SelectionButton.Click += new System.EventHandler(this.SelectionButton_Click);
            // 
            // AddPathNode
            // 
            this.AddPathNode.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.AddPathNode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddPathNode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.AddPathNode.Location = new System.Drawing.Point(9, 112);
            this.AddPathNode.Name = "AddPathNode";
            this.AddPathNode.Size = new System.Drawing.Size(64, 30);
            this.AddPathNode.TabIndex = 5;
            this.AddPathNode.Text = "Add";
            this.AddPathNode.UseVisualStyleBackColor = true;
            this.AddPathNode.Click += new System.EventHandler(this.AddPathNode_Click);
            // 
            // TypeComboBox
            // 
            this.TypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TypeComboBox.FormattingEnabled = true;
            this.TypeComboBox.Location = new System.Drawing.Point(6, 80);
            this.TypeComboBox.Name = "TypeComboBox";
            this.TypeComboBox.Size = new System.Drawing.Size(67, 21);
            this.TypeComboBox.TabIndex = 8;
            this.TypeComboBox.SelectedIndexChanged += new System.EventHandler(this.TypeComboBox_SelectedIndexChanged);
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
            // ToggleDisplayAll
            // 
            this.ToggleDisplayAll.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ToggleDisplayAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ToggleDisplayAll.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ToggleDisplayAll.Location = new System.Drawing.Point(75, 156);
            this.ToggleDisplayAll.Name = "ToggleDisplayAll";
            this.ToggleDisplayAll.Size = new System.Drawing.Size(64, 30);
            this.ToggleDisplayAll.TabIndex = 5;
            this.ToggleDisplayAll.Text = "Show All";
            this.ToggleDisplayAll.UseVisualStyleBackColor = true;
            this.ToggleDisplayAll.Click += new System.EventHandler(this.ToggleDisplayAll_Click);
            // 
            // ClosePanel
            // 
            this.ClosePanel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ClosePanel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClosePanel.Location = new System.Drawing.Point(407, 3);
            this.ClosePanel.Name = "ClosePanel";
            this.ClosePanel.Size = new System.Drawing.Size(27, 26);
            this.ClosePanel.TabIndex = 11;
            this.ClosePanel.Text = "X";
            this.ClosePanel.UseVisualStyleBackColor = true;
            this.ClosePanel.Click += new System.EventHandler(this.ClosePanel_Click);
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.Font = new System.Drawing.Font("Agency FB", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.label.Location = new System.Drawing.Point(244, 4);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(80, 25);
            this.label.TabIndex = 16;
            this.label.Text = "Path Nodes";
            // 
            // PathNodesPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.label);
            this.Controls.Add(this.ClosePanel);
            this.Controls.Add(this.InfoBox);
            this.Controls.Add(this.ToggleDisplayAll);
            this.Controls.Add(this.RemovePathButton);
            this.Controls.Add(this.AddPathButton);
            this.Controls.Add(this.NodeLists);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.Name = "PathNodesPanel";
            this.Size = new System.Drawing.Size(442, 222);
            this.InfoBox.ResumeLayout(false);
            this.InfoBox.PerformLayout();
            this.NodeInfo.ResumeLayout(false);
            this.NodeInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button AddPathButton;
        private System.Windows.Forms.Button RemovePathButton;
        private System.Windows.Forms.GroupBox InfoBox;
        private System.Windows.Forms.TextBox PathName;
        private System.Windows.Forms.ComboBox TypeComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ToggleDisplayAll;
        public System.Windows.Forms.ListBox NodeLists;
        private System.Windows.Forms.Button AddPathNode;
        private System.Windows.Forms.Button SelectionButton;
        private System.Windows.Forms.GroupBox NodeInfo;
        private System.Windows.Forms.ComboBox NodeTypeCombobox;
        private System.Windows.Forms.Label TypeLabel;
        private System.Windows.Forms.Button DeleteNode;
        private System.Windows.Forms.Button Align;
        private System.Windows.Forms.Button ClosePanel;
        private System.Windows.Forms.Label label;
    }
}
