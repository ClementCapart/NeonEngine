using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NeonEngine;
using System;

namespace NeonStarEditor
{
    public class EditorSpriteSheet
    {
        public bool isPlaying = true;
        public int currentFrame = 0;
        public SpriteSheetInfo spriteSheetInfo;
        public double TimePerFrame;
        double frameTimer = 0f;
        public SpriteEffects spriteEffects = SpriteEffects.None;
        public bool isHUD;
        public Vector2 Position;
        public bool IsLooped = true;
        public float opacity = 1f;
        private bool _reverseLoop = false;
        private bool _reverse = false;

        public bool Reverse
        {
            get { return _reverse; }
            set { _reverse = value; }
        }
        public bool Active = true;   
  

        public bool ReverseLoop
        {
            get { return _reverseLoop; }
            set { _reverseLoop = value; }
        }

        public float scale = 1.0f;

        public EditorSpriteSheet(SpriteSheetInfo ssi, float Layer)
        {
            spriteSheetInfo = ssi;
            Init();
        }

        public void Init()
        {
            if (spriteSheetInfo == null)
                return;

            if (spriteSheetInfo.Fps == 0)
                isPlaying = false;
            else
                TimePerFrame = 1 / spriteSheetInfo.Fps;
            
            frameTimer = 0;
        }

        public void Update(TimeSpan deltaTime)
        {
            if (spriteSheetInfo != null && spriteSheetInfo.Frames != null)
            {
                    frameTimer += deltaTime.TotalSeconds;

                    while (frameTimer > TimePerFrame)
                    {
                        if (isPlaying)
                        {
                            if (!_reverseLoop)
                            {
                                if (!_reverse)
                                {
                                    if (currentFrame == spriteSheetInfo.Frames.Length - 1)
                                    {
                                        if (IsLooped)
                                            currentFrame = 0;
                                        else
                                        {
                                            isPlaying = false;
                                            
                                        }
                                    }
                                    else
                                        currentFrame++;
                                }
                                else
                                {
                                    if (currentFrame == 0)
                                    {
                                        if (IsLooped)
                                            currentFrame = spriteSheetInfo.Frames.Length - 1;
                                        else
                                        {
                                            isPlaying = false;
                                            
                                        }
                                    }
                                    else
                                        currentFrame--;
                                }
                                
                            }
                            else
                            {
                                if (!_reverse)
                                {
                                    if (currentFrame == spriteSheetInfo.Frames.Length - 1)
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

                        frameTimer -= TimePerFrame;
                    }                  
            }
        }

        public void SetFrame(int frame)
        {
            currentFrame = frame;
        }

        public float Zoom = 2.0f;
        public void Draw(SpriteBatch spritebatch)
        {
            if (spriteSheetInfo != null && spriteSheetInfo.Frames != null && Active)
            {
                if (currentFrame >= 0 && currentFrame <= spriteSheetInfo.Frames.Length - 1)
                {
                    spritebatch.Draw(spriteSheetInfo.Frames[currentFrame], Position, null,
                        Color.White, 0.0f, new Vector2(spriteSheetInfo.FrameWidth / 2, spriteSheetInfo.FrameHeight / 2), Zoom, SpriteEffects.None, 1.0f);
                }
            }
               
        }
    }
}
