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

        private float _lightAttackDelay = 0.3f;
        public float LightAttackDelay
        {
            get { return _lightAttackDelay; }
            set { _lightAttackDelay = value; }
        }

        private float _uppercutDelay = 0.5f;
        public float UppercutDelay
        {
            get { return _uppercutDelay; }
            set { _uppercutDelay = value; }
        }

        private float _rushAttackDelay = 0.5f;
        public float RushAttackDelay
        {
            get { return _rushAttackDelay; }
            set { _rushAttackDelay = value; }
        }

        private float _diveAttackDelay = 0.5f;
        public float DiveAttackDelay
        {
            get { return _diveAttackDelay; }
            set { _diveAttackDelay = value; }
        }


        private float _specialDelay = 0.0f;
        private float _lightDelay = 0.0f;
        private bool _isDiving = false;
        private float _lastHitDelay = 0.0f;
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
                ThirdPersonController.CanMove = false;
            }
            else
            {
                _lightDelay = 0.0f;
                ThirdPersonController.CanMove = true;
            }

            if (_specialDelay > 0.0f)
            {
                _specialDelay -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                ThirdPersonController.CanMove = false;
            }
            else
            {
                _specialDelay = 0.0f;
                ThirdPersonController.CanMove = true;
            }

            if (!_isDiving && ReleasedAttackButton)
            {
                if (Neon.Input.PressedComboInput(NeonStarInput.Attack, 0.3f, NeonStarInput.MoveUp) && _specialDelay <= 0.0f)
                {
                    CheckComboHit();
                    if (_currentComboHit == ComboSequence.Finish)
                    {
                        if (Debug) Console.WriteLine("Finisher -> Uppercut ! Current Combo : " + _currentComboHit);
                        entity.spritesheets.ChangeAnimation(UppercutAnimation, 1, true, false, false);
                    }
                    else
                    {
                        if (Debug) Console.WriteLine("Uppercut ! Current Combo : " + _currentComboHit);
                        entity.spritesheets.ChangeAnimation(UppercutAnimation, 1, true, false, false);
                    }  
                    _specialDelay = UppercutDelay;
                    ReleasedAttackButton = false;
                }
                else if (Neon.Input.PressedComboInput(NeonStarInput.Attack, 0.3f, NeonStarInput.MoveLeft) && Neon.Input.CheckPressedDelay(NeonStarInput.MoveLeft, 0.3f) == DelayStatus.Valid && _specialDelay <= 0.0f)
                {
                    CheckComboHit();
                    if (_currentComboHit == ComboSequence.Finish)
                    {
                        if (Debug) Console.WriteLine("Finisher -> Left Rush Attack ! Current Combo : " + _currentComboHit);
                        entity.spritesheets.ChangeSide(Side.Left);
                        entity.spritesheets.ChangeAnimation(RushAttackAnimation, 1, true, false, false);
                    }
                    else
                    {
                        if (Debug) Console.WriteLine("Left Rush Attack ! Current Combo : " + _currentComboHit);
                        entity.spritesheets.ChangeSide(Side.Left);
                        entity.spritesheets.ChangeAnimation(RushAttackAnimation, 1, true, false, false);
                    }  
                    _specialDelay = RushAttackDelay;
                    ReleasedAttackButton = false;
                }
                else if (Neon.Input.PressedComboInput(NeonStarInput.Attack, 0.3f, NeonStarInput.MoveRight) && Neon.Input.CheckPressedDelay(NeonStarInput.MoveRight, 0.3f) == DelayStatus.Valid && _specialDelay <= 0.0f)
                {
                    CheckComboHit();
                    if (_currentComboHit == ComboSequence.Finish)
                    {
                        if (Debug) Console.WriteLine("Finisher -> Right Rush Attack ! Current Combo : " + _currentComboHit);
                        entity.spritesheets.ChangeSide(Side.Right);
                        entity.spritesheets.ChangeAnimation(RushAttackAnimation, 1, true, false, false);
                    }
                    else
                    {
                        if (Debug) Console.WriteLine("Right Rush Attack ! Current Combo : " + _currentComboHit);
                        entity.spritesheets.ChangeSide(Side.Right);
                        entity.spritesheets.ChangeAnimation(RushAttackAnimation, 1, true, false, false);
                    }  
                    _specialDelay = RushAttackDelay;
                    ReleasedAttackButton = false;
                }
                else if (Neon.Input.PressedComboInput(NeonStarInput.Attack, 0.3f, NeonStarInput.MoveDown) && !entity.rigidbody.isGrounded)
                {
                    CheckComboHit();
                    if (_currentComboHit == ComboSequence.Finish)
                    {
                        if (Debug) Console.WriteLine("Finisher -> Dive Attack ! Current Combo : " + _currentComboHit);
                        entity.spritesheets.ChangeAnimation(DiveAttackAnimation, 1, true, false, false);
                    }
                    else
                    {
                        if (Debug) Console.WriteLine("Dive Attack ! Current Combo : " + _currentComboHit);
                        entity.spritesheets.ChangeAnimation(DiveAttackAnimation, 1, true, false, false);
                    }  
                    _isDiving = true;
                    _specialDelay = DiveAttackDelay;
                    ReleasedAttackButton = false;
                }
                else if (Neon.Input.Pressed(NeonStarInput.Attack) && _lightDelay <= 0.0f && _specialDelay <= 0.0f)
                {
                    CheckComboHit();
                    if(_currentComboHit == ComboSequence.Finish)
                    {
                        if (Debug) Console.WriteLine("Finisher -> Light Attack ! Current Combo : " + _currentComboHit);
                        entity.spritesheets.ChangeAnimation(LightAttackAnimation, 1, true, false, false);
                    }
                    else
                    {
                        if (Debug) Console.WriteLine("Light Attack ! Current Combo : " + _currentComboHit);
                        entity.spritesheets.ChangeAnimation(LightAttackAnimation, 1, true, false, false);
                    }               
                    _lightDelay = LightAttackDelay;
                    ReleasedAttackButton = false;
                }
            }

            if (_isDiving)
                if (entity.rigidbody.isGrounded)
                    _isDiving = false;

            if (_currentComboHit != ComboSequence.None)
            {
                _lastHitDelay += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if(_lastHitDelay >= ComboDelayMax)
                {
                    _currentComboHit = ComboSequence.None;
                    _lastHitDelay = 0.0f;
                }
                    
            }
            else
            {
                _lastHitDelay = 0.0f;
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
