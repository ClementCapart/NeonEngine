using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NeonEngine
{
    public class SpriteSheetInfo
    {
        public Texture2D[] Frames;
        public int FrameWidth;
        public int FrameHeight;
        public int FrameCount;
        public float Fps;
        public Vector2 Offset;
    }

    static public class AssetManager
    {
        #region fields
        static public ContentManager Content;

        static Dictionary<string, Effect> _commonEffectList;
        static Dictionary<string, Texture2D> _commonAssetsList;
        static Dictionary<string, SpriteSheetInfo> _commonSpritesheetList;
        public static Dictionary<string, string> CommonEffects;
        public static Dictionary<string, string> CommonAssets;
        public static Dictionary<string, string> CommonSpritesheets;

        static Dictionary<string, Effect> _groupEffectList;
        static Dictionary<string, Texture2D> _groupAssetsList;
        static Dictionary<string, SpriteSheetInfo> _groupSpritesheetList;
        public static Dictionary<string, string> GroupEffects;
        public static Dictionary<string, string> GroupAssets;
        public static Dictionary<string, string> GroupSpritesheets;

        static Dictionary<string, Effect> _levelEffectList;
        static Dictionary<string, Texture2D> _levelAssetsList;
        static Dictionary<string, SpriteSheetInfo> _levelSpritesheetList;
        public static Dictionary<string, string> LevelEffects;
        public static Dictionary<string, string> LevelAssets;
        public static Dictionary<string, string> LevelSpritesheets;
        #endregion

        static public void Initialize(GraphicsDevice device)
        {
            Content = new ContentManager(Neon.game.Services, "Content");

            CommonAssets = new Dictionary<string, string>();
            CommonSpritesheets = new Dictionary<string, string>();
            CommonEffects = new Dictionary<string, string>();

            _commonAssetsList = new Dictionary<string, Texture2D>();
            _commonSpritesheetList = new Dictionary<string, SpriteSheetInfo>();
            _commonEffectList = new Dictionary<string, Effect>();

            GroupAssets = new Dictionary<string, string>();
            GroupSpritesheets = new Dictionary<string, string>();
            GroupEffects = new Dictionary<string, string>();

            _groupAssetsList = new Dictionary<string, Texture2D>();
            _groupSpritesheetList = new Dictionary<string, SpriteSheetInfo>();
            _groupEffectList = new Dictionary<string, Effect>();

            LevelAssets = new Dictionary<string, string>();
            LevelSpritesheets = new Dictionary<string, string>();
            LevelEffects = new Dictionary<string, string>();

            _levelAssetsList = new Dictionary<string, Texture2D>();
            _levelSpritesheetList = new Dictionary<string, SpriteSheetInfo>();
            _levelEffectList = new Dictionary<string, Effect>();
        }

        static public void LoadCommonData(GraphicsDevice device)
        {
            foreach (KeyValuePair<string, SpriteSheetInfo> kvp in _commonSpritesheetList)
                foreach (Texture2D tex in kvp.Value.Frames)
                    tex.Dispose();

            foreach (KeyValuePair<string, Texture2D> kvp in _commonAssetsList)
                kvp.Value.Dispose();

            CommonAssets = new Dictionary<string, string>();
            CommonSpritesheets = new Dictionary<string, string>();
            CommonEffects = new Dictionary<string, string>();

            _commonAssetsList = new Dictionary<string, Texture2D>();
            _commonSpritesheetList = new Dictionary<string, SpriteSheetInfo>();
            _commonEffectList = new Dictionary<string, Effect>();

            Texture2D fade = new Texture2D(device, Neon.ScreenWidth, Neon.ScreenHeight);
            Color[] fill = new Color[fade.Width * fade.Height];
            for (int i = 0; i < fill.Length; i++)
                fill[i] = Neon.fadeColor;
            fade.SetData(fill);
            Texture2D black = new Texture2D(device, 50, 50);
            fill = new Color[black.Width * black.Height];
            for (int i = 0; i < fill.Length; i++)
                fill[i] = Color.Black;
            black.SetData(fill);

            _commonAssetsList.Add("neon_screen", fade);
            _commonAssetsList.Add("neon_black_tile", black);

            List<string> filesPath = DirectorySearch(@"../Data/ContentStream/Common");
#if DEBUG
            filesPath.AddRange(DirectorySearch(@"../Data/ContentStream/EditorContent"));
#endif
            
            foreach (string s in filesPath)
            {
                string[] fileNameProcessing = s.Split('\\');
                string fileName = fileNameProcessing[fileNameProcessing.Length - 1].Split('.')[0];
                string[] ssiInfo = fileName.Split('_');
      
                if (ssiInfo.Length <= 1)
                    CommonAssets.Add(ssiInfo[0], s);
                else if (ssiInfo.Length <= 7)
                    CommonSpritesheets.Add(ssiInfo[0], s);
                else
                    for (int i = 7; i < ssiInfo.Length; i++)
                        CommonSpritesheets.Add(ssiInfo[0]+ssiInfo[i].Split('-')[0], s);
            }    

            foreach(KeyValuePair<string, string> kvp in CommonEffects)
                _commonEffectList.Add(kvp.Key, Content.Load<Effect>(kvp.Value));

            foreach(string s in CommonSpritesheets.Keys)
                LoadSpritesheet(s, CommonSpritesheets, _commonSpritesheetList);

            foreach (string s in CommonAssets.Keys)
                LoadTexture(s, CommonAssets, _commonAssetsList);
        }

        static public void LoadGroupData(GraphicsDevice device, string groupName)
        {
            foreach (KeyValuePair<string, SpriteSheetInfo> kvp in _groupSpritesheetList)
                foreach (Texture2D tex in kvp.Value.Frames)
                    tex.Dispose();

            foreach (KeyValuePair<string, Texture2D> kvp in _groupAssetsList)
                kvp.Value.Dispose();

            GroupAssets = new Dictionary<string, string>();
            GroupSpritesheets = new Dictionary<string, string>();

            _groupAssetsList = new Dictionary<string, Texture2D>();
            _groupSpritesheetList = new Dictionary<string, SpriteSheetInfo>();

            List<string> filesPath = DirectorySearch(@"../Data/ContentStream/LevelsContent/" + groupName + "/Common");

            foreach (string s in filesPath)
            {
                string[] fileNameProcessing = s.Split('\\');
                string fileName = fileNameProcessing[fileNameProcessing.Length - 1].Split('.')[0];
                string[] ssiInfo = fileName.Split('_');

                if (ssiInfo.Length <= 1)
                    GroupAssets.Add(ssiInfo[0], s);
                else if (ssiInfo.Length <= 7)
                    GroupSpritesheets.Add(ssiInfo[0], s);
                else
                    for (int i = 7; i < ssiInfo.Length; i++)
                        GroupSpritesheets.Add(ssiInfo[0] + ssiInfo[i].Split('-')[0], s);
            }

            foreach (KeyValuePair<string, string> kvp in GroupEffects)
                _groupEffectList.Add(kvp.Key, Content.Load<Effect>(kvp.Value));

            foreach (string s in GroupSpritesheets.Keys)
                LoadSpritesheet(s, GroupSpritesheets, _groupSpritesheetList);

            foreach (string s in GroupAssets.Keys)
                LoadTexture(s, GroupAssets, _groupAssetsList);
        }

        static public void LoadLevelData(GraphicsDevice device, string groupName, string levelName)
        {
            foreach (KeyValuePair<string, SpriteSheetInfo> kvp in _levelSpritesheetList)
                foreach (Texture2D tex in kvp.Value.Frames)
                    tex.Dispose();

            foreach (KeyValuePair<string, Texture2D> kvp in _levelAssetsList)
                kvp.Value.Dispose();

            LevelAssets = new Dictionary<string, string>();
            LevelSpritesheets = new Dictionary<string, string>();

            _levelAssetsList = new Dictionary<string, Texture2D>();
            _levelSpritesheetList = new Dictionary<string, SpriteSheetInfo>();

            foreach (KeyValuePair<string, string> kvp in LevelAssets)
                _levelAssetsList.Add(kvp.Key, Content.Load<Texture2D>(kvp.Value));

            List<string> filesPath = DirectorySearch(@"../Data/ContentStream/LevelsContent/" + groupName + "/" + levelName);

            foreach (string s in filesPath)
            {
                string[] fileNameProcessing = s.Split('\\');
                string fileName = fileNameProcessing[fileNameProcessing.Length - 1].Split('.')[0];
                string[] ssiInfo = fileName.Split('_');

                if (ssiInfo.Length <= 1)
                    LevelAssets.Add(ssiInfo[0], s);
                else if (ssiInfo.Length <= 7)
                    LevelSpritesheets.Add(ssiInfo[0], s);
                else
                    for (int i = 7; i < ssiInfo.Length; i++)
                        LevelSpritesheets.Add(ssiInfo[0] + ssiInfo[i].Split('-')[0], s);
            }

            foreach (string s in LevelSpritesheets.Keys)
                LoadSpritesheet(s, LevelSpritesheets, _levelSpritesheetList);

            foreach (string s in LevelAssets.Keys)
                LoadTexture(s, LevelAssets, _levelAssetsList);
        }

        static List<string> DirectorySearch(string currentDirectory)
        {
            List<string> fileList = new List<string>();
            
            if (!Directory.Exists(currentDirectory))
                Directory.CreateDirectory(currentDirectory);

            string[] directories = Directory.GetDirectories(currentDirectory);
            foreach (string d in directories)
            {
                foreach (string f in Directory.GetFiles(d))
                    fileList.Add(f);

                List<string> subList = DirectorySearch(d);
                foreach (string f in subList)
                    fileList.Add(f);
            }

            return fileList;
        }

        public static Texture2D PremultiplyTexture(String filePath, GraphicsDevice device)
        {
            Texture2D texture;

            using (FileStream titleStream = File.OpenRead(filePath))
            {
                texture = Texture2D.FromStream(device, titleStream);
            }

            Color[] buffer = new Color[texture.Width * texture.Height];
            texture.GetData(buffer);
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = Color.FromNonPremultiplied(
                        buffer[i].R, buffer[i].G, buffer[i].B, buffer[i].A);
            }
            texture.SetData(buffer);

            return texture;
        }

        static public Texture2D GetTexture(string tag)
        {
            if (_commonAssetsList.ContainsKey(tag))
                return _commonAssetsList[tag];
            else if (_groupAssetsList.ContainsKey(tag))
                return _groupAssetsList[tag];
            else if (_levelAssetsList.ContainsKey(tag))
                return _levelAssetsList[tag];

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("!! AssetManager !!");
            Console.WriteLine("!! Missing Texture : " + tag + " !!");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.White;
            return null;
        }

        private static Texture2D LoadTexture(string tag, Dictionary<string, string> dataLibrary, Dictionary<string, Texture2D> assetsLibrary)
        {
            Texture2D texture = PremultiplyTexture(dataLibrary[tag], Neon.graphicsDevice);
            assetsLibrary.Add(tag, texture);
            return texture;
        }

        static public SpriteSheetInfo GetSpriteSheet(string tag)
        {
            if (tag == "" || tag == null)
                return null;

            if (_commonSpritesheetList.ContainsKey(tag))
                return _commonSpritesheetList[tag];
            else if (_groupSpritesheetList.ContainsKey(tag))
                return _groupSpritesheetList[tag];
            else if (_levelSpritesheetList.ContainsKey(tag))
                return _levelSpritesheetList[tag];

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("!! AssetManager !!");
            Console.WriteLine("!! Missing Spritesheet : "+ tag +" !!");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.White;
            return null;
        }

        private static void LoadSpritesheet(string tag, Dictionary<string, string> dataLibrary, Dictionary<string, SpriteSheetInfo> assetsLibrary)
        {
            string s = dataLibrary[tag];
            Texture2D texture = PremultiplyTexture(s, Neon.graphicsDevice);
            string[] fileNameProcessing = s.Split('\\');
            string fileName = fileNameProcessing[fileNameProcessing.Length - 1].Split('.')[0];
            string[] ssiInfo = fileName.Split('_');

            if (tag == ssiInfo[0])
            {
                SpriteSheetInfo ssi = new SpriteSheetInfo();
                ssi.FrameCount = int.Parse(ssiInfo[1].Remove(ssiInfo[1].Length - 1));
                ssi.FrameWidth = int.Parse(ssiInfo[2].Remove(ssiInfo[2].Length - 3));
                ssi.FrameHeight = int.Parse(ssiInfo[3].Remove(ssiInfo[3].Length - 3));

                if (ssiInfo.Length >= 5)
                    ssi.Fps = int.Parse(ssiInfo[4].Remove(ssiInfo[4].Length - 3));
                else
                    ssi.Fps = 24;
                if (ssiInfo.Length >= 7)
                {
                    ssi.Offset.X = int.Parse(ssiInfo[5].Remove(ssiInfo[5].Length - 3));
                    ssi.Offset.Y = int.Parse(ssiInfo[6].Remove(ssiInfo[6].Length - 3));
                }

                ssi.Frames = GenerateSpritesheetFrames(texture, ssi.FrameWidth, ssi.FrameHeight, ssi.FrameCount, 0);
                assetsLibrary.Add(ssiInfo[0], ssi);
            }
            else
            {
                for (int i = 7; i < ssiInfo.Length; i++)
                {
                    string[] sequenceInfo = ssiInfo[i].Split('-');
                    if (tag == ssiInfo[0] + sequenceInfo[0])
                    {
                        SpriteSheetInfo ssiSequence = new SpriteSheetInfo();
                        ssiSequence.FrameCount = int.Parse(sequenceInfo[2]);
                        ssiSequence.FrameWidth = int.Parse(ssiInfo[2].Remove(ssiInfo[2].Length - 3));
                        ssiSequence.FrameHeight = int.Parse(ssiInfo[3].Remove(ssiInfo[3].Length - 3));

                        if (ssiInfo.Length >= 5)
                            ssiSequence.Fps = int.Parse(ssiInfo[4].Remove(ssiInfo[4].Length - 3));
                        else
                            ssiSequence.Fps = 24;
                        if (ssiInfo.Length >= 7)
                        {
                            ssiSequence.Offset.X = int.Parse(ssiInfo[5].Remove(ssiInfo[5].Length - 3));
                            ssiSequence.Offset.Y = int.Parse(ssiInfo[6].Remove(ssiInfo[6].Length - 3));
                        }

                        ssiSequence.Frames = GenerateSpritesheetFrames(texture, ssiSequence.FrameWidth, ssiSequence.FrameHeight, ssiSequence.FrameCount, int.Parse(sequenceInfo[1]));
                        assetsLibrary.Add(ssiInfo[0] + sequenceInfo[0], ssiSequence);
                    }
                }
            }
        }

        static public string GetSpritesheetTag(SpriteSheetInfo ssi)
        {
            if (_commonSpritesheetList.Where(p => p.Value == ssi).Count() > 0)
                return _commonSpritesheetList.Where(p => p.Value == ssi).Select(p => p.Key).First();
            else if (_groupSpritesheetList.Where(p => p.Value == ssi).Count() > 0)
                return _groupSpritesheetList.Where(p => p.Value == ssi).Select(p => p.Key).First();
            else if (_levelSpritesheetList.Where(p => p.Value == ssi).Count() > 0)
                return _levelSpritesheetList.Where(p => p.Value == ssi).Select(p => p.Key).First();
            return null;
        }

        static public Texture2D[] GenerateSpritesheetFrames(Texture2D texture, int frameWidth, int frameHeight, int frameCount, int startingFrame)
        {
            Texture2D[] frames = null;            

            int columns = texture.Width / frameWidth;
            int rows = texture.Height / frameHeight;

            frames = new Texture2D[frameCount];

            int currentColumn = 0, currentRow = 0;

            while (startingFrame > 0)
            {
                if (currentColumn == columns - 1)
                {
                    currentColumn = 0;
                    currentRow++;
                }
                else
                    currentColumn++;
                
                startingFrame--;
            }

            for (int i = 0; i < frameCount; i++)
            {
                Color[] currentColors = new Color[frameWidth * frameHeight];
                texture.GetData<Color>(0, new Rectangle(currentColumn * frameWidth, currentRow * frameHeight, frameWidth, frameHeight), currentColors, 0, currentColors.Length);

                frames[i] = new Texture2D(Neon.graphicsDevice, frameWidth, frameHeight, true, SurfaceFormat.Color);
                frames[i].SetData(currentColors);

                if (currentColumn == columns - 1)
                {
                    currentColumn = 0;
                    currentRow++;
                }
                else
                    currentColumn++;
            }
            
            return frames;
        }

        static public Effect GetEffect(string tag)
        {
            return _commonEffectList[tag];
        }
    }
}
