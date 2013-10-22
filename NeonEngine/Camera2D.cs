using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.Collections.Generic;

namespace NeonEngine
{
    public class Camera2D
    {
        protected float _zoom; // Camera Zoom
        Matrix _transform; // Matrix Transform
        Vector2 _pos; // Camera Position
        Vector2 _ppos; // camera Position since last frame
        protected float _rotation; // Camera Rotation
        private bool _bounded = false;

        public float mass = 10f;
        public float stiffness = 1000f;
        public float damping = 170f;
        private Vector2 velocity; 

        public List<CameraBound> CameraBounds = new List<CameraBound>();

        public bool Bounded
        {
            get { return _bounded; }
            set { _bounded = value; }
        }

        
        public float Zoom
        {
            get { return _zoom; }
            set { _zoom = value; if (_zoom < 0.1f) _zoom = 0.1f; } // Negative zoom will flip image
        }

        public float Rotation
        {
            get { return _rotation; }
        }

        public Vector2 Position
        {
            get { return _pos; }
            set
            {
                _pos = value;
            }
        }

        public Vector2 PreviousPosition
        {
            get { return _ppos; }
            set { _ppos = value; }
        }

        public Vector2 Delta
        {
            get { return _pos - _ppos; }
        }

        public Camera2D()
        {
            _zoom = 1.0f;
            _rotation = 0.0f;
            _pos = Vector2.Zero;
        }

        public void SmoothFollow(Vector2 targetPosition)
        {
            Position -= (Position - targetPosition) / Math.Max((1250 / Vector2.Distance(Position, targetPosition)), 1);
        }

        public void SmoothFollow(Entity targetObject)
        {         
            if (Bounded && CameraBounds.Count > 0)
            {
                float NewPositionX = Position.X + (float)Math.Sqrt(Math.Pow(Position.X - targetObject.transform.Position.X, 2f));
                float NewPositionY = Position.Y + (float)Math.Sqrt(Math.Pow(Position.Y - targetObject.transform.Position.Y, 2f));

                foreach (CameraBound cb in CameraBounds)
                {
                    if (cb.entity.ViewedByCamera(new Vector2(NewPositionX, NewPositionY)))
                    {
                        switch (cb.BoundSide)
                        {
                            case Side.Up:
                                NewPositionY = MathHelper.Clamp(NewPositionY, cb.entity.transform.Position.Y + Neon.HalfScreen.Y / Zoom, float.MaxValue);
                                break;

                            case Side.Right:
                                NewPositionX =MathHelper.Clamp(NewPositionX, float.MinValue, cb.entity.transform.Position.X - Neon.HalfScreen.X / Zoom);
                                break;

                            case Side.Down:
                                NewPositionY = MathHelper.Clamp(NewPositionY, float.MinValue, cb.entity.transform.Position.Y - Neon.HalfScreen.Y / Zoom);
                                break;

                            case Side.Left:
                                NewPositionX = MathHelper.Clamp(NewPositionX, cb.entity.transform.Position.X + Neon.HalfScreen.X / Zoom, float.MaxValue);
                                break;
                        }
                    }                   
                }

                Position = new Vector2(NewPositionX, NewPositionY);
            }
            else
                Position -= (Position - targetObject.transform.Position) / Math.Max((1250 / Vector2.Distance(Position, targetObject.transform.Position)), 1);
        }

        public void SetCameraPhysics(float mass, float damping, float stiffness)
        {
            this.mass = mass;
            this.damping = damping;
            this.stiffness = stiffness;
        } 

        public void Chase(Vector2 desiredPosition,GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 NewPosition = new Vector2(_pos.X, _pos.Y);

            Vector2 stretch;
            Vector2.Subtract(ref NewPosition, ref desiredPosition, out stretch);

            Vector2 multOne;
            Vector2 multTwo;
            Vector2.Multiply(ref stretch, -stiffness, out multOne);
            Vector2.Multiply(ref velocity, damping, out multTwo);

            Vector2 force;
            Vector2.Subtract(ref multOne, ref multTwo, out force);

            Vector2 acceleration;
            Vector2.Divide(ref force, mass, out acceleration);

            Vector2 multThree;
            Vector2.Multiply(ref acceleration, elapsed, out multThree);

            Vector2.Add(ref velocity, ref multThree, out velocity);

            Vector2 finalVel;
            Vector2.Multiply(ref velocity, elapsed, out finalVel);

            Vector2.Add(ref NewPosition, ref finalVel, out NewPosition);

            if (Bounded && CameraBounds.Count > 0)
            {
                foreach (CameraBound cb in CameraBounds)
                {
                    if (cb.entity.ViewedByCamera(NewPosition))
                    {
                        switch (cb.BoundSide)
                        {
                            case Side.Up:
                                NewPosition.Y = (float)Math.Floor(MathHelper.Clamp(NewPosition.Y, cb.entity.transform.Position.Y + Neon.HalfScreen.Y / Zoom, float.MaxValue));
                                break;

                            case Side.Right:
                                NewPosition.X = (float)Math.Floor(MathHelper.Clamp(NewPosition.X, float.MinValue, cb.entity.transform.Position.X - Neon.HalfScreen.X / Zoom));
                                break;

                            case Side.Down:
                                NewPosition.Y = (float)Math.Floor(MathHelper.Clamp(NewPosition.Y, float.MinValue, cb.entity.transform.Position.Y - Neon.HalfScreen.Y / Zoom));
                                break;

                            case Side.Left:
                                NewPosition.X = (float)Math.Floor(MathHelper.Clamp(NewPosition.X, cb.entity.transform.Position.X + Neon.HalfScreen.X / Zoom, float.MaxValue));
                                break;
                        }
                    }
                }

                
            }

            if (Math.Abs(NewPosition.X - _pos.X) >= 1) _pos.X = NewPosition.X;
            if (Math.Abs(NewPosition.Y - _pos.Y) >= 1) _pos.Y = NewPosition.Y;
        }


        public void Move(int x, int y)
        {
            Position += new Vector2(x, y);
        }

        public void Rotate(float addToAngle)
        {
            _rotation += addToAngle;
        }

        public Matrix get_transformation(GraphicsDevice graphicsDevice)
        {
            _transform =
              Matrix.CreateTranslation(new Vector3(-_pos.X, -_pos.Y, 0)) *
                                         Matrix.CreateRotationZ(Rotation) *
                                         Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                                         Matrix.CreateTranslation(new Vector3(graphicsDevice.Viewport.Width * 0.5f, graphicsDevice.Viewport.Height * 0.5f, 0));
            return _transform;
        }

        public Entity[] GetEntitiesInView(Vector2 cameraPosition)
        {
            return Neon.world.entities.Where(en => en.ViewedByCamera(cameraPosition)).ToArray();
        }
    }
}
