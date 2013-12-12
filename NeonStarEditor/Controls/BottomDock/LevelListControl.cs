﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using NeonEngine;

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
                LevelDirectories = Directory.EnumerateDirectories(InitialLevelDirectory).ToArray();
                foreach (string s in LevelDirectories)
                {
                    TreeNode levelDirectory = new TreeNode(Path.GetFileName(s));
                    foreach(string fs in Directory.EnumerateFiles(s))
                    {
                        TreeNode levelFile = new TreeNode(Path.GetFileNameWithoutExtension(fs));
                        levelFile.Name = fs;
                        levelDirectory.Nodes.Add(levelFile);
                    }
                    
                    this.levelListTreeView.Nodes.Add(levelDirectory);
                }
            }
        }

        private void LoadLevel_Click(object sender, EventArgs e)
        {
            if (levelListTreeView.SelectedNode.Name.EndsWith(".xml"))
            {
                Neon.world.ChangeScreen(new LoadingScreen(Neon.game, levelListTreeView.SelectedNode.Name));
            }
        }

        private void newFolder_Click(object sender, EventArgs e)
        {

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
                File.Move(@"../Data/Levels/" + levelListTreeView.SelectedNode.Parent.Text + "/" + levelListTreeView.SelectedNode.Text + ".xml", @"../Data/Levels/" + levelListTreeView.SelectedNode.Parent.Text + "/" + levelNameText.Text + ".xml");
                Directory.Move(@"../Data/ContentStream/LevelsContent/" + levelListTreeView.SelectedNode.Parent.Text + "/" + levelListTreeView.SelectedNode.Text, @"../Data/ContentStream/LevelsContent/" + levelListTreeView.SelectedNode.Parent.Text + "/" + levelNameText.Text);
                levelListTreeView.SelectedNode.Text = groupNameText.Text;
                levelListTreeView.SelectedNode.Text = levelNameText.Text;
            }

            this.GameWorld.FocusedTextBox = null;
        }
    }
}
