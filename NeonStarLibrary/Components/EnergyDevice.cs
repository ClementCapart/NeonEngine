using NeonEngine;
using NeonStarLibrary.Components.Avatar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.EnergyObjects
{
    public abstract class EnergyDevice : Component
    {
        #region Properties

        private DeviceState _state = DeviceState.Deactivated;

        public DeviceState State
        {
            get { return _state; }
            set { _state = value; }
        }

        #endregion

       

        public EnergyDevice(Entity entity)
            :base(entity, "EnergyDevice")
        {
        }

        public virtual void ActivateDevice()
        {
            State = DeviceState.Activated;
            DeviceManager.SetDeviceState(entity.GameWorld.LevelGroupName, entity.GameWorld.LevelName, entity.Name, State);
        }

        public virtual void DeactivateDevice()
        {
            State = DeviceState.Deactivated;
            DeviceManager.SetDeviceState(entity.GameWorld.LevelGroupName, entity.GameWorld.LevelName, entity.Name, State);
        }
    }
}
