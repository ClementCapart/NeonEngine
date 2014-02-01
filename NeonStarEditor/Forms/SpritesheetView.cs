using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NeonEngine;
using NeonEngine.Components.Graphics2D;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NeonStarEditor
{
    public class SpritesheetView : GraphicsDeviceControl
    {
        public SpriteSheetInfo SpritesheetToDraw;
        public SpriteBatch SpriteBatch;
        public ContentManager Content;
        public Vector2 Position;
        public Color BackgroundColor = Color.Gray;
        public float Zoom = 2.0f;

        public Entity entity = new Entity(null);
        public EditorSpriteSheet Spritesheet;

        public DateTime lastTime;

        public SpritesheetView()
        {
        }

        protected override void Initialize()
        {
            Application.Idle += delegate { Invalidate(); };
            SpriteBatch = new SpriteBatch(this.GraphicsDevice);
        }

        protected override void Draw()
        {
            if (Neon.Input.MouseCheck(MouseButton.LeftButton))
            {
                Position += Neon.Input.DeltaMouse;
            }

            if (Neon.Input.MouseCheck(MouseButton.RightButton))
            {
                Zoom += 1f;
                Zoom %= 4;
                if (Zoom == 0.0f)
                    Zoom = 1f;
            }

            Zoom += Neon.Input.MouseWheel();
            GraphicsDevice.Clear(BackgroundColor);
            if (SpritesheetToDraw != null)
            {
                Spritesheet.Position = Position;
                Spritesheet.Zoom = Zoom;
                Spritesheet.Update(DateTime.Now - lastTime);
                SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, null, null);
                Spritesheet.Draw(SpriteBatch);
                SpriteBatch.End();
            }
            lastTime = DateTime.Now;

        }

        public void LoadSpritesheet(string filePath, string tag)
        {
            Texture2D texture = AssetManager.PremultiplyTexture(filePath, GraphicsDevice);
            string[] fileNameProcessing = filePath.Split('\\');
            string fileName = fileNameProcessing[fileNameProcessing.Length - 1].Split('.')[0];
            string[] ssiInfo = fileName.Split('_');
            texture.Name = ssiInfo[0];

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

                ssi.Frames = AssetManager.GenerateSpritesheetFrames(GraphicsDevice, texture, ssi.FrameWidth, ssi.FrameHeight, ssi.FrameCount, 0);
                SpritesheetToDraw = ssi;
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

                        ssiSequence.Frames = AssetManager.GenerateSpritesheetFrames(GraphicsDevice, texture, ssiSequence.FrameWidth, ssiSequence.FrameHeight, ssiSequence.FrameCount, int.Parse(sequenceInfo[1]));
                        SpritesheetToDraw = ssiSequence;
                    }
                }
            }

            if (SpritesheetToDraw.Frames != null)
            {
                Spritesheet = new EditorSpriteSheet(SpritesheetToDraw, 1.0f);
                Spritesheet.IsLooped = true;
                Spritesheet.Init();
            }

           
        }

        private Texture2D PremultiplyTexture(string filePath, Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice)
        {
            throw new NotImplementedException();
        }
    }
}
