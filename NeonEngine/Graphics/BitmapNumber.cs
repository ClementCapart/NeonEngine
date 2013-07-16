using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NeonEngine
{
    public class BitmapNumber
    {
        List<Texture2D> bitmap;
        public int Length;
        string style;

        public BitmapNumber(string style)
        {
            this.style = style;
            bitmap = new List<Texture2D>();
        }

        public void GenerateBitmap(string number)
        {
            bitmap.Clear();
            float XoffSet = 0;
            for (int i = 0; i < number.Length; i++)
            {
                bitmap.Add(AssetManager.GetTexture(style + number[i]));
                XoffSet += (bitmap[bitmap.Count - 1].Width + 5);
            }
            Length = (int)XoffSet;
        }

        public void Draw(SpriteBatch sb, Vector2 position)
        {
            float XoffSet = 0;
            foreach (Texture2D t in bitmap)
            {
                sb.Draw(t, position + new Vector2(XoffSet, 0), Color.White);
                XoffSet += t.Width + 5;
            }
        }
    }
}
