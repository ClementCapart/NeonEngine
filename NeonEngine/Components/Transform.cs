using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine.Private
{
    public delegate void PositionChange();

    public class Transform : Component
    {
        public event PositionChange PositionChanged;

        private bool _autoChangeInitialPosition = true;

        public bool AutoChangeInitialPosition
        {
            get { return _autoChangeInitialPosition; }
            set { _autoChangeInitialPosition = value; }
        }

        Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set 
            {
                position = value;
                if (_autoChangeInitialPosition)
                    _initialPosition = position;
                if (PositionChanged != null)
                    PositionChanged();
            }
        }

        Vector2 _initialPosition;
        public Vector2 InitialPosition
        {
            get 
            {
                return _initialPosition; 
            }
            set
            {
                _initialPosition = value;
            }
        }

        public float rotation;
        public float Rotation
        {
            get { return (float)(rotation * (180f / Math.PI)); }
            set { rotation = (float)(value / (180f / Math.PI)) % 360; }
        }

        float scale = 1f;
        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public Transform(Entity entity)
            : base(entity, "Transform")
        {
        }

        public override void Init()
        {
            this.position = InitialPosition;
            base.Init();
        }

        public override void PreUpdate(GameTime gameTime)
        {
            base.PreUpdate(gameTime);
        }
    }
}
