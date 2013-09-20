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
            Items.Add(new ToolStripDropDownButton("File"));
            Items.Add(new ToolStripButton("Play"));
            Dock = DockStyle.Top;
        }

    }
}
