using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine
{
    public delegate void ComponentRemoved(object sender, EventArgs e);

    public abstract class Component
    {
        public Entity entity;
        public string Name;
        public event ComponentRemoved Removed;
        public int ID;

        public Type[] RequiredComponents;

        public Component(Entity entity, string Name)
        {
            this.entity = entity;
            this.Name = Name;
        }

        public virtual void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
        }

        public virtual void Init()
        {

        }

        public virtual void Remove()
        {
            if (Removed != null)
                Removed(this, null);
            entity.Components.Remove(this);
        }
    }
}
