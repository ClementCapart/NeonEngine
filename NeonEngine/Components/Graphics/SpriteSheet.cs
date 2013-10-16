using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

        public SpriteSheet(SpriteSheetInfo ssi, float Layer, Vector2 Position, Entity entity)
            :base(Layer, entity, "Spritesheet")
        {
            spriteSheetInfo = ssi;
            this.Position = Position;
            isHUD = true;
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
        }

        public void SetFrame(int frame)
        {
            currentFrame = frame;
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            if (spriteSheetInfo != null)
            {
                if (!isHUD)
                    spritebatch.Draw(spriteSheetInfo.Texture, new Vector2((int)entity.transform.Position.X + ((spriteEffects == SpriteEffects.None ? (int)spriteSheetInfo.Offset.X : -(int)spriteSheetInfo.Offset.X) * entity.transform.Scale), (int)entity.transform.Position.Y + ((int)spriteSheetInfo.Offset.Y * entity.transform.Scale)), frames[currentFrame],
                        Color.Lerp(Color.Transparent, (entity.Name == "LiOn" ? /*TOREMOVE*/Color.Gray : Color.White), opacity), entity.transform.rotation, new Vector2(spriteSheetInfo.FrameWidth / 2, spriteSheetInfo.FrameHeight / 2), entity.transform.Scale, spriteEffects, Layer);
                else
                    spritebatch.Draw(spriteSheetInfo.Texture, new Rectangle((int)Position.X, (int)Position.Y, spriteSheetInfo.FrameWidth, spriteSheetInfo.FrameHeight), frames[currentFrame],
                        Color.White, entity.transform.rotation, new Vector2(spriteSheetInfo.FrameWidth / 2, spriteSheetInfo.FrameHeight / 2), spriteEffects, Layer);
            }
        }
    }
}
