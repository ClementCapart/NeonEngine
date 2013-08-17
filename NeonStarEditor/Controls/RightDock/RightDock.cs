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
            this.Width = this.MinimumSize.Width;
            this.Reverse = true;
            this.Side = true;
            this.GameWorld = GameWorld;
            this.Dock = DockStyle.Right;
            
        }
    }
}
