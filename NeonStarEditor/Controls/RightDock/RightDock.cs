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
    public partial class RightDock : MinimizableControl
    {
        public EditorScreen GameWorld;

        public RightDock(EditorScreen GameWorld)
        {
            InitializeComponent();
            Width = MinimumSize.Width;
            Reverse = true;
            Side = true;
            this.GameWorld = GameWorld;
            InspectorControl.GameWorld = GameWorld;

            Dock = DockStyle.Right;
            
        }
    }
}
