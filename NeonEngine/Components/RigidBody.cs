using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Common.PolygonManipulation;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using NeonEngine.Private;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine
{
    public class Rigidbody : Component
    {       
        public Hitbox hitbox;
        
        public Body body;
        public bool isGrounded;
        
        private bool wasGrounded;

        private PositionChange PositionChanged;
        private ComponentRemoved HitboxRemoved;
        public Beacon beacon;

        public List<Vector2> verticesList;

        private FarseerPhysics.Dynamics.World physicWorld;
        
        BodyType bodyType;
        public BodyType BodyType
        {
            get { return bodyType; }
            set { bodyType = value; }
        }

        private float gravityScale;
        public float GravityScale
        {
            get 
            {
                if (body != null)
                    return body.GravityScale;
                else
                    return gravityScale;
            }
            set 
            {
                if (body != null)
                    body.GravityScale = value;
                gravityScale = value;
            }
        }

        
        bool useGravity;
        public bool UseGravity
        {
            get 
            { 
                if(body != null)
                    return !body.IgnoreGravity;

                return useGravity;
            }
            set 
            {
                if (body != null)
                    body.IgnoreGravity = !value;
                else
                    useGravity = value;
            }
        }

        float restitution;
        public float Restitution
        {
            get 
            {
                if (body != null && !body.IsDisposed)
                     return body.Restitution;
                else
                    return restitution;
            }
            set 
            {
                if (body != null)
                    body.Restitution = value;
                else
                    restitution = value;
            }
        }

        float friction;
        public float Friction
        {
            get
            {
                if (body != null && !body.IsDisposed)
                    return body.Friction;
                else
                    return friction;
            }
            set
            {
                if (body != null)
                    body.Friction = value;
                else
                    friction = value;
            }
        }

        float mass;
        public float Mass
        {
            get
            {
                if (body != null && !body.IsDisposed)
                    return body.Mass;
                else
                    return mass;
            }
            set
            {
                if (body != null)
                    body.Mass = value;
                 mass = value;

            }
        }

        bool oneWayPlatform;
        public bool OneWayPlatform
        {
            get { return oneWayPlatform; }
            set { oneWayPlatform = value; }
        }

        bool sleepingAllowed = true;
        public bool SleepingAllowed
        {
            get 
            {
                if (body != null)
                    return body.SleepingAllowed;
                else
                    return sleepingAllowed;
            }
            set
            {
                if (body != null)
                    body.SleepingAllowed = value;

                sleepingAllowed = value;
            }
        }

        private bool isGround = false;
        public bool IsGround
        {
            get { return isGround; }
            set { isGround = value; }
        }

        public Hitbox Hitbox
        {
            get { return hitbox; }
            set 
            { 
                hitbox = value;
                hitbox.Removed += HitboxRemoved;
                Init();
            }
        }

        bool sensors;
        public bool Sensors
        {
            get { return sensors; }
            set { sensors = value; }
        }

        bool fixedRotation;
        public bool FixedRotation
        {
            get 
            {
                if (body != null)
                    return body.FixedRotation;
                else
                    return fixedRotation;
            }
            set 
            {
                if(body != null)
                    body.FixedRotation = value;
                fixedRotation = value; 
            }
        }
        

        public Rigidbody(Entity entity)
            :base(entity, "Rigidbody")
        {
            PositionChanged = new PositionChange(RefreshBodyPosition);
            HitboxRemoved = new ComponentRemoved(RemoveHitbox);
            entity.transform.PositionChanged += PositionChanged;
            entity.rigidbody = this;
            physicWorld = entity.containerWorld.physicWorld;
        }

        public override void Init()
        {
            if (hitbox != null)
            {
                if (body != null)
                    body.Dispose();

                switch (hitbox.ShapeType)
                {
                    case BodyShapeType.Rectangle:
                        body = BodyFactory.CreateRectangle(physicWorld,
                            CoordinateConversion.screenToWorld(hitbox.Width),
                            CoordinateConversion.screenToWorld(hitbox.Height),
                            1f);
                        break;
                    case BodyShapeType.Circle:
                        body = BodyFactory.CreateCircle(physicWorld, CoordinateConversion.screenToWorld(hitbox.Width / 2), 1f);
                        break;
                }

                body.BodyType = bodyType;
                body.FixedRotation = fixedRotation;
                body.IgnoreGravity = useGravity;
                body.Friction = friction;
                body.Restitution = restitution;
                body.GravityScale = gravityScale;
                body.Mass = mass;
                
                body.SleepingAllowed = sleepingAllowed;
                body.Position = CoordinateConversion.screenToWorld(hitbox.Center);
                if (entity.Name != "Entity")
                    body.OnCollision += body_OnCollision;
                if (sensors)
                    beacon = new Beacon(hitbox, physicWorld);

                
            }            
        }

        bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
 	        if(OneWayPlatform)
            {
                float offset = 0;
                Entity EntityA = Neon.utils.GetEntityByBody(fixtureA.Body);
                Entity EntityB = Neon.utils.GetEntityByBody(fixtureB.Body);

                if (EntityB != null)
                {
                    Hitbox hitbox = EntityB.GetComponent<Hitbox>();
                    if (hitbox != null && EntityB.transform.Position.Y + hitbox.Height / 2 + offset > entity.transform.Position.Y - this.hitbox.Height / 2)
                    {
                        contact.Enabled = false;
                        return false;
                    }
                }
            }

            return true;
        }

        public override void Update(GameTime gameTime)
        {
            if (body != null)
            {
                if (hitbox != null)
                {
                    if (Sensors)
                    {
                        beacon.Update(gameTime);
                        Rigidbody rg = beacon.CheckGround(body);
                        if (rg != null)
                            isGrounded = rg.isGround ? true : false;
                        else
                            isGrounded = false;
                        if (!isGrounded && !body.Awake)
                            body.Awake = true;
                        wasGrounded = isGrounded;
                    }
                }

                Position = Position;
            }
           
            
            base.Update(gameTime);
        }

        public override void Remove()
        {
            body.Dispose();

            base.Remove();
        }

        private Vector2 Position
        {
            get { return CoordinateConversion.worldToScreen(body.Position); }
            set 
            { 
                body.Position = CoordinateConversion.screenToWorld(value);
                entity.transform.Position = CoordinateConversion.worldToScreen(body.Position);
            }
        }

        public void GenerateNewBody(Hitbox hitbox)
        {
            body.Dispose();
            body = BodyFactory.CreateRectangle(physicWorld,
                        CoordinateConversion.screenToWorld(hitbox.Width),
                        CoordinateConversion.screenToWorld(hitbox.Height),
                        1f);

            body.BodyType = BodyType.Dynamic; ;
            body.Position = CoordinateConversion.screenToWorld(new Vector2(hitbox.X + hitbox.Width / 2, hitbox.Y + hitbox.Height / 2));
            if (Sensors)
            {
                beacon = new Beacon(hitbox, physicWorld);
            }  
        }

        private void RefreshBodyPosition()
        {
            if(body != null)
                body.Position = CoordinateConversion.screenToWorld(entity.transform.Position);
        }

        private void RemoveHitbox(object sender, EventArgs e)
        {
            hitbox.Removed -= HitboxRemoved;
            hitbox = null;
        }
    }
}
