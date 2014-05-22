using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NeonEngine;
using NeonEngine.Components.Graphics2D;
using NeonEngine.Components.Text2D;
using NeonStarLibrary.Components.Avatar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.HUD
{
    public class LifeBar : DrawableComponent
    {
        private Texture2D texture;
        private AvatarCore _avatar;

        private string _avatarName = "";

        public string AvatarName
        {
            get { return _avatarName; }
            set { _avatarName = value; }
        }

        public float DrawLayer
        {
            get { return Layer; }
            set { Layer = value; }
        }

        public string graphicTag;
        public string GraphicTag
        {
            get
            {
                return graphicTag;
            }
            set
            {
                graphicTag = value;
                texture = AssetManager.GetTexture(value);
            }
        }

        private float _blinkTresholdValue = 30.0f;

        public float BlinkTresholdValue
        {
            get { return _blinkTresholdValue; }
            set { _blinkTresholdValue = value; }
        }

        private float _blinkRange = 0.1f;

        public float BlinkRange
        {
            get { return _blinkRange; }
            set { _blinkRange = value; }
        }

        private float _blinkSpeed = 1.0f;

        public float BlinkSpeed
        {
            get { return _blinkSpeed; }
            set { _blinkSpeed = value; }
        }

        private float _currentValue;
        private float _targetValue;

        private float _currentWidth;

        private float _redValue = 0.0f;
        private bool _isBlinking = false;
        private bool _blinkGoingUp = true;
        private TextDisplay _lifeDisplay;
        private float _lastFrameHealthPoints = 0.0f;
        SpriteSheet _healSpritesheet;
        
        private Color _redBlink;

        public LifeBar(Entity entity)
            :base(1.0f, entity, "LifeBar")
        {
            IsHUD = true;
        }

        public override void Init()
        {
            if (_avatarName != "")
            {
                Entity avatar = entity.GameWorld.GetEntityByName(_avatarName);
                if(avatar != null)
                    _avatar = avatar.GetComponent<AvatarCore>();
            }
            if (_avatar != null)
            {
                _currentValue = _avatar.CurrentHealthPoints;
                _targetValue = _avatar.CurrentHealthPoints;
                _lastFrameHealthPoints = _targetValue;
            }

            _lifeDisplay = entity.GetComponentByNickname("LifePoints") as TextDisplay;
            _redBlink = Color.FromNonPremultiplied(194, 48, 48, 255);

            _healSpritesheet = entity.GetComponentByNickname("HealAnimation") as SpriteSheet;
            if(_healSpritesheet != null)
                _healSpritesheet.Active = false;
            base.Init();
        }

        public override void Update(GameTime gameTime)
        {          
            if (_avatar != null)
            {
                if (_lastFrameHealthPoints < _avatar.CurrentHealthPoints)
                {
                    HealEffect();
                }
                _lastFrameHealthPoints = _avatar.CurrentHealthPoints;
                if (_avatar.CurrentHealthPoints <= _blinkTresholdValue)
                {
                    if (!_isBlinking)
                    {
                        _isBlinking = true;
                        _blinkGoingUp = true;
                        _redValue = 0.0f;
                    }
                }
                else
                {
                    _isBlinking = false;
                    _redValue = 0.0f;
                }
                    
                _targetValue = _avatar.CurrentHealthPoints;

                SmoothInterpolate();

                if(texture != null)
                    _currentWidth = _currentValue / _avatar.StartingHealthPoints * texture.Width; 
            }

            if (_isBlinking)
            {
                if (_blinkGoingUp)
                {
                    _redValue += (float)gameTime.ElapsedGameTime.TotalSeconds * _blinkSpeed;
                    if (_redValue >= 1.0f)
                    {
                        _redValue = 1.0f;
                        _blinkGoingUp = false;
                    }
                }
                else
                {
                    _redValue -= (float)gameTime.ElapsedGameTime.TotalSeconds * _blinkSpeed;
                    if (_redValue <= _blinkRange)
                    {
                        _redValue = _blinkRange;
                        _blinkGoingUp = true;
                    }
                }

                if (_lifeDisplay != null)
                {
                    _lifeDisplay.TextColor = Color.Lerp(Color.White, _redBlink, _redValue);
                }
            }
            else
            {
                if (_lifeDisplay != null)
                    _lifeDisplay.TextColor = Color.White;
            }

            if(_healSpritesheet != null)
                _healSpritesheet.Offset = new Vector2(_currentWidth - 90, -15); 
            if (_healSpritesheet != null && _healSpritesheet.IsFinished)
            {
                _healSpritesheet.Active = false;
            }
            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            if(texture != null)
                spriteBatch.Draw(texture, entity.transform.Position + Offset, new Microsoft.Xna.Framework.Rectangle(0, 0, (int)_currentWidth, texture.Height), _isBlinking ? Color.Lerp(Color.White, _redBlink, _redValue): Color.White, entity.transform.rotation, Vector2.Zero, entity.transform.Scale, SpriteEffects.None, Layer);
            base.Draw(spriteBatch);
        }

        public void SmoothInterpolate()
        {
            if (_currentValue != _targetValue && Math.Abs(_currentValue - _targetValue) > 1.0f)
                _currentValue = MathHelper.Lerp(_currentValue, _targetValue, 0.3f);
            else
                _currentValue = _targetValue;
        }

        public void HealEffect()
        {
            if (_healSpritesheet != null)
            {
                _healSpritesheet.Active = true;
                _healSpritesheet.currentFrame = 0;
                _healSpritesheet.IsFinished = false;
                _healSpritesheet.isPlaying = true;
            }
        }
    }
}
