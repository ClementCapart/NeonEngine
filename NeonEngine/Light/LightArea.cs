using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NeonEngine;

namespace NeonEngine
{
    public class LightArea
    {
        public RenderTarget2D RenderTarget { get; private set; }
        public Vector2 LightPosition { get; set; }
        public Vector2 LightAreaSize { get; set; }
        public Color color;

        public LightArea(ShadowmapSize size, Vector2 Position, Color color)
        {
            this.color = color;
            LightPosition = Position;
            int baseSize = 2 << (int)size;
            LightAreaSize = new Vector2(baseSize);
            RenderTarget = new RenderTarget2D(Neon.GraphicsDevice, baseSize, baseSize);
        }

        public Vector2 ToRelativePosition(Vector2 worldPosition)
        {
            return worldPosition - (LightPosition - LightAreaSize * 0.5f);
        }

        public void BeginDrawingShadowCasters()
        {
            Neon.GraphicsDevice.SetRenderTarget(RenderTarget);
            Neon.GraphicsDevice.Clear(Color.Transparent);
        }

        public void EndDrawingShadowCasters()
        {
            Neon.GraphicsDevice.SetRenderTarget(null);
        }
    }
}
