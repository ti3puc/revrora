using System;
using System.Collections;
using System.Collections.Generic;
using Managers.Scenes;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu
{
    public class MainMenuUI : MonoBehaviour
    {
        #region Fields

        [SerializeField] private List<MainMenuPanelUI> _panels = new();
        [SerializeField] private Button _continueButton;

        [Header("Debug")]
        [SerializeField, ReadOnly] private MainMenuPanelUI _currentPanel;

        #endregion

        #region Unity Messages

        private void Awake()
        {
            foreach (var panel in _panels)
            {
                panel.SubscribeToEvents();
                panel.OnButtonClicked += UpdateCurrentPanel;
            }

            _continueButton.gameObject.SetActive(false); // TODO: check for save
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

        #endregion

        #region Private Methods

        private void UpdateCurrentPanel(MainMenuPanelUI panel)
        {
            _currentPanel = panel;

            // update panel visual
            for (int i = 0; i < _panels.Count; i++)
            {
                if (_currentPanel == null)
                    _panels[i].GameObject.SetActive(i == 0);
                else
                    _panels[i].GameObject.SetActive(_panels[i] == _currentPanel);
            }
        }

        #endregion

        #region Public Methods

        #region Methods for UnityEvent
        // These functions are used inside Unity editor, on the Button
        // component. Thats why vscode maybe wont find the reference
        // where they're being used.

        public void ContinueGame()
        {
            // TODO: continue last save
        }

        public void StartNewGame()
        {
            ScenesManager.LoadFirstScene();
        }

        public void LoadGame()
        {
            Debug.Log("Trying to load game.");
        }

        public void QuitGame()
        {
            Application.Quit();
        }
        #endregion

        #endregion
    }
}
