using NeonEngine;
using NeonStarLibrary.Components.Private;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace NeonStarLibrary
{
    static public class CollectibleManager
    {
        static private List<Collectible> _collectibles = new List<Collectible>();

        static public void InitializeCollectibles(GameScreen gameWorld)
        {
            foreach(Entity e in gameWorld.Entities)
            {
                List<Collectible> collectibles = e.GetComponentsByInheritance<Collectible>();
                if (collectibles != null)
                {
                    foreach (Collectible c in collectibles)
                    {
                        bool alreadyInList = false;
                        for(int i = _collectibles.Count - 1; i >= 0; i --)
                        {
                            Collectible c2 = _collectibles[i];
                            if (c2.EntityName == c.EntityName && c2.GroupName == c.GroupName && c2.LevelName == c.LevelName)
                            {
                                c.State = c2.State;
                                c.Init();
                                _collectibles[i] = c;
                                alreadyInList = true;
                            } 
                        }
                        if (!alreadyInList)
                            _collectibles.Add(c);
                                               
                    }
                }
            }
        }

        static public void InitializeCollectiblesFromCheckpointData(XElement checkpointData)
        {
            ResetCollectibles();
            foreach (XElement c in checkpointData.Elements("Collectible"))
            {
                string entityName = c.Element("EntityName").Value;
                string levelName = c.Element("LevelName").Value;
                string groupName = c.Element("GroupName").Value;

                Collectible collectible = new Collectible(null, "TemplateCollectible");
                collectible.EntityName = entityName;
                collectible.GroupName = groupName;
                collectible.LevelName = levelName;
                collectible.State = CollectibleState.Used;
                _collectibles.Add(collectible);
            }
        }

        static public XElement SaveCollectiblesState()
        {
            XElement collectiblesState = new XElement("GatheredCollectibles");

            foreach (Collectible c in _collectibles)
            {
                if (c.State != CollectibleState.Available)
                {
                    XElement collectible = new XElement("Collectible");
                    XElement entityName = new XElement("EntityName", c.EntityName);
                    XElement levelName = new XElement("LevelName", c.LevelName);
                    XElement groupName = new XElement("GroupName", c.GroupName);

                    collectible.Add(entityName, levelName, groupName);

                    collectiblesState.Add(collectible);
                }
            }

            return collectiblesState;         
        }

        static public void ResetCollectibles()
        {
            _collectibles = new List<Collectible>();
        }
    }
}
