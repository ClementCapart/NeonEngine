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
using NeonEngine.Components.Private;
using Microsoft.Xna.Framework;

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
                tb.Location = new System.Drawing.Point(10, (int)NextItemHeight);
                tb.Width = 60;
                tb.GotFocus += tb_GotFocus;
                tb.LostFocus += tb_LostFocus;
                this.Controls.Add(tb);

                Label currentSound = new Label();
                currentSound.Location = new System.Drawing.Point(75, (int)NextItemHeight);
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
                openSoundPicker.Location = new System.Drawing.Point(10, (int)NextItemHeight + tb.Height + 5);
                openSoundPicker.Width = 200;
                openSoundPicker.Height = 20;
                openSoundPicker.AutoSize = false;
                openSoundPicker.Text = "Sound Picker";

                this.Controls.Add(openSoundPicker);

                NextItemHeight += openSoundPicker.Height + currentSound.Height + 4;
                openSoundPicker.Click += delegate(object sender, EventArgs e)
                {
                    GameWorld.ToggleSoundPicker(this, tb, sii, currentSound);
                };

                CheckBox is3DSound = new CheckBox();
                is3DSound.Checked = sii.Is3DSound;
                is3DSound.Tag = sii;
                is3DSound.Text = "Is3DSound";
                is3DSound.Font = new Font("Calibri", 8.0f);
                is3DSound.Location = new System.Drawing.Point(10, (int)NextItemHeight);
                is3DSound.CheckedChanged += is3DSound_CheckedChanged;
                this.Controls.Add(is3DSound);
                NextItemHeight += is3DSound.Height + 8;

                Label volumeLabel = new Label();
                volumeLabel.Text = "Volume";
                volumeLabel.Width = 40;
                volumeLabel.Height = 10;
                volumeLabel.Font = new Font("Calibri", 8.0f);
                volumeLabel.Location = new System.Drawing.Point(10, (int)NextItemHeight);
                this.Controls.Add(volumeLabel);

                NumericUpDown volume = new NumericUpDown();
                volume.Name = "Volume";
                volume.Value = (decimal)sii.Volume;
                volume.Tag = sii;
                volume.DecimalPlaces = 2;
                volume.Increment = 0.1m;
                volume.Width = 50;
                volume.Enter += NU_Enter;
                volume.ValueChanged += NU_ValueChanged;
                volume.Leave += NU_Leave;
                volume.Location = new System.Drawing.Point(55, (int)NextItemHeight - 5);
                this.Controls.Add(volume);

                Label pitchLabel = new Label();
                pitchLabel.Text = "Pitch";
                pitchLabel.Width = 33;
                pitchLabel.Height = 10;
                pitchLabel.Font = new Font("Calibri", 8.0f);
                pitchLabel.Location = new System.Drawing.Point(10 + volume.Location.X + volume.Width, (int)NextItemHeight);
                this.Controls.Add(pitchLabel);

                NumericUpDown pitch = new NumericUpDown();
                pitch.Name = "Pitch";
                pitch.Value = (decimal)sii.Pitch;
                pitch.Tag = sii;
                pitch.Increment = 0.1m;
                pitch.DecimalPlaces = 2;
                pitch.Width = 50;
                pitch.Enter += NU_Enter;
                pitch.ValueChanged += NU_ValueChanged;
                pitch.Leave += NU_Leave;
                pitch.Location = new System.Drawing.Point(45 + volume.Location.X + volume.Width, (int)NextItemHeight - 5);
                this.Controls.Add(pitch);

                NextItemHeight += volume.Height;

                Label offsetLabel = new Label();
                offsetLabel.Text = "Offset";
                offsetLabel.Width = 40;
                offsetLabel.Height = 10;
                offsetLabel.Font = new Font("Calibri", 8.0f);
                offsetLabel.Location = new System.Drawing.Point(10, (int)NextItemHeight);
                this.Controls.Add(offsetLabel);

                NumericUpDown offsetX = new NumericUpDown();
                offsetX.Name = "OffsetX";
                offsetX.Value = (decimal)sii.Offset.X;
                offsetX.Tag = sii;
                offsetX.Increment = 1m;
                offsetX.DecimalPlaces = 2;
                offsetX.Width = 70;
                offsetX.Location = new System.Drawing.Point(10, (int)NextItemHeight + offsetLabel.Height + 5);
                offsetX.Enter += NU_Enter;
                offsetX.ValueChanged += NU_ValueChanged;
                offsetX.Leave += NU_Leave;
                offsetX.Minimum = -1000m;
                offsetX.Maximum = 1000m;
                this.Controls.Add(offsetX);

                NumericUpDown offsetY = new NumericUpDown();
                offsetY.Name = "OffsetY";
                offsetY.Value = (decimal)sii.Offset.Y;
                offsetY.Tag = sii;
                offsetY.Increment = 1m;
                offsetY.DecimalPlaces = 2;
                offsetY.Width = 70;
                offsetY.Enter += NU_Enter;
                offsetY.ValueChanged += NU_ValueChanged;
                offsetY.Leave += NU_Leave;
                offsetY.Minimum = -1000m;
                offsetY.Maximum = 1000m;
                offsetY.Location = new System.Drawing.Point(offsetX.Location.X + offsetX.Width + 5, (int)NextItemHeight + offsetLabel.Height + 5);
                this.Controls.Add(offsetY);

                NextItemHeight += offsetLabel.Height + offsetX.Height + 20;
            }

            Button button = new Button();
            button.Text = "Add";
            button.Click += button_Click;
            button.Location = new System.Drawing.Point(100, (int)NextItemHeight);
            this.Controls.Add(button);

            if (SoundsList.Count > 0)
            {
                Button button2 = new Button();
                button2.Text = "Remove";
                button2.Click += button2_Click;
                button2.Location = new System.Drawing.Point(10, (int)NextItemHeight);
                this.Controls.Add(button2);
            }
        }

        private void NU_Leave(object sender, EventArgs e)
        {
            GameWorld.FocusedNumericUpDown = null;
        }

        private void NU_ValueChanged(object sender, EventArgs e)
        {
            switch((sender as NumericUpDown).Name)
            {
                case "Volume":
                    ((sender as NumericUpDown).Tag as SoundInstanceInfo).Volume = MathHelper.Clamp((float)(sender as NumericUpDown).Value, 0, 1);
                    break;

                case "Pitch":
                    ((sender as NumericUpDown).Tag as SoundInstanceInfo).Pitch = MathHelper.Clamp((float)(sender as NumericUpDown).Value, 0, 1);
                    break;

                case "OffsetX":
                    ((sender as NumericUpDown).Tag as SoundInstanceInfo).Offset.X = (float)(sender as NumericUpDown).Value;
                    break;

                case "OffsetY":
                    ((sender as NumericUpDown).Tag as SoundInstanceInfo).Offset.Y = (float)(sender as NumericUpDown).Value;
                    break;
            }
        }

        private void NU_Enter(object sender, EventArgs e)
        {
            GameWorld.FocusedNumericUpDown = sender as NumericUpDown;
        }

        private void is3DSound_CheckedChanged(object sender, EventArgs e)
        {
            ((sender as CheckBox).Tag as SoundInstanceInfo).Is3DSound = (sender as CheckBox).Checked;
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
