using Microsoft.Xna.Framework;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Linq;

namespace NeonStarEditor
{
    public class LoadingScreen : World
    {
        public bool ThreadFinished = false;
        public string LevelToLoad = "";
        public string GroupToLoad = "";
        public Entity LoadingAnim;

        private XElement _statusToLoad;

        private bool _loadPreferences;

        public int _startingSpawnPointIndex;

        public LoadingScreen(Game game, int startingSpawnPointIndex, string groupToLoad = "", string levelToLoad = "", XElement statusToLoad = null, bool loadPreferences = false)
            :base(game)
        {
            this._startingSpawnPointIndex = startingSpawnPointIndex;
            LevelToLoad = levelToLoad;
            GroupToLoad = groupToLoad;
            _statusToLoad = statusToLoad;
            _loadPreferences = loadPreferences;
            
            if (loadPreferences)
            {
                try
                {
                    LevelToLoad = XDocument.Load(@"../Data/Config/EditorPreferences.xml").Element("XnaContent").Element("Preferences").Element("LevelToLoad").Value;
                    GroupToLoad = XDocument.Load(@"../Data/Config/EditorPreferences.xml").Element("XnaContent").Element("Preferences").Element("GroupToLoad").Value;
                }
                catch
                {
                    Console.WriteLine("Preferences file not found ! It will be created the next time you exit the program.");
                }
            }
            else
                LevelToLoad = levelToLoad;

            if (LevelToLoad == "" || GroupToLoad == "")
            {
                GroupToLoad = "Tests";
                LevelToLoad = "LevelEmpty";
            }
        }

        public void LoadNextLevelAssets()
        {
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
                LoadNextLevelAssets();
            }

            if (ThreadFinished && LevelToLoad != "")
            {
                this.ChangeScreen(new EditorScreen(GroupToLoad, LevelToLoad, _startingSpawnPointIndex, _statusToLoad, Neon.Game, Neon.GraphicsDeviceManager, _loadPreferences));
            }
            base.Update(gameTime);
        }
    }
}
