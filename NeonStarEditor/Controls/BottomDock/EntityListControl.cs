using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
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
                EntityListBox.DataSource = GameWorld.entityList;
                EntityListBox.DisplayMember = "Name";
                EntityListBox.SelectedItem = null;
            }
            base.OnLoad(e);
        }

        private void RemoveEntityButton_Click(object sender, EventArgs e)
        {
            if (GameWorld.SelectedEntity != null)
            {
                ActionManager.SaveAction(ActionType.DeleteEntity, new object[2] { DataManager.SavePrefab(GameWorld.SelectedEntity), GameWorld });
                GameWorld.SelectedEntity.Destroy();
                GameWorld.SelectedEntity = null;
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
                SavePrefabDialog.InitialDirectory = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"../Data/Prefabs");
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
            GameWorld.SelectedEntity = (Entity)EntityListBox.SelectedItem;
            GameWorld.EntityChangedThisFrame = true;
            GameWorld.FocusedNumericUpDown = null;
            GameWorld.FocusedTextBox = null;
            if (GameWorld.SelectedEntity != null)
                GameWorld.RefreshInspector(GameWorld.SelectedEntity);
            else
                GameWorld.RefreshInspector(null);
        }

        private void duplicateButton_Click(object sender, EventArgs e)
        {
            if (GameWorld.SelectedEntity != null)
            {
                DataManager.LoadPrefab(DataManager.SavePrefab(GameWorld.SelectedEntity), GameWorld);
                Entity entity = GameWorld.entities.Last();
                entity.transform.Position += new Microsoft.Xna.Framework.Vector2(100, -100);
                entity.transform.InitialPosition = entity.transform.Position;
            }
        }
    }
}
