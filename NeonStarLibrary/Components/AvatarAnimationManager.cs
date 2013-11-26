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
        public int LastAttackHashCode = 0;

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

            switch (AvatarComponent.State)
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
                        if (!entity.rigidbody.wasGrounded)
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
                    if (AvatarComponent.MeleeFight.CurrentAttack.Name.StartsWith(AvatarComponent.MeleeFight.LightAttackName))
                    {
                        switch(AvatarComponent.MeleeFight.CurrentComboHit)
                        {
                            case ComboSequence.Starter:
                                if(AvatarComponent.MeleeFight.CurrentAttack.GetHashCode() != LastAttackHashCode)
                                    entity.spritesheets.ChangeAnimation(AvatarComponent.MeleeFight.LightAttackAnimation + "Starter", 0, true, true, false);
                                break;

                            case ComboSequence.Link:
                                if (AvatarComponent.MeleeFight.CurrentAttack.GetHashCode() != LastAttackHashCode)
                                    entity.spritesheets.ChangeAnimation(AvatarComponent.MeleeFight.LightAttackAnimation + "Link", 0, true, true, false);
                                break;

                            case ComboSequence.Finish:
                                if (AvatarComponent.MeleeFight.CurrentAttack.GetHashCode() != LastAttackHashCode)
                                    entity.spritesheets.ChangeAnimation(AvatarComponent.MeleeFight.LightAttackAnimation + "Finish", 0, true, true, false);
                                break;
                        }                   
                    }
                    else if (AvatarComponent.MeleeFight.CurrentAttack.Name.StartsWith(AvatarComponent.MeleeFight.UppercutName))
                    {
                        if (AvatarComponent.MeleeFight.CurrentAttack.GetHashCode() != LastAttackHashCode)
                            entity.spritesheets.ChangeAnimation(AvatarComponent.MeleeFight.UppercutAnimation, 0, true, true, false);
                    }
                    else if (AvatarComponent.MeleeFight.CurrentAttack.Name.StartsWith(AvatarComponent.MeleeFight.RushAttackName))
                    {
                        if (AvatarComponent.MeleeFight.CurrentAttack.GetHashCode() != LastAttackHashCode)
                            entity.spritesheets.ChangeAnimation(AvatarComponent.MeleeFight.RushAttackAnimation, 0, true, true, false);
                    }
                    else if(AvatarComponent.MeleeFight.CurrentAttack.Name.StartsWith(AvatarComponent.MeleeFight.DiveAttackName))
                    {
                        if(entity.spritesheets.CurrentSpritesheetName != AvatarComponent.MeleeFight.DiveAttackLoopAnimation && entity.spritesheets.CurrentSpritesheetName != AvatarComponent.MeleeFight.DiveAttackLandAnimation)
                            if (AvatarComponent.MeleeFight.CurrentAttack.GetHashCode() != LastAttackHashCode)
                                entity.spritesheets.ChangeAnimation(AvatarComponent.MeleeFight.DiveAttackStartAnimation, 0, true, true, false);
                        if(entity.spritesheets.CurrentSpritesheetName == AvatarComponent.MeleeFight.DiveAttackStartAnimation && entity.spritesheets.IsFinished())
                                entity.spritesheets.ChangeAnimation(AvatarComponent.MeleeFight.DiveAttackLoopAnimation, 0, true, true, true);
                        if (entity.spritesheets.CurrentSpritesheetName == AvatarComponent.MeleeFight.DiveAttackLoopAnimation && entity.rigidbody.isGrounded)
                                entity.spritesheets.ChangeAnimation(AvatarComponent.MeleeFight.DiveAttackLandAnimation, 0, true, true, false);
                    }

                    if (AvatarComponent.MeleeFight.CurrentAttack.GetHashCode() != LastAttackHashCode)
                    {
                        LastAttackHashCode = AvatarComponent.MeleeFight.CurrentAttack.GetHashCode();
                    }
                        
                    break;

                case AvatarState.UsingElement:
                    if (AvatarComponent.ElementSystem.CurrentElementEffect.EffectElement == Element.Fire)
                    {
                        switch(AvatarComponent.ElementSystem.CurrentElementEffect.State)
                        {
                            case ElementState.Effect:
                                entity.spritesheets.ChangeAnimation(AvatarComponent.ElementSystem.FireLaunchAnimation, 0, true, false, false);
                                break;
                        }

                    }
                    break;
            }

               
            base.PostUpdate(gameTime);
        }
    }
}
