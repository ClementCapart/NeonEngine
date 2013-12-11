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

namespace NeonStarEditor.Controls.BottomDock
{
    public partial class LevelListControl : UserControl
    {
        public string InitialLevelDirectory = @"../Data/Levels/";

        public string[] LevelDirectories;

        public LevelListControl()
        {
            InitializeComponent();           
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
    }
}
