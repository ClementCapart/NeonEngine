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

        protected Vector2 _parallaxPosition;

        protected Vector2 _parallaxForce;

        public Vector2 ParallaxForce
        {
            get { return _parallaxForce; }
            set { _parallaxForce = value; }
        }

        public Color InitialTintcolor = Color.White;
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

        public override void Init()
        {
            InitialTintcolor = TintColor;
            base.Init();
        }

        public override void Update(GameTime gameTime)
        {
            if(entity != null)
                _parallaxPosition = entity.containerWorld.camera.Position * ParallaxForce;
            base.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            TintColor = Color.White;
        }

        public override void Remove()
        {
            entity.containerWorld.DrawableComponents.Remove(this);
            base.Remove();
        }
    }
}
