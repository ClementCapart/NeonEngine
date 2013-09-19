using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NeonEngine
{
    public class SpriteSheetInfo
    {
        public Texture2D Texture;
        public int FrameWidth;
        public int FrameHeight;
        public int FrameCount;
        public float FPS;
        public Vector2 Offset;

        public SpriteSheetInfo()
        {
        }
    }

    static public class AssetManager
    {
        #region fields
        static public ContentManager Content;

        static Dictionary<string, Effect> effectList;
        static Dictionary<string, Texture2D> assetsList;
        static Dictionary<string, SpriteSheetInfo> spritesheetList;
        public static Dictionary<string, string> effects;
        public static Dictionary<string, string> assets;
        public static Dictionary<string, string> spritesheets;
        #endregion

        static public void LoadAssets(GraphicsDevice device)
        {
            Content = new ContentManager(Neon.game.Services, "Content");

            assets = new Dictionary<string, string>();
            spritesheets = new Dictionary<string, string>();
            effects = new Dictionary<string, string>();

            /* use assets.add("tag", "filePath") to load your assets
             * the tag will be use in your entities to call your assets
             * 
             * the filepath is the absolute path to your file from your projet content root w/o the extention
             * 
             * ex : assets.Add("menuPlayButton", @"menu\buttons\play");
             */    
            effects.Add("ChromaticAberration", @"Shaders\ChromaticAberration");

            Load(device);
        }

        static private void Load(GraphicsDevice device)
        {
            assetsList = new Dictionary<string, Texture2D>();
            foreach (KeyValuePair<string, string> kvp in assets)
                assetsList.Add(kvp.Key, Content.Load<Texture2D>(kvp.Value));

            Texture2D fade = new Texture2D(device, Neon.ScreenWidth, Neon.ScreenHeight);
            Color[] fill = new Color[fade.Width * fade.Height];
            for (int i = 0; i < fill.Length; i++)
                fill[i] = Neon.fadeColor;
            fade.SetData<Color>(fill);
            Texture2D _black = new Texture2D(device, 50, 50);
            Color[] _fill = new Color[_black.Width * _black.Height];
            for (int i = 0; i < _fill.Length; i++)
                _fill[i] = Color.Black;
            _black.SetData<Color>(_fill);

            assetsList.Add("neon_screen", fade);
            assetsList.Add("neon_black_tile", _black);

            spritesheetList = new Dictionary<string, SpriteSheetInfo>();

            List<string> FilesPath = DirectorySearch("ContentStream");
            
            foreach (string s in FilesPath)
            {
                string[] FileNameProcessing = s.Split('\\');
                string FileName = FileNameProcessing[FileNameProcessing.Length - 1].Split('.')[0];
                string[] ssiInfo = FileName.Split('_');
      
                if (ssiInfo.Length <= 1)
                    assets.Add(ssiInfo[0], s);
                else
                    spritesheets.Add(ssiInfo[0], s);
            }
            
            effectList = new Dictionary<string,Effect>();

            foreach(KeyValuePair<string, string> kvp in effects)
                effectList.Add(kvp.Key, Content.Load<Effect>(kvp.Value));
        }

        static List<string> DirectorySearch(string CurrentDirectory)
        {
            List<string> FileList = new List<string>();
            
            if (!Directory.Exists(CurrentDirectory))
                Directory.CreateDirectory(CurrentDirectory);

            string[] Directories = Directory.GetDirectories(CurrentDirectory);
            foreach (string d in Directories)
            {
                foreach (string f in Directory.GetFiles(d))
                    FileList.Add(f);

                List<string> SubList = DirectorySearch(d);
                foreach (string f in SubList)
                    FileList.Add(f);
            }

            return FileList;
        }

        public static Texture2D PremultiplyTexture(String FilePath, GraphicsDevice device)
        {
            Texture2D texture;

            using (FileStream titleStream = File.OpenRead(FilePath))
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
            if (assetsList.ContainsKey(tag))
                return assetsList[tag];
            else
            {
                Texture2D texture = PremultiplyTexture(assets[tag], Neon.graphicsDevice);
                assetsList.Add(tag, texture);
                return texture;
            }
        }

        static public SpriteSheetInfo GetSpriteSheet(string tag)
        {
            if (spritesheetList.ContainsKey(tag))
                return spritesheetList[tag];
            else
            {
                string s = spritesheets[tag];
                Texture2D texture = PremultiplyTexture(s, Neon.graphicsDevice);
                string[] FileNameProcessing = s.Split('\\');
                string FileName = FileNameProcessing[FileNameProcessing.Length - 1].Split('.')[0];
                string[] ssiInfo = FileName.Split('_');

                SpriteSheetInfo ssi = new SpriteSheetInfo();
                ssi.Texture = texture;
                string Name = ssiInfo[0];
                ssi.FrameCount = int.Parse(ssiInfo[1].Remove(ssiInfo[1].Length - 1));
                ssi.FrameWidth = int.Parse(ssiInfo[2].Remove(ssiInfo[2].Length - 3));
                ssi.FrameHeight = int.Parse(ssiInfo[3].Remove(ssiInfo[3].Length - 3));
                if (ssiInfo.Length >= 5)
                    ssi.FPS = int.Parse(ssiInfo[4].Remove(ssiInfo[4].Length - 3));
                else
                    ssi.FPS = 24;
                if (ssiInfo.Length == 7)
                {
                    ssi.Offset.X = int.Parse(ssiInfo[5].Remove(ssiInfo[5].Length - 3));
                    ssi.Offset.Y = int.Parse(ssiInfo[6].Remove(ssiInfo[6].Length - 3));
                }
                spritesheetList.Add(Name, ssi);
                return ssi;
            }
        }

        static public Effect GetEffect(string tag)
        {
            return effectList[tag];
        }
    }
}
