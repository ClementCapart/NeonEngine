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

        Dictionary<string, List<Entity>> _layerList = new Dictionary<string, List<Entity>>();

        public EntityListControl()
            :base()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            InitializeEntityList();
            base.OnLoad(e);
        }

        public void InitializeEntityList()
        {
            if (GameWorld != null)
            {
                _layerList.Clear();
                EntityListBox.Nodes.Clear();

                if (GameWorld.Entities.Count > 0)
                {
                    foreach (Entity entity in GameWorld.Entities)
                    {
                        if (_layerList.ContainsKey(entity.Layer))
                        {
                            _layerList[entity.Layer].Add(entity);
                            TreeNode tn = new TreeNode(entity.Name);
                            tn.Tag = entity;
                            EntityListBox.Nodes[(entity.Layer != "" ? entity.Layer : "NoLayer")].Nodes.Add(tn);
                        }
                        else
                        {
                            _layerList.Add(entity.Layer, new List<Entity>());
                            TreeNode layerNode = new TreeNode((entity.Layer != "" ? entity.Layer : "NoLayer"));
                            layerNode.Name = (entity.Layer != "" ? entity.Layer : "NoLayer");
                            EntityListBox.Nodes.Add(layerNode);
                            TreeNode tn = new TreeNode(entity.Name);
                            tn.Tag = entity;
                            EntityListBox.Nodes[(entity.Layer != "" ? entity.Layer : "NoLayer")].Nodes.Add(tn);
                            _layerList[entity.Layer].Add(entity);
                        }
                    }
                }
            }
        }

        private void RemoveEntityButton_Click(object sender, EventArgs e)
        {
            if (GameWorld.SelectedEntity != null)
            {
                //ActionManager.SaveAction(ActionType.DeleteEntity, new object[2] { DataManager.SavePrefab(GameWorld.SelectedEntity), GameWorld });
                GameWorld.SelectedEntity.Destroy();
                GameWorld.SelectedEntity = null;
            }
            else
                Console.WriteLine("No entity selected");

        }

        private void AddEntityButton_Click(object sender, EventArgs e)
        {
            Entity ent = new Entity(GameWorld);
            ent.transform.Position = Neon.World.Camera.Position;
            ent.Layer = GameWorld.BottomDockControl.levelList.DefaultLayerBox.Text;
            GameWorld.AddEntity(ent);
            //ActionManager.SaveAction(ActionType.AddEntity, GameWorld.Entities.Last());
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

        private void EntityListBox_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (EntityListBox.SelectedNode.Tag != null)
            {
                GameWorld.SelectedEntity = (Entity)EntityListBox.SelectedNode.Tag;
                GameWorld.EntityChangedThisFrame = true;
                GameWorld.FocusedNumericUpDown = null;
                GameWorld.FocusedTextBox = null;
                if (GameWorld.SelectedEntity != null)
                    GameWorld.RefreshInspector(GameWorld.SelectedEntity);
                else
                    GameWorld.RefreshInspector(null);
            }         
        }

        private void duplicateButton_Click(object sender, EventArgs e)
        {
            if (GameWorld.SelectedEntity != null)
            {
                DataManager.LoadPrefab(DataManager.SavePrefab(GameWorld.SelectedEntity), GameWorld);
                Entity entity = GameWorld.Entities.Last();
                entity.transform.Position += new Microsoft.Xna.Framework.Vector2(100, -100);
                entity.transform.InitialPosition = entity.transform.Position;
                entity.Layer = GameWorld.BottomDockControl.levelList.DefaultLayerBox.Text;
                SelectEntityNode(entity);
            }
        }

        public void SelectEntityNode(Entity entity)
        {
            foreach (TreeNode tn in EntityListBox.Nodes[entity.Layer != "" ? entity.Layer : "NoLayer"].Nodes)
                if (tn.Tag == entity)
                {
                    EntityListBox.SelectedNode = tn;
                    EntityListBox.Select();
                    break;
                }
        }

        private void OrderList_Click(object sender, EventArgs e)
        {
            for (int i = _layerList.Count - 1; i >= 0; i--)
            {
                KeyValuePair<string, List<Entity>> kvp = _layerList.ElementAt(i);
                _layerList.Remove(kvp.Key);
                _layerList.Add(kvp.Key, kvp.Value.OrderBy(ent => ent.Name).ToList());
            }

            _layerList = _layerList.OrderBy(k => k.Key).ToDictionary(k => k.Key, elem => elem.Value);

            EntityListBox.Nodes.Clear();

            foreach (KeyValuePair<string, List<Entity>> kvp in _layerList)
            {
                TreeNode layerNode = new TreeNode((kvp.Key != "" ? kvp.Key : "NoLayer"));
                layerNode.Name = (kvp.Key != "" ? kvp.Key : "NoLayer");
                EntityListBox.Nodes.Add(layerNode);
                foreach (Entity ent in kvp.Value)
                {
                    TreeNode tn = new TreeNode(ent.Name);
                    tn.Tag = ent;
                    EntityListBox.Nodes[kvp.Key != "" ? kvp.Key : "NoLayer"].Nodes.Add(tn);
                }
            }
        }


    }
}
