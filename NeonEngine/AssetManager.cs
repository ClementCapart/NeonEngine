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

        static Dictionary<string, Effect> _effectList;
        static Dictionary<string, Texture2D> _assetsList;
        static Dictionary<string, SpriteSheetInfo> _spritesheetList;
        public static Dictionary<string, string> Effects;
        public static Dictionary<string, string> Assets;
        public static Dictionary<string, string> Spritesheets;
        #endregion

        static public void LoadAssets(GraphicsDevice device)
        {
            Content = new ContentManager(Neon.game.Services, "Content");

            Assets = new Dictionary<string, string>();
            Spritesheets = new Dictionary<string, string>();
            Effects = new Dictionary<string, string>();

            /* use assets.add("tag", "filePath") to load your assets
             * the tag will be use in your entities to call your assets
             * 
             * the filepath is the absolute path to your file from your projet content root w/o the extention
             * 
             * ex : assets.Add("menuPlayButton", @"menu\buttons\play");
             */    
            Effects.Add("ChromaticAberration", @"Shaders\ChromaticAberration");

            Load(device);
        }

        static private void Load(GraphicsDevice device)
        {
            _assetsList = new Dictionary<string, Texture2D>();
            foreach (KeyValuePair<string, string> kvp in Assets)
                _assetsList.Add(kvp.Key, Content.Load<Texture2D>(kvp.Value));

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

            _assetsList.Add("neon_screen", fade);
            _assetsList.Add("neon_black_tile", black);

            _spritesheetList = new Dictionary<string, SpriteSheetInfo>();

            List<string> filesPath = DirectorySearch(@"../Data/ContentStream");
            
            foreach (string s in filesPath)
            {
                string[] fileNameProcessing = s.Split('\\');
                string fileName = fileNameProcessing[fileNameProcessing.Length - 1].Split('.')[0];
                string[] ssiInfo = fileName.Split('_');
      
                if (ssiInfo.Length <= 1)
                    Assets.Add(ssiInfo[0], s);
                else
                    Spritesheets.Add(ssiInfo[0], s);
            }
            
            _effectList = new Dictionary<string,Effect>();

            foreach(KeyValuePair<string, string> kvp in Effects)
                _effectList.Add(kvp.Key, Content.Load<Effect>(kvp.Value));
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
            if (_assetsList.ContainsKey(tag))
                return _assetsList[tag];
                        
            Texture2D texture = PremultiplyTexture(Assets[tag], Neon.graphicsDevice);
            _assetsList.Add(tag, texture);
            return texture;
        }

        static public SpriteSheetInfo GetSpriteSheet(string tag)
        {
            if (tag == "" || tag == null)
                return null;

            if (_spritesheetList.ContainsKey(tag))
                return _spritesheetList[tag];

            string s = Spritesheets[tag];
            Texture2D texture = PremultiplyTexture(s, Neon.graphicsDevice);
            string[] fileNameProcessing = s.Split('\\');
            string fileName = fileNameProcessing[fileNameProcessing.Length - 1].Split('.')[0];
            string[] ssiInfo = fileName.Split('_');

            SpriteSheetInfo ssi = new SpriteSheetInfo();
            ssi.FrameCount = int.Parse(ssiInfo[1].Remove(ssiInfo[1].Length - 1));
            ssi.FrameWidth = int.Parse(ssiInfo[2].Remove(ssiInfo[2].Length - 3));
            ssi.FrameHeight = int.Parse(ssiInfo[3].Remove(ssiInfo[3].Length - 3));
            ssi.Frames = GenerateSpritesheetFrames(texture, ssi.FrameWidth, ssi.FrameHeight, ssi.FrameCount);
            string name = ssiInfo[0];
            
            if (ssiInfo.Length >= 5)
                ssi.Fps = int.Parse(ssiInfo[4].Remove(ssiInfo[4].Length - 3));
            else
                ssi.Fps = 24;
            if (ssiInfo.Length == 7)
            {
                ssi.Offset.X = int.Parse(ssiInfo[5].Remove(ssiInfo[5].Length - 3));
                ssi.Offset.Y = int.Parse(ssiInfo[6].Remove(ssiInfo[6].Length - 3));
            }
            _spritesheetList.Add(name, ssi);
            return ssi;
        }

        static public string GetSpritesheetTag(SpriteSheetInfo ssi)
        {
            if (_spritesheetList.Where(p => p.Value == ssi).Count() > 0)
                return _spritesheetList.Where(p => p.Value == ssi).Select(p => p.Key).First();
            else
                return null;
        }

        static public Texture2D[] GenerateSpritesheetFrames(Texture2D texture, int frameWidth, int frameHeight, int frameCount)
        {
            Texture2D[] frames = null;            

            int columns = texture.Width / frameWidth;
            int rows = texture.Height / frameHeight;

            frames = new Texture2D[frameCount];

            int currentColumn = 0, currentRow = 0;

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
            return _effectList[tag];
        }
    }
}
