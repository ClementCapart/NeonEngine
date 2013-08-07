using Microsoft.Xna.Framework;
using NeonEngine;
using NeonEngine.Private;
using NeonStarLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace NeonStarEditor
{
    public struct PropertyComponentControl
    {

        public PropertyInfo pi;
        public NeonEngine.Component c;
        public Control ctrl;

        public PropertyComponentControl(PropertyInfo pi, NeonEngine.Component c, Control ctrl)
        {
            this.pi = pi;
            this.c = c;
            this.ctrl = ctrl;
        }
    }

    public partial class EntityList : Form
    {
        MainEditor mainEditor;

        PropertyInfo[] propertiesInfo;
        public Entity SelectedEntity;
        Dictionary<Button, NeonEngine.Component> InitButtons = new Dictionary<Button,NeonEngine.Component>();
        Dictionary<Button, NeonEngine.Component> RemoveButtons = new Dictionary<Button, NeonEngine.Component>();

        public List<PropertyComponentControl> PropertyControlList;

        public EntityList(MainEditor mainEditor)
        {
            this.mainEditor = mainEditor;
            InitializeComponent();
            this.EntityListBox.DataSource = mainEditor.currentWorld.entityList;
            this.EntityListBox.DisplayMember = "Name";
            this.EntityListBox.SelectedItem = null;
            List<Type> Components = new List<Type>(Neon.utils.GetTypesInNamespace(Assembly.GetAssembly(typeof(Neon)), "NeonEngine").Where(t => t.IsSubclassOf(typeof(NeonEngine.Component)) && !(t.IsAbstract)));
            if(Neon.Scripts != null)
                Components.AddRange(Neon.Scripts);
            Components.Add(typeof(ScriptComponent));
            ComponentList.DataSource = Components;
            ComponentList.DisplayMember = "Name";
        }

        private void EntityListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            mainEditor.currentWorld.SelectedEntity = (Entity)this.EntityListBox.SelectedItem;
            SelectedEntity = (Entity)this.EntityListBox.SelectedItem;
            if (SelectedEntity != null)
                InstantiateProperties();
            else
                this.Inspector.Controls.Clear();
        }

        public void InstantiateProperties()
        {
            Type type = SelectedEntity.GetType();
            propertiesInfo = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            PropertyControlList = new List<PropertyComponentControl>();
            int Y = 5;

            this.Inspector.Controls.Clear();
            this.RemoveButtons.Clear();
            this.InitButtons.Clear();

            TextBox EntityName = new TextBox();
            EntityName.Text = SelectedEntity.Name;
            EntityName.Font = new Font("Agency FB", 18, FontStyle.Italic);
            EntityName.BorderStyle = BorderStyle.FixedSingle;
            EntityName.TextAlign = HorizontalAlignment.Center;
            EntityName.ForeColor = System.Drawing.Color.FromArgb(240, 240, 240);
            EntityName.Width = 150;
            EntityName.Location = new System.Drawing.Point(this.Inspector.Width / 2 - EntityName.Width / 2, Y);
            EntityName.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            EntityName.KeyPress += EntityName_KeyPress;
            EntityName.AcceptsTab = true;
            EntityName.Validated += EntityName_Validated;
            this.Inspector.Controls.Add(EntityName);
            Y = 45;            

            foreach(NeonEngine.Component c in SelectedEntity.Components)
            {
                GroupBox gb = new GroupBox();
                gb.Text = c.Name;
                gb.Width = Inspector.Width - 20;
                gb.Location = new System.Drawing.Point(0, Y);
                gb.AutoSize = true;
                gb.AutoSizeMode = AutoSizeMode.GrowOnly;

                int localY = 20;
                foreach (PropertyInfo pi in c.GetType().GetProperties())
                {
                    Label label = new Label();
                    label.Text = pi.Name;
                    label.Font = new Font("Calibri", 10);
                    label.Location = new System.Drawing.Point(10, localY);
                    label.AutoSize = true;
                    gb.Controls.Add(label);
                    localY += label.Height + 5;

                    if (pi.PropertyType.Equals(typeof(bool)))
                    {
                        CheckBox checkBox = new CheckBox();
                        checkBox.Checked = (bool)pi.GetValue(c, null);
                        checkBox.Location = new System.Drawing.Point(10, localY);
                        checkBox.CheckedChanged += checkBox_CheckedChanged;
                        PropertyControlList.Add(new PropertyComponentControl(pi, c, checkBox));

                        gb.Controls.Add(checkBox);

                        localY += label.Height + 5;
                    }
                    else if (pi.PropertyType.Equals(typeof(Vector2)))
                    {
                        Vector2 vector = (Vector2)pi.GetValue(c, null);

                        NumericUpDown VectorX = new NumericUpDown();
                        NumericUpDown VectorY = new NumericUpDown();
                        VectorX.DecimalPlaces = 2;
                        VectorY.DecimalPlaces = 2;
                        VectorX.Minimum = decimal.MinValue;
                        VectorX.Maximum = decimal.MaxValue;
                        VectorX.Width = 70;
                        VectorX.Name = "X";

                        PropertyControlList.Add(new PropertyComponentControl(pi, c, VectorX));
                        VectorX.ValueChanged += VectorX_ValueChanged;
                        VectorX.Location = new System.Drawing.Point(10, localY);
                        VectorX.Value = (decimal)vector.X;

                        VectorY.Minimum = decimal.MinValue;
                        VectorY.Maximum = decimal.MaxValue;
                        VectorY.Width = 70;
                        VectorY.Name = "Y";
                        PropertyControlList.Add(new PropertyComponentControl(pi, c, VectorY));
                        VectorY.ValueChanged += VectorY_ValueChanged;
                        VectorY.Location = new System.Drawing.Point(90, localY);
                        VectorY.Value = (decimal)vector.Y;

                        gb.Controls.Add(VectorX);
                        gb.Controls.Add(VectorY);

                        localY += VectorX.Height + 5;
                    }
                    else if (pi.PropertyType.Equals(typeof(float)))
                    {
                        Vector2 NumValue = new Vector2((float)pi.GetValue(c, null), 0f);
                        NumericUpDown number = new NumericUpDown();
                        number.Minimum = decimal.MinValue;
                        number.Maximum = decimal.MaxValue;
                        number.DecimalPlaces = 2;
                        number.Increment = 0.1M;
                        number.Width = 70;
                        PropertyControlList.Add(new PropertyComponentControl(pi, c, number));
                        number.ValueChanged += number_ValueChanged;
                        number.Location = new System.Drawing.Point(10, localY);
                        number.Value = (decimal)NumValue.X;

                        gb.Controls.Add(number);

                        localY += number.Height + 5;
                    }
                    else if (pi.PropertyType.IsEnum)
                    {
                        Enum enumeration = (Enum)pi.GetValue(c, null);
                        ComboBox comboBox = new ComboBox();
                        foreach (Enum e in Enum.GetValues(pi.PropertyType))
                            comboBox.Items.Add(e);
                        comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                        comboBox.Location = new System.Drawing.Point(10, localY);
                        comboBox.SelectedItem = enumeration;
                        PropertyControlList.Add(new PropertyComponentControl(pi, c, comboBox));
                        comboBox.SelectedValueChanged += comboBox_SelectedValueChanged;
                        gb.Controls.Add(comboBox);

                        localY += comboBox.Height + 5;
                    }
                    else if (pi.Name == "SpriteSheetTag")
                    {
                        ComboBox comboBox = new ComboBox();
                        comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                        comboBox.Location = new System.Drawing.Point(10, localY);
                        comboBox.BindingContext = new BindingContext();
                        BindingSource bs = new BindingSource();
                        bs.DataSource = AssetManager.spritesheets.Keys;

                        comboBox.DataSource = bs;
                        comboBox.SelectedItem = (string)pi.GetValue(c, null);
                        gb.Controls.Add(comboBox);
                        PropertyControlList.Add(new PropertyComponentControl(pi, c, comboBox));
                        comboBox.SelectedValueChanged += Spritesheet_SelectedValueChanged;
                        if (comboBox.SelectedIndex == -1)
                            comboBox.SelectedIndex = 0;
                        
                        localY += comboBox.Height + 5;
                    }
                    else if (pi.Name == "GraphicTag")
                    {
                        ComboBox comboBox = new ComboBox();
                        comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                        comboBox.Location = new System.Drawing.Point(10, localY);
                        comboBox.BindingContext = new BindingContext();
                        BindingSource bs = new BindingSource();
                        bs.DataSource = AssetManager.assets.Keys;

                        comboBox.DataSource = bs;
                        comboBox.SelectedItem = (string)pi.GetValue(c, null);
                        gb.Controls.Add(comboBox);
                        PropertyControlList.Add(new PropertyComponentControl(pi, c, comboBox));
                        comboBox.SelectedValueChanged += Spritesheet_SelectedValueChanged;
                        if (comboBox.SelectedIndex == -1)
                            comboBox.SelectedIndex = 0;

                        localY += comboBox.Height + 5;
                    }
                    else if (pi.PropertyType.IsSubclassOf(typeof(NeonEngine.Component)))
                    {
                        BindingSource bs = new BindingSource();
                        bs.DataSource = SelectedEntity.Components.Where(compo => compo.GetType().Equals(pi.PropertyType)).ToArray<NeonEngine.Component>();
                        if (bs.Count > 0)
                        {
                            ComboBox comboBox = new ComboBox();
                            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                            comboBox.Location = new System.Drawing.Point(10, localY);
                            comboBox.BindingContext = new BindingContext();
                            comboBox.DataSource = bs;
                            comboBox.SelectedItem = (NeonEngine.Component)pi.GetValue(c, null);
                            gb.Controls.Add(comboBox);
                            PropertyControlList.Add(new PropertyComponentControl(pi, c, comboBox));
                            comboBox.SelectedValueChanged += Component_SelectedValueChanged;
                            if (comboBox.SelectedIndex == -1)
                                comboBox.SelectedIndex = 0;
                            localY += comboBox.Height + 5;
                        }
                    }
                }

                Y += localY != 20 ? localY + 50 : gb.Height + 10;
                Button InitButton = new Button();
                InitButton.Text = "Reset";
                InitButton.FlatStyle = FlatStyle.Flat;
                InitButton.Location = new System.Drawing.Point(10, localY + 5);
                InitButton.Click += InitButton_Click;
                InitButtons.Add(InitButton, c);
                gb.Controls.Add(InitButton);

                Button RemoveButton = new Button();
                RemoveButton.Text = "Remove";
                RemoveButton.FlatStyle = FlatStyle.Flat;
                RemoveButton.Location = new System.Drawing.Point(InitButton.Width + 15, localY + 5);
                RemoveButton.Click += RemoveButton_Click;
                RemoveButtons.Add(RemoveButton, c);
                gb.Controls.Add(RemoveButton);

                this.Inspector.Controls.Add(gb);
            }
        }

        private void Component_SelectedValueChanged(object sender, EventArgs e)
        {
            PropertyComponentControl pcc = PropertyControlList.First(first => first.ctrl == (Control)sender);
            pcc.pi.SetValue(pcc.c, (NeonEngine.Component)((sender as ComboBox).SelectedValue), null);
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
 	        RemoveButtons[(Button)sender].Remove();
            Entity CurrentEntity = SelectedEntity;
            this.EntityListBox.SelectedItem = null;
            this.EntityListBox.SelectedItem = CurrentEntity;
        }

        void InitButton_Click(object sender, EventArgs e)
        {
            InitButtons[(Button)sender].Init();
        }

        void EntityName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                (sender as TextBox).Parent.Focus();
        }

        void EntityName_Validated(object sender, EventArgs e)
        {
            if (SelectedEntity != null)
            {
                SelectedEntity.Name = (sender as TextBox).Text;
                mainEditor.currentWorld.entityList.ResetItem(mainEditor.currentWorld.entityList.IndexOf(SelectedEntity));
            }
        }

        private void Spritesheet_SelectedValueChanged(object sender, EventArgs e)
        {
            PropertyComponentControl pcc = PropertyControlList.First(first => first.ctrl == (Control)sender);
            pcc.pi.SetValue(pcc.c, (string)((sender as ComboBox).SelectedValue), null);
        }

        void number_ValueChanged(object sender, EventArgs e)
        {
            PropertyComponentControl pcc = PropertyControlList.First(first => first.ctrl == (Control)sender);
            pcc.pi.SetValue(pcc.c, (float)(sender as NumericUpDown).Value, null);
        }

        void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            PropertyComponentControl pcc = PropertyControlList.First(first => first.ctrl == (Control)sender);
            pcc.pi.SetValue(pcc.c, (bool)((sender as CheckBox).Checked), null);
        }

        void button_Click(object sender, EventArgs e)
        {
            mainEditor.currentWorld.CurrentTool = new AddWaypoint(mainEditor.currentWorld);
        }

        private void comboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            PropertyComponentControl pcc = PropertyControlList.First(first => first.ctrl == (Control)sender);
            pcc.pi.SetValue(pcc.c, (Enum)((sender as ComboBox).SelectedItem), null);
        }

        void VectorX_ValueChanged(object sender, EventArgs e)
        {
            PropertyComponentControl pcc = PropertyControlList.First(first => first.ctrl == (Control)sender);
            Vector2 vector = (Vector2)pcc.pi.GetValue(pcc.c, null);
            pcc.pi.SetValue(pcc.c, new Vector2((float)((sender as NumericUpDown).Value), vector.Y), null);
        }
        void VectorY_ValueChanged(object sender, EventArgs e)
        {
            PropertyComponentControl pcc = PropertyControlList.First(first => first.ctrl == (Control)sender);
            Vector2 vector = (Vector2)pcc.pi.GetValue(pcc.c, null);
            pcc.pi.SetValue(pcc.c, new Vector2(vector.X, (float)(sender as NumericUpDown).Value), null);
        }

        private void AddComponent_Click(object sender, EventArgs e)
        {
            if (this.SelectedEntity != null)
            {
                if (ComponentList.Text != "ScriptComponent")
                {
                    Entity CurrentEntity = SelectedEntity;
                    
                    NeonEngine.Component c = (NeonEngine.Component)Activator.CreateInstance((Type)ComponentList.SelectedValue, SelectedEntity);
                    c.Init();
                    c.ID = CurrentEntity.GetLastID() + 1;
                    this.SelectedEntity.AddComponent(c);
                    this.EntityListBox.SelectedItem = null;
                    this.EntityListBox.SelectedItem = CurrentEntity;
                }
                else
                {
                    if (OpenScript.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        Type resultType = Neon.NeonScripting.CompileScript(OpenScript.FileName);
                        List<Type> Components = new List<Type>(Neon.utils.GetTypesInNamespace(Assembly.GetAssembly(typeof(Neon)), "NeonEngine").Where(t => t.IsSubclassOf(typeof(NeonEngine.Component)) && !(t.IsAbstract)));
                        if (Neon.Scripts != null)
                            Components.AddRange(Neon.Scripts);
                        Components.Add(typeof(ScriptComponent));
                        ComponentList.DataSource = Components;
                        ComponentList.DisplayMember = "Name";
                    }
                }
            }
        }

    }
}
