using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NeonEngine
{
    public class SpriteSheet : DrawableComponent
    {
        public bool isPlaying = true;
        Rectangle[] frames;
        public int currentFrame;
        public SpriteSheetInfo spriteSheetInfo;
        float timePerFrame;
        float frameTimer = 0f;
        public SpriteEffects spriteEffects = SpriteEffects.None;
        public bool isHUD;
        public Vector2 Position;
        public bool IsLooped = true;
        public float opacity = 1f;

        private string spriteSheetTag;
        public string SpriteSheetTag
        {
            get { return spriteSheetTag; }
            set
            {
                spriteSheetTag = value;
                spriteSheetInfo = AssetManager.GetSpriteSheet(value);
                this.Init();
            }
        }

        public DrawLayer drawLayer
        {
            get { return this.DrawType; }
            set { ChangeLayer(value); }
        }

        public SpriteSheet(SpriteSheetInfo ssi, DrawLayer drawType, Entity entity)
            :this(entity)
        {
            this.DrawType = drawType;
            spriteSheetInfo = ssi;
            Init();
        }

        public SpriteSheet(Entity entity)
            :base(DrawLayer.None, entity, "Spritesheet")
        {
            this.entity = entity;
        }

        public SpriteSheet(SpriteSheetInfo ssi, DrawLayer drawType, Vector2 Position, Entity entity)
            :base(drawType, entity, "Spritesheet")
        {
            this.spriteSheetInfo = ssi;
            this.Position = Position;
            isHUD = true;
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

            currentFrame = 0;
            if (spriteSheetInfo.FPS == 0)
                isPlaying = false;
            else
                timePerFrame = 1 / spriteSheetInfo.FPS;
        }

        public override void Update(GameTime gameTime = null)
        {
            if (isPlaying)
            {
                if (frameTimer > timePerFrame)
                {
                    if (currentFrame == frames.Length - 1)
                    {
                        if (IsLooped)
                            currentFrame = 0;
                        else
                            this.isPlaying = false;
                    }
                    else
                        currentFrame++;

                    frameTimer = 0f; 
                }
                else 
                    frameTimer += gameTime != null ? (float)gameTime.ElapsedGameTime.TotalSeconds : 0.020f;
            }
        }

        public void SetFrame(int frame)
        {
            currentFrame = frame;
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            if(!isHUD)
                spritebatch.Draw(spriteSheetInfo.Texture, new Vector2((int)entity.transform.Position.X + (spriteEffects == SpriteEffects.None ? (int)spriteSheetInfo.Offset.X : -(int)spriteSheetInfo.Offset.X), (int)entity.transform.Position.Y + (int)spriteSheetInfo.Offset.Y), frames[currentFrame],
                    Color.Lerp(Color.Transparent, Color.White, opacity), entity.transform.rotation, new Vector2(spriteSheetInfo.FrameWidth / 2, spriteSheetInfo.FrameHeight / 2), entity.transform.Scale, spriteEffects, 0);
            else
                spritebatch.Draw(spriteSheetInfo.Texture, new Rectangle((int)Position.X, (int)Position.Y, spriteSheetInfo.FrameWidth, spriteSheetInfo.FrameHeight), frames[currentFrame],
                    Color.White, entity.transform.rotation, new Vector2(spriteSheetInfo.FrameWidth / 2, spriteSheetInfo.FrameHeight / 2), spriteEffects, 0);
        }
    }
}
