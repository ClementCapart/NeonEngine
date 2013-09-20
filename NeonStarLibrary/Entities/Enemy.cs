using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NeonEngine;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using NeonEngine.Private;
using World = NeonEngine.World;

namespace NeonStarLibrary
{
    public abstract class Enemy : Entity
    {
        protected Fight fight;

        protected ElementType element;
        protected Avatar avatar;

        protected ChangedBody changedBody;

        public Waypoints waypoints;
        public SpriteSheet currentSpriteSheet;
        public bool Dying;

        public SideDirection CurrentSide;

        public ElementType Element
        {
            get
            {
                return element;
            }
            set
            {
                element = value;
            }
        }

        public bool AddWaypoint
        {
            get
            {
                return waypoints.FollowPath;
            }
            set
            {
                waypoints.FollowPath = value;
            }
        }

        float _lifePoints;
        public float LifePoints
        {
            get { return _lifePoints; }
            set { _lifePoints = value; }
        }

        public Enemy(Vector2 Position, int Width, int Height, float StartLifePoints, ElementType StartElement, World currentWorld)
            : base(currentWorld)
        {
            transform.Position = Position;
            changedBody = new ChangedBody(IgnoreThisBody);
            LifePoints = StartLifePoints;
            element = StartElement;

            if (containerWorld is GameScreen)
                avatar = (containerWorld as GameScreen).avatar;
            else
                avatar = (containerWorld as HoloRoom).avatar;
            
            hitbox = AddComponent(new Hitbox(this));
            hitbox.Width = Width;
            hitbox.Height = Height;
            hitbox.Init();

            rigidbody = AddComponent(new Rigidbody(this));
            rigidbody.Sensors = true;
            rigidbody.BodyType = BodyType.Dynamic;
            rigidbody.Init();
            rigidbody.body.FixedRotation = true;
            
            if (avatar != null)
            {
                avatar.ChangedItsBody += changedBody;
                rigidbody.body.IgnoreCollisionWith(avatar.rigidbody.body);
            }
            rigidbody.Friction = 10f;
            foreach(Enemy e in containerWorld.entities.Where(e => e is Enemy))
                e.rigidbody.body.IgnoreCollisionWith(rigidbody.body);

            fight = AddComponent(new Fight(hitbox, new Rectangle(50, 50, 0, 0), this));
            AddComponent(new HitboxRenderer(fight));
        }

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);

            if (fight.TakeHit(avatar.fight))
                TakeDamage(5, avatar.elementsManager.MainElement);
        }

        public void TakeDamage(float damage, ElementType element)
        {
            LifePoints -= damage;
            if(!Dying)
                if(LifePoints <= 0)
                    Die();
        }

        public void IgnoreThisBody(Body body, EventArgs e)
        {
            rigidbody.body.IgnoreCollisionWith(body);
        }

        public virtual void Die(bool NoElement = false)
        {
            if(!NoElement)
                avatar.elementsManager.StockElement(element);
            Dying = true;
        }

        public override void Destroy()
        {
            avatar.ChangedItsBody -= changedBody;
            base.Destroy();
        }

        public void ChangeAnimation(SpriteSheet spriteSheet)
        {
            if (currentSpriteSheet != null)
            {
                currentSpriteSheet.Remove(); 
                spriteSheet.spriteEffects = currentSpriteSheet.spriteEffects;
            }
 
            currentSpriteSheet = spriteSheet;
            AddComponent(spriteSheet);
        }
    }
}
