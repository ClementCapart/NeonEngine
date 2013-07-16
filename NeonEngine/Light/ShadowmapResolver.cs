using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine
{
    class ShadowmapResolver
    {
        private int reductionChainCount;
        private int baseSize;
        private int depthBufferSize;

        Effect resolveShadowsEffect;
        Effect reductionEffect;

        RenderTarget2D distortRT;
        RenderTarget2D shadowMap;
        RenderTarget2D shadowsRT;
        RenderTarget2D processedShadowsRT;

        QuadRenderComponent quadRender;
        RenderTarget2D distancesRT;
        RenderTarget2D[] reductionRT;

        public ShadowmapResolver(QuadRenderComponent quadRender, ShadowmapSize maxShadowmapSize, ShadowmapSize maxDepthBufferSize)
        {
            this.quadRender = quadRender;
            reductionChainCount = (int)maxShadowmapSize;
            baseSize = 2 << reductionChainCount;
            depthBufferSize = 2 << (int)maxDepthBufferSize;
        }

        public void LoadContent(ContentManager content)
        {
            reductionEffect = content.Load<Effect>(@"Shaders\reductionEffect");
            resolveShadowsEffect = content.Load<Effect>(@"Shaders\resolveShadowsEffect");

            distortRT = new RenderTarget2D(Neon.graphicsDevice, baseSize, baseSize, false, SurfaceFormat.HalfVector2, DepthFormat.None);
            distancesRT = new RenderTarget2D(Neon.graphicsDevice, baseSize, baseSize, false, SurfaceFormat.HalfVector2, DepthFormat.None);
            shadowMap = new RenderTarget2D(Neon.graphicsDevice, 2, baseSize, false, SurfaceFormat.HalfVector2, DepthFormat.None);
            reductionRT = new RenderTarget2D[reductionChainCount];
            for (int i = 0; i < reductionChainCount; i++)
                reductionRT[i] = new RenderTarget2D(Neon.graphicsDevice, 2 << i, baseSize, false, SurfaceFormat.HalfVector2, DepthFormat.None);

            shadowsRT = new RenderTarget2D(Neon.graphicsDevice, baseSize, baseSize);
            processedShadowsRT = new RenderTarget2D(Neon.graphicsDevice, baseSize, baseSize);
        }

        public void ResolveShadows(Texture2D shadowCastersTexture, RenderTarget2D result, Vector2 lightPosition)
        {
            Neon.graphicsDevice.BlendState = BlendState.Opaque;

            ExecuteTechnique(shadowCastersTexture, distancesRT, "ComputeDistances");
            ExecuteTechnique(distancesRT, distortRT, "Distort");
            ApplyHorizontalReduction(distortRT, shadowMap);
            ExecuteTechnique(null, shadowsRT, "DrawShadows", shadowMap);
            ExecuteTechnique(shadowsRT, processedShadowsRT, "BlurHorizontally");
            ExecuteTechnique(processedShadowsRT, result, "BlurVerticallyAndAttenuate");
        }

        private void ExecuteTechnique(Texture2D source, RenderTarget2D destination, string techniqueName)
        {
            ExecuteTechnique(source, destination, techniqueName, null);
        }

        private void ExecuteTechnique(Texture2D source, RenderTarget2D destination, string techniqueName, Texture2D shadowMap)
        {
            Vector2 renderTargetSize;
            renderTargetSize = new Vector2((float)baseSize, (float)baseSize);
            Neon.graphicsDevice.SetRenderTarget(destination);
            Neon.graphicsDevice.Clear(Color.White);
            resolveShadowsEffect.Parameters["renderTargetSize"].SetValue(renderTargetSize);

            if (source != null)
                resolveShadowsEffect.Parameters["InputTexture"].SetValue(source);
            if (shadowMap != null)
                resolveShadowsEffect.Parameters["ShadowMapTexture"].SetValue(shadowMap);

            resolveShadowsEffect.CurrentTechnique = resolveShadowsEffect.Techniques[techniqueName];

            foreach (EffectPass pass in resolveShadowsEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                quadRender.Render(Vector2.One * -1, Vector2.One);
            }

            Neon.graphicsDevice.SetRenderTarget(null);
        }

        private void ApplyHorizontalReduction(RenderTarget2D source, RenderTarget2D destination)
        {
            int step = reductionChainCount - 1;

            RenderTarget2D s = source;
            RenderTarget2D d = reductionRT[step];
            reductionEffect.CurrentTechnique = reductionEffect.Techniques["HorizontalReduction"];

            while (step >= 0)
            {
                d = reductionRT[step];

                Neon.graphicsDevice.SetRenderTarget(d);
                Neon.graphicsDevice.Clear(Color.White);

                reductionEffect.Parameters["SourceTexture"].SetValue(s);

                Vector2 textureDim = new Vector2(1.0f / (float)s.Width, 1.0f / (float)s.Height);
                reductionEffect.Parameters["TextureDimensions"].SetValue(textureDim);

                foreach (EffectPass pass in reductionEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    quadRender.Render(Vector2.One * -1, new Vector2(1, 1));
                }

                Neon.graphicsDevice.SetRenderTarget(null);
                s = d;
                step--;
            }

            Neon.graphicsDevice.SetRenderTarget(destination);
            reductionEffect.CurrentTechnique = reductionEffect.Techniques["Copy"];
            reductionEffect.Parameters["SourceTexture"].SetValue(d);

            foreach (EffectPass pass in reductionEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                quadRender.Render(Vector2.One * -1, new Vector2(1, 1));
            }

            reductionEffect.Parameters["SourceTexture"].SetValue(reductionRT[reductionChainCount - 1]);
            Neon.graphicsDevice.SetRenderTarget(null);
        }

    }
}
