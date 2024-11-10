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

        [Header("Sliders")]
        [SerializeField] private SliderSettingsButton _musicSlider;
        [SerializeField] private SliderSettingsButton _sfxSlider;

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
            GetVolumesFromPrefs();
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

        #region Public Methods
        public void ChangeMusicVolume(int newValue)
        {
            SettingsManager.Instance.MusicVolume = newValue;
        }

        public void ChangeSfxVolume(int newValue)
        {
            SettingsManager.Instance.SfxVolume = newValue;
        }
        #endregion

        #region Private Methods
        private void GetVolumesFromPrefs()
        {
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