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

        private float _rushAttackSideDelay = 1.0f;

        public float RushAttackSideDelay
        {
            get { return _rushAttackSideDelay; }
            set { _rushAttackSideDelay = value; }
        }

        private float _lastHitDelay = 0.0f;
        private bool ReleasedAttackButton = true;

        private Queue<AttackInfo> AttacksQueue = new Queue<AttackInfo>();


        private ThirdPersonController _thirdPersonController;
        public ThirdPersonController ThirdPersonController
        {
            get { return _thirdPersonController; }
            set { _thirdPersonController = value; }
        }

        public ComboSequence _currentComboHit = ComboSequence.None;

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

            if (CurrentAttack != null)
            {
                if (CurrentAttack.CooldownFinished && CurrentAttack.AirLockFinished)
                {
                    CurrentAttack = null;
                    entity.spritesheets.CurrentPriority = 0;
                }
                else
                    CurrentAttack.Update(gameTime);
            }

            if (ReleasedAttackButton && CurrentAttack == null || (CurrentAttack != null && CurrentAttack.CooldownFinished))
            {
                if (Neon.Input.PressedComboInput(NeonStarInput.Attack, 0.3f, NeonStarInput.MoveUp))
                {
                    if (CurrentAttack == null || CurrentAttack != null && (CurrentAttack.CooldownFinished || CurrentAttack.Type == AttackType.MeleeLight))
                    {
                        CheckComboHit();
                        if (_currentComboHit == ComboSequence.Finish)
                        {
                            if (Debug) Console.WriteLine("Finisher -> Uppercut ! Current Combo : " + _currentComboHit);
                            entity.spritesheets.ChangeAnimation(UppercutAnimation, 1, true, false, false);
                            CurrentAttack = AttacksManager.GetAttack("UppercutFinish", entity.spritesheets.CurrentSide, entity);

                        }
                        else
                        {
                            if (Debug) Console.WriteLine("Uppercut ! Current Combo : " + _currentComboHit);
                            entity.spritesheets.ChangeAnimation(UppercutAnimation, 1, true, false, false);
                            CurrentAttack = AttacksManager.GetAttack("Uppercut", entity.spritesheets.CurrentSide, entity);
                        }

                        entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                        ReleasedAttackButton = false;
                    }   
                }
                else if (Neon.Input.PressedComboInput(NeonStarInput.Attack, 0.3f, NeonStarInput.MoveLeft) && Neon.Input.CheckPressedDelay(NeonStarInput.MoveLeft, 0.3f) == DelayStatus.Valid)
                {
                    if (_rushAttackSideDelay <= ThirdPersonController.LastSideChangedDelay)
                    {
                        if (CurrentAttack == null || CurrentAttack != null && (CurrentAttack.CooldownFinished || CurrentAttack.Type == AttackType.MeleeLight))
                        {

                            CheckComboHit();
                            if (_currentComboHit == ComboSequence.Finish)
                            {
                                if (Debug) Console.WriteLine("Finisher -> Left Rush Attack ! Current Combo : " + _currentComboHit);
                                entity.spritesheets.ChangeSide(Side.Left);
                                entity.spritesheets.ChangeAnimation(RushAttackAnimation, 1, false, false, false);
                                CurrentAttack = AttacksManager.GetAttack("RushAttackFinish", entity.spritesheets.CurrentSide, entity);
                            }
                            else
                            {
                                if (Debug) Console.WriteLine("Left Rush Attack ! Current Combo : " + _currentComboHit);
                                entity.spritesheets.ChangeSide(Side.Left);
                                entity.spritesheets.ChangeAnimation(RushAttackAnimation, 1, false, false, false);
                                CurrentAttack = AttacksManager.GetAttack("RushAttack", entity.spritesheets.CurrentSide, entity);
                            }

                            entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                            ReleasedAttackButton = false;
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
                        if (CurrentAttack == null || CurrentAttack != null && (CurrentAttack.CooldownFinished || CurrentAttack.Type == AttackType.MeleeLight))
                        {

                            CheckComboHit();
                            if (_currentComboHit == ComboSequence.Finish)
                            {
                                if (Debug) Console.WriteLine("Finisher -> Right Rush Attack ! Current Combo : " + _currentComboHit);
                                entity.spritesheets.ChangeSide(Side.Right);
                                entity.spritesheets.ChangeAnimation(RushAttackAnimation, 1, false, false, false);
                                CurrentAttack = AttacksManager.GetAttack("RushAttackFinish", entity.spritesheets.CurrentSide, entity);
                            }
                            else
                            {
                                if (Debug) Console.WriteLine("Right Rush Attack ! Current Combo : " + _currentComboHit);
                                entity.spritesheets.ChangeSide(Side.Right);
                                entity.spritesheets.ChangeAnimation(RushAttackAnimation, 1, false, false, false);
                                CurrentAttack = AttacksManager.GetAttack("RushAttack", entity.spritesheets.CurrentSide, entity);
                            }

                            entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                            ReleasedAttackButton = false;
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
                else if (Neon.Input.PressedComboInput(NeonStarInput.Attack, 0.3f, NeonStarInput.MoveDown)
                    && (CurrentAttack == null || CurrentAttack != null && (CurrentAttack.CooldownFinished || CurrentAttack.Type == AttackType.MeleeLight)))
                {
                    CheckComboHit();
                    if (_currentComboHit == ComboSequence.Finish)
                    {
                        if (Debug) Console.WriteLine("Finisher -> Dive Attack ! Current Combo : " + _currentComboHit);
                        entity.spritesheets.ChangeAnimation(DiveAttackAnimation, 1, true, false, false);
                        CurrentAttack = AttacksManager.GetAttack("DiveAttackFinish", entity.spritesheets.CurrentSide, entity);
                    }
                    else
                    {
                        if (Debug) Console.WriteLine("Dive Attack ! Current Combo : " + _currentComboHit);
                        entity.spritesheets.ChangeAnimation(DiveAttackAnimation, 1, true, false, false);
                        CurrentAttack = AttacksManager.GetAttack("DiveAttack", entity.spritesheets.CurrentSide, entity);
                    }
                    ReleasedAttackButton = false;
                }              
                if (Neon.Input.Pressed(NeonStarInput.Attack) && (CurrentAttack == null || CurrentAttack != null && CurrentAttack.CooldownFinished))
                {
                    PerformLightAttack();
                }
            }

            if (_currentComboHit != ComboSequence.None && (CurrentAttack != null && CurrentAttack.DurationFinished) || CurrentAttack == null)
            {
                _lastHitDelay += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if(_lastHitDelay >= ComboDelayMax)
                {
                    _currentComboHit = ComboSequence.None;
                    _lastHitDelay = 0.0f;
                }    
            }

            if (_currentComboHit == ComboSequence.None)
            {
                _lastHitDelay = 0.0f;
                if (CurrentAttack == null)
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
                if (CurrentAttack.AirLocked)
                {
                    entity.rigidbody.body.GravityScale = 0.0f;                    
                }
                else
                {
                    entity.rigidbody.body.GravityScale = entity.rigidbody.InitialGravityScale;
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
            

            base.Update(gameTime);
        }

        private void PerformLightAttack()
        {
            CheckComboHit();
            if (_currentComboHit == ComboSequence.Finish)
            {
                if (Debug) Console.WriteLine("Finisher -> Light Attack ! Current Combo : " + _currentComboHit);
                entity.spritesheets.ChangeAnimation(LightAttackAnimation, 1, true, false, false);
                entity.spritesheets.CurrentSpritesheet.isPlaying = true;
                entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                CurrentAttack = AttacksManager.GetAttack("LightAttackFinish", entity.spritesheets.CurrentSide, entity);
            }
            else
            {
                if (Debug) Console.WriteLine("Light Attack ! Current Combo : " + _currentComboHit);
                if (_currentComboHit == ComboSequence.Starter)
                    entity.spritesheets.ChangeAnimation(LightAttackAnimation, 1, true, false, false, 0);
                else
                    entity.spritesheets.ChangeAnimation(LightAttackAnimation, 1, true, false, false);
                entity.spritesheets.CurrentSpritesheet.isPlaying = true;
                entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                CurrentAttack = AttacksManager.GetAttack("LightAttack", entity.spritesheets.CurrentSide, entity);
            }
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
