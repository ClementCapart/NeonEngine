using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NeonEngine;
using NeonEngine.Private;
using System.Reflection;
using Microsoft.Xna.Framework;
using System.Xml.Linq;
using Color = System.Drawing.Color;
using Component = NeonEngine.Component;
using Point = System.Drawing.Point;

namespace NeonStarEditor
{
    public struct PropertyComponentControl
    {

        public PropertyInfo pi;
        public Component c;
        public Control ctrl;

        public PropertyComponentControl(PropertyInfo pi, Component c, Control ctrl)
        {
            this.pi = pi;
            this.c = c;
            this.ctrl = ctrl;
        }
    }

    public partial class InspectorControl : UserControl
    {
        public EditorScreen GameWorld;
        PropertyInfo[] propertiesInfo;

        Dictionary<Button, Component> InitButtons = new Dictionary<Button, Component>();
        Dictionary<Button, Component> RemoveButtons = new Dictionary<Button, Component>();

        public List<PropertyComponentControl> PropertyControlList = new List<PropertyComponentControl>();


        public InspectorControl()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            if (GameWorld != null)
            {
                List<Type> Components = new List<Type>(Neon.utils.GetTypesInNamespace(Assembly.GetAssembly(typeof(Neon)), "NeonEngine").Where(t => t.IsSubclassOf(typeof(Component)) && !(t.IsAbstract)));
                if (Neon.Scripts != null)
                    Components.AddRange(Neon.Scripts);
                Components.Add(typeof(ScriptComponent));
                Components.AddRange(Neon.utils.GetTypesInNamespace(Assembly.GetAssembly(typeof(NeonStarLibrary.GameScreen)), "NeonStarLibrary").Where(t => t.IsSubclassOf(typeof(Component)) && !(t.IsAbstract)));
                Components = Components.OrderBy(c => c.Name).ToList();
                ComponentList.DataSource = Components;
                ComponentList.DisplayMember = "Name";
            }
            base.OnLoad(e);
        }

        public void ClearInspector()
        {
            Inspector.Controls.Clear();
            InspectorTab.TabPages.Clear();
            RemoveButtons.Clear();
            InitButtons.Clear();

            Label NoEntity = new Label();
            NoEntity.AutoSize = true;
            NoEntity.Text = "No entity selected !";
            NoEntity.Font = new Font("Agency FB", 24, FontStyle.Regular);
            NoEntity.Location = new Point(60, 250);
            
            Inspector.Controls.Add(NoEntity);
        }

