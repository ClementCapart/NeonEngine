using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NeonEngine.Components.Graphics2D
{
    public class SpritesheetManager : DrawableComponent
    {
        public Dictionary<string, SpriteSheetInfo> SpritesheetList = new Dictionary<string,SpriteSheetInfo>();
        public SpriteSheet CurrentSpritesheet;
        public string CurrentSpritesheetName;
        public int CurrentPriority;
        public bool Active = true;

        new public Effect CurrentEffect
        {
            get { return CurrentSpritesheet.CurrentEffect;  }
            set { CurrentSpritesheet.CurrentEffect = value; }
        }

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

        public override Vector2 ParallaxForce
        {
            get
            {
                return base.ParallaxForce;
            }
            set
            {
                base.ParallaxForce = value;
                if (CurrentSpritesheet != null)
                    CurrentSpritesheet.ParallaxForce = value;
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

        public override bool Tint
        {
            get
            {
                return CurrentSpritesheet.Tint;
            }
            set
            {
                CurrentSpritesheet.Tint = value;
            }
        }

        public override Vector2 RotationCenter
        {
            get
            {
                return CurrentSpritesheet.RotationCenter;
            }
            set
            {
                CurrentSpritesheet.RotationCenter = value;
            }
        }

        public override float RotationOffset
        {
            get
            {
                return CurrentSpritesheet.RotationOffset;
            }
            set
            {
                CurrentSpritesheet.RotationOffset = value;
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
            if (CurrentSpritesheet != null)
            {
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
                    if (CurrentSpritesheet != null)
                        CurrentSpritesheet._parallaxPosition = _parallaxPosition;
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
                if (_currentSide == Side.Left)
                    CurrentSpritesheet.CurrentSide = Side.Left;
                else if (_currentSide == Side.Right)
                    CurrentSpritesheet.CurrentSide = Side.Right;
            }
        }

        public void ChangeAnimation(string spriteSheetName, bool forceSwitch = false, int priority = 0, bool IsPlaying = true, bool Reset = false, bool Loop = true, int StartingFrame = -1)
        {
            if (!forceSwitch && !CurrentSpritesheet.IsLooped && !CurrentSpritesheet.IsFinished)
                return;
            
            if (CurrentPriority > priority)
                return;

            if (spriteSheetName == null || spriteSheetName == "")
                return;

            if (!SpritesheetList.ContainsKey(spriteSheetName))
                return;

            if (CurrentSpritesheetName == spriteSheetName && StartingFrame == -1 && !Reset)
            {
                    if (Reset)
                        CurrentSpritesheet.SetFrame(0);
                CurrentSpritesheet.isPlaying = IsPlaying;
                return;
            }
            CurrentSpritesheetName = spriteSheetName;
            CurrentSpritesheet.spriteSheetInfo = SpritesheetList[spriteSheetName];
            CurrentPriority = priority;
            CurrentSpritesheet.ReverseLoop = false;
            CurrentSpritesheet.isPlaying = IsPlaying;
            CurrentSpritesheet.IsFinished = false;
            CurrentSpritesheet.SetFrame(0);
            CurrentSpritesheet.Layer = Layer;
            CurrentSpritesheet.ParallaxForce = ParallaxForce;
            if(CurrentSpritesheet.spriteSheetInfo != null)
                if (StartingFrame != -1 && StartingFrame < CurrentSpritesheet.spriteSheetInfo.FrameCount && StartingFrame >= 0)
                    CurrentSpritesheet.SetFrame(StartingFrame);
            CurrentSpritesheet.IsLooped = Loop;
            CurrentSpritesheet.Init();
            CurrentSpritesheet.isPlaying = IsPlaying;
        }

        public void ChangeAnimation(string spriteSheetName, int priority = 0, bool IsPlaying = true, bool Reset = false, bool Loop = true, int StartingFrame = -1)
        {
            ChangeAnimation(spriteSheetName, false, priority, IsPlaying, Reset, Loop, StartingFrame);
        }

        public void ChangeAnimation(string spriteSheetName)
        {
            ChangeAnimation(spriteSheetName, false, 0, true, false, true, -1);
        }

        public void ChangeOpacity(float value)
        {
            CurrentSpritesheet.opacity += value;
        }          
    }


}
