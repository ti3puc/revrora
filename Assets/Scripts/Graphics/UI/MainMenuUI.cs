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
        [SerializeField] private List<MainMenuPanelUI> panels = new();

        [Header("Main Panel Buttons")]
        [SerializeField] private Button continueButton;
        [SerializeField] private Button newGameButton;
        [SerializeField] private Button loadGameButton;
        [SerializeField] private Button quitButton;

        [Header("Debug")]
        [SerializeField, ReadOnly] private MainMenuPanelUI currentPanel;

        private void Awake()
        {
            foreach (var panel in panels)
            {
                panel.SubscribeToEvents();
                panel.OnButtonClicked += UpdateCurrentPanel;
            }

            continueButton.onClick.AddListener(ContinueGame);
            newGameButton.onClick.AddListener(StartNewGame);
            loadGameButton.onClick.AddListener(LoadGame);
            quitButton.onClick.AddListener(QuitGame);

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

            continueButton.onClick.RemoveListener(ContinueGame);
            newGameButton.onClick.RemoveListener(StartNewGame);
            loadGameButton.onClick.RemoveListener(LoadGame);
            quitButton.onClick.RemoveListener(QuitGame);
        }

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
    }
}
