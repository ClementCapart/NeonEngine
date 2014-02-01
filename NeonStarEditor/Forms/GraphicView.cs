using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NeonStarEditor
{
    public class GraphicView : GraphicsDeviceControl
    {
        public Texture2D TextureToDraw;
        public SpriteBatch SpriteBatch;
        public ContentManager Content;
        public Vector2 Position = Vector2.Zero;
        public Color BackgroundColor = Color.Gray;
        public float Zoom = 2.0f;

        public GraphicView()
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
            if (TextureToDraw != null)
            {
                SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, null, null);
                SpriteBatch.Draw(TextureToDraw, Position + new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2), null, Color.White, 0.0f, new Vector2(TextureToDraw.Width / 2, TextureToDraw.Height / 2), Zoom, SpriteEffects.None, 0.0f);
                SpriteBatch.End();
            }

        }

        internal void LoadTexture(string filePath)
        {
            using (FileStream titleStream = File.OpenRead(filePath))
            {
                TextureToDraw = Texture2D.FromStream(GraphicsDevice, titleStream);
            }

            Color[] buffer = new Color[TextureToDraw.Width * TextureToDraw.Height];
            TextureToDraw.GetData(buffer);
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = Color.FromNonPremultiplied(
                        buffer[i].R, buffer[i].G, buffer[i].B, buffer[i].A);
            }
            TextureToDraw.SetData(buffer);
        }
    }
}
