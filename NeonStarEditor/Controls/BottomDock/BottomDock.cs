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
            Reverse = true;
            Dock = DockStyle.Bottom;
            Height = MinimumSize.Height;
            this.GameWorld = GameWorld;
            entityListControl.GameWorld = this.GameWorld;
            prefabListControl.GameWorld = this.GameWorld;
            levelList.GameWorld = this.GameWorld;
        }
    }
}
