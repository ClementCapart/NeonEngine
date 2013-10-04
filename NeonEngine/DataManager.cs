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

namespace NeonEngine
{
    static public class DataManager
    {
        static public void SaveLevel(World CurrentWorld, string FilePath)
        {
            Level currentLevel = CurrentWorld.levelMap;

            XDocument document = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
            XElement content = new XElement("XnaContent");
            XElement level = new XElement("Level");

            if (CurrentWorld.entities.Count > 0)
            {
                XElement Entities = new XElement("Entities");

                foreach (Entity e in CurrentWorld.entities)
                {
                    XElement Entity = new XElement("Entity", new XAttribute("Name", e.Name));
                    XElement Components = new XElement("Components");
                    foreach (Component c in e.Components)
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
                            else if(pi.Name == "Spritesheets")
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
                            else
                            {
                                XElement Property = null;
                                if(pi.PropertyType == typeof(Single))
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
                    Entities.Add(Entity);
                }

                level.Add(Entities);
            }

            content.Add(level);
            document.Add(content);
            document.Save(FilePath+ ".xml");
        }

        static public void LoadLevel(string FilePath, World GameWorld)
        {
            Console.WriteLine("LoadLevel() to implement.");
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

                XElement Entity = new XElement("Entity", new XAttribute("Name", entity.Name));
                XElement Components = new XElement("Components");
                foreach (Component c in entity.Components)
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

        static public void LoadPrefab(string filePath, World gameWorld)
        {
            Stream stream = File.OpenRead(filePath);
            XDocument prefab = XDocument.Load(stream);
            XElement Prefab = prefab.Element("Prefab");
            LoadPrefab(Prefab, gameWorld);
            stream.Close();
        }

        static public void LoadPrefab(XElement prefab, World gameWorld)
        {
            XElement ent = prefab.Element("Entity");

            Entity entity = new Entity(gameWorld);
            if (ent != null)
                entity.Name = ent.Attribute("Name").Value;

            foreach (XElement Comp in ent.Element("Components").Elements())
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

                    Component component = (Component)Activator.CreateInstance(t, entity);
                    component.ID = int.Parse(Comp.Attribute("ID").Value);
                    foreach (XElement Property in Comp.Element("Properties").Elements())
                    {

                        PropertyInfo pi = t.GetProperty(Property.Name.ToString());

                        if (pi.PropertyType.IsSubclassOf(typeof(Component)))
                            continue;
                        else if (pi.PropertyType.Equals(typeof(Vector2)))
                            pi.SetValue(component, Neon.utils.ParseVector2(Property.Attribute("Value").Value), null);
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
                Component comp = entity.Components.First(c => c.ID == int.Parse(Comp.Attribute("ID").Value));

                if (Comp.Name == "Rigidbody" || Comp.Name == "Spritesheet" || Comp.Name == "Graphic" || Comp.Name == "Hitbox" || Comp.Name == "Mover")
                {
                    foreach (XElement Property in Comp.Element("Properties").Elements())
                    {
                        PropertyInfo pi = comp.GetType().GetProperty(Property.Name.ToString());

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
            }

            gameWorld.AddEntity(entity);
        }

        static public XElement SaveComponentParameters(Component c)
        {
            XElement component = new XElement(c.Name, new XAttribute("Type", c.GetType().ToString()), new XAttribute("ID", c.ID.ToString()));
            XElement properties = new XElement("Properties");
            foreach (PropertyInfo pi in c.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (pi.PropertyType.IsSubclassOf(typeof(Component)))
                {
                    Component comp = (Component)pi.GetValue(c, null);
                    XElement property = new XElement(pi.Name, new XAttribute("Value", comp != null ? comp.ID.ToString() : "None"));
                    properties.Add(property);
                }
                else
                {

                    XElement property = null;
                    if (pi.PropertyType == typeof(Single))
                    {
                        property = new XElement(pi.Name, new XAttribute("Value", ((float)pi.GetValue(c, null)).ToString("G", CultureInfo.InvariantCulture)));
                    }
                    else
                    {
                        property = new XElement(pi.Name, new XAttribute("Value", pi.GetValue(c, null).ToString()));
                    }
                    properties.Add(property);
                }
            }
            component.Add(properties);

            return component;
        }

        static public void LoadComponentParameters(XElement Comp, Component c)
        {
            if (Comp.Name == "Transform")
            {
                (c as Transform).Position = Neon.utils.ParseVector2(Comp.Element("Properties").Element("Position").Attribute("Value").Value);
                (c as Transform).Rotation = float.Parse(Comp.Element("Properties").Element("Rotation").Attribute("Value").Value);
                (c as Transform).Scale = float.Parse(Comp.Element("Properties").Element("Scale").Attribute("Value").Value);
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
                        pi.SetValue(c, Neon.utils.ParseVector2(Property.Attribute("Value").Value), null);
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
