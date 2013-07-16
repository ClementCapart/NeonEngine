using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace NeonEngine
{
    public class Light
    {
        public Texture2D gradient;
        public Vector2 position;

        public Light(Vector2 position, int radius)
        {
            this.position = position;
            gradient = Neon.utils.generateRadialGradient(radius);
        }

        public void DrawLightMask(SpriteBatch sb)
        {
            sb.Draw(gradient, position, Color.White);
        }
    }
}
