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
        public float Layer = 0.5f;
        private bool _tint = false;
        public ColorEmitter TintedBy;

        private bool isHUD = false;

        public bool IsHUD
        {
            get { return isHUD; }
            set { isHUD = value; }
        }

        public Effect CurrentEffect;

        protected Side _currentSide = Side.Right;

        public virtual Side CurrentSide
        {
            get { return _currentSide; }
            set { _currentSide = value; }
        }
        
        public virtual bool Tint
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

        private Vector2 _offset;

        public virtual Vector2 Offset
        {
            get { return _offset; }
            set { _offset = value; }
        }

        private float _rotationOffset = 0.0f;

        public virtual float RotationOffset
        {
            get { return _rotationOffset; }
            set { _rotationOffset = value; }
        }

        private Vector2 _rotationCenter = new Vector2();

        public virtual Vector2 RotationCenter
        {
            get { return _rotationCenter; }
            set { _rotationCenter = value; }
        }

        public Color InitialTintcolor = Color.White;
        private Color _tintColor = Color.White;
        public virtual Color TintColor
        {
            get { return _tintColor; }
            set { _tintColor = value; }
        }

        public Color MainColor = Color.White;

        public DrawableComponent(float Layer, Entity entity, string Name)
            :base(entity, Name)
        {
            this.Layer = Layer;
        }

        public override void Init()
        {
            CurrentEffect = AssetManager.GetEffect("BasicRender");
            InitialTintcolor = TintColor;
            base.Init();
        }

        public override void FinalUpdate(GameTime gameTime)
        {
            if(entity != null)
                _parallaxPosition = entity.containerWorld.Camera.Position * ParallaxForce;
            base.FinalUpdate(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            TintColor = Color.White;
        }

        public override void Remove()
        {
            if (isHUD)
                entity.containerWorld.HUDComponents.Remove(this);
            else
                entity.containerWorld.DrawableComponents.Remove(this);
            
            base.Remove();
        }
    }
}
