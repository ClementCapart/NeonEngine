using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    public class EnergyDevice : Component
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

        private DeviceState _state = DeviceState.Deactivated;

        public DeviceState State
        {
            get { return _state; }
            set { _state = value; }
        }

        #endregion

        public Avatar _avatar;
        private bool _isFilling = false;
        private float _fillingTimer = 0.0f;

        public EnergyDevice(Entity entity)
            :base(entity, "EnergyDevice")
        {
        }

        public override void Init()
        {
            Entity avatarEntity = entity.containerWorld.GetEntityByName(_avatarName);
            if (avatarEntity != null)
                _avatar = avatarEntity.GetComponent<Avatar>();
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (entity.hitboxes[0] != null && _avatar != null && _avatar.entity.hitboxes[0] != null)
            {
                if (entity.hitboxes[0].hitboxRectangle.Intersects(_avatar.entity.hitboxes[0].hitboxRectangle))
                {
                    if (Neon.Input.Pressed(Microsoft.Xna.Framework.Input.Buttons.Y))
                    {
                        _isFilling = true;
                    }
                    else if (Neon.Input.Check(Microsoft.Xna.Framework.Input.Buttons.Y))
                    {
                        if (_isFilling)
                        {
                            _fillingTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                            if (_fillingTimer >= _fillingDuration)
                            {
                                if (State == DeviceState.Activated)
                                {
                                    State = DeviceState.Deactivated;
                                    DeviceManager.SetDeviceState(entity.containerWorld.LevelGroupName, entity.containerWorld.LevelName, entity.Name, State);
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
                                            State = DeviceState.Activated;
                                            DeviceManager.SetDeviceState(entity.containerWorld.LevelGroupName, entity.containerWorld.LevelName, entity.Name, State);
                                        }
                                    }
                                }
                            }
                        }               
                    }

                    if (Neon.Input.Released(Microsoft.Xna.Framework.Input.Buttons.Y))
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

            base.Update(gameTime);
        }
    }
}
