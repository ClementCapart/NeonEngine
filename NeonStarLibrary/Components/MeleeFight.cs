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
        #region Properties
        private bool _Debug = false;

        public bool Debug
        {
            get { return _Debug; }
            set { _Debug = value; }
        }

        public Attack CurrentAttack;

        private string _lightAttackName;

        public string LightAttackName
        {
            get { return _lightAttackName; }
            set { _lightAttackName = value; }
        }

        private string _rushAttackName;

        public string RushAttackName
        {
            get { return _rushAttackName; }
            set { _rushAttackName = value; }
        }

        private string _uppercutName;

        public string UppercutName
        {
            get { return _uppercutName; }
            set { _uppercutName = value; }
        }

        private string _diveAttackName;

        public string DiveAttackName
        {
            get { return _diveAttackName; }
            set { _diveAttackName = value; }
        }

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

        private float _rushAttackStickDelay = 0.3f;

        public float RushAttackStickDelay
        {
            get { return _rushAttackStickDelay; }
            set { _rushAttackStickDelay = value; }
        }
        #endregion

        public Avatar AvatarComponent = null; 

        public ComboSequence LastComboHit = ComboSequence.None;
        public ComboSequence CurrentComboHit = ComboSequence.None;

        public List<string> AttacksWhileInAir = new List<string>();
        public float DamageModifier = 1.0f;
        public float DamageModifierTimer = 0.0f;
        
        private float _chainDelayTimer = 0.0f;
        private string _nextAttack = "";
        private bool _triedAttacking = false;

        public MeleeFight(Entity entity)
            :base(entity, "MeleeFight")
        {
        }

        public override void Init()
        {
            AvatarComponent = entity.GetComponent<Avatar>();
            base.Init();
        }

        public override void PreUpdate(GameTime gameTime)
        {
            if (DamageModifierTimer > 0.0f)
                DamageModifierTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            else
                DamageModifier = 1.0f;

            if (CurrentAttack != null)
            {
                CurrentAttack.Update(gameTime);
                if (CurrentAttack.CooldownFinished)
                    CurrentAttack = null;
            }

            if (!ReleasedAttackButton)
                if (!Neon.Input.Check(NeonStarInput.Attack))
                    ReleasedAttackButton = true;

            if (entity.rigidbody != null && entity.rigidbody.isGrounded)
                AttacksWhileInAir.Clear();

            if (CurrentComboHit != ComboSequence.None && (CurrentAttack != null && CurrentAttack.DurationFinished) || CurrentAttack == null)
            {
                _lastHitDelay += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_lastHitDelay >= ComboDelayMax)
                {
                    CurrentComboHit = ComboSequence.None;
                    LastComboHit = ComboSequence.None;
                    _lastHitDelay = 0.0f;
                }
            }
            else
            {
                AvatarComponent.State = AvatarState.Attacking;
                AvatarComponent.CanMove = false;
                AvatarComponent.CanTurn = false;
            }

            base.PreUpdate(gameTime);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (_nextAttack != "")
                if (AvatarComponent.CanAttack && CurrentAttack == null && AvatarComponent.State != AvatarState.Stunlocked)
                    LaunchBufferedAttack();
                else
                    _chainDelayTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            

            if (ReleasedAttackButton)
            {
                if (Neon.Input.PressedComboInput(NeonStarInput.Attack, 0.0f, NeonStarInput.MoveUp))
                {
                    if (CurrentAttack == null || (CurrentAttack != null && CurrentAttack.Type == AttackType.MeleeLight && CurrentAttack.DurationFinished) && AvatarComponent.CanAttack)
                        PerformUppercut();
                    else if(_nextAttack == "")
                    {
                        _nextAttack = "Uppercut";
                        _chainDelayTimer = 0;
                    }
                }
                else if (Neon.Input.PressedComboInput(NeonStarInput.Attack, 0.0f, NeonStarInput.MoveLeft) && Neon.Input.CheckPressedDelay(NeonStarInput.MoveLeft, _rushAttackStickDelay) == DelayStatus.Valid)
                {
                    if (_rushAttackSideDelay <= AvatarComponent.ThirdPersonController.LastSideChangedDelay)
                    {
                        if (CurrentAttack == null || (CurrentAttack != null && CurrentAttack.Type == AttackType.MeleeLight && CurrentAttack.DurationFinished) && AvatarComponent.CanAttack)
                            PerformLeftRushAttack();
                        else if(_nextAttack == "")
                        {
                            _nextAttack = "LeftRushAttack";
                            _chainDelayTimer = 0;
                        }
                    }
                    else
                        if (CurrentAttack == null || CurrentAttack != null && CurrentAttack.CooldownFinished)
                            PerformLightAttack();                                      
                }
                else if (Neon.Input.PressedComboInput(NeonStarInput.Attack, 0.0f, NeonStarInput.MoveRight) && Neon.Input.CheckPressedDelay(NeonStarInput.MoveRight, _rushAttackStickDelay) == DelayStatus.Valid && entity.spritesheets.CurrentSide == Side.Right)
                {
                    if (_rushAttackSideDelay <= AvatarComponent.ThirdPersonController.LastSideChangedDelay)
                    {
                        if (CurrentAttack == null || (CurrentAttack != null && CurrentAttack.Type == AttackType.MeleeLight && CurrentAttack.DurationFinished) && AvatarComponent.CanAttack)
                            PerformRightRushAttack();
                        else if (_nextAttack == "")
                        {
                            _nextAttack = "RightRushAttack";
                            _chainDelayTimer = 0;
                        }
                    }
                    else
                        if (CurrentAttack == null || CurrentAttack != null && CurrentAttack.CooldownFinished)
                            PerformLightAttack();
                }
                else if (Neon.Input.PressedComboInput(NeonStarInput.Attack, 0.0f, NeonStarInput.MoveDown))                  
                {
                    if (CurrentAttack == null || (CurrentAttack != null && CurrentAttack.Type == AttackType.MeleeLight && CurrentAttack.DurationFinished) && AvatarComponent.CanAttack)
                    {
                        PerformDiveAttack();
                        if (CurrentAttack == null)
                            PerformLightAttack();
                    }
                    else if (_nextAttack == "")
                    {
                        _nextAttack = "DiveAttack";
                        _chainDelayTimer = 0;
                    }                
                }
                else if (Neon.Input.Pressed(NeonStarInput.Attack) && !_triedAttacking && AvatarComponent.CanAttack)
                {
                    if (CurrentAttack == null)
                        PerformLightAttack();
                    else if (_nextAttack == "")
                    {
                        _nextAttack = "LightAttack";
                        _chainDelayTimer = 0;
                    }
                }
                else if(Neon.Input.Pressed(NeonStarInput.Jump) && !AvatarComponent.ThirdPersonController.StartJumping)
                {
                    AvatarComponent.ThirdPersonController.MustJumpAsSoonAsPossible = true;
                }               
            }

            if (CurrentComboHit == ComboSequence.None)
            {
                _lastHitDelay = 0.0f;
            }       

            _triedAttacking = false;
            base.Update(gameTime);
        }

        private void LaunchBufferedAttack()
        {
            if (_nextAttack != "")
            {
                if (Neon.Input.Check(NeonStarInput.MoveLeft))
                {
                    if (AvatarComponent.CurrentSide != Side.Left)
                    {
                        AvatarComponent.CurrentSide = Side.Left;
                        AvatarComponent.ThirdPersonController.LastSideChangedDelay = 0.0f;
                    }
                }
                else if (Neon.Input.Check(NeonStarInput.MoveRight))
                {
                    if (AvatarComponent.CurrentSide != Side.Right)
                    {
                        AvatarComponent.CurrentSide = Side.Right;
                        AvatarComponent.ThirdPersonController.LastSideChangedDelay = 0.0f;
                    }
                }

                if (_chainDelayTimer < _chainDelay)
                {
                    switch (_nextAttack)
                    {
                        case "Uppercut":
                            PerformUppercut();
                            break;

                        case "DiveAttack":
                            PerformDiveAttack();
                            break;

                        case "LeftRushAttack":
                            if (AvatarComponent.CurrentSide == Side.Left && _rushAttackSideDelay <= AvatarComponent.ThirdPersonController.LastSideChangedDelay)
                                PerformLeftRushAttack();
                            else
                                PerformLightAttack();
                            break;

                        case "RightRushAttack":
                            if (AvatarComponent.CurrentSide == Side.Right && _rushAttackSideDelay <= AvatarComponent.ThirdPersonController.LastSideChangedDelay)
                                PerformRightRushAttack();
                            else
                                PerformLightAttack();
                            break;

                        case "LightAttack":
                            PerformLightAttack();
                            break;
                    }
                }

                _nextAttack = "";
                _chainDelayTimer = 0.0f;
            }
        }

        private void PerformUppercut()
        {
            CheckComboHit();
            if (CurrentComboHit == ComboSequence.Finish)
            {
                CurrentAttack = AttacksManager.GetAttack(_uppercutName + "Finish", AvatarComponent.CurrentSide, entity);
                if (DamageModifierTimer > 0.0f)
                {
                    CurrentAttack.DamageOnHit *= DamageModifier;
                }
                if (!CurrentAttack.Canceled)
                {
                    AvatarComponent.State = AvatarState.Attacking;
                    entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                }
            }
            else
            {
                CurrentAttack = AttacksManager.GetAttack(_uppercutName, AvatarComponent.CurrentSide, entity);
                if (DamageModifierTimer > 0.0f)
                {
                    CurrentAttack.DamageOnHit *= DamageModifier;
                }
                if (!CurrentAttack.Canceled)
                {
                    AvatarComponent.State = AvatarState.Attacking;
                    entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                }

            }
            ReleasedAttackButton = false;
            _triedAttacking = true;
        }

        private void PerformLeftRushAttack()
        {
            CheckComboHit();
            if (CurrentComboHit == ComboSequence.Finish)
            {               
                CurrentAttack = AttacksManager.GetAttack(_rushAttackName+"Finish", Side.Left, entity);
                if (DamageModifierTimer > 0.0f)
                {
                    CurrentAttack.DamageOnHit *= DamageModifier;
                }
                if (!CurrentAttack.Canceled)
                {
                    AvatarComponent.CurrentSide = Side.Left;
                    AvatarComponent.State = AvatarState.Attacking;
                    entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                }
            }
            else
            {
                CurrentAttack = AttacksManager.GetAttack(_rushAttackName, Side.Left, entity);
                if (!CurrentAttack.Canceled)
                {
                    AvatarComponent.CurrentSide = Side.Left;
                    AvatarComponent.State = AvatarState.Attacking;
                    entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                }                
            }

            ReleasedAttackButton = false;
            _triedAttacking = true;
        }

        private void PerformRightRushAttack()
        {
            CheckComboHit();
            if (CurrentComboHit == ComboSequence.Finish)
            {
                CurrentAttack = AttacksManager.GetAttack(_rushAttackName + "Finish", Side.Right, entity);
                if (DamageModifierTimer > 0.0f)
                {
                    CurrentAttack.DamageOnHit *= DamageModifier;
                }
                if (!CurrentAttack.Canceled)
                {
                    AvatarComponent.CurrentSide = Side.Right;
                    AvatarComponent.State = AvatarState.Attacking;
                    entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                }
            }
            else
            {
                CurrentAttack = AttacksManager.GetAttack(_rushAttackName, Side.Right, entity);
                if (DamageModifierTimer > 0.0f)
                {
                    CurrentAttack.DamageOnHit *= DamageModifier;
                }
                if (!CurrentAttack.Canceled)
                {
                    AvatarComponent.CurrentSide = Side.Right;
                    AvatarComponent.State = AvatarState.Attacking;
                    entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                }
            }
            ReleasedAttackButton = false;
            _triedAttacking = true;
        }

        private void PerformDiveAttack()
        {
            CheckComboHit();
            if (CurrentComboHit == ComboSequence.Finish)
            {
                CurrentAttack = AttacksManager.GetAttack(_diveAttackName + "Finish", AvatarComponent.CurrentSide, entity);
                if (DamageModifierTimer > 0.0f)
                {
                    CurrentAttack.DamageOnHit *= DamageModifier;
                }
                if (!CurrentAttack.Canceled)
                    AvatarComponent.State = AvatarState.Attacking;
            }
            else
            {
                CurrentAttack = AttacksManager.GetAttack(_diveAttackName, AvatarComponent.CurrentSide, entity);
                if (DamageModifierTimer > 0.0f)
                {
                    CurrentAttack.DamageOnHit *= DamageModifier;
                }
                if (!CurrentAttack.Canceled)
                    AvatarComponent.State = AvatarState.Attacking;
            }
            ReleasedAttackButton = false;
        }

        private void PerformLightAttack()
        {
            CheckComboHit();
            if (CurrentComboHit == ComboSequence.Finish)
            {
                CurrentAttack = AttacksManager.GetAttack(_lightAttackName + "Finish", AvatarComponent.CurrentSide, entity);
                if (DamageModifierTimer > 0.0f)
                {
                    CurrentAttack.DamageOnHit *= DamageModifier;
                }
                if (!CurrentAttack.Canceled)
                {
                    AvatarComponent.State = AvatarState.Attacking;
                }
            }
            else
            {
                CurrentAttack = AttacksManager.GetAttack(_lightAttackName, AvatarComponent.CurrentSide, entity);
                if (DamageModifierTimer > 0.0f)
                {
                    CurrentAttack.DamageOnHit *= DamageModifier;
                }
                if (!CurrentAttack.Canceled)
                {
                    AvatarComponent.State = AvatarState.Attacking;
                }
                           
            }
            ReleasedAttackButton = false;
        }

        private void CheckComboHit()
        {
            LastComboHit = CurrentComboHit;
            if (CurrentComboHit != ComboSequence.None)
            {
                if (_lastHitDelay < ComboDelayMax)
                {
                    switch (CurrentComboHit)
                    {
                        case ComboSequence.Starter:
                            CurrentComboHit = ComboSequence.Link;
                            break;

                        case ComboSequence.Link:
                            CurrentComboHit = ComboSequence.Finish;
                            break;

                        case ComboSequence.Finish:
                            CurrentComboHit = ComboSequence.Starter;
                            break;
                    }
                }
                else
                {
                    CurrentComboHit = ComboSequence.Starter;
                }
            }
            else
            {
                CurrentComboHit = ComboSequence.Starter;
            }

            _lastHitDelay = 0.0f;
        }

        public void ResetComboHit()
        {
            CurrentComboHit = ComboSequence.None;
        }
    }
}
