using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine
{
    public enum MoveStyle
    {
        PlatformPatrol, RangePatrol
    }

    public class Mover : Component
    {
        private ComponentRemoved RigidbodyRemoved;

        MoveStyle moveStyle;
        public MoveStyle MoveStyle
        {
            get { return moveStyle; }
            set { moveStyle = value; }
        }

        float range;
        public float Range
        {
            get { return range; }
            set { range = value; }
        }

        float speed;
        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        bool active;
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        Rigidbody rigidbody;
        public Rigidbody Rigidbody
        {
            get { return rigidbody; }
            set
            {                
                rigidbody = value;
                rigidbody.Removed += RigidbodyRemoved;
            }
        }

        public Vector2 startPosition;
        public Side CurrentDirection = Side.Left;

        public Mover(Entity entity)
            :base(entity, "Mover")
        {
            RigidbodyRemoved = new ComponentRemoved(RemoveRigidbody);
            startPosition = entity.transform.Position;
        }

        public override void Init()
        {
            startPosition = entity.transform.Position;
            base.Init();
        }

        public override void Update(GameTime gameTime)
        {
            if (rigidbody != null)
                Console.WriteLine("Hey I'm here !");

            if (active)
            {
                if (MoveStyle == MoveStyle.RangePatrol)
                {
                    if (CurrentDirection == Side.Left)
                    {

                        entity.transform.Position -= new Vector2(speed * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
                        if (Math.Abs(startPosition.X - entity.transform.Position.X) >= range)
                            CurrentDirection = Side.Right;
                    }
                    else if (CurrentDirection == Side.Right)
                    {
                        entity.transform.Position += new Vector2(speed * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
                        if (Math.Abs(startPosition.X - entity.transform.Position.X) >= range)
                            CurrentDirection = Side.Left;
                    }
                }
            }           
            base.Update(gameTime);
        }

        public void RemoveRigidbody(object sender, EventArgs e)
        {
            rigidbody.Removed -= RigidbodyRemoved;
            rigidbody = null;
        }
    }
}
