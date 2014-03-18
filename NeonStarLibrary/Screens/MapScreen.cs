using Microsoft.Xna.Framework;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    public class MapScreen : World
    {
        public GameScreen CurrentGameScreen;

        private Dictionary<string, List<string>> _discoveredMap;

        public MapScreen(Game game)
            :base(game)
        {
            DataManager.LoadPrefab(@"../Data/Prefabs/Map.prefab", this);
            _discoveredMap = new Dictionary<string, List<string>>();
            Camera.Position = Vector2.Zero;
        }

        public override void Update(GameTime gameTime)
        {
            if(Neon.Input.Pressed(Microsoft.Xna.Framework.Input.Buttons.Back))
            {
                Neon.World = CurrentGameScreen;
            }
            base.Update(gameTime);
        }

        public void InitializeMapData()
        {
            _discoveredMap = new Dictionary<string, List<string>>();
        }

        public void AddLevelToMap(string groupName, string levelName)
        {
            if (!_discoveredMap.ContainsKey(groupName))
                _discoveredMap.Add(groupName, new List<string>());

            if (!_discoveredMap[groupName].Contains(levelName))
                _discoveredMap[groupName].Add(levelName);
        }
    }
}
