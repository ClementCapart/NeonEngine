using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine.Components.Camera
{
    public class CameraBound : Component
    {
        Side _boundSide;

        public Side BoundSide
        {
            get { return _boundSide; }
            set { _boundSide = value; }
        }

        private float _boundStrength = 1.0f;

        public float BoundStrength
        {
            get { return _boundStrength; }
            set { _boundStrength = value; }
        }

        private float _softBoundStrength = 1.0f;

        public float SoftBoundStrength
        {
            get { return _softBoundStrength; }
            set { _softBoundStrength = value; }
        }

        private float _strengtheningRate = 0.5f;

        public float StrengtheningRate
        {
            get { return _strengtheningRate; }
            set { _strengtheningRate = value; }
        }

        private bool _enabled = true;

        public bool Enabled
        {
            get { return _enabled; }
            set 
            { 
                _enabled = value;
                if (!_enabled && entity.GameWorld.Camera.CameraBounds.Contains(this))
                {
                    entity.GameWorld.Camera.CameraBounds.Remove(this);
                }
                else if (_enabled && !entity.GameWorld.Camera.CameraBounds.Contains(this))
                {
                    entity.GameWorld.Camera.CameraBounds.Add(this);
                }
            }
        }

        public CameraBound(Entity entity)
            :base(entity, "CameraBound")
        {
        }

        public override void Init()
        {
            base.Init();
        }

        public override void Remove()
        {
            if(_enabled)
                entity.GameWorld.Camera.CameraBounds.Remove(this);
            base.Remove();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (_boundStrength < 1.0f)
                _boundStrength += (float)gameTime.ElapsedGameTime.TotalSeconds * _strengtheningRate;

            if (_boundStrength > 1.0f)
                _boundStrength = 1.0f;

            base.Update(gameTime);
        }
    }
}
