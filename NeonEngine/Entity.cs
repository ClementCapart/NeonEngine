using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        public SpritesheetManager spritesheets;
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

        public virtual void PreUpdate(GameTime gameTime)
        {
            for (int i = Components.Count - 1; i >= 0; i--)
                Components[i].PreUpdate(gameTime);
        }

        public virtual void Update(GameTime gameTime)
        {
            for (int i = Components.Count - 1; i >= 0; i--)
                Components[i].Update(gameTime);
        }

        public virtual void PostUpdate(GameTime gameTime)
        {
            for (int i = Components.Count - 1; i >= 0; i--)
                Components[i].PostUpdate(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }

        public T AddComponent<T>(T component)
            where T : Component
        {
            Components.Add(component);
            if (component is SpritesheetManager)
                spritesheets = component as SpritesheetManager;
            else if (component is Rigidbody)
                rigidbody = component as Rigidbody;
            else if (component is Hitbox && hitbox == null)
                hitbox = component as Hitbox;
            else if (component is Graphic)
                graphic = component as Graphic;

            if (component is DrawableComponent)
                if(component is HUDComponent)
                    containerWorld.HUDComponents.Add(component as HUDComponent);            
                else
                    containerWorld.DrawableComponents.Add(component as DrawableComponent);
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
            foreach (Component comp in Components)
                if (ReferenceEquals(comp.GetType(), typeof(T)))
                    component = comp as T;
                else if (CheckForSubClasses && comp.GetType().IsSubclassOf(typeof(T)))
                    component = comp as T;

            return component;
        }

        public Component GetComponentByName(string name)
        {
            foreach (Component comp in Components)
                if (comp.Name == name)
                    return comp;

            return null;
        }

        public int GetLastID()
        {
            return Components[Components.Count - 1].ID;
        }

        public bool ViewedByCamera(Vector2 cameraPosition)
        {
            if (new Rectangle((int)(cameraPosition.X - Neon.HalfScreen.X / containerWorld.camera.Zoom), (int)(cameraPosition.Y - Neon.HalfScreen.Y / containerWorld.camera.Zoom), (int)(Neon.ScreenWidth / containerWorld.camera.Zoom), (int)(Neon.ScreenHeight / containerWorld.camera.Zoom)).Intersects(new Rectangle((int)this.transform.Position.X, (int)this.transform.Position.Y, 1, 1)))
            {
                return true;
            }
            return false;
        }
    }
}
