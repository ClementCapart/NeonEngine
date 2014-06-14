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
        private bool _respawning = false;
        private bool _sameGroup = false;

        public LoadingScreen(Game game, int startingSpawnPointIndex, bool sameGroup, string groupToLoad = "", string levelToLoad = "", XElement statusToLoad = null, bool loadPreferences = false)
            :base(game)
        {
            if (!SoundManager.MusicLock)
            {
                if (groupToLoad == "00TitleScreen" && (SoundManager.NextTrackName != "TitleMusic" && SoundManager.CurrentTrackName != "TitleMusic"))
                    SoundManager.CrossFadeLoopTrack("TitleMusic");
                else if (groupToLoad != "00TitleScreen" && SoundManager.NextTrackName != "GameLoop" && SoundManager.CurrentTrackName != "GameLoop")
                    SoundManager.CrossFadeLoopTrack("GameLoop");

            }

            this._startingSpawnPointIndex = startingSpawnPointIndex;
            _sameGroup = sameGroup;
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

        public LoadingScreen(Game game, XElement statusCheckPoint, string levelGroupName)
            : base(game)
        {
            string indexString = statusCheckPoint.Element("CurrentLevel").Element("SpawnPoint").Value;
            _startingSpawnPointIndex = indexString != "None" ? int.Parse(indexString) : 0;
            LevelToLoad = statusCheckPoint.Element("CurrentLevel").Element("LevelName").Value;
            GroupToLoad = statusCheckPoint.Element("CurrentLevel").Element("GroupName").Value;
            if (GroupToLoad == levelGroupName) _sameGroup = true;
            else _sameGroup = false;
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
                LoadNextLevelAssets();
            }

            if (ThreadFinished && LevelToLoad != "")
            {
                this.ChangeScreen(new EditorScreen(GroupToLoad, LevelToLoad, _startingSpawnPointIndex, _statusToLoad, Neon.Game, Neon.GraphicsDeviceManager, _loadPreferences, _respawning));
            }
            base.Update(gameTime);
        }
    }
}