        public void InstantiateProperties(Entity SelectedEntity)
        {
            Type type = SelectedEntity.GetType();
            propertiesInfo = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            PropertyControlList = new List<PropertyComponentControl>();
            int Y = 5;
            InspectorTab.TabPages.Clear();
            Inspector.Controls.Clear();
            RemoveButtons.Clear();
            InitButtons.Clear();

            TextBox EntityName = new TextBox();
            EntityName.Text = SelectedEntity.Name;
            EntityName.Font = new Font("Agency FB", 18, FontStyle.Italic);
            EntityName.BorderStyle = BorderStyle.FixedSingle;
            EntityName.TextAlign = HorizontalAlignment.Center;
            EntityName.ForeColor = Color.FromArgb(240, 240, 240);
            EntityName.Width = 150;
            EntityName.Location = new Point(Inspector.Width / 2 - EntityName.Width / 2, Y);
            EntityName.BackColor = Color.FromArgb(64, 64, 64);
            EntityName.GotFocus += EntityName_GotFocus;
            EntityName.LostFocus += EntityName_LostFocus;
            EntityName.AcceptsTab = true;
            Inspector.Controls.Add(EntityName);
            Y = 45;

            foreach (Component c in SelectedEntity.Components)
            {                
                TabPage tp = new TabPage();
                tp.Text = c.Name;
                tp.Location = new Point(0, 0);
                tp.AutoScroll = true;
                tp.AutoScrollMargin = new Size(0, 20);
                tp.BackColor = Color.FromArgb(255, 64, 64, 64);
                tp.ForeColor = Color.FromArgb(255, 240, 240, 240);
                tp.BorderStyle = System.Windows.Forms.BorderStyle.None;

                int localY = 20;
                foreach (PropertyInfo pi in c.GetType().GetProperties())
                {
                    Label label = new Label();
                    label.Text = pi.Name;
                    label.Font = new Font("Calibri", 10);
                    label.Location = new Point(10, localY);
                    label.AutoSize = true;
                    tp.Controls.Add(label);
                    localY += label.Height + 5;

                    if (pi.PropertyType.Equals(typeof(bool)))
                    {
                        CheckBox checkBox = new CheckBox();
                        checkBox.Checked = (bool)pi.GetValue(c, null);
                        checkBox.Location = new Point(10, localY);
                        checkBox.CheckedChanged += checkBox_CheckedChanged;
                        PropertyControlList.Add(new PropertyComponentControl(pi, c, checkBox));

                        tp.Controls.Add(checkBox);

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
                        VectorX.GotFocus += VectorX_GotFocus;
                        VectorX.Location = new Point(10, localY);
                        VectorX.Value = (decimal)vector.X;

                        VectorY.Minimum = decimal.MinValue;
                        VectorY.Maximum = decimal.MaxValue;
                        VectorY.Width = 70;
                        VectorY.Name = "Y";
                        PropertyControlList.Add(new PropertyComponentControl(pi, c, VectorY));
                        VectorY.ValueChanged += VectorY_ValueChanged;
                        VectorY.Location = new Point(90, localY);
                        VectorY.Value = (decimal)vector.Y;
                        VectorY.GotFocus += VectorX_GotFocus;

                        VectorX.LostFocus += VectorX_LostFocus;
                        VectorY.LostFocus += VectorX_LostFocus;

                        tp.Controls.Add(VectorX);
                        tp.Controls.Add(VectorY);

                        localY += VectorX.Height + 5;
                    }
                    else if (pi.Name == "SpriteSheetTag")
                    {
                        ComboBox comboBox = new ComboBox();
                        comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                        comboBox.Location = new Point(10, localY);
                        comboBox.BindingContext = new BindingContext();
                        BindingSource bs = new BindingSource();
                        bs.DataSource = AssetManager.Spritesheets.Keys.OrderBy(k => k.ToString());

                        comboBox.DataSource = bs;
                        comboBox.SelectedItem = (string)pi.GetValue(c, null);
                        tp.Controls.Add(comboBox);
                        PropertyControlList.Add(new PropertyComponentControl(pi, c, comboBox));
                        comboBox.SelectedValueChanged += Spritesheet_SelectedValueChanged;
                        if (comboBox.SelectedIndex == -1)
                            comboBox.SelectedIndex = 0;

                        localY += comboBox.Height + 5;
                    }
                    else if (pi.Name == "Font")
                    {
                        ComboBox comboBox = new ComboBox();
                        comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                        comboBox.Location = new Point(10, localY);
                        comboBox.BindingContext = new BindingContext();
                        BindingSource bs = new BindingSource();
                        bs.DataSource = TextManager.FontList.Keys;

                        comboBox.DataSource = bs;
                        comboBox.SelectedItem = pi.GetValue(c, null) != null ? TextManager.FontList.Where(kvp => kvp.Value == pi.GetValue(c, null)).First().Key : null;

                        tp.Controls.Add(comboBox);
                        PropertyControlList.Add(new PropertyComponentControl(pi, c, comboBox));
                        comboBox.SelectedValueChanged += SpriteFont_SelectedValueChanged;
                        if (comboBox.SelectedIndex == -1)
                            comboBox.SelectedIndex = 0;

                        localY += comboBox.Height + 5;
                    }
                    else if (pi.Name == "GraphicTag")
                    {
                        ComboBox comboBox = new ComboBox();
                        comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                        comboBox.Location = new Point(10, localY);
                        comboBox.BindingContext = new BindingContext();
                        BindingSource bs = new BindingSource();
                        bs.DataSource = AssetManager.Assets.Keys.OrderBy(k => k.ToString());

                        comboBox.DataSource = bs;
                        comboBox.SelectedItem = (string)pi.GetValue(c, null);
                        tp.Controls.Add(comboBox);
                        PropertyControlList.Add(new PropertyComponentControl(pi, c, comboBox));
                        comboBox.SelectedValueChanged += Spritesheet_SelectedValueChanged;
                        if (comboBox.SelectedIndex == -1)
                            comboBox.SelectedIndex = 0;

                        localY += comboBox.Height + 5;
                    }
                    else if (pi.PropertyType.Equals(typeof(Microsoft.Xna.Framework.Color)))
                    {
                        Button chooseColor = new Button();
                        chooseColor.Text = "Color";
                        chooseColor.Location = new Point(10, localY);
                        tp.Controls.Add(chooseColor);
                        PropertyControlList.Add(new PropertyComponentControl(pi, c, chooseColor));
                        chooseColor.Click += chooseColor_Click;
                        localY += chooseColor.Height + 5;
                    }
                    else if (pi.PropertyType.Equals(typeof(string)))
                    {
                        TextBox tb = new TextBox();
                        tb.Location = new Point(10, localY);
                        tb.Width = 70;
                        PropertyControlList.Add(new PropertyComponentControl(pi, c, tb));
                        tb.GotFocus += tb_GotFocus;
                        tb.LostFocus += tb_LostFocus;
                        tb.Text = (string)pi.GetValue(c, null);

                        tp.Controls.Add(tb);

                        localY += tb.Height + 5;

                    }
                    else if (pi.PropertyType.Equals(typeof(float)))
                    {
                        Vector2 NumValue = new Vector2((float)pi.GetValue(c, null), 0f);
                        NumericUpDown number = new NumericUpDown();
                        number.Minimum = decimal.MinValue;
                        number.Maximum = decimal.MaxValue;
                        number.DecimalPlaces = 3;
                        number.Increment = 0.1M;
                        number.Width = 70;
                        PropertyControlList.Add(new PropertyComponentControl(pi, c, number));
                        number.ValueChanged += number_ValueChanged;
                        number.Location = new Point(10, localY);
                        number.Value = (decimal)NumValue.X;
                        number.GotFocus += VectorX_GotFocus;
                        number.LostFocus += VectorX_LostFocus;
                        tp.Controls.Add(number);

                        localY += number.Height + 5;
                    }
                    else if (pi.PropertyType.IsEnum)
                    {
                        Enum enumeration = (Enum)pi.GetValue(c, null);
                        ComboBox comboBox = new ComboBox();
                        foreach (Enum e in Enum.GetValues(pi.PropertyType))
                            comboBox.Items.Add(e);
                        comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                        comboBox.Location = new Point(10, localY);
                        comboBox.SelectedItem = enumeration;
                        PropertyControlList.Add(new PropertyComponentControl(pi, c, comboBox));
                        comboBox.SelectedValueChanged += comboBox_SelectedValueChanged;
                        tp.Controls.Add(comboBox);

                        localY += comboBox.Height + 5;
                    }
                    else if (pi.PropertyType.IsSubclassOf(typeof(Component)))
                    {
                        BindingSource bs = new BindingSource();
                        bs.DataSource = SelectedEntity.Components.Where(compo => compo.GetType().Equals(pi.PropertyType)).ToArray<Component>();
                        if (bs.Count > 0)
                        {
                            ComboBox comboBox = new ComboBox();
                            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                            comboBox.Location = new Point(10, localY);
                            comboBox.BindingContext = new BindingContext();
                            comboBox.DataSource = bs;
                            comboBox.SelectedItem = (Component)pi.GetValue(c, null);
                            tp.Controls.Add(comboBox);
                            PropertyControlList.Add(new PropertyComponentControl(pi, c, comboBox));
                            comboBox.SelectedValueChanged += Component_SelectedValueChanged;
                            if (comboBox.SelectedIndex == -1)
                                comboBox.SelectedIndex = 0;
                            localY += comboBox.Height + 5;
                        }
                    }
                    else if (pi.Name == "Spritesheets")
                    {
                        SpritesheetInspector ssinspector = new SpritesheetInspector(SelectedEntity.spritesheets.Spritesheets, GameWorld);
                        ssinspector.Location = new Point(10, localY);
                        tp.Controls.Add(ssinspector);

                        localY += ssinspector.Height + 5;
                    }
                    else if (pi.PropertyType.Equals(typeof(PathNodeList)))
                    {
                        ComboBox cb = new ComboBox();
                        cb.DropDownStyle = ComboBoxStyle.DropDownList;
                        cb.Location = new Point(10, localY);
                        cb.BindingContext = new BindingContext();
                        cb.DataSource = GameWorld.NodeLists;
                        cb.DisplayMember = "Name";
                        tp.Controls.Add(cb);
                        cb.SelectedIndex = GameWorld.NodeLists.IndexOf((PathNodeList)pi.GetValue(c, null));


                        PropertyControlList.Add(new PropertyComponentControl(pi, c, cb));

                        cb.SelectedValueChanged += cb_SelectedValueChanged;
                        localY += cb.Height + 5;
                    }
                }

                Y += localY != 20 ? localY + 50 : tp.Height + 10;
                Button InitButton = new Button();
                InitButton.Text = "Init";
                InitButton.FlatStyle = FlatStyle.Flat;
                InitButton.Location = new Point(10, localY + 5);
                InitButton.Click += InitButton_Click;
                InitButtons.Add(InitButton, c);
                tp.Controls.Add(InitButton);

                Button RemoveButton = new Button();
                RemoveButton.Text = "Remove";
                RemoveButton.FlatStyle = FlatStyle.Flat;
                RemoveButton.Location = new Point(InitButton.Width + 15, localY + 5);
                RemoveButton.Click += RemoveButton_Click;
                RemoveButtons.Add(RemoveButton, c);
                tp.Controls.Add(RemoveButton);

                tp.BorderStyle = System.Windows.Forms.BorderStyle.None;
                this.InspectorTab.DrawItem += InspectorTab_DrawItem;
                try
                {
                    this.InspectorTab.TabPages.Add(tp);
                }
                catch
                {
                    Console.WriteLine("Exception while creation window handle !");
                }
                
                tp.Height += 20;


            }

                if (InspectorTab.TabPages.Count % 2 == 1)
                {
                    TabPage tp2 = new TabPage("");
                    InspectorTab.TabPages.Add(tp2);
                    tp2.BackColor = Color.FromArgb(255, 64, 64, 64);
                }

            Inspector.Controls.Add(InspectorTab);
        }

