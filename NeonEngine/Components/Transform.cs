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

        Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set 
            { 
                position = value;
                if (PositionChanged != null)
                    PositionChanged();
            }
        }

        public float rotation;
        public float Rotation
        {
            get { return (float)(rotation * (180f / Math.PI)); }
            set { rotation = (float)(value / (180f / Math.PI)); }
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
    }
}
