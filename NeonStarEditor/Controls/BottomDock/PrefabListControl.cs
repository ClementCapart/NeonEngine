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
using System.IO;
using Microsoft.Xna.Framework;

namespace NeonStarEditor
{
    public partial class PrefabListControl : UserControl
    {
        public EditorScreen GameWorld;

        public PrefabListControl()
            :base()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            if (GameWorld != null)
            {
                List<string> PrefabsList;
                PrefabsList = new List<string>(Directory.GetFiles("Prefabs"));
                for (int i = PrefabsList.Count - 1; i >= 0; i--)
                    PrefabsList[i] = Path.GetFileNameWithoutExtension(PrefabsList[i]);
                this.PrefabListBox.DataSource = PrefabsList;
            }
            base.OnLoad(e);
        }

        private void AddPrefabButton_Click(object sender, EventArgs e)
        {
            string path = @"Prefabs/" + PrefabListBox.Text + ".prefab";
            Stream stream = File.OpenRead(path);

            XDocument prefab = XDocument.Load(stream);
            XElement Prefab = prefab.Element("Prefab");
            XElement Ent = Prefab.Element("Entity");

            Entity entity = new Entity(this.GameWorld);
            entity.Name = Ent.Attribute("Name").Value;

            foreach (XElement Comp in Ent.Element("Components").Elements())
            {

                if (Comp.Name == "Transform")
                {
                    entity.transform.Position = Neon.utils.ParseVector2(Comp.Element("Properties").Element("Position").Attribute("Value").Value);
                    entity.transform.Rotation = float.Parse(Comp.Element("Properties").Element("Rotation").Attribute("Value").Value);
                    entity.transform.Scale = float.Parse(Comp.Element("Properties").Element("Scale").Attribute("Value").Value);
                }
                else
                {
                    string AssemblyName = Comp.Attribute("Type").Value.Split('.')[0];
                    string TypeName = Comp.Attribute("Type").Value.Split('.')[1];
                    Type t = Type.GetType(AssemblyName + "." + TypeName + ", " + AssemblyName);
                    if (t == null)
                        foreach (Type type in Neon.Scripts)
                            if (type.Name == TypeName)
                            {
                                t = type;
                                break;
                            }

                    NeonEngine.Component component = (NeonEngine.Component)Activator.CreateInstance(t, entity);
                    component.ID = int.Parse(Comp.Attribute("ID").Value);
                    foreach (XElement Property in Comp.Element("Properties").Elements())
                    {

                        PropertyInfo pi = t.GetProperty(Property.Name.ToString());

                        if (pi.PropertyType.IsSubclassOf(typeof(NeonEngine.Component)))
                            continue;
                        else if (pi.PropertyType.Equals(typeof(Vector2)))
                            pi.SetValue(component, Neon.utils.ParseVector2(Property.Attribute("Value").Value), null);
                        else if (pi.PropertyType.IsEnum)
                            pi.SetValue(component, Enum.Parse(pi.PropertyType, Property.Attribute("Value").Value), null);
                        else if (pi.PropertyType.Equals(typeof(Single)))
                            pi.SetValue(component, float.Parse(Property.Attribute("Value").Value), null);
                        else if (pi.PropertyType.Equals(typeof(bool)))
                            pi.SetValue(component, bool.Parse(Property.Attribute("Value").Value), null);
                        else if (pi.PropertyType.Equals(typeof(Int32)))
                            pi.SetValue(component, int.Parse(Property.Attribute("Value").Value), null);
                        else if (pi.PropertyType.Equals(typeof(String)))
                            pi.SetValue(component, Property.Attribute("Value").Value, null);
                    }

                    component.Init();

                    entity.AddComponent(component);
                }
            }
            foreach (XElement Comp in Ent.Element("Components").Elements())
            {
                NeonEngine.Component comp = entity.Components.First(c => c.ID == int.Parse(Comp.Attribute("ID").Value));

                if (Comp.Name == "Rigidbody" || Comp.Name == "Spritesheet" || Comp.Name == "Graphic" || Comp.Name == "Hitbox" || Comp.Name == "Mover")
                {
                    foreach (XElement Property in Comp.Element("Properties").Elements())
                    {
                        PropertyInfo pi = comp.GetType().GetProperty(Property.Name.ToString());

                        if (pi.PropertyType.IsSubclassOf(typeof(NeonEngine.Component)))
                        {
                            if (Property.Attribute("Value").Value != "None")
                            {
                                NeonEngine.Component Value = entity.Components.First(c => c.ID == int.Parse(Property.Attribute("Value").Value));
                                pi.SetValue(comp, Value, null);
                            }
                        }
                    }
                }
            }

            this.GameWorld.entityList.Add(entity);
            stream.Close();
        }
    }
}
