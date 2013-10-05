using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NeonEngine
{
    public class PolygonRenderer
    {
        Texture2D pixel;
        public List<Vector2> vectors;
        public Vector2 Position;
        public Color Color;

        public PolygonRenderer(GraphicsDevice graphicsDevice, Vector2 Position)
        {
            pixel = new Texture2D(graphicsDevice, 1, 1);
            Color[] pixels = new Color[1];
            pixels[0] = Color.White;
            pixel.SetData<Color>(pixels);
            this.Position = Position;
            Color = Color.White;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (vectors.Count < 2)
                return;

            for (int i = 1; i < vectors.Count; i++)
            {
                Vector2 vector1 = (Vector2)vectors[i - 1];
                Vector2 vector2 = (Vector2)vectors[i];

                float distance = Vector2.Distance(vector1, vector2);
                float angle = (float)Math.Atan2((double)(vector2.Y - vector1.Y),
                    (double)(vector2.X - vector1.X));

                spriteBatch.Draw(pixel,
                    vector1 + Position,
                    null,
                    Color,
                    angle,
                    Vector2.Zero,
                    new Vector2(distance, 3),
                    SpriteEffects.None,
                    1);
            }
        }
    }
}
