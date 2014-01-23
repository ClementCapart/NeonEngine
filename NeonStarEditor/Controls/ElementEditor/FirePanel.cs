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
    public partial class FirePanel : UserControl
    {
        EditorScreen GameWorld;

        public FirePanel()
        {
            InitializeComponent();
            GameWorld = Neon.World as EditorScreen;
            InitializeData();
        }

        public void InitializeData()
        {
            this.GaugeSpeed.Value = (decimal)(float)(ElementManager.FireParameters[0][0]);
            this.MaxChargeDuration.Value = (decimal)(float)(ElementManager.FireParameters[0][1]);

            this.GaugeCostLevelOne.Value = (decimal)(float)(ElementManager.FireParameters[1][0]);
            this.StageOneAttackLevelOne.Text = (string)(ElementManager.FireParameters[1][1]);
            this.StageTwoAttackLevelOne.Text = (string)(ElementManager.FireParameters[1][2]);
            this.StageThreeAttackLevelOne.Text = (string)(ElementManager.FireParameters[1][3]);
            this.StageTwoThresholdLevelOne.Value = (decimal)(float)(ElementManager.FireParameters[1][4]);
            this.StageThreeThresholdLevelOne.Value = (decimal)(float)(ElementManager.FireParameters[1][5]);

            this.GaugeCostLevelTwo.Value = (decimal)(float)(ElementManager.FireParameters[2][0]);
            this.StageOneAttackLevelTwo.Text = (string)(ElementManager.FireParameters[2][1]);
            this.StageTwoAttackLevelTwo.Text = (string)(ElementManager.FireParameters[2][2]);
            this.StageThreeAttackLevelTwo.Text = (string)(ElementManager.FireParameters[2][3]);
            this.StageTwoThresholdLevelTwo.Value = (decimal)(float)(ElementManager.FireParameters[2][4]);
            this.StageThreeThresholdLevelTwo.Value = (decimal)(float)(ElementManager.FireParameters[2][5]);
            this.StageFourAttackLevelTwo.Text = (string)(ElementManager.FireParameters[2][6]);
            this.StageFourThresholdLevelTwo.Value = (decimal)(float)(ElementManager.FireParameters[2][7]);

            this.GaugeCostLevelThree.Value = (decimal)(float)(ElementManager.FireParameters[3][0]);
            this.StageOneAttackLevelThree.Text = (string)(ElementManager.FireParameters[3][1]);
            this.StageTwoAttackLevelThree.Text = (string)(ElementManager.FireParameters[3][2]);
            this.StageThreeAttackLevelThree.Text = (string)(ElementManager.FireParameters[3][3]);
            this.StageTwoThresholdLevelThree.Value = (decimal)(float)(ElementManager.FireParameters[3][4]);
            this.StageThreeThresholdLevelThree.Value = (decimal)(float)(ElementManager.FireParameters[3][5]);
            this.StageFourAttackLevelThree.Text = (string)(ElementManager.FireParameters[3][6]);
            this.StageFourThresholdLevelThree.Value = (decimal)(float)(ElementManager.FireParameters[3][7]);

            this.DamageModifier.Value = (decimal)(float)(ElementManager.FireParameters[4][0]);
            this.ModifierDurationLevelOne.Value = (decimal)(float)(ElementManager.FireParameters[4][1]);
            this.ModifierDurationLevelTwo.Value = (decimal)(float)(ElementManager.FireParameters[4][2]);
            this.ModifierDurationLevelThree.Value = (decimal)(float)(ElementManager.FireParameters[4][3]);
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
                case "GaugeSpeed":
                    ElementManager.FireParameters[0][0] = (float)(sender as NumericUpDown).Value;
                    break;

                case "MaxChargeDuration":
                    ElementManager.FireParameters[0][1] = (float)(sender as NumericUpDown).Value;
                    break;

                case "GaugeCostLevelOne":
                    ElementManager.FireParameters[1][0] = (float)(sender as NumericUpDown).Value;
                    break;

                case "GaugeCostLevelTwo":
                    ElementManager.FireParameters[2][0] = (float)(sender as NumericUpDown).Value;
                    break;

                case "GaugeCostLevelThree":
                    ElementManager.FireParameters[3][0] = (float)(sender as NumericUpDown).Value;
                    break;

                case "StageTwoThresholdLevelOne":
                    ElementManager.FireParameters[1][4] = (float)(sender as NumericUpDown).Value;
                    break;

                case "StageThreeThresholdLevelOne":
                    ElementManager.FireParameters[1][5] = (float)(sender as NumericUpDown).Value;
                    break;

                case "StageTwoThresholdLevelTwo":
                    ElementManager.FireParameters[2][4] = (float)(sender as NumericUpDown).Value;
                    break;

                case "StageThreeThresholdLevelTwo":
                    ElementManager.FireParameters[2][5] = (float)(sender as NumericUpDown).Value;
                    break;

                case "StageFourThresholdLevelTwo":
                    ElementManager.FireParameters[2][7] = (float)(sender as NumericUpDown).Value;
                    break;

                case "StageTwoThresholdLevelThree":
                    ElementManager.FireParameters[3][4] = (float)(sender as NumericUpDown).Value;
                    break;

                case "StageThreeThresholdLevelThree":
                    ElementManager.FireParameters[3][5] = (float)(sender as NumericUpDown).Value;
                    break;

                case "StageFourThresholdLevelThree":
                    ElementManager.FireParameters[3][7] = (float)(sender as NumericUpDown).Value;
                    break;

                case "DamageModifier":
                    ElementManager.FireParameters[4][0] = (float)(sender as NumericUpDown).Value;
                    break;

                case "ModifierDurationLevelOne":
                    ElementManager.FireParameters[4][1] = (float)(sender as NumericUpDown).Value;
                    break;

                case "ModifierDurationLevelTwo":
                    ElementManager.FireParameters[4][2] = (float)(sender as NumericUpDown).Value;
                    break;

                case "ModifierDurationLevelThree":
                    ElementManager.FireParameters[4][3] = (float)(sender as NumericUpDown).Value;
                    break;
            }
        }

        private void numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            switch ((sender as NumericUpDown).Name)
            {
                case "GaugeSpeed":
                    ElementManager.FireParameters[0][0] = (float)(sender as NumericUpDown).Value;
                    break;

                case "MaxChargeDuration":
                    ElementManager.FireParameters[0][1] = (float)(sender as NumericUpDown).Value;
                    break;

                case "GaugeCostLevelOne":
                    ElementManager.FireParameters[1][0] = (float)(sender as NumericUpDown).Value;
                    break;

                case "GaugeCostLevelTwo":
                    ElementManager.FireParameters[2][0] = (float)(sender as NumericUpDown).Value;
                    break;

                case "GaugeCostLevelThree":
                    ElementManager.FireParameters[3][0] = (float)(sender as NumericUpDown).Value;
                    break;

                case "StageTwoThresholdLevelOne":
                    ElementManager.FireParameters[1][4] = (float)(sender as NumericUpDown).Value;
                    break;

                case "StageThreeThresholdLevelOne":
                    ElementManager.FireParameters[1][5] = (float)(sender as NumericUpDown).Value;
                    break;

                case "StageTwoThresholdLevelTwo":
                    ElementManager.FireParameters[2][4] = (float)(sender as NumericUpDown).Value;
                    break;

                case "StageThreeThresholdLevelTwo":
                    ElementManager.FireParameters[2][5] = (float)(sender as NumericUpDown).Value;
                    break;

                case "StageFourThresholdLevelTwo":
                    ElementManager.FireParameters[2][7] = (float)(sender as NumericUpDown).Value;
                    break;

                case "StageTwoThresholdLevelThree":
                    ElementManager.FireParameters[3][4] = (float)(sender as NumericUpDown).Value;
                    break;

                case "StageThreeThresholdLevelThree":
                    ElementManager.FireParameters[3][5] = (float)(sender as NumericUpDown).Value;
                    break;

                case "StageFourThresholdLevelThree":
                    ElementManager.FireParameters[3][7] = (float)(sender as NumericUpDown).Value;
                    break;

                case "DamageModifier":
                    ElementManager.FireParameters[4][0] = (float)(sender as NumericUpDown).Value;
                    break;

                case "ModifierDurationLevelOne":
                    ElementManager.FireParameters[4][1] = (float)(sender as NumericUpDown).Value;
                    break;

                case "ModifierDurationLevelTwo":
                    ElementManager.FireParameters[4][2] = (float)(sender as NumericUpDown).Value;
                    break;

                case "ModifierDurationLevelThree":
                    ElementManager.FireParameters[4][3] = (float)(sender as NumericUpDown).Value;
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

            switch ((sender as TextBox).Name)
            {
                case "StageOneAttackLevelOne":
                    ElementManager.FireParameters[1][1] = (sender as TextBox).Text;
                    break;

                case "StageTwoAttackLevelOne":
                    ElementManager.FireParameters[1][2] = (sender as TextBox).Text;
                    break;

                case "StageThreeAttackLevelOne":
                    ElementManager.FireParameters[1][3] = (sender as TextBox).Text;
                    break;

                case "StageOneAttackLevelTwo":
                    ElementManager.FireParameters[2][1] = (sender as TextBox).Text;
                    break;

                case "StageTwoAttackLevelTwo":
                    ElementManager.FireParameters[2][2] = (sender as TextBox).Text;
                    break;

                case "StageThreeAttackLevelTwo":
                    ElementManager.FireParameters[2][3] = (sender as TextBox).Text;
                    break;

                case "StageFourAttackLevelTwo":
                    ElementManager.FireParameters[2][6] = (sender as TextBox).Text;
                    break;

                case "StageOneAttackLevelThree":
                    ElementManager.FireParameters[3][1] = (sender as TextBox).Text;
                    break;

                case "StageTwoAttackLevelThree":
                    ElementManager.FireParameters[3][2] = (sender as TextBox).Text;
                    break;

                case "StageThreeAttackLevelThree":
                    ElementManager.FireParameters[3][3] = (sender as TextBox).Text;
                    break;

                case "StageFourAttackLevelThree":
                    ElementManager.FireParameters[3][6] = (sender as TextBox).Text;
                    break;
            }
        }
    }
}
