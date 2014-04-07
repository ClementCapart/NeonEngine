using NeonEngine;
using NeonEngine.Components.Graphics2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.EnergyObjects
{
    public abstract class EnergyComponent : Component
    {
        #region Properties
        private string _firstActivatingDeviceName = "";

        public string FirstActivatingDeviceName
        {
            get { return _firstActivatingDeviceName; }
            set { _firstActivatingDeviceName = value; }
        }

        private bool _firstDeviceInSameLevel = true;

        public bool FirstDeviceInSameLevel
        {
            get { return _firstDeviceInSameLevel; }
            set { _firstDeviceInSameLevel = value; }
        }

        private string _firstActivatingDeviceLevelGroup = "";

        public string FirstActivatingDeviceLevelGroup
        {
            get { return _firstActivatingDeviceLevelGroup; }
            set { _firstActivatingDeviceLevelGroup = value; }
        }

        private string _firstActivatingDeviceLevelName = "";

        public string FirstActivatingDeviceLevelName
        {
            get { return _firstActivatingDeviceLevelName; }
            set { _firstActivatingDeviceLevelName = value; }
        }

        private string _secondActivatingDeviceName = "";

        public string SecondActivatingDeviceName
        {
            get { return _secondActivatingDeviceName; }
            set { _secondActivatingDeviceName = value; }
        }

        private bool _secondDeviceInSameLevel = true;

        public bool SecondDeviceInSameLevel
        {
            get { return _secondDeviceInSameLevel; }
            set { _secondDeviceInSameLevel = value; }
        }

        private string _secondActivatingDeviceLevelGroup = "";

        public string SecondActivatingDeviceLevelGroup
        {
            get { return _secondActivatingDeviceLevelGroup; }
            set { _secondActivatingDeviceLevelGroup = value; }
        }

        private string _secondActivatingDeviceLevelName = "";

        public string SecondActivatingDeviceLevelName
        {
            get { return _secondActivatingDeviceLevelName; }
            set { _secondActivatingDeviceLevelName = value; }
        }

        private string _thirdActivatingDeviceName = "";

        public string ThirdActivatingDeviceName
        {
            get { return _thirdActivatingDeviceName; }
            set { _thirdActivatingDeviceName = value; }
        }

        private bool _thirdDeviceInSameLevel = true;

        public bool ThirdDeviceInSameLevel
        {
            get { return _thirdDeviceInSameLevel; }
            set { _thirdDeviceInSameLevel = value; }
        }

        private string _thirdActivatingDeviceLevelGroup = "";

        public string ThirdActivatingDeviceLevelGroup
        {
            get { return _thirdActivatingDeviceLevelGroup; }
            set { _thirdActivatingDeviceLevelGroup = value; }
        }

        private string _thirdActivatingDeviceLevelName = "";

        public string ThirdActivatingDeviceLevelName
        {
            get { return _thirdActivatingDeviceLevelName; }
            set { _thirdActivatingDeviceLevelName = value; }
        }

        private string _fourthActivatingDeviceName = "";

        public string FourthActivatingDeviceName
        {
            get { return _fourthActivatingDeviceName; }
            set { _fourthActivatingDeviceName = value; }
        }

        private bool _fourthDeviceInSameLevel = true;

        public bool FourthDeviceInSameLevel
        {
            get { return _fourthDeviceInSameLevel; }
            set { _fourthDeviceInSameLevel = value; }
        }

        private string _fourthActivatingDeviceLevelGroup = "";

        public string FourthActivatingDeviceLevelGroup
        {
            get { return _fourthActivatingDeviceLevelGroup; }
            set { _fourthActivatingDeviceLevelGroup = value; }
        }

        private string _fourthActivatingDeviceLevelName = "";

        public string FourthActivatingDeviceLevelName
        {
            get { return _fourthActivatingDeviceLevelName; }
            set { _fourthActivatingDeviceLevelName = value; }
        }

        private string _fifthActivatingDeviceName = "";

        public string FifthActivatingDeviceName
        {
            get { return _fifthActivatingDeviceName; }
            set { _fifthActivatingDeviceName = value; }
        }

        private bool _fifthDeviceInSameLevel = true;

        public bool FifthDeviceInSameLevel
        {
            get { return _fifthDeviceInSameLevel; }
            set { _fifthDeviceInSameLevel = value; }
        }

        private string _fifthActivatingDeviceLevelGroup = "";

        public string FifthActivatingDeviceLevelGroup
        {
            get { return _fifthActivatingDeviceLevelGroup; }
            set { _fifthActivatingDeviceLevelGroup = value; }
        }

        private string _fifthActivatingDeviceLevelName = "";

        public string FifthActivatingDeviceLevelName
        {
            get { return _fifthActivatingDeviceLevelName; }
            set { _fifthActivatingDeviceLevelName = value; }
        }

          

        #endregion

        protected List<Device> _devicesToPower;
        protected bool _powered = false;
        protected int _numberOfDeviceActivated = 0;

        public EnergyComponent(Entity entity)
            :base(entity, "EnergyComponent")
        {
        }

        public override void Init()
        {
            _devicesToPower = new List<Device>();

            if (_firstActivatingDeviceName != "")
                AddDeviceToPower(_firstActivatingDeviceName, _firstDeviceInSameLevel, _firstActivatingDeviceLevelGroup, _firstActivatingDeviceLevelName);
            if (_secondActivatingDeviceName != "")
                AddDeviceToPower(_secondActivatingDeviceName, _secondDeviceInSameLevel, _secondActivatingDeviceLevelGroup, _secondActivatingDeviceLevelName);
            if (_thirdActivatingDeviceName != "")
                AddDeviceToPower(_thirdActivatingDeviceName, _thirdDeviceInSameLevel, _thirdActivatingDeviceLevelGroup, _thirdActivatingDeviceLevelName);
            if (_fourthActivatingDeviceName != "")
                AddDeviceToPower(_fourthActivatingDeviceName, _fourthDeviceInSameLevel, _fourthActivatingDeviceLevelGroup, _fourthActivatingDeviceLevelName);
            if (_fifthActivatingDeviceName != "")
                AddDeviceToPower(_fifthActivatingDeviceName, _fifthDeviceInSameLevel, _fifthActivatingDeviceLevelGroup, _fifthActivatingDeviceLevelName);
            
            bool allPowered = true;
            foreach (Device deviceToPower in _devicesToPower)
                if (deviceToPower.State != DeviceState.Activated)
                    allPowered = false;

            if (allPowered)
                _powered = true;

            base.Init();
        }

        private void AddDeviceToPower(string deviceName, bool sameLevel, string deviceGroupName, string deviceLevelName)
        {
            Device d = DeviceManager.GetDevice(sameLevel ? entity.GameWorld.LevelGroupName : deviceGroupName, sameLevel ? entity.GameWorld.LevelName : deviceLevelName, deviceName);

            if (d != null)
                _devicesToPower.Add(d);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (_devicesToPower.Count > 0)
            {
                if (!_powered)
                {
                    _numberOfDeviceActivated = 0;
                    bool allPowered = true;
                    foreach (Device deviceToPower in _devicesToPower)
                        if (deviceToPower.State != DeviceState.Activated)
                        {
                            allPowered = false;
                        }
                        else
                        {
                            _numberOfDeviceActivated++;
                        }

                    if (allPowered)
                        PowerDevice();
                }
                else
                {
                    _numberOfDeviceActivated = 0;
                    bool allPowered = true;
                    foreach (Device deviceToPower in _devicesToPower)
                        if (deviceToPower.State != DeviceState.Activated)
                        {
                            allPowered = false;
                        }
                        else
                        {
                            _numberOfDeviceActivated++;
                        }

                    if (!allPowered)
                        UnpowerDevice();
                }
            }
            
            base.Update(gameTime);
        }

        public virtual void PowerDevice()
        {
            Console.WriteLine("Energy Device : " + entity.Name + " Device Powered !");
            _powered = true;
        }

        public virtual void UnpowerDevice()
        {
            Console.WriteLine("Energy Device : " + entity.Name + " Device Unpowered !");
            _powered = false;
        }


    }
}
