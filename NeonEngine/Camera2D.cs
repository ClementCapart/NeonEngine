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
        private Vector2 _lastLegitPosition;

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
                Vector2 NewPosition = Position - (Position - targetObject.transform.Position) / Math.Max((1250 / Vector2.Distance(Position, targetObject.transform.Position)), 1);

                foreach (CameraBound cb in CameraBounds)
                {
                    if (cb.entity.ViewedByCamera(NewPosition))
                    {
                        switch (cb.BoundSide)
                        {
                            case Side.Up:
                                NewPosition = new Vector2(NewPosition.X, MathHelper.Clamp(NewPosition.Y, cb.entity.transform.Position.Y + Neon.HalfScreen.Y / Zoom, float.MaxValue));
                                break;

                            case Side.Right:
                                NewPosition = new Vector2(MathHelper.Clamp(NewPosition.X, float.MinValue, cb.entity.transform.Position.X - Neon.HalfScreen.X / Zoom), NewPosition.Y);
                                break;

                            case Side.Down:
                                NewPosition = new Vector2(NewPosition.X, MathHelper.Clamp(NewPosition.Y, float.MinValue, cb.entity.transform.Position.Y - Neon.HalfScreen.Y / Zoom));
                                break;

                            case Side.Left:
                                NewPosition = new Vector2(MathHelper.Clamp(NewPosition.X, cb.entity.transform.Position.X + Neon.HalfScreen.X / Zoom, float.MaxValue), NewPosition.Y);
                                break;
                        }
                    }                   
                }

                Position = NewPosition;
            }
            else
                Position -= (Position - targetObject.transform.Position) / Math.Max((1250 / Vector2.Distance(Position, targetObject.transform.Position)), 1);
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
