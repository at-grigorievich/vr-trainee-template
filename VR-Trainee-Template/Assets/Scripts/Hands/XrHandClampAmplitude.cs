using System;
using UnityEngine;

namespace ATG.Hands
{
    [Serializable]
    public sealed class XrHandClampAmplitudeCreator
    {
        [SerializeField] private Vector2 defaultTriggerAmplitude;
        [SerializeField] private Vector2 defaultGripAmplitude;

        public XrHandClampAmplitude Create() => 
            new XrHandClampAmplitude(defaultTriggerAmplitude, defaultGripAmplitude);
    }
    
    public class XrHandClampAmplitude
    {
        public readonly float DefaultMinTriggerAmplitude;
        public readonly float DefaultMaxTriggerAmplitude;

        public readonly float DefaultMinGripAmplitude;
        public readonly float DefaultMaxGripAmplitude;
        
        public float CurrentMinTriggerAmplitude {get; private set; }
        public float CurrentMaxTriggerAmplitude { get; private set; }
        
        public float CurrentMinGripAmplitude {get; private set; }
        public float CurrentMaxGripAmplitude { get; private set; }

        public XrHandClampAmplitude(Vector2 defaultTriggerMinMax, Vector2 defaultGripMinMax)
        {
            DefaultMinTriggerAmplitude = defaultTriggerMinMax[0];
            DefaultMaxTriggerAmplitude = defaultTriggerMinMax[1];
            
            DefaultMinGripAmplitude = defaultGripMinMax[0];
            DefaultMaxGripAmplitude = defaultGripMinMax[1];
        }

        public void ResetAll()
        {
            ResetTrigger();
            ResetGrip();
        }
        
        public void ResetTrigger()
        {
            SetMaxTriggerAmplitude(DefaultMaxTriggerAmplitude);
            SetMinTriggerAmplitude(DefaultMinTriggerAmplitude);
        }

        public void ResetGrip()
        {
            SetMaxGripAmplitude(DefaultMaxGripAmplitude);
            SetMinGripAmplitude(DefaultMinGripAmplitude);
        }

        public float GetClampedTriggerAmplitude(float value)
        {
            return Mathf.Clamp(value, CurrentMinTriggerAmplitude,CurrentMaxTriggerAmplitude);
        }
        
        public float GetClampedGripAmplitude(float value)
        {
            return Mathf.Clamp(value, CurrentMinGripAmplitude, CurrentMaxGripAmplitude);
        }

        public void SetMinTriggerAmplitude(float value)
        {
            CurrentMinTriggerAmplitude = value;
        }

        public void SetMaxTriggerAmplitude(float value)
        {
            CurrentMaxTriggerAmplitude = value;
        }
        
        public void SetMinGripAmplitude(float value)
        {
            CurrentMinGripAmplitude = value;
        }

        public void SetMaxGripAmplitude(float value)
        {
            CurrentMaxGripAmplitude = value;
        }
    }
}