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

        [SerializeField] private List<MainMenuPanelUI> panels = new();
        [SerializeField] private Button continueButton;

        [Header("Debug")]
        [SerializeField, ReadOnly] private MainMenuPanelUI currentPanel;

        #endregion

        #region Unity Messages

        private void Awake()
        {
            foreach (var panel in panels)
            {
                panel.SubscribeToEvents();
                panel.OnButtonClicked += UpdateCurrentPanel;
            }

            continueButton.gameObject.SetActive(false); // TODO: check for save
            UpdateCurrentPanel(null);
        }

        private void OnDestroy()
        {
            foreach (var panel in panels)
            {
                panel.UnsubscribeToEvents();
                panel.OnButtonClicked -= UpdateCurrentPanel;
            }
        }

        #endregion

        #region Private Methods

        private void UpdateCurrentPanel(MainMenuPanelUI panel)
        {
            currentPanel = panel;

            // update panel visual
            for (int i = 0; i < panels.Count; i++)
            {
                if (currentPanel == null)
                    panels[i].GameObject.SetActive(i == 0);
                else
                    panels[i].GameObject.SetActive(panels[i] == currentPanel);
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
