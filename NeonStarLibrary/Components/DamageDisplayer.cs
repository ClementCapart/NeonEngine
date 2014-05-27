using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NeonEngine;
using NeonEngine.Components.Graphics2D;
using NeonEngine.Components.Text2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.Graphics2D
{
    public class DamageDisplayer : DrawableComponent
    {
        public class DamageDisplayInformation
        {
            public TextDisplay TextDisplayer;
            public float CurrentValue;
            public float TargetValue;
            public float DisplayDuration;
        }

        #region Properties
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

        private Color _fontColor = Color.White;

        public Color FontColor
        {
            get { return _fontColor; }
            set { _fontColor = value; }
        }

        private bool _isOutlined;

        public bool IsOutlined
        {
            get { return _isOutlined; }
            set { _isOutlined = value; }
        }

        private float _outlineWidth;

        public float OutlineWidth
        {
            get { return _outlineWidth; }
            set { _outlineWidth = value; }
        }

        private Color _outlineColor = Color.Black;

        public Color OutlineColor
        {
            get { return _outlineColor; }
            set { _outlineColor = value; }
        }

        private float _displayerDuration = 1.0f;

        public float DisplayerDuration
        {
            get { return _displayerDuration; }
            set { _displayerDuration = value; }
        }

        private float _damageDisplayLerp = 0.2f;

        public float DamageDisplayLerp
        {
            get { return _damageDisplayLerp; }
            set { _damageDisplayLerp = value; }
        }

        private float _damageMoveLerp = 0.5f;

        public float DamageMoveLerp
        {
            get { return _damageMoveLerp; }
            set { _damageMoveLerp = value; }
        }

        private Vector2 _startingOffset;

        public Vector2 StartingOffset
        {
            get { return _startingOffset; }
            set { _startingOffset = value; }
        }

        private Vector2 _targetOffset;

        public Vector2 TargetOffset
        {
            get { return _targetOffset; }
            set { _targetOffset = value; }
        }
        #endregion

        private List<DamageDisplayInformation> _displayers;

        public DamageDisplayer(Entity entity)
            :base(0.99999f, entity, "DamageDisplayer")
        {
        }

        public override void Init()
        {
            _displayers = new List<DamageDisplayInformation>();
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            for (int i = _displayers.Count - 1; i >= 0; i--)
            {
                DamageDisplayInformation ddi = _displayers[i];
                ddi.DisplayDuration -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (ddi.DisplayDuration <= 0.0f)
                    _displayers.Remove(ddi);
                else
                {
                    SmoothInterpolate(ddi);
                    ddi.TextDisplayer.Text = Math.Floor(ddi.CurrentValue).ToString();
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (DamageDisplayInformation ddi in _displayers)
            {
                ddi.TextDisplayer.Draw(spriteBatch);
            }
            base.Draw(spriteBatch);
        }

        public void DisplayDamage(float value)
        {
            if (value <= 0)
            {
                int damageValue = (int)Math.Abs(value);
                TextDisplay td = new TextDisplay(entity);
                td.DrawLayer = DrawLayer;
                td.Font = Font;
                td.TextColor = _fontColor;
                td.Offset = _startingOffset;
                td.OutlineDisplacement = OutlineWidth;
                td.OutlineColor = OutlineColor;
                td.Outline = IsOutlined;
                td.HasToBeSaved = false;

                DamageDisplayInformation ddi = new DamageDisplayInformation();
                ddi.TextDisplayer = td;
                ddi.DisplayDuration = _displayerDuration;
                ddi.CurrentValue = 0;
                ddi.TargetValue = damageValue;
                _displayers.Add(ddi);
            }
        }

        public void SmoothInterpolate(DamageDisplayInformation ddi)
        {
            if (ddi.CurrentValue != ddi.TargetValue && Math.Abs(ddi.CurrentValue - ddi.TargetValue) > 1.0f)
                ddi.CurrentValue = MathHelper.Lerp(ddi.CurrentValue, ddi.TargetValue, _damageDisplayLerp);
            else
                ddi.CurrentValue = ddi.TargetValue;

            ddi.TextDisplayer.Offset = Vector2.Lerp(ddi.TextDisplayer.Offset, _targetOffset, _damageMoveLerp);

        }


    }
}
