using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework.Graphics;
using NeonEngine;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Common;
using FarseerPhysics.Common.PolygonManipulation;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Factories;


namespace NeonEngine
{
    public class LevelGeometry : Entity
    {
        public WallType wallType;
        public WallType Type
        {
            get { return wallType; }
            set { wallType = value; }
        }

        public bool IsBeingWalkedOn = false;
        private PolygonRenderer renderer;
        public int Width;
        public int Height;

        public LevelGeometry(World containerWorld, Vector2 Position, Vertices geometry, WallType wallType)
            : base(containerWorld)
        {
            this.wallType = wallType;
            transform.Position = Position;
            this.containerWorld = containerWorld;
            rigidbody = AddComponent(new Rigidbody(this));
            rigidbody.BodyType = BodyType.Static;
            rigidbody.Init();
            rigidbody.body.OnCollision += body_OnCollision;
            
            foreach(Vector2 vertice in rigidbody.verticesList)
            {
                if(vertice.Y != 0)
                    Height = (int)Math.Abs(vertice.Y);
                if(vertice.X != 0)
                    Width = (int)Math.Abs(vertice.X);
            }

            
        }

        bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
 	        if(wallType == WallType.OneWay)
            {
                float offset = 0;
                Entity EntityA = Neon.utils.GetEntityByBody(fixtureA.Body);
                Entity EntityB = Neon.utils.GetEntityByBody(fixtureB.Body);

                if(EntityA != null)
                {
                    Hitbox hitbox = EntityA.GetComponent<Hitbox>();
                    if (hitbox != null && EntityA.transform.Position.Y + hitbox.Height / 2 + offset > transform.Position.Y)
                    {
                        contact.Enabled = false;
                        return false;
                    }
                }

                if (EntityB != null)
                {
                    Hitbox hitbox = EntityB.GetComponent<Hitbox>();
                    if (hitbox != null && EntityB.transform.Position.Y + hitbox.Height / 2 + offset > transform.Position.Y)
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
            base.Update(gameTime);         
            renderer.Position = transform.Position;
            
        }

    }
}
