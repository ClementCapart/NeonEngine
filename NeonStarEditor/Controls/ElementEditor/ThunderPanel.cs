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
    public partial class ThunderPanel : UserControl
    {
        EditorScreen GameWorld;

        public ThunderPanel()
        {
            InitializeComponent();
            GameWorld = Neon.World as EditorScreen;
            InitializeData();
        }

        public void InitializeData()
        {
            this.GaugeCostLevelOne.Value = (decimal)(float)(ElementManager.ThunderParameters[0][0]);
            this.DashHorizontalImpulseLevelOne.Value = (decimal)(float)ElementManager.ThunderParameters[0][1];
            this.DashDurationLevelOne.Value = (decimal)(float)ElementManager.ThunderParameters[0][2];
            this.AttackToLaunchLevelOne.Text = (string)ElementManager.ThunderParameters[0][3];

            this.GaugeCostLevelTwo.Value = (decimal)(float)ElementManager.ThunderParameters[1][0];
            this.DashHorizontalImpulseLevelTwo.Value = (decimal)(float)ElementManager.ThunderParameters[1][1];
            this.DashDurationLevelTwo.Value = (decimal)(float)ElementManager.ThunderParameters[1][2];
            this.AttackToLaunchLevelTwo.Text = (string)ElementManager.ThunderParameters[1][3];
            this.DashVerticalUpImpulseLevelTwo.Value = (decimal)(float)ElementManager.ThunderParameters[1][4];
            this.DashVerticalDownImpulseLevelTwo.Value = (decimal)(float)ElementManager.ThunderParameters[1][5];

            this.GaugeCostLevelThree.Value = (decimal)(float)ElementManager.ThunderParameters[2][0];
            this.DashHorizontalImpulseLevelThree.Value = (decimal)(float)ElementManager.ThunderParameters[2][1];
            this.DashDurationLevelThree.Value = (decimal)(float)ElementManager.ThunderParameters[2][2];
            this.AttackToLaunchLevelThree.Text = (string)ElementManager.ThunderParameters[2][3];
            this.DashVerticalUpImpulseLevelThree.Value = (decimal)(float)ElementManager.ThunderParameters[2][4];
            this.DashVerticalDownImpulseLevelThree.Value = (decimal)(float)ElementManager.ThunderParameters[2][5];

            this.ModifierDurationLevelOne.Value = (decimal)(float)ElementManager.ThunderParameters[3][0];
            this.MovementSpeedModifierLevelOne.Value = (decimal)(float)ElementManager.ThunderParameters[3][1];
            this.AttackSpeedModifierLevelOne.Value = (decimal)(float)ElementManager.ThunderParameters[3][2];

            this.ModifierDurationLevelTwo.Value = (decimal)(float)ElementManager.ThunderParameters[4][0];
            this.MovementSpeedModifierLevelTwo.Value = (decimal)(float)ElementManager.ThunderParameters[4][1];
            this.AttackSpeedModifierLevelTwo.Value = (decimal)(float)ElementManager.ThunderParameters[4][2];

            this.ModifierDurationLevelThree.Value = (decimal)(float)ElementManager.ThunderParameters[5][0];
            this.MovementSpeedModifierLevelThree.Value = (decimal)(float)ElementManager.ThunderParameters[5][1];
            this.AttackSpeedModifierLevelThree.Value = (decimal)(float)ElementManager.ThunderParameters[5][2];
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
                case "GaugeCostLevelOne":
                    ElementManager.ThunderParameters[0][0] = (float)(sender as NumericUpDown).Value;
                    break;

                case "DashHorizontalImpulseLevelOne":
                    ElementManager.ThunderParameters[0][1] = (float)(sender as NumericUpDown).Value;
                    break;

                case "DashDurationLevelOne":
                    ElementManager.ThunderParameters[0][2] = (float)(sender as NumericUpDown).Value;
                    break;

                case "GaugeCostLevelTwo":
                    ElementManager.ThunderParameters[1][0] = (float)(sender as NumericUpDown).Value;
                    break;

                case "DashHorizontalImpulseLevelTwo":
                    ElementManager.ThunderParameters[1][1] = (float)(sender as NumericUpDown).Value;
                    break;

                case "DashDurationLevelTwo":
                    ElementManager.ThunderParameters[1][2] = (float)(sender as NumericUpDown).Value;
                    break;

                case "DashVerticalUpImpulseLevelTwo":
                    ElementManager.ThunderParameters[1][4] = (float)(sender as NumericUpDown).Value;
                    break;

                case "DashVerticalDownImpulseLevelTwo":
                    ElementManager.ThunderParameters[1][5] = (float)(sender as NumericUpDown).Value;
                    break;

                case "GaugeCostLevelThree":
                    ElementManager.ThunderParameters[2][0] = (float)(sender as NumericUpDown).Value;
                    break;

                case "DashHorizontalImpulseLevelThree":
                    ElementManager.ThunderParameters[2][1] = (float)(sender as NumericUpDown).Value;
                    break;

                case "DashDurationLevelThree":
                    ElementManager.ThunderParameters[2][2] = (float)(sender as NumericUpDown).Value;
                    break;

                case "DashVerticalUpImpulseLevelThree":
                    ElementManager.ThunderParameters[2][4] = (float)(sender as NumericUpDown).Value;
                    break;

                case "DashVerticalDownImpulseLevelThree":
                    ElementManager.ThunderParameters[2][5] = (float)(sender as NumericUpDown).Value;
                    break;

                case "ModifierDurationLevelOne":
                    ElementManager.ThunderParameters[3][0] = (float)(sender as NumericUpDown).Value;
                    break;

                case "MovementSpeedModifierLevelOne":
                    ElementManager.ThunderParameters[3][1] = (float)(sender as NumericUpDown).Value;
                    break;

                case "AttackSpeedModifierLevelOne":
                    ElementManager.ThunderParameters[3][2] = (float)(sender as NumericUpDown).Value;
                    break;

                case "ModifierDurationLevelTwo":
                    ElementManager.ThunderParameters[4][0] = (float)(sender as NumericUpDown).Value;
                    break;

                case "MovementSpeedModifierLevelTwo":
                    ElementManager.ThunderParameters[4][1] = (float)(sender as NumericUpDown).Value;
                    break;

                case "AttackSpeedModifierLevelTwo":
                    ElementManager.ThunderParameters[4][2] = (float)(sender as NumericUpDown).Value;
                    break;

                case "ModifierDurationLevelThree":
                    ElementManager.ThunderParameters[5][0] = (float)(sender as NumericUpDown).Value;
                    break;

                case "MovementSpeedModifierLevelThree":
                    ElementManager.ThunderParameters[5][1] = (float)(sender as NumericUpDown).Value;
                    break;

                case "AttackSpeedModifierLevelThree":
                    ElementManager.ThunderParameters[5][2] = (float)(sender as NumericUpDown).Value;
                    break;
            }
        }

        private void numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            switch ((sender as NumericUpDown).Name)
            {
                case "GaugeCostLevelOne":
                    ElementManager.ThunderParameters[0][0] = (float)(sender as NumericUpDown).Value;
                    break;

                case "DashHorizontalImpulseLevelOne":
                    ElementManager.ThunderParameters[0][1] = (float)(sender as NumericUpDown).Value;
                    break;

                case "DashDurationLevelOne":
                    ElementManager.ThunderParameters[0][2] = (float)(sender as NumericUpDown).Value;
                    break;

                case "GaugeCostLevelTwo":
                    ElementManager.ThunderParameters[1][0] = (float)(sender as NumericUpDown).Value;
                    break;

                case "DashHorizontalImpulseLevelTwo":
                    ElementManager.ThunderParameters[1][1] = (float)(sender as NumericUpDown).Value;
                    break;

                case "DashDurationLevelTwo":
                    ElementManager.ThunderParameters[1][2] = (float)(sender as NumericUpDown).Value;
                    break;

                case "DashVerticalUpImpulseLevelTwo":
                    ElementManager.ThunderParameters[1][4] = (float)(sender as NumericUpDown).Value;
                    break;

                case "DashVerticalDownImpulseLevelTwo":
                    ElementManager.ThunderParameters[1][5] = (float)(sender as NumericUpDown).Value;
                    break;

                case "GaugeCostLevelThree":
                    ElementManager.ThunderParameters[2][0] = (float)(sender as NumericUpDown).Value;
                    break;

                case "DashHorizontalImpulseLevelThree":
                    ElementManager.ThunderParameters[2][1] = (float)(sender as NumericUpDown).Value;
                    break;

                case "DashDurationLevelThree":
                    ElementManager.ThunderParameters[2][2] = (float)(sender as NumericUpDown).Value;
                    break;

                case "DashVerticalUpImpulseLevelThree":
                    ElementManager.ThunderParameters[2][4] = (float)(sender as NumericUpDown).Value;
                    break;

                case "DashVerticalDownImpulseLevelThree":
                    ElementManager.ThunderParameters[2][5] = (float)(sender as NumericUpDown).Value;
                    break;

                case "ModifierDurationLevelOne":
                    ElementManager.ThunderParameters[3][0] = (float)(sender as NumericUpDown).Value;
                    break;

                case "MovementSpeedModifierLevelOne":
                    ElementManager.ThunderParameters[3][1] = (float)(sender as NumericUpDown).Value;
                    break;

                case "AttackSpeedModifierLevelOne":
                    ElementManager.ThunderParameters[3][2] = (float)(sender as NumericUpDown).Value;
                    break;

                case "ModifierDurationLevelTwo":
                    ElementManager.ThunderParameters[4][0] = (float)(sender as NumericUpDown).Value;
                    break;

                case "MovementSpeedModifierLevelTwo":
                    ElementManager.ThunderParameters[4][1] = (float)(sender as NumericUpDown).Value;
                    break;

                case "AttackSpeedModifierLevelTwo":
                    ElementManager.ThunderParameters[4][2] = (float)(sender as NumericUpDown).Value;
                    break;

                case "ModifierDurationLevelThree":
                    ElementManager.ThunderParameters[5][0] = (float)(sender as NumericUpDown).Value;
                    break;

                case "MovementSpeedModifierLevelThree":
                    ElementManager.ThunderParameters[5][1] = (float)(sender as NumericUpDown).Value;
                    break;

                case "AttackSpeedModifierLevelThree":
                    ElementManager.ThunderParameters[5][2] = (float)(sender as NumericUpDown).Value;
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
                case "AttackToLaunchLevelOne":
                    ElementManager.ThunderParameters[0][3] = (sender as TextBox).Text;
                    break;

                case "AttackToLaunchLevelTwo":
                    ElementManager.ThunderParameters[1][3] = (sender as TextBox).Text;
                    break;

                case "AttackToLaunchLevelThree":
                    ElementManager.ThunderParameters[2][3] = (sender as TextBox).Text;
                    break;
            }
        }
    }
}
