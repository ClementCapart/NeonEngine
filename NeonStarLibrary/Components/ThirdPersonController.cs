using Microsoft.Xna.Framework;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    public class ThirdPersonController : Component
    {
        private float _jumpImpulseHeight = 4.5f;

        public float JumpImpulseHeight
        {
            get { return _jumpImpulseHeight; }
            set { _jumpImpulseHeight = value; }
        }

        private float _groundAccelerationSpeed = 3f;
        public float GroundAccelerationSpeed
        {
            get { return _groundAccelerationSpeed; }
            set { _groundAccelerationSpeed = value; }
        }

        private float _airAccelerationSpeed = 2.5f;
        public float AirAccelerationSpeed
        {
            get { return _airAccelerationSpeed; }
            set { _airAccelerationSpeed = value; }
        }

        private float _groundMaxSpeed = 4f;
        public float GroundMaxSpeed
        {
            get { return _groundMaxSpeed; }
            set { _groundMaxSpeed = value; }
        }

        private float _airMaxSpeed = 3f;
        public float AirMaxSpeed
        {
            get { return _airMaxSpeed; }
            set { _airMaxSpeed = value; }
        }

        private string _idleAnimation;
        public string IdleAnimation
        {
            get { return _idleAnimation; }
            set { _idleAnimation = value; }
        }

        private Side _currentSide = Side.Right;

        public ThirdPersonController(Entity entity)
            :base(entity, "ThirdPersonController")
        {
        }

        public override void Init()
        {
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (entity.rigidbody.isGrounded)
            {
                if (Neon.Input.Check(NeonStarInput.MoveLeft))
                {
                    _currentSide = Side.Left;
                    if (entity.rigidbody.body.LinearVelocity.X > -(_groundMaxSpeed))
                        entity.rigidbody.body.LinearVelocity += new Vector2(-(_groundAccelerationSpeed), 0);
                }
                else if (Neon.Input.Check(NeonStarInput.MoveRight))
                {
                    _currentSide = Side.Left;
                    if (entity.rigidbody.body.LinearVelocity.X < _groundMaxSpeed)
                        entity.rigidbody.body.LinearVelocity += new Vector2(_groundAccelerationSpeed, 0);
                }

                if (Neon.Input.Pressed(NeonStarInput.Jump))
                {
                    entity.rigidbody.body.ApplyLinearImpulse(new Vector2(0, -(_jumpImpulseHeight)));
                }
            }
            else
            {
                if (Neon.Input.Check(NeonStarInput.MoveLeft))
                {
                    _currentSide = Side.Right;
                    if (entity.rigidbody.body.LinearVelocity.X > -(_airMaxSpeed))
                        entity.rigidbody.body.LinearVelocity += new Vector2(-(_airAccelerationSpeed), 0);
                }
                else if (Neon.Input.Check(NeonStarInput.MoveRight))
                {
                    _currentSide = Side.Right;
                    if (entity.rigidbody.body.LinearVelocity.X < _airMaxSpeed)
                        entity.rigidbody.body.LinearVelocity += new Vector2(_airAccelerationSpeed, 0);
                }
                else
                    entity.rigidbody.body.LinearVelocity = new Vector2(entity.rigidbody.body.LinearVelocity.X * 0.95f, entity.rigidbody.body.LinearVelocity.Y);

            }
            
            base.Update(gameTime);
        }
    }
}
