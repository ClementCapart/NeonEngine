namespace NeonStarEditor
{
    partial class AddComponentControl
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
            this.ComponentList = new System.Windows.Forms.TreeView();
            this.label = new System.Windows.Forms.Label();
            this.AddComponent = new System.Windows.Forms.Button();
            this.ClosePanel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ComponentList
            // 
            this.ComponentList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ComponentList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ComponentList.Location = new System.Drawing.Point(14, 34);
            this.ComponentList.Name = "ComponentList";
            this.ComponentList.Size = new System.Drawing.Size(339, 224);
            this.ComponentList.TabIndex = 0;
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.Font = new System.Drawing.Font("Agency FB", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.label.Location = new System.Drawing.Point(142, 4);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(85, 25);
            this.label.TabIndex = 15;
            this.label.Text = "Components";
            // 
            // AddComponent
            // 
            this.AddComponent.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.AddComponent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddComponent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.AddComponent.Location = new System.Drawing.Point(117, 264);
            this.AddComponent.Name = "AddComponent";
            this.AddComponent.Size = new System.Drawing.Size(134, 23);
            this.AddComponent.TabIndex = 16;
            this.AddComponent.Text = "Add to selected Entity";
            this.AddComponent.UseVisualStyleBackColor = true;
            this.AddComponent.Click += new System.EventHandler(this.AddComponent_Click);
            // 
            // ClosePanel
            // 
            this.ClosePanel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ClosePanel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClosePanel.Location = new System.Drawing.Point(337, 3);
            this.ClosePanel.Name = "ClosePanel";
            this.ClosePanel.Size = new System.Drawing.Size(27, 26);
            this.ClosePanel.TabIndex = 17;
            this.ClosePanel.Text = "X";
            this.ClosePanel.UseVisualStyleBackColor = true;
            this.ClosePanel.Click += new System.EventHandler(this.ClosePanel_Click);
            // 
            // AddComponentControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.ClosePanel);
            this.Controls.Add(this.AddComponent);
            this.Controls.Add(this.label);
            this.Controls.Add(this.ComponentList);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.Name = "AddComponentControl";
            this.Size = new System.Drawing.Size(365, 292);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView ComponentList;
        private System.Windows.Forms.Label label;
        private System.Windows.Forms.Button AddComponent;
        private System.Windows.Forms.Button ClosePanel;
    }
}
