using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine
{
    public class SpritesheetManager : DrawableComponent
    {
        public Dictionary<string, SpriteSheet> SpritesheetList;
        public SpriteSheet CurrentSpritesheet;
        public string CurrentSpritesheetName;

        public Dictionary<string, SpriteSheet> Spritesheets
        {
            get { return SpritesheetList; }
            set
            { 
                SpritesheetList = value;
                foreach (KeyValuePair<string, SpriteSheet> kvp in SpritesheetList)
                    kvp.Value.Init();
                if(SpritesheetList.Count > 0)
                CurrentSpritesheet = SpritesheetList.ElementAt(0).Value;
            }
        }

        public DrawLayer drawLayer
        {
            get { return this.DrawType; }
            set { ChangeLayer(value); }
        }

        public SpritesheetManager(Entity entity)
            :base(DrawLayer.None, entity, "SpritesheetManager")
        {
            
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            CurrentSpritesheet.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            CurrentSpritesheet.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }

        public bool IsFinished()
        {
            if (!(CurrentSpritesheet.IsLooped) && CurrentSpritesheet.currentFrame == CurrentSpritesheet.spriteSheetInfo.FrameCount - 1)
                return true;

            return false;
        }

        public void ChangeAnimation(string spriteSheetName, bool IsPlaying = true, bool Reset = false, bool Loop = true, int StartingFrame = -1)
        {
            SpriteSheet spritesheet = SpritesheetList[spriteSheetName];

            if (CurrentSpritesheet != null)
                spritesheet.spriteEffects = CurrentSpritesheet.spriteEffects;

            CurrentSpritesheet = spritesheet;
            CurrentSpritesheetName = spriteSheetName;
            CurrentSpritesheet.isPlaying = IsPlaying;
            if (Reset)
                CurrentSpritesheet.SetFrame(0);
            if (StartingFrame != -1 && StartingFrame < CurrentSpritesheet.spriteSheetInfo.FrameCount && StartingFrame >= 0)
                CurrentSpritesheet.SetFrame(StartingFrame);
            CurrentSpritesheet.IsLooped = Loop;
        }
            
    }
}
