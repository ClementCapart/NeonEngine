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
    public partial class BottomDock : MinimizableControl
    {
        public EditorScreen GameWorld;

        public BottomDock(EditorScreen GameWorld)
        {
            InitializeComponent();
            this.Dock = DockStyle.Bottom;
            this.Height = this.MinimumSize.Height;
            this.GameWorld = GameWorld;
            this.entityListControl.GameWorld = this.GameWorld;
            this.prefabListControl.GameWorld = this.GameWorld;
        }
    }
}
