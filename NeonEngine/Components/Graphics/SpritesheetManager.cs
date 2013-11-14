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
        public bool Active = true;

        public override Side CurrentSide
        {
            get
            {
                return base.CurrentSide;
            }
            set
            {
                ChangeSide(value);
            }
        }

        public override Vector2 Offset
        {
            get
            {
                return CurrentSpritesheet.Offset;
            }
            set
            {
                CurrentSpritesheet.Offset = value;
            }
        }

        public override Color TintColor
        {
            get
            {
                return CurrentSpritesheet.TintColor;
            }
            set
            {
                CurrentSpritesheet.TintColor = value;
            }
        }

        public Dictionary<string, SpriteSheetInfo> Spritesheets
        {
            get { return SpritesheetList; }
            set { SpritesheetList = value;  }
        }

        public float DrawLayer
        {
            get { return Layer; }
            set 
            { 
                Layer = value;
                if (CurrentSpritesheet != null)
                {
                    CurrentSpritesheet.DrawLayer = value;
                }
            }
        }

        public SpritesheetManager(Entity entity)
            :base(0, entity, "SpritesheetManager")
        {
            CurrentSpritesheet = new SpriteSheet(entity);
        }

        public override void Update(GameTime gameTime)
        {
            if (Active)
            {
                if (CurrentSpritesheet != null)
                    CurrentSpritesheet.Update(gameTime);
            }
            
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Active)
            {
                if (CurrentSpritesheet != null)
                {
                    CurrentSpritesheet.Draw(spriteBatch);
                }
            }
            
            base.Draw(spriteBatch);
        }

        public bool IsFinished()
        {
            if (CurrentSpritesheet.IsFinished)
                return true;

            return false;
        }

        public void ChangeSide(Side side)
        {
            if (_currentSide != side)
            {
                _currentSide = side;
                if( _currentSide == Side.Left)
                    CurrentSpritesheet.spriteEffects = SpriteEffects.FlipHorizontally;
                else if (_currentSide == Side.Right)
                    CurrentSpritesheet.spriteEffects = SpriteEffects.None;
            }
        }

        public void ChangeAnimation(string spriteSheetName, int priority = 0, bool IsPlaying = true, bool Reset = false, bool Loop = true, int StartingFrame = -1)
        {          
            if (CurrentPriority > priority)
                return;

            if (spriteSheetName == null || spriteSheetName == "")
                return;

            if (!SpritesheetList.ContainsKey(spriteSheetName))
                return;
            if (CurrentSpritesheetName == spriteSheetName && StartingFrame == -1)
            {
                    if (Reset)
                        CurrentSpritesheet.SetFrame(0);
                CurrentSpritesheet.isPlaying = IsPlaying;
                return;
            }

            CurrentSpritesheetName = spriteSheetName;
            CurrentSpritesheet.spriteSheetInfo = SpritesheetList[spriteSheetName];
            CurrentPriority = priority;
            CurrentSpritesheet.isPlaying = IsPlaying;
            CurrentSpritesheet.SetFrame(0);
            CurrentSpritesheet.Layer = Layer;
            if (StartingFrame != -1 && StartingFrame < CurrentSpritesheet.spriteSheetInfo.FrameCount && StartingFrame >= 0)
                CurrentSpritesheet.SetFrame(StartingFrame);
            CurrentSpritesheet.IsLooped = Loop;
            CurrentSpritesheet.Init();
        }
            
    }
}
