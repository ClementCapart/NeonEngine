using Microsoft.Xna.Framework;
using NeonEngine;
using NeonEngine.Components.Graphics2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.Avatar
{
    public class AvatarAnimationManager : Component
    {
        public AvatarCore AvatarComponent = null;
        public int LastAttackHashCode = 0;

        public AvatarAnimationManager(Entity entity)
            : base(entity, "AvatarAnimationManager")
        {
            RequiredComponents = new Type[] { typeof(AvatarCore), typeof(SpritesheetManager) };
        }

        public override void Init()
        {
            AvatarComponent = entity.GetComponent<AvatarCore>();
            base.Init();
        }

        public override void PostUpdate(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (entity.spritesheets != null)
            {
                entity.spritesheets.ChangeSide(AvatarComponent.CurrentSide);
                switch (AvatarComponent.State)
                {

                    case AvatarState.Saving:
                        if (entity.spritesheets != null)
                        {
                            entity.spritesheets.ChangeAnimation(AvatarComponent.ThirdPersonController.IdleAnimation);
                            entity.spritesheets.CurrentSpritesheet.opacity -= 1.0f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        }
                        break;

                    case AvatarState.FinishSaving:
                        if (entity.spritesheets != null)
                        {
                            entity.spritesheets.ChangeAnimation(AvatarComponent.ThirdPersonController.IdleAnimation);
                            entity.spritesheets.CurrentSpritesheet.opacity += 1.0f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        }
                        break;

                    case AvatarState.Respawning:
                        if (entity.spritesheets != null)
                        {
                            entity.spritesheets.CurrentSpritesheet.Reverse = false;
                            entity.spritesheets.Active = true;
                            entity.spritesheets.ChangeAnimation(AvatarComponent.RespawnAnimation, true, 0, false, false, false);
                        }
                        break;

                    case AvatarState.Idle:
                        entity.spritesheets.Active = true;
                        if (entity.rigidbody.isGrounded)
                        {
                            if (entity.spritesheets.CurrentSpritesheetName == AvatarComponent.MeleeFight.DiveAttackLoopAnimation || entity.spritesheets.CurrentSpritesheetName == AvatarComponent.MeleeFight.DiveAttackStartAnimation)
                                entity.spritesheets.ChangeAnimation(AvatarComponent.MeleeFight.DiveAttackLandAnimation, 0, true, false, false);
                            if (entity.rigidbody.wasGrounded)
                            {
                                entity.spritesheets.ChangeAnimation(AvatarComponent.ThirdPersonController.IdleAnimation, false);
                            }
                            else if(entity.rigidbody.body.LinearVelocity.Y >= 0.0f)
                                entity.spritesheets.ChangeAnimation(AvatarComponent.ThirdPersonController.LandingAnimation, true, 0, true, false, false);
                        }
                        else
                        {
                            if (AvatarComponent.ThirdPersonController.StartJumping)
                                entity.spritesheets.ChangeAnimation(AvatarComponent.ThirdPersonController.JumpAnimation, true, 0, true, true, false);
                            else if (entity.spritesheets.CurrentSpritesheetName == AvatarComponent.ThirdPersonController.JumpAnimation)
                                entity.spritesheets.ChangeAnimation(AvatarComponent.ThirdPersonController.StartFallAnimation, 0, true, true, false);
                            else
                                entity.spritesheets.ChangeAnimation(AvatarComponent.ThirdPersonController.FallLoopAnimation, false);
                        }
                        break;

                    case AvatarState.Moving:
                        entity.spritesheets.Active = true;
                        if (entity.rigidbody.isGrounded && !AvatarComponent.ThirdPersonController.StartJumping)
                        {
                            if (!entity.rigidbody.wasGrounded && entity.rigidbody.body.LinearVelocity.Y >= 0.0f)
                                entity.spritesheets.ChangeAnimation(AvatarComponent.ThirdPersonController.LandingAnimation, true, 0, true, false, false);
                            else
                                entity.spritesheets.ChangeAnimation(AvatarComponent.ThirdPersonController.WalkAnimation);
                        }
                        else
                        {
                            if (AvatarComponent.ThirdPersonController.StartJumping)
                                entity.spritesheets.ChangeAnimation(AvatarComponent.ThirdPersonController.JumpAnimation, true, 0, true, true, false);
                            else if (entity.spritesheets.CurrentSpritesheetName == AvatarComponent.ThirdPersonController.JumpAnimation)
                                entity.spritesheets.ChangeAnimation(AvatarComponent.ThirdPersonController.StartFallAnimation, 0, true, false, false);
                            else
                                entity.spritesheets.ChangeAnimation(AvatarComponent.ThirdPersonController.FallLoopAnimation, false);
                        }
                        break;

                    case AvatarState.Guarding:
                        entity.spritesheets.Active = true;
                        if (entity.spritesheets.CurrentSpritesheetName != AvatarComponent.HitGuardAnim)
                            entity.spritesheets.ChangeAnimation(AvatarComponent.Guard.GuardAnimation, true, 0, true, false, false);
                        break;

                    case AvatarState.Rolling:
                        entity.spritesheets.Active = true;
                        entity.spritesheets.ChangeAnimation(AvatarComponent.Guard.RollAnimation, true, 0, true, false, false);
                        break;

                    case AvatarState.AirDashing:
                        entity.spritesheets.Active = true;
                        entity.spritesheets.ChangeAnimation(AvatarComponent.Guard.DashAnimation, true, 0, true, false, false);
                        break;

                    case AvatarState.Attacking:
                        entity.spritesheets.Active = true;
                        if (AvatarComponent.MeleeFight.CurrentAttack == null)
                        {
                            entity.spritesheets.ChangeAnimation(AvatarComponent.ThirdPersonController.IdleAnimation, false);
                        }
                        else if (AvatarComponent.MeleeFight.CurrentAttack.Name.StartsWith(AvatarComponent.MeleeFight.LightAttackName))
                        {
                            switch (AvatarComponent.MeleeFight.CurrentComboHit)
                            {
                                case ComboSequence.Starter:
                                    if (AvatarComponent.MeleeFight.CurrentAttack.GetHashCode() != LastAttackHashCode)
                                    {
                                        entity.spritesheets.ChangeAnimation(AvatarComponent.MeleeFight.LightAttackAnimation + "Starter", true, 0, true, true, false);
                                        if (AvatarComponent.MeleeFight != null)
                                            entity.spritesheets.CurrentSpritesheet.TimePerFrame /= AvatarComponent.MeleeFight.AttackSpeedModifier;
                                    }
                                    break;

                                case ComboSequence.Link:
                                    if (AvatarComponent.MeleeFight.CurrentAttack.GetHashCode() != LastAttackHashCode)
                                    {
                                        entity.spritesheets.ChangeAnimation(AvatarComponent.MeleeFight.LightAttackAnimation + "Link", true, 0, true, true, false);
                                        if (AvatarComponent.MeleeFight != null)
                                            entity.spritesheets.CurrentSpritesheet.TimePerFrame /= AvatarComponent.MeleeFight.AttackSpeedModifier;
                                    }
                                    break;

                                case ComboSequence.Finish:
                                    if (AvatarComponent.MeleeFight.CurrentAttack.GetHashCode() != LastAttackHashCode)
                                    {
                                        entity.spritesheets.ChangeAnimation(AvatarComponent.MeleeFight.LightAttackAnimation + "Finish", true, 0, true, true, false);
                                        if (AvatarComponent.MeleeFight != null)
                                            entity.spritesheets.CurrentSpritesheet.TimePerFrame /= AvatarComponent.MeleeFight.AttackSpeedModifier;
                                    }
                                    break;
                            }
                        }
                        else if (AvatarComponent.MeleeFight.CurrentAttack.Name.StartsWith(AvatarComponent.MeleeFight.UppercutName))
                        {
                            if (AvatarComponent.MeleeFight.CurrentAttack.GetHashCode() != LastAttackHashCode)
                            {
                                entity.spritesheets.ChangeAnimation(AvatarComponent.MeleeFight.UppercutAnimation, true, 0, true, true, false);
                                if (AvatarComponent.MeleeFight != null)
                                    entity.spritesheets.CurrentSpritesheet.TimePerFrame /= AvatarComponent.MeleeFight.AttackSpeedModifier;
                            }
                        }
                        else if (AvatarComponent.MeleeFight.CurrentAttack.Name.StartsWith(AvatarComponent.MeleeFight.RushAttackName))
                        {
                            if (AvatarComponent.MeleeFight.CurrentAttack.GetHashCode() != LastAttackHashCode)
                            {
                                entity.spritesheets.ChangeAnimation(AvatarComponent.MeleeFight.RushAttackAnimation, true, 0, true, true, false);
                                if (AvatarComponent.MeleeFight != null)
                                    entity.spritesheets.CurrentSpritesheet.TimePerFrame /= AvatarComponent.MeleeFight.AttackSpeedModifier;
                            }
                        }
                        else if (AvatarComponent.MeleeFight.CurrentAttack.Name.StartsWith(AvatarComponent.MeleeFight.DiveAttackName))
                        {
                            if (entity.spritesheets.CurrentSpritesheetName != AvatarComponent.MeleeFight.DiveAttackLoopAnimation && entity.spritesheets.CurrentSpritesheetName != AvatarComponent.MeleeFight.DiveAttackLandAnimation)
                                if (AvatarComponent.MeleeFight.CurrentAttack.GetHashCode() != LastAttackHashCode)
                                {
                                    entity.spritesheets.ChangeAnimation(AvatarComponent.MeleeFight.DiveAttackStartAnimation, true, 0, true, true, false);
                                    if (AvatarComponent.MeleeFight != null)
                                        entity.spritesheets.CurrentSpritesheet.TimePerFrame /= AvatarComponent.MeleeFight.AttackSpeedModifier;
                                }
                            if (entity.spritesheets.CurrentSpritesheetName == AvatarComponent.MeleeFight.DiveAttackStartAnimation && entity.spritesheets.IsFinished())
                            {
                                entity.spritesheets.ChangeAnimation(AvatarComponent.MeleeFight.DiveAttackLoopAnimation, true, 0, true, true, true);
                                if (AvatarComponent.MeleeFight != null)
                                    entity.spritesheets.CurrentSpritesheet.TimePerFrame /= AvatarComponent.MeleeFight.AttackSpeedModifier;
                            }

                        }

                        if (AvatarComponent.MeleeFight.CurrentAttack != null && AvatarComponent.MeleeFight.CurrentAttack.GetHashCode() != LastAttackHashCode)
                        {
                            LastAttackHashCode = AvatarComponent.MeleeFight.CurrentAttack.GetHashCode();
                        }

                        break;

                    case AvatarState.UsingElement:
                        if (AvatarComponent.ElementSystem.CurrentElementEffect.EffectElement == Element.Fire)
                        {
                            string fireLaunchAnim = "";
                            string fireLaunchLoopAnim = "";
                            string fireReleaseAnim = "";


                            switch(AvatarComponent.ElementSystem.CurrentElementEffect.ElementLevel)
                            {
                                case 1:
                                    fireLaunchAnim = AvatarComponent.ElementSystem.FireLaunchAnimationLevelOne;
                                    fireLaunchLoopAnim = AvatarComponent.ElementSystem.FireLaunchLoopAnimationLevelOne;
                                    fireReleaseAnim = AvatarComponent.ElementSystem.FireReleaseAnimationLevelOne;
                                    break;

                                case 2:
                                    fireLaunchAnim = AvatarComponent.ElementSystem.FireLaunchAnimationLevelTwo;
                                    fireLaunchLoopAnim = AvatarComponent.ElementSystem.FireLaunchLoopAnimationLevelTwo;
                                    fireReleaseAnim = AvatarComponent.ElementSystem.FireReleaseAnimationLevelTwo;
                                    break;

                                case 3:
                                    fireLaunchAnim = AvatarComponent.ElementSystem.FireLaunchAnimationLevelThree;
                                    fireLaunchLoopAnim = AvatarComponent.ElementSystem.FireLaunchLoopAnimationLevelThree;
                                    fireReleaseAnim = AvatarComponent.ElementSystem.FireReleaseAnimationLevelThree;
                                    break;
                            }

                            switch (AvatarComponent.ElementSystem.CurrentElementEffect.State)
                            {
                                case ElementState.Charge:
                                    if (entity.spritesheets.CurrentSpritesheetName == fireLaunchAnim && entity.spritesheets.IsFinished())
                                    {
                                        entity.spritesheets.ChangeAnimation(fireLaunchLoopAnim, true, 0, true, false, true);
                                    }
                                    else if (entity.spritesheets.CurrentSpritesheetName != fireLaunchLoopAnim)
                                        entity.spritesheets.ChangeAnimation(fireLaunchAnim, true, 0, true, false, false);
                                    break;

                                case ElementState.Effect:
                                    if (entity.spritesheets.CurrentSpritesheetName != fireReleaseAnim)
                                        entity.spritesheets.ChangeAnimation(fireReleaseAnim, true, 0, true, false, false);
                                    break;
                            }
                        }
                        else if (AvatarComponent.ElementSystem.CurrentElementEffect.EffectElement == Element.Thunder)
                        {
                            switch (AvatarComponent.ElementSystem.CurrentElementEffect.State)
                            {
                                case ElementState.Charge:
                                    entity.spritesheets.ChangeAnimation(AvatarComponent.ElementSystem.ThunderLaunchAnimation, true, 0, true, false, false);
                                    break;
                            }
                        }
                        break;
                }
            }
            

               
            base.PostUpdate(gameTime);
        }
    }
}