        void InspectorTab_DrawItem(object sender, DrawItemEventArgs e)
        {
            TabPage CurrentTab = InspectorTab.TabPages[e.Index];
            System.Drawing.Rectangle ItemRect = InspectorTab.GetTabRect(e.Index);
            SolidBrush FillBrush = new SolidBrush(Color.Red);
            SolidBrush TextBrush = new SolidBrush(Color.White);
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;

            //If we are currently painting the Selected TabItem we'll
            //change the brush colors and inflate the rectangle.
            if (System.Convert.ToBoolean(e.State & DrawItemState.Selected))
            {
                FillBrush.Color = Color.FromArgb(255, 50, 50, 50);
                TextBrush.Color = Color.FromArgb(255, 240, 240, 240);
                ItemRect.Inflate(2, 2);
            }
            else
            {
                FillBrush.Color = Color.FromArgb(255, 64, 64, 64);
                TextBrush.Color = Color.FromArgb(255, 240, 240, 240);
            }

            //Set up rotation for left and right aligned tabs
            if (InspectorTab.Alignment == TabAlignment.Left || InspectorTab.Alignment == TabAlignment.Right)
            {
                float RotateAngle = 90;
                if (InspectorTab.Alignment == TabAlignment.Left)
                    RotateAngle = 270;
                PointF cp = new PointF(ItemRect.Left + (ItemRect.Width / 2), ItemRect.Top + (ItemRect.Height / 2));
                e.Graphics.TranslateTransform(cp.X, cp.Y);
                e.Graphics.RotateTransform(RotateAngle);
                ItemRect = new System.Drawing.Rectangle(-(ItemRect.Height / 2), -(ItemRect.Width / 2), ItemRect.Height, ItemRect.Width);
            }

            //Next we'll paint the TabItem with our Fill Brush
            e.Graphics.FillRectangle(FillBrush, ItemRect);

            //Now draw the text.
            e.Graphics.DrawString(CurrentTab.Text, e.Font, TextBrush, (RectangleF)ItemRect, sf);

            //Reset any Graphics rotation
            e.Graphics.ResetTransform();

            //Finally, we should Dispose of our brushes.
            FillBrush.Dispose();
            TextBrush.Dispose();
        }

