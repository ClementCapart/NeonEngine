using NeonEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NeonEngine.Private;

namespace NeonStarLibrary
{
    public class Minion : Enemy
    {
        SpriteSheet IdleAnimation;
        SpriteSheet RunAnimation;
        SpriteSheet DeathAnimation;

        float SmokeDelay = 0.5f;
        float SmokeTimer = 0f;      

        public Minion(Vector2 Position, ElementType Element, float LifePoints, World currentWorld, float Speed)
            : base(Position, 75, 65, LifePoints, Element, currentWorld)
        {
            IdleAnimation = new SpriteSheet(AssetManager.GetSpriteSheet(Element.ToString() + "MinionIdle"), 0.4f, this);
            RunAnimation = new SpriteSheet(AssetManager.GetSpriteSheet(Element.ToString() + "MinionRun"), 0.4f, this);
            DeathAnimation = new SpriteSheet(AssetManager.GetSpriteSheet(Element.ToString() + "MinionDeath"), 0.4f, this);
            DeathAnimation.IsLooped = false;

            waypoints = AddComponent(new Waypoints(this));
            waypoints.Speed = Speed;
            ChangeAnimation(IdleAnimation);
        }

        public Minion(ElementType Element, float LifePoints, World currentWorld)
            : this(Vector2.Zero, Element, LifePoints, currentWorld, 10f)
        {

        }

        public override void Update(GameTime gameTime)
        {
            currentSpriteSheet.spriteEffects = CurrentSide == SideDirection.Right ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            if (Dying)
            {
                waypoints.FollowPath = false;

                if (DeathAnimation.currentFrame == DeathAnimation.spriteSheetInfo.FrameCount - 1)
                    DeathAnimation.opacity -= 0.02f;

                if (DeathAnimation.opacity <= 0)
                    Destroy();
            }
            else
            {
                if (waypoints.FollowPath && currentSpriteSheet != RunAnimation)
                    ChangeAnimation(RunAnimation);
                else if (!waypoints.FollowPath && currentSpriteSheet != IdleAnimation)
                    ChangeAnimation(IdleAnimation);
                if (waypoints.FollowPath && SmokeTimer >= SmokeDelay)
                {
                    containerWorld.entities.Add(new Feedback("DustMinion", transform.Position + new Vector2(waypoints.direction == SideDirection.Left ? hitbox.Width / 2 : -hitbox.Width / 2, currentSpriteSheet.spriteSheetInfo.FrameHeight / 2 - 4), 0.9f, CurrentSide, 1f, containerWorld));
                    SmokeTimer = 0f;
                }
                else
                    SmokeTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (!rigidbody.isGrounded)
                    waypoints.FollowPath = false;
                else if(waypoints.waypoints.Count > 0)
                    waypoints.FollowPath = true;

                if (waypoints.ReachedWaypoint && currentSpriteSheet != IdleAnimation)
                    ChangeAnimation(IdleAnimation);
                else if (waypoints.ReachedWaypoint == false && currentSpriteSheet != RunAnimation)
                    ChangeAnimation(RunAnimation);
            }
            if(waypoints.FollowPath)
                CurrentSide = waypoints.direction > 0 ? SideDirection.Left : SideDirection.Right;
            
            base.Update(gameTime);
        }

        public override void Die(bool NoElement = false)
        {
            ChangeAnimation(DeathAnimation);
            base.Die(NoElement);
        }
    }
}
