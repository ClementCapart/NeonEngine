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
        public bool Side = false;
        public bool Reverse = false;

        public string MinButtonName = "MinimizeButton";
        public string MaxButtonName = "MaximizeButton";
        public string LeftButtonName = "Left";
        public string RightButtonName = "Right";
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MinimizableControl));

        public MinimizableControl()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            Point ButtonLocation;

            if(!Side)
                if (!Reverse)
                {
                    this.MinimizeButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject(MinButtonName)));
                    ButtonLocation = new Point(this.Width - this.MinimizeButton.Width - 3, this.Height - this.MinimizeButton.Height - 3);
                }
                else
                {
                    this.MinimizeButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject(MaxButtonName)));
                    ButtonLocation = new Point(this.Width - this.MinimizeButton.Width - 3, 3);
                }
            else
                if (!Reverse)
                {
                    this.MinimizeButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject(RightButtonName)));
                    ButtonLocation = new Point(this.Width - this.MinimizeButton.Width - 3, this.Height - this.MinimizeButton.Height -3);
                }
                else
                {
                    this.MinimizeButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject(LeftButtonName)));
                    ButtonLocation = new Point(3, this.Height - this.MinimizeButton.Height - 3);
                }
            this.MinimizeButton.Location = new Point(this.Width - this.MinimizeButton.Width - 3, 3);
            base.OnLoad(e);
        }

        public void MinimizeButton_Click(object sender, EventArgs e)
        {
            if (!Minimized)
            {
                if (!Side)
                {
                    if (!Reverse)
                        this.MinimizeButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject(MinButtonName)));
                    else
                        this.MinimizeButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject(MaxButtonName)));
                    this.Height = this.MinimumSize.Height;
                }
                else
                {
                    if (!Reverse)
                        this.MinimizeButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject(RightButtonName)));
                    else
                        this.MinimizeButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject(LeftButtonName)));
                    this.Width = this.MinimumSize.Width;
                }
                this.Minimized = true;
            }
            else
            {
                if (!Side)
                {
                    if (!Reverse)
                        this.MinimizeButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject(MaxButtonName)));
                    else
                        this.MinimizeButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject(MinButtonName)));
                    this.Height = this.MaximumSize.Height;
                }
                else
                {
                    if (!Reverse)
                        this.MinimizeButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject(LeftButtonName)));
                    else
                        this.MinimizeButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject(RightButtonName)));
                    this.Width = this.MaximumSize.Width;
                }              
                this.Minimized = false;
            }
        }
    }
}
