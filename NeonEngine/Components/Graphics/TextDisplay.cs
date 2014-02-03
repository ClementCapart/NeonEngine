﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NeonEngine.Components.Graphics2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine.Components.Text2D
{
    public class TextDisplay : DrawableComponent
    {
        public float DrawLayer
        {
            get { return Layer; }
            set { Layer = value; }
        }
        
        private SpriteFont _font;

        public SpriteFont Font
        {
            get { return _font; }
            set { _font = value; }
        }

        private string _text = "";

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        private Color _textColor;

        public Color TextColor
        {
            get { return _textColor; }
            set { _textColor = value; }
        }

        private bool _outline;

        public bool Outline
        {
            get { return _outline; }
            set { _outline = value; }
        }

        private Color _outlineColor;

        public Color OutlineColor
        {
            get { return _outlineColor; }
            set { _outlineColor = value; }
        }

        private float _outlineDisplacement = 0.0f;

        public float OutlineDisplacement
        {
            get { return _outlineDisplacement; }
            set { _outlineDisplacement = value; }
        }

        private bool _active = true;

        public bool Active
        {
            get { return _active; }
            set { _active = value; }
        }

        public TextDisplay(Entity entity)
            :base(1.0f, entity, "TextDisplay")
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_active)
            {
                if (_outline)
                {
                    spriteBatch.DrawString(_font, _text, new Vector2((entity.transform.Position.X + Offset.X + _parallaxPosition.X + _outlineDisplacement), (entity.transform.Position.Y + Offset.Y + _parallaxPosition.Y)), _outlineColor, entity.transform.Rotation, Vector2.Zero, 1, SpriteEffects.None, Layer - 0.001f);
                    spriteBatch.DrawString(_font, _text, new Vector2((entity.transform.Position.X + Offset.X + _parallaxPosition.X - _outlineDisplacement), (entity.transform.Position.Y + Offset.Y + _parallaxPosition.Y)), _outlineColor, entity.transform.Rotation, Vector2.Zero, 1, SpriteEffects.None, Layer - 0.001f);
                    spriteBatch.DrawString(_font, _text, new Vector2((entity.transform.Position.X + Offset.X + _parallaxPosition.X + _outlineDisplacement), (entity.transform.Position.Y + Offset.Y + _parallaxPosition.Y + _outlineDisplacement)), _outlineColor, entity.transform.Rotation, Vector2.Zero, 1, SpriteEffects.None, Layer - 0.001f);
                    spriteBatch.DrawString(_font, _text, new Vector2((entity.transform.Position.X + Offset.X + _parallaxPosition.X + _outlineDisplacement), (entity.transform.Position.Y + Offset.Y + _parallaxPosition.Y - _outlineDisplacement)), _outlineColor, entity.transform.Rotation, Vector2.Zero, 1, SpriteEffects.None, Layer - 0.001f);
                    spriteBatch.DrawString(_font, _text, new Vector2((entity.transform.Position.X + Offset.X + _parallaxPosition.X - _outlineDisplacement), (entity.transform.Position.Y + Offset.Y + _parallaxPosition.Y - _outlineDisplacement)), _outlineColor, entity.transform.Rotation, Vector2.Zero, 1, SpriteEffects.None, Layer - 0.001f);
                    spriteBatch.DrawString(_font, _text, new Vector2((entity.transform.Position.X + Offset.X + _parallaxPosition.X - _outlineDisplacement), (entity.transform.Position.Y + Offset.Y + _parallaxPosition.Y + _outlineDisplacement)), _outlineColor, entity.transform.Rotation, Vector2.Zero, 1, SpriteEffects.None, Layer - 0.001f);
                    spriteBatch.DrawString(_font, _text, new Vector2((entity.transform.Position.X + Offset.X + _parallaxPosition.X), (entity.transform.Position.Y + Offset.Y + _parallaxPosition.Y + _outlineDisplacement)), _outlineColor, entity.transform.Rotation, Vector2.Zero, 1, SpriteEffects.None, Layer - 0.001f);
                    spriteBatch.DrawString(_font, _text, new Vector2((entity.transform.Position.X + Offset.X + _parallaxPosition.X), (entity.transform.Position.Y + Offset.Y + _parallaxPosition.Y - _outlineDisplacement)), _outlineColor, entity.transform.Rotation, Vector2.Zero, 1, SpriteEffects.None, Layer - 0.001f);
                }
                spriteBatch.DrawString(_font, _text, new Vector2((entity.transform.Position.X + Offset.X + _parallaxPosition.X), (entity.transform.Position.Y + Offset.Y + _parallaxPosition.Y)), _textColor, entity.transform.Rotation, Vector2.Zero, 1, SpriteEffects.None, Layer);
            }
            
            base.Draw(spriteBatch);
        }
    }
}
