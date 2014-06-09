using NeonEngine;
using NeonEngine.Components.Text2D;
using NeonStarLibrary.Components.Avatar;
using NeonStarLibrary.Components.EnergyObjects;
using NeonStarLibrary.Components.Graphics2D;
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

        private float _frameToActivate = 32f;

        public float FrameToActivate
        {
            get { return _frameToActivate; }
            set { _frameToActivate = value; }
        }

        #endregion

        public AvatarCore _avatar;
        private bool _isFilling = false;
        private TextDisplay _textDisplay;
        private bool _isFilled = false;
        private bool _deactivatingDevice = false;
        private FadingSpritesheet _fadingSpritesheet;

        public StableEnergyDevice(Entity entity)
            :base(entity)
        {
            Name = "StableEnergyDevice";
        }

        public override void Init()
        {
            State = DeviceManager.GetDeviceState(entity.GameWorld.LevelGroupName, entity.GameWorld.LevelName, entity.Name);
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
                _isFilled = true;
                if (_textDisplay != null)
                    _textDisplay.Active = false;
            }
            _fadingSpritesheet = entity.GetComponent<FadingSpritesheet>();
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (entity.hitboxes.Count > 0 && entity.hitboxes[0] != null && _avatar != null && _avatar.entity.hitboxes.Count > 0 && (entity.spritesheets.CurrentSpritesheet.IsFinished || entity.spritesheets.CurrentSpritesheet.IsLooped))
            {
                if (entity.hitboxes[0].hitboxRectangle.Intersects(_avatar.entity.hitboxes[0].hitboxRectangle))
                {
                    if (Neon.Input.Pressed(NeonStarInput.Interact) && !_isFilling)
                    {
                        if (State == DeviceState.Activated)
                        {
                            _deactivatingDevice = true;
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
                                    _isFilled = true;
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
            else
            {
                _isFilling = false;
            }

            if (entity.spritesheets != null)
            {
                if (entity.spritesheets.CurrentSpritesheetName == _activationAnimation)
                {
                    if(entity.spritesheets.IsFinished())
                    {
                        entity.spritesheets.ChangeAnimation(_idleActivatedAnimation, true, 0, true, false, true);                     
                    }
                    else if (entity.spritesheets.CurrentSpritesheet.currentFrame == _frameToActivate)
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

            if (this._fadingSpritesheet != null && entity.spritesheets != null && (entity.spritesheets.CurrentSpritesheetName == "Deactivation" || entity.spritesheets.CurrentSpritesheetName == "Activation"))
            {
                _fadingSpritesheet.Active = false;
            }
            else if (_fadingSpritesheet != null && (_avatar.EnergySystem.CurrentEnergyStock >= _energyCost || _state == DeviceState.Activated))
            {
                if (!_fadingSpritesheet.Active)
                    _fadingSpritesheet.Opacity = 0.0f;
                _fadingSpritesheet.Active = true;
            }
            else
            {
                _fadingSpritesheet.Active = false;
            }
            
            if (_deactivatingDevice && entity.spritesheets.CurrentSpritesheet.currentFrame == entity.spritesheets.CurrentSpritesheet.spriteSheetInfo.FrameCount - 1)
                DeactivateDevice();
            base.Update(gameTime);
        }

        public override void ActivateDevice()
        {
            _isFilled = true;
            base.ActivateDevice();
        }

        public override void DeactivateDevice()
        {
            _deactivatingDevice = false;
            _isFilled = false;
            if(entity.spritesheets != null)
                entity.spritesheets.ChangeAnimation(_deactivationAnimation, true, 0, true, false, false);
            base.DeactivateDevice();
        }

        public override void OnChangeLevel()
        {
            if (_isFilled && State == DeviceState.Deactivated)
                ActivateDevice();
            else if (!_isFilled && State == DeviceState.Activated)
                DeactivateDevice();
            base.OnChangeLevel();
        }

    }
}
