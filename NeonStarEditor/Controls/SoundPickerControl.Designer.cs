namespace NeonStarEditor.Controls
{
    partial class SoundPickerControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SoundPickerControl));
            this.EntityList = new System.Windows.Forms.Label();
            this.soundList = new System.Windows.Forms.TreeView();
            this.entityInfo = new System.Windows.Forms.Label();
            this.PlayButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.Length = new System.Windows.Forms.Label();
            this.SelectButton = new System.Windows.Forms.Button();
            this.PlayLoopButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // EntityList
            // 
            this.EntityList.AutoSize = true;
            this.EntityList.Font = new System.Drawing.Font("Agency FB", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EntityList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.EntityList.Location = new System.Drawing.Point(126, 0);
            this.EntityList.Name = "EntityList";
            this.EntityList.Size = new System.Drawing.Size(91, 25);
            this.EntityList.TabIndex = 15;
            this.EntityList.Text = "Sound Picker";
            // 
            // soundList
            // 
            this.soundList.Location = new System.Drawing.Point(3, 53);
            this.soundList.Name = "soundList";
            this.soundList.Size = new System.Drawing.Size(191, 455);
            this.soundList.TabIndex = 16;
            this.soundList.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.soundList_AfterSelect);
            // 
            // entityInfo
            // 
            this.entityInfo.AutoSize = true;
            this.entityInfo.Location = new System.Drawing.Point(3, 30);
            this.entityInfo.Name = "entityInfo";
            this.entityInfo.Size = new System.Drawing.Size(51, 13);
            this.entityInfo.TabIndex = 17;
            this.entityInfo.Text = "EntityInfo";
            // 
            // PlayButton
            // 
            this.PlayButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PlayButton.Image = ((System.Drawing.Image)(resources.GetObject("PlayButton.Image")));
            this.PlayButton.Location = new System.Drawing.Point(210, 86);
            this.PlayButton.Name = "PlayButton";
            this.PlayButton.Size = new System.Drawing.Size(61, 58);
            this.PlayButton.TabIndex = 18;
            this.PlayButton.UseVisualStyleBackColor = true;
            this.PlayButton.Click += new System.EventHandler(this.PlayButton_Click);
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.Location = new System.Drawing.Point(287, 86);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(61, 58);
            this.button1.TabIndex = 18;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(222, 220);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "Length : ";
            // 
            // Length
            // 
            this.Length.AutoSize = true;
            this.Length.Location = new System.Drawing.Point(287, 220);
            this.Length.Name = "Length";
            this.Length.Size = new System.Drawing.Size(34, 13);
            this.Length.TabIndex = 20;
            this.Length.Text = "00:00";
            // 
            // SelectButton
            // 
            this.SelectButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SelectButton.Location = new System.Drawing.Point(233, 255);
            this.SelectButton.Name = "SelectButton";
            this.SelectButton.Size = new System.Drawing.Size(88, 36);
            this.SelectButton.TabIndex = 21;
            this.SelectButton.Text = "Select";
            this.SelectButton.UseVisualStyleBackColor = true;
            this.SelectButton.Click += new System.EventHandler(this.SelectButton_Click);
            // 
            // PlayLoopButton
            // 
            this.PlayLoopButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PlayLoopButton.Image = ((System.Drawing.Image)(resources.GetObject("PlayLoopButton.Image")));
            this.PlayLoopButton.Location = new System.Drawing.Point(210, 152);
            this.PlayLoopButton.Name = "PlayLoopButton";
            this.PlayLoopButton.Size = new System.Drawing.Size(61, 58);
            this.PlayLoopButton.TabIndex = 18;
            this.PlayLoopButton.UseVisualStyleBackColor = true;
            this.PlayLoopButton.Click += new System.EventHandler(this.PlayLoopButton_Click);
            // 
            // SoundPickerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.SelectButton);
            this.Controls.Add(this.Length);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.PlayLoopButton);
            this.Controls.Add(this.PlayButton);
            this.Controls.Add(this.entityInfo);
            this.Controls.Add(this.soundList);
            this.Controls.Add(this.EntityList);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.Name = "SoundPickerControl";
            this.Size = new System.Drawing.Size(362, 515);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label EntityList;
        private System.Windows.Forms.TreeView soundList;
        private System.Windows.Forms.Label entityInfo;
        private System.Windows.Forms.Button PlayButton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label Length;
        private System.Windows.Forms.Button SelectButton;
        private System.Windows.Forms.Button PlayLoopButton;
    }
}
