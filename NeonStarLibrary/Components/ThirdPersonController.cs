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
        #region Properties
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

        private float _maxJumpInputDelay = 0.5f;

        public float MaxJumpInputDelay
        {
            get { return _maxJumpInputDelay; }
            set { _maxJumpInputDelay = value; }
        }

        private float _maxFallSpeed = 20.0f;

        public float MaxFallSpeed
        {
            get { return _maxFallSpeed; }
            set { _maxFallSpeed = value; }
        }

        private bool _canDoubleJump = true;

        public bool CanDoubleJump
        {
            get { return _canDoubleJump; }
            set { _canDoubleJump = value; }
        }
        #endregion

        public Avatar AvatarComponent = null;

        public bool StartJumping = false;
        public float LastSideChangedDelay = 0.0f;
        public bool MustJumpAsSoonAsPossible = false;

        private List<Rigidbody> _ignoredGeometry = new List<Rigidbody>();    
        private float _jumpInputDelay = 0.0f;

        private bool _hasAlreadyAirJumped = false;

        public ThirdPersonController(Entity entity)
            :base(entity, "ThirdPersonController")
        {
        }

        public override void Init()
        {
            AvatarComponent = entity.GetComponent<Avatar>();
            base.Init();
        }

        public override void PreUpdate(GameTime gameTime)
        {
            LastSideChangedDelay += (float)gameTime.ElapsedGameTime.TotalSeconds;

            base.PreUpdate(gameTime);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            for(int i = _ignoredGeometry.Count - 1; i >= 0; i --)
            {
                if (entity.transform.Position.Y + entity.hitboxes[0].Height / 2 > _ignoredGeometry[i].entity.transform.Position.Y + _ignoredGeometry[i].entity.hitboxes[0].Height / 2 || entity.rigidbody.body.LinearVelocity.Y <= 0.05f)
                {
                    entity.rigidbody.body.RestoreCollisionWith(_ignoredGeometry[i].body);
                    _ignoredGeometry.RemoveAt(i);
                }
            }

            if(AvatarComponent.CanTurn)
            {
                if(Neon.Input.Check(NeonStarInput.MoveLeft))
                {
                    if (AvatarComponent.CurrentSide != Side.Left)
                    {
                        AvatarComponent.CurrentSide = Side.Left;
                        LastSideChangedDelay = 0.0f;
                    }     
                }
                else if(Neon.Input.Check(NeonStarInput.MoveRight))
                {
                    if (AvatarComponent.CurrentSide != Side.Right)
                    {
                        AvatarComponent.CurrentSide = Side.Right;
                        LastSideChangedDelay = 0.0f;
                    }             
                }
            }    

            if (AvatarComponent.CanMove)
            {
                if (entity.rigidbody.isGrounded && !StartJumping)
                {
                    _hasAlreadyAirJumped = false;
                    if (Neon.Input.Check(NeonStarInput.MoveLeft))
                    {
                        if (entity.rigidbody.body.LinearVelocity.X > -(_groundMaxSpeed) && entity.rigidbody.beacon.CheckLeftSide(1) == null)
                            entity.rigidbody.body.LinearVelocity += new Vector2(-(_groundAccelerationSpeed), 0);

                        AvatarComponent.State = AvatarState.Moving;
                    }
                    else if (Neon.Input.Check(NeonStarInput.MoveRight))
                    {
                        if (entity.rigidbody.body.LinearVelocity.X < _groundMaxSpeed && entity.rigidbody.beacon.CheckRightSide(1) == null)
                            entity.rigidbody.body.LinearVelocity += new Vector2(_groundAccelerationSpeed, 0);

                        AvatarComponent.State = AvatarState.Moving;
                    }
                    else
                    {
                        AvatarComponent.State = AvatarState.Idle;
                        entity.rigidbody.body.LinearVelocity = new Vector2(entity.rigidbody.body.LinearVelocity.X * 0.5f, entity.rigidbody.body.LinearVelocity.Y);
                    }

                    if (Neon.Input.PressedComboInput(NeonStarInput.Jump, 0.2, NeonStarInput.MoveDown))
                    {    
                        Rigidbody rg = entity.rigidbody.beacon.CheckGround();
                         
                        if (rg != null)
                            if (rg.OneWayPlatform)
                            {
                                _ignoredGeometry.Add(rg);
                                StartJumping = true;
                            }
                    }
                    else if (Neon.Input.Pressed(NeonStarInput.Jump))
                    {
                        MustJumpAsSoonAsPossible = true;
                    }

                    if (MustJumpAsSoonAsPossible && _jumpInputDelay < _maxJumpInputDelay && Neon.Input.Check(NeonStarInput.Jump))
                    {
                        entity.rigidbody.body.ApplyLinearImpulse(new Vector2(0, -(_jumpImpulseHeight)));
                        AvatarComponent.MeleeFight.CurrentComboHit = ComboSequence.None;
                        StartJumping = true;
                        EffectsManager.GetEffect(AssetManager.GetSpriteSheet("FXJumpUP"), AvatarComponent.CurrentSide, entity.transform.Position, 0, new Vector2(0, 22), 2.0f,entity.spritesheets.DrawLayer + 0.01f);
                        _jumpInputDelay = 0.0f;
                        MustJumpAsSoonAsPossible = false;
                    }

                    if (_jumpInputDelay >= _maxJumpInputDelay || !Neon.Input.Check(NeonStarInput.Jump))
                    {
                        _jumpInputDelay = 0.0f;
                        MustJumpAsSoonAsPossible = false;
                    }

                    
                }
                else
                {
                    if (Neon.Input.Check(NeonStarInput.MoveLeft))
                    {
                        AvatarComponent.State = AvatarState.Moving;
                        AvatarComponent.CurrentSide = Side.Left;

                        if (entity.rigidbody.body.LinearVelocity.X > -(_airMaxSpeed) && entity.rigidbody.beacon.CheckLeftSide(0) == null)
                            entity.rigidbody.body.LinearVelocity += new Vector2(-(_airAccelerationSpeed), 0);
                    }
                    else if (Neon.Input.Check(NeonStarInput.MoveRight))
                    {
                        AvatarComponent.State = AvatarState.Moving;
                        AvatarComponent.CurrentSide = Side.Right;

                        if (entity.rigidbody.body.LinearVelocity.X < _airMaxSpeed && entity.rigidbody.beacon.CheckRightSide(0) == null)
                            entity.rigidbody.body.LinearVelocity += new Vector2(_airAccelerationSpeed, 0);
                    }
                    else if (AvatarComponent.CanMove)
                    {
                        entity.rigidbody.body.LinearVelocity = new Vector2(entity.rigidbody.body.LinearVelocity.X * 0.95f, entity.rigidbody.body.LinearVelocity.Y);
                        AvatarComponent.State = AvatarState.Idle;
                    }


                    if (Neon.Input.Pressed(NeonStarInput.Jump))
                    {
                        MustJumpAsSoonAsPossible = true;
                    }

                    if (MustJumpAsSoonAsPossible && !_hasAlreadyAirJumped && Neon.Input.Check(NeonStarInput.Jump) && CanDoubleJump)
                    {
                        entity.rigidbody.body.LinearVelocity = new Vector2(entity.rigidbody.body.LinearVelocity.X, 0);
                        entity.rigidbody.body.ApplyLinearImpulse(new Vector2(0, -(_jumpImpulseHeight)));
                        AvatarComponent.MeleeFight.CurrentComboHit = ComboSequence.None;
                        StartJumping = true;
                        EffectsManager.GetEffect(AssetManager.GetSpriteSheet("FXJumpUP"), AvatarComponent.CurrentSide, entity.transform.Position, 0, new Vector2(0, 22), 2.0f, entity.spritesheets.DrawLayer + 0.01f);
                        _jumpInputDelay = 0.0f;
                        MustJumpAsSoonAsPossible = false;
                        _hasAlreadyAirJumped = true;
                    }

                    if (entity.rigidbody.body.LinearVelocity.Y > 0)
                        StartJumping = false;

                    if (MustJumpAsSoonAsPossible)
                        _jumpInputDelay += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    else
                        _jumpInputDelay = 0.0f;

                }
            }

            if(entity.rigidbody.isGrounded && !entity.rigidbody.wasGrounded && entity.rigidbody.body.LinearVelocity.Y >= 0)
                EffectsManager.GetEffect(AssetManager.GetSpriteSheet("FXJumpDOWN"), AvatarComponent.CurrentSide, entity.transform.Position, 0, new Vector2(0, 56), 2.0f, entity.spritesheets.DrawLayer + 0.01f);

            if (entity.rigidbody.body.LinearVelocity.Y > _maxFallSpeed)
                entity.rigidbody.body.LinearVelocity = new Vector2(entity.rigidbody.body.LinearVelocity.X, _maxFallSpeed);

            foreach (Rigidbody rg in _ignoredGeometry)
                entity.rigidbody.body.IgnoreCollisionWith(rg.body);
            base.Update(gameTime);
        }
    }
}
