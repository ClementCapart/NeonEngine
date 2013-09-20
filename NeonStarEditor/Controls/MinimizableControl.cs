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
        ComponentResourceManager resources = new ComponentResourceManager(typeof(MinimizableControl));

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
                    MinimizeButton.BackgroundImage = ((Image)(resources.GetObject(MinButtonName)));
                    ButtonLocation = new Point(Width - MinimizeButton.Width - 3, Height - MinimizeButton.Height - 3);
                }
                else
                {
                    MinimizeButton.BackgroundImage = ((Image)(resources.GetObject(MaxButtonName)));
                    ButtonLocation = new Point(Width - MinimizeButton.Width - 3, 3);
                }
            else
                if (!Reverse)
                {
                    MinimizeButton.BackgroundImage = ((Image)(resources.GetObject(RightButtonName)));
                    ButtonLocation = new Point(Width - MinimizeButton.Width - 3, Height - MinimizeButton.Height -3);
                }
                else
                {
                    MinimizeButton.BackgroundImage = ((Image)(resources.GetObject(LeftButtonName)));
                    ButtonLocation = new Point(3, Height - MinimizeButton.Height - 3);
                }
            MinimizeButton.Location = new Point(Width - MinimizeButton.Width - 3, 3);
            base.OnLoad(e);
        }

        public void MinimizeButton_Click(object sender, EventArgs e)
        {
            if (!Minimized)
            {
                if (!Side)
                {
                    if (!Reverse)
                        MinimizeButton.BackgroundImage = ((Image)(resources.GetObject(MinButtonName)));
                    else
                        MinimizeButton.BackgroundImage = ((Image)(resources.GetObject(MaxButtonName)));
                    Height = MinimumSize.Height;
                }
                else
                {
                    if (!Reverse)
                        MinimizeButton.BackgroundImage = ((Image)(resources.GetObject(RightButtonName)));
                    else
                        MinimizeButton.BackgroundImage = ((Image)(resources.GetObject(LeftButtonName)));
                    Width = MinimumSize.Width;
                }
                Minimized = true;
            }
            else
            {
                if (!Side)
                {
                    if (!Reverse)
                        MinimizeButton.BackgroundImage = ((Image)(resources.GetObject(MaxButtonName)));
                    else
                        MinimizeButton.BackgroundImage = ((Image)(resources.GetObject(MinButtonName)));
                    Height = MaximumSize.Height;
                }
                else
                {
                    if (!Reverse)
                        MinimizeButton.BackgroundImage = ((Image)(resources.GetObject(LeftButtonName)));
                    else
                        MinimizeButton.BackgroundImage = ((Image)(resources.GetObject(RightButtonName)));
                    Width = MaximumSize.Width;
                }              
                Minimized = false;
            }
        }
    }
}
