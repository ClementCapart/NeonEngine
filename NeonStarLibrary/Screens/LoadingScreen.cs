using Microsoft.Xna.Framework;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace NeonStarLibrary
{
    public class LoadingScreen : World
    {
        public bool ThreadFinished = false;
        public string LevelToLoad = "";
        public string GroupToLoad = "";

        public Entity LoadingAnim;
        private int _startingSpawnPointIndex = 0;

        public LoadingScreen(Game game, int startingSpawnPointIndex = 0, string groupToLoad = "", string levelToLoad = "")
            : base(game)
        {
            this._startingSpawnPointIndex = startingSpawnPointIndex;
            LevelToLoad = levelToLoad;
            GroupToLoad = groupToLoad;
        }

        public void LoadNextLevelAssets()
        {
            string[] folderNameFull = Path.GetDirectoryName(LevelToLoad).Split('\\');
            string folderName = folderNameFull[folderNameFull.Length - 1];
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
                if (LevelToLoad != "")
                {
                    LoadNextLevelAssets();
                }
                else
                {
                    LoadCommonAssets();
                }
            }

            
            if (ThreadFinished && LevelToLoad == "")
                this.ChangeScreen(new GameScreen("Tests", "LevelEmpty", _startingSpawnPointIndex, Neon.Game));
            else if (ThreadFinished)
                this.ChangeScreen(new GameScreen(GroupToLoad, LevelToLoad, _startingSpawnPointIndex, Neon.Game));
            base.Update(gameTime);
        }
    }
}
