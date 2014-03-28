using Microsoft.Xna.Framework;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Linq;

namespace NeonStarLibrary
{
    public class LoadingScreen : World
    {
        public bool ThreadFinished = false;
        public string LevelToLoad = "";
        public string GroupToLoad = "";

        public Entity LoadingAnim;
        private int _startingSpawnPointIndex = 0;
        private XElement _statusToLoad;
        private bool _respawning = false;
        private bool _sameGroup = false;

        public LoadingScreen(Game game, bool sameGroup, int startingSpawnPointIndex = 0, string groupToLoad = "", string levelToLoad = "", XElement statusToLoad = null)
            : base(game)
        {
            _sameGroup = sameGroup;
            this._startingSpawnPointIndex = startingSpawnPointIndex;
            LevelToLoad = levelToLoad;
            GroupToLoad = groupToLoad;
            _statusToLoad = statusToLoad;
        }

        public LoadingScreen(Game game, XElement statusCheckPoint)
            : base (game)
        {
            string indexString = statusCheckPoint.Element("CurrentLevel").Element("SpawnPoint").Value;
            _startingSpawnPointIndex = indexString != "None" ? int.Parse(indexString) : 0;
            LevelToLoad = statusCheckPoint.Element("CurrentLevel").Element("LevelName").Value;
            GroupToLoad = statusCheckPoint.Element("CurrentLevel").Element("GroupName").Value;
            _statusToLoad = statusCheckPoint;
            _respawning = true;
        }

        public void LoadNextLevelAssets()
        {
            if(!_sameGroup)
                AssetManager.LoadGroupData(Neon.GraphicsDevice, GroupToLoad);
            AssetManager.LoadLevelData(Neon.GraphicsDevice, GroupToLoad, LevelToLoad);
            ThreadFinished = true;
        }

        public void LoadCommonAssets()
        {
            AssetManager.LoadCommonData(Neon.GraphicsDevice);
            ThreadFinished = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (FirstUpdate)
            {
                if (!AssetManager.CommonLoaded)
                    LoadCommonAssets();
                if(LevelToLoad != "")
                    LoadNextLevelAssets();
            }

            
            if (ThreadFinished && LevelToLoad == "")
                this.ChangeScreen(new GameScreen("Tests", "LevelEmpty", _startingSpawnPointIndex, _statusToLoad, Neon.Game));
            else if (ThreadFinished)
                this.ChangeScreen(new GameScreen(GroupToLoad, LevelToLoad, _startingSpawnPointIndex, _statusToLoad, Neon.Game, _respawning));
            base.Update(gameTime);
        }
    }
}
