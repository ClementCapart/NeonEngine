using Microsoft.Xna.Framework;
using System.Linq;
using System.Xml.Linq;
using System.IO;
using System;
using System.Reflection;
using System.Globalization;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace NeonEngine
{
    public class Level
    {
        public Vector2 spawnPoint;
        public Rectangle bounds;
        public string Name = "";
        public string Group = "";

        public Level(string groupName, string levelName, World container, bool collideWorld)
        {
            if (!File.Exists(@"../Data/Levels/" + groupName + "/" + levelName + "/" + levelName + "_Info.xml"))
            {
                Console.WriteLine("Warning : Level " + levelName + " in group " + groupName + " doesn't exist !");
                Console.WriteLine("");
                return;
            }

            Name = levelName;
            Group = groupName;
            container.LevelGroupName = Group;
            container.LevelName = Name;

            Console.WriteLine("Level loading...");
            Console.WriteLine("");

            foreach (string filePath in Directory.GetFiles(@"../Data/Levels/" + groupName + "/" + levelName))
            {
                if (Path.GetFileNameWithoutExtension(filePath).EndsWith("Info"))
                    continue;

                Stream stream = File.OpenRead(filePath);

                XDocument document = XDocument.Load(stream);

                XElement XnaContent = document.Element("XnaContent");
                XElement Level = XnaContent.Element("Level");

                XElement LevelData = Level.Element("LevelData");
                XElement Geometry = Level.Element("Geometry");
                XElement WaterZones = Level.Element("WaterZones");
                XElement Entities = Level.Element("Entities");

                XElement PathNodeLists = Level.Element("PathNodeLists");

                if (PathNodeLists != null)
                {
                    foreach (XElement pathNodeList in PathNodeLists.Elements("PathNodeList"))
                    {
                        PathNodeList pnl = new PathNodeList();
                        pnl.Name = pathNodeList.Attribute("Name").Value;
                        pnl.Type = (PathType)Enum.Parse(typeof(PathType), pathNodeList.Attribute("Type").Value);

                        foreach (XElement node in pathNodeList.Elements("Node"))
                        {
                            Node n = new Node();
                            n.index = int.Parse(node.Attribute("Index").Value);
                            n.Type = (NodeType)Enum.Parse(typeof(NodeType), node.Attribute("Type").Value);
                            n.Position = Neon.Utils.ParseVector2(node.Attribute("Position").Value);

                            if (n.Type == NodeType.DelayedMove)
                                n.NodeDelay = float.Parse(node.Attribute("Delay").Value, CultureInfo.InvariantCulture);
                            pnl.Nodes.Add(n);
                        }

                        container.NodeLists.Add(pnl);
                    }
                }

                if (Entities != null)
                    EntityImport(Entities, container);

                if (WaterZones != null)
                    if (WaterZones != null)
                        foreach (XElement w in WaterZones.Elements("Water"))
                        {
                            Rectangle area = new Rectangle();
                            area.X = int.Parse(w.Attribute("X").Value);
                            area.Y = int.Parse(w.Attribute("Y").Value);
                            area.Width = int.Parse(w.Attribute("Width").Value);
                            area.Height = int.Parse(w.Attribute("Height").Value);
                            container.Waterzones.Add(new Water(container, area));
                        }

                stream.Close();
            }


            foreach (Entity e in container.Entities)
            {
                for (int i = e.Components.Count - 1; i >= 0; i-- )
                {
                    e.Components[i].Init();
                }
            }

            Console.WriteLine("");

            Console.WriteLine("Level loaded !");
            Console.WriteLine("");
        }

        public virtual void EntityImport(XElement Entities, World containerWorld)
        {
            foreach (XElement Ent in Entities.Elements("Entity"))
            {
                Entity entity = new Entity(containerWorld);
                entity.Name = Ent.Attribute("Name").Value;
                entity.Layer = Ent.Attribute("Layer").Value;

                foreach (XElement Comp in Ent.Element("Components").Elements())
                {
                    if (Comp.Name == "Transform")
                    {
                        entity.transform.AutoChangeInitialPosition = bool.Parse(Comp.Element("Properties").Element("AutoChangeInitialPosition").Attribute("Value").Value);
                        entity.transform.InitialPosition = Neon.Utils.ParseVector2(Comp.Element("Properties").Element("InitialPosition").Attribute("Value").Value);
                        entity.transform.Rotation = float.Parse(Comp.Element("Properties").Element("Rotation").Attribute("Value").Value, CultureInfo.InvariantCulture);
                        entity.transform.Scale = float.Parse(Comp.Element("Properties").Element("Scale").Attribute("Value").Value, CultureInfo.InvariantCulture);
                        entity.transform.Init();
                    }
                    else
                    {
                        string[] splitTypeName = Comp.Attribute("Type").Value.Split('.');
                        string AssemblyName = splitTypeName[0];
                        string TypeName = splitTypeName.Last();

                        Type t = Type.GetType(Comp.Attribute("Type").Value + ", " + AssemblyName);
                        if (t == null)
                            foreach (Type type in Neon.Scripts)
                                if (type.Name == TypeName)
                                {
                                    t = type;
                                    break;
                                }
                        if (t == null || t.IsAbstract)
                            continue;

                        Component component = (Component)Activator.CreateInstance(t, entity);
                        component.ID = int.Parse(Comp.Attribute("ID").Value);
                        foreach (XElement Property in Comp.Element("Properties").Elements())
                        {
                            PropertyInfo pi = t.GetProperty(Property.Name.ToString());
                            if (pi == null)
                                continue;

                            if (pi.PropertyType.IsSubclassOf(typeof(Component)))
                                continue;
                            else if (pi.PropertyType.Equals(typeof(PathNodeList)))
                            {
                                List<PathNodeList> pnl = containerWorld.NodeLists.Where(nl => nl.Name == Property.Attribute("Value").Value).ToList<PathNodeList>();
                                if (pnl.Count > 0)
                                    pi.SetValue(component, pnl.First(), null);
                            }
                            else if (pi.PropertyType.Equals(typeof(Vector2)))
                                pi.SetValue(component, Neon.Utils.ParseVector2(Property.Attribute("Value").Value), null);
                            else if (pi.PropertyType.Equals(typeof(SpriteFont)))
                                pi.SetValue(component, TextManager.FontList[Property.Attribute("Value").Value], null);
                            else if (pi.PropertyType.Equals(typeof(Color)))
                                pi.SetValue(component, Neon.Utils.ParseColor(Property.Attribute("Value").Value), null);
                            else if (pi.PropertyType.IsEnum)
                                pi.SetValue(component, Enum.Parse(pi.PropertyType, Property.Attribute("Value").Value), null);
                            else if (pi.PropertyType.Equals(typeof(Single)))
                                pi.SetValue(component, float.Parse(Property.Attribute("Value").Value, CultureInfo.InvariantCulture), null);
                            else if (pi.PropertyType.Equals(typeof(bool)))
                                pi.SetValue(component, bool.Parse(Property.Attribute("Value").Value), null);
                            else if (pi.PropertyType.Equals(typeof(Int32)))
                                pi.SetValue(component, int.Parse(Property.Attribute("Value").Value), null);
                            else if (pi.PropertyType.Equals(typeof(String)))
                                pi.SetValue(component, Property.Attribute("Value").Value, null);
                            else if (Property.Name == "Spritesheets")
                            {
                                Dictionary<string, SpriteSheetInfo> spritesheetList = new Dictionary<string, SpriteSheetInfo>();
                                foreach (XElement Animation in Property.Elements("Animation"))
                                {
                                    spritesheetList.Add(Animation.Attribute("Name").Value, AssetManager.GetSpriteSheet(Animation.Attribute("SpritesheetTag").Value));
                                }
                                pi.SetValue(component, spritesheetList, null);
                            }
                        }
                        entity.AddComponent(component);
                    }
                }

                foreach (XElement Comp in Ent.Element("Components").Elements())
                {
                    Component comp;
                    try
                    {
                        comp = entity.Components.First(c => c.ID == int.Parse(Comp.Attribute("ID").Value));
                    }
                    catch
                    {
                        continue;
                    }
                     

                    foreach (XElement Property in Comp.Element("Properties").Elements())
                    {
                        PropertyInfo pi = comp.GetType().GetProperty(Property.Name.ToString());
                        if (pi == null)
                            continue;

                        if (pi.PropertyType.IsSubclassOf(typeof(Component)))
                            if (Property.Attribute("Value").Value != "None")
                            {
                                Component Value = entity.Components.First(c => c.ID == int.Parse(Property.Attribute("Value").Value));
                                pi.SetValue(comp, Value, null);
                            }
                    }

                }
                containerWorld.AddEntity(entity);
            }
        }
    }
}
