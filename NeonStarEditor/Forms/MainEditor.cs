using Microsoft.Xna.Framework;
using NeonEngine;
using NeonEngine.Private;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Linq;

namespace NeonStarEditor
{
    public partial class MainEditor : Form
    {
        public EditorScreen currentWorld;

        public MainEditor(EditorScreen currentWorld)
        {
            Application.CurrentCulture = CultureInfo.InvariantCulture;
            this.currentWorld = currentWorld;
            InitializeComponent();
                        
        }

        protected override void OnLoad(EventArgs e)
        {
            Screen screen = Screen.FromPoint(this.Location);
            this.Location = new System.Drawing.Point(screen.WorkingArea.Right - this.Width, screen.WorkingArea.Top);
            LoadPrefabsList();
            base.OnLoad(e);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
        }

        
        private void LoadPrefabsList()
        {
            List<string> PrefabsList;
            PrefabsList = new List<string>(Directory.GetFiles("Prefabs"));
            for (int i = PrefabsList.Count - 1; i >= 0; i--)
                PrefabsList[i] = Path.GetFileNameWithoutExtension(PrefabsList[i]);
            this.PrefabList.DataSource = PrefabsList;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
        }

        private void CreateRectangle_Click(object sender, EventArgs e)
        {
            currentWorld.CurrentTool = new CreateRectangle(currentWorld);
        }
        
        private void Selection_Click(object sender, EventArgs e)
        {
            currentWorld.CurrentTool = new Selection(currentWorld);
        }

        private void PauseButton_CheckedChanged(object sender, EventArgs e)
        {
            if (PauseButton.Checked)
            {
                PlayButton.Checked = false;
                currentWorld.Pause = true;
            }
        }

        private void PlayButton_CheckedChanged(object sender, EventArgs e)
        {
            if (PlayButton.Checked)
            {
                PauseButton.Checked = false;
                currentWorld.Pause = false;
            }
        }

        private void SaveCurrentMap_Click(object sender, EventArgs e)
        {
            SaveLevel();
        }

        private void SaveLevel()
        {
            Level currentLevel = currentWorld.levelMap;

            XDocument document = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
            XElement content = new XElement("XnaContent");
            XElement level = new XElement("Level");

            if(currentWorld.entities.Count > 0)
            {
                XElement Entities = new XElement("Entities");

                foreach(Entity e in currentWorld.entities)
                {
                    XElement Entity = new XElement("Entity", new XAttribute("Name", e.Name));
                    XElement Components = new XElement("Components");
                    foreach(NeonEngine.Component c in e.Components)
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
                    Entities.Add(Entity);
                }

                level.Add(Entities);
            }
            

            content.Add(level);
            document.Add(content);
            document.Save(currentWorld.levelFilePath+".xml");
        }

        private int ND(float value)
        {
            return (int)Math.Round(value);
        }

        private void AddEntity_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Add : Entity");
            currentWorld.entityList.Add(new Entity(currentWorld));
        }

        public void SetPause()
        {
            PauseButton.Checked = true;
            PlayButton.Checked = false;

            currentWorld.Pause = true;
        }

        public void UnsetPause()
        {
            PauseButton.Checked = false;
            PlayButton.Checked = true;

            currentWorld.Pause = false;
        }

        private void ReloadButton_Click(object sender, EventArgs e)
        {
            this.currentWorld.ChangeScreen(new EditorScreen(this.currentWorld.game, currentWorld.graphics));
            this.Close();
        }

        private void StateCreation_Click(object sender, EventArgs e)
        {
            StateCreation sc = new StateCreation();
            sc.Show();
        }

        private void ReloadScript_Click(object sender, EventArgs e)
        {
            Neon.NeonScripting.CompileScripts();
            if (Neon.Scripts != null)          
                foreach(Entity ent in currentWorld.entities)
                    for(int i = ent.Components.Count - 1; i >= 0; i--)
                    {
                        ScriptComponent sc = ent.Components[i] is ScriptComponent ? (ScriptComponent)ent.Components[i] : null;
                        if (sc != null)
                        {
                            sc.Remove();
                            foreach (Type t in Neon.Scripts)
                                if (sc.GetType().Name == t.Name)
                                    ent.AddComponent((NeonEngine.Component)Activator.CreateInstance(t, ent));
                        }
                    }

            List<Type> Components = new List<Type>(Neon.utils.GetTypesInNamespace(Assembly.GetAssembly(typeof(Neon)), "NeonEngine").Where(t => t.IsSubclassOf(typeof(NeonEngine.Component)) && !(t.IsAbstract)));
            if(Neon.Scripts != null)
                Components.AddRange(Neon.Scripts);
            Components.Add(typeof(ScriptComponent));
            currentWorld.RightDockControl.InspectorControl.ComponentList.DataSource = Components;
            currentWorld.RightDockControl.InspectorControl.ComponentList.DisplayMember = "Name";
        }

        private void SavePrefab_Click(object sender, EventArgs e)
        {
            if (currentWorld.SelectedEntity != null)
            {
                SavePrefabDialog.InitialDirectory = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Application.ExecutablePath), @"Prefabs");
                SavePrefabDialog.FileName = currentWorld.SelectedEntity.Name;
                if (SavePrefabDialog.ShowDialog() == DialogResult.OK)
                {
                    XDocument prefab = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
                    XElement Prefab = new XElement("Prefab");

                    XElement Entity = new XElement("Entity", new XAttribute("Name", currentWorld.SelectedEntity.Name));
                    XElement Components = new XElement("Components");
                    foreach (NeonEngine.Component c in currentWorld.SelectedEntity.Components)
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
                    LoadPrefabsList();
                }
            }
        }

        private void AddPrefab_Click(object sender, EventArgs e)
        {
            string path = @"Prefabs/" + PrefabList.Text + ".prefab";
            Stream stream = File.OpenRead(path);
            
            XDocument prefab = XDocument.Load(stream);
            XElement Prefab = prefab.Element("Prefab");
            XElement Ent = Prefab.Element("Entity");

            Entity entity = new Entity(this.currentWorld);
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

            this.currentWorld.entityList.Add(entity);
            stream.Close();
        }
    }
}
