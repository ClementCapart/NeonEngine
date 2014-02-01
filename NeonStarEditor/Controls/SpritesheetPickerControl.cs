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
    public partial class SpritesheetPickerControl : UserControl
    {
        public EditorScreen GameWorld;
        PropertyInfo _propertyInfo;
        NeonEngine.Component _component;
        Label _label;

        SpritesheetInspector _spritesheetInspector;
        TextBox _currentAnimation;
        Label _currentSpritesheet;


        public SpritesheetPickerControl(EditorScreen gameWorld, PropertyInfo pi, NeonEngine.Component c, Label l)
        {
            GameWorld = gameWorld;

            _propertyInfo = pi;
            _component = c;
            _label = l;
         
            InitializeComponent();
            if (GameWorld.SelectedEntity != null)
                this.entityInfo.Text = GameWorld.SelectedEntity.Name + " - " + c.Name + " - " + pi.Name;
            else
                GameWorld.ToggleSpritesheetPicker();
            this.Location = new System.Drawing.Point((int)(Neon.HalfScreen.X - this.Width / 2) - 150, (int)(Neon.HalfScreen.Y - this.Height / 2));

            InitializeSpritesheetList();
           
        }

        public SpritesheetPickerControl(EditorScreen gameWorld, SpritesheetInspector spritesheetInspector, TextBox currentAnimation, Label currentSpritesheet)
        {
            GameWorld = gameWorld;
            _spritesheetInspector = spritesheetInspector;
            _currentAnimation = currentAnimation;
            _currentSpritesheet = currentSpritesheet;

            InitializeComponent();
            if (GameWorld.SelectedEntity != null)
                this.entityInfo.Text = GameWorld.SelectedEntity.Name + " - " + "SpritesheetManager" + " - " + currentAnimation.Text;
            else
                GameWorld.ToggleSpritesheetPicker();

            this.Location = new System.Drawing.Point((int)(Neon.HalfScreen.X - this.Width / 2) - 150, (int)(Neon.HalfScreen.Y - this.Height / 2));

            InitializeSpritesheetList();
        }

        public void InitializeSpritesheetList()
        {
            TreeNode commonAssets = new TreeNode("CommonSpritesheets");
            commonAssets.Name = "CommonSpritesheets";
            assetList.Nodes.Add(commonAssets);

            foreach (KeyValuePair<string, string> kvp in AssetManager.CommonSpritesheets)
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

            TreeNode groupAssets = new TreeNode("GroupSpritesheets");
            groupAssets.Name = "GroupSpritesheets";
            assetList.Nodes.Add(groupAssets);

            foreach (KeyValuePair<string, string> kvp in AssetManager.GroupSpritesheets)
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

            TreeNode levelAssets = new TreeNode("LevelSpritesheets");
            levelAssets.Name = "LevelSpritesheets";
            assetList.Nodes.Add(levelAssets);

            foreach (KeyValuePair<string, string> kvp in AssetManager.LevelSpritesheets)
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
            GameWorld.ToggleSpritesheetPicker();
        }

        private void SelectButton_Click(object sender, EventArgs e)
        {
            if (GameWorld.SelectedEntity != null && _propertyInfo != null && _component != null && _label != null && assetList.SelectedNode.Nodes.Count == 0)
            {
                _label.Text = assetList.SelectedNode.Text;
                _propertyInfo.SetValue(_component, assetList.SelectedNode.Text, null);
            }
            else if (GameWorld.SelectedEntity != null && _spritesheetInspector != null && _currentAnimation != null && _currentSpritesheet != null && assetList.SelectedNode.Nodes.Count == 0)
            {
                _currentSpritesheet.Text = assetList.SelectedNode.Text;
                
                if(_spritesheetInspector.spritesheetList.ContainsKey(_currentAnimation.Text))
                    _spritesheetInspector.spritesheetList[_currentAnimation.Text] = AssetManager.GetSpriteSheet(assetList.SelectedNode.Text);
            }
            else
            {
                Console.WriteLine("Error : Couldn't select this as a Spritesheet");
            }
        }

        private void assetList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (assetList.SelectedNode.Nodes.Count == 0)
            {
                this.spritesheetView1.LoadSpritesheet((string)assetList.SelectedNode.Tag, assetList.SelectedNode.Text);
                this.spritesheetView1.Position = new Vector2(spritesheetView1.Width / 2, spritesheetView1.Height / 2);
                this.spritesheetView1.Spritesheet.Zoom = 2.0f;
            }
        }

        private void ChangeBackgroundColorButton_Click(object sender, EventArgs e)
        {
            DialogResult result = BackgroundColorDialog.ShowDialog();
            // See if user pressed ok.
            if (result == DialogResult.OK)
            {
                // Set form background to the selected color.
                this.spritesheetView1.BackgroundColor = new Microsoft.Xna.Framework.Color(BackgroundColorDialog.Color.R, BackgroundColorDialog.Color.G, BackgroundColorDialog.Color.B, BackgroundColorDialog.Color.A);
            }
        }

        private void GraphicPickerControl_Load(object sender, EventArgs e)
        {

        }
    }
}
