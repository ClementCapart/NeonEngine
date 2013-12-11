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

        public Entity LoadingAnim;

        public LoadingScreen(Game game, string levelToLoad = "")
            : base(game)
        {
            LevelToLoad = levelToLoad;
            if (levelToLoad != "")
            {
                Thread LoadingThread = new Thread(new ThreadStart(LoadNextLevelAssets));
                LoadingThread.Start();
            }
            else
            {
                Thread LoadingThread = new Thread(new ThreadStart(LoadCommonAssets));
                LoadingThread.Start();
            }

            LoadingAnim = new Entity(this);
            SpriteSheet ss = new SpriteSheet(LoadingAnim);
            ss.SpriteSheetTag = "LiOnRun";
            ss.Init();
            LoadingAnim.AddComponent(ss);
            entities.Add(LoadingAnim);
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
            if (ThreadFinished && LevelToLoad == "")
                this.ChangeScreen(new GameScreen(@"../Data/Levels/Level_Empty.xml", Neon.game));
            else if (ThreadFinished)
                this.ChangeScreen(new GameScreen(LevelToLoad, Neon.game));
            base.Update(gameTime);
        }
    }
}