        private void SpriteFont_SelectedValueChanged(object sender, EventArgs e)
        {
            PropertyComponentControl pcc;
            try
            {
                pcc = PropertyControlList.First(first => first.ctrl == (Control)sender);
            }
            catch (InvalidOperationException exc)
            {
                Console.WriteLine(exc.ToString());
                return;
            }
            pcc.pi.SetValue(pcc.c, TextManager.FontList[(string)((sender as ComboBox).SelectedValue)], null);
        }

        void cb_SelectedValueChanged(object sender, EventArgs e)
        {
            PropertyComponentControl pcc;
            try
            {
                pcc = PropertyControlList.First(first => first.ctrl == (Control)sender);
            }
            catch (InvalidOperationException exc)
            {
                Console.WriteLine(exc.ToString());
                return;
            }
            pcc.pi.SetValue(pcc.c, (PathNodeList)((sender as ComboBox).SelectedValue), null);
        }

        void chooseColor_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.AllowFullOpen = true;
            PropertyComponentControl pcc = PropertyControlList.First(first => first.ctrl == (Control)sender);          
            Microsoft.Xna.Framework.Color xnaColor = (Microsoft.Xna.Framework.Color)pcc.pi.GetValue(pcc.c, null);
            colorDialog.Color = Color.FromArgb(xnaColor.A, xnaColor.R, xnaColor.G, xnaColor.B);
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                pcc.pi.SetValue(pcc.c, new Microsoft.Xna.Framework.Color(colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B, colorDialog.Color.A), null);
            }
        }

        void tb_LostFocus(object sender, EventArgs e)
        {
            if (GameWorld.FocusedTextBox == (sender as TextBox))
                GameWorld.FocusedTextBox = null;

            PropertyComponentControl pcc;
            try
            {
                pcc = PropertyControlList.First(first => first.ctrl == (Control)sender);
            }
            catch (InvalidOperationException exc)
            {
                Console.WriteLine(exc.ToString());
                return;
            }
            pcc.pi.SetValue(pcc.c, ((sender as TextBox).Text), null);
        }

        void tb_GotFocus(object sender, EventArgs e)
        {
            GameWorld.FocusedTextBox = (sender as TextBox);
        }

        void VectorX_LostFocus(object sender, EventArgs e)
        {
            if(GameWorld.FocusedNumericUpDown == (sender as NumericUpDown))
                GameWorld.FocusedNumericUpDown = null;
        }

        void VectorX_GotFocus(object sender, EventArgs e)
        {
            GameWorld.FocusedNumericUpDown = (sender as NumericUpDown);
        }

        void EntityName_LostFocus(object sender, EventArgs e)
        {
            if (GameWorld.EntityChangedThisFrame)
                return;

            if (GameWorld.FocusedTextBox == (sender as TextBox))
            {
                GameWorld.FocusedTextBox = null;
                GameWorld.SelectedEntity.Name = (sender as TextBox).Text;
                GameWorld.BottomDockControl.entityListControl.EntityListBox.DataSource = null;
                GameWorld.BottomDockControl.entityListControl.EntityListBox.DataSource = GameWorld.entities;
                GameWorld.BottomDockControl.entityListControl.EntityListBox.DisplayMember = "Name";
            }
        }

        void EntityName_GotFocus(object sender, EventArgs e)
        {
            GameWorld.FocusedTextBox = (sender as TextBox);
        }

        private void Component_SelectedValueChanged(object sender, EventArgs e)
        {
            PropertyComponentControl pcc;
            try
            {
                pcc = PropertyControlList.First(first => first.ctrl == (Control)sender);
            }
            catch (InvalidOperationException exc)
            {
                Console.WriteLine(exc.ToString());
                return;
            }
            pcc.pi.SetValue(pcc.c, (Component)((sender as ComboBox).SelectedValue), null);
        }

        void InitButton_Click(object sender, EventArgs e)
        {
            InitButtons[(Button)sender].Init();
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            ActionManager.SaveAction(ActionType.DeleteComponent, new object[4] { RemoveButtons[(Button)sender].GetType(), DataManager.SaveComponentParameters(RemoveButtons[(Button)sender] as Component), GameWorld.SelectedEntity, this });
        
            RemoveButtons[(Button)sender].Remove();
            Entity CurrentEntity = GameWorld.SelectedEntity;
            InstantiateProperties(CurrentEntity);
        }    

        private void Spritesheet_SelectedValueChanged(object sender, EventArgs e)
        {
            PropertyComponentControl pcc;
            try
            {
                pcc = PropertyControlList.First(first => first.ctrl == (Control)sender);
            }
            catch (InvalidOperationException exc)
            {
                Console.WriteLine(exc.ToString());
                return;
            }
            pcc.pi.SetValue(pcc.c, (string)((sender as ComboBox).SelectedValue), null);
            
        }

        void number_ValueChanged(object sender, EventArgs e)
        {

            PropertyComponentControl pcc;
            try
            {
                pcc = PropertyControlList.First(first => first.ctrl == (Control)sender);
            }
            catch (InvalidOperationException exc)
            {
                Console.WriteLine(exc.ToString());
                return;
            }
            
            pcc.pi.SetValue(pcc.c, (float)(sender as NumericUpDown).Value, null);
        }

        void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            PropertyComponentControl pcc;
            try
            {
                pcc = PropertyControlList.First(first => first.ctrl == (Control)sender);
            }
            catch (InvalidOperationException exc)
            {
                Console.WriteLine(exc.ToString());
                return;
            }

            pcc.pi.SetValue(pcc.c, (bool)((sender as CheckBox).Checked), null);
        }


        private void comboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            PropertyComponentControl pcc;
            try
            {
                pcc = PropertyControlList.First(first => first.ctrl == (Control)sender);
            }
            catch (InvalidOperationException exc)
            {
                Console.WriteLine(exc.ToString());
                return;
            }
            pcc.pi.SetValue(pcc.c, (Enum)((sender as ComboBox).SelectedItem), null);
        }

        void VectorX_ValueChanged(object sender, EventArgs e)
        {
            if (!GameWorld.ManagingInspector)
            {
                PropertyComponentControl pcc;
                try
                {
                    pcc = PropertyControlList.First(first => first.ctrl == (Control)sender);
                }
                catch (InvalidOperationException exc)
                {
                    Console.WriteLine(exc.ToString());
                    return;
                }
                Vector2 vector = (Vector2)pcc.pi.GetValue(pcc.c, null);
                pcc.pi.SetValue(pcc.c, new Vector2((float)((sender as NumericUpDown).Value), vector.Y), null);
            }
            
        }
        void VectorY_ValueChanged(object sender, EventArgs e)
        {
            if (!GameWorld.ManagingInspector)
            {
                PropertyComponentControl pcc;
                try
                {
                    pcc = PropertyControlList.First(first => first.ctrl == (Control)sender);
                }
                catch (InvalidOperationException exc)
                {
                    Console.WriteLine(exc.ToString());
                    return;
                }

                Vector2 vector = (Vector2)pcc.pi.GetValue(pcc.c, null);
                pcc.pi.SetValue(pcc.c, new Vector2(vector.X, (float)(sender as NumericUpDown).Value), null); 
            }           
        }

        private void AddComponent_Click(object sender, EventArgs e)
        {
            if (GameWorld.SelectedEntity != null)
            {
                if (ComponentList.Text != "ScriptComponent")
                {
                    Entity CurrentEntity = GameWorld.SelectedEntity;

                    Component c = (Component)Activator.CreateInstance((Type)ComponentList.SelectedValue, CurrentEntity);
                    c.Init();
                    c.ID = CurrentEntity.GetLastID() + 1;
                    CurrentEntity.AddComponent(c);
                    ActionManager.SaveAction(ActionType.AddComponent, new object[2] { c, this });
                    InstantiateProperties(CurrentEntity);
                }
                else
                {
                    if (OpenScript.ShowDialog() == DialogResult.OK)
                    {
                        Type resultType = Neon.NeonScripting.CompileScript(OpenScript.FileName);
                        List<Type> Components = new List<Type>(Neon.utils.GetTypesInNamespace(Assembly.GetAssembly(typeof(Neon)), "NeonEngine").Where(t => t.IsSubclassOf(typeof(Component)) && !(t.IsAbstract)));
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
