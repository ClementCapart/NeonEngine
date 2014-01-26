namespace NeonStarEditor
{
    partial class InspectorControl
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
            this.EntityList = new System.Windows.Forms.Label();
            this.Inspector = new System.Windows.Forms.Panel();
            this.InspectorTab = new System.Windows.Forms.TabControl();
            this.AddComponent = new System.Windows.Forms.Button();
            this.OpenScript = new System.Windows.Forms.OpenFileDialog();
            this.Inspector.SuspendLayout();
            this.SuspendLayout();
            // 
            // EntityList
            // 
            this.EntityList.AutoSize = true;
            this.EntityList.Font = new System.Drawing.Font("Agency FB", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EntityList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.EntityList.Location = new System.Drawing.Point(3, 98);
            this.EntityList.Name = "EntityList";
            this.EntityList.Size = new System.Drawing.Size(25, 306);
            this.EntityList.TabIndex = 3;
            this.EntityList.Text = "I\r\nn\r\ns\r\np\r\ne\r\nc\r\nt\r\no\r\nr";
            // 
            // Inspector
            // 
            this.Inspector.AutoScroll = true;
            this.Inspector.Controls.Add(this.InspectorTab);
            this.Inspector.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.Inspector.Location = new System.Drawing.Point(40, 0);
            this.Inspector.Name = "Inspector";
            this.Inspector.Size = new System.Drawing.Size(325, 595);
            this.Inspector.TabIndex = 4;
            // 
            // InspectorTab
            // 
            this.InspectorTab.Appearance = System.Windows.Forms.TabAppearance.Buttons;
            this.InspectorTab.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.InspectorTab.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InspectorTab.ItemSize = new System.Drawing.Size(156, 20);
            this.InspectorTab.Location = new System.Drawing.Point(3, 98);
            this.InspectorTab.Multiline = true;
            this.InspectorTab.Name = "InspectorTab";
            this.InspectorTab.SelectedIndex = 0;
            this.InspectorTab.Size = new System.Drawing.Size(320, 497);
            this.InspectorTab.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.InspectorTab.TabIndex = 0;
            // 
            // AddComponent
            // 
            this.AddComponent.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.AddComponent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddComponent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.AddComponent.Location = new System.Drawing.Point(133, 605);
            this.AddComponent.Name = "AddComponent";
            this.AddComponent.Size = new System.Drawing.Size(141, 23);
            this.AddComponent.TabIndex = 6;
            this.AddComponent.Text = "Add Component...";
            this.AddComponent.UseVisualStyleBackColor = true;
            this.AddComponent.Click += new System.EventHandler(this.AddComponent_Click);
            // 
            // OpenScript
            // 
            this.OpenScript.Filter = "\"Fichiers NeonStar C#|*.nscs";
            // 
            // InspectorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Controls.Add(this.AddComponent);
            this.Controls.Add(this.Inspector);
            this.Controls.Add(this.EntityList);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.Name = "InspectorControl";
            this.Size = new System.Drawing.Size(368, 640);
            this.Inspector.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label EntityList;
        private System.Windows.Forms.Panel Inspector;
        private System.Windows.Forms.Button AddComponent;
        private System.Windows.Forms.OpenFileDialog OpenScript;
        private System.Windows.Forms.TabControl InspectorTab;
    }
}
