using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NeonEngine.Components.Audio;
using NeonEngine.Components.CollisionDetection;
using NeonEngine.Components.Graphics2D;
using NeonEngine.Components.Private;
using NeonEngine.Private;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine
{
    public class Entity
    {
        public bool HasToBeSaved = true;

        private string _layer = "";

        public string Layer
        {
            get { return _layer; }
            set { _layer = value; }
        }

        public List<Component> Components = new List<Component>();

        public Transform transform;

        public Rigidbody rigidbody;
        public SpritesheetManager spritesheets;
        public Graphic graphic;
        public List<Hitbox> hitboxes = new List<Hitbox>();
        public ManagedSoundEmitter soundEmitter;

        private string name = "Entity";
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public bool toDestroy;

        public World GameWorld;

        public Entity(World containerWorld)
        {
            this.GameWorld = containerWorld;
            transform = AddComponent<Transform>(new Transform(this));
        }

        public virtual void PreUpdate(GameTime gameTime)
        {
            for (int i = Components.Count - 1; i >= 0; i--)
            {
                if (Components.Count == 0)
                    break;
                Components[i].PreUpdate(gameTime);
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            for (int i = Components.Count - 1; i >= 0; i--)
            {
                if (Components.Count == 0)
                    break;
                Components[i].Update(gameTime);
            }
        }

        public virtual void PostUpdate(GameTime gameTime)
        {
            for (int i = Components.Count - 1; i >= 0; i--)
            {
                if (Components.Count == 0)
                    break;
                Components[i].PostUpdate(gameTime);
            }
        }

        public virtual void FinalUpdate(GameTime gameTime)
        {
            for (int i = Components.Count - 1; i >= 0; i--)
            {
                if (Components.Count == 0)
                    break;
                Components[i].FinalUpdate(gameTime);
            }
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
            else if (component is Hitbox)
                hitboxes.Add(component as Hitbox);
            else if (component is Graphic)
                graphic = component as Graphic;
            else if (component is ManagedSoundEmitter)
                soundEmitter = component as ManagedSoundEmitter;

            if (component is DrawableComponent)
                if((component  as DrawableComponent).IsHUD)
                    GameWorld.HUDComponents.Add(component as DrawableComponent);            
                else
                    GameWorld.DrawableComponents.Add(component as DrawableComponent);
            return component;
        }

        public virtual void OnChangeLevel()
        {
            foreach (Component c in Components)
                c.OnChangeLevel();
        }

        public virtual void Destroy()
        {         
            for(int i = Components.Count - 1; i >= 0; i--)
                Components[i].Remove();
            GameWorld.RemoveEntity(this);
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

        public Component GetComponentByTypeName(string typeName)
        {
            foreach (Component comp in Components)
            {
                if (comp.GetType().Name == typeName)
                    return comp;
            }

            return null;
        }

        public Component GetComponentByName(string name)
        {
            foreach (Component comp in Components)
                if (comp.Name == name)
                    return comp;

            return null;
        }

        public List<T> GetComponentsByInheritance<T>()
            where T : Component
        {
            List<T> list = new List<T>();

            foreach (Component comp in Components)
                if (comp.GetType().IsSubclassOf(typeof(T)) || ReferenceEquals(comp.GetType(), typeof(T)))
                    list.Add((T)comp);

            return list;
        }

        public int GetLastID()
        {
            int maxID = 0;
            foreach (Component c in Components)
            {
                if (c.ID > maxID)
                {
                    maxID = c.ID;
                }
            }
            return maxID + 1;
        }

        public bool ViewedByCamera(Vector2 cameraPosition)
        {
            if (new Rectangle((int)(cameraPosition.X - Neon.HalfScreen.X / GameWorld.Camera.Zoom), (int)(cameraPosition.Y - Neon.HalfScreen.Y / GameWorld.Camera.Zoom), (int)(Neon.ScreenWidth / GameWorld.Camera.Zoom), (int)(Neon.ScreenHeight / GameWorld.Camera.Zoom)).Intersects(new Rectangle((int)this.transform.Position.X, (int)this.transform.Position.Y, 1, 1)))
            {
                return true;
            }
            return false;
        }
    }
}
