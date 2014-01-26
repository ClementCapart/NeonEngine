using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NeonStarLibrary;
using NeonEngine;
using System.IO;
using System.Xml.Linq;
using System.Globalization;
using NeonStarLibrary.Components.Avatar;

namespace NeonStarEditor
{
    public partial class ElementPanel : UserControl
    {
        public ElementPanel()
        {
            InitializeComponent();
            this.Location = new Point((int)Neon.HalfScreen.X - this.Width / 2, (int)Neon.HalfScreen.Y - this.Height / 2);
            InitializeData();
        }

        public void InitializeData()
        {
            elementCombobox.DataSource = Enum.GetValues(typeof(Element));

        }

        private void elementCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (Control c in settingPanel.Controls)
                c.Dispose();

            switch((Element)((sender as ComboBox).SelectedValue))
            {
                case Element.Neutral:
                    Console.WriteLine("Neutral");
                    break;

                case Element.Fire:
                    settingPanel.Controls.Add(new FirePanel());
                    break;

                case Element.Thunder:
                    settingPanel.Controls.Add(new ThunderPanel());
                    break;
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            XDocument document = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));

            XElement elements = new XElement("Elements");

            SaveFireParameters(elements);
            SaveThunderParameters(elements);        

            document.Add(elements);
            document.Save(@"../Data/Config/Elements.xml");
        }

        private void SaveFireParameters(XElement elements)
        {
            XElement fireParameters = new XElement("Fire");
            
            XElement general = new XElement("General");
            XElement gaugeSpeed = new XElement("GaugeSpeed", ((float)(ElementManager.FireParameters[0][0])).ToString());
            XElement maxChargeDuration = new XElement("MaxChargeDuration", ((float)(ElementManager.FireParameters[0][1])).ToString());
           
            general.Add(gaugeSpeed);
            general.Add(maxChargeDuration);

            fireParameters.Add(general);

            XElement firstLevel = new XElement("FirstLevel");
            XElement gaugeCost = new XElement("GaugeCost", ((float)(ElementManager.FireParameters[1][0])).ToString());
            XElement stageOneAttack = new XElement("StageOneAttack", (string)(ElementManager.FireParameters[1][1]));
            XElement stageTwoAttack = new XElement("StageTwoAttack", (string)(ElementManager.FireParameters[1][2]));
            XElement stageThreeAttack = new XElement("StageThreeAttack", (string)(ElementManager.FireParameters[1][3]));
            XElement stageTwoTreshold = new XElement("StageTwoThreshold", ((float)(ElementManager.FireParameters[1][4])).ToString());
            XElement stageThreeTreshold = new XElement("StageThreeThreshold", ((float)(ElementManager.FireParameters[1][5])).ToString());

            firstLevel.Add(gaugeCost);
            firstLevel.Add(stageOneAttack);
            firstLevel.Add(stageTwoAttack);
            firstLevel.Add(stageThreeAttack);
            firstLevel.Add(stageTwoTreshold);
            firstLevel.Add(stageThreeTreshold);

            fireParameters.Add(firstLevel);

            XElement secondLevel = new XElement("SecondLevel");
            gaugeCost = new XElement("GaugeCost", ((float)(ElementManager.FireParameters[2][0])).ToString());
            stageOneAttack = new XElement("StageOneAttack", (string)(ElementManager.FireParameters[2][1]));
            stageTwoAttack = new XElement("StageTwoAttack", (string)(ElementManager.FireParameters[2][2]));
            stageThreeAttack = new XElement("StageThreeAttack", (string)(ElementManager.FireParameters[2][3]));
            stageTwoTreshold = new XElement("StageTwoThreshold", ((float)(ElementManager.FireParameters[2][4])).ToString());
            stageThreeTreshold = new XElement("StageThreeThreshold", ((float)(ElementManager.FireParameters[2][5])).ToString());
            XElement stageFourAttack = new XElement("StageFourAttack", (string)(ElementManager.FireParameters[2][6]));
            XElement stageFourThreshold = new XElement("StageFourThreshold", ((float)(ElementManager.FireParameters[2][7])).ToString());

            secondLevel.Add(gaugeCost);
            secondLevel.Add(stageOneAttack);
            secondLevel.Add(stageTwoAttack);
            secondLevel.Add(stageThreeAttack);
            secondLevel.Add(stageTwoTreshold);
            secondLevel.Add(stageThreeTreshold);
            secondLevel.Add(stageFourAttack);
            secondLevel.Add(stageFourThreshold);

            fireParameters.Add(secondLevel);

            XElement thirdLevel = new XElement("ThirdLevel");
            gaugeCost = new XElement("GaugeCost", ((float)(ElementManager.FireParameters[3][0])).ToString());
            stageOneAttack = new XElement("StageOneAttack", (string)(ElementManager.FireParameters[3][1]));
            stageTwoAttack = new XElement("StageTwoAttack", (string)(ElementManager.FireParameters[3][2]));
            stageThreeAttack = new XElement("StageThreeAttack", (string)(ElementManager.FireParameters[3][3]));
            stageTwoTreshold = new XElement("StageTwoThreshold", ((float)(ElementManager.FireParameters[3][4])).ToString());
            stageThreeTreshold = new XElement("StageThreeThreshold", ((float)(ElementManager.FireParameters[3][5])).ToString());
            stageFourAttack = new XElement("StageFourAttack", (string)(ElementManager.FireParameters[3][6]));
            stageFourThreshold = new XElement("StageFourThreshold", ((float)(ElementManager.FireParameters[3][7])).ToString());

            thirdLevel.Add(gaugeCost);
            thirdLevel.Add(stageOneAttack);
            thirdLevel.Add(stageTwoAttack);
            thirdLevel.Add(stageThreeAttack);
            thirdLevel.Add(stageTwoTreshold);
            thirdLevel.Add(stageThreeTreshold);
            thirdLevel.Add(stageFourAttack);
            thirdLevel.Add(stageFourThreshold);

            fireParameters.Add(thirdLevel);

            XElement assimilation = new XElement("Assimilation");
            XElement modifierDurationLevelOne = new XElement("ModifierDurationLevelOne", ((float)(ElementManager.FireParameters[4][1])).ToString());
            XElement modifierDurationLevelTwo = new XElement("ModifierDurationLevelTwo", ((float)(ElementManager.FireParameters[4][2])).ToString());
            XElement modifierDurationLevelThree = new XElement("ModifierDurationLevelThree", ((float)(ElementManager.FireParameters[4][3])).ToString());
            XElement damageModifier = new XElement("DamageModifier", ((float)(ElementManager.FireParameters[4][0])).ToString());

            assimilation.Add(modifierDurationLevelOne);
            assimilation.Add(modifierDurationLevelTwo);
            assimilation.Add(modifierDurationLevelThree);
            assimilation.Add(damageModifier);

            fireParameters.Add(assimilation);

            elements.Add(fireParameters);           
        }

        private void SaveThunderParameters(XElement elements)
        {
            XElement thunderParameters = new XElement("Thunder");

            XElement firstLevel = new XElement("FirstLevel");
            XElement gaugeCost = new XElement("GaugeCost", ((float)(ElementManager.ThunderParameters[0][0])).ToString("G", CultureInfo.InvariantCulture));
            XElement dashHorizontalImpulse = new XElement("DashHorizontalImpulse", ((float)(ElementManager.ThunderParameters[0][1])).ToString("G", CultureInfo.InvariantCulture));
            XElement dashDuration = new XElement("DashDuration", ((float)(ElementManager.ThunderParameters[0][2])).ToString("G", CultureInfo.InvariantCulture));
            XElement attackToLaunch = new XElement("AttackToLaunch", (string)ElementManager.ThunderParameters[0][3]);

            firstLevel.Add(gaugeCost);
            firstLevel.Add(dashHorizontalImpulse);
            firstLevel.Add(dashDuration);
            firstLevel.Add(attackToLaunch);

            thunderParameters.Add(firstLevel);

            XElement secondLevel = new XElement("SecondLevel");
            gaugeCost = new XElement("GaugeCost", ((float)(ElementManager.ThunderParameters[1][0])).ToString("G", CultureInfo.InvariantCulture));
            dashHorizontalImpulse = new XElement("DashHorizontalImpulse", ((float)(ElementManager.ThunderParameters[1][1])).ToString("G", CultureInfo.InvariantCulture));
            dashDuration = new XElement("DashDuration", ((float)(ElementManager.ThunderParameters[1][2])).ToString("G", CultureInfo.InvariantCulture));
            attackToLaunch = new XElement("AttackToLaunch", (string)ElementManager.ThunderParameters[1][3]);
            XElement dashVerticalUpImpulse = new XElement("DashVerticalUpImpulse", ((float)(ElementManager.ThunderParameters[1][4])).ToString("G", CultureInfo.InvariantCulture));
            XElement dashVerticalDownImpulse = new XElement("DashVerticalDownImpulse", ((float)(ElementManager.ThunderParameters[1][5])).ToString("G", CultureInfo.InvariantCulture));

            secondLevel.Add(gaugeCost);
            secondLevel.Add(dashHorizontalImpulse);
            secondLevel.Add(dashDuration);
            secondLevel.Add(attackToLaunch);
            secondLevel.Add(dashVerticalUpImpulse);
            secondLevel.Add(dashVerticalDownImpulse);

            thunderParameters.Add(secondLevel);

            XElement thirdLevel = new XElement("ThirdLevel");
            gaugeCost = new XElement("GaugeCost", ((float)(ElementManager.ThunderParameters[2][0])).ToString("G", CultureInfo.InvariantCulture));
            dashHorizontalImpulse = new XElement("DashHorizontalImpulse", ((float)(ElementManager.ThunderParameters[2][1])).ToString("G", CultureInfo.InvariantCulture));
            dashDuration = new XElement("DashDuration", ((float)(ElementManager.ThunderParameters[2][2])).ToString("G", CultureInfo.InvariantCulture));
            attackToLaunch = new XElement("AttackToLaunch", (string)ElementManager.ThunderParameters[2][3]);
            dashVerticalUpImpulse = new XElement("DashVerticalUpImpulse", ((float)(ElementManager.ThunderParameters[2][4])).ToString("G", CultureInfo.InvariantCulture));
            dashVerticalDownImpulse = new XElement("DashVerticalDownImpulse", ((float)(ElementManager.ThunderParameters[2][5])).ToString("G", CultureInfo.InvariantCulture));

            thirdLevel.Add(gaugeCost);
            thirdLevel.Add(dashHorizontalImpulse);
            thirdLevel.Add(dashDuration);
            thirdLevel.Add(attackToLaunch);
            thirdLevel.Add(dashVerticalUpImpulse);
            thirdLevel.Add(dashVerticalDownImpulse);

            thunderParameters.Add(thirdLevel);

            XElement assimilation = new XElement("Assimilation");
            XElement assimilationFirstLevel = new XElement("FirstLevel");
            XElement modifierDuration = new XElement("ModifierDuration", ((float)(ElementManager.ThunderParameters[3][0])).ToString("G", CultureInfo.InvariantCulture));
            XElement movementSpeedModifier = new XElement("MovementSpeedModifier", ((float)(ElementManager.ThunderParameters[3][1])).ToString("G", CultureInfo.InvariantCulture));
            XElement attackSpeedModifier = new XElement("AttackSpeedModifier", ((float)(ElementManager.ThunderParameters[3][2])).ToString("G", CultureInfo.InvariantCulture));

            assimilationFirstLevel.Add(modifierDuration);
            assimilationFirstLevel.Add(movementSpeedModifier);
            assimilationFirstLevel.Add(attackSpeedModifier);

            assimilation.Add(assimilationFirstLevel);

            XElement assimilationSecondLevel = new XElement("SecondLevel");
            modifierDuration = new XElement("ModifierDuration", ((float)(ElementManager.ThunderParameters[4][0])).ToString("G", CultureInfo.InvariantCulture));
            movementSpeedModifier = new XElement("MovementSpeedModifier", ((float)(ElementManager.ThunderParameters[4][1])).ToString("G", CultureInfo.InvariantCulture));
            attackSpeedModifier = new XElement("AttackSpeedModifier", ((float)(ElementManager.ThunderParameters[4][2])).ToString("G", CultureInfo.InvariantCulture));

            assimilationSecondLevel.Add(modifierDuration);
            assimilationSecondLevel.Add(movementSpeedModifier);
            assimilationSecondLevel.Add(attackSpeedModifier);

            assimilation.Add(assimilationSecondLevel);

            XElement assimilationThirdLevel = new XElement("ThirdLevel");
            modifierDuration = new XElement("ModifierDuration", ((float)(ElementManager.ThunderParameters[5][0])).ToString("G", CultureInfo.InvariantCulture));
            movementSpeedModifier = new XElement("MovementSpeedModifier", ((float)(ElementManager.ThunderParameters[5][1])).ToString("G", CultureInfo.InvariantCulture));
            attackSpeedModifier = new XElement("AttackSpeedModifier", ((float)(ElementManager.ThunderParameters[5][2])).ToString("G", CultureInfo.InvariantCulture));

            assimilationThirdLevel.Add(modifierDuration);
            assimilationThirdLevel.Add(movementSpeedModifier);
            assimilationThirdLevel.Add(attackSpeedModifier);

            assimilation.Add(assimilationThirdLevel);

            thunderParameters.Add(assimilation);

            elements.Add(thunderParameters);
        }

        private void ClosePanel_Click(object sender, EventArgs e)
        {
            (Neon.World as EditorScreen).ToggleElementPanel();
        }
    }
}
