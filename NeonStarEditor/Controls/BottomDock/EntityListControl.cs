using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NeonEngine;
using System.Xml.Linq;
using System.Reflection;

namespace NeonStarEditor
{
    public partial class EntityListControl : UserControl
    {
        public EditorScreen GameWorld = null;

        public EntityListControl()
            :base()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            if (GameWorld != null)
            {
                this.EntityListBox.DataSource = GameWorld.entityList;
                this.EntityListBox.DisplayMember = "Name";
                this.EntityListBox.SelectedItem = null;
            }
            base.OnLoad(e);
        }

        private void RemoveEntityButton_Click(object sender, EventArgs e)
        {
            if (this.GameWorld.SelectedEntity != null)
            {
                this.GameWorld.SelectedEntity.Destroy();  
            }
            else
                Console.WriteLine("No entity selected");

        }

        private void AddEntityButton_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Add : Entity");
            GameWorld.entityList.Add(new Entity(GameWorld));
            ActionManager.SaveAction(ActionType.AddEntity, GameWorld.entityList.Last());
        }

        private void SaveAsPrefabButton_Click(object sender, EventArgs e)
        {
            if (GameWorld.SelectedEntity != null)
            {
                SavePrefabDialog.InitialDirectory = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Application.ExecutablePath), @"Prefabs");
                SavePrefabDialog.FileName = GameWorld.SelectedEntity.Name;
                if (SavePrefabDialog.ShowDialog() == DialogResult.OK)
                {
                    DataManager.SavePrefab(GameWorld.SelectedEntity, SavePrefabDialog.FileName);
                    GameWorld.BottomDockControl.prefabListControl.RefreshList();
                }
            }
        }

        private void EntityListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            GameWorld.SelectedEntity = (Entity)this.EntityListBox.SelectedItem;
            if (GameWorld.SelectedEntity != null)
                GameWorld.RefreshInspector(GameWorld.SelectedEntity);
            else
                GameWorld.RefreshInspector(null);
        }
    }
}
