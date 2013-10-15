using Microsoft.Xna.Framework;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    public enum ComboSequence
    {
        None,
        Starter,
        Link,
        Finish
    }

    public class MeleeFight : Component
    {
        private bool _Debug = false;

        public bool Debug
        {
            get { return _Debug; }
            set { _Debug = value; }
        }

        public Attack CurrentAttack;

        private float _comboDelayMax = 1.0f;
        public float ComboDelayMax
        {
            get { return _comboDelayMax; }
            set { _comboDelayMax = value; }
        }

        private string _lightAttackAnimation;
        public string LightAttackAnimation
        {
            get { return _lightAttackAnimation; }
            set { _lightAttackAnimation = value; }
        }

        private string _uppercutAnimation;
        public string UppercutAnimation
        {
            get { return _uppercutAnimation; }
            set { _uppercutAnimation = value; }
        }

        private string _rushAttackAnimation;
        public string RushAttackAnimation
        {
            get { return _rushAttackAnimation; }
            set { _rushAttackAnimation = value; }
        }

        private string _diveAttackStartAnimation;
        public string DiveAttackStartAnimation
        {
            get { return _diveAttackStartAnimation; }
            set { _diveAttackStartAnimation = value; }
        }

        private string _diveAttackLoopAnimation;

        public string DiveAttackLoopAnimation
        {
            get { return _diveAttackLoopAnimation; }
            set { _diveAttackLoopAnimation = value; }
        }

        private string _diveAttackLandAnimation;

        public string DiveAttackLandAnimation
        {
            get { return _diveAttackLandAnimation; }
            set { _diveAttackLandAnimation = value; }
        }

        private float _rushAttackChargeTime;
        public float RushAttackChargeTime
        {
            get { return _rushAttackChargeTime; }
            set { _rushAttackChargeTime = value; }
        }

        private float _rushAttackSideDelay = 1.0f;

        public float RushAttackSideDelay
        {
            get { return _rushAttackSideDelay; }
            set { _rushAttackSideDelay = value; }
        }

        private float _lastHitDelay = 0.0f;
        private bool ReleasedAttackButton = true;

        private float _chainDelay = 0.3f;

        public float ChainDelay
        {
            get { return _chainDelay; }
            set { _chainDelay = value; }
        }

        public List<string> AttacksWhileInAir = new List<string>();

        private float _chainDelayTimer = 0.0f;
        private string NextAttack = "";


        private ThirdPersonController _thirdPersonController;
        public ThirdPersonController ThirdPersonController
        {
            get { return _thirdPersonController; }
            set { _thirdPersonController = value; }
        }

        public ComboSequence LastComboHit = ComboSequence.None;

        private bool _triedAttacking = false;
        private ComboSequence _currentComboHit = ComboSequence.None;

        public ComboSequence CurrentComboHit
        {
            get { return _currentComboHit; }
            set { _currentComboHit = value; }
        }

        public MeleeFight(Entity entity)
            :base(entity, "MeleeFight")
        {
        }

        public override void Init()
        {
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (!ReleasedAttackButton)
                if (Neon.Input.Released(NeonStarInput.Attack))
                    ReleasedAttackButton = true;

            if (entity.rigidbody.isGrounded)
            {
                AttacksWhileInAir.Clear();
            }

            if (CurrentAttack != null && (CurrentAttack.AttackInfo.Name == "DiveAttack" || CurrentAttack.AttackInfo.Name == "DiveAttackFinish"))
            {
                if (entity.spritesheets.CurrentSpritesheetName == DiveAttackStartAnimation && entity.spritesheets.IsFinished())
                {
                    entity.spritesheets.ChangeAnimation(DiveAttackLoopAnimation, 1, true, false, true);
                }
                else if (entity.spritesheets.CurrentSpritesheetName == DiveAttackLoopAnimation && entity.rigidbody.isGrounded)
                {
                    entity.spritesheets.ChangeAnimation(DiveAttackLandAnimation, 1, true, false, false);
                }
            }

            if (CurrentAttack != null)
            {
                if (CurrentAttack.CooldownFinished && CurrentAttack.AirLockFinished && (entity.spritesheets.CurrentSpritesheet.IsLooped || !entity.spritesheets.CurrentSpritesheet.IsLooped && entity.spritesheets.IsFinished()))
                {
                    CurrentAttack = null;
                    entity.spritesheets.CurrentPriority = 0;
                }
                else
                    CurrentAttack.Update(gameTime);

            }

            

            if (CurrentAttack == null)
            {
                if (NextAttack != "")
                {
                    switch(NextAttack)
                    {
                        case "Uppercut":
                            PerformUppercut();
                            break;

                        case "DiveAttack":
                            PerformDiveAttack();
                            break;

                        case "RushAttack":
                            if (entity.spritesheets.CurrentSide == Side.Left)
                                PerformLeftRushAttack();
                            else
                                PerformRightRushAttack();
                            break;

                        case "LightAttack":
                            PerformLightAttack();
                            break;
                    }

                    NextAttack = "";
                }
            } 

            if (ReleasedAttackButton)
            {
                if (Neon.Input.PressedComboInput(NeonStarInput.Attack, 0.3f, NeonStarInput.MoveUp))
                {
                    if (CurrentAttack == null || CurrentAttack != null && (CurrentAttack.CooldownFinished || (CurrentAttack.Type == AttackType.MeleeLight && CurrentAttack.DurationFinished)))
                    {
                        PerformUppercut();
                    }
                    else if(NextAttack == "")
                    {
                        NextAttack = "Uppercut";
                    }
                }
                else if (Neon.Input.PressedComboInput(NeonStarInput.Attack, 0.3f, NeonStarInput.MoveLeft) && Neon.Input.CheckPressedDelay(NeonStarInput.MoveLeft, 0.3f) == DelayStatus.Valid)
                {
                    if (_rushAttackSideDelay <= ThirdPersonController.LastSideChangedDelay)
                    {
                        if (CurrentAttack == null || CurrentAttack != null && (CurrentAttack.CooldownFinished || (CurrentAttack.Type == AttackType.MeleeLight && CurrentAttack.DurationFinished)))
                        {
                            PerformLeftRushAttack();
                        }
                        else if(NextAttack == "")
                        {
                            NextAttack = "RushAttack";
                        }
                    }
                    else
                    {
                        if (CurrentAttack == null || CurrentAttack != null && CurrentAttack.CooldownFinished)
                        {
                            PerformLightAttack();
                        }
                    }                                        
                }
                else if (Neon.Input.PressedComboInput(NeonStarInput.Attack, 0.3f, NeonStarInput.MoveRight) && Neon.Input.CheckPressedDelay(NeonStarInput.MoveRight, 0.3f) == DelayStatus.Valid && entity.spritesheets.CurrentSide == Side.Right)
                {
                    if (_rushAttackSideDelay <= ThirdPersonController.LastSideChangedDelay)
                    {
                        if (CurrentAttack == null || CurrentAttack != null && (CurrentAttack.CooldownFinished || (CurrentAttack.Type == AttackType.MeleeLight && CurrentAttack.DurationFinished)))
                        {
                            PerformRightRushAttack();
                        }
                        else if (NextAttack == "")
                        {
                            NextAttack = "RushAttack";
                        }
                    }
                    else
                    {
                        if (CurrentAttack == null || CurrentAttack != null && CurrentAttack.CooldownFinished)
                        {
                            PerformLightAttack();
                        }
                    }                    
                }
                else if (Neon.Input.PressedComboInput(NeonStarInput.Attack, 0.3f, NeonStarInput.MoveDown))                  
                {
                    if (CurrentAttack == null || CurrentAttack != null && (CurrentAttack.CooldownFinished || (CurrentAttack.Type == AttackType.MeleeLight && CurrentAttack.DurationFinished)))
                    {
                        PerformDiveAttack();
                        if (CurrentAttack == null)
                        {
                            PerformLightAttack();
                        }
                    }
                    else if (NextAttack == "")
                    {
                        NextAttack = "DiveAttack";
                    }                
                }
                else if (Neon.Input.Pressed(NeonStarInput.Attack) && !_triedAttacking)
                {
                    if (CurrentAttack == null || (CurrentAttack != null && CurrentAttack.CooldownFinished))
                        PerformLightAttack();
                    else if (NextAttack == "")
                        NextAttack = "LightAttack";                 
                }
            }

            if (_currentComboHit != ComboSequence.None && (CurrentAttack != null && CurrentAttack.DurationFinished) || CurrentAttack == null)
            {
                _lastHitDelay += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if(_lastHitDelay >= ComboDelayMax)
                {
                    _currentComboHit = ComboSequence.None;
                    LastComboHit = ComboSequence.None;
                    _lastHitDelay = 0.0f;
                }    
            }

            if (_currentComboHit == ComboSequence.None)
            {
                _lastHitDelay = 0.0f;
            }

            

            if (CurrentAttack != null)
            {
                if (CurrentAttack.AirLocked)
                {
                    entity.rigidbody.body.GravityScale = 0.0f;                    
                }
                else
                {
                    entity.rigidbody.body.GravityScale = entity.rigidbody.InitialGravityScale;
                }

                if (CurrentAttack.DurationFinished && entity.spritesheets.IsFinished())
                {
                    
                    entity.spritesheets.CurrentPriority = 0;
                    entity.spritesheets.ChangeAnimation(ThirdPersonController.IdleAnimation);
                }
            }

            if (CurrentAttack == null)
            {
                ThirdPersonController.CanMove = true;
                ThirdPersonController.CanTurn = true;
            }
            else
            {
                if (((CurrentAttack.DelayStarted && !CurrentAttack.DelayFinished) || (CurrentAttack.DurationStarted && !CurrentAttack.DurationFinished) || (CurrentAttack.CooldownStarted && !CurrentAttack.CooldownFinished)))
                {
                    ThirdPersonController.CanMove = false;
                    ThirdPersonController.CanTurn = false;
                }
                else
                {
                    if(CurrentAttack.AirLocked)
                    {
                        ThirdPersonController.CanMove = false;
                        ThirdPersonController.CanTurn = true;
                    }
                    else
                    {
                        ThirdPersonController.CanMove = true;
                        ThirdPersonController.CanTurn = true;
                    }

                }

            }
            _triedAttacking = false;
            base.Update(gameTime);
        }

        private void PerformUppercut()
        {
            CheckComboHit();
            if (_currentComboHit == ComboSequence.Finish)
            {
                CurrentAttack = AttacksManager.GetAttack("UppercutFinish", entity.spritesheets.CurrentSide, entity);
                if (!CurrentAttack.Canceled)
                {
                    entity.spritesheets.ChangeAnimation(UppercutAnimation, 1, true, false, false);
                    entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                }
            }
            else
            {
                CurrentAttack = AttacksManager.GetAttack("Uppercut", entity.spritesheets.CurrentSide, entity);
                if (!CurrentAttack.Canceled)
                {
                    entity.spritesheets.ChangeAnimation(UppercutAnimation, 1, true, false, false);
                    entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                }

            }
            ReleasedAttackButton = false;
            _triedAttacking = true;
        }

        private void PerformLeftRushAttack()
        {
            CheckComboHit();
            if (_currentComboHit == ComboSequence.Finish)
            {               
                CurrentAttack = AttacksManager.GetAttack("RushAttackFinish", entity.spritesheets.CurrentSide, entity);
                if (!CurrentAttack.Canceled)
                {
                    entity.spritesheets.ChangeSide(Side.Left);
                    entity.spritesheets.ChangeAnimation(RushAttackAnimation, 1, false, false, false);
                    entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                }
            }
            else
            {
                CurrentAttack = AttacksManager.GetAttack("RushAttack", entity.spritesheets.CurrentSide, entity);
                if (!CurrentAttack.Canceled)
                {
                    entity.spritesheets.ChangeSide(Side.Left);
                    entity.spritesheets.ChangeAnimation(RushAttackAnimation, 1, false, false, false);
                    entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                }                
            }

            ReleasedAttackButton = false;
            _triedAttacking = true;
        }

        private void PerformRightRushAttack()
        {
            CheckComboHit();
            if (_currentComboHit == ComboSequence.Finish)
            {
                CurrentAttack = AttacksManager.GetAttack("RushAttackFinish", entity.spritesheets.CurrentSide, entity);
                if (!CurrentAttack.Canceled)
                {
                    entity.spritesheets.ChangeSide(Side.Right);
                    entity.spritesheets.ChangeAnimation(RushAttackAnimation, 1, false, false, false);
                    entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                }
            }
            else
            {
                CurrentAttack = AttacksManager.GetAttack("RushAttack", entity.spritesheets.CurrentSide, entity);
                if (!CurrentAttack.Canceled)
                {
                    entity.spritesheets.ChangeSide(Side.Right);
                    entity.spritesheets.ChangeAnimation(RushAttackAnimation, 1, false, false, false);
                    entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                }
            }
            ReleasedAttackButton = false;
            _triedAttacking = true;
        }

        private void PerformDiveAttack()
        {
            CheckComboHit();
            if (_currentComboHit == ComboSequence.Finish)
            {
                CurrentAttack = AttacksManager.GetAttack("DiveAttackFinish", entity.spritesheets.CurrentSide, entity);
                if (!CurrentAttack.Canceled)
                    entity.spritesheets.ChangeAnimation(DiveAttackStartAnimation, 1, true, false, false);
            }
            else
            {
                CurrentAttack = AttacksManager.GetAttack("DiveAttack", entity.spritesheets.CurrentSide, entity);
                if (!CurrentAttack.Canceled)
                    entity.spritesheets.ChangeAnimation(DiveAttackStartAnimation, 1, true, false, false);
            }
            ReleasedAttackButton = false;
        }

        private void PerformLightAttack()
        {
            CheckComboHit();
            if (_currentComboHit == ComboSequence.Finish)
            {
                CurrentAttack = AttacksManager.GetAttack("LightAttackFinish", entity.spritesheets.CurrentSide, entity);
                if (!CurrentAttack.Canceled)
                {
                    entity.spritesheets.ChangeAnimation(LightAttackAnimation + "Finish", 1, true, false, false);
                    entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                }                          
            }
            else
            {
                CurrentAttack = AttacksManager.GetAttack("LightAttack", entity.spritesheets.CurrentSide, entity);
                if (!CurrentAttack.Canceled)
                {
                    if (_currentComboHit == ComboSequence.Starter)
                        entity.spritesheets.ChangeAnimation(LightAttackAnimation + "Starter", 1, true, false, false);
                    else
                        entity.spritesheets.ChangeAnimation(LightAttackAnimation + "Link", 1, true, false, false);

                    entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                }
                           
            }
        }

        private void CheckComboHit()
        {
            LastComboHit = _currentComboHit;
            if (_currentComboHit != ComboSequence.None)
            {
                if (_lastHitDelay < ComboDelayMax)
                {
                    switch (_currentComboHit)
                    {
                        case ComboSequence.Starter:
                            _currentComboHit = ComboSequence.Link;
                            break;

                        case ComboSequence.Link:
                            _currentComboHit = ComboSequence.Finish;
                            break;

                        case ComboSequence.Finish:
                            _currentComboHit = ComboSequence.Starter;
                            break;
                    }
                }
                else
                {
                    _currentComboHit = ComboSequence.Starter;
                }
            }
            else
            {
                _currentComboHit = ComboSequence.Starter;
            }

            _lastHitDelay = 0.0f;
        }
    }
}
