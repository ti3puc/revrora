using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace UI.Menu
{
    public class MainMenuSettingsUI : MonoBehaviour
    {
        [SerializeField] private List<MainMenuPanelUI> _panels = new();

        [Header("Debug")]
        [SerializeField, ReadOnly] private MainMenuPanelUI _currentPanel;

        private void Awake()
        {
            foreach (var panel in _panels)
            {
                panel.SubscribeToEvents();
                panel.OnButtonClicked += UpdateCurrentPanel;
            }

            UpdateCurrentPanel(null);
        }

        private void OnDestroy()
        {
            foreach (var panel in _panels)
            {
                panel.UnsubscribeToEvents();
                panel.OnButtonClicked -= UpdateCurrentPanel;
            }
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
    }
}