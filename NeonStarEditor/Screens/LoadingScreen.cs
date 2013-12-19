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
        public Entity LoadingAnim;

        private bool _loadPreferences;

        public LoadingScreen(Game game, string levelToLoad = "", bool loadPreferences = false)
            :base(game)
        {
            _loadPreferences = loadPreferences;
            if (loadPreferences)
            {
                LevelToLoad = XDocument.Load(@"../Data/Config/EditorPreferences.xml").Element("XnaContent").Element("Preferences").Element("LevelToLoad").Value;
            }
            else
                LevelToLoad = levelToLoad;

            if(LevelToLoad == "")
                LevelToLoad = @"../Data/Levels/Level_Empty.xml";
        }

        public void LoadNextLevelAssets()
        {
            string[] folderNameFull = Path.GetDirectoryName(LevelToLoad).Split('\\');
            string folderName = folderNameFull[folderNameFull.Length - 1]; 
            AssetManager.LoadGroupData(Neon.graphicsDevice, folderName);
            AssetManager.LoadLevelData(Neon.graphicsDevice, folderName, Path.GetFileNameWithoutExtension(LevelToLoad));
            ThreadFinished = true;
        }

        public void LoadCommonAssets()
        {
            AssetManager.LoadCommonData(Neon.graphicsDevice);
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
                this.ChangeScreen(new EditorScreen(LevelToLoad, Neon.game, Neon.GraphicsDeviceManager, _loadPreferences));
            }
            base.Update(gameTime);
        }
    }
}
