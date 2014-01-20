using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace NeonStarLibrary
{
    static public class DeviceManager
    {
        static private List<Device> _devices;

        static public void LoadDevicesInformation()
        {
            _devices = new List<Device>();
        }

        static public void SaveDevicesInformation()
        {
            if (_devices != null && _devices.Count > 0)
            {

            }
        }

        static public DeviceState GetDeviceState(string groupName, string levelName, string deviceName)
        {
            foreach (Device d in _devices)
                if (d.GroupName == groupName && d.LevelName == levelName && d.DeviceName == deviceName)
                    return d.State;

            return DeviceState.Disabled;
        }

        static public void SetDeviceState(string groupName, string levelName, string deviceName, DeviceState state)
        {
            foreach (Device d in _devices)
                if (d.GroupName == groupName && d.LevelName == levelName && d.DeviceName == deviceName)
                {
                    d.State = state;
                    return;
                }
        }

        static public void SaveDevicesTemplate(string groupName, string levelName, List<EnergyDevice> devices)
        {
            if(!Directory.Exists(@"../Data/Config"))
                Directory.CreateDirectory(@"../Data/Config/");

            if(!File.Exists(@"../Data/Config/DevicesSaveTemplate.xml"))
            {
                XDocument saveTemplate = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
                saveTemplate.Add(new XElement("Devices"));

                saveTemplate.Save(@"../Data/Config/DevicesSaveTemplate.xml");
            }

            XDocument document = XDocument.Load(@"../Data/Config/DevicesSaveTemplate.xml");
        }
    }

    public class Device
    {
        public string GroupName;
        public string LevelName;
        public string DeviceName;

        public DeviceState State;
    }

    public enum DeviceState
    {
        Activated,
        Deactivated,
        Disabled
    }

}
