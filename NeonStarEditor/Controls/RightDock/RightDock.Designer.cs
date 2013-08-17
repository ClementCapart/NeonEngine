namespace NeonStarEditor
{
    partial class RightDock
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RightDock));
            this.inspectorControl1 = new NeonStarEditor.InspectorControl();
            this.SuspendLayout();
            // 
            // MinimizeButton
            // 
            this.MinimizeButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("MinimizeButton.BackgroundImage")));
            this.MinimizeButton.Location = new System.Drawing.Point(3, 3);
            // 
            // inspectorControl1
            // 
            this.inspectorControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.inspectorControl1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.inspectorControl1.Location = new System.Drawing.Point(3, 36);
            this.inspectorControl1.Name = "inspectorControl1";
            this.inspectorControl1.Size = new System.Drawing.Size(235, 644);
            this.inspectorControl1.TabIndex = 1;
            // 
            // RightDock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Controls.Add(this.inspectorControl1);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.MaximumSize = new System.Drawing.Size(320, 685);
            this.MinimumSize = new System.Drawing.Size(35, 685);
            this.Name = "RightDock";
            this.Size = new System.Drawing.Size(248, 683);
            this.Controls.SetChildIndex(this.MinimizeButton, 0);
            this.Controls.SetChildIndex(this.inspectorControl1, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private InspectorControl inspectorControl1;
    }
}
