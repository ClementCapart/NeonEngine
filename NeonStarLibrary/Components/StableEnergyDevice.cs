using NeonEngine;
using NeonEngine.Components.Text2D;
using NeonStarLibrary.Components.Avatar;
using NeonStarLibrary.Components.EnergyObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.EnergyObjects
{
    public class StableEnergyDevice : EnergyDevice
    {
        #region Properties
        private float _energyCost = 100.0f;

        public float EnergyCost
        {
            get { return _energyCost; }
            set { _energyCost = value; }
        }

        private string _avatarName = "LiOn";

        public string AvatarName
        {
            get { return _avatarName; }
            set { _avatarName = value; }
        }

        private float _fillingDuration = 1.0f;

        public float FillingDuration
        {
            get { return _fillingDuration; }
            set { _fillingDuration = value; }
        }

        private string _idleDeactivatedAnimation = "";

        public string IdleDeactivatedAnimation
        {
            get { return _idleDeactivatedAnimation; }
            set { _idleDeactivatedAnimation = value; }
        }  

        private string _idleActivatedAnimation = "";

        public string IdleActivatedAnimation
        {
            get { return _idleActivatedAnimation; }
            set { _idleActivatedAnimation = value; }
        }

        private string _activationAnimation = "";

        public string ActivationAnimation
        {
            get { return _activationAnimation; }
            set { _activationAnimation = value; }
        }

        private string _deactivationAnimation = "";

        public string DeactivationAnimation
        {
            get { return _deactivationAnimation; }
            set { _deactivationAnimation = value; }
        }
        
        #endregion

        public AvatarCore _avatar;
        private bool _isFilling = false;
        private float _fillingTimer = 0.0f;
        private TextDisplay _textDisplay;

        public StableEnergyDevice(Entity entity)
            :base(entity)
        {
            Name = "StableEnergyDevice";
        }

        public override void Init()
        {
            _textDisplay = entity.GetComponent<TextDisplay>();
            Entity avatarEntity = entity.GameWorld.GetEntityByName(_avatarName);
            if (avatarEntity != null)
                _avatar = avatarEntity.GetComponent<AvatarCore>();
            if (State == DeviceState.Deactivated)
            {
                if(this.entity.spritesheets != null)
                    this.entity.spritesheets.ChangeAnimation(_idleDeactivatedAnimation, true, 0, true, false, true);
            }
            else if (State == DeviceState.Activated)
            {
                if(this.entity.spritesheets != null)
                    this.entity.spritesheets.ChangeAnimation(_idleActivatedAnimation, true, 0, true, false, true);
            }
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (entity.hitboxes[0] != null && _avatar != null && _avatar.entity.hitboxes[0] != null)
            {
                if (entity.hitboxes[0].hitboxRectangle.Intersects(_avatar.entity.hitboxes[0].hitboxRectangle))
                {
                    if (Neon.Input.Pressed(NeonStarInput.Interact))
                    {
                        _isFilling = true;
                    }
                    else if (Neon.Input.Check(NeonStarInput.Interact))
                    {
                        if (_isFilling)
                        {
                            _fillingTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                            if (_fillingTimer >= _fillingDuration)
                            {
                                if (State == DeviceState.Activated)
                                {
                                    DeactivateDevice();
                                    _isFilling = false;
                                    if (_avatar.EnergySystem != null)
                                        _avatar.EnergySystem.CurrentEnergyStock += EnergyCost;
                                }
                                else
                                {
                                    if (_avatar.EnergySystem != null)
                                    {
                                        if (_avatar.EnergySystem.CurrentEnergyStock >= EnergyCost)
                                        {
                                            _isFilling = false;
                                            _avatar.EnergySystem.CurrentEnergyStock -= EnergyCost;
                                            if (entity.spritesheets != null)
                                                entity.spritesheets.ChangeAnimation(_activationAnimation, true, 0, true, false, false);
                                            if (_textDisplay != null)
                                                _textDisplay.Active = false;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (Neon.Input.Released(NeonStarInput.Interact))
                    {
                        _isFilling = false;
                        _fillingTimer = 0.0f;
                    }
                }
            }
            else
            {
                _isFilling = false;
                _fillingTimer = 0.0f;
            }

            if (entity.spritesheets != null)
            {
                if (entity.spritesheets.CurrentSpritesheetName == _activationAnimation)
                {
                    if(entity.spritesheets.IsFinished())
                    {
                        entity.spritesheets.ChangeAnimation(_idleActivatedAnimation, true, 0, true, false, true);
                        
                    }
                    else if (entity.spritesheets.CurrentSpritesheet.currentFrame == 32)
                    {
                        ActivateDevice();
                    }
                }    
                else if (entity.spritesheets.CurrentSpritesheetName == _deactivationAnimation && entity.spritesheets.IsFinished())
                {
                    entity.spritesheets.ChangeAnimation(_idleDeactivatedAnimation, true, 0, true, false, true);
                }
            }
            if (entity.spritesheets != null)
            {
                if (entity.spritesheets.CurrentSpritesheetName == _idleDeactivatedAnimation)
                    if (_textDisplay != null)
                        _textDisplay.Active = true;
            }
            
            base.Update(gameTime);
        }

        public override void ActivateDevice()
        {
            base.ActivateDevice();
        }

        public override void DeactivateDevice()
        {
            if(entity.spritesheets != null)
                entity.spritesheets.ChangeAnimation(_deactivationAnimation, true, 0, true, false, false);
            base.DeactivateDevice();
        }

    }
}
