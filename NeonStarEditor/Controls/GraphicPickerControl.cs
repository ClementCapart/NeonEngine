using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NeonEngine;
using System.Reflection;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace NeonStarEditor.Controls
{
    public partial class GraphicPickerControl : UserControl
    {
        public EditorScreen GameWorld;
        PropertyInfo _propertyInfo;
        NeonEngine.Component _component;
        Label _label;


        public GraphicPickerControl(EditorScreen gameWorld, PropertyInfo pi, NeonEngine.Component c, Label l)
        {
            GameWorld = gameWorld;

            _propertyInfo = pi;
            _component = c;
            _label = l;
         
            InitializeComponent();
            if (GameWorld.SelectedEntity != null)
                this.entityInfo.Text = GameWorld.SelectedEntity.Name + " - " + c.Name + " - " + pi.Name;
            else
                GameWorld.ToggleGraphicPicker();
            this.Location = new System.Drawing.Point((int)(Neon.HalfScreen.X - this.Width / 2) - 150, (int)(Neon.HalfScreen.Y - this.Height / 2));

            InitializeGraphicList();
           
        }

        public void InitializeGraphicList()
        {
            Dictionary<string, string> allAssets = new Dictionary<string, string>();

            TreeNode noAsset = new TreeNode("NoGraphic");
            noAsset.Name = "NoGraphic";
            assetList.Nodes.Add(noAsset);

            TreeNode commonAssets = new TreeNode("CommonAssets");
            commonAssets.Name = "CommonAssets";
            assetList.Nodes.Add(commonAssets);

            foreach (KeyValuePair<string, string> kvp in AssetManager.CommonAssets)
            {
                string[] folders = kvp.Value.Split('\\');
                TreeNode tn = commonAssets;

                for (int i = 1; i < folders.Length; i++)
                {
                    if (i == folders.Length - 1)
                    {
                        TreeNode newNode = new TreeNode(kvp.Key);
                        newNode.Name = kvp.Key;
                        newNode.Tag = kvp.Value;
                        tn.Nodes.Add(newNode);
                        tn = newNode;
                        if (_label.Text == newNode.Name)
                            graphicView1.NodeToSelect = newNode;
                        continue;
                    }
                    if (!tn.Nodes.ContainsKey(folders[i]))
                    {
                        TreeNode newNode = new TreeNode(folders[i]);
                        newNode.Name = folders[i];
                        tn.Nodes.Add(newNode);
                        tn = newNode;
                    }
                    else
                    {
                        tn = tn.Nodes[folders[i]];
                    }
                }
            }

            if (commonAssets.Nodes.Count == 0)
                commonAssets.Remove();

            TreeNode groupAssets = new TreeNode("GroupAssets");
            groupAssets.Name = "GroupAssets";
            assetList.Nodes.Add(groupAssets);

            foreach (KeyValuePair<string, string> kvp in AssetManager.GroupAssets)
            {
                string[] folders = kvp.Value.Split('\\');
                TreeNode tn = groupAssets;

                for (int i = 1; i < folders.Length; i++)
                {
                    if (i == folders.Length - 1)
                    {
                        TreeNode newNode = new TreeNode(kvp.Key);
                        newNode.Name = kvp.Key;
                        newNode.Tag = kvp.Value;
                        tn.Nodes.Add(newNode);
                        tn = newNode;
                        if (_label.Text == newNode.Name)
                            graphicView1.NodeToSelect = newNode;
                        continue;
                    }
                    if (!tn.Nodes.ContainsKey(folders[i]))
                    {
                        TreeNode newNode = new TreeNode(folders[i]);
                        newNode.Name = folders[i];
                        tn.Nodes.Add(newNode);
                        tn = newNode;
                    }
                    else
                    {
                        tn = tn.Nodes[folders[i]];
                    }
                }
            }

            if (groupAssets.Nodes.Count == 0)
                groupAssets.Remove();

            TreeNode levelAssets = new TreeNode("LevelAssets");
            levelAssets.Name = "LevelAssets";
            assetList.Nodes.Add(levelAssets);

            foreach (KeyValuePair<string, string> kvp in AssetManager.LevelAssets)
            {
                string[] folders = kvp.Value.Split('\\');
                TreeNode tn = levelAssets;

                for (int i = 1; i < folders.Length; i++)
                {
                    if (i == folders.Length - 1)
                    {
                        TreeNode newNode = new TreeNode(kvp.Key);
                        newNode.Name = kvp.Key;
                        newNode.Tag = kvp.Value;
                        tn.Nodes.Add(newNode);
                        tn = newNode;
                        if (_label.Text == newNode.Name)
                            graphicView1.NodeToSelect = newNode;
                        continue;
                    }
                    if (!tn.Nodes.ContainsKey(folders[i]))
                    {
                        TreeNode newNode = new TreeNode(folders[i]);
                        newNode.Name = folders[i];
                        tn.Nodes.Add(newNode);
                        tn = newNode;
                    }
                    else
                    {
                        tn = tn.Nodes[folders[i]];
                    }
                }
            }
            if (levelAssets.Nodes.Count == 0)
                levelAssets.Remove();
        }

        private void ClosePanel_Click(object sender, EventArgs e)
        {
            GameWorld.ToggleGraphicPicker();
        }

        private void SelectButton_Click(object sender, EventArgs e)
        {
            if (GameWorld.SelectedEntity != null && _propertyInfo != null && _component != null && _label != null && assetList.SelectedNode != null && assetList.SelectedNode.Nodes.Count == 0)
            {
                _label.Text = assetList.SelectedNode.Text;
                _propertyInfo.SetValue(_component, assetList.SelectedNode.Text, null);
                GameWorld.ToggleGraphicPicker();
            }
            else if (GameWorld.SelectedEntity != null && _propertyInfo != null && _component != null && _label != null && assetList.SelectedNode != null && assetList.SelectedNode.Name == "NoGraphic")
            {
                _label.Text = "";
                _propertyInfo.SetValue(_component, "", null);
                GameWorld.ToggleGraphicPicker();
            }
            else
            {
                Console.WriteLine("Error : Couldn't select this as a Graphic");
            }
        }

        private void assetList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (assetList.SelectedNode.Nodes.Count == 0 && assetList.SelectedNode.Tag != null)
            {
                    graphicView1.LoadTexture((string)assetList.SelectedNode.Tag);
                    graphicView1.Position = Vector2.Zero;
                    graphicView1.Zoom = 2.0f;               
            }
        }

        private void ChangeBackgroundColorButton_Click(object sender, EventArgs e)
        {
            DialogResult result = BackgroundColorDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                graphicView1.BackgroundColor = new Microsoft.Xna.Framework.Color(BackgroundColorDialog.Color.R, BackgroundColorDialog.Color.G, BackgroundColorDialog.Color.B, BackgroundColorDialog.Color.A);
            }
        }
    }
}
