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
                if (entity.Name == "TrainingNoviceLeak01")
                    Console.WriteLine("TrainingNociveLeak01");
            }

            if (entity.Name == "TrainingNoviceLeak01")
                Console.WriteLine("TrainingNociveLeak01");
        }

        bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            
 	        if(OneWayPlatform)
            {
                float offset = 0;
                Entity EntityA = Neon.Utils.GetEntityByBody(fixtureA.Body);
                Entity EntityB = Neon.Utils.GetEntityByBody(fixtureB.Body);
                if (EntityB != entity && EntityB != null)
                {
                    Hitbox hitboxB = EntityB.hitboxes[0];
                    if (hitbox != null && EntityB.transform.Position.Y + hitboxB.Height / 2 + offset +hitboxB.OffsetY > entity.transform.Position.Y - this.hitbox.Height / 2  + hitbox.OffsetY
                        || (entity.transform.Position.X - hitbox.Width / 2 + hitbox.OffsetX > EntityB.transform.Position.X + hitboxB.Width / 2 + offset + hitbox.OffsetX || entity.transform.Position.X + this.hitbox.Width / 2 < EntityB.transform.Position.X - hitboxB.Width / 2 - offset + hitboxB.OffsetX))
                    {
                        contact.Enabled = false;
                        return false;
                    }
                    else if (isGround && !(entity.hitboxes[0] != null && EntityB.transform.Position.Y - EntityB.hitboxes[0].Height / 2 + offset + hitboxB.OffsetY > entity.transform.Position.Y + entity.hitboxes[0].Height / 2 + entity.hitboxes[0].OffsetY))
                    {
                        contact.Friction = 0.0f;
                        _currentContacts.Add(contact);
                        return true;
                    }
                }
                else if (EntityA != entity && EntityA != null)
                {
                    Hitbox hitboxA = EntityA.hitboxes[0];
                    if (hitbox != null && EntityA.transform.Position.Y + hitboxA.Height / 2 + offset + hitboxA.OffsetY > entity.transform.Position.Y - this.hitbox.Height / 2 + hitbox.OffsetY
                        || (entity.transform.Position.X - hitbox.Width / 2 + hitbox.OffsetX > EntityA.transform.Position.X + hitboxA.Width / 2 + offset + hitboxA.OffsetX || entity.transform.Position.X + this.hitbox.Width / 2 + hitbox.OffsetX < EntityA.transform.Position.X - hitboxA.Width / 2 + offset + hitboxA.OffsetX ))
                    {
                        contact.Enabled = false;
                        return false;
                    }
                    else if (isGround && !(entity.hitboxes[0] != null && EntityA.transform.Position.Y - EntityA.hitboxes[0].Height / 2 + offset + EntityA.hitboxes[0].OffsetY > entity.transform.Position.Y + entity.hitboxes[0].Height / 2 + entity.hitboxes[0].OffsetY))
                    {
                        contact.Friction = 0.0f;
                        _currentContacts.Add(contact);
                        return true;
                    }
                }


                
            }
            else if (isGround)
            {
                float offset = 0;
                Entity EntityA = Neon.Utils.GetEntityByBody(fixtureA.Body);
                Entity EntityB = Neon.Utils.GetEntityByBody(fixtureB.Body);

                if (EntityB != entity && EntityB != null)
                {
                    Hitbox hitboxB = EntityB.hitboxes[0];
                    if (isGround && !(entity.hitboxes[0] != null && EntityB.transform.Position.Y - EntityB.hitboxes[0].Height / 2 + offset + EntityB.hitboxes[0].OffsetY > entity.transform.Position.Y + entity.hitboxes[0].Height / 2 + entity.hitboxes[0].OffsetY))
                    {
                        contact.Friction = 0.0f;
                        _currentContacts.Add(contact);
                        return true;
                    }
                }
                else if (EntityA != entity && EntityA != null)
                {
                    Hitbox hitboxA = EntityB.hitboxes[0];
                    if (isGround && !(entity.hitboxes[0] != null && EntityA.transform.Position.Y - EntityA.hitboxes[0].Height / 2 + offset + EntityA.hitboxes[0].OffsetY > entity.transform.Position.Y + entity.hitboxes[0].Height / 2 + entity.hitboxes[0].OffsetY))
                    {
                        contact.Friction = 0.0f;
                        _currentContacts.Add(contact);
                        return true;
                    }
                }
                
            }

            return true;
        }

        public override void PreUpdate(GameTime gameTime)
        {
            if (body != null)
            {
                if (hitbox != null)
                {
                    if (Sensors)
                    {
                        wasGrounded = isGrounded;
                        beacon.Update(gameTime);
                        Rigidbody rg = beacon.CheckGround(body);
                        if (rg != null)
                            isGrounded = rg.isGround ? true : false;
                        else
                            isGrounded = false;
                        if (!isGrounded && !body.Awake)
                            body.Awake = true;
                    }
                }

                
            }
            if (isGround)
            {
                for (int i = _currentContacts.Count - 1; i >= 0; i--)
                {
                    Contact _currentContact = _currentContacts[i];
                    if (!_currentContact.IsTouching())
                    {
                        _currentContacts.Remove(_currentContact);
                    }
                    else
                    {
                        Entity EntityA = Neon.Utils.GetEntityByBody(_currentContact.FixtureA.Body);
                        Entity EntityB = Neon.Utils.GetEntityByBody(_currentContact.FixtureB.Body);
                        if (EntityB != entity && EntityB != null)
                        {
                            if (EntityB.hitboxes[0] != null && (EntityB.transform.Position.Y + EntityB.hitboxes[0].Height / 2 + EntityB.hitboxes[0].OffsetY <= EntityA.transform.Position.Y - EntityA.hitboxes[0].Height / 2 + EntityA.hitboxes[0].OffsetY))
                            {
                                _currentContact.ResetFriction();
                                _currentContacts.Remove(_currentContact);
                            }
                        }
                        else if (EntityA != entity && EntityA != null)
                        {
                            if (EntityA.hitboxes[0] != null && (EntityA.transform.Position.Y + EntityA.hitboxes[0].Height / 2 + EntityA.hitboxes[0].OffsetY <= EntityB.transform.Position.Y - EntityB.hitboxes[0].Height / 2 + EntityB.hitboxes[0].OffsetY))
                            {
                                _currentContact.ResetFriction();
                                _currentContacts.Remove(_currentContact);
                            }
                        }
                        
                    }
                }
            }

            if (this.fixedRotation)
                if (body != null)
                    body.Rotation = MathHelper.ToRadians(entity.transform.Rotation);

            base.PreUpdate(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void PostUpdate(GameTime gameTime)
        {
            Position = Position;
            if(body != null) body.GravityScale = InitialGravityScale;
            base.PostUpdate(gameTime);
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
