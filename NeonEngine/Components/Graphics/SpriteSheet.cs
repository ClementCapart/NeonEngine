using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace NeonEngine.Components.Graphics2D
{
    public class SpriteSheet : DrawableComponent
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


        private float _startingFrame = 0.0f;

        public float StartingFrame
        {
            get { return _startingFrame; }
            set 
            {
                if (spriteSheetInfo != null)
                {
                    if (value > spriteSheetInfo.FrameCount - 1)
                        _startingFrame = 0.0f;
                    else
                        _startingFrame = value;
                }
                else
                    _startingFrame = value;
            }
        }

        public bool Reverse
        {
            get { return _reverse; }
            set { _reverse = value; }
        }
        public bool Active = true;


        private bool _changeSideEveryLoop = false;

        public bool ChangeSideEveryLoop
        {
            get { return _changeSideEveryLoop; }
            set { _changeSideEveryLoop = value; }
        }

        public bool ReverseLoop
        {
            get { return _reverseLoop; }
            set { _reverseLoop = value; }
        }


        private float _delayBeforeLoopAgain = 0.0f;

        public float DelayBeforeLoopAgain
        {
          get { return _delayBeforeLoopAgain; }
          set { _delayBeforeLoopAgain = value; }
        }

        private bool _invisibleDuringDelay = false;

        public bool InvisibleDuringDelay
        {
            get { return _invisibleDuringDelay; }
            set { _invisibleDuringDelay = value; }
        }

        private bool _delayBeforeLoop = false;

        public bool DelayBeforeLoop
        {
            get { return _delayBeforeLoop; }
            set { _delayBeforeLoop = value; }
        }

        private string spriteSheetTag;
        public string SpriteSheetTag
        {
            get { return spriteSheetTag; }
            set
            {
                currentFrame = (int)_startingFrame;
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

        private float _delayBeforeLoopAgainTimer = 0.0f;
        private bool _isInDelayBeforeLoop = false;

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
            : base(0.5f, null, "Spritesheet")
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
            currentFrame = (int)_startingFrame;

            if (spriteSheetInfo == null)
                return;

            if (spriteSheetInfo.Fps == 0)
                isPlaying = false;
            else
                TimePerFrame = 1 / spriteSheetInfo.Fps;
            
            frameTimer = 0;
            _isFinished = false;
            base.Init();
        }

        public override void Update(GameTime gameTime = null)
        {       
            if (_isInDelayBeforeLoop)
            {
                if(_invisibleDuringDelay)
                    Active = false;
                this._delayBeforeLoopAgainTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (this._delayBeforeLoopAgainTimer <= 0.0f)
                {
                    if (ReverseLoop)
                    {
                        if (_reverse)
                        {
                            _reverse = false;
                            currentFrame++;
                        }
                        else
                        {
                            _reverse = true;
                            currentFrame--;
                        }

                        if (_changeSideEveryLoop)
                        {
                            if (CurrentSide == Side.Left)
                                CurrentSide = Side.Right;
                            else if (CurrentSide == Side.Right)
                                CurrentSide = Side.Left;
                        }

                        this._delayBeforeLoopAgainTimer = 0.0f;                 
                        _isInDelayBeforeLoop = false;
                        if(_invisibleDuringDelay)
                            Active = true;
                    }
                    else
                    {
                        if (_reverse)
                            currentFrame--;
                        else
                            currentFrame++;

                        if (_changeSideEveryLoop)
                        {
                            if (CurrentSide == Side.Left)
                                CurrentSide = Side.Right;
                            else if (CurrentSide == Side.Right)
                                CurrentSide = Side.Left;
                        }

                        this._delayBeforeLoopAgainTimer = 0.0f;
                        _isInDelayBeforeLoop = false;
                        if(_invisibleDuringDelay)
                            Active = true;
                    }
                }

                if (spriteSheetInfo != null)
                {
                    currentFrame = (int)currentFrame < 0 ? spriteSheetInfo.Frames.Length - 1 : currentFrame;
                    currentFrame = (int)currentFrame > spriteSheetInfo.Frames.Length - 1 ? 0 : currentFrame;
                }
                return;
            }


            if (spriteSheetInfo != null && spriteSheetInfo.Frames != null)
            {
                    frameTimer += gameTime != null ? gameTime.ElapsedGameTime.TotalSeconds : 0.020f;

                    while (frameTimer > TimePerFrame)
                    {
                        if (isPlaying)
                        {
                            if (!_reverseLoop)
                            {
                                if (!_reverse)
                                {
                                    if (currentFrame == ((_startingFrame - 1 < 0 ? spriteSheetInfo.Frames.Length - 1 : _startingFrame - 1)))
                                    {
                                        if (IsLooped)
                                        {
                                            if (DelayBeforeLoop)
                                            {
                                                _isInDelayBeforeLoop = true;
                                                _delayBeforeLoopAgainTimer = _delayBeforeLoopAgain;
                                            }
                                            else
                                            {
                                                currentFrame++;
                                                if (_changeSideEveryLoop)
                                                {
                                                    if (CurrentSide == Side.Left)
                                                        CurrentSide = Side.Right;
                                                    else if (CurrentSide == Side.Right)
                                                        CurrentSide = Side.Left;
                                                }
                                            }                                           
                                        }
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
                                    if (currentFrame == (_startingFrame + 1 > spriteSheetInfo.Frames.Length - 1 ? 0 : _startingFrame + 1))
                                    {
                                        if (IsLooped)
                                        {
                                            if (DelayBeforeLoop)
                                            {
                                                _isInDelayBeforeLoop = true;
                                                _delayBeforeLoopAgainTimer = _delayBeforeLoopAgain;
                                            }
                                            else
                                            {
                                                currentFrame--;
                                                if (_changeSideEveryLoop)
                                                {
                                                    if (CurrentSide == Side.Left)
                                                        CurrentSide = Side.Right;
                                                    else if (CurrentSide == Side.Right)
                                                        CurrentSide = Side.Left;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            isPlaying = false;
                                            _isFinished = true;
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
                                        if (DelayBeforeLoop)
                                        {
                                            _isInDelayBeforeLoop = true;
                                            _delayBeforeLoopAgainTimer = _delayBeforeLoopAgain;
                                        }
                                        else
                                        {
                                            _reverse = true;
                                            currentFrame--;
                                        }
                                    }
                                    else
                                        currentFrame++;
                                }
                                else
                                {
                                    if (currentFrame == 0)
                                    {
                                        if (DelayBeforeLoop)
                                        {
                                            _isInDelayBeforeLoop = true;
                                            _delayBeforeLoopAgainTimer = _delayBeforeLoopAgain;
                                        }
                                        else
                                        {
                                            _reverse = false;
                                            currentFrame++;
                                        }
                                    }
                                    else
                                        currentFrame--;
                                }
                            }
                        }

                        frameTimer -= TimePerFrame;
                    }                  
            }

            if (spriteSheetInfo != null)
            {
                currentFrame = (int)currentFrame < 0 ? spriteSheetInfo.Frames.Length - 1 : currentFrame;
                currentFrame = (int)currentFrame > spriteSheetInfo.Frames.Length - 1 ? 0 : currentFrame;
            }

            
            base.Update(gameTime);
        }

        public void SetFrame(int frame)
        {
            currentFrame = frame;
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            if (spriteSheetInfo != null && spriteSheetInfo.Frames != null && Active)
            {
                if (particle == null)
                {
                    if (currentFrame >= 0 && currentFrame <= spriteSheetInfo.Frames.Length - 1)
                    {
                        CurrentEffect.CurrentTechnique.Passes[0].Apply();
                        spritebatch.Draw(spriteSheetInfo.Frames[currentFrame], new Vector2((int)(entity.transform.Position.X + this._parallaxPosition.X + ((CurrentSide == Side.Right ? (int)spriteSheetInfo.Offset.X + (int)Offset.X : -(int)spriteSheetInfo.Offset.X - (int)Offset.X) * entity.transform.Scale)), (int)(entity.transform.Position.Y + this._parallaxPosition.Y + (((int)spriteSheetInfo.Offset.Y + Offset.Y) * entity.transform.Scale))), null,
                            Color.Lerp(Color.Transparent, Tint ? Color.Lerp(Color.White, TintColor, 0.5f) : MainColor, opacity), RotationOffset, new Vector2(spriteSheetInfo.FrameWidth / 2, spriteSheetInfo.FrameHeight / 2) + RotationCenter, entity.transform.Scale, CurrentSide == Side.Right ? SpriteEffects.None : SpriteEffects.FlipHorizontally, Layer);
                    }
                    else
                    {
                        Console.WriteLine(currentFrame.ToString());
                    }
                }
                else
                {
                    spritebatch.Draw(spriteSheetInfo.Frames[currentFrame], particle.Position, null,
                        TintColor, particle.Angle, new Vector2(spriteSheetInfo.FrameWidth / 2, spriteSheetInfo.FrameHeight / 2), scale, spriteEffects, Layer);
                }
            }
            base.Draw(spritebatch);
        }
    }
}
