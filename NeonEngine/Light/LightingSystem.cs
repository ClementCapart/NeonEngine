using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NeonEngine
{
    public class LightingSystem
    {
        List<Light> lights;
        public bool LightingEnabled = false;
        public RenderTarget2D lightMask;
        public float ambientDarkness = 0.1f;

        public LightingSystem()
        {
            lights = new List<Light>();
            lightMask = new RenderTarget2D(Neon.graphicsDevice, Neon.ScreenWidth, Neon.ScreenHeight);
        }

        public void AddLight(Light light)
        {
            lights.Add(light);
        }

        public void ComposeLightMask(SpriteBatch sb, World container)
        {
            if (LightingEnabled)
            {
                Neon.graphicsDevice.SetRenderTarget(lightMask);
                Neon.graphicsDevice.Clear(new Color(ambientDarkness, ambientDarkness, ambientDarkness));
                sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, container.camera.get_transformation(Neon.graphicsDevice));
                foreach (Light l in lights)
                    l.DrawLightMask(sb);
                sb.End();
                Neon.graphicsDevice.SetRenderTarget(null);
            }
        }
    }
}
