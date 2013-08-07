using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine
{
    public abstract class DrawableComponent : Component
    {
        public bool CastShadow;
        public float Layer = 0;

        public DrawableComponent(float Layer, Entity entity, string Name)
            :base(entity, Name)
        {
            this.Layer = Layer;
        }

        public virtual void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
        }

        public override void Remove()
        {
            entity.containerWorld.DrawableComponents.Remove(this);
            base.Remove();
        }
    }
}
