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

        Attack CurrentAttack;


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

        private string _diveAttackAnimation;
        public string DiveAttackAnimation
        {
            get { return _diveAttackAnimation; }
            set { _diveAttackAnimation = value; }
        }

        private float _rushAttackChargeTime;
        public float RushAttackChargeTime
        {
            get { return _rushAttackChargeTime; }
            set { _rushAttackChargeTime = value; }
        }
        

        private float _specialDelay = 0.0f;
        private float _lightDelay = 0.0f;
        private bool _isDiving = false;
        private float _lastHitDelay = 0.0f;
        private float _rushChargeTimer = 0.0f;
        private bool _rushAttacking = false;
        private bool ReleasedAttackButton = true;

        private ThirdPersonController _thirdPersonController;
        public ThirdPersonController ThirdPersonController
        {
            get { return _thirdPersonController; }
            set { _thirdPersonController = value; }
        }

        private ComboSequence _currentComboHit = ComboSequence.None;

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


            if (_lightDelay > 0.0f) 
            {
                _lightDelay -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                _lightDelay = 0.0f;
            }

            if (_specialDelay > 0.0f)
            {
                _specialDelay -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                _specialDelay = 0.0f;
                if (_rushAttacking)
                {
                    ThirdPersonController.CanMove = false;
                }
            }

            if (CurrentAttack != null && CurrentAttack.Duration > 0)
                ThirdPersonController.CanMove = false;
            else
                ThirdPersonController.CanMove = true;

            if (!_isDiving && ReleasedAttackButton)
            {
                if (Neon.Input.PressedComboInput(NeonStarInput.Attack, 0.3f, NeonStarInput.MoveUp) && _specialDelay <= 0.0f)
                {
                    CheckComboHit();
                    if (_currentComboHit == ComboSequence.Finish)
                    {
                        if (Debug) Console.WriteLine("Finisher -> Uppercut ! Current Combo : " + _currentComboHit);
                        entity.spritesheets.ChangeAnimation(UppercutAnimation, 1, true, false, false);
                        if (CurrentAttack != null)
                            CurrentAttack.CancelAttack();
                        CurrentAttack = AttacksManager.GetAttack("UppercutFinish", entity.spritesheets.CurrentSide, entity);
                    }
                    else
                    {
                        if (Debug) Console.WriteLine("Uppercut ! Current Combo : " + _currentComboHit);
                        entity.spritesheets.ChangeAnimation(UppercutAnimation, 1, true, false, false);
                        if (CurrentAttack != null)
                            CurrentAttack.CancelAttack();
                        CurrentAttack = AttacksManager.GetAttack("Uppercut", entity.spritesheets.CurrentSide, entity);
                    }
                    _specialDelay = CurrentAttack.Cooldown;
                    ReleasedAttackButton = false;
                }
                else if (Neon.Input.PressedComboInput(NeonStarInput.Attack, 0.3f, NeonStarInput.MoveLeft) && Neon.Input.CheckPressedDelay(NeonStarInput.MoveLeft, 0.3f) == DelayStatus.Valid && _specialDelay <= 0.0f)
                {
                    CheckComboHit();
                    if (_currentComboHit == ComboSequence.Finish)
                    {
                        if (Debug) Console.WriteLine("Finisher -> Left Rush Attack ! Current Combo : " + _currentComboHit);
                        entity.spritesheets.ChangeSide(Side.Left);
                        entity.spritesheets.ChangeAnimation(RushAttackAnimation, 1, false, false, false);
                    }
                    else
                    {
                        if (Debug) Console.WriteLine("Left Rush Attack ! Current Combo : " + _currentComboHit);
                        entity.spritesheets.ChangeSide(Side.Left);
                        entity.spritesheets.ChangeAnimation(RushAttackAnimation, 1, false, false, false);
                    }
                    _rushAttacking = true;
                    ReleasedAttackButton = false;
                }
                else if (Neon.Input.PressedComboInput(NeonStarInput.Attack, 0.3f, NeonStarInput.MoveRight) && Neon.Input.CheckPressedDelay(NeonStarInput.MoveRight, 0.3f) == DelayStatus.Valid && _specialDelay <= 0.0f)
                {
                    CheckComboHit();
                    if (_currentComboHit == ComboSequence.Finish)
                    {
                        if (Debug) Console.WriteLine("Finisher -> Right Rush Attack ! Current Combo : " + _currentComboHit);
                        entity.spritesheets.ChangeSide(Side.Right);
                        entity.spritesheets.ChangeAnimation(RushAttackAnimation, 1, false, false, false);
                    }
                    else
                    {
                        if (Debug) Console.WriteLine("Right Rush Attack ! Current Combo : " + _currentComboHit);
                        entity.spritesheets.ChangeSide(Side.Right);
                        entity.spritesheets.ChangeAnimation(RushAttackAnimation, 1, false, false, false);
                    }
                    _rushAttacking = true;
                    ReleasedAttackButton = false;      
                }
                else if (Neon.Input.PressedComboInput(NeonStarInput.Attack, 0.3f, NeonStarInput.MoveDown) && !entity.rigidbody.isGrounded)
                {
                    CheckComboHit();
                    if (_currentComboHit == ComboSequence.Finish)
                    {
                        if (Debug) Console.WriteLine("Finisher -> Dive Attack ! Current Combo : " + _currentComboHit);
                        entity.spritesheets.ChangeAnimation(DiveAttackAnimation, 1, true, false, false);
                        if (CurrentAttack != null)
                            CurrentAttack.CancelAttack();
                        CurrentAttack = AttacksManager.GetAttack("DiveAttackFinish", entity.spritesheets.CurrentSide, entity);
                    }
                    else
                    {
                        if (Debug) Console.WriteLine("Dive Attack ! Current Combo : " + _currentComboHit);
                        entity.spritesheets.ChangeAnimation(DiveAttackAnimation, 1, true, false, false);
                        if (CurrentAttack != null)
                            CurrentAttack.CancelAttack();
                        CurrentAttack = AttacksManager.GetAttack("DiveAttack", entity.spritesheets.CurrentSide, entity);

                    }  
                    _isDiving = true;
                    _specialDelay = CurrentAttack.Cooldown;
                    ReleasedAttackButton = false;
                }
                else if (Neon.Input.Pressed(NeonStarInput.Attack) && _lightDelay <= 0.0f && _specialDelay <= 0.0f)
                {
                    CheckComboHit();
                    if(_currentComboHit == ComboSequence.Finish)
                    {
                        if (Debug) Console.WriteLine("Finisher -> Light Attack ! Current Combo : " + _currentComboHit);
                        entity.spritesheets.ChangeAnimation(LightAttackAnimation, 1, true, false, false);
                        entity.spritesheets.CurrentSpritesheet.isPlaying = true;
                        if (CurrentAttack != null)
                            CurrentAttack.CancelAttack();
                        CurrentAttack = AttacksManager.GetAttack("LightAttackFinish", entity.spritesheets.CurrentSide, entity);
                    }
                    else
                    {
                        if (Debug) Console.WriteLine("Light Attack ! Current Combo : " + _currentComboHit);
                        if(_currentComboHit == ComboSequence.Starter)
                            entity.spritesheets.ChangeAnimation(LightAttackAnimation, 1, true, false, false, 0);
                        else
                            entity.spritesheets.ChangeAnimation(LightAttackAnimation, 1, true, false, false);
                        entity.spritesheets.CurrentSpritesheet.isPlaying = true;
                        if (CurrentAttack != null)
                            CurrentAttack.CancelAttack();
                        CurrentAttack = AttacksManager.GetAttack("LightAttack", entity.spritesheets.CurrentSide, entity);
                    }               
                    _lightDelay = CurrentAttack.Cooldown;
                    ReleasedAttackButton = false;
                }
            }

            if (_isDiving)
                if (entity.rigidbody.isGrounded)
                {
                    _isDiving = false;
                    entity.spritesheets.CurrentPriority = 0;
                    CurrentAttack.CancelAttack();
                    CurrentAttack = null;
                }

            if (_currentComboHit != ComboSequence.None && !_rushAttacking)
            {
                _lastHitDelay += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if(_lastHitDelay >= ComboDelayMax)
                {
                    _currentComboHit = ComboSequence.None;
                    _lastHitDelay = 0.0f;
                }    
            }

            if (entity.spritesheets.CurrentSpritesheetName == LightAttackAnimation)
            {
                switch(_currentComboHit)
                {
                    case ComboSequence.Starter:
                        if (entity.spritesheets.CurrentSpritesheet.currentFrame == 1)
                        {
                            entity.spritesheets.CurrentSpritesheet.isPlaying = false;
                        }
                        break;

                    case ComboSequence.Link:
                        if (entity.spritesheets.CurrentSpritesheet.currentFrame == 3)
                        {
                            entity.spritesheets.CurrentSpritesheet.isPlaying = false;
                        }
                        break;
                }
            }
            else if (entity.spritesheets.CurrentSpritesheetName == RushAttackAnimation && _rushAttacking)
            {
                if (_rushChargeTimer >= _rushAttackChargeTime)
                {
                    if (CurrentAttack != null)
                        CurrentAttack.CancelAttack();
                    CurrentAttack = AttacksManager.GetAttack(_currentComboHit == ComboSequence.Finish ? "RushAttackFinish" : "RushAttack", entity.spritesheets.CurrentSide, entity);
                    entity.spritesheets.CurrentSpritesheet.isPlaying = true;
                    _rushChargeTimer = 0.0f;
                    _rushAttacking = false;
                    _specialDelay = CurrentAttack.Cooldown;
                    
                }
                else
                {
                    _rushChargeTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }

            if (_currentComboHit == ComboSequence.None)
            {
                _lastHitDelay = 0.0f;
                if (_specialDelay <= 0.0f && !_isDiving)
                {
                    if (entity.spritesheets.CurrentSpritesheetName == UppercutAnimation
                        || entity.spritesheets.CurrentSpritesheetName == LightAttackAnimation
                        || entity.spritesheets.CurrentSpritesheetName == DiveAttackAnimation
                        || entity.spritesheets.CurrentSpritesheetName == RushAttackAnimation)
                        entity.spritesheets.CurrentPriority = 0;
                }
            }
            if (CurrentAttack != null)
            {
                CurrentAttack.Update(gameTime);
                if (!CurrentAttack.Active)
                    CurrentAttack = null;
            }
            
            base.Update(gameTime);
        }

        private void CheckComboHit()
        {
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
