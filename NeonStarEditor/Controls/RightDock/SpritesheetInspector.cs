using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NeonEngine;

namespace NeonStarEditor
{
    public partial class SpritesheetInspector : UserControl
    {
        public Dictionary<string, SpriteSheetInfo> spritesheetList;
        public EditorScreen GameWorld;
        private string _lastSpritesheetKey;
        private Dictionary<ComboBox, TextBox> lineRelationship = new Dictionary<ComboBox, TextBox>();

        public SpritesheetInspector(Dictionary<string, SpriteSheetInfo> spritesheetList, EditorScreen GameWorld)
        {
            this.spritesheetList = spritesheetList;
            InitializeComponent();
            this.GameWorld = GameWorld;
            RefreshData();
        }

        void RefreshData()
        {
            for (int i = this.Controls.Count - 1; i >= 0; i--)
                this.Controls.RemoveAt(i);

            float NextItemHeight = 10f;

            foreach (KeyValuePair<string, SpriteSheetInfo> kvp in spritesheetList)
            {   
                TextBox tb = new TextBox();
                
                tb.Text = kvp.Key;
                tb.Location = new Point(10, (int)NextItemHeight);
                tb.Width = 60;
                tb.GotFocus += tb_GotFocus;
                tb.LostFocus += tb_LostFocus;
                this.Controls.Add(tb);

                ComboBox cb = new ComboBox();
                cb.Name = tb.Text;
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
                cb.Location = new Point(75, (int)NextItemHeight);
                cb.BindingContext = new BindingContext();
                BindingSource bs = new BindingSource();
                List<string> spritesheets = new List<string>();
                spritesheets.AddRange(AssetManager.CommonSpritesheets.Keys.OrderBy(k => k.ToString()));
                spritesheets.AddRange(AssetManager.GroupSpritesheets.Keys.OrderBy(k => k.ToString()));
                spritesheets.AddRange(AssetManager.LevelSpritesheets.Keys.OrderBy(k => k.ToString()));

                bs.DataSource = spritesheets;
                cb.DataSource = bs;
                
                cb.SelectedItem = AssetManager.GetSpritesheetTag(kvp.Value);

                cb.Width = 120;
                this.Controls.Add(cb);

                this.lineRelationship.Add(cb, tb);
                cb.SelectedValueChanged += cb_SelectedValueChanged;

                NextItemHeight += 30;
            }

            Button button = new Button();
            button.Text = "Add";
            button.Click += button_Click;
            button.Location = new Point(100, (int)NextItemHeight);
            this.Controls.Add(button);

            if (spritesheetList.Count > 0)
            {
                Button button2 = new Button();
                button2.Text = "Remove last";
                button2.Click += button2_Click;
                button2.Location = new Point(10, (int)NextItemHeight);
                this.Controls.Add(button2);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            spritesheetList.Remove(spritesheetList.Last().Key);

            RefreshData();
        }

        void button_Click(object sender, EventArgs e)
        {
            spritesheetList.Add("Anim" + spritesheetList.Count, new SpriteSheetInfo());

            RefreshData();
        }

        void cb_SelectedValueChanged(object sender, EventArgs e)
        {
            string CurrentKey = lineRelationship[(sender as ComboBox)].Text;
            if (spritesheetList.ContainsKey(CurrentKey))
                spritesheetList[CurrentKey] = AssetManager.GetSpriteSheet((sender as ComboBox).SelectedValue.ToString());
        }

        void tb_LostFocus(object sender, EventArgs e)
        {
            if (spritesheetList.ContainsKey(_lastSpritesheetKey))
            {
                SpriteSheetInfo ssi = spritesheetList[_lastSpritesheetKey];
                spritesheetList.Remove(_lastSpritesheetKey);
                spritesheetList.Add((sender as TextBox).Text, ssi);
            }
            
        }

        void tb_GotFocus(object sender, EventArgs e)
        {
           GameWorld.FocusedTextBox = (sender as TextBox);
            _lastSpritesheetKey = (sender as TextBox).Text;
        }

    }
}
