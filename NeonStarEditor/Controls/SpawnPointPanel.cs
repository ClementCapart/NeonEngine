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
        public SpawnPoint CurrentSpawnPointSelected;

        public SpawnPointPanel(EditorScreen GameWorld)
        {
            this.GameWorld = GameWorld;
            this.Location = new Point(36, 0);
      
            InitializeComponent();
            InitializeSpawnPointData();
        }

        public void InitializeSpawnPointData()
        {
            this.sideComboBox.DataSource = Enum.GetValues(typeof(Side));

            GameWorld.SpawnPoints = GameWorld.SpawnPoints.OrderBy(sp => sp.Index).ToList();
            this.SpawnPointList.DataSource = null;
            this.SpawnPointList.DataSource = GameWorld.SpawnPoints;
            this.SpawnPointList.DisplayMember = "Index";
            
            if (CurrentSpawnPointSelected != null)
                InitializeSelectedData();
        }

        public void InitializeSelectedData()
        {
            this.sideComboBox.SelectedItem = CurrentSpawnPointSelected.Side;
        }

        private void ToggleDisplayAll_Click(object sender, EventArgs e)
        {
            GameWorld.DisplayAllSpawnPoint = !GameWorld.DisplayAllSpawnPoint;
        }

        private void AddSpawnButton_Click(object sender, EventArgs e)
        {
            GameWorld.CurrentTool = new AddSpawn(GameWorld);
        }

        private void sideComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CurrentSpawnPointSelected != null)
                CurrentSpawnPointSelected.Side = (Side)this.sideComboBox.SelectedItem;
        }

        private void RemoveSpawnButton_Click(object sender, EventArgs e)
        {
            if (CurrentSpawnPointSelected != null)
            {
                GameWorld.SpawnPoints.Remove(CurrentSpawnPointSelected);
                CurrentSpawnPointSelected = null;
                InitializeSpawnPointData();
            }
        }

        private void SpawnPointList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SpawnPointList.SelectedIndex != -1)
            {
                CurrentSpawnPointSelected = SpawnPointList.SelectedItem as SpawnPoint;
                InitializeSelectedData();
            }
        }
    }
}
