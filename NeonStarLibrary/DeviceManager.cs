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
        static public bool AlreadyLoaded = false;

        static public void LoadDevicesInformation()
        {
            _devices = new List<Device>();

            if (Directory.Exists(@"../Save/"))
            {
                if (File.Exists(@"../Save/DevicesSave.xml"))
                {
                    XDocument devicesDocument = XDocument.Load(@"../Save/DevicesSave.xml");

                    foreach (XElement device in devicesDocument.Element("Devices").Elements("Device"))
                    {
                        Device d = new Device();
                        d.GroupName = device.Element("GroupName").Value;
                        d.LevelName = device.Element("LevelName").Value;
                        d.DeviceName = device.Element("DeviceName").Value;
                        d.State = (DeviceState)Enum.Parse(typeof(DeviceState), device.Element("State").Value);
                        _devices.Add(d);
                    }
                }
                else
                {
                    if (File.Exists(@"../Data/Config/DevicesSaveTemplate.xml"))
                    {
                        XDocument devicesDocument = XDocument.Load(@"../Data/Config/DevicesSaveTemplate.xml");

                        foreach (XElement device in devicesDocument.Element("Devices").Elements("Device"))
                        {
                            Device d = new Device();
                            d.GroupName = device.Element("GroupName").Value;
                            d.LevelName = device.Element("LevelName").Value;
                            d.DeviceName = device.Element("DeviceName").Value;
                            d.State = (DeviceState)Enum.Parse(typeof(DeviceState), device.Element("State").Value);
                            _devices.Add(d);
                        }
                    }
                }
            }
            
            AlreadyLoaded = true;
        }

        static public void SaveDevicesInformation()
        {
            if (_devices != null && _devices.Count > 0)
            {
                if (Directory.Exists(@"../Save/"))
                {
                    XDocument deviceDocument = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
                    XElement devices = new XElement("Devices");

                    foreach (Device d in _devices)
                    {
                        XElement device = new XElement("Device");
                        XElement group = new XElement("GroupName", d.GroupName);
                        XElement level = new XElement("LevelName", d.LevelName);
                        XElement name = new XElement("DeviceName", d.DeviceName);
                        XElement state = new XElement("State", d.State.ToString());

                        device.Add(group);
                        device.Add(level);
                        device.Add(name);
                        device.Add(state);

                        devices.Add(device);
                    }

                    deviceDocument.Add(devices);

                    deviceDocument.Save(@"../Save/DevicesSave.xml");
                }
            }
        }

        static public DeviceState GetDeviceState(string groupName, string levelName, string deviceName)
        {
            foreach (Device d in _devices)
                if (d.GroupName == groupName && d.LevelName == levelName && d.DeviceName == deviceName)
                    return d.State;

            return DeviceState.Disabled;
        }

        static public Device GetDevice(string groupName, string levelName, string deviceName)
        {
            foreach (Device d in _devices)
                if (d.GroupName == groupName && d.LevelName == levelName && d.DeviceName == deviceName)
                    return d;

            return null;
        }

        static public List<Device> GetLevelDevices(string groupName, string levelName)
        {
            List<Device> devices = new List<Device>();
            foreach (Device d in _devices)
                if (d.GroupName == groupName && d.LevelName == levelName)
                    devices.Add(d);

            return devices;
        }

        static public void SetDeviceState(string groupName, string levelName, string deviceName, DeviceState state)
        {
            foreach (Device d in _devices)
                if (d.GroupName == groupName && d.LevelName == levelName && d.DeviceName == deviceName)
                {
                    d.State = state;
                    return;
                }

            Console.WriteLine("Warning : No Device found in Devices List, state change not effective !");
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

            for (int i = document.Element("Devices").Elements("Device").Count() - 1; i >= 0; i--)
            {
                XElement device = document.Element("Devices").Elements("Device").ElementAt(i);
                if (device.Element("GroupName").Value == groupName && device.Element("LevelName").Value == levelName)
                    device.Remove();
            }
            
            foreach(EnergyDevice ed in devices)
            {
                XElement device = new XElement("Device");
                XElement group = new XElement("GroupName", groupName);
                XElement level = new XElement("LevelName", levelName);
                XElement name = new XElement("DeviceName", ed.entity.Name);
                XElement state = new XElement("State", ed.State.ToString());

                device.Add(group);
                device.Add(level);
                device.Add(name);
                device.Add(state);

                document.Element("Devices").Add(device);
            }

            document.Save(@"../Data/Config/DevicesSaveTemplate.xml");
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
