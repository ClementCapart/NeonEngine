using Microsoft.Xna.Framework;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    public class Guard : Component
    {
        public Avatar AvatarComponent;

        private string _rollAnimation = "";

        public string RollAnimation
        {
            get { return _rollAnimation; }
            set { _rollAnimation = value; }
        }

        private string _dashAnimation = "";

        public string DashAnimation
        {
            get { return _dashAnimation; }
            set { _dashAnimation = value; }
        }

        private string _guardAnimation = "";

        public string GuardAnimation
        {
            get { return _guardAnimation; }
            set { _guardAnimation = value; }
        }

        private float _rollDuration = 0.0f;

        public float RollDuration
        {
            get { return _rollDuration; }
            set { _rollDuration = value; }
        }

        private float _dashDuration = 0.0f;

        public float DashDuration
        {
            get { return _dashDuration; }
            set { _dashDuration = value; }
        }

        private float _guardDuration = 0.0f;

        public float GuardDuration
        {
            get { return _guardDuration; }
            set { _guardDuration = value; }
        }

        private float _rollCooldown = 0.0f;

        public float RollCooldown
        {
            get { return _rollCooldown; }
            set { _rollCooldown = value; }
        }

        private float _dashCooldown = 0.0f;

        public float DashCooldown
        {
            get { return _dashCooldown; }
            set { _dashCooldown = value; }
        }

        private float _guardCooldown = 0.0f;

        public float GuardCooldown
        {
            get { return _guardCooldown; }
            set { _guardCooldown = value; }
        }

        private float _rollImpulse = 0.0f;

        public float RollImpulse
        {
            get { return _rollImpulse; }
            set { _rollImpulse = value; }
        }

        private float _dashImpulse = 0.0f;

        public float DashImpulse
        {
            get { return _dashImpulse; }
            set { _dashImpulse = value; }
        }

        private float _guardDamageReduce = 0.0f;

        public float GuardDamageReduce
        {
            get { return _guardDamageReduce; }
            set { _guardDamageReduce = value; }
        }


        private float _rollCooldownTimer = 0.0f;
        private float _guardCooldownTimer = 0.0f;
        private float _durationTimer = 0.0f;

        private bool isGuarding = false;

        public bool IsGuarding
        {
            get { return isGuarding; }
            set { isGuarding = value; }
        }
        private bool isDashing = false;

        private bool _alreadyDashed = false;

        public Guard(Entity entity)
            :base(entity, "Roll")
        {
        }

        public override void Init()
        {
            AvatarComponent = entity.GetComponent<Avatar>();
            base.Init();
        }

        public override void PreUpdate(GameTime gameTime)
        {
            if (entity.rigidbody.isGrounded)
            {
                _alreadyDashed = false;
            }

            if (_durationTimer > 0f)
            {
                AvatarComponent.thirdPersonController.CanMove = false;
                AvatarComponent.thirdPersonController.CanTurn = false;
                AvatarComponent.meleeFight.CanAttack = false;
                if (isDashing || (isGuarding && !entity.rigidbody.isGrounded))
                    entity.rigidbody.body.GravityScale = 0;
            }
            else if (isDashing)
            {
                isDashing = false;
                entity.rigidbody.body.LinearVelocity = Vector2.Zero;
            }
            else if (isGuarding)
            {
                isGuarding = false;
            }
            base.PreUpdate(gameTime);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (_durationTimer <= 0.0f)
            {
                if (entity.spritesheets.CurrentSpritesheetName == _rollAnimation)
                    entity.spritesheets.CurrentPriority = 0;

                if (_rollCooldownTimer <= 0.0f)
                {
                    if (entity.rigidbody.isGrounded)
                    {
                        _rollCooldownTimer = 0.0f;
                        if (Neon.Input.Pressed(NeonStarInput.Guard) && AvatarComponent.StunLockDuration <= 0.0f)
                        {
                            if (Neon.Input.Check(NeonStarInput.MoveLeft))
                            {
                                entity.spritesheets.ChangeSide(Side.Left);
                                PerformRoll();
                                _durationTimer = _rollDuration;
                                isGuarding = false;
                                isDashing = false;
                            }
                            else if (Neon.Input.Check(NeonStarInput.MoveRight))
                            {
                                entity.spritesheets.ChangeSide(Side.Right);
                                PerformRoll();
                                _durationTimer = _rollDuration;
                                isGuarding = false;
                                isDashing = false;
                            }
                        }
                    }
                    else
                    {
                        _rollCooldownTimer = 0.0f;
                        if (Neon.Input.Pressed(NeonStarInput.Guard) && AvatarComponent.StunLockDuration <= 0.0f && !_alreadyDashed)
                        {
                            if (Neon.Input.Check(NeonStarInput.MoveLeft))
                            {
                                entity.spritesheets.ChangeSide(Side.Left);
                                PerformDash();
                                _durationTimer = _dashDuration;
                                _alreadyDashed = true;
                                isGuarding = false;
                                isDashing = true;
                            }
                            else if (Neon.Input.Check(NeonStarInput.MoveRight))
                            {
                                entity.spritesheets.ChangeSide(Side.Right);
                                PerformDash();
                                _alreadyDashed = true;
                                _durationTimer = _dashDuration;
                                isGuarding = false;
                                isDashing = true;
                            }
                        }
                    }                  
                }
                else
                {
                    _rollCooldownTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                }

                if (_guardCooldownTimer <= 0.0f && _durationTimer <= 0.0f )
                {
                    _guardCooldownTimer = 0.0f;
                    if (Neon.Input.Pressed(NeonStarInput.Guard) && !Neon.Input.Check(NeonStarInput.MoveRight) && !Neon.Input.Check(NeonStarInput.MoveLeft) && AvatarComponent.StunLockDuration <= 0.0f)
                    {
                        PerformGuard();
                        _durationTimer = _guardDuration;
                        isGuarding = true;
                        isDashing = false;
                    }
                }
                else
                {
                    _guardCooldownTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
            else
            {               
                _durationTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_durationTimer <= 0.0f)
                {
                    if (isGuarding)
                        _guardCooldownTimer = _guardCooldown;
                    else if (isDashing)
                        _rollCooldownTimer = _dashCooldown;
                    else
                        _rollCooldownTimer = _rollCooldown;
                }
            }

            base.Update(gameTime);
            
        }

        private void PerformRoll()
        {
            if(AvatarComponent.meleeFight.CurrentAttack != null)
                AvatarComponent.meleeFight.CurrentAttack.CancelAttack();

            entity.rigidbody.body.LinearVelocity = Vector2.Zero;
            entity.rigidbody.body.ApplyLinearImpulse(new Vector2(entity.spritesheets.CurrentSide == Side.Right ? _rollImpulse : -_rollImpulse, 0));
            AvatarComponent.meleeFight.CurrentAttack = null;
            AvatarComponent.meleeFight.ResetComboHit();
            if (AvatarComponent.elementSystem.CurrentElementEffect != null)
                AvatarComponent.elementSystem.CurrentElementEffect.End();
            entity.spritesheets.ChangeAnimation(_rollAnimation, 1, true, true, false);
            entity.hitboxes[0].SwitchType(HitboxType.Invincible, _rollDuration); 
        }

        private void PerformGuard()
        {
            if (AvatarComponent.meleeFight.CurrentAttack != null)
                AvatarComponent.meleeFight.CurrentAttack.CancelAttack();
            entity.rigidbody.GravityScale = 0;
            entity.rigidbody.body.LinearVelocity = Vector2.Zero;
            if(AvatarComponent.elementSystem.CurrentElementEffect != null)
                AvatarComponent.elementSystem.CurrentElementEffect.End();
            AvatarComponent.meleeFight.CurrentAttack = null;
            AvatarComponent.meleeFight.ResetComboHit();
            entity.spritesheets.ChangeAnimation(_guardAnimation, 1, true, true, false);
        }

        private void PerformDash()
        {
            if (AvatarComponent.meleeFight.CurrentAttack != null)
                AvatarComponent.meleeFight.CurrentAttack.CancelAttack();
            AvatarComponent.AirLock(0.0f);
            entity.rigidbody.GravityScale = 0;
            entity.rigidbody.body.LinearVelocity = Vector2.Zero;
            entity.rigidbody.body.ApplyLinearImpulse(new Vector2(entity.spritesheets.CurrentSide == Side.Right ? _dashImpulse : -_dashImpulse, 0));
            AvatarComponent.meleeFight.CurrentAttack = null;
            AvatarComponent.meleeFight.ResetComboHit();
            if (AvatarComponent.elementSystem.CurrentElementEffect != null)
                AvatarComponent.elementSystem.CurrentElementEffect.End();
            entity.spritesheets.ChangeAnimation(_dashAnimation, 1, true, true, false);
            entity.hitboxes[0].SwitchType(HitboxType.Invincible, _dashDuration);
        }
    }
}
