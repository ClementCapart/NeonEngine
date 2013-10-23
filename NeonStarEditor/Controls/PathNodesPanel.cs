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
    public partial class PathNodesPanel : UserControl
    {
        public EditorScreen GameWorld;
        public Node CurrentNodeSelected;

        public PathNodesPanel(EditorScreen GameWorld)
        {
            InitializeComponent();
            this.GameWorld = GameWorld;
            this.Location = new Point(36, 0);
            InitializeData();
            InitializeNodeData();
        }

        public void InitializeData()
        {
            this.TypeComboBox.DataSource = Enum.GetValues(typeof(PathType));
            this.NodeTypeCombobox.DataSource = Enum.GetValues(typeof(NodeType));
            
            this.NodeLists.DataSource = null;
            this.NodeLists.DataSource = GameWorld.NodeLists;
            this.NodeLists.DisplayMember = "Name";

            
        }

        public void InitializeNodeData()
        {
            if (CurrentNodeSelected != null)
            {
                this.NodeInfo.Show();
                this.NodeTypeCombobox.SelectedItem = CurrentNodeSelected.Type;
                
            }
            else
            {
                this.NodeInfo.Hide();
            }
        }

        private void RemovePathButton_Click(object sender, EventArgs e)
        {
            if (this.NodeLists.SelectedItem != null)
            {
                this.GameWorld.NodeLists.RemoveAt(this.NodeLists.SelectedIndex);
                this.NodeLists.SelectedIndex = -1;
                this.InitializeData();
            }
        }

        private void AddPathButton_Click(object sender, EventArgs e)
        {
            int i = 1;
            foreach (PathNodeList pnl in GameWorld.NodeLists)
            {
                if (pnl.Name == "NodeList" + i)
                    i++;
            }

            PathNodeList pathNodeList = new PathNodeList();
            pathNodeList.Name = "NodeList" + i;

       
            this.GameWorld.NodeLists.Add(pathNodeList);
            InitializeData();
        }

        private void NodeLists_SelectedIndexChanged(object sender, EventArgs e)
        {
            GameWorld.CurrentTool = new Selection(GameWorld);
            CurrentNodeSelected = null;
            if (NodeLists.SelectedIndex != -1)
            {
                this.PathName.Text = GameWorld.NodeLists[NodeLists.SelectedIndex].Name;
                this.TypeComboBox.SelectedItem = GameWorld.NodeLists[NodeLists.SelectedIndex].Type;
            }
        }

        private void PathName_Enter(object sender, EventArgs e)
        {
            (Neon.world as EditorScreen).FocusedTextBox = sender as TextBox;
        }

        private void PathName_Leave(object sender, EventArgs e)
        {
            GameWorld.NodeLists[NodeLists.SelectedIndex].Name = (sender as TextBox).Text;
            (Neon.world as EditorScreen).FocusedTextBox = null;
            InitializeData();
        }

        private void TypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(NodeLists.SelectedIndex != -1 && (NodeLists.DataSource as List<PathNodeList>).Count > 0)
                GameWorld.NodeLists[NodeLists.SelectedIndex].Type = (PathType)Enum.Parse(typeof(PathType), (sender as ComboBox).SelectedItem.ToString());
        }

        private void ToggleDisplayAll_Click(object sender, EventArgs e)
        {
            GameWorld.DisplayAllPathNodeList = !GameWorld.DisplayAllPathNodeList;
        }

        private void AddPathNode_Click(object sender, EventArgs e)
        {
            GameWorld.CurrentTool = new AddNode(GameWorld.NodeLists[NodeLists.SelectedIndex], GameWorld);
        }

        private void SelectionButton_Click(object sender, EventArgs e)
        {
            GameWorld.CurrentTool = new SelectNode(GameWorld.NodeLists[NodeLists.SelectedIndex], GameWorld);
        }

        private void DeleteNode_Click(object sender, EventArgs e)
        {
            if (CurrentNodeSelected != null)
            {
                GameWorld.NodeLists[NodeLists.SelectedIndex].Nodes.Remove(CurrentNodeSelected);
                CurrentNodeSelected = null;
                InitializeNodeData();
            }
        }

        private void NodeTypeCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CurrentNodeSelected != null)
            {
                CurrentNodeSelected.Type = (NodeType)Enum.Parse(typeof(NodeType), (sender as ComboBox).SelectedItem.ToString());
            }
        }

        private void Align_Click(object sender, EventArgs e)
        {
            if (CurrentNodeSelected != null)
            {
                foreach (Node n in GameWorld.NodeLists[NodeLists.SelectedIndex].Nodes)
                {
                    n.Position.Y = CurrentNodeSelected.Position.Y;
                }
            }
        }
    }
}
