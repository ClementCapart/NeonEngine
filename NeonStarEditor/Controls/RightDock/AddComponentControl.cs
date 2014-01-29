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
using NeonEngine.Private;

namespace NeonStarEditor
{
    public partial class AddComponentControl : UserControl
    {
        public EditorScreen GameWorld;

        public AddComponentControl(EditorScreen gameWorld)
        {
            GameWorld = gameWorld;
            
            InitializeComponent();

            this.Location = new Point((int)Neon.HalfScreen.X - this.Width / 2, (int)Neon.HalfScreen.Y - this.Height / 2);
            InitializeData();
        }

        public void InitializeData()
        {
            if (GameWorld != null)
            {
                List<Type> Components = new List<Type>(Neon.Utils.GetTypesInNamespace(Assembly.GetAssembly(typeof(Neon)), "NeonEngine").Where(t => t.IsSubclassOf(typeof(NeonEngine.Component)) && !(t.IsAbstract) && !t.Namespace.EndsWith("Private")));
                if (Neon.Scripts != null)
                    Components.AddRange(Neon.Scripts);
                Components.Add(typeof(ScriptComponent));
                Components.AddRange(Neon.Utils.GetTypesInNamespace(Assembly.GetAssembly(typeof(NeonStarLibrary.GameScreen)), "NeonStarLibrary").Where(t => t.IsSubclassOf(typeof(NeonEngine.Component)) && !(t.IsAbstract) && !t.Namespace.EndsWith("Private")));
                Components = Components.OrderBy(c => c.Name).ToList();

                Dictionary<string, List<Type>> nodeNames = new Dictionary<string, List<Type>>();

                foreach (Type t in Components)
                {
                    string lastNamespacePart = t.Namespace.Split('.').Last();
                    if (lastNamespacePart == "NeonStarLibrary" || lastNamespacePart == "NeonEngine" || lastNamespacePart == "Private")
                        continue;

                    

                    if (!nodeNames.ContainsKey(lastNamespacePart))
                        nodeNames.Add(lastNamespacePart, new List<Type>());

                    nodeNames[lastNamespacePart].Add(t);
                }

                nodeNames = nodeNames.OrderBy(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp2 => kvp2.Value);

                foreach (KeyValuePair<string, List<Type>> kvp in nodeNames)
                {
                    TreeNode pn = new TreeNode();
                    pn.Name = kvp.Key;
                    pn.Text = kvp.Key;

                    foreach (Type type in kvp.Value)
                    {
                        TreeNode n = new TreeNode();
                        n.Name = type.Name;
                        n.Text = type.Name;
                        n.Tag = type;

                        pn.Nodes.Add(n);
                    }

                    ComponentList.Nodes.Add(pn);
                }
            }
        }

        private void ClosePanel_Click(object sender, EventArgs e)
        {
            GameWorld.ToggleAddComponentPanel();
        }

        private void AddComponent_Click(object sender, EventArgs e)
        {
            if (GameWorld.SelectedEntity != null && ComponentList.SelectedNode != null && ComponentList.SelectedNode.Parent != null)
            {
                Entity CurrentEntity = GameWorld.SelectedEntity;

                NeonEngine.Component c = (NeonEngine.Component)Activator.CreateInstance((Type)ComponentList.SelectedNode.Tag, CurrentEntity);
                CurrentEntity.AddComponent(c);

                if (c.RequiredComponents.Length > 0)
                    AddRequiredComponents(CurrentEntity, c.RequiredComponents);

                c.Init();
                c.ID = CurrentEntity.GetLastID();
                //ActionManager.SaveAction(ActionType.AddComponent, new object[2] { c, this });
                GameWorld.RightDockControl.InspectorControl.InstantiateProperties(CurrentEntity);
                
            }
        }

        private void AddRequiredComponents(Entity entity, Type[] RequiredComponents)
        {
            foreach (Type t in RequiredComponents)
            {
                NeonEngine.Component comp = entity.GetComponentByTypeName(t.Name);

                if (comp != null)
                    continue;

                NeonEngine.Component c = (NeonEngine.Component)Activator.CreateInstance(t, entity);
                entity.AddComponent(c);

                if(c.RequiredComponents.Length > 0)
                    AddRequiredComponents(entity, c.RequiredComponents);

                c.Init();
                c.ID = entity.GetLastID();
            }
        }
    }
}
