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
    public class Slasher : Enemy
    {
        SpriteSheet IdleAnim;
        SpriteSheet MoveAnim;
        SpriteSheet DeathAnim;
        SpriteSheet AttackAnim;
        SpriteSheet LoadDashAnim;
        SpriteSheet DashAnim;

        float AttackRange = 200f;
        float AggroRange = 300f;
        float LoseAggroRange = 300f;

        bool IsAttacking = false;
        float DashTarget;

        float SmokeDelay = 0.7f;
        float SmokeTimer = 0f;

        Feedback LoadSmoke;

        int State = 0;

        public Slasher(Vector2 Position, World containerWorld)
            :base(Position, 80, 100, 20, ElementType.Steel, containerWorld)
        {
            IdleAnim = new SpriteSheet(AssetManager.GetSpriteSheet("SlasherIdle"), 0.5f, this);
            MoveAnim = new SpriteSheet(AssetManager.GetSpriteSheet("SlasherRun"), 0.5f, this);
            DeathAnim = new SpriteSheet(AssetManager.GetSpriteSheet("SlasherDeath"), 0.5f, this);
            DeathAnim.IsLooped = false;
            AttackAnim = new SpriteSheet(AssetManager.GetSpriteSheet("SlasherAttack"), 0.5f, this);
            AttackAnim.IsLooped = false;
            LoadDashAnim = new SpriteSheet(AssetManager.GetSpriteSheet("SlasherDashLoad"), 0.5f, this);
            LoadDashAnim.IsLooped = false;
            DashAnim = new SpriteSheet(AssetManager.GetSpriteSheet("SlasherDash"), 0.5f, this);
            DashAnim.IsLooped = false;

            waypoints = (Waypoints)AddComponent(new Waypoints(this));

            ChangeAnimation(IdleAnim);
            //AddComponent(new HitboxRenderer(fight, containerWorld));
        }

        public override void Update(GameTime gameTime)
        {
            currentSpriteSheet.spriteEffects = CurrentSide == SideDirection.Left ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            if (Dying)
            {
                waypoints.FollowPath = false;
                if (currentSpriteSheet != DeathAnim)
                {
                    DeathAnim.SetFrame(0);
                    ChangeAnimation(DeathAnim);
                }

                if (DeathAnim.currentFrame == DeathAnim.spriteSheetInfo.FrameCount - 1)
                    DeathAnim.opacity -= 0.02f;

                if (DeathAnim.opacity <= 0)
                    this.Destroy();
            }
            else if (State == 0)
            {
                if (currentSpriteSheet == MoveAnim)
                {
                    if (SmokeTimer > SmokeDelay)
                    {
                        containerWorld.AddEntity(new Feedback("MoveSmoke", this.transform.Position + new Vector2(0, this.hitbox.Height / 2 - 7), 0.5f, CurrentSide, 1f, containerWorld));
                        SmokeTimer = 0f;
                    }
                    else
                        SmokeTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                if (waypoints.waypoints.Count > 0)
                {
                    waypoints.FollowPath = true;

                    CurrentSide = waypoints.direction > 0 ? SideDirection.Left : SideDirection.Right;                
                }

                bool PlayerInRange = false;
                containerWorld.physicWorld.RayCast((fixture, hitPosition, normal, fraction) =>
                {
                    if (Neon.utils.GetEntityByBody(fixture.Body) is Avatar)
                    {
                        PlayerInRange = true;
                        return 0;
                    }
                    return -1;
                },
                CoordinateConversion.screenToWorld(this.transform.Position),
                CoordinateConversion.screenToWorld(this.transform.Position + new Vector2((CurrentSide == SideDirection.Left ? -this.hitbox.Width / 2 - AggroRange : this.hitbox.Width / 2 + AggroRange), 0)));

                if (PlayerInRange)
                    State = 1;

                if (waypoints.ReachedWaypoint && currentSpriteSheet != IdleAnim)
                    ChangeAnimation(IdleAnim);
                else if (waypoints.ReachedWaypoint == false && currentSpriteSheet != MoveAnim)
                    ChangeAnimation(MoveAnim);
            }
            else if (State == 1)
            {
                ChangeAnimation(MoveAnim);
                if (SmokeTimer > SmokeDelay)
                {
                    containerWorld.AddEntity(new Feedback("MoveSmoke", this.transform.Position + new Vector2(0, this.hitbox.Height / 2 - 7), 0.5f, CurrentSide, 1f, containerWorld));
                    SmokeTimer = 0f;
                }
                else
                    SmokeTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                waypoints.FollowPath = false;
                
                bool InAttackRange = false;

                containerWorld.physicWorld.RayCast((fixture, hitPosition, normal, fraction) =>
                {
                    if (Neon.utils.GetEntityByBody(fixture.Body) is Avatar)
                    {
                        InAttackRange = true;
                        return 0;
                    }
                    return -1;
                },
                CoordinateConversion.screenToWorld(this.transform.Position),
                CoordinateConversion.screenToWorld(this.transform.Position + new Vector2((CurrentSide == SideDirection.Left ? -this.hitbox.Width / 2 - AttackRange : this.hitbox.Width / 2 + AttackRange), 0)));

                if (InAttackRange)
                {
                    State = 2;
                }
                else
                {
                    if (avatar.transform.Position.X <= this.transform.Position.X)
                    {
                        CurrentSide = SideDirection.Left;
                        this.rigidbody.body.LinearVelocity = new Vector2(-1.5f, 0);
                    }
                    else
                    {
                        CurrentSide = SideDirection.Right;
                        this.rigidbody.body.LinearVelocity = new Vector2(1.5f, 0);
                    }
                }
                           
                if(Vector2.Distance(this.transform.Position, avatar.transform.Position) > LoseAggroRange)
                    State = 0;

                if (this.hitbox.hitboxRectangle.Intersects(avatar.hitbox.hitboxRectangle))
                    State = 2;


            }
            else if (State == 2)
            {
                if (LoadDashAnim != currentSpriteSheet && IsAttacking == false)
                {
                    LoadDashAnim.SetFrame(0);
                    ChangeAnimation(LoadDashAnim);
                    LoadSmoke = null;
                }
                else
                {
                    if (LoadSmoke == null)
                    {
                        LoadSmoke = new Feedback("DashLoadSmoke01", this.transform.Position + new Vector2((CurrentSide == SideDirection.Left ? -10 : 10), this.hitbox.Height / 2 - 7), 0.5f, CurrentSide, 1f, containerWorld);
                        containerWorld.entities.Add(LoadSmoke);
                    }
                    else if (LoadSmoke.animation.isPlaying == false)
                        LoadSmoke = null;

                    if (LoadDashAnim.currentFrame == LoadDashAnim.spriteSheetInfo.FrameCount - 1)
                    {
                        if (IsAttacking)
                            CheckDash();
                        else
                            StartAttack();
                    }
                }
            }

            base.Update(gameTime);
        }

        public void StartAttack()
        {
            DashAnim.SetFrame(0);
            ChangeAnimation(DashAnim);
            IsAttacking = true;
            DashTarget = this.transform.Position.X + (CurrentSide == SideDirection.Left ? -AttackRange : AttackRange);
            this.rigidbody.body.LinearVelocity = new Vector2((CurrentSide == SideDirection.Left ? - 10 : 10), 0);
            this.containerWorld.entities.Add(new Feedback("DashFire", this.transform.Position, 0.6f, CurrentSide, 1f, containerWorld));
        }

        public void CheckDash()
        {
            if (AttackAnim == currentSpriteSheet)
            {
                if (AttackAnim.currentFrame == AttackAnim.spriteSheetInfo.FrameCount - 1)
                {
                    IsAttacking = false;
                    State = 1;
                }
            }
            else
            {
                if (CurrentSide == SideDirection.Left && transform.Position.X <= DashTarget || CurrentSide == SideDirection.Right && transform.Position.X >= DashTarget)
                {
                    AttackAnim.SetFrame(0);
                    ChangeAnimation(AttackAnim);
                    rigidbody.body.LinearVelocity *= 0.5f;
                }
                else if (rigidbody.body.LinearVelocity.X < 0.5f && rigidbody.body.LinearVelocity.X > -0.5f)
                {
                    rigidbody.body.LinearVelocity = Vector2.Zero;
                    State = 1;
                }
                else
                {
                    this.containerWorld.entities.Add(new Feedback("DashSmoke", this.transform.Position + new Vector2(0, this.hitbox.Height / 2 - 20), 0.6f, CurrentSide, 1f, containerWorld));
                    this.containerWorld.entities.Add(new Feedback("DashEffect", this.transform.Position + new Vector2(0, 10) , 0.6f, CurrentSide, 1f, containerWorld));
                }
            }
        }

        
    }
}
