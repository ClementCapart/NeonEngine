using Microsoft.Xna.Framework;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine.Components.Camera
{
    public class CameraBound : Component
    {
        Side _boundSide;

        private bool _reverseBound = false;

        public bool ReverseBound
        {
            get { return _reverseBound; }
            set { _reverseBound = value; }
        }

        private bool _softBound = false;

        public bool SoftBound
        {
            get { return _softBound; }
            set { _softBound = value; }
        }

        public Side BoundSide
        {
            get { return _boundSide; }
            set { _boundSide = value; }
        }

        private float _boundStrength = 1.0f;

        public float BoundStrength
        {
            get { return _boundStrength; }
            set { _boundStrength = value; }
        }

        private float _softBoundStrength = 1.0f;

        public float SoftBoundStrength
        {
            get { return _softBoundStrength; }
            set { _softBoundStrength = value; }
        }

        private float _strengtheningRate = 0.5f;

        public float StrengtheningRate
        {
            get { return _strengtheningRate; }
            set { _strengtheningRate = value; }
        }

        private bool _enabled = true;

        public bool Enabled
        {
            get { return _enabled; }
            set 
            { 
                _enabled = value;
                if (!_enabled && entity.GameWorld.Camera.CameraBounds.Contains(this))
                {
                    entity.GameWorld.Camera.CameraBounds.Remove(this);
                }
                else if (_enabled && !entity.GameWorld.Camera.CameraBounds.Contains(this))
                {
                    entity.GameWorld.Camera.CameraBounds.Add(this);
                }
            }
        }

        public CameraBound(Entity entity)
            :base(entity, "CameraBound")
        {
        }

        public override void Init()
        {
            if (_softBound)
                _boundStrength = 0.0f;
            base.Init();
        }

        public override void Remove()
        {
            if(_enabled)
                entity.GameWorld.Camera.CameraBounds.Remove(this);
            base.Remove();
        }

        public override void PreUpdate(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (_softBound)
            {
                Vector2 positionCamera = entity.GameWorld.Camera.Position;

                switch (BoundSide)
                {
                    case Side.Left:
                        positionCamera.X -= 100f;
                        break;

                    case Side.Right:
                        positionCamera.X += 100f;
                        break;

                    case Side.Up:
                        positionCamera.Y -= 100f;
                        break;

                    case Side.Down:
                        positionCamera.Y += 100f;
                        break;
                }

                if (entity.GameWorld.MustSoftenBounds)
                {
                    if (entity.GameWorld.Camera.GetEntitiesInView(positionCamera).Contains(entity))
                    // if (new Rectangle((int)(entity.GameWorld.Camera.BasePosition.X - Neon.HalfScreen.X / entity.GameWorld.Camera.Zoom), (int)(entity.GameWorld.Camera.BasePosition.Y - Neon.HalfScreen.Y / entity.GameWorld.Camera.Zoom), (int)(Neon.ScreenWidth / entity.GameWorld.Camera.Zoom), (int)(Neon.ScreenHeight / entity.GameWorld.Camera.Zoom)).Contains((int)entity.transform.Position.X, (int)entity.transform.Position.Y))
                    {
                        if (!_reverseBound)
                        {
                            if (_boundStrength < 1.0f)
                                _boundStrength += (float)gameTime.ElapsedGameTime.TotalSeconds * _strengtheningRate;

                            if (_boundStrength > 1.0f)
                                _boundStrength = 1.0f;
                        }
                        else
                        {
                            if (_boundStrength > 0.0f)
                                _boundStrength -= (float)gameTime.ElapsedGameTime.TotalSeconds * _strengtheningRate;

                            if (_boundStrength < 0.0f)
                                _boundStrength = 0.0f;
                        }
                    }
                    else
                    {
                        /*if (!_reverseBound)
                        {
                            if (_boundStrength > 0.0f)
                                _boundStrength -= (float)gameTime.ElapsedGameTime.TotalSeconds * _strengtheningRate;

                            if (_boundStrength < 0.0f)
                                _boundStrength = 0.0f;

                        }
                        else
                        {
                            if (_boundStrength < 1.0f)
                                _boundStrength += (float)gameTime.ElapsedGameTime.TotalSeconds * _strengtheningRate;

                            if (_boundStrength > 1.0f)
                                _boundStrength = 1.0f;
                        }*/
                    }
                }
               
            }
            
            
            base.PreUpdate(gameTime);
        }
    }
}
