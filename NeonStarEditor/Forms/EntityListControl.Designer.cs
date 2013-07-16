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
            this.EntityListBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // EntityListBox
            // 
            this.EntityListBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.EntityListBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.EntityListBox.FormattingEnabled = true;
            this.EntityListBox.Location = new System.Drawing.Point(-1, -1);
            this.EntityListBox.Name = "EntityListBox";
            this.EntityListBox.Size = new System.Drawing.Size(269, 121);
            this.EntityListBox.TabIndex = 1;
            // 
            // EntityListControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.EntityListBox);
            this.Name = "EntityListControl";
            this.Size = new System.Drawing.Size(267, 477);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.ListBox EntityListBox;
    }
}
