using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NeonStarEditor
{
    public partial class MinimizableControl : UserControl
    {
        public bool Minimized = true;
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MinimizableControl));

        public MinimizableControl()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            this.MinimizeButton.Location = new Point(this.Width - this.MinimizeButton.Width - 3, 3);
            base.OnLoad(e);
        }

        public void MinimizeButton_Click(object sender, EventArgs e)
        {
            if (!Minimized)
            {
                this.MinimizeButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("MaximizeButton")));
                this.Height = this.MinimumSize.Height;
                this.Minimized = true;
            }
            else
            {
                this.MinimizeButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("MinimizeButton")));
                this.Height = this.MaximumSize.Height;
                this.Minimized = false;
            }
        }
    }
}
