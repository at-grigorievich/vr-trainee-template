using System;
using ATG.Activator;
using ATG.UI;
using Unity.XR.CoreUtils;
using UnityEngine;

namespace ATG.Services.Calibration
{
    [Serializable]
    public sealed class CalibrationServiceFactory
    {
        [SerializeField] private XROrigin xrOrigin;
        [SerializeField] private HeightCalibrateView view;

        public ICallibrationService Create()
        {
            return new HeightCalibrationService(xrOrigin, view);
        }
    }
    
    /// <summary>
    /// Сервис позволяет калибровать уровень высоты игрока
    /// И сохраняет установленное значение в PlayerPrefs
    /// </summary>
    public sealed class HeightCalibrationService : ICallibrationService
    {
        private const string HeightRef = "vr-agent-height";

        private readonly XROrigin _xrOrigin;

        private readonly HeightCalibrateView _uiView;

        public ActiveStatus Status { get; set; }

        public HeightCalibrationService(XROrigin agentXROrigin, HeightCalibrateView uiView)
        {
            _xrOrigin = agentXROrigin;
            _uiView = uiView;
        }

        public void SetActiveTotal(bool isActive)
        {
            SetActiveLogical(isActive);
            SetActiveVisual(isActive);
        }

        public void SetActiveVisual(bool isActive)
        {
            this.SetFlag(ActiveStatus.VISUAL_ACTIVE, isActive);
            
            if(isActive == true)
            {
                _uiView.Show(this, (Action)OnHeightChangedSaved);
            }
            else
            {
                _uiView.Hide();
            }
        }

        public void SetActiveLogical(bool isActive)
        {
            this.SetFlag(ActiveStatus.LOGIC_ACTIVE, isActive);

            if (isActive == true)
            {
                _uiView.SetupInitialHeight(GetHeightFromPrefs());
                _uiView.OnSliderValueChanged.AddListener(OnHeightValueChanged);
            }
            else
            {
                _uiView.OnSliderValueChanged.RemoveListener(OnHeightValueChanged);
                _xrOrigin.CameraYOffset = GetHeightFromPrefs();
            }
        }

        private void OnHeightValueChanged(float height)
        {
            _xrOrigin.CameraYOffset = height;
        }

        private void OnHeightChangedSaved()
        {
            float newHeightValue = _uiView.Value;
            
            SaveNewHeightInPrefs(newHeightValue);
        }

#region PlayerPrefs
        public static float GetHeightFromPrefs()
        {
            return PlayerPrefs.HasKey(HeightRef) == false ? 1.75f : PlayerPrefs.GetFloat(HeightRef);
        }

        private void SaveNewHeightInPrefs(float newValue)
        {
            PlayerPrefs.SetFloat(HeightRef, newValue);
        }
#endregion
    }
}