using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    public class LifeBar : DrawableComponent
    {
        private Texture2D texture;
        private Avatar _avatar;

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

        private float _currentValue;
        private float _targetValue;

        private float _currentWidth;

        public LifeBar(Entity entity)
            :base(1.0f, entity, "LifeBar")
        {
            IsHUD = true;
        }

        public override void Init()
        {
            if (_avatarName != "")
            {
                Entity avatar = Neon.world.GetEntityByName(_avatarName);
                if(avatar != null)
                    _avatar = avatar.GetComponent<Avatar>();
            }
            if (_avatar != null)
            {
                _currentValue = _avatar.CurrentHealthPoints;
                _targetValue = _avatar.CurrentHealthPoints;
            }
            
            base.Init();
        }

        public override void Update(GameTime gameTime)
        {
            if (_avatar != null)
            {
                _targetValue = _avatar.CurrentHealthPoints;

                SmoothInterpolate();

                if(texture != null)
                    _currentWidth = _currentValue / _avatar.StartingHealthPoints * texture.Width; 
            }
            
            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            if(texture != null)
                spriteBatch.Draw(texture, entity.transform.Position + Offset, new Microsoft.Xna.Framework.Rectangle(0, 0, (int)_currentWidth, texture.Height), Color.White, entity.transform.rotation, Vector2.Zero, entity.transform.Scale, SpriteEffects.None, Layer);
            base.Draw(spriteBatch);
        }

        public void SmoothInterpolate()
        {
            if (_currentValue != _targetValue && Math.Abs(_currentValue - _targetValue) > 1.0f)
                _currentValue = MathHelper.Lerp(_currentValue, _targetValue, 0.3f);
            else
                _currentValue = _targetValue;
        }
    }
}
