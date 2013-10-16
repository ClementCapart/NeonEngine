using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace NeonEngine
{
    public abstract class DrawableComponent : Component
    {
        public bool CastShadow;
        public float Layer = 0;
        private bool _tint = false;
        public ColorEmitter TintedBy;
        public bool Tint
        {
            get { return _tint; }
            set 
            {
                if (value == false)
                    TintColor = Color.White;
                _tint = value;
            }
        }

        private Color _tintColor = Color.White;

        public virtual Color TintColor
        {
            get { return _tintColor; }
            set { _tintColor = value; }
        }

        public DrawableComponent(float Layer, Entity entity, string Name)
            :base(entity, Name)
        {
            this.Layer = Layer;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
        }

        public override void Remove()
        {
            entity.containerWorld.DrawableComponents.Remove(this);
            base.Remove();
        }
    }
}
