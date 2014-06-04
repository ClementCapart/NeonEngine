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

        private float _cameraSpeed = 400.0f;

        private List<Graphic> _currentMapRooms;
        private SpriteSheet _liOnPositionToken;

        private Entity _mapEntity;
        private float k = 0.2f;
        private float kcube = 0.3f;
        private float _leftSide = float.MaxValue;
        private float _rightSide = float.MinValue;
        private float _topSide = float.MaxValue;
        private float _bottomSide = float.MinValue;

        private float _colorValue = 1.0f;
        private bool _goingUp = false;

        private Graphic _currentMap;

        private float _border = 200.0f;

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
            if(Neon.Input.Pressed(NeonStarInput.Map) || Neon.Input.Pressed(NeonStarInput.Guard))
            {
                CurrentGameScreen.Alpha = 1.0f;
                Neon.World = CurrentGameScreen;
            }

            if (Neon.Input.Check(NeonStarInput.MoveRight) || Neon.Input.Check(NeonStarInput.CameraRight))
            {
                if (((Camera.Position + new Vector2(_cameraSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds, 0) - Neon.HalfScreen).X < _rightSide - _border))
                    Camera.Position += new Vector2(_cameraSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
            }
            else if (Neon.Input.Check(NeonStarInput.MoveLeft) || Neon.Input.Check(NeonStarInput.CameraLeft))
            {
                if (((Camera.Position - new Vector2(_cameraSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds, 0) + Neon.HalfScreen).X > _leftSide + _border))
                    Camera.Position -= new Vector2(_cameraSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
            }

            if (Neon.Input.Check(NeonStarInput.MoveDown) || Neon.Input.Check(NeonStarInput.CameraDown))
            {
                if (((Camera.Position + new Vector2(0, _cameraSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds) - Neon.HalfScreen).Y < _bottomSide - _border))
                    Camera.Position += new Vector2(0, _cameraSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
            else if (Neon.Input.Check(NeonStarInput.MoveUp) || Neon.Input.Check(NeonStarInput.CameraUp))
            {
                if (((Camera.Position - new Vector2(0, _cameraSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds) + Neon.HalfScreen).Y > _topSide+ _border))
                    Camera.Position -= new Vector2(0, _cameraSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
                
            }

            if (_currentMap != null)
            {
                _currentMap.CurrentEffect.Parameters["ColorToUse"].SetValue(Color.Lerp(Color.Black, Color.FromNonPremultiplied(150, 31, 31, 255), _colorValue).ToVector4());
                if (_goingUp)
                {
                    if (_colorValue < 1.0f)
                    {
                        _colorValue += (float)gameTime.ElapsedGameTime.TotalSeconds * 0.5f;
                    }
                    else
                    {
                        _colorValue = 1.0f;
                        _goingUp = false;
                    }
                }
                else
                {
                    if (_colorValue > 0.3f)
                    {
                        _colorValue -= (float)gameTime.ElapsedGameTime.TotalSeconds * 0.5f;
                    }
                    else
                    {
                        _colorValue = 0.3f;
                        _goingUp = true;
                    }
                }
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
            if (_mapEntity != null)
            {
                _currentMapRooms = _mapEntity.GetComponentsByInheritance<Graphic>().Where(g => g.GraphicTag != "MapCaption" && g.GraphicTag != "MapBackground" && g.GraphicTag != "MapForeground" && g.GraphicTag != "MapOverlay" && !g.GraphicTag.StartsWith("MapName")).ToList<Graphic>();
                List<SpriteSheet> ss = _mapEntity.GetComponentsByInheritance<SpriteSheet>();
                if(ss.Where(s => s.SpriteSheetTag == "MapLionToken").Count() > 0)
                    _liOnPositionToken = ss.Where(s => s.SpriteSheetTag == "MapLionToken").First();

            }
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
                    if (g.GraphicTag == CurrentGameScreen.LevelName)
                    {
                        Camera.Position = _mapEntity.transform.Position + g.Offset;
                        /*if (_liOnPositionToken != null)
                            _liOnPositionToken.Offset = g.Offset / _mapEntity.transform.Scale + new Vector2(0, -_liOnPositionToken.spriteSheetInfo.FrameHeight / 2);*/
                        g.CurrentEffect = AssetManager.GetEffect("MapGlow");
                        _currentMap = g;
                    }


                    if (!_discoveredMap[CurrentGameScreen.LevelGroupName].Contains(g.GraphicTag))
                        g.Opacity = 0.0f;
                    else
                    {
                        g.Opacity = 1.0f;
                        if (g.Offset.X + g.texture.Width > _rightSide)
                            _rightSide = g.Offset.X + g.texture.Width;
                        if (g.Offset.X - g.texture.Width < _leftSide)
                            _leftSide = g.Offset.X - g.texture.Width;
                        if (g.Offset.Y + g.texture.Height > _bottomSide)
                            _bottomSide = g.Offset.Y + g.texture.Height;
                        if (g.Offset.Y - g.texture.Height < _topSide)
                            _topSide = g.Offset.Y - g.texture.Height;

                    }
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
