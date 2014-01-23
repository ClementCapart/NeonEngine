using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    public class FireGauge : DrawableComponent
    {
        #region Properties
        #endregion

        private ElementSystem _elementSystem;

        private Texture2D _gaugeTexture;
        private Texture2D _cursorTexture;
        private Texture2D _segmentTexture;
        private Texture2D _orangePartTexture;
        private Texture2D _yellowPartTexture;

        private float _cursorXPosition = 0.0f;

        private float _yellowPartWidth;
        private float _orangePartWidth;
        private float _redPartWidth;

        public FireGauge(Entity entity)
            :base(1.0f, entity, "FireGauge")
        {
        }

        public override void Init()
        {
            _elementSystem = entity.GetComponent<ElementSystem>();

            _gaugeTexture = AssetManager.GetTexture("HUDBarFire");
            _cursorTexture = AssetManager.GetTexture("HUDCursorFire");

            _segmentTexture = new Texture2D(Neon.GraphicsDevice, 1, _gaugeTexture.Height + 1);
            Color[] fill = new Color[_gaugeTexture.Height + 1];
            for (int i = 0; i < fill.Length - 1; i++)
                fill[i] = Color.White;
            _segmentTexture.SetData(fill);
            
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (_elementSystem.CurrentElementEffect != null && _elementSystem.CurrentElementEffect.GetType() == typeof(Fire) && entity != null)
            {
                _cursorXPosition = (float)(_elementSystem.CurrentElementEffect as Fire).CurrentCharge * (_gaugeTexture.Width / 100.0f) * entity.transform.Scale;
                _yellowPartWidth = _gaugeTexture.Width / 100.0f * (float)(_elementSystem.CurrentElementEffect as Fire).StageTwoThreshold;
                _orangePartWidth = (_gaugeTexture.Width / 100.0f * (float)(_elementSystem.CurrentElementEffect as Fire).StageThreeThreshold) - _yellowPartWidth;
                _redPartWidth = (_gaugeTexture.Width - _orangePartWidth - _yellowPartWidth);
            }
            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            if (_elementSystem.CurrentElementEffect != null && _elementSystem.CurrentElementEffect.GetType() == typeof(Fire) && entity != null)
            {
                spriteBatch.Draw(_segmentTexture, entity.transform.Position + Offset + new Vector2(-_gaugeTexture.Width + _yellowPartWidth, 0), new Rectangle(0, 0, (int)_yellowPartWidth, (int)_gaugeTexture.Height), Color.Yellow, entity.transform.Rotation, new Vector2(_yellowPartWidth / 2, _gaugeTexture.Height / 2), entity.transform.Scale, SpriteEffects.None, Layer);
                spriteBatch.Draw(_segmentTexture, entity.transform.Position + Offset + new Vector2(-_gaugeTexture.Width + _yellowPartWidth * 2 + _orangePartWidth, 0), new Rectangle(0, 0, (int)_orangePartWidth + 1, (int)_gaugeTexture.Height), Color.Orange, entity.transform.Rotation, new Vector2(_orangePartWidth / 2, _gaugeTexture.Height / 2), entity.transform.Scale, SpriteEffects.None, Layer);
                spriteBatch.Draw(_segmentTexture, entity.transform.Position + Offset + new Vector2(-_gaugeTexture.Width + _yellowPartWidth * 2 + _orangePartWidth * 2 + _redPartWidth, 0), new Rectangle(0, 0, (int)_redPartWidth, (int)_gaugeTexture.Height), Color.Red, entity.transform.Rotation, new Vector2(_redPartWidth / 2, _gaugeTexture.Height / 2), entity.transform.Scale, SpriteEffects.None, Layer);
               
                spriteBatch.Draw(_gaugeTexture, entity.transform.Position + Offset, null, Color.White, entity.transform.rotation, new Vector2(_gaugeTexture.Width / 2, _gaugeTexture.Height / 2), entity.transform.Scale, SpriteEffects.None, Layer);
                spriteBatch.Draw(_cursorTexture, entity.transform.Position + Offset + new Vector2(-_gaugeTexture.Width, 0) + new Vector2(_cursorXPosition, 0), null, Color.White, entity.transform.rotation, new Vector2(_cursorTexture.Width / 2, _cursorTexture.Height / 2), entity.transform.Scale, SpriteEffects.None, Layer);
            }
            base.Draw(spriteBatch);
        }


    }
}
