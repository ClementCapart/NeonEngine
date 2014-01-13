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
    }
}
