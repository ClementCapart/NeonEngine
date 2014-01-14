using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using NeonEngine;
using System.Xml.Linq;
using Microsoft.Xna.Framework;

namespace NeonStarEditor.Controls.BottomDock
{
    public partial class LevelListControl : UserControl
    {
        public string InitialLevelDirectory = @"../Data/Levels/";

        public string[] LevelDirectories;

        public EditorScreen GameWorld;

        public LevelListControl()
        {
            InitializeComponent();
            label1.Hide();
            label2.Hide();
            groupNameText.Hide();
            levelNameText.Hide();
        }

        private void LevelListControl_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                InitListTreeView();
            }
        }

        private void InitListTreeView()
        {
            levelListTreeView.Nodes.Clear();
            LevelDirectories = Directory.EnumerateDirectories(InitialLevelDirectory).ToArray();
            foreach (string s in LevelDirectories)
            {
                TreeNode levelDirectory = new TreeNode(Path.GetFileName(s));
                foreach (string fs in Directory.EnumerateDirectories(s))
                {
                    string levelName = Path.GetFileName(fs);
                    TreeNode levelFile = new TreeNode(levelName);
                    levelDirectory.Nodes.Add(levelFile);
                }

                this.levelListTreeView.Nodes.Add(levelDirectory);
            }
        }

        private void LoadLevel_Click(object sender, EventArgs e)
        {
            if (levelListTreeView.SelectedNode != null)
            {
                if (levelListTreeView.SelectedNode.Parent != null)
                {
                    Neon.World.ChangeScreen(new LoadingScreen(Neon.Game, (int)loadSpawnPoint.Value, levelListTreeView.SelectedNode.Parent.Text ,levelListTreeView.SelectedNode.Text));
                }
            }
        }

        private void newFolder_Click(object sender, EventArgs e)
        {
            int i = 0;
            bool nameIsOk;

            do
            {
                nameIsOk = true;

                foreach (TreeNode tr in levelListTreeView.Nodes)
                {
                    if (tr.Text == "NewGroup" + i)
                    {
                        i++;
                        nameIsOk = false;
                        break;
                    }
                }
            }
            while (!nameIsOk);        

            Directory.CreateDirectory(@"../Data/Levels/NewGroup" + i);
            Directory.CreateDirectory(@"../Data/ContentStream/LevelsContent/NewGroup" + i + "/Common");
            InitListTreeView();

        }

        private void levelListTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (levelListTreeView.SelectedNode.Parent != null)
            {
                label1.Show();
                label2.Show();
                groupNameText.Show();
                groupNameText.Text = levelListTreeView.SelectedNode.Parent.Text;
                levelNameText.Show();
                levelNameText.Text = levelListTreeView.SelectedNode.Text;
            }
            else
            {
                label2.Show();
                groupNameText.Show();
                groupNameText.Text = levelListTreeView.SelectedNode.Text;
                label1.Hide();
                levelNameText.Text = "";
                levelNameText.Hide();
            }
        }

        private void levelNameText_Enter(object sender, EventArgs e)
        {
            this.GameWorld.FocusedTextBox = (TextBox)sender;
        }

        private void levelNameText_Leave(object sender, EventArgs e)
        {
            if ((sender as TextBox) == groupNameText)
            {
                if (levelListTreeView.SelectedNode.Parent == null)
                {
                    Directory.Move(@"../Data/Levels/" + levelListTreeView.SelectedNode.Text, @"../Data/Levels/" + groupNameText.Text);
                    Directory.Move(@"../Data/ContentStream/LevelsContent/" + levelListTreeView.SelectedNode.Text, @"../Data/ContentStream/LevelsContent/" + groupNameText.Text);
                    levelListTreeView.SelectedNode.Text = groupNameText.Text;
                }
                else
                {
                    Directory.Move(@"../Data/Levels/" + levelListTreeView.SelectedNode.Parent.Text, @"../Data/Levels/" + groupNameText.Text);
                    Directory.Move(@"../Data/ContentStream/LevelsContent/" + levelListTreeView.SelectedNode.Parent.Text, @"../Data/ContentStream/LevelsContent/" + groupNameText.Text);
                    levelListTreeView.SelectedNode.Parent.Text = groupNameText.Text;
                }
            }
            else
            {
                Directory.Move(@"../Data/Levels/" + levelListTreeView.SelectedNode.Parent.Text + "/" + levelListTreeView.SelectedNode.Text, @"../Data/Levels/" + levelListTreeView.SelectedNode.Parent.Text + "/" + levelNameText.Text);
                Directory.Move(@"../Data/ContentStream/LevelsContent/" + levelListTreeView.SelectedNode.Parent.Text + "/" + levelListTreeView.SelectedNode.Text, @"../Data/ContentStream/LevelsContent/" + levelListTreeView.SelectedNode.Parent.Text + "/" + levelNameText.Text);

                foreach (string file in Directory.EnumerateFiles(@"../Data/Levels/" + levelListTreeView.SelectedNode.Parent.Text + "/" + levelNameText.Text))
                {
                    string fileName = Path.GetFileNameWithoutExtension(file);
                    if(fileName.Split('_').Length > 1)
                    {
                        File.Move(file, Path.GetDirectoryName(file) + "/" + levelNameText.Text + "_" + fileName.Split('_')[1] + ".xml");
                    }
                    else
                    {
                        File.Move(file, Path.GetDirectoryName(file) + "/" + levelNameText.Text + ".xml");
                    }
                }

                levelListTreeView.SelectedNode.Text = groupNameText.Text;
                levelListTreeView.SelectedNode.Text = levelNameText.Text;
            }

            this.GameWorld.FocusedTextBox = null;
        }

        private void AddEntityButton_Click(object sender, EventArgs e)
        {
            if (levelListTreeView.SelectedNode.Parent == null)
            {
                int i = 0;
                bool nameIsOk;

                do
                {
                    nameIsOk = true;

                    foreach (TreeNode tr in levelListTreeView.Nodes)
                    {
                        if (tr.Text == "NewLevel" + i)
                        {
                            i++;
                            nameIsOk = false;
                            break;
                        }
                    }
                }
                while (!nameIsOk);

                XDocument document = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
                XElement level = new XElement("LevelInfos");

                document.Add(level);
                Directory.CreateDirectory(@"../Data/Levels/" + levelListTreeView.SelectedNode.Text + "/NewLevel" + i + "/");
                document.Save(@"../Data/Levels/" + levelListTreeView.SelectedNode.Text + "/NewLevel" + i + "/NewLevel" + i + "_Info.xml");

                Directory.CreateDirectory(@"../Data/ContentStream/LevelsContent/" + levelListTreeView.SelectedNode.Text + " /NewLevel" + i);
                InitListTreeView();
            }
        }

        private void loadSpawnPoint_Leave(object sender, EventArgs e)
        {
            GameWorld.FocusedNumericUpDown = null;
        }

        private void loadSpawnPoint_Enter(object sender, EventArgs e)
        {
            GameWorld.FocusedNumericUpDown = (sender as NumericUpDown);
        }

        private void DefaultLayerBox_Enter(object sender, EventArgs e)
        {
            GameWorld.FocusedTextBox = sender as TextBox;
        }


        private void DefaultLayerBox_TextChanged(object sender, EventArgs e)
        {
            GameWorld.DefaultLayer = (sender as TextBox).Text;
        }
    }
}
