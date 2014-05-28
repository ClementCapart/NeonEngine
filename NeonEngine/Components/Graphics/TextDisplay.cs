using Microsoft.Xna.Framework;
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
        private bool _useRightCentering = false;

        public bool UseRightCentering
        {
            get { return _useRightCentering; }
            set { _useRightCentering = value; }
        }

        private bool _useTextCentering = false;

        public bool UseTextCentering
        {
            get { return _useTextCentering; }
            set { _useTextCentering = value; }
        }

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
                Vector2 size = Vector2.Zero;
                if (_useRightCentering)
                {
                    size = _font.MeasureString(_text);
                }
                else if(_useTextCentering)
                {
                    size = _font.MeasureString(_text) / 2;
                }

                if (_outline)
                {
                    Color colorToUse = Color.Lerp(Color.Transparent, _outlineColor, Opacity);
                    spriteBatch.DrawString(_font, _text, -size + new Vector2((entity.transform.Position.X + (CurrentSide == Side.Right ? Offset.X : -Offset.X) + _parallaxPosition.X + _outlineDisplacement), (entity.transform.Position.Y + Offset.Y + _parallaxPosition.Y)), colorToUse, 0.0f, Vector2.Zero, 1, SpriteEffects.None, Layer - 0.001f);
                    spriteBatch.DrawString(_font, _text, -size + new Vector2((entity.transform.Position.X + (CurrentSide == Side.Right ? Offset.X : -Offset.X) + _parallaxPosition.X - _outlineDisplacement), (entity.transform.Position.Y + Offset.Y + _parallaxPosition.Y)), colorToUse, 0.0f, Vector2.Zero, 1, SpriteEffects.None, Layer - 0.001f);
                    spriteBatch.DrawString(_font, _text, -size + new Vector2((entity.transform.Position.X + (CurrentSide == Side.Right ? Offset.X : -Offset.X) + _parallaxPosition.X + _outlineDisplacement), (entity.transform.Position.Y + Offset.Y + _parallaxPosition.Y + _outlineDisplacement)), colorToUse, 0.0f, Vector2.Zero, 1, SpriteEffects.None, Layer - 0.001f);
                    spriteBatch.DrawString(_font, _text, -size + new Vector2((entity.transform.Position.X + (CurrentSide == Side.Right ? Offset.X : -Offset.X) + _parallaxPosition.X + _outlineDisplacement), (entity.transform.Position.Y + Offset.Y + _parallaxPosition.Y - _outlineDisplacement)), colorToUse, 0.0f, Vector2.Zero, 1, SpriteEffects.None, Layer - 0.001f);
                    spriteBatch.DrawString(_font, _text, -size + new Vector2((entity.transform.Position.X + (CurrentSide == Side.Right ? Offset.X : -Offset.X) + _parallaxPosition.X - _outlineDisplacement), (entity.transform.Position.Y + Offset.Y + _parallaxPosition.Y - _outlineDisplacement)), colorToUse, 0.0f, Vector2.Zero, 1, SpriteEffects.None, Layer - 0.001f);
                    spriteBatch.DrawString(_font, _text, -size + new Vector2((entity.transform.Position.X + (CurrentSide == Side.Right ? Offset.X : -Offset.X) + _parallaxPosition.X - _outlineDisplacement), (entity.transform.Position.Y + Offset.Y + _parallaxPosition.Y + _outlineDisplacement)), colorToUse, 0.0f, Vector2.Zero, 1, SpriteEffects.None, Layer - 0.001f);
                    spriteBatch.DrawString(_font, _text, -size + new Vector2((entity.transform.Position.X + (CurrentSide == Side.Right ? Offset.X : -Offset.X) + _parallaxPosition.X), (entity.transform.Position.Y + Offset.Y + _parallaxPosition.Y + _outlineDisplacement)), colorToUse, 0.0f, Vector2.Zero, 1, SpriteEffects.None, Layer - 0.001f);
                    spriteBatch.DrawString(_font, _text, -size + new Vector2((entity.transform.Position.X + (CurrentSide == Side.Right ? Offset.X : -Offset.X) + _parallaxPosition.X), (entity.transform.Position.Y + Offset.Y + _parallaxPosition.Y - _outlineDisplacement)), colorToUse, 0.0f, Vector2.Zero, 1, SpriteEffects.None, Layer - 0.001f);
                }
                spriteBatch.DrawString(_font, _text, -size + (new Vector2((entity.transform.Position.X + (CurrentSide == Side.Right ? Offset.X : -Offset.X) + _parallaxPosition.X), (entity.transform.Position.Y + Offset.Y + _parallaxPosition.Y))), Color.Lerp(Color.Transparent, _textColor, Opacity), 0.0f, Vector2.Zero, 1, SpriteEffects.None, Layer);
            }
            
            base.Draw(spriteBatch);
        }
    }
}
