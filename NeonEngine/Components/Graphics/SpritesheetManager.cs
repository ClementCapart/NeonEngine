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
        public Dictionary<string, SpriteSheetInfo> SpritesheetList = new Dictionary<string,SpriteSheetInfo>();
        public SpriteSheet CurrentSpritesheet;
        public string CurrentSpritesheetName;
        public int CurrentPriority;

        public Side CurrentSide = Side.Right;

        public Dictionary<string, SpriteSheetInfo> Spritesheets
        {
            get { return SpritesheetList; }
            set { SpritesheetList = value;  }
        }

        public float DrawLayer
        {
            get { return Layer; }
            set { Layer = value; }
        }

        public SpritesheetManager(Entity entity)
            :base(0, entity, "SpritesheetManager")
        {
            CurrentSpritesheet = new SpriteSheet(entity);
        }

        public override void Update(GameTime gameTime)
        {
            if(CurrentSpritesheet != null)
                CurrentSpritesheet.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (CurrentSpritesheet != null)
                CurrentSpritesheet.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }

        public bool IsFinished()
        {
            if (!(CurrentSpritesheet.IsLooped) && CurrentSpritesheet.currentFrame == CurrentSpritesheet.spriteSheetInfo.FrameCount - 1)
                return true;

            return false;
        }

        public void ChangeSide(Side side)
        {
            if (CurrentSide != side)
            {
                CurrentSide = side;
                if (CurrentSide == Side.Left)
                    CurrentSpritesheet.spriteEffects = SpriteEffects.FlipHorizontally;
                else if (CurrentSide == Side.Right)
                    CurrentSpritesheet.spriteEffects = SpriteEffects.None;
            }
        }

        public void ChangeAnimation(string spriteSheetName, int priority = 0, bool IsPlaying = true, bool Reset = false, bool Loop = true, int StartingFrame = -1)
        {
            if (!SpritesheetList.ContainsKey(spriteSheetName))
                return;
            if (CurrentSpritesheetName == spriteSheetName && StartingFrame == -1)
                return;
            if (CurrentPriority > priority && !IsFinished())
                return;

            CurrentSpritesheet.spriteSheetInfo = SpritesheetList[spriteSheetName];

            CurrentPriority = priority;
            CurrentSpritesheetName = spriteSheetName;
            CurrentSpritesheet.isPlaying = IsPlaying;
            CurrentSpritesheet.SetFrame(0);
            if (StartingFrame != -1 && StartingFrame < CurrentSpritesheet.spriteSheetInfo.FrameCount && StartingFrame >= 0)
                CurrentSpritesheet.SetFrame(StartingFrame);
            CurrentSpritesheet.IsLooped = Loop;

            CurrentSpritesheet.Init();
        }
            
    }
}
