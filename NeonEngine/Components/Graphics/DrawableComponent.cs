using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

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
