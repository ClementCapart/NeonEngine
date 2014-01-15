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
                    Console.WriteLine("Fire");
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

            SaveThunderParameters(elements);
            SaveFireParameters(elements);

            document.Add(elements);
            document.Save(@"../Data/Config/Elements.xml");
        }

        private void SaveFireParameters(XElement elements)
        {
            //Not done yet
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
            XElement stunDuration = new XElement("StunDuration", ((float)(ElementManager.ThunderParameters[2][6])).ToString("G", CultureInfo.InvariantCulture));

            thirdLevel.Add(gaugeCost);
            thirdLevel.Add(dashHorizontalImpulse);
            thirdLevel.Add(dashDuration);
            thirdLevel.Add(attackToLaunch);
            thirdLevel.Add(dashVerticalUpImpulse);
            thirdLevel.Add(dashVerticalDownImpulse);
            thirdLevel.Add(stunDuration);

            thunderParameters.Add(thirdLevel);

            XElement assimilation = new XElement("Assimilation");
            XElement modifierDuration = new XElement("ModifierDuration", ((float)(ElementManager.ThunderParameters[3][0])).ToString("G", CultureInfo.InvariantCulture));
            XElement movementSpeedModifier = new XElement("MovementSpeedModifier", ((float)(ElementManager.ThunderParameters[3][1])).ToString("G", CultureInfo.InvariantCulture));
            XElement attackSpeedModifier = new XElement("AttackSpeedModifier", ((float)(ElementManager.ThunderParameters[3][2])).ToString("G", CultureInfo.InvariantCulture));

            assimilation.Add(modifierDuration);
            assimilation.Add(movementSpeedModifier);
            assimilation.Add(attackSpeedModifier);

            thunderParameters.Add(assimilation);

            elements.Add(thunderParameters);
        }
    }
}
