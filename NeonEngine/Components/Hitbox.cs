using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine
{
    public enum HitboxType
    {
        None, Solid, Main, Hit, Part, Bullet, Invincible, Trigger, OneWay
    }

    public class Hitbox : Component
    {
        private int circleRadius;

        public BodyShapeType ShapeType;
        private HitboxType _type;
        private HitboxType _initialType;
        private float _switchDuration = 0.0f;

        public Hitbox LastIntersects;

        public HitboxType Type
        {
            get { return _type; }
            set 
            { 
                _type = value;
                _initialType = value;
            }
        }
        
        public Vector2 Center
        {
            get { return new Vector2(hitboxRectangle.X + hitboxRectangle.Width / 2, hitboxRectangle.Y + hitboxRectangle.Height / 2); }
            set 
            { 
                hitboxRectangle.X = (int)value.X - hitboxRectangle.Width / 2 + (int)Offset.X;
                hitboxRectangle.Y = (int)value.Y - hitboxRectangle.Height / 2 + (int)Offset.Y;
            }
        }
        public float X
        {
            get { return hitboxRectangle.X; }
            set { hitboxRectangle.X = (int)value + (int)Offset.X; }
        }
        public float Y
        {
            get { return hitboxRectangle.Y; }
            set { hitboxRectangle.Y = (int)value + (int)Offset.Y; }
        }

        public Rectangle hitboxRectangle;

        public List<Vector2> vectors;

        private Vector2 Offset;


        public float OffsetX
        {
            get { return Offset.X; }
            set
            {
                hitboxRectangle.X += (int)(value - Offset.X);
                Offset.X = value;
            }
        }

        public float OffsetY
        {
            get { return Offset.Y; }
            set
            {
                hitboxRectangle.Y += (int)(value - Offset.Y);
                Offset.Y = value;
            }
        }

        public float Width
        {
            get 
            {
                if (ShapeType == BodyShapeType.Rectangle)                
                    return hitboxRectangle.Width;
                else if (ShapeType == BodyShapeType.Circle)
                    return circleRadius;
                else return 0;
            }
            set 
            {
                if (ShapeType == BodyShapeType.Rectangle)
                {
                    GenerateVectorList((int)value, (int)Height);
                    hitboxRectangle.Width = (int)value;
                }
                else if (ShapeType == BodyShapeType.Circle)
                    circleRadius = (int)value;
            }
        }

        public float Height
        {
            get
            {
                if (ShapeType == BodyShapeType.Rectangle) 
                    return hitboxRectangle.Height;
                else if (ShapeType == BodyShapeType.Circle)
                    return circleRadius;
                else return 0;
            }
            set
            {
                if (ShapeType == BodyShapeType.Rectangle)
                {
                    GenerateVectorList((int)Width, (int)value);
                    hitboxRectangle.Height = (int)value;
                }
                else if (ShapeType == BodyShapeType.Circle)
                    circleRadius = (int)value;
            }
        }

        public bool InUse = false;

        public Hitbox(Entity entity)
            :base(entity, "Hitbox")
        {
            vectors = new List<Vector2>();
            hitboxRectangle = new Rectangle();
            this.Width = 50;
            this.Height = 50;
            InUse = true;
            entity.containerWorld.Hitboxes.Add(this);
            ShapeType = BodyShapeType.Rectangle;           
        }

        public Hitbox()
            :base(null, "Hitbox")
        {
            InUse = false;
            vectors = new List<Vector2>();
            hitboxRectangle = new Rectangle();
            this.Width = 50;
            this.Height = 50;

            ShapeType = BodyShapeType.Rectangle;   
        }

        public override void Init()
        {       
            Center = entity.transform.Position;
            _initialType = _type;
            base.Init();
        }

        public void PoolInit(Entity entity)
        {
            this.LastIntersects = null;
            InUse = false;
            vectors = new List<Vector2>();
            hitboxRectangle = new Rectangle();
            this.Width = 50;
            this.Height = 50;

            ShapeType = BodyShapeType.Rectangle; 
            InUse = true;
            this.entity = entity;
            entity.containerWorld.Hitboxes.Add(this);
            Center = entity.transform.Position;
            _initialType = _type;
        }

        public override void PreUpdate(GameTime gameTime)
        {
            
            base.PreUpdate(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            Center = entity.transform.Position;
            base.Update(gameTime);
        }

        public override void PostUpdate(GameTime gameTime)
        {          
            if (_switchDuration > 0.0f)
                _switchDuration -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            else
                _type = _initialType;

            base.PostUpdate(gameTime);
        }
        public void GenerateVectorList(int Width, int Height)
        {
            vectors.Clear();
            vectors.Add(new Vector2(-Width / 2, -Height / 2));
            vectors.Add(new Vector2(Width / 2, -Height / 2));
            vectors.Add(new Vector2(Width / 2, Height / 2));
            vectors.Add(new Vector2(-Width / 2, Height / 2));
            vectors.Add(new Vector2(-Width / 2, -Height / 2));
        }

        public override void Remove()
        {
            InUse = false;
            entity.containerWorld.Hitboxes.Remove(this);
            entity.hitboxes.Remove(this);
            entity.containerWorld.HitboxPool.FlagAvailableItem(this);
            base.Remove();
        }

        public void SwitchType(HitboxType type, float duration)
        {
            _type = type;
            _switchDuration = duration;
        }
    }
}
