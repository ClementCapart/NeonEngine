using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace NeonEngine
{
    public class SpriteSheet : DrawableComponent
    {
        public bool isPlaying = true;
        Rectangle[] frames;
        public int currentFrame = 0;
        public SpriteSheetInfo spriteSheetInfo;
        double timePerFrame;
        double frameTimer = 0f;
        public SpriteEffects spriteEffects = SpriteEffects.None;
        public bool isHUD;
        public Vector2 Position;
        public bool IsLooped = true;
        public float opacity = 1f;
        private bool _reverseLoop = false;
        private bool _reverse = false;
        public bool Active = true;

        public bool ReverseLoop
        {
            get { return _reverseLoop; }
            set { _reverseLoop = value; }
        }

        private string spriteSheetTag;
        public string SpriteSheetTag
        {
            get { return spriteSheetTag; }
            set
            {
                currentFrame = 0;
                spriteSheetTag = value;
                spriteSheetInfo = AssetManager.GetSpriteSheet(value);
                Init();
            }
        }

        public float scale = 1.0f;

        public float DrawLayer
        {
            get { return Layer; }
            set { Layer = value; }
        }

        public SpriteSheet(SpriteSheetInfo ssi, float Layer, Entity entity)
            :this(entity)
        {
            DrawLayer = Layer;
            spriteSheetInfo = ssi;
            Init();
        }

        public SpriteSheet(Entity entity)
            :base(0, entity, "Spritesheet")
        {
            this.entity = entity;
        }

        private Particle particle;

        public SpriteSheet(SpriteSheetInfo ssi, float Layer, Particle particle)
            : base(0, null, "Spritesheet")
        {
            DrawLayer = Layer;
            spriteSheetInfo = ssi;
            this.particle = particle;
        }

        private bool _isFinished = false;
        public bool IsFinished
        {
            get { return _isFinished; }
            set { _isFinished = value; }
        }

        public override void Init()
        {
            if (spriteSheetInfo == null)
                return;

            int mapWidth = spriteSheetInfo.Texture.Width;
            int mapHeight = spriteSheetInfo.Texture.Height;

            int columns = mapWidth / spriteSheetInfo.FrameWidth;
            int rows = mapHeight / spriteSheetInfo.FrameWidth;

            frames = new Rectangle[spriteSheetInfo.FrameCount];

            int currentColumn = 0, currentRow = 0;
            for (int i = 0; i < spriteSheetInfo.FrameCount; i++)
            {
                frames[i] = new Rectangle(currentColumn * spriteSheetInfo.FrameWidth, currentRow * spriteSheetInfo.FrameHeight, spriteSheetInfo.FrameWidth, spriteSheetInfo.FrameHeight);

                if (currentColumn == columns - 1)
                {
                    currentColumn = 0;
                    currentRow++;
                }
                else
                    currentColumn++;
            }

            if (spriteSheetInfo.Fps == 0)
                isPlaying = false;
            else
                timePerFrame = 1 / spriteSheetInfo.Fps;
            
            isPlaying = true;
            frameTimer = 0;
            _isFinished = false;
        }

        public override void Update(GameTime gameTime = null)
        {
            if (spriteSheetInfo != null)
            {
                    frameTimer += gameTime != null ? gameTime.ElapsedGameTime.TotalSeconds : 0.020f;

                    while (frameTimer > timePerFrame)
                    {
                        if (isPlaying)
                        {
                            if (!_reverseLoop)
                            {
                                if (currentFrame == frames.Length - 1)
                                {
                                    if (IsLooped)
                                        currentFrame = 0;
                                    else
                                    {
                                        isPlaying = false;
                                        _isFinished = true;
                                    }
                                }
                                else
                                    currentFrame++;
                            }
                            else
                            {
                                if (!_reverse)
                                {
                                    if (currentFrame == frames.Length - 1)
                                    {
                                        _reverse = true;
                                        currentFrame--;
                                    }
                                    else
                                        currentFrame++;
                                }
                                else
                                {
                                    if (currentFrame == 0)
                                    {
                                        _reverse = false;
                                        currentFrame++;
                                    }
                                    else
                                        currentFrame--;
                                }
                            }

                        }

                        frameTimer -= timePerFrame;
                    }                  
            }
            base.Update(gameTime);
        }

        public void SetFrame(int frame)
        {
            currentFrame = frame;
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            if (spriteSheetInfo != null && Active)
            {
                if(particle == null)
                    spritebatch.Draw(spriteSheetInfo.Texture, new Vector2((int)entity.transform.Position.X + this._parallaxPosition.X +  ((spriteEffects == SpriteEffects.None ? (int)spriteSheetInfo.Offset.X + (int)Offset.X : -(int)spriteSheetInfo.Offset.X - (int)Offset.X) * entity.transform.Scale), (int)entity.transform.Position.Y + this._parallaxPosition.Y +((int)spriteSheetInfo.Offset.Y + Offset.Y * entity.transform.Scale)), frames[currentFrame],
                        Color.Lerp(Color.Transparent, TintColor, opacity), entity.transform.rotation, new Vector2(spriteSheetInfo.FrameWidth / 2, spriteSheetInfo.FrameHeight / 2), entity.transform.Scale, spriteEffects, Layer);
                else
                    spritebatch.Draw(spriteSheetInfo.Texture, particle.Position, frames[currentFrame],
                        TintColor, particle.Angle, new Vector2(spriteSheetInfo.FrameWidth / 2, spriteSheetInfo.FrameHeight / 2), scale, spriteEffects, Layer);
            }
            base.Draw(spritebatch);
        }
    }
}
