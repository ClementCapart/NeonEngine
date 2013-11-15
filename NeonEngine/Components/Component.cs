using Microsoft.Xna.Framework;
using NeonEngine.Private;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

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

        public virtual void PreUpdate(GameTime gameTime)
        {
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void PostUpdate(GameTime gameTime)
        {
        }

        public virtual void OnTrigger(Entity trigger, Entity triggeringEntity, object[] parameters = null)
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
