using System.Diagnostics;
using Microsoft.Xna.Framework;
using NeonEngine.Private;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Graphics;
using NeonEngine.Components.CollisionDetection;
using NeonEngine.Components.Audio;

namespace NeonEngine
{
    static public class DataManager
    {
        static public void SaveLevel(World CurrentWorld, string avatarEntity, string hudEntity)
        {
            Console.WriteLine("Save : Archiving older files...");

            if (Directory.Exists(@"../Data/Levels/" + CurrentWorld.LevelGroupName + "/" + CurrentWorld.LevelName + "/Old"))
                Directory.Delete(@"../Data/Levels/" + CurrentWorld.LevelGroupName + "/" + CurrentWorld.LevelName + "/Old", true);
            Directory.CreateDirectory(@"../Data/Levels/" + CurrentWorld.LevelGroupName + "/" + CurrentWorld.LevelName + "/Old");
            try
            {
                foreach (string s in Directory.GetFiles(@"../Data/Levels/" + CurrentWorld.LevelGroupName + "/" + CurrentWorld.LevelName + "/"))
                {
                    if(!s.EndsWith("Lock.xml"))
                        File.Move(s, @"../Data/Levels/" + CurrentWorld.LevelGroupName + "/" + CurrentWorld.LevelName + "/Old/" + Path.GetFileName(s));
                }
            }
            catch { }
            
            
            Console.WriteLine("Save : Level save started...");

            Level currentLevel = CurrentWorld.LevelMap;

            XDocument levelInfo = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
            XElement levelInfos = new XElement("LevelInfos");

            if (CurrentWorld.NodeLists.Count > 0)
            {
                XElement pathNodeLists = new XElement("PathNodeLists");

                foreach (PathNodeList pnl in CurrentWorld.NodeLists)
                {
                    XElement nodeList = new XElement("PathNodeList", new XAttribute("Name", pnl.Name), new XAttribute("Type", pnl.Type.ToString()));

                    foreach (Node n in pnl.Nodes)
                    {
                        XElement node;
                        if (n.Type == NodeType.DelayedMove)
                            node = new XElement("Node", new XAttribute("Type", n.Type), new XAttribute("Index", n.index), new XAttribute("Position", Neon.Utils.Vector2ToString(n.Position)), new XAttribute("Delay", n.NodeDelay.ToString("G", CultureInfo.InvariantCulture)));
                        else
                            node = new XElement("Node", new XAttribute("Type", n.Type), new XAttribute("Index", n.index), new XAttribute("Position", Neon.Utils.Vector2ToString(n.Position)));
                        nodeList.Add(node);
                    }
                    pathNodeLists.Add(nodeList);
                }

                levelInfos.Add(pathNodeLists);
            }

            if (CurrentWorld.SpawnPoints.Count > 0)
            {
                XElement spawnPointList = new XElement("SpawnPointList");

                foreach (SpawnPoint sp in CurrentWorld.SpawnPoints)
                {
                    XElement spawnPoint = new XElement("SpawnPoint", new XAttribute("Index", sp.Index), new XAttribute("Position", Neon.Utils.Vector2ToString(sp.Position)), new XAttribute("Side", sp.Side.ToString()));
                    spawnPointList.Add(spawnPoint);
                }

                levelInfos.Add(spawnPointList);
            }

            levelInfo.Add(levelInfos);
            levelInfo.Save(@"../Data/Levels/" + CurrentWorld.LevelGroupName + "/" + CurrentWorld.LevelName + "/" + CurrentWorld.LevelName + "_Info.xml");

            Console.WriteLine("Save : Level infos saved succesfully !");

            Dictionary<string, List<Entity>> layerList = new Dictionary<string, List<Entity>>();

            if (CurrentWorld.Entities.Count > 0)
            {

                foreach (Entity entity in CurrentWorld.Entities)
                {
                    if (entity.Name == avatarEntity || entity.Name == hudEntity || entity.Layer == "Lock" || !entity.HasToBeSaved)
                        continue;

                    if (layerList.ContainsKey(entity.Layer))
                        layerList[entity.Layer].Add(entity);
                    else
                    {
                        layerList.Add(entity.Layer, new List<Entity>());
                        layerList[entity.Layer].Add(entity);
                    }
                }
            }

            Console.WriteLine("Save : " + layerList.Count + " layer(s) to save...");

            if (layerList.Count > 0)
            {
                foreach (KeyValuePair<string, List<Entity>> kvp in layerList)
                {
                    SaveLayer(CurrentWorld, kvp.Key, kvp.Value);
                }
            }
            else
            {
                Console.WriteLine("Save : No entity to save !");
                
            }

            Console.WriteLine("Save : Level saved succesfully !");
            Console.WriteLine("");
        }

