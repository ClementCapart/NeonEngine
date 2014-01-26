using Microsoft.Xna.Framework;
using NeonEngine.Components.Graphics2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine.Components.VisualFX
{
    public class ColorEmitter : Component
    {
        private bool _debug = false;

        public bool Debug
        {
            get { return _debug; }
            set { _debug = value; }
        }

        private Color _color = Color.Blue;

        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        public Color currentColor;

        private float _range = 1000.0f;

        public float Range
        {
            get { return _range; }
            set { _range = value; }
        }

        private bool _pulse = false;

        public bool Pulse
        {
            get { return _pulse; }
            set { _pulse = value; }
        }

        private float _pulseStartDelay = 1.0f;

        public float PulseStartDelay
        {
            get { return _pulseStartDelay; }
            set { _pulseStartDelay = value; }
        }

        private float _pulseStartTimer = 0.0f;

        private float _pulseDuration = 2.0f;

        public float PulseDuration
        {
            get { return _pulseDuration; }
            set { _pulseDuration = value; }
        }

        private float _pulseTimer = 0.0f;
        private bool _goingUp = true;

        public ColorEmitter(Entity entity)
            :base(entity, "ColorEmitter")
        {
        }

        public override void Init()
        {
            base.Init();
        }

        public override void Update(GameTime gameTime)
        {
            if (Debug)
            {
                currentColor = Color.Lerp(Color.White, Color, Pulse ? _pulseTimer / _pulseDuration : 1);
            }
            if (_pulseStartTimer < _pulseStartDelay)
            {
                _pulseStartTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                foreach (DrawableComponent dc in entity.containerWorld.DrawableComponents.Where(dc => dc.Tint))
                {
                    float Distance = Vector2.Distance(this.entity.transform.Position, dc.entity.transform.Position);
                    if (Distance < Range)
                        dc.TintColor = Color.Lerp(dc.TintColor, Color, ((Distance / _range) - 1) * -1 * (Pulse ? _pulseTimer / _pulseDuration : 1));
                }

                if (Pulse)
                {
                    if (_goingUp)
                    {
                        if (_pulseTimer < _pulseDuration)
                        {
                            _pulseTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                        }
                        else
                        {
                            _pulseTimer = _pulseDuration;
                            _goingUp = false;
                        }
                    }
                    else
                    {
                        if (_pulseTimer > 0)
                        {
                            _pulseTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                        }
                        else
                        {
                            _pulseTimer = 0;
                            _goingUp = true;
                        }
                    }
                }
            }
            base.Update(gameTime);
        }


    }
}
