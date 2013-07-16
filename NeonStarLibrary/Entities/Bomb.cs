using FarseerPhysics.Dynamics;
using NeonEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NeonEngine.Private;

namespace NeonStarLibrary
{
    public class Bomb : Enemy
    {
        Bomber b;
        ImageSequence Explosion;
        bool Exploded = false;

        public Bomb(Vector2 Position, NeonEngine.World containerWorld, Bomber b)
            :base(Position, 120, 108, 1000, ElementType.Fire, containerWorld)
        {
            Explosion = new ImageSequence("explo", 1, 11, 16, this, DrawLayer.Foreground6);
            Explosion.isPlaying = false;
            this.b = b;
            rigidbody.body.IsSensor = false;
            rigidbody.UseGravity = true;
            rigidbody.body.ApplyLinearImpulse(b.CurrentSide == SideDirection.Left ? new Vector2(-10, 0) : new Vector2(10, 0));
            waypoints = (Waypoints)AddComponent(new Waypoints(this));
            waypoints.Speed = 50f;
            currentSpriteSheet = new SpriteSheet(AssetManager.GetSpriteSheet("BombAnim"), DrawLayer.Middleground3, this);
            currentSpriteSheet.isPlaying = false;
            ChangeAnimation(currentSpriteSheet);
        }

        public override void Update(GameTime gameTime)
        {
            if (Exploded)
            {
                currentSpriteSheet.Remove();
                if (Explosion.completed)
                {
                    b.BombExploded = true;
                    this.Destroy();
                }
            }
            else if (rigidbody.body.LinearVelocity.X < CoordinateConversion.screenToWorld(1) && rigidbody.body.LinearVelocity.X > -CoordinateConversion.screenToWorld(1))
            {
                currentSpriteSheet.isPlaying = true;
                if(this.currentSpriteSheet.currentFrame == 8)
                    this.Explode();
            }

            base.Update(gameTime);
        }

        public void Explode()
        {
            for(int i = containerWorld.entities.Count - 1; i >= 0; i--)
                if(containerWorld.entities[i] is Minion)
                    (containerWorld.entities[i] as Minion).Die(true);
            Explosion.Scale = 2f;
            Explosion.X = (int)this.transform.Position.X - (int)this.hitbox.Width;
            Explosion.Y = (int)this.transform.Position.Y - (int)this.hitbox.Height;
            Explosion.isPlaying = true;
            AddComponent(Explosion);
            Minion m = new Minion(new Vector2(-414, -379), ElementType.Steel, 0, this.containerWorld, 50f);
            m.waypoints.AddWaypoint(new Vector2(-485, -53));
            m.waypoints.AddWaypoint(new Vector2(-289, -53));
            containerWorld.entities.Add(m);

            m = new Minion(new Vector2(160, -379), ElementType.Steel, 0, this.containerWorld, 50f);
            m.waypoints.AddWaypoint(new Vector2(-145, 104));
            m.waypoints.AddWaypoint(new Vector2(241, 104));
            containerWorld.entities.Add(m);
            
            m = new Minion(new Vector2(-80, -379), ElementType.Fire, 0, this.containerWorld, 50f);
            m.waypoints.AddWaypoint(new Vector2(-145, 104));
            m.waypoints.AddWaypoint(new Vector2(241, 104));
            containerWorld.entities.Add(m);

            m = new Minion(new Vector2(443, -379), ElementType.Steel, 0, this.containerWorld, 50f);
            m.waypoints.AddWaypoint(new Vector2(465, -54));
            m.waypoints.AddWaypoint(new Vector2(585, -54));
            containerWorld.entities.Add(m);

            avatar.TakeDamage(200);
            Exploded = true;
        }

    }
}
