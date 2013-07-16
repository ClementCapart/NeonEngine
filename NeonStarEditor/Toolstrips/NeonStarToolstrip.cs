using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NeonStarEditor
{
    public class NeonStarToolstrip : ToolStrip
    {
    
        public NeonStarToolstrip()
            :base()
        {
            this.Items.Add(new ToolStripDropDownButton("File"));
            this.Items.Add(new ToolStripButton("Play"));
            this.Dock = DockStyle.Top;
        }

    }
}