        public static void SaveLayer(World currentWorld, string layerName, List<Entity> entities = null)
        {
            Console.WriteLine("Save : Layer '" + layerName + "' save started...");

            if(entities == null)
            {
                entities = new List<Entity>();
                foreach(Entity e in currentWorld.Entities)
                    if(e.Layer == layerName && e.HasToBeSaved) entities.Add(e);

                if (entities.Count == 0)
                    return;
            }

            XDocument document = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
            XElement content = new XElement("XnaContent");
            XElement level = new XElement("Level", new XAttribute("Layer", layerName));

            XElement Entities = new XElement("Entities");

            foreach (Entity e in entities)
            {
                XElement Entity = new XElement("Entity", new XAttribute("Name", e.Name), new XAttribute("Layer", e.Layer));
                XElement Components = new XElement("Components");
                foreach (Component c in e.Components)
                {
                    if (c.HasToBeSaved)
                    {
                        if (c.GetType().Equals(typeof(Hitbox)) && (c as Hitbox).Type == HitboxType.Hit)
                            continue;

                        Components.Add(SaveComponentParameters(c));
                    }
                }
                Entity.Add(Components);
                Entities.Add(Entity);
            }

            level.Add(Entities);

            content.Add(level);
            document.Add(content);

            if (layerName != "")
                document.Save(@"../Data/Levels/" + currentWorld.LevelGroupName + "/" + currentWorld.LevelName + "/" + currentWorld.LevelName + "_" + layerName + ".xml");
            else
                document.Save(@"../Data/Levels/" + currentWorld.LevelGroupName + "/" + currentWorld.LevelName + "/" + currentWorld.LevelName + ".xml");

            Console.WriteLine("Save : Layer '" + layerName + "' saved succesfully !");
        }

        static public void LoadLevel(string FilePath, World GameWorld)
        {
            Console.WriteLine("LoadLevel() to implement.");
        }

        static public void LoadLevelInfo(string groupName, string levelName, World GameWorld)
        {
            if (!File.Exists(@"../Data/Levels/" + groupName + "/" + levelName + "/" + levelName + "_Info.xml"))
            {
                Console.WriteLine("Warning : Level " + levelName + " in group " + groupName + " doesn't exist !");
                Console.WriteLine("");
                return;
            }

            Console.WriteLine("Level information loading...");
            Console.WriteLine("");

            Stream stream = File.OpenRead(@"../Data/Levels/" + groupName + "/" + levelName + "/"  + levelName + "_Info.xml");

            XDocument document = XDocument.Load(stream);

            XElement PathNodeLists = document.Element("LevelInfos").Element("PathNodeLists");

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

                    GameWorld.NodeLists.Add(pnl);
                }
            }

            XElement SpawnPointList = document.Element("LevelInfos").Element("SpawnPointList");

            if (SpawnPointList != null)
            {
                foreach (XElement spawnPoint in SpawnPointList.Elements("SpawnPoint"))
                {
                    SpawnPoint sp = new SpawnPoint();
                    sp.Index = int.Parse(spawnPoint.Attribute("Index").Value);
                    sp.Position = Neon.Utils.ParseVector2(spawnPoint.Attribute("Position").Value);
                    sp.Side = (Side)Enum.Parse(typeof(Side), spawnPoint.Attribute("Side").Value);

                    GameWorld.SpawnPoints.Add(sp);
                }
            }

