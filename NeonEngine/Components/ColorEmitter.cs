using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine
{
    public class ColorEmitter : Component
    {
        private Color _color = Color.Blue;

        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

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
            foreach (DrawableComponent dc in entity.containerWorld.DrawableComponents.Where(dc => dc.Tint))
            {
                float Distance = Vector2.Distance(this.entity.transform.Position, dc.entity.transform.Position);
                if(Distance < Range)           
                    dc.TintColor = Color.Lerp(Color.White, Color, ((Distance / _range) - 1) * -1 * (Pulse ? _pulseTimer / _pulseDuration : 1));
                
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
                        _goingUp = false;
                }
                else
                {
                    if (_pulseTimer > 0)
                    {
                        _pulseTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                    else
                        _goingUp = true;
                }
               
            }
             

            base.Update(gameTime);
        }


    }
}
