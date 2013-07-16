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
    public partial class EntityListControl : UserControl
    {
        public EditorScreen GameWorld;

        public EntityListControl()
            :base()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            this.GameWorld = (this.Parent as BottomDock).GameWorld;
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
            if (this.EntityListBox.SelectedValue != null)
            {
                (this.EntityListBox.SelectedValue as Entity).Destroy();
                Console.WriteLine((this.EntityListBox.SelectedValue as Entity).Name + " has been destroyed.\n");
            }
            else
                Console.WriteLine("No entity selected");

        }

        private void AddEntityButton_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Add : Entity");
            GameWorld.entityList.Add(new Entity(GameWorld));
        }
    }
}