            stream.Close();
        }

        static public void SavePrefab(Entity entity, string FilePath)
        {
            if (entity != null)
            {
                XDocument prefab = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));

                prefab.Add(SavePrefab(entity));

                prefab.Save(FilePath);
            }
        }

        static public XElement SavePrefab(Entity entity)
        {
            if (entity != null)
            {
                XElement Prefab = new XElement("Prefab");

                XElement Entity = new XElement("Entity", new XAttribute("Name", entity.Name), new XAttribute("Layer", entity.Layer));
                XElement Components = new XElement("Components");
                foreach (Component c in entity.Components)
                {
                    XElement Component = new XElement(c.Name, new XAttribute("Type", c.GetType().ToString()), new XAttribute("ID", c.ID.ToString()));
                    XElement Properties = new XElement("Properties");
                    foreach (PropertyInfo pi in c.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                    {
                        if (pi.PropertyType.Equals(typeof(Effect)))
                            continue;
                        if (pi.PropertyType.IsSubclassOf(typeof(Component)))
                        {
                            Component comp = (Component)pi.GetValue(c, null);
                            XElement Property = new XElement(pi.Name, new XAttribute("Value", comp != null ? comp.ID.ToString() : "None"));
                            Properties.Add(Property);
                        }
                        else if (pi.Name == "Font")
                        {
                            XElement Property = new XElement(pi.Name, new XAttribute("Value", TextManager.FontList.Where(kvp => kvp.Value == (SpriteFont)pi.GetValue(c, null)).First().Key));
                            Properties.Add(Property);

                        }
                        else if (pi.Name == "Spritesheets")
                        {
                            XElement Property = new XElement(pi.Name);
                            Dictionary<string, SpriteSheetInfo> propertyDictionary = (Dictionary<string, SpriteSheetInfo>)pi.GetValue(c, null);
                            foreach (KeyValuePair<string, SpriteSheetInfo> kvp in propertyDictionary)
                            {
                                XElement Animation = new XElement("Animation",
                                                                    new XAttribute("Name", kvp.Key),
                                                                    new XAttribute("SpritesheetTag", AssetManager.GetSpritesheetTag(kvp.Value))
                                                                    );

                                Property.Add(Animation);
                            }

                            Properties.Add(Property);
                        }
                        else if (pi.PropertyType.Equals(typeof(PathNodeList)))
                        {
                            PathNodeList pnl = (pi.GetValue(c, null) as PathNodeList);
                            XElement Property;
                            if (pnl != null)
                            {
                                Property = new XElement(pi.Name, new XAttribute("Value", pnl.Name));
                                Properties.Add(Property);
                            }
                        }
                        else if (pi.PropertyType.Equals(typeof(Vector2)))
                        {
                            XElement Property = null;
                            Property = new XElement(pi.Name, new XAttribute("Value", Neon.Utils.Vector2ToString((Vector2)pi.GetValue(c, null))));
                            Properties.Add(Property);
                        }
                        else
                        {
                            XElement Property = null;
                            if (pi.PropertyType == typeof(Single))
                            {
                                Property = new XElement(pi.Name, new XAttribute("Value", ((float)pi.GetValue(c, null)).ToString("G", CultureInfo.InvariantCulture)));
                            }
                            else
                            {
                                Property = new XElement(pi.Name, new XAttribute("Value", pi.GetValue(c, null).ToString()));
                            }
                            Properties.Add(Property);
                        }
                    }

                    Component.Add(Properties);
                    Components.Add(Component);
                }
                Entity.Add(Components);

                Prefab.Add(Entity);
                return Prefab;
            }

            return null;
        }

        static public Entity LoadPrefab(string filePath, World gameWorld)
        {
            Stream stream = File.OpenRead(filePath);
            XDocument prefab = XDocument.Load(stream);
            XElement Prefab = prefab.Element("Prefab");
            Entity entity = LoadPrefab(Prefab, gameWorld);
            stream.Close();
            return entity;
        }

        static public Entity LoadPrefab(XElement prefab, World gameWorld)
        {
            XElement ent = prefab.Element("Entity");

            Entity entity = new Entity(gameWorld);
            if (ent != null)
            {
                entity.Name = ent.Attribute("Name").Value;
                if(ent.Attribute("Layer") != null)
                    entity.Layer = ent.Attribute("Layer").Value;
            }

            foreach (XElement Comp in ent.Element("Components").Elements())
            {

                if (Comp.Name == "Transform")
                {
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
                        if (pi == null) continue;
                        if (pi.PropertyType.IsSubclassOf(typeof(Component)))
                            continue;
                        else if (pi.PropertyType.Equals(typeof(PathNodeList)))
                        {
                            List<PathNodeList> pnl = gameWorld.NodeLists.Where(nl => nl.Name == Property.Attribute("Value").Value).ToList<PathNodeList>();
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
                            pi.SetValue(component, Single.Parse(Property.Attribute("Value").Value, NumberStyles.Any, CultureInfo.InvariantCulture), null);
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

                    component.Init();

                    entity.AddComponent(component);
                }
            }
            foreach (XElement Comp in ent.Element("Components").Elements())
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
                    {
                        if (Property.Attribute("Value").Value != "None")
                        {
                            Component Value = entity.Components.First(c => c.ID == int.Parse(Property.Attribute("Value").Value));
                            pi.SetValue(comp, Value, null);
                        }
                    }
                }
            }

            foreach (Component c in entity.Components)
                c.Init();

            gameWorld.AddEntity(entity);
            return entity;
        }

        static public XElement SaveComponentParameters(Component c)
        {
            XElement Component = new XElement(c.Name, new XAttribute("Type", c.GetType().ToString()), new XAttribute("ID", c.ID.ToString()));
            XElement Properties = new XElement("Properties");
            foreach (PropertyInfo pi in c.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (pi.PropertyType.IsSubclassOf(typeof(Component)))
                {
                    Component comp = (Component)pi.GetValue(c, null);
                    XElement Property = new XElement(pi.Name, new XAttribute("Value", comp != null ? comp.ID.ToString() : "None"));
                    Properties.Add(Property);
                }
                else if (pi.Name == "InitialPosition" || pi.Name == "Position")
                {
                    if (pi.Name == "InitialPosition")
                    {
                        Vector2 value = (Vector2)pi.GetValue(c, null);
                        value = new Vector2((float)Math.Round(value.X), (float)Math.Round(value.Y));
                        XElement Property = new XElement(pi.Name, new XAttribute("Value", value));
                        Properties.Add(Property);
                    }
                }
                else if (pi.Name == "Font")
                {
                    XElement Property = new XElement(pi.Name, new XAttribute("Value", TextManager.FontList.Where(kvp2 => kvp2.Value == (SpriteFont)pi.GetValue(c, null)).First().Key));
                    Properties.Add(Property);

                }
                else if (pi.Name == "Spritesheets")
                {
                    XElement Property = new XElement(pi.Name);
                    Dictionary<string, SpriteSheetInfo> propertyDictionary = (Dictionary<string, SpriteSheetInfo>)pi.GetValue(c, null);
                    foreach (KeyValuePair<string, SpriteSheetInfo> kvp2 in propertyDictionary)
                    {
                        if (kvp2.Value != null && kvp2.Value.Frames != null)
                        {
                            XElement Animation = new XElement("Animation",
                                                                new XAttribute("Name", kvp2.Key),
                                                                new XAttribute("SpritesheetTag", AssetManager.GetSpritesheetTag(kvp2.Value))
                                                                );
                            Property.Add(Animation);
                        }
                    }

                    Properties.Add(Property);
                }
                else if (pi.PropertyType.Equals(typeof(List<SoundInstanceInfo>)))
                {
                    XElement Property = new XElement(pi.Name);
                    List<SoundInstanceInfo> propertyList = (List<SoundInstanceInfo>)pi.GetValue(c, null);
                    foreach (SoundInstanceInfo sii in propertyList)
                    {
                        if(sii.Sound != null)
                        {
                            XElement SoundInstance = new XElement("SoundInstanceInfo", new XAttribute("Name", sii.Name),
                                                                                       new XAttribute("Sound", sii.Sound.Name),
                                                                                       new XAttribute("3DSound", sii.Is3DSound.ToString()),
                                                                                       new XAttribute("Volume", sii.Volume.ToString()),
                                                                                       new XAttribute("Pitch", sii.Pitch.ToString()),
                                                                                       new XAttribute("Offset", Neon.Utils.Vector2ToString(sii.Offset)));
                            Property.Add(SoundInstance);
                        }
                        
                    }
                }
                else if (pi.PropertyType.Equals(typeof(PathNodeList)))
                {
                    PathNodeList pnl = (pi.GetValue(c, null) as PathNodeList);
                    XElement Property;
                    if (pnl != null)
                    {
                        Property = new XElement(pi.Name, new XAttribute("Value", pnl.Name));
                        Properties.Add(Property);
                    }

                }
                else if (pi.PropertyType.Equals(typeof(Vector2)))
                {
                    XElement Property = null;
                    Property = new XElement(pi.Name, new XAttribute("Value", Neon.Utils.Vector2ToString((Vector2)pi.GetValue(c, null))));
                    Properties.Add(Property);
                }
                else if (pi.Name != "CurrentEffect")
                {
                    XElement Property = null;
                    if (pi.PropertyType == typeof(Single))
                    {
                        Property = new XElement(pi.Name, new XAttribute("Value", ((float)pi.GetValue(c, null)).ToString("G", CultureInfo.InvariantCulture)));
                    }
                    else
                    {
                        Property = new XElement(pi.Name, new XAttribute("Value", pi.GetValue(c, null).ToString()));
                    }
                    Properties.Add(Property);
                }
            }

            Component.Add(Properties);

            return Component;
        }

        static public Component LoadComponent(XElement Comp, Entity entityOwner)
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
                return null;

            Component component = (Component)Activator.CreateInstance(t, entityOwner);
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
                    List<PathNodeList> pnl = entityOwner.GameWorld.NodeLists.Where(nl => nl.Name == Property.Attribute("Value").Value).ToList<PathNodeList>();
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

            entityOwner.AddComponent(component);
            component.Init();

            return component;
        }

        static public void LoadComponentParameters(XElement Comp, Component c)
        {
            if (Comp.Name == "Transform")
            {
                (c as Transform).Position = Neon.Utils.ParseVector2(Comp.Element("Properties").Element("Position").Attribute("Value").Value);
                (c as Transform).Rotation = float.Parse(Comp.Element("Properties").Element("Rotation").Attribute("Value").Value, CultureInfo.InvariantCulture);
                (c as Transform).Scale = float.Parse(Comp.Element("Properties").Element("Scale").Attribute("Value").Value, CultureInfo.InvariantCulture);
            }
            else
            {
                c.ID = int.Parse(Comp.Attribute("ID").Value);
                foreach (XElement Property in Comp.Element("Properties").Elements())
                {
                    PropertyInfo pi = c.GetType().GetProperty(Property.Name.ToString());

                    if (pi.PropertyType.IsSubclassOf(typeof(Component)))
                        continue;
                    else if (pi.PropertyType.Equals(typeof(Vector2)))
                        pi.SetValue(c, Neon.Utils.ParseVector2(Property.Attribute("Value").Value), null);
                    else if (pi.PropertyType.IsEnum)
                        pi.SetValue(c, Enum.Parse(pi.PropertyType, Property.Attribute("Value").Value), null);
                    else if (pi.PropertyType.Equals(typeof(Single)))
                        pi.SetValue(c, Single.Parse(Property.Attribute("Value").Value, NumberStyles.Any, CultureInfo.InvariantCulture), null);
                    else if (pi.PropertyType.Equals(typeof(bool)))
                        pi.SetValue(c, bool.Parse(Property.Attribute("Value").Value), null);
                    else if (pi.PropertyType.Equals(typeof(Int32)))
                        pi.SetValue(c, int.Parse(Property.Attribute("Value").Value), null);
                    else if (pi.PropertyType.Equals(typeof(String)))
                        pi.SetValue(c, Property.Attribute("Value").Value, null);
                }

                c.Init();
            }
        }
    }
}
