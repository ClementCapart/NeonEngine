using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace NeonEngine.Private
{
    public class Waypoints : Component
    {
        public bool ReachedWaypoint = true;

        public float WaypointWaitTimer = 1.5f;
        public float WaypointWaitDelay = 1.5f;

        public List<Vector2> waypoints = new List<Vector2>();
        int CurrentNode;

        public bool FollowPath = true;
        public float Speed = 100f;
        Rigidbody rigidbody;

        public Side direction;

        public Waypoints(Entity entity)
            :base(entity, "Waypoints")
        {
            rigidbody = entity.GetComponent<Rigidbody>();
        }

        public override void Update(GameTime gameTime)
        {
            if (waypoints.Count > 0)
                if (FollowPath)
                {
                    if (!ReachedWaypoint)
                    {
                        Vector2 NextNode;
                        if (CurrentNode == waypoints.Count - 1)
                            NextNode = waypoints[0];
                        else
                            NextNode = waypoints[CurrentNode + 1];

                        float Distance = Vector2.Distance(NextNode, waypoints[CurrentNode]);
                        Vector2 Direction = NextNode - waypoints[CurrentNode];
                        direction = Direction.X < 0 ? Side.Left : Side.Right;
                        Direction.Normalize();
                        float CurrentDistance = Vector2.Distance(entity.transform.Position, waypoints[CurrentNode]);

                        if (CurrentDistance < Distance)
                        {
                                entity.transform.Position += Direction * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        }
                            
                        else
                            ReachedWaypoint = true;
                        
                    }
                    else
                    {
                        if (WaypointWaitTimer > WaypointWaitDelay)
                        {
                            ReachedWaypoint = false;
                            WaypointWaitTimer = 0f;
                            if (CurrentNode == waypoints.Count - 1)
                                CurrentNode = 0;
                            else
                                CurrentNode++;
                        }
                        WaypointWaitTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                }

            base.Update(gameTime);
        }

        public void AddWaypoint(Vector2 wp)
        {
            waypoints.Add(wp);
        }
    }
}
