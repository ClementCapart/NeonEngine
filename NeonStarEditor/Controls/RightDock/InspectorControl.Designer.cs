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
            this.AddComponent = new System.Windows.Forms.Button();
            this.ComponentList = new System.Windows.Forms.ComboBox();
            this.OpenScript = new System.Windows.Forms.OpenFileDialog();
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
            this.Inspector.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.Inspector.Location = new System.Drawing.Point(40, 0);
            this.Inspector.Name = "Inspector";
            this.Inspector.Size = new System.Drawing.Size(269, 563);
            this.Inspector.TabIndex = 4;
            this.Inspector.Paint += new System.Windows.Forms.PaintEventHandler(this.Inspector_Paint);
            // 
            // AddComponent
            // 
            this.AddComponent.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.AddComponent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddComponent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.AddComponent.Location = new System.Drawing.Point(240, 595);
            this.AddComponent.Name = "AddComponent";
            this.AddComponent.Size = new System.Drawing.Size(69, 23);
            this.AddComponent.TabIndex = 6;
            this.AddComponent.Text = "Add";
            this.AddComponent.UseVisualStyleBackColor = true;
            this.AddComponent.Click += new System.EventHandler(this.AddComponent_Click);
            // 
            // ComponentList
            // 
            this.ComponentList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComponentList.FormattingEnabled = true;
            this.ComponentList.Location = new System.Drawing.Point(40, 595);
            this.ComponentList.Name = "ComponentList";
            this.ComponentList.Size = new System.Drawing.Size(194, 21);
            this.ComponentList.TabIndex = 5;
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
            this.Controls.Add(this.ComponentList);
            this.Controls.Add(this.Inspector);
            this.Controls.Add(this.EntityList);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.Name = "InspectorControl";
            this.Size = new System.Drawing.Size(320, 640);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label EntityList;
        private System.Windows.Forms.Panel Inspector;
        private System.Windows.Forms.Button AddComponent;
        public System.Windows.Forms.ComboBox ComponentList;
        private System.Windows.Forms.OpenFileDialog OpenScript;
    }
}
