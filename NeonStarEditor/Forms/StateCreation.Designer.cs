namespace NeonStarEditor
{
    partial class StateCreation
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ConditionsList = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.AddCondition = new System.Windows.Forms.Button();
            this.ConditionsAvailable = new System.Windows.Forms.ComboBox();
            this.CurrentConditionSettings = new System.Windows.Forms.DataGridView();
            this.Setting = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ActionsList = new System.Windows.Forms.ListBox();
            this.SelectedState = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.CreateState = new System.Windows.Forms.Button();
            this.SaveState = new System.Windows.Forms.Button();
            this.StateName = new System.Windows.Forms.TextBox();
            this.AddAction = new System.Windows.Forms.Button();
            this.ActionsAvailable = new System.Windows.Forms.ComboBox();
            this.CurrentActionSettings = new System.Windows.Forms.DataGridView();
            this.ActionSetting = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ActionValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label4 = new System.Windows.Forms.Label();
            this.NewAI = new System.Windows.Forms.Button();
            this.SelectedAI = new System.Windows.Forms.ComboBox();
            this.AddToAI = new System.Windows.Forms.Button();
            this.AIName = new System.Windows.Forms.TextBox();
            this.SaveAI = new System.Windows.Forms.Button();
            this.AIStates = new System.Windows.Forms.ListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.ConditionsLinksData = new System.Windows.Forms.DataGridView();
            this.Condition = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LinkedTo = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentConditionSettings)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentActionSettings)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ConditionsLinksData)).BeginInit();
            this.SuspendLayout();
            // 
            // ConditionsList
            // 
            this.ConditionsList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ConditionsList.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConditionsList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ConditionsList.FormattingEnabled = true;
            this.ConditionsList.ItemHeight = 18;
            this.ConditionsList.Location = new System.Drawing.Point(427, 151);
            this.ConditionsList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ConditionsList.Name = "ConditionsList";
            this.ConditionsList.Size = new System.Drawing.Size(383, 94);
            this.ConditionsList.TabIndex = 0;
            this.ConditionsList.SelectedIndexChanged += new System.EventHandler(this.ConditionsList_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Agency FB", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(10, 114);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 34);
            this.label1.TabIndex = 1;
            this.label1.Text = "Actions";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Agency FB", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(421, 114);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(296, 34);
            this.label2.TabIndex = 1;
            this.label2.Text = "Change State Conditions";
            // 
            // AddCondition
            // 
            this.AddCondition.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.AddCondition.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddCondition.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddCondition.Location = new System.Drawing.Point(734, 253);
            this.AddCondition.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.AddCondition.Name = "AddCondition";
            this.AddCondition.Size = new System.Drawing.Size(75, 35);
            this.AddCondition.TabIndex = 2;
            this.AddCondition.Text = "Add";
            this.AddCondition.UseVisualStyleBackColor = true;
            this.AddCondition.Click += new System.EventHandler(this.AddCondition_Click);
            // 
            // ConditionsAvailable
            // 
            this.ConditionsAvailable.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ConditionsAvailable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ConditionsAvailable.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ConditionsAvailable.FormattingEnabled = true;
            this.ConditionsAvailable.Location = new System.Drawing.Point(426, 260);
            this.ConditionsAvailable.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ConditionsAvailable.Name = "ConditionsAvailable";
            this.ConditionsAvailable.Size = new System.Drawing.Size(301, 23);
            this.ConditionsAvailable.TabIndex = 3;
            // 
            // CurrentConditionSettings
            // 
            this.CurrentConditionSettings.AllowUserToAddRows = false;
            this.CurrentConditionSettings.AllowUserToDeleteRows = false;
            this.CurrentConditionSettings.AllowUserToResizeColumns = false;
            this.CurrentConditionSettings.AllowUserToResizeRows = false;
            this.CurrentConditionSettings.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.CurrentConditionSettings.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.CurrentConditionSettings.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.CurrentConditionSettings.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.CurrentConditionSettings.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.CurrentConditionSettings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.CurrentConditionSettings.ColumnHeadersVisible = false;
            this.CurrentConditionSettings.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Setting,
            this.Value});
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.Color.Coral;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.CurrentConditionSettings.DefaultCellStyle = dataGridViewCellStyle11;
            this.CurrentConditionSettings.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.CurrentConditionSettings.Location = new System.Drawing.Point(427, 291);
            this.CurrentConditionSettings.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CurrentConditionSettings.MultiSelect = false;
            this.CurrentConditionSettings.Name = "CurrentConditionSettings";
            this.CurrentConditionSettings.RowHeadersVisible = false;
            this.CurrentConditionSettings.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(127)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.CurrentConditionSettings.RowsDefaultCellStyle = dataGridViewCellStyle12;
            this.CurrentConditionSettings.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.CurrentConditionSettings.Size = new System.Drawing.Size(384, 138);
            this.CurrentConditionSettings.TabIndex = 5;
            this.CurrentConditionSettings.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.CurrentConditionSettings_CellEndEdit);
            this.CurrentConditionSettings.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.CurrentConditionSettings_DataError);
            // 
            // Setting
            // 
            this.Setting.HeaderText = "Setting";
            this.Setting.Name = "Setting";
            this.Setting.ReadOnly = true;
            // 
            // Value
            // 
            this.Value.HeaderText = "Value";
            this.Value.Name = "Value";
            // 
            // ActionsList
            // 
            this.ActionsList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ActionsList.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ActionsList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ActionsList.FormattingEnabled = true;
            this.ActionsList.ItemHeight = 18;
            this.ActionsList.Location = new System.Drawing.Point(15, 151);
            this.ActionsList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ActionsList.Name = "ActionsList";
            this.ActionsList.Size = new System.Drawing.Size(383, 94);
            this.ActionsList.TabIndex = 0;
            this.ActionsList.SelectedIndexChanged += new System.EventHandler(this.ActionsList_SelectedIndexChanged);
            // 
            // SelectedState
            // 
            this.SelectedState.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.SelectedState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SelectedState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.SelectedState.FormattingEnabled = true;
            this.SelectedState.Location = new System.Drawing.Point(498, 22);
            this.SelectedState.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SelectedState.Name = "SelectedState";
            this.SelectedState.Size = new System.Drawing.Size(310, 23);
            this.SelectedState.TabIndex = 3;
            this.SelectedState.SelectedIndexChanged += new System.EventHandler(this.SelectedState_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Agency FB", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(265, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(227, 34);
            this.label3.TabIndex = 1;
            this.label3.Text = "... or select an existing one";
            // 
            // CreateState
            // 
            this.CreateState.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.CreateState.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CreateState.Font = new System.Drawing.Font("Agency FB", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CreateState.Location = new System.Drawing.Point(58, 8);
            this.CreateState.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CreateState.Name = "CreateState";
            this.CreateState.Size = new System.Drawing.Size(161, 46);
            this.CreateState.TabIndex = 2;
            this.CreateState.Text = "Create new State";
            this.CreateState.UseVisualStyleBackColor = true;
            this.CreateState.Click += new System.EventHandler(this.CreateState_Click);
            // 
            // SaveState
            // 
            this.SaveState.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.SaveState.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SaveState.Font = new System.Drawing.Font("Agency FB", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaveState.Location = new System.Drawing.Point(58, 64);
            this.SaveState.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SaveState.Name = "SaveState";
            this.SaveState.Size = new System.Drawing.Size(161, 46);
            this.SaveState.TabIndex = 2;
            this.SaveState.Text = "Save States List";
            this.SaveState.UseVisualStyleBackColor = true;
            this.SaveState.Click += new System.EventHandler(this.SaveState_Click);
            // 
            // StateName
            // 
            this.StateName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.StateName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.StateName.Font = new System.Drawing.Font("Agency FB", 18F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StateName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.StateName.Location = new System.Drawing.Point(300, 63);
            this.StateName.Name = "StateName";
            this.StateName.Size = new System.Drawing.Size(218, 36);
            this.StateName.TabIndex = 6;
            this.StateName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.StateName.TextChanged += new System.EventHandler(this.StateName_TextChanged);
            // 
            // AddAction
            // 
            this.AddAction.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.AddAction.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddAction.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddAction.Location = new System.Drawing.Point(322, 253);
            this.AddAction.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.AddAction.Name = "AddAction";
            this.AddAction.Size = new System.Drawing.Size(75, 35);
            this.AddAction.TabIndex = 2;
            this.AddAction.Text = "Add";
            this.AddAction.UseVisualStyleBackColor = true;
            this.AddAction.Click += new System.EventHandler(this.AddAction_Click);
            // 
            // ActionsAvailable
            // 
            this.ActionsAvailable.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ActionsAvailable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ActionsAvailable.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ActionsAvailable.FormattingEnabled = true;
            this.ActionsAvailable.Location = new System.Drawing.Point(14, 260);
            this.ActionsAvailable.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ActionsAvailable.Name = "ActionsAvailable";
            this.ActionsAvailable.Size = new System.Drawing.Size(301, 23);
            this.ActionsAvailable.TabIndex = 3;
            // 
            // CurrentActionSettings
            // 
            this.CurrentActionSettings.AllowUserToAddRows = false;
            this.CurrentActionSettings.AllowUserToDeleteRows = false;
            this.CurrentActionSettings.AllowUserToResizeColumns = false;
            this.CurrentActionSettings.AllowUserToResizeRows = false;
            this.CurrentActionSettings.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.CurrentActionSettings.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.CurrentActionSettings.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.CurrentActionSettings.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.CurrentActionSettings.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle13;
            this.CurrentActionSettings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.CurrentActionSettings.ColumnHeadersVisible = false;
            this.CurrentActionSettings.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ActionSetting,
            this.ActionValue});
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle14.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.Color.Coral;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.CurrentActionSettings.DefaultCellStyle = dataGridViewCellStyle14;
            this.CurrentActionSettings.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.CurrentActionSettings.Location = new System.Drawing.Point(12, 291);
            this.CurrentActionSettings.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CurrentActionSettings.MultiSelect = false;
            this.CurrentActionSettings.Name = "CurrentActionSettings";
            this.CurrentActionSettings.RowHeadersVisible = false;
            this.CurrentActionSettings.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle15.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(127)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle15.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.CurrentActionSettings.RowsDefaultCellStyle = dataGridViewCellStyle15;
            this.CurrentActionSettings.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.CurrentActionSettings.Size = new System.Drawing.Size(384, 138);
            this.CurrentActionSettings.TabIndex = 5;
            this.CurrentActionSettings.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.CurrentActionSettings_CellEndEdit);
            this.CurrentActionSettings.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.CurrentConditionSettings_DataError);
            // 
            // ActionSetting
            // 
            this.ActionSetting.HeaderText = "Setting";
            this.ActionSetting.Name = "ActionSetting";
            this.ActionSetting.ReadOnly = true;
            // 
            // ActionValue
            // 
            this.ActionValue.HeaderText = "Value";
            this.ActionValue.Name = "ActionValue";
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Agency FB", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(265, 446);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(227, 34);
            this.label4.TabIndex = 1;
            this.label4.Text = "... or select an existing one";
            // 
            // NewAI
            // 
            this.NewAI.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.NewAI.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.NewAI.Font = new System.Drawing.Font("Agency FB", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NewAI.Location = new System.Drawing.Point(58, 437);
            this.NewAI.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.NewAI.Name = "NewAI";
            this.NewAI.Size = new System.Drawing.Size(161, 46);
            this.NewAI.TabIndex = 2;
            this.NewAI.Text = "Create new AI";
            this.NewAI.UseVisualStyleBackColor = true;
            this.NewAI.Click += new System.EventHandler(this.NewAI_Click);
            // 
            // SelectedAI
            // 
            this.SelectedAI.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.SelectedAI.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SelectedAI.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.SelectedAI.FormattingEnabled = true;
            this.SelectedAI.Location = new System.Drawing.Point(498, 453);
            this.SelectedAI.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SelectedAI.Name = "SelectedAI";
            this.SelectedAI.Size = new System.Drawing.Size(310, 23);
            this.SelectedAI.TabIndex = 3;
            // 
            // AddToAI
            // 
            this.AddToAI.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.AddToAI.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddToAI.Font = new System.Drawing.Font("Agency FB", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddToAI.Location = new System.Drawing.Point(650, 64);
            this.AddToAI.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.AddToAI.Name = "AddToAI";
            this.AddToAI.Size = new System.Drawing.Size(161, 46);
            this.AddToAI.TabIndex = 2;
            this.AddToAI.Text = "Add to current AI";
            this.AddToAI.UseVisualStyleBackColor = true;
            this.AddToAI.Click += new System.EventHandler(this.AddToAI_Click);
            // 
            // AIName
            // 
            this.AIName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.AIName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.AIName.Font = new System.Drawing.Font("Agency FB", 18F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AIName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.AIName.Location = new System.Drawing.Point(300, 503);
            this.AIName.Name = "AIName";
            this.AIName.Size = new System.Drawing.Size(218, 36);
            this.AIName.TabIndex = 6;
            this.AIName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.AIName.TextChanged += new System.EventHandler(this.AIName_TextChanged);
            // 
            // SaveAI
            // 
            this.SaveAI.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.SaveAI.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SaveAI.Font = new System.Drawing.Font("Agency FB", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaveAI.Location = new System.Drawing.Point(58, 493);
            this.SaveAI.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SaveAI.Name = "SaveAI";
            this.SaveAI.Size = new System.Drawing.Size(161, 46);
            this.SaveAI.TabIndex = 2;
            this.SaveAI.Text = "Save AIs List";
            this.SaveAI.UseVisualStyleBackColor = true;
            this.SaveAI.Click += new System.EventHandler(this.SaveAI_Click);
            // 
            // AIStates
            // 
            this.AIStates.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.AIStates.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AIStates.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.AIStates.FormattingEnabled = true;
            this.AIStates.ItemHeight = 18;
            this.AIStates.Location = new System.Drawing.Point(27, 592);
            this.AIStates.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.AIStates.Name = "AIStates";
            this.AIStates.Size = new System.Drawing.Size(242, 256);
            this.AIStates.TabIndex = 0;
            this.AIStates.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.AIStates_DrawItem);
            this.AIStates.SelectedIndexChanged += new System.EventHandler(this.AIStates_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Agency FB", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(22, 554);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(99, 34);
            this.label5.TabIndex = 1;
            this.label5.Text = "States";
            // 
            // ConditionsLinksData
            // 
            this.ConditionsLinksData.AllowUserToAddRows = false;
            this.ConditionsLinksData.AllowUserToDeleteRows = false;
            this.ConditionsLinksData.AllowUserToResizeColumns = false;
            this.ConditionsLinksData.AllowUserToResizeRows = false;
            this.ConditionsLinksData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.ConditionsLinksData.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ConditionsLinksData.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ConditionsLinksData.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle16.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            dataGridViewCellStyle16.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle16.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle16.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ConditionsLinksData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle16;
            this.ConditionsLinksData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.ConditionsLinksData.ColumnHeadersVisible = false;
            this.ConditionsLinksData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Condition,
            this.LinkedTo});
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle17.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            dataGridViewCellStyle17.SelectionBackColor = System.Drawing.Color.Coral;
            dataGridViewCellStyle17.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            dataGridViewCellStyle17.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ConditionsLinksData.DefaultCellStyle = dataGridViewCellStyle17;
            this.ConditionsLinksData.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ConditionsLinksData.Location = new System.Drawing.Point(300, 592);
            this.ConditionsLinksData.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ConditionsLinksData.MultiSelect = false;
            this.ConditionsLinksData.Name = "ConditionsLinksData";
            this.ConditionsLinksData.RowHeadersVisible = false;
            this.ConditionsLinksData.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle18.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle18.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            dataGridViewCellStyle18.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(127)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle18.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ConditionsLinksData.RowsDefaultCellStyle = dataGridViewCellStyle18;
            this.ConditionsLinksData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ConditionsLinksData.Size = new System.Drawing.Size(384, 138);
            this.ConditionsLinksData.TabIndex = 5;
            this.ConditionsLinksData.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.ConditionsLinksData_CellEndEdit);
            this.ConditionsLinksData.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.CurrentConditionSettings_DataError);
            // 
            // Condition
            // 
            this.Condition.HeaderText = "Condition";
            this.Condition.Name = "Condition";
            this.Condition.ReadOnly = true;
            // 
            // LinkedTo
            // 
            this.LinkedTo.HeaderText = "LinkedTo";
            this.LinkedTo.Name = "LinkedTo";
            this.LinkedTo.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.LinkedTo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Agency FB", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(295, 554);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(171, 34);
            this.label6.TabIndex = 1;
            this.label6.Text = "Conditions Links";
            // 
            // StateCreation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(829, 880);
            this.Controls.Add(this.AIName);
            this.Controls.Add(this.StateName);
            this.Controls.Add(this.CurrentActionSettings);
            this.Controls.Add(this.ConditionsLinksData);
            this.Controls.Add(this.CurrentConditionSettings);
            this.Controls.Add(this.SelectedAI);
            this.Controls.Add(this.SelectedState);
            this.Controls.Add(this.ActionsAvailable);
            this.Controls.Add(this.ConditionsAvailable);
            this.Controls.Add(this.AddToAI);
            this.Controls.Add(this.SaveAI);
            this.Controls.Add(this.SaveState);
            this.Controls.Add(this.AddAction);
            this.Controls.Add(this.NewAI);
            this.Controls.Add(this.CreateState);
            this.Controls.Add(this.AddCondition);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.AIStates);
            this.Controls.Add(this.ActionsList);
            this.Controls.Add(this.ConditionsList);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "StateCreation";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "State Creation";
            this.TransparencyKey = System.Drawing.Color.Fuchsia;
            ((System.ComponentModel.ISupportInitialize)(this.CurrentConditionSettings)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentActionSettings)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ConditionsLinksData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox ConditionsList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button AddCondition;
        private System.Windows.Forms.ComboBox ConditionsAvailable;
        private System.Windows.Forms.DataGridView CurrentConditionSettings;
        private System.Windows.Forms.DataGridViewTextBoxColumn Setting;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
        private System.Windows.Forms.ListBox ActionsList;
        private System.Windows.Forms.ComboBox SelectedState;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button CreateState;
        private System.Windows.Forms.Button SaveState;
        private System.Windows.Forms.TextBox StateName;
        private System.Windows.Forms.Button AddAction;
        private System.Windows.Forms.ComboBox ActionsAvailable;
        private System.Windows.Forms.DataGridView CurrentActionSettings;
        private System.Windows.Forms.DataGridViewTextBoxColumn ActionSetting;
        private System.Windows.Forms.DataGridViewTextBoxColumn ActionValue;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button NewAI;
        private System.Windows.Forms.ComboBox SelectedAI;
        private System.Windows.Forms.Button AddToAI;
        private System.Windows.Forms.TextBox AIName;
        private System.Windows.Forms.Button SaveAI;
        private System.Windows.Forms.ListBox AIStates;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView ConditionsLinksData;
        private System.Windows.Forms.DataGridViewTextBoxColumn Condition;
        private System.Windows.Forms.DataGridViewComboBoxColumn LinkedTo;
        private System.Windows.Forms.Label label6;
    }
}