﻿using NeonEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NeonEngine.Private;

namespace NeonStarLibrary
{
    public class Bomber : Enemy
    {

        public int Counter = 0;       
        
        public float DelaySummon = 1f;
        public float TimerSummon = 0f;

        public float DelayBomb = 10f;
        public float TimerBomb = 0f;
        public Bomb BombSpawned;
        public bool BombExploded;

        public SpriteSheet LaunchBombAnim;
        public SpriteSheet IdleAnim;
        public SpriteSheet RunAnim;
        public SpriteSheet ShotAnim;
        public SpriteSheet MeleeAnim;
        public SpriteSheet DeathAnim;

        public bool MeleeAttacking = false;
        public float MeleeDelay = 2f;
        public float MeleeTimer = 0f;

        public float RangeDelay = 1f;
        public float RangeTimer = 0f;
        public bool hasShot;
        public int State = 0;

        public Vector2 initialPosition;

        public Bomber(Vector2 Position, World containerWorld)
            :base(Position, 240, 180, 150, ElementType.Fire, containerWorld)
        {
            IdleAnim = new SpriteSheet(AssetManager.GetSpriteSheet("BomberIdle"), DrawLayer.Middleground3, this);
            RunAnim = new SpriteSheet(AssetManager.GetSpriteSheet("BomberRun"), DrawLayer.Middleground3, this);
            ShotAnim = new SpriteSheet(AssetManager.GetSpriteSheet("BomberRange"), DrawLayer.Middleground3, this);
            ShotAnim.IsLooped = false;
            MeleeAnim = new SpriteSheet(AssetManager.GetSpriteSheet("BomberMelee"), DrawLayer.Middleground3, this);
            MeleeAnim.IsLooped = false;
            LaunchBombAnim = new SpriteSheet(AssetManager.GetSpriteSheet("BomberBomb"), DrawLayer.Middleground3, this);
            LaunchBombAnim.IsLooped = false;
            DeathAnim = new SpriteSheet(AssetManager.GetSpriteSheet("BomberDeath"), DrawLayer.Background1, this);
            DeathAnim.IsLooped = false;

            waypoints = (Waypoints)AddComponent(new Waypoints(this));
            waypoints.FollowPath = false;
            this.initialPosition = Position;

            this.rigidbody.UseGravity = false;
            ChangeAnimation(IdleAnim);
            fight.Remove();
            fight = (Fight)AddComponent(new Fight(this.hitbox, new Rectangle(110, 100, 100, 100), this));
            //AddComponent(new HitboxRenderer(fight, containerWorld));
        }

