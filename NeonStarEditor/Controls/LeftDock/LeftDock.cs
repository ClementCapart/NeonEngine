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
    public partial class LeftDock : MinimizableControl
    {
        public EditorScreen GameWorld;

        public LeftDock(EditorScreen GameWorld)
        {
            InitializeComponent();
            this.Width = this.MinimumSize.Width;
            this.Reverse = false;
            this.Side = true;
            this.GameWorld = GameWorld;
            this.MainToolbar.GameWorld = this.GameWorld;
        }

        private void ReloadButton_Click(object sender, EventArgs e)
        {

        }
    }
}
