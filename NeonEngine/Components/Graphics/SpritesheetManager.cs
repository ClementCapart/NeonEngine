using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

        public float DrawLayer
        {
            get { return Layer; }
            set { Layer = value; }
        }

        public SpritesheetManager(Entity entity)
            :base(0, entity, "SpritesheetManager")
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            CurrentSpritesheet.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
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
