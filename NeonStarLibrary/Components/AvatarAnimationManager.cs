using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    public class AvatarAnimationManager : Component
    {
        public Avatar AvatarComponent = null;

        public AvatarAnimationManager(Entity entity)
            : base(entity, "AvatarAnimationManager")
        {
        }

        public override void Init()
        {
            AvatarComponent = entity.GetComponent<Avatar>();
            base.Init();
        }

        public override void PostUpdate(Microsoft.Xna.Framework.GameTime gameTime)
        {
            entity.spritesheets.ChangeSide(AvatarComponent.CurrentSide);

            switch(AvatarComponent.State)
            {
                case AvatarState.Idle:
                    if (entity.rigidbody.isGrounded)
                    {
                        if (entity.rigidbody.wasGrounded)
                            entity.spritesheets.ChangeAnimation(AvatarComponent.ThirdPersonController.IdleAnimation, false);
                        else
                            entity.spritesheets.ChangeAnimation(AvatarComponent.ThirdPersonController.LandingAnimation, true, 0, true, false, false);
                    }
                    else
                    {
                        if (AvatarComponent.ThirdPersonController.StartJumping)
                            entity.spritesheets.ChangeAnimation(AvatarComponent.ThirdPersonController.JumpAnimation, true, 0, true, false, false);
                        else
                            entity.spritesheets.ChangeAnimation(AvatarComponent.ThirdPersonController.FallLoopAnimation);
                    }
                    break;

                case AvatarState.Moving:
                    if (entity.rigidbody.isGrounded && !AvatarComponent.ThirdPersonController.StartJumping)
                    {
                        if(!entity.rigidbody.wasGrounded)
                            entity.spritesheets.ChangeAnimation(AvatarComponent.ThirdPersonController.LandingAnimation, true, 0, true, false, false);
                        else
                            entity.spritesheets.ChangeAnimation(AvatarComponent.ThirdPersonController.WalkAnimation);
                    
                    }    
                    else
                    {
                        if (AvatarComponent.ThirdPersonController.StartJumping)
                            entity.spritesheets.ChangeAnimation(AvatarComponent.ThirdPersonController.JumpAnimation, true, 0, true, false, false);
                        else
                            entity.spritesheets.ChangeAnimation(AvatarComponent.ThirdPersonController.FallLoopAnimation);
                    }
                    break;

                case AvatarState.Guarding:
                    entity.spritesheets.ChangeAnimation(AvatarComponent.Guard.GuardAnimation, true, 0, true, false, false);
                    break;

                case AvatarState.Rolling:
                    entity.spritesheets.ChangeAnimation(AvatarComponent.Guard.RollAnimation, true, 0, true, false, false);
                    break;

                case AvatarState.AirDashing:
                    entity.spritesheets.ChangeAnimation(AvatarComponent.Guard.DashAnimation, true, 0, true, false, false);
                    break;

                case AvatarState.Attacking:
                    break;
            }

               
            base.PostUpdate(gameTime);
        }
    }
}
