using NeonEngine.Components.Graphics2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine.Components.Utils
{
    public class ChangeOpacity : Component
    {
        private float _opacitySpeed = 0.5f;

        public float OpacitySpeed
        {
            get { return _opacitySpeed; }
            set { _opacitySpeed = value; }
        }

        private float _opacityLowThreshold = 0.3f;

        public float OpacityLowThreshold
        {
            get { return _opacityLowThreshold; }
            set { _opacityLowThreshold = value; }
        }

        private float _opacityHighThreshold = 1.0f;

        public float OpacityHighThreshold
        {
            get { return _opacityHighThreshold; }
            set { _opacityHighThreshold = value; }
        }

        List<DrawableComponent> _drawableComponents;

        bool _goingUp = false;

        public ChangeOpacity(Entity entity)
            : base(entity, "ChangeOpacity")
        {

        }

        public override void Init()
        {
            _drawableComponents = entity.GetComponentsByInheritance<DrawableComponent>();
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            bool change = false;
            if(_drawableComponents != null)
                foreach (DrawableComponent dc in _drawableComponents)
                {
                    if (_goingUp)
                    {
                        dc.Opacity += _opacitySpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        if (dc.Opacity >= OpacityHighThreshold)
                            change = true;

                        if (change)
                            dc.Opacity = OpacityHighThreshold;
                    }
                    else
                    {
                        dc.Opacity -= _opacitySpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        if (dc.Opacity <= OpacityLowThreshold)
                            change = true;

                        if (change)
                            dc.Opacity = OpacityLowThreshold;
                    }
                }

            if (change)
                _goingUp = !_goingUp;
                    
            base.Update(gameTime);
        }


    }
}
