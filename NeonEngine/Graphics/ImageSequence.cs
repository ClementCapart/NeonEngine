using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace NeonEngine.Private
{
    public class ImageSequence : DrawableComponent
    {
        Texture2D[] sequence;
        int beginFrame, endFrame;
        public int currentFrame;
        float timePerFrame;
        double lastFrameChanged;

        public int Width, Height;

        public int X, Y;

        public float Scale = 1f;

        public bool isPlaying = true;
        public bool loop = false;
        public bool completed = false;

        public ImageSequence(string pathAndFileName, int beginFrame, int endFrame, int framesPerSeconds, Entity entity, DrawLayer drawLayer)
            :base(drawLayer, entity, "Image Sequence")
        {
            this.beginFrame = beginFrame;
            this.endFrame = endFrame;

            List<Texture2D> import = new List<Texture2D>();
            for (int i = beginFrame; i < (endFrame - beginFrame) + 1; i++)
                import.Add(AssetManager.GetTexture(pathAndFileName + i.ToString()));
            sequence = import.ToArray();

            Width = sequence[0].Width;
            Height = sequence[0].Height;

            currentFrame = 0;
            timePerFrame = 1000 / framesPerSeconds;
        }

        public override void Update(GameTime gameTime)
        {
            if (isPlaying)
            {
                if (gameTime.TotalGameTime.TotalMilliseconds - lastFrameChanged > timePerFrame)
                {
                    if (currentFrame == sequence.Length - 1)
                    {
                        completed = true;
                        if (loop)
                        {
                            currentFrame = 0;
                            completed = false;
                        }
                    }
                    else
                        currentFrame++;
                    lastFrameChanged = gameTime.TotalGameTime.TotalMilliseconds;
                }
            }
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(sequence[currentFrame], new Vector2(X, Y), null,
                Color.Lerp(Color.Transparent, Color.White, 1f), 0f, new Vector2(Width / 2, Height / 2), this.Scale, SpriteEffects.None, 0);
            base.Draw(spritebatch);
        }
    }
}
