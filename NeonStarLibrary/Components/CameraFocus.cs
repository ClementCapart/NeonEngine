using Microsoft.Xna.Framework;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.Camera
{
    public class CameraFocus : Component
    {
        #region Properties
        private float _maxDistanceX = 500.0f;

        public float MaxDistanceX
        {
            get { return _maxDistanceX; }
            set { _maxDistanceX = value; }
        }

        private float _maxDistanceY = 400.0f;

        public float MaxDistanceY
        {
            get { return _maxDistanceY; }
            set { _maxDistanceY = value; }
        }

        private float _movementRate = 500.0f;

        public float MovementRate
        {
            get { return _movementRate; }
            set { _movementRate = value; }
        }

        private float _forceFeedbackRate = 700.0f;

        public float ForceFeedbackRate
        {
            get { return _forceFeedbackRate; }
            set { _forceFeedbackRate = value; }
        }

        #endregion

        public Vector2 FocusPosition;
        private Vector2 _focusDisplacement = Vector2.Zero;

        public CameraFocus(Entity entity)
            :base(entity, "CameraFocus")
        {
            
        }

        public override void Init()
        {
            base.Init();
        }

        public override void PreUpdate(Microsoft.Xna.Framework.GameTime gameTime)
        {
            FocusPosition = entity.transform.Position;

            if (Neon.Input.Check(NeonStarInput.CameraRight))
            {
                Vector2 newDisplacement = _focusDisplacement;
                newDisplacement.X += (float)gameTime.ElapsedGameTime.TotalSeconds * _movementRate;
                if (newDisplacement.LengthSquared() < _maxDistanceX * _maxDistanceX)
                    _focusDisplacement = newDisplacement;
            }
            else if (Neon.Input.Check(NeonStarInput.CameraLeft))
            {
                Vector2 newDisplacement = _focusDisplacement;
                newDisplacement.X -= (float)gameTime.ElapsedGameTime.TotalSeconds * _movementRate;
                if (newDisplacement.LengthSquared() < _maxDistanceX * _maxDistanceX)
                    _focusDisplacement = newDisplacement;
            }
            else
            {
                if (_focusDisplacement.X > 0.0f)
                {
                    _focusDisplacement.X -= (float)gameTime.ElapsedGameTime.TotalSeconds * _forceFeedbackRate;
                    if (_focusDisplacement.X < 0.0f)
                        _focusDisplacement.X = 0.0f;
                }
                else if (_focusDisplacement.X < 0.0f)
                {
                    _focusDisplacement.X += (float)gameTime.ElapsedGameTime.TotalSeconds * _forceFeedbackRate;
                    if (_focusDisplacement.X > 0.0f)
                        _focusDisplacement.X = 0.0f;
                }
            }

            if (Neon.Input.Check(NeonStarInput.CameraDown))
            {
                Vector2 newDisplacement = _focusDisplacement;
                newDisplacement.Y += (float)gameTime.ElapsedGameTime.TotalSeconds * _movementRate;
                if (newDisplacement.LengthSquared() < _maxDistanceY * _maxDistanceY)
                    _focusDisplacement = newDisplacement;
            }
            else if (Neon.Input.Check(NeonStarInput.CameraUp))
            {
                Vector2 newDisplacement = _focusDisplacement;
                newDisplacement.Y -= (float)gameTime.ElapsedGameTime.TotalSeconds * _movementRate;
                if (newDisplacement.LengthSquared() < _maxDistanceY * _maxDistanceY)
                    _focusDisplacement = newDisplacement;
            }
            else
            {
                if (_focusDisplacement.Y > 0.0f)
                {
                    _focusDisplacement.Y -= (float)gameTime.ElapsedGameTime.TotalSeconds * _forceFeedbackRate;
                    if (_focusDisplacement.Y < 0.0f)
                        _focusDisplacement.Y = 0.0f;
                }
                else if (_focusDisplacement.Y < 0.0f)
                {
                    _focusDisplacement.Y += (float)gameTime.ElapsedGameTime.TotalSeconds * _forceFeedbackRate;
                    if (_focusDisplacement.Y > 0.0f)
                        _focusDisplacement.Y = 0.0f;
                }
            }

            FocusPosition += _focusDisplacement;

            base.PreUpdate(gameTime);
        }
    }
}
