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

        private float _rollCooldownTimer = 0.0f;
        private float _guardCooldownTimer = 0.0f;
        private float _durationTimer = 0.0f;

        private bool isGuarding = false;

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
            if (_durationTimer > 0f)
            {
                AvatarComponent.thirdPersonController.CanMove = false;
                AvatarComponent.thirdPersonController.CanTurn = false;
                AvatarComponent.meleeFight.CanAttack = false;
            }
            base.PreUpdate(gameTime);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (_durationTimer <= 0.0f)
            {
                if (entity.spritesheets.CurrentSpritesheetName == _rollAnimation)
                    entity.spritesheets.CurrentPriority = 0;

                if (_rollCooldownTimer <= 0.0f && entity.rigidbody.isGrounded)
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
                        }
                        else if (Neon.Input.Check(NeonStarInput.MoveRight))
                        {
                            entity.spritesheets.ChangeSide(Side.Right);
                            PerformRoll();
                            _durationTimer = _rollDuration;
                            isGuarding = false;
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
                    else
                        _rollCooldownTimer = _rollCooldown;
                }
                AvatarComponent.thirdPersonController.CanMove = false;
                AvatarComponent.thirdPersonController.CanTurn = false;
                AvatarComponent.meleeFight.CanAttack = false;
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
            entity.spritesheets.ChangeAnimation(_rollAnimation, 1, true, true, false);
            entity.hitbox.SwitchType(HitboxType.Invincible, _rollDuration);
            
        }

        private void PerformGuard()
        {
            if (AvatarComponent.meleeFight.CurrentAttack != null)
                AvatarComponent.meleeFight.CurrentAttack.CancelAttack();
            AvatarComponent.meleeFight.CurrentAttack = null;
            AvatarComponent.meleeFight.ResetComboHit();
            entity.spritesheets.ChangeAnimation(_guardAnimation, 1, true, true, false);
        }
    }
}
