using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace NeonEngine.Private
{
    public class Beacon : Component
    {   
        public Hitbox hitbox;
        public FarseerPhysics.Dynamics.World  PhysicWorld;

        Vector2 GroundRaycast = new Vector2();
        Vector2 GroundRaycastTarget = new Vector2();

        Vector2 LeftRaycast = new Vector2();
        Vector2 LeftRaycastTarget = new Vector2();

        Vector2 RightRaycast = new Vector2();
        Vector2 RightRaycastTarget = new Vector2();

        Vector2 RearGroundRaycast = new Vector2();
        Vector2 RearGroundRaycastTarget = new Vector2();

        Vector2 FrontGroundRaycast = new Vector2();
        Vector2 FrontGroundRaycastTarget = new Vector2();

        Vector2 LeftBottomRaycast = new Vector2();
        Vector2 LeftTopRaycast = new Vector2();
        Vector2 LeftBottomRaycastTarget = new Vector2();
        Vector2 LeftTopRaycastTarget = new Vector2();

        Vector2 RightBottomRaycast = new Vector2();
        Vector2 RightBottomRaycastTarget = new Vector2();
        Vector2 RightTopRaycast = new Vector2();
        Vector2 RightTopRaycastTarget = new Vector2();

        public Beacon(Hitbox hitbox, FarseerPhysics.Dynamics.World PhysicWorld)
            :base(hitbox.entity, "Beacon")
        {
            this.hitbox = hitbox;
            this.PhysicWorld = PhysicWorld;
            RefreshRaycastPosition();
        }

        public void RefreshRaycastPosition()
        {
            GroundRaycast = new Vector2(hitbox.X - hitbox.OffsetX + hitbox.Width / 2, hitbox.Y - hitbox.OffsetY + hitbox.Height - 5);
            GroundRaycastTarget = GroundRaycast + new Vector2(0, 8);

            RearGroundRaycast = GroundRaycastTarget;
            RearGroundRaycastTarget = RearGroundRaycast + new Vector2(-hitbox.Width / 2 - 2, 0);

            FrontGroundRaycast = GroundRaycastTarget;
            FrontGroundRaycastTarget = FrontGroundRaycast + new Vector2(hitbox.Width / 2 + 2, 0);

            LeftRaycast = new Vector2(hitbox.X + 5, hitbox.Y + hitbox.Height / 2);
            LeftRaycastTarget = LeftRaycast + new Vector2(-10, 0);
            LeftBottomRaycast = LeftRaycastTarget;
            LeftBottomRaycastTarget = LeftRaycastTarget + new Vector2(0, hitbox.Height / 2);
            LeftTopRaycast = LeftRaycastTarget;
            LeftTopRaycastTarget = LeftRaycastTarget + new Vector2(0, -hitbox.Height / 2);

            RightRaycast = new Vector2(hitbox.X + hitbox.Width - 5, hitbox.Y + hitbox.Height / 2);
            RightRaycastTarget = RightRaycast + new Vector2(10, 0);
            RightBottomRaycast = RightRaycastTarget;
            RightBottomRaycastTarget = RightRaycastTarget + new Vector2(0, hitbox.Height / 2);
            RightTopRaycast = RightRaycastTarget;
            RightTopRaycastTarget = RightRaycastTarget + new Vector2(0, -hitbox.Height / 2);                     
        }

        public override void Update(GameTime gameTime)
        {
            RefreshRaycastPosition();
            base.Update(gameTime);
        }

        public Rigidbody CheckGround(Body body = null)
        {
            Rigidbody CurrentGround = null;
            bool hasHit = false;

            PhysicWorld.RayCast((fixture, hitPosition, normal, fraction) =>
            {
                if (fixture.Body != body && fixture.CollisionCategories == Category.Cat1)
                {
                    CurrentGround = Neon.utils.GetEntityByBody(fixture.Body).GetComponent<Rigidbody>();
                    hasHit = true;
                    return 0;
                }
                return -1;
            },
            CoordinateConversion.screenToWorld(GroundRaycast),
            CoordinateConversion.screenToWorld(GroundRaycastTarget));

            if (!hasHit)
            {
                PhysicWorld.RayCast((fixture, hitPosition, normal, fraction) =>
                {
                    if (fixture.Body != body)
                    {
                        CurrentGround = Neon.utils.GetEntityByBody(fixture.Body).GetComponent<Rigidbody>();
                        hasHit = true;
                        return 0;
                    }
                    return -1;
                },
                CoordinateConversion.screenToWorld(FrontGroundRaycast),
                CoordinateConversion.screenToWorld(FrontGroundRaycastTarget));

                PhysicWorld.RayCast((fixture, hitPosition, normal, fraction) =>
                {
                    if (fixture.Body != body)
                    {
                        CurrentGround = Neon.utils.GetEntityByBody(fixture.Body).GetComponent<Rigidbody>();
                        hasHit = true;
                        return 0;
                    }
                    return -1;
                },
                CoordinateConversion.screenToWorld(RearGroundRaycast),
                CoordinateConversion.screenToWorld(RearGroundRaycastTarget));
            }

            return CurrentGround;
        }

        public bool CheckLeftSide(Body body = null)
        {
            bool hasHit = false;

            PhysicWorld.RayCast((fixture, hitPosition, normal, fraction) =>
            {
                if (fixture.Body != body && fixture.CollisionCategories == Category.Cat1)
                {
                    hasHit = true;
                    return 0;
                }
                return -1;
            },
            CoordinateConversion.screenToWorld(LeftRaycast),
            CoordinateConversion.screenToWorld(LeftRaycastTarget));

            if (!hasHit)
            {
                PhysicWorld.RayCast((fixture, hitPosition, normal, fraction) =>
                {
                    if (fixture.Body != body && fixture.CollisionCategories == Category.Cat1)
                    {
                        hasHit = true;
                        return 0;
                    }
                    return -1;
                },
                CoordinateConversion.screenToWorld(LeftBottomRaycast),
                CoordinateConversion.screenToWorld(LeftBottomRaycastTarget));

                PhysicWorld.RayCast((fixture, hitPosition, normal, fraction) =>
                {
                    if (fixture.Body != body && fixture.CollisionCategories == Category.Cat1)
                    {
                        hasHit = true;
                        return 0;
                    }
                    return -1;
                },
                CoordinateConversion.screenToWorld(LeftTopRaycast),
                CoordinateConversion.screenToWorld(LeftTopRaycastTarget));
            }

            return hasHit;
        }

        public bool CheckRightSide(Body body = null)
        {
            bool hasHit = false;

            PhysicWorld.RayCast((fixture, hitPosition, normal, fraction) =>
            {
                if (fixture.Body != body && fixture.CollisionCategories == Category.Cat1)
                {
                    hasHit = true;
                    return 0;
                }
                return -1;
            },
            CoordinateConversion.screenToWorld(RightRaycast),
            CoordinateConversion.screenToWorld(RightRaycastTarget));

            if (!hasHit)
            {
                PhysicWorld.RayCast((fixture, hitPosition, normal, fraction) =>
                {
                    if (fixture.Body != body && fixture.CollisionCategories == Category.Cat1)
                    {
                        hasHit = true;
                        return 0;
                    }
                    return -1;
                },
                CoordinateConversion.screenToWorld(RightBottomRaycast),
                CoordinateConversion.screenToWorld(RightBottomRaycastTarget));

                PhysicWorld.RayCast((fixture, hitPosition, normal, fraction) =>
                {
                    if (fixture.Body != body && fixture.CollisionCategories == Category.Cat1)
                    {
                        hasHit = true;
                        return 0;
                    }
                    return -1;
                },
                CoordinateConversion.screenToWorld(RightTopRaycast),
                CoordinateConversion.screenToWorld(RightTopRaycastTarget));
            }


            return hasHit;
        }

        public bool CheckCeiling(Body body = null)
        {
            bool hasHit = false;

            return hasHit;
        }

        public bool CheckTopLeft(Body body = null)
        {
            bool hasHit = false;

            return hasHit;
        }

        public bool CheckTopRight(Body body = null)
        {
            bool hasHit = false;

            return hasHit;
        }


    }
}
