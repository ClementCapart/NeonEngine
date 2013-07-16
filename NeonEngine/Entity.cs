using Microsoft.Xna.Framework;
using NeonEngine.Private;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine
{
    public class Entity
    {
        public List<Component> Components = new List<Component>();

        public Transform transform;

        public Rigidbody rigidbody;
        public SpriteSheet spritesheet;
        public Graphic graphic;
        public Hitbox hitbox;

        private string name = "Entity";
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public bool toDestroy;

        public World containerWorld;

        public Entity(World containerWorld)
        {
            this.containerWorld = containerWorld;
            transform = AddComponent<Transform>(new Transform(this));
        }

        public virtual void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            foreach (Component c in Components)
                c.Update(gameTime);
        }

        public virtual void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {

        }

        public T AddComponent<T>(T component)
            where T : Component
        {
            Components.Add(component);
            if (component is DrawableComponent)
            {
                DrawableComponent dc = (component as DrawableComponent);
                dc.ChangeLayer(dc.DrawType);
            }
            return component;
        }

        public virtual void Destroy()
        {         
            for(int i = Components.Count - 1; i >= 0; i--)
                Components[i].Remove();
            containerWorld.RemoveEntity(this);
        }

        public T GetComponent<T>(bool CheckForSubClasses = true)
            where T : Component
        {
            T component = null;
            foreach (Component comp in this.Components)
                if (Object.ReferenceEquals(comp.GetType(), typeof(T)))
                    component = comp as T;
                else if (CheckForSubClasses && comp.GetType().IsSubclassOf(typeof(T)))
                    component = comp as T;

            return component;
        }

        public int GetLastID()
        {
            return Components[Components.Count - 1].ID;
        }
    }
}
