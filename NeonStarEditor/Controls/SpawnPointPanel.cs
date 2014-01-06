using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NeonEngine;

namespace NeonStarEditor
{
    public partial class SpawnPointPanel : UserControl
    {
        public EditorScreen GameWorld;
        public Node CurrentNodeSelected;

        public SpawnPointPanel(EditorScreen GameWorld)
        {
            this.GameWorld = GameWorld;
            this.Location = new Point(36, 0);
            InitializeComponent();
        }

        private void ToggleDisplayAll_Click(object sender, EventArgs e)
        {
            GameWorld.DisplayAllPathNodeList = !GameWorld.DisplayAllPathNodeList;
        }
    }
}
