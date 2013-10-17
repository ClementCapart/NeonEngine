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

        private string _walkAnimation;
        public string WalkAnimation
        {
            get { return _walkAnimation; }
            set { _walkAnimation = value; }
        }

        private string _jumpAnimation;
        public string JumpAnimation
        {
            get { return _jumpAnimation; }
            set { _jumpAnimation = value; }
        }

        private string _startFallAnimation;
        public string StartFallAnimation
        {
            get { return _startFallAnimation; }
            set { _startFallAnimation = value; }
        }

        private string _fallLoopAnimation;
        public string FallLoopAnimation
        {
            get { return _fallLoopAnimation; }
            set { _fallLoopAnimation = value; }
        }

        private string _landingAnimation;
        public string LandingAnimation
        {
            get { return _landingAnimation; }
            set { _landingAnimation = value; }
        }

        private bool _canMove = true;
        public bool CanMove
        {
            get { return _canMove; }
            set { _canMove = value; }
        }

        private bool _canTurn = true;
        public bool CanTurn
        {
            get { return _canTurn; }
            set { _canTurn = value; }
        }

        private Side _currentSide = Side.Right;
        private bool _startJumping = false;
        private List<Rigidbody> _ignoredGeometry = new List<Rigidbody>();
        private MeleeFight _meleeFight;

        public float LastSideChangedDelay = 0.0f;
        public bool _mustJumpAsSoonAsPossible = false;

        private float _maxJumpInputDelay = 0.5f;

        public float MaxJumpInputDelay
        {
            get { return _maxJumpInputDelay; }
            set { _maxJumpInputDelay = value; }
        }

        private float _jumpInputDelay = 0.0f;

        public ThirdPersonController(Entity entity)
            :base(entity, "ThirdPersonController")
        {
        }

        public override void Init()
        {
            _meleeFight = entity.GetComponent<MeleeFight>();

            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            for(int i = _ignoredGeometry.Count - 1; i >= 0; i --)
            {
                if (entity.rigidbody.body.Position.Y > _ignoredGeometry[i].body.Position.Y)
                {
                    entity.rigidbody.body.RestoreCollisionWith(_ignoredGeometry[i].body);
                    _ignoredGeometry.RemoveAt(i);
                }
            }
            LastSideChangedDelay += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(_canTurn)
            {
                if (Neon.Input.Check(NeonStarInput.MoveLeft))
                {
                    if (_currentSide != Side.Left)
                    {
                        _currentSide = Side.Left;
                        LastSideChangedDelay = 0.0f;
                        entity.spritesheets.ChangeSide(_currentSide);
                    }
                    
                }
                else if(Neon.Input.Check(NeonStarInput.MoveRight))
                {
                    if (_currentSide != Side.Right)
                    {
                        _currentSide = Side.Right;
                        LastSideChangedDelay = 0.0f;
                        entity.spritesheets.ChangeSide(_currentSide);
                    }             
                }
            }    

             bool NotMoving = true;

            if (CanMove)
            {
                if (entity.rigidbody.isGrounded && !_startJumping)
                {

                    if (Neon.Input.Check(NeonStarInput.MoveLeft))
                    {
                        if (entity.rigidbody.body.LinearVelocity.X > -(_groundMaxSpeed) && !entity.rigidbody.beacon.CheckLeftSide())
                            entity.rigidbody.body.LinearVelocity += new Vector2(-(_groundAccelerationSpeed), 0);
                        entity.spritesheets.ChangeAnimation(WalkAnimation);
                        entity.spritesheets.ChangeSide(_currentSide);
                        NotMoving = false;
                    }
                    else if (Neon.Input.Check(NeonStarInput.MoveRight))
                    {
                        if (entity.rigidbody.body.LinearVelocity.X < _groundMaxSpeed && !entity.rigidbody.beacon.CheckRightSide())
                            entity.rigidbody.body.LinearVelocity += new Vector2(_groundAccelerationSpeed, 0);
                        entity.spritesheets.ChangeAnimation(WalkAnimation);
                        entity.spritesheets.ChangeSide(_currentSide);

                        NotMoving = false;
                    }

                    if (Neon.Input.PressedComboInput(NeonStarInput.Jump, 0.2, NeonStarInput.MoveDown))
                    {
                        Rigidbody rg = entity.rigidbody.beacon.CheckGround();
                        if (rg != null)
                            if (rg.OneWayPlatform)
                                _ignoredGeometry.Add(rg);

                    }
                    else if (Neon.Input.Pressed(NeonStarInput.Jump))
                    {
                        entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                        entity.rigidbody.body.ApplyLinearImpulse(new Vector2(0, -(_jumpImpulseHeight)));
                        _meleeFight.CurrentComboHit = ComboSequence.None;
                        entity.spritesheets.ChangeAnimation(JumpAnimation, 0, true, false, false, 0);
                        _startJumping = true;
                    }
                    else if (_mustJumpAsSoonAsPossible && _jumpInputDelay < _maxJumpInputDelay && Neon.Input.Check(NeonStarInput.Jump))
                    {
                        entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                        entity.rigidbody.body.ApplyLinearImpulse(new Vector2(0, -(_jumpImpulseHeight)));
                        _meleeFight.CurrentComboHit = ComboSequence.None;
                        entity.spritesheets.ChangeAnimation(JumpAnimation, 0, true, true, false, 0);
                        _startJumping = true;
                        _jumpInputDelay = 0.0f;
                        _mustJumpAsSoonAsPossible = false;
                    }
                    
                    if (_jumpInputDelay >= _maxJumpInputDelay || !Neon.Input.Check(NeonStarInput.Jump))
                    {
                        _jumpInputDelay = 0.0f;
                        _mustJumpAsSoonAsPossible = false;
                    }
                }
                else
                {
                    if (Neon.Input.Check(NeonStarInput.MoveLeft))
                    {
                        _currentSide = Side.Left;
                        if (entity.rigidbody.body.LinearVelocity.X > -(_airMaxSpeed) && !entity.rigidbody.beacon.CheckLeftSide())
                        {
                            entity.rigidbody.body.LinearVelocity += new Vector2(-(_airAccelerationSpeed), 0);
                        }

                        entity.spritesheets.ChangeSide(_currentSide);
                    }
                    else if (Neon.Input.Check(NeonStarInput.MoveRight))
                    {
                        _currentSide = Side.Right;
                        if (entity.rigidbody.body.LinearVelocity.X < _airMaxSpeed && !entity.rigidbody.beacon.CheckRightSide())
                            entity.rigidbody.body.LinearVelocity += new Vector2(_airAccelerationSpeed, 0);

                        entity.spritesheets.ChangeSide(_currentSide);
                    }
                    else
                        entity.rigidbody.body.LinearVelocity = new Vector2(entity.rigidbody.body.LinearVelocity.X * 0.95f, entity.rigidbody.body.LinearVelocity.Y);

                    if (Neon.Input.Pressed(NeonStarInput.Jump))
                    {
                        _mustJumpAsSoonAsPossible = true;
                    }

                    if (entity.rigidbody.body.LinearVelocity.Y > 0)
                    {
                        if (!entity.rigidbody.isGrounded)
                        {
                            if (entity.spritesheets.CurrentSpritesheetName != FallLoopAnimation && entity.spritesheets.CurrentSpritesheetName != StartFallAnimation)
                            {
                                entity.spritesheets.ChangeAnimation(StartFallAnimation, 0, true, false, false, -1);
                            }
                            else if (entity.spritesheets.CurrentSpritesheetName == StartFallAnimation && entity.spritesheets.IsFinished())
                            {
                                entity.spritesheets.ChangeAnimation(FallLoopAnimation, 0, true, false, true, -1);
                            }  
                        }
                        _startJumping = false;                                            
                    }

                    if (_mustJumpAsSoonAsPossible)
                        _jumpInputDelay += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    else
                        _jumpInputDelay = 0.0f;
                    
                }

                if (entity.rigidbody.isGrounded && !entity.rigidbody.wasGrounded && !_startJumping)
                    entity.spritesheets.ChangeAnimation(LandingAnimation, 0, true, false, false, -1);

                if (entity.rigidbody.isGrounded && NotMoving && (entity.spritesheets.CurrentSpritesheet.IsLooped || entity.spritesheets.CurrentSpritesheet.IsFinished || !entity.spritesheets.CurrentSpritesheet.isPlaying))
                {
                    entity.spritesheets.ChangeAnimation(IdleAnimation, 0, true, false, true);                   
                }

            }
            
            foreach (Rigidbody rg in _ignoredGeometry)
                entity.rigidbody.body.IgnoreCollisionWith(rg.body);
            base.Update(gameTime);

        }
    }
}
