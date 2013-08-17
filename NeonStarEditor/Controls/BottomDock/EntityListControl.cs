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

        private void SaveAsPrefabButton_Click(object sender, EventArgs e)
        {
            if (GameWorld.SelectedEntity != null)
            {
                SavePrefabDialog.InitialDirectory = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Application.ExecutablePath), @"Prefabs");
                SavePrefabDialog.FileName = GameWorld.SelectedEntity.Name;
                if (SavePrefabDialog.ShowDialog() == DialogResult.OK)
                {
                    XDocument prefab = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
                    XElement Prefab = new XElement("Prefab");

                    XElement Entity = new XElement("Entity", new XAttribute("Name", GameWorld.SelectedEntity.Name));
                    XElement Components = new XElement("Components");
                    foreach (NeonEngine.Component c in GameWorld.SelectedEntity.Components)
                    {
                        XElement Component = new XElement(c.Name, new XAttribute("Type", c.GetType().ToString()), new XAttribute("ID", c.ID.ToString()));
                        XElement Properties = new XElement("Properties");
                        foreach (PropertyInfo pi in c.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                        {
                            if (pi.PropertyType.IsSubclassOf(typeof(NeonEngine.Component)))
                            {
                                NeonEngine.Component comp = (NeonEngine.Component)pi.GetValue(c, null);
                                XElement Property = new XElement(pi.Name, new XAttribute("Value", comp != null ? comp.ID.ToString() : "None"));
                                Properties.Add(Property);
                            }
                            else
                            {
                                XElement Property = new XElement(pi.Name, new XAttribute("Value", pi.GetValue(c, null).ToString()));
                                Properties.Add(Property);
                            }
                        }

                        Component.Add(Properties);
                        Components.Add(Component);
                    }
                    Entity.Add(Components);

                    Prefab.Add(Entity);
                    prefab.Add(Prefab);

                    prefab.Save(SavePrefabDialog.FileName);
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
