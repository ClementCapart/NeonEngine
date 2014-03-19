using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NeonEngine;
using NeonStarLibrary;

namespace NeonStarEditor
{
    public partial class WindPanel : UserControl
    {
        EditorScreen GameWorld;

        public WindPanel()
        {
            InitializeComponent();
            GameWorld = Neon.World as EditorScreen;
            InitializeData();
        }

        public void InitializeData()
        {
            this.GaugeCost.Value = (decimal)((float)(ElementManager.WindParameters[0][0]));
            this.AirVerticalImpulse.Value = (decimal)(float)ElementManager.WindParameters[0][1];
            this.ImpulseDuration.Value = (decimal)(float)ElementManager.WindParameters[0][2];
            this.AttackToLaunch.Text = (string)ElementManager.WindParameters[0][3];
            this.TimedGaugeComsuption.Value = (decimal)(float)ElementManager.WindParameters[0][4];
            this.AirControlSpeed.Value = (decimal)(float)ElementManager.WindParameters[0][5];
            this.AirVerticalVelocity.Value = (decimal)(float)ElementManager.WindParameters[0][6];
            this.AirVerticalMaxVelocity.Value = (decimal)(float)ElementManager.WindParameters[0][7];
        }

        private void numericUpDown_Enter(object sender, EventArgs e)
        {
            GameWorld.FocusedNumericUpDown = sender as NumericUpDown;
        }

        private void numericUpDown_Leave(object sender, EventArgs e)
        {
            GameWorld.FocusedNumericUpDown = null;

            switch ((sender as NumericUpDown).Name)
            {
                case "GaugeCost":
                    ElementManager.WindParameters[0][0] = (float)(sender as NumericUpDown).Value;
                    break;

                case "AirVerticalImpulse":
                    ElementManager.WindParameters[0][1] = (float)(sender as NumericUpDown).Value;
                    break;

                case "ImpulseDuration":
                    ElementManager.WindParameters[0][2] = (float)(sender as NumericUpDown).Value;
                    break;

                case "TimedGaugeComsuption":
                    ElementManager.WindParameters[0][4] = (float)(sender as NumericUpDown).Value;
                    break;

                case "AirControlSpeed":
                    ElementManager.WindParameters[0][5] = (float)(sender as NumericUpDown).Value;
                    break;

                case "AirVerticalVelocity":
                    ElementManager.WindParameters[0][6] = (float)(sender as NumericUpDown).Value;
                    break;

                case "AirVerticalMaxVelocity":
                    ElementManager.WindParameters[0][7] = (float)(sender as NumericUpDown).Value;
                    break;
            }
        }

        private void numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            switch ((sender as NumericUpDown).Name)
            {
                case "GaugeCost":
                    ElementManager.WindParameters[0][0] = (float)(sender as NumericUpDown).Value;
                    break;

                case "AirVerticalImpulse":
                    ElementManager.WindParameters[0][1] = (float)(sender as NumericUpDown).Value;
                    break;

                case "ImpulseDuration":
                    ElementManager.WindParameters[0][2] = (float)(sender as NumericUpDown).Value;
                    break;

                case "TimedGaugeComsuption":
                    ElementManager.WindParameters[0][4] = (float)(sender as NumericUpDown).Value;
                    break;

                case "AirControlSpeed":
                    ElementManager.WindParameters[0][5] = (float)(sender as NumericUpDown).Value;
                    break;

                case "AirVerticalVelocity":
                    ElementManager.WindParameters[0][6] = (float)(sender as NumericUpDown).Value;
                    break;

                case "AirVerticalMaxVelocity":
                    ElementManager.WindParameters[0][7] = (float)(sender as NumericUpDown).Value;
                    break;
            }
        }

        private void textBox_Enter(object sender, EventArgs e)
        {
            GameWorld.FocusedTextBox = sender as TextBox;
        }

        private void textBox_Leave(object sender, EventArgs e)
        {
            GameWorld.FocusedTextBox = null;

            switch((sender as TextBox).Name)
            {
                case "AttackToLaunch":
                    ElementManager.WindParameters[0][3] = (sender as TextBox).Text;
                    break;
            }
        }
    }
}
