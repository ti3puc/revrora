using System.Collections;
using System.Collections.Generic;
using Managers.Settings;
using NaughtyAttributes;
using UI.Menu.Settings;
using UnityEngine;

namespace UI.Menu
{
    public class MainMenuSettingsUI : MonoBehaviour
    {
        [SerializeField] private List<MainMenuPanelUI> _panels = new();

        [Header("System Settings")]
        [SerializeField] private DoubleSettingsButton _autosaveButton;
        [SerializeField] private TripleSettingsButton _textSpeedButton;
        [SerializeField] private DoubleSettingsButton _cameraDistanceButton;

        [Header("Video Settings")]
        [SerializeField] private TripleSettingsButton _graphicsButton;
        [SerializeField] private DoubleSettingsButton _postProcessButton;
        [SerializeField] private DoubleSettingsButton _fullscreenButton;
        [SerializeField] private TripleSettingsButton _framerateButton;
        [SerializeField] private DoubleSettingsButton _vSyncButton;


        [Header("Audio Settings")]
        [SerializeField] private SliderSettingsButton _musicSlider;
        [SerializeField] private DoubleSettingsButton _musicButton;
        [SerializeField] private SliderSettingsButton _sfxSlider;
        [SerializeField] private DoubleSettingsButton _sfxButton;

        [Header("Debug")]
        [SerializeField, ReadOnly] private MainMenuPanelUI _currentPanel;

        #region Unity Messages
        private void Awake()
        {
            foreach (var panel in _panels)
            {
                panel.SubscribeToEvents();
                panel.OnButtonClicked += UpdateCurrentPanel;
            }

            UpdateCurrentPanel(null);
            UpdateSettings();
        }

        private void OnDestroy()
        {
            foreach (var panel in _panels)
            {
                panel.UnsubscribeToEvents();
                panel.OnButtonClicked -= UpdateCurrentPanel;
            }
        }
        #endregion

        #region Public Methods (used on UnityEvents inside inspector)
        public void ChangeAutosave(int newValue) => SettingsManager.Instance.Autosave = newValue;
        public void ChangeTextSpeed(int newValue) => SettingsManager.Instance.TextSpeed = newValue;
        public void ChangeCameraDistance(int newValue) => SettingsManager.Instance.CameraDistance = newValue;

        public void ChangeGraphics(int newValue) => SettingsManager.Instance.Graphics = newValue;
        public void ChangeFullscreen(int newValue) => SettingsManager.Instance.Fullscreen = newValue;
        public void ChangeVSync(int newValue) => SettingsManager.Instance.VSync = newValue;
        public void ChangeFramerate(int newValue) => SettingsManager.Instance.Framerate = newValue;
        public void ChangePostProcess(int newValue) => SettingsManager.Instance.PostProcess = newValue;

        public void ChangeMusicVolume(int newValue) => SettingsManager.Instance.MusicVolume = newValue;
        public void ChangeSfxVolume(int newValue) => SettingsManager.Instance.SfxVolume = newValue;
        public void ChangeMusic(int newValue) => SettingsManager.Instance.Music = newValue;
        public void ChangeSfx(int newValue) => SettingsManager.Instance.Sfx = newValue;
        #endregion

        #region Private Methods
        private void UpdateSettings()
        {
            _autosaveButton.UpdateSelectedButton(SettingsManager.Instance.Autosave);
            _textSpeedButton.UpdateSelectedButton(SettingsManager.Instance.TextSpeed);
            _cameraDistanceButton.UpdateSelectedButton(SettingsManager.Instance.CameraDistance);

            _graphicsButton.UpdateSelectedButton(SettingsManager.Instance.Graphics);
            _postProcessButton.UpdateSelectedButton(SettingsManager.Instance.PostProcess);
            _fullscreenButton.UpdateSelectedButton(SettingsManager.Instance.Fullscreen);
            _framerateButton.UpdateSelectedButton(SettingsManager.Instance.Framerate);
            _vSyncButton.UpdateSelectedButton(SettingsManager.Instance.VSync);

            _musicButton.UpdateSelectedButton(SettingsManager.Instance.Music);
            _sfxButton.UpdateSelectedButton(SettingsManager.Instance.Sfx);
            _musicSlider.CurrentValue = SettingsManager.Instance.MusicVolume;
            _sfxSlider.CurrentValue = SettingsManager.Instance.SfxVolume;
        }

        private void UpdateCurrentPanel(MainMenuPanelUI panel)
        {
            if (panel == null)
                _currentPanel = _panels[0];
            else
                _currentPanel = panel;

            // update panel visual
            for (int i = 0; i < _panels.Count; i++)
            {
                if (_panels[i].DisableButtonsIfActive)
                    _panels[i].ChangeButtonsState(_panels[i] != _currentPanel);

                _panels[i].GameObject.SetActive(_panels[i] == _currentPanel);
            }
        }
        #endregion
    }
}