namespace NeonStarEditor
{
    partial class BottomDock
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
            this.entityListControl1 = new NeonStarEditor.EntityListControl();
            this.SuspendLayout();
            // 
            // entityListControl1
            // 
            this.entityListControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.entityListControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.entityListControl1.Location = new System.Drawing.Point(0, 0);
            this.entityListControl1.MaximumSize = new System.Drawing.Size(640, 160);
            this.entityListControl1.MinimumSize = new System.Drawing.Size(640, 35);
            this.entityListControl1.Name = "entityListControl1";
            this.entityListControl1.Size = new System.Drawing.Size(640, 158);
            this.entityListControl1.TabIndex = 1;
            // 
            // BottomDock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.entityListControl1);
            this.MaximumSize = new System.Drawing.Size(1280, 160);
            this.MinimumSize = new System.Drawing.Size(1280, 35);
            this.Name = "BottomDock";
            this.Size = new System.Drawing.Size(1278, 158);
            this.Controls.SetChildIndex(this.entityListControl1, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private EntityListControl entityListControl1;
    }
}