        public override void Update(GameTime gameTime)
        {
            if (Dying)
            {

                if (DeathAnim.currentFrame == DeathAnim.spriteSheetInfo.FrameCount - 1)
                    DeathAnim.opacity -= 0.02f;

                if (DeathAnim.opacity <= 0)
                    this.Destroy();
            }
            else if(State == 0)
            {
                if (currentSpriteSheet != RunAnim)
                    ChangeAnimation(RunAnim);
                currentSpriteSheet.spriteEffects = SpriteEffects.FlipHorizontally;
                if (Counter < 4)
                {
                    if (TimerSummon > DelaySummon)
                    {
                        Minion m = new Minion(initialPosition + new Vector2(-100, 0) + new Vector2(0, this.hitbox.Height / 2 - 32), ElementType.Steel, 0, containerWorld, 200f);
                        m.waypoints.AddWaypoint(initialPosition + new Vector2(-100,this.hitbox.Height / 2 - 32));
                        m.waypoints.AddWaypoint(initialPosition + new Vector2(-1800, this.hitbox.Height / 2 - 32));
                        containerWorld.AddEntity(m);
                        Counter++;
                        TimerSummon = 0f;
                        DelaySummon = (float)new Random().NextDouble() * 0.7f + 0.05f;
                    }
                    else
                    {
                        TimerSummon += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }

                }
                else if (!waypoints.FollowPath)
                {
                    waypoints.FollowPath = true;
                    Counter = -3;
                }
                else if (waypoints.FollowPath)
                {
                    transform.Position += new Vector2(-150, 0) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                if (transform.Position.X < 22 && transform.Position.X > 0)
                {
                    waypoints.FollowPath = false;
                    waypoints.waypoints.Clear();
                    State = 1;
                    waypoints.Remove();
                }        
            }
            else if (State == 1)
            {
                TimerBomb += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (MeleeAttacking)
                {
                        if(currentSpriteSheet != MeleeAnim)
                        {
                            MeleeAnim.SetFrame(0);
                            ChangeAnimation(MeleeAnim);
                        }
                        else if(MeleeAnim.currentFrame == 15)
                        {
                            
                        }
                        else if (MeleeAnim.currentFrame == MeleeAnim.spriteSheetInfo.FrameCount - 1)
                        {
                            MeleeAttacking = false;
                            MeleeAnim.SetFrame(0);
                        }
                }
                else if (TimerBomb > DelayBomb)
                {
                    TimerBomb = 0;
                    LaunchBombAnim.SetFrame(0);
                    ChangeAnimation(LaunchBombAnim);
                    LaunchBombAnim.isPlaying = true;
                    State = 2;
                }
                else
                {
                    if (avatar.transform.Position.X <= this.transform.Position.X)
                    {
                        CurrentSide = SideDirection.Left;
                        currentSpriteSheet.spriteEffects = SpriteEffects.FlipHorizontally;
                    }
                    else
                    {
                        CurrentSide = SideDirection.Right;
                        currentSpriteSheet.spriteEffects = SpriteEffects.None;
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
                    CoordinateConversion.screenToWorld(this.transform.Position + new Vector2((CurrentSide == SideDirection.Left ? -this.hitbox.Width / 2 - 40 : this.hitbox.Width / 2 + 40), this.hitbox.Height / 2)));

                    if (PlayerInRange)
                    {
                        StartAttack();
                    }
                    else
                    {
                        RangeTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                        if (RangeTimer > RangeDelay)
                        {
                            if (currentSpriteSheet != ShotAnim)
                            {
                                ShotAnim.SetFrame(0);
                                ChangeAnimation(ShotAnim);
                            }
                            else if (ShotAnim.currentFrame == 7 && !hasShot)
                            {
                                if (CurrentSide == SideDirection.Right)
                                    containerWorld.AddEntity(new Bullet(this.transform.Position + new Vector2(150, 0), "Bullet", Vector2.UnitX, containerWorld));
                                else
                                    containerWorld.AddEntity(new Bullet(this.transform.Position + new Vector2(-190, 0), "Bullet", -Vector2.UnitX, containerWorld));
                                hasShot = true;
                            }
                            else if (ShotAnim.currentFrame == ShotAnim.spriteSheetInfo.FrameCount - 1)
                            {
                                hasShot = false;
                                ShotAnim.SetFrame(0);
                                RangeTimer = 0f;
                            }
                        }
                        else
                        {
                            if (currentSpriteSheet != IdleAnim)
                                ChangeAnimation(IdleAnim);
                        }
                    }
                }
            }
            else if (State == 2)
            {
                if (BombSpawned == null)
                {
                    if (LaunchBombAnim.currentFrame == 12)
                    {
                        BombSpawned = new Bomb(this.transform.Position + (CurrentSide == SideDirection.Left ? new Vector2(-70, -50) : new Vector2(70, -50)), this.containerWorld, this);
                        containerWorld.entities.Add(BombSpawned);
                    }
                }
                else
                {
                    if (BombExploded)
                    {
                        BombSpawned = null;
                        State = 1;
                        BombExploded = false;
                    }
                }
                if (currentSpriteSheet != IdleAnim && LaunchBombAnim.currentFrame == LaunchBombAnim.spriteSheetInfo.FrameCount - 3)
                    ChangeAnimation(IdleAnim);
            }

            base.Update(gameTime);
        }
           
        public void StartAttack()
        {
            MeleeTimer = 0f;
            MeleeAttacking = true;
            ChangeAnimation(MeleeAnim);
        }

        public override void Die(bool NoElement = false)
        {
            ChangeAnimation(DeathAnim);
            base.Die(NoElement);
        }
    }
}
