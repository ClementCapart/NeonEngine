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
    public partial class CameraPanel : UserControl
    {
        public CameraPanel()
        {
            InitializeComponent();
            this.MassNU.Value = (decimal)Neon.world.camera.mass;
            this.DampingNU.Value = (decimal)Neon.world.camera.damping;
            this.StiffnessNU.Value = (decimal)Neon.world.camera.stiffness;
        }

        private void ClosePanel_Click(object sender, EventArgs e)
        {
            (Neon.world as EditorScreen).ToggleCameraManager();
        }

        private void MassNU_ValueChanged(object sender, EventArgs e)
        {
            Neon.world.camera.mass = (float)(sender as NumericUpDown).Value;
        }

        private void DampingNU_ValueChanged(object sender, EventArgs e)
        {
            Neon.world.camera.damping = (float)(sender as NumericUpDown).Value;
        }

        private void StiffnessNU_ValueChanged(object sender, EventArgs e)
        {
            Neon.world.camera.stiffness = (float)(sender as NumericUpDown).Value;
        }

        private void Numeric_Enter(object sender, EventArgs e)
        {
            (Neon.world as EditorScreen).FocusedNumericUpDown = sender as NumericUpDown;
        }

        private void Numeric_Leave(object sender, EventArgs e)
        {
            (Neon.world as EditorScreen).FocusedNumericUpDown = null;
        }


    }
}
