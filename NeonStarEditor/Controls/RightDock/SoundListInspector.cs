using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NeonEngine;
using NeonEngine.Components.Audio;

namespace NeonStarEditor
{
    public partial class SoundListInspector : UserControl
    {
        public List<SoundInstanceInfo> SoundsList;
        public EditorScreen GameWorld;
        private string _lastSoundKey;

        public SoundListInspector(List<SoundInstanceInfo> soundsList, EditorScreen GameWorld)
        {
            SoundsList = soundsList;
            InitializeComponent();
            this.GameWorld = GameWorld;
            this.AutoSize = true;
            RefreshData();
        }

        public void RefreshData()
        {
            for (int i = this.Controls.Count - 1; i >= 0; i--)
                this.Controls.RemoveAt(i);

            float NextItemHeight = 10f;

            foreach (SoundInstanceInfo sii in SoundsList)
            {
                TextBox tb = new TextBox();

                tb.Text = sii.Name;
                tb.Location = new Point(10, (int)NextItemHeight);
                tb.Width = 60;
                tb.GotFocus += tb_GotFocus;
                tb.LostFocus += tb_LostFocus;
                this.Controls.Add(tb);


                Label currentSound = new Label();
                currentSound.Location = new Point(75, (int)NextItemHeight);
                currentSound.AutoSize = false;
                currentSound.Width = 190;
                currentSound.Height = 20;
                if (sii.Sound != null)
                    currentSound.Text = sii.Sound.Name;
                else
                    currentSound.Text = "";
                currentSound.Font = new Font("Calibri", 8.0f);
                Controls.Add(currentSound);

                Button openSoundPicker = new Button();
                openSoundPicker.FlatStyle = FlatStyle.Flat;
                openSoundPicker.Location = new Point(10, (int)NextItemHeight + tb.Height + 5);
                openSoundPicker.Width = 200;
                openSoundPicker.Height = 20;
                openSoundPicker.AutoSize = false;
                openSoundPicker.Text = "Sound Picker";

                this.Controls.Add(openSoundPicker);

                NextItemHeight += openSoundPicker.Height + currentSound.Height + 8;
                openSoundPicker.Click += delegate(object sender, EventArgs e)
                {
                    GameWorld.ToggleSoundPicker(this, tb, sii, currentSound);
                };
            }

            Button button = new Button();
            button.Text = "Add";
            button.Click += button_Click;
            button.Location = new Point(100, (int)NextItemHeight);
            this.Controls.Add(button);

            if (SoundsList.Count > 0)
            {
                Button button2 = new Button();
                button2.Text = "Remove";
                button2.Click += button2_Click;
                button2.Location = new Point(10, (int)NextItemHeight);
                this.Controls.Add(button2);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SoundsList.Remove(SoundsList.Last());

            RefreshData();
        }

        void button_Click(object sender, EventArgs e)
        {
            SoundInstanceInfo sii = new SoundInstanceInfo();
            sii.Name = "Sound" + SoundsList.Count;
            SoundsList.Add(sii);

            RefreshData();
        }

        void tb_LostFocus(object sender, EventArgs e)
        {
            SoundInstanceInfo[] changedSii = SoundsList.Where(siiName => siiName.Name == _lastSoundKey).ToArray();
            if(changedSii.Length <= 0)
                return;
   
            SoundInstanceInfo sii = changedSii.First();;
            sii.Name = (sender as TextBox).Text;
        }

        void tb_GotFocus(object sender, EventArgs e)
        {
            GameWorld.FocusedTextBox = (sender as TextBox);
            _lastSoundKey = (sender as TextBox).Text;
        }

    }
}
