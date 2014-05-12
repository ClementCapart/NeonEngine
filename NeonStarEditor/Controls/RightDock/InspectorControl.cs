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
using NeonEngine.Components.Graphics2D;
using NeonEngine.Components.Audio;
using NeonEngine.Components.Private;

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
        public XElement ClipboardComponent = null;

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
            base.OnLoad(e);
        }

        public void ClearInspector()
        {
            Inspector.Controls.Clear();
            

            for (int i = InspectorTab.TabPages.Count - 1; i >= 0; i--)
            {
                for (int j = InspectorTab.TabPages[i].Controls.Count - 1; j >= 0; j--)
                {
                    InspectorTab.TabPages[i].Controls[j].Dispose();
                }
                InspectorTab.TabPages[i].Dispose();
            }

            for (int i = RemoveButtons.Keys.Count - 1; i >= 0; i--)
            {
                RemoveButtons.Keys.ElementAt(i).Dispose();
            }

            for (int i = InitButtons.Keys.Count - 1; i >= 0; i--)
            {
                InitButtons.Keys.ElementAt(i).Dispose();
            }

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

            for (int i = Inspector.Controls.Count - 1; i >= 0; i--)
            {
                if(Inspector.Controls[i].Name != "InspectorTab")
                    Inspector.Controls[i].Dispose();
            }

            for (int i = InspectorTab.TabPages.Count - 1; i >= 0; i--)
            {
                for (int j = InspectorTab.TabPages[i].Controls.Count - 1; j >= 0; j--)
                {
                    InspectorTab.TabPages[i].Controls[j].Dispose();
                }
                InspectorTab.TabPages[i].Dispose();
            }

            for (int i = RemoveButtons.Keys.Count - 1; i >= 0; i--)
            {
                RemoveButtons.Keys.ElementAt(i).Dispose();
            }

            for (int i = InitButtons.Keys.Count - 1; i >= 0; i--)
            {
                InitButtons.Keys.ElementAt(i).Dispose();
            }

            TextBox entityName = new TextBox();
            entityName.Text = SelectedEntity.Name;
            entityName.Font = new Font("Agency FB", 18, FontStyle.Italic);
            entityName.BorderStyle = BorderStyle.FixedSingle;
            entityName.TextAlign = HorizontalAlignment.Center;
            entityName.ForeColor = Color.FromArgb(240, 240, 240);
            entityName.Width = 150;
            entityName.Location = new Point(Inspector.Width / 2 - entityName.Width / 2, Y);
            entityName.BackColor = Color.FromArgb(64, 64, 64);
            entityName.GotFocus += EntityName_GotFocus;
            entityName.LostFocus += EntityName_LostFocus;
            entityName.AcceptsTab = true;
            Inspector.Controls.Add(entityName);

            Label layerLabel = new Label();
            layerLabel.Width = 95;
            layerLabel.Height = 15;
            layerLabel.Text = "Save Layer Name";
            layerLabel.Location = new Point(Inspector.Width / 2 - layerLabel.Width / 2, entityName.Location.Y + entityName.Height + 5);         

            Inspector.Controls.Add(layerLabel);

            TextBox layerName = new TextBox();
            layerName.Text = SelectedEntity.Layer;
            layerName.Width = 150;
            layerName.BackColor = Color.FromArgb(64, 64, 64);
            layerName.ForeColor = Color.FromArgb(240, 240, 240);
            layerName.BorderStyle = BorderStyle.FixedSingle;
            layerName.TextAlign = HorizontalAlignment.Center;
            layerName.GotFocus += layerName_GotFocus;
            layerName.LostFocus += layerName_LostFocus;
            layerName.Location = new Point(Inspector.Width / 2 - layerName.Width / 2, layerLabel.Location.Y + layerLabel.Height / 2 + 10);
            layerName.AcceptsTab = true;
            Inspector.Controls.Add(layerName);

            InspectorTab.Hide();

            Y = 0;

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

                List<PropertyInfo> pis = c.GetType().GetProperties().ToList();
                PropertyInfo nickName = pis.Where(p => p.Name == "NickName").First();

                pis.Remove(nickName);

                Label labelo = new Label();
                labelo.Text = nickName.Name;
                labelo.Font = new Font("Calibri", 10);
                labelo.Location = new Point(10, localY);
                labelo.AutoSize = true;
                tp.Controls.Add(labelo);
                localY += labelo.Height + 5;

                TextBox texb = new TextBox();
                texb.Location = new Point(10, localY);
                texb.Width = 250;
                PropertyControlList.Add(new PropertyComponentControl(nickName, c, texb));
                texb.GotFocus += tb_GotFocus;
                texb.LostFocus += tb_LostFocus;
                texb.Text = (string)nickName.GetValue(c, null);

                tp.Controls.Add(texb);

                localY += texb.Height + 5;

                foreach (PropertyInfo pi in pis)
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
                    else if (pi.Name.EndsWith("SpriteSheetTag"))
                    {
                        Button openGraphicPicker = new Button();
                        openGraphicPicker.FlatStyle = FlatStyle.Flat;
                        openGraphicPicker.Location = new Point(10, localY);
                        openGraphicPicker.AutoSize = true;
                        openGraphicPicker.Text = "Spritesheet Picker";

                        tp.Controls.Add(openGraphicPicker);

                        Label currentGraphic = new Label();
                        currentGraphic.Location = new Point(openGraphicPicker.Width + 35, localY);
                        currentGraphic.AutoSize = false;
                        currentGraphic.Width = 190;
                        currentGraphic.Height = 30;
                        currentGraphic.Text = (string)pi.GetValue(c, null);
                        currentGraphic.Font = new Font("Calibri", 8.0f);
                        tp.Controls.Add(currentGraphic);

                        openGraphicPicker.Click += delegate(object sender, EventArgs e)
                        {
                            GameWorld.ToggleSpritesheetPicker(pi, c, currentGraphic);
                        };

                        localY += openGraphicPicker.Height + 5;
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
                    else if (pi.Name.EndsWith("SoundTag"))
                    {
                        Button openSoundPicker = new Button();
                        openSoundPicker.FlatStyle = FlatStyle.Flat;
                        openSoundPicker.Location = new Point(10, localY);
                        openSoundPicker.AutoSize = true;
                        openSoundPicker.Text = "Sound Picker";

                        tp.Controls.Add(openSoundPicker);

                        Label currentSound = new Label();
                        currentSound.Location = new Point(openSoundPicker.Width + 35, localY);
                        currentSound.AutoSize = false;
                        currentSound.Width = 190;
                        currentSound.Height = 30;
                        currentSound.Text = (string)pi.GetValue(c, null);
                        currentSound.Font = new Font("Calibri", 8.0f);
                        tp.Controls.Add(currentSound);

                        openSoundPicker.Click += delegate(object sender, EventArgs e)
                        {
                            GameWorld.ToggleSoundPicker(pi, c, currentSound);
                        };

                        localY += openSoundPicker.Height + 5;
                    }
                    else if (pi.Name.EndsWith("GraphicTag"))
                    {
                        Button openGraphicPicker = new Button();
                        openGraphicPicker.FlatStyle = FlatStyle.Flat;
                        openGraphicPicker.Location = new Point(10, localY);
                        openGraphicPicker.AutoSize = true;
                        openGraphicPicker.Text = "Graphic Picker";
                        
                        tp.Controls.Add(openGraphicPicker);

                        Label currentGraphic = new Label();
                        currentGraphic.Location = new Point(openGraphicPicker.Width + 35, localY);
                        currentGraphic.AutoSize = false;
                        currentGraphic.Width = 190;
                        currentGraphic.Height = 30;
                        currentGraphic.Text = (string)pi.GetValue(c, null);
                        currentGraphic.Font = new Font("Calibri", 8.0f);
                        tp.Controls.Add(currentGraphic);

                        openGraphicPicker.Click += delegate(object sender, EventArgs e)
                        {
                            GameWorld.ToggleGraphicPicker(pi, c, currentGraphic);
                        };

                        localY += openGraphicPicker.Height + 5;
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
                        SpritesheetInspector ssinspector = new SpritesheetInspector((c as SpritesheetManager).Spritesheets, GameWorld);
                        ssinspector.Location = new Point(10, localY);
                        tp.Controls.Add(ssinspector);
                        ssinspector.RefreshData();
                        localY += ssinspector.Height + 5;
                    }
                    else if (pi.PropertyType.Equals(typeof(List<SoundInstanceInfo>)))
                    {
                        SoundListInspector sli = new SoundListInspector((List<SoundInstanceInfo>)pi.GetValue(c, null), GameWorld);
                        sli.Location = new Point(10, localY);
                        tp.Controls.Add(sli);
                        sli.RefreshData();
                        localY += sli.Height + 5;
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
                tp.Tag = c;

                this.InspectorTab.DrawItem += InspectorTab_DrawItem;

                this.InspectorTab.TabPages.Add(tp);
                
                tp.Height += 20;


            }

            if (InspectorTab.TabPages.Count % 2 == 1)
            {
                TabPage tp2 = new TabPage("");
                InspectorTab.TabPages.Add(tp2);
                tp2.BackColor = Color.FromArgb(255, 64, 64, 64);
            }

            InspectorTab.Show();
            Inspector.Controls.Add(InspectorTab);
        }

        void layerName_LostFocus(object sender, EventArgs e)
        {
            if (GameWorld.EntityChangedThisFrame)
            {
                GameWorld.FocusedTextBox = null;
                return;
            }

            if (GameWorld.SelectedEntity != null)
            {
                GameWorld.SelectedEntity.Layer = (sender as TextBox).Text;
                GameWorld.BottomDockControl.entityListControl.InitializeEntityList();
            }

            GameWorld.FocusedTextBox = null;
        }

        void layerName_GotFocus(object sender, EventArgs e)
        {
            GameWorld.FocusedTextBox = (sender as TextBox);
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
                if (exc != null)
                    return;
                else
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
            {
                GameWorld.FocusedTextBox = null;
                return;
            }

            if (GameWorld.SelectedEntity == null)
            {
                GameWorld.FocusedTextBox = null;
                return;
            }

            if ((sender as TextBox) != null && GameWorld.FocusedTextBox == (sender as TextBox) && (sender as TextBox).Text != GameWorld.AvatarName)
            {
                GameWorld.FocusedTextBox = null;
                if (GameWorld.BottomDockControl.entityListControl.EntityListBox.SelectedNode != null && GameWorld.BottomDockControl.entityListControl.EntityListBox.SelectedNode.Text == GameWorld.SelectedEntity.Name)
                    GameWorld.BottomDockControl.entityListControl.EntityListBox.SelectedNode.Text = (sender as TextBox).Text;
                GameWorld.SelectedEntity.Name = (sender as TextBox).Text;                           
            }
            else if ((sender as TextBox).Text == GameWorld.AvatarName)
            {
                GameWorld.FocusedTextBox = null;
                if (GameWorld.SelectedEntity != null)
                {
                    (sender as TextBox).Text = GameWorld.SelectedEntity.Name;
                    Console.WriteLine("Warning : Can't name an entity '" + GameWorld.AvatarName + "', this name is reserved");
                }
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
            //ActionManager.SaveAction(ActionType.DeleteComponent, new object[4] { RemoveButtons[(Button)sender].GetType(), DataManager.SaveComponentParameters(RemoveButtons[(Button)sender] as Component), GameWorld.SelectedEntity, this });
        
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
            GameWorld.ToggleAddComponentPanel();
        }

        private void CopyComponentButton_Click(object sender, EventArgs e)
        {
            if (GameWorld.SelectedEntity != null && InspectorTab.SelectedTab.Tag != null)
            {
                this.ClipboardComponent = DataManager.SaveComponentParameters(InspectorTab.SelectedTab.Tag as Component);
            }
        }

        private void PasteComponentButton_Click(object sender, EventArgs e)
        {
            if (GameWorld.SelectedEntity != null && ClipboardComponent != null)
            {
                DataManager.LoadComponent(ClipboardComponent, GameWorld.SelectedEntity);
                this.InstantiateProperties(GameWorld.SelectedEntity);
            }
        }

        public Component GetSelectedComponent()
        {
            return InspectorTab.SelectedTab.Tag as Component;
        }

    }
}
