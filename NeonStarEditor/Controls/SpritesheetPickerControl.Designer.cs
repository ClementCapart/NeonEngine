namespace NeonStarEditor.Controls
{
    partial class SpritesheetPickerControl
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
            this.assetList = new System.Windows.Forms.TreeView();
            this.SelectButton = new System.Windows.Forms.Button();
            this.ClosePanel = new System.Windows.Forms.Button();
            this.Title = new System.Windows.Forms.Label();
            this.entityName = new System.Windows.Forms.Label();
            this.entityInfo = new System.Windows.Forms.Label();
            this.BackgroundColorDialog = new System.Windows.Forms.ColorDialog();
            this.ChangeBackgroundColorButton = new System.Windows.Forms.Button();
            this.spritesheetView1 = new NeonStarEditor.SpritesheetView();
            this.SuspendLayout();
            // 
            // assetList
            // 
            this.assetList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.assetList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.assetList.Location = new System.Drawing.Point(419, 50);
            this.assetList.Name = "assetList";
            this.assetList.Size = new System.Drawing.Size(277, 377);
            this.assetList.TabIndex = 1;
            this.assetList.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.assetList_AfterSelect);
            // 
            // SelectButton
            // 
            this.SelectButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SelectButton.Location = new System.Drawing.Point(598, 433);
            this.SelectButton.Name = "SelectButton";
            this.SelectButton.Size = new System.Drawing.Size(88, 36);
            this.SelectButton.TabIndex = 2;
            this.SelectButton.Text = "Select";
            this.SelectButton.UseVisualStyleBackColor = true;
            this.SelectButton.Click += new System.EventHandler(this.SelectButton_Click);
            // 
            // ClosePanel
            // 
            this.ClosePanel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ClosePanel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClosePanel.Location = new System.Drawing.Point(669, 3);
            this.ClosePanel.Name = "ClosePanel";
            this.ClosePanel.Size = new System.Drawing.Size(27, 26);
            this.ClosePanel.TabIndex = 3;
            this.ClosePanel.Text = "X";
            this.ClosePanel.UseVisualStyleBackColor = true;
            this.ClosePanel.Click += new System.EventHandler(this.ClosePanel_Click);
            // 
            // Title
            // 
            this.Title.AutoSize = true;
            this.Title.Font = new System.Drawing.Font("Agency FB", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.Title.Location = new System.Drawing.Point(474, 3);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(172, 34);
            this.Title.TabIndex = 4;
            this.Title.Text = "Spritesheet Picker";
            // 
            // entityName
            // 
            this.entityName.AutoSize = true;
            this.entityName.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.entityName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.entityName.Location = new System.Drawing.Point(442, 50);
            this.entityName.Name = "entityName";
            this.entityName.Size = new System.Drawing.Size(0, 15);
            this.entityName.TabIndex = 4;
            // 
            // entityInfo
            // 
            this.entityInfo.AutoSize = true;
            this.entityInfo.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.entityInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.entityInfo.Location = new System.Drawing.Point(3, 14);
            this.entityInfo.Name = "entityInfo";
            this.entityInfo.Size = new System.Drawing.Size(56, 15);
            this.entityInfo.TabIndex = 4;
            this.entityInfo.Text = "No Name";
            // 
            // BackgroundColorDialog
            // 
            this.BackgroundColorDialog.Color = System.Drawing.Color.Gray;
            // 
            // ChangeBackgroundColorButton
            // 
            this.ChangeBackgroundColorButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ChangeBackgroundColorButton.Location = new System.Drawing.Point(428, 433);
            this.ChangeBackgroundColorButton.Name = "ChangeBackgroundColorButton";
            this.ChangeBackgroundColorButton.Size = new System.Drawing.Size(153, 36);
            this.ChangeBackgroundColorButton.TabIndex = 2;
            this.ChangeBackgroundColorButton.Text = "Change Background Color";
            this.ChangeBackgroundColorButton.UseVisualStyleBackColor = true;
            this.ChangeBackgroundColorButton.Click += new System.EventHandler(this.ChangeBackgroundColorButton_Click);
            // 
            // spritesheetView1
            // 
            this.spritesheetView1.Location = new System.Drawing.Point(3, 42);
            this.spritesheetView1.Name = "spritesheetView1";
            this.spritesheetView1.Size = new System.Drawing.Size(410, 434);
            this.spritesheetView1.TabIndex = 5;
            this.spritesheetView1.Text = "spritesheetView1";
            // 
            // SpritesheetPickerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.spritesheetView1);
            this.Controls.Add(this.entityName);
            this.Controls.Add(this.entityInfo);
            this.Controls.Add(this.Title);
            this.Controls.Add(this.ClosePanel);
            this.Controls.Add(this.ChangeBackgroundColorButton);
            this.Controls.Add(this.SelectButton);
            this.Controls.Add(this.assetList);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.Name = "SpritesheetPickerControl";
            this.Size = new System.Drawing.Size(699, 479);
            this.Load += new System.EventHandler(this.GraphicPickerControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView assetList;
        private System.Windows.Forms.Button SelectButton;
        private System.Windows.Forms.Button ClosePanel;
        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.Label entityName;
        private System.Windows.Forms.Label entityInfo;
        private System.Windows.Forms.ColorDialog BackgroundColorDialog;
        private System.Windows.Forms.Button ChangeBackgroundColorButton;
        private SpritesheetView spritesheetView1;
    }
}
