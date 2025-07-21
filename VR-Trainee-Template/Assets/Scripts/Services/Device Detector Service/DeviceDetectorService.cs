using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace ATG.Services.VRDevice
{
    [Serializable]
    public sealed class DeviceDetectorCreator
    {
        [SerializeField] private DeviceData config;

        public void Create(IContainerBuilder builder)
        {
            builder.Register<DeviceDetectorService>(Lifetime.Singleton)
                .WithParameter<DeviceData>(config);
        }
    }
    
    public sealed class DeviceDetectorService
    {
        private readonly IEnumerable<KeyValuePair<string, SupportedDeviceType>> _devices;

        public DeviceDetectorService(DeviceData config)
        {
            _devices = config.SupportedDevices;
        }

        public void DebugDevices()
        {
            var inputDevices = new List<UnityEngine.XR.InputDevice>();
            UnityEngine.XR.InputDevices.GetDevices(inputDevices);

            foreach (var device in inputDevices)
            {
                Debug.Log(string.Format("Device found with name '{0}' and manufacturer '{1}'", 
                    device.name, device.manufacturer));
            }
        }

        public SupportedDeviceType GetSupportedDeviceType()
        {
            var inputDevices = new List<UnityEngine.XR.InputDevice>();
            UnityEngine.XR.InputDevices.GetDevices(inputDevices);
            
            foreach (var device in inputDevices)
            {
                foreach (var suppertedDevice in _devices)
                {
                    if (device.manufacturer.Contains(suppertedDevice.Key) == true)
                        return suppertedDevice.Value;
                }
            }
            return SupportedDeviceType.None;
        }
    }
}