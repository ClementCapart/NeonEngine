using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NeonEngine;
using NeonEngine.Components.Graphics2D;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace NeonStarLibrary
{
    public class MapScreen : World
    {
        public GameScreen CurrentGameScreen;

        private Dictionary<string, List<string>> _discoveredMap;

        private float _cameraSpeed = 250.0f;

        private List<Graphic> _currentMapRooms;

        private Entity _mapEntity;
        private float k = 0.2f;
        private float kcube = 0.3f;

        public MapScreen(Game game)
            :base(game)
        {
            this.ScreenEffect = AssetManager.GetEffect("CubicLens");
            ScreenEffect.Parameters["k"].SetValue(k);
            ScreenEffect.Parameters["kcube"].SetValue(kcube);
            Camera.Position = Vector2.Zero;
        }

        public override void Update(GameTime gameTime)
        {

            if(Neon.Input.Pressed(Microsoft.Xna.Framework.Input.Buttons.Back))
            {
                Neon.World = CurrentGameScreen;
            }

            if (Neon.Input.Check(NeonStarInput.MoveLeft) || Neon.Input.Check(NeonStarInput.CameraLeft))
            {
                Camera.Position -= new Vector2(_cameraSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
            }
            else if (Neon.Input.Check(NeonStarInput.MoveRight) || Neon.Input.Check(NeonStarInput.CameraRight))
            {
                Camera.Position += new Vector2(_cameraSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
            }

            if (Neon.Input.Check(NeonStarInput.MoveUp) || Neon.Input.Check(NeonStarInput.CameraUp))
            {
                Camera.Position -= new Vector2(0, _cameraSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
            else if (Neon.Input.Check(NeonStarInput.MoveDown) || Neon.Input.Check(NeonStarInput.CameraDown))
            {
                Camera.Position += new Vector2(0, _cameraSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
            base.Update(gameTime);
        }

        public void InitializeMapData(XElement saveStatus = null)
        {
            if(_mapEntity != null)
                _mapEntity.Destroy();
            _mapEntity = null;
            _discoveredMap = new Dictionary<string, List<string>>();

            if (saveStatus != null)
            {
                XElement mapStatus = saveStatus.Element("MapStatus");
                if (mapStatus != null)
                {
                    foreach (XElement groupDiscovered in mapStatus.Elements("Group"))
                    {
                        _discoveredMap.Add(groupDiscovered.Attribute("Name").Value, new List<string>());
                        foreach (XElement levelDiscovered in groupDiscovered.Elements("Level"))
                            _discoveredMap[groupDiscovered.Attribute("Name").Value].Add(levelDiscovered.Attribute("Name").Value);
                    }
                }            
            }
            if (File.Exists(@"../Data/Prefabs/Map" + CurrentGameScreen.LevelGroupName + ".prefab"))
            {
                _mapEntity = DataManager.LoadPrefab(@"../Data/Prefabs/Map" + CurrentGameScreen.LevelGroupName + ".prefab", this);
                _mapEntity.transform.Position = Vector2.Zero;
            }
            if(_mapEntity != null)
                _currentMapRooms = _mapEntity.GetComponentsByInheritance<Graphic>().Where(g => g.GraphicTag != "MapBackground" && g.GraphicTag != "MapForeground").ToList<Graphic>();
            
        }

        public void RefreshMapData()
        {
            Alpha = 1.0f;
            if (!_discoveredMap.ContainsKey(CurrentGameScreen.LevelGroupName))
                return;

            if (_currentMapRooms != null)
            {
                foreach (Graphic g in _currentMapRooms)
                {
                   /* if (g.GraphicTag == CurrentGameScreen.LevelName)
                    {
                        Camera.Position = _mapEntity.transform.Position + g.Offset;
                        g.CurrentEffect = AssetManager.GetEffect("WhiteBlink");
                    }
                    else
                        g.CurrentEffect = AssetManager.GetEffect("BasicRender");*/

                    /*if (!_discoveredMap[CurrentGameScreen.LevelGroupName].Contains(g.GraphicTag))
                        g.opacity = 0.0f;
                    else
                        g.opacity = 1.0f;*/
                }
            }

        }

        public void AddLevelToMap(string groupName, string levelName)
        {
            if (!_discoveredMap.ContainsKey(groupName))
                _discoveredMap.Add(groupName, new List<string>());

            if (!_discoveredMap[groupName].Contains(levelName))
                _discoveredMap[groupName].Add(levelName);
        }

        public XElement SaveMapStatus()
        {
            XElement mapStatus = new XElement("MapStatus");

            foreach (KeyValuePair<string, List<string>> kvp in _discoveredMap)
            {
                XElement groupStatus = new XElement("Group", new XAttribute("Name", kvp.Key));

                foreach (string s in kvp.Value)
                    groupStatus.Add(new XElement("Level", new XAttribute("Name", s)));

                mapStatus.Add(groupStatus);
            }

            return mapStatus;
        }
    }
}
