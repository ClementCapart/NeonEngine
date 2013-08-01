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
            this.entityListControl = new NeonStarEditor.EntityListControl();
            this.prefabListControl = new NeonStarEditor.PrefabListControl();
            this.SuspendLayout();
            // 
            // entityListControl
            // 
            this.entityListControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.entityListControl.Dock = System.Windows.Forms.DockStyle.Left;
            this.entityListControl.Location = new System.Drawing.Point(0, 0);
            this.entityListControl.MaximumSize = new System.Drawing.Size(640, 160);
            this.entityListControl.MinimumSize = new System.Drawing.Size(640, 35);
            this.entityListControl.Name = "entityListControl";
            this.entityListControl.Size = new System.Drawing.Size(640, 158);
            this.entityListControl.TabIndex = 1;
            // 
            // prefabListControl
            // 
            this.prefabListControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.prefabListControl.Location = new System.Drawing.Point(505, 0);
            this.prefabListControl.MaximumSize = new System.Drawing.Size(540, 160);
            this.prefabListControl.MinimumSize = new System.Drawing.Size(540, 35);
            this.prefabListControl.Name = "prefabListControl";
            this.prefabListControl.Size = new System.Drawing.Size(540, 158);
            this.prefabListControl.TabIndex = 2;
            // 
            // BottomDock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.prefabListControl);
            this.Controls.Add(this.entityListControl);
            this.MaximumSize = new System.Drawing.Size(1280, 160);
            this.MinimumSize = new System.Drawing.Size(1280, 35);
            this.Name = "BottomDock";
            this.Size = new System.Drawing.Size(1278, 158);
            this.Controls.SetChildIndex(this.entityListControl, 0);
            this.Controls.SetChildIndex(this.prefabListControl, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private EntityListControl entityListControl;
        private PrefabListControl prefabListControl;
    }
}
