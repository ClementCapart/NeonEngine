using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Common.PolygonManipulation;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using NeonEngine.Components.Private;
using NeonEngine.Private;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine.Components.CollisionDetection
{
    public class Rigidbody : Component
    {       
        public Hitbox hitbox;
        
        public Body body;
        public bool isGrounded = true;
        
        public bool wasGrounded = true;

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

        public float InitialGravityScale;
        private float gravityScale = 2f;
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


        bool useGravity = true;
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

        float friction = 10f;
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
            set 
            { 
                sensors = value;
                if (sensors)
                {
                    if(hitbox != null)
                        beacon = new Beacon(hitbox, physicWorld);
                }
                else
                {
                    beacon = null;
                }
            }
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

        bool fixedRotation = true;
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

        private List<Contact> _currentContacts = null;       

        public Rigidbody(Entity entity)
            :base(entity, "Rigidbody")
        {
            RequiredComponents = new Type[] { typeof(Hitbox) };
            PositionChanged = new PositionChange(RefreshBodyPosition);
            HitboxRemoved = new ComponentRemoved(RemoveHitbox);
            entity.transform.PositionChanged += PositionChanged;
            entity.rigidbody = this;
            physicWorld = entity.GameWorld.PhysicWorld;
        }

        public override void Init()
        {
            if (entity.hitboxes != null && entity.hitboxes.Count > 0)
            {
                hitbox = entity.hitboxes[0];
                if (body != null)
                    body.Dispose();

                switch (hitbox.ShapeType)
                {
                    case BodyShapeType.Rectangle:
                        body = BodyFactory.CreateRoundedRectangle(physicWorld,
                            CoordinateConversion.screenToWorld(hitbox.Width),
                            CoordinateConversion.screenToWorld(hitbox.Height),
                            0.005f,0.005f, 0, 1f, entity);
                        /*body = BodyFactory.CreateRectangle(physicWorld, CoordinateConversion.screenToWorld(hitbox.Width),
                            CoordinateConversion.screenToWorld(hitbox.Height), 1.0f, entity);*/
                        break;
                    case BodyShapeType.Circle:
                        body = BodyFactory.CreateCircle(physicWorld, CoordinateConversion.screenToWorld(hitbox.Width / 2), 1f, entity);
                        break;
                }

                body.BodyType = bodyType;
                body.FixedRotation = fixedRotation;
                body.IgnoreGravity = !useGravity;
                body.Friction = friction;
                body.Restitution = restitution;
                body.GravityScale = gravityScale;
                InitialGravityScale = gravityScale;
                body.Mass = mass;
                
                if (IsGround)
                    body.CollisionCategories = Category.Cat1;
                else
                {
                    body.CollisionCategories = Category.Cat2;
                    body.CollidesWith = Category.Cat1;
                }
                
                body.SleepingAllowed = sleepingAllowed;
                body.Position = CoordinateConversion.screenToWorld(hitbox.Center);
                if (entity.Name != "Entity")
                    body.OnCollision += body_OnCollision;
                if (sensors)
                    beacon = new Beacon(hitbox, physicWorld);

                _currentContacts = new List<Contact>();

            }
        }

        bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            Entity EntityA = fixtureA.UserData as Entity;
            Entity EntityB = fixtureB.UserData as Entity;
 	        if(EntityA.rigidbody.OneWayPlatform || EntityB.rigidbody.OneWayPlatform)
            {
                if (!EntityA.rigidbody.OneWayPlatform)
                {
                    Entity e = EntityA;
                    EntityA = EntityB;
                    EntityB = e;
                }

                float offset = 0;
                
                Hitbox hitboxB = EntityB.hitboxes[0];
                if (EntityB.transform.Position.Y + hitboxB.Height / 2 + offset +hitboxB.OffsetY > EntityA.transform.Position.Y - EntityA.hitboxes[0].Height / 2  + EntityA.hitboxes[0].OffsetY
                    || (EntityA.transform.Position.X - EntityA.hitboxes[0].Width / 2 + EntityA.hitboxes[0].OffsetX > EntityB.transform.Position.X + hitboxB.Width / 2 + offset + hitboxB.OffsetX || EntityA.transform.Position.X + EntityA.hitboxes[0].Width / 2 < EntityB.transform.Position.X - hitboxB.Width / 2 - offset + hitboxB.OffsetX))
                {
                    contact.Enabled = false;
                    return false;
                }
                else if (EntityA.rigidbody.isGround && !(EntityA.hitboxes[0] != null && EntityB.transform.Position.Y - EntityB.hitboxes[0].Height / 2 + offset + hitboxB.OffsetY > EntityA.transform.Position.Y + EntityA.hitboxes[0].Height / 2 + EntityA.hitboxes[0].OffsetY))
                {
                    contact.Friction = 0.0f;
                    _currentContacts.Add(contact);
                    return true;
                }
            }
            else if (EntityA.rigidbody.isGround || EntityB.rigidbody.isGround)
            {
                if (!EntityA.rigidbody.isGround)
                {
                    Entity e = EntityA;
                    EntityA = EntityB;
                    EntityB = e;
                }
                float offset = 0;

                Hitbox hitboxB = EntityB.hitboxes[0];
                if (EntityA.rigidbody.isGround && !(EntityA.hitboxes[0] != null && EntityB.transform.Position.Y - EntityB.hitboxes[0].Height / 2 + offset + EntityB.hitboxes[0].OffsetY > EntityA.transform.Position.Y + EntityA.hitboxes[0].Height / 2 + EntityA.hitboxes[0].OffsetY))
                {
                    contact.Friction = 0.0f;
                    _currentContacts.Add(contact);
                    return true;
                }
            }
            return true;
        }

        public override void PreUpdate(GameTime gameTime)
        {
            
            base.PreUpdate(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            
            
            base.Update(gameTime);
        }

        public override void PostUpdate(GameTime gameTime)
        {
            Position = Position;
            
            for (int i = _currentContacts.Count - 1; i >= 0; i--)
            {
                Contact _currentContact = _currentContacts[i];
                if (!_currentContact.IsTouching())
                {
                    _currentContacts.Remove(_currentContact);
                }
                else
                {
                    Entity EntityA = _currentContact.FixtureA.UserData as Entity;
                    Entity EntityB = _currentContact.FixtureB.UserData as Entity;

                    if (!EntityA.rigidbody.IsGround)
                    {
                        Entity e = EntityA;
                        EntityA = EntityB;
                        EntityB = e;
                    }

                    if (EntityB.hitboxes[0] != null && (EntityB.transform.Position.Y + EntityB.hitboxes[0].Height / 2 + EntityB.hitboxes[0].OffsetY <= EntityA.transform.Position.Y - EntityA.hitboxes[0].Height / 2 + EntityA.hitboxes[0].OffsetY))
                    {
                        _currentContact.ResetFriction();
                        _currentContacts.Remove(_currentContact);
                    }
                }
            }
            if (this.fixedRotation)
                if (body != null)
                    body.Rotation = MathHelper.ToRadians(entity.transform.Rotation);
            if (body != null) body.GravityScale = InitialGravityScale;
            if (beacon != null) beacon.GroundOffset = Vector2.Zero;
            if (entity.GameWorld.FirstUpdate)
                isGrounded = true;
            base.PostUpdate(gameTime);
        }

        public override void FinalUpdate(GameTime gameTime)
        {
            if (body != null)
            {
                if (hitbox != null)
                {
                    if (Sensors)
                    {
                        wasGrounded = isGrounded;
                        beacon.Update(gameTime);
                        Rigidbody rg = beacon.CheckGround(Vector2.Zero, body);
                        if (rg != null)
                            isGrounded = rg.isGround ? true : false;
                        else
                            isGrounded = false;
                        if (!isGrounded && !body.Awake)
                            body.Awake = true;
                    }
                }
            }
            base.FinalUpdate(gameTime);
        }

        public override void Remove()
        {
            body.Dispose();
            base.Remove();
        }     

        public void GenerateNewBody(Hitbox hitbox)
        {
            body.Dispose();
            body = BodyFactory.CreateRectangle(physicWorld,
                        CoordinateConversion.screenToWorld(hitbox.Width),
                        CoordinateConversion.screenToWorld(hitbox.Height),
                        1f, entity);

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
