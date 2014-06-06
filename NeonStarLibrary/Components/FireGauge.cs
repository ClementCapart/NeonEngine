using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NeonEngine;
using NeonEngine.Components.Graphics2D;
using NeonStarLibrary.Components.Avatar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.HUD
{
    public class FireGauge : DrawableComponent
    {
        #region Properties
        private Vector2 _gaugeOffset;

        public Vector2 GaugeOffset
        {
            get { return _gaugeOffset; }
            set { _gaugeOffset = value; }
        }
        #endregion

        private ElementSystem _elementSystem;

        private Texture2D _gaugeBackTexture;
        private Texture2D _gaugeFrontTexture;
        private Texture2D _gaugeStateTexture;
        private Texture2D _cursor;

        private float _cursorXPosition = 0.0f;

        public FireGauge(Entity entity)
            :base(1.0f, entity, "FireGauge")
        {
        }

        public override void Init()
        {
            _elementSystem = entity.GetComponent<ElementSystem>();

            _gaugeBackTexture = AssetManager.GetTexture("CrystalGaugeBack");
            _gaugeFrontTexture = AssetManager.GetTexture("CrystalGaugeFront");
            _gaugeStateTexture = AssetManager.GetTexture("CrystalGauge");
            _cursor = AssetManager.GetTexture("CrystalGaugeCursor");
            
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (_elementSystem.CurrentElementEffect != null && _elementSystem.CurrentElementEffect.GetType() == typeof(Fire) && entity != null)
            {
                _cursorXPosition = (float)(_elementSystem.CurrentElementEffect as Fire).CurrentCharge * (_gaugeStateTexture.Width / 100.0f);
            }
            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            if (_elementSystem.CurrentElementEffect != null && _elementSystem.CurrentElementEffect.GetType() == typeof(Fire) && entity != null)
            {
                spriteBatch.Draw(_gaugeBackTexture, entity.transform.Position + Offset, null, Color.White, entity.transform.rotation, new Vector2(_gaugeBackTexture.Width / 2, _gaugeBackTexture.Height / 2), entity.transform.Scale, SpriteEffects.None, Layer);
                spriteBatch.Draw(_gaugeStateTexture, entity.transform.Position + Offset + _gaugeOffset, new Rectangle(0, 0, (int)_cursorXPosition, (int)_gaugeStateTexture.Height), Color.White, entity.transform.Rotation, Vector2.Zero, entity.transform.Scale, SpriteEffects.None, Layer);
                spriteBatch.Draw(_gaugeFrontTexture, entity.transform.Position + Offset, null, Color.White, entity.transform.rotation, new Vector2(_gaugeFrontTexture.Width / 2, _gaugeFrontTexture.Height / 2), entity.transform.Scale, SpriteEffects.None, Layer);
                spriteBatch.Draw(_cursor, entity.transform.Position + Offset + _gaugeOffset + new Vector2(_cursorXPosition * entity.transform.Scale, 0) + new Vector2(-2, 4), null, Color.White, entity.transform.rotation, new Vector2(_cursor.Width / 2, _cursor.Height / 2), entity.transform.Scale, SpriteEffects.None, Layer);               
            }
            base.Draw(spriteBatch);
        }


    }
}
