using Microsoft.Xna.Framework;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace NeonStarEditor
{
    public class LoadingScreen : World
    {
        public bool ThreadFinished = false;
        public string LevelToLoad = "";
        public Entity LoadingAnim;

        public LoadingScreen(Game game, string levelToLoad = "")
            :base(game)
        {
            LevelToLoad = levelToLoad;
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
                if (LevelToLoad != "")
                {
                    LoadNextLevelAssets();
                }
                else
                {
                    LoadCommonAssets();
                }
            }


            if (ThreadFinished && LevelToLoad != "")
            {
                this.ChangeScreen(new EditorScreen(LevelToLoad, Neon.game, Neon.GraphicsDeviceManager));
            }
            else if (ThreadFinished)
            {
                this.ChangeScreen(new EditorScreen(@"../Data/Levels/Level_Empty.xml", Neon.game, Neon.GraphicsDeviceManager));
            }
            base.Update(gameTime);
        }
    }
}
